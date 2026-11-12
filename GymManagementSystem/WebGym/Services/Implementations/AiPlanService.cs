using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebGym.Data;
using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;
using WebGym.Services.Fitness;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    /// <summary>
    /// Tạo giáo án cá nhân hoá. Nguyên tắc: con số do server tính (FitnessCalculator), nội dung do AI soạn,
    /// kết quả do server kiểm tra lại. AI lỗi / hết giờ / sai định dạng thì dùng giáo án mẫu (PlanTemplates).
    /// </summary>
    public class AiPlanService : IAiPlanService
    {
        public const int DailyLimit = 5;
        private const string EquipmentOk = "Hoạt động tốt";
        private static readonly string[] DefaultFallbackModels = { "gemini-3.7-flash", "gemini-flash-latest", "gemini-flash-lite-latest" };
        private const string DefaultModel = "gemini-3.8-flash";

        // Một lần gọi tối đa 45 giây; cả lượt tạo (kể cả gọi lại khi sai định dạng) tối đa 60 giây
        private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(45);
        private static readonly TimeSpan TotalBudget = TimeSpan.FromSeconds(60);

        private static readonly ConcurrentDictionary<string, DateTime> SkipModelUntil = new(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<string, bool> NoThinkingConfig = new(StringComparer.OrdinalIgnoreCase);

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AiPlanService> _logger;

        public AiPlanService(ApplicationDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory,
            IMemoryCache cache, ILogger<AiPlanService> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _logger = logger;
        }

        // ================== API công khai ==================

        public async Task<PlanGenerationResult> GenerateAsync(int memberId, PlanSurveyDto survey, double? inBodyBmr, CancellationToken cancellationToken = default)
        {
            var today = FitnessCalculator.TodayVn();
            var targets = FitnessCalculator.ComputeTargets(survey, inBodyBmr, today);
            var split = FitnessCalculator.BuildSplit(survey);
            var allowed = await LoadAllowedExercisesAsync(survey, cancellationToken);

            var doc = new PlanDocument { Targets = targets, SplitName = split.Name };
            var result = await FillWithAiAsync(memberId, survey, targets, split, allowed, doc, "all", null, cancellationToken);
            return result;
        }

        public async Task<PlanGenerationResult> RegenerateAsync(int memberId, PlanSurveyDto survey, double? inBodyBmr, PlanDocument current,
            string part, int? weekday, string? reason, CancellationToken cancellationToken = default)
        {
            part = part is "meals" or "day" ? part : "all";
            if (part == "all") return await GenerateAsyncWithReason(memberId, survey, inBodyBmr, reason, cancellationToken);

            var today = FitnessCalculator.TodayVn();
            var targets = FitnessCalculator.ComputeTargets(survey, inBodyBmr, today);
            var split = FitnessCalculator.BuildSplit(survey);
            var allowed = await LoadAllowedExercisesAsync(survey, cancellationToken);

            var doc = Clone(current);
            doc.Targets = targets;
            doc.Notices = new List<string>();
            doc.GeneratedAt = DateTime.UtcNow;

            if (part == "day")
            {
                var target = split.Days.FirstOrDefault(d => d.Weekday == weekday);
                if (target == null) return Fail("Không tìm thấy buổi tập cần đổi.");
                split = new SplitPlan { Name = split.Name, Days = new List<SplitDay> { target } };
            }
            return await FillWithAiAsync(memberId, survey, targets, split, allowed, doc, part, reason, cancellationToken);
        }

        public int RemainingToday(int memberId) =>
            Math.Max(0, DailyLimit - _cache.Get<int>(UsageKey(memberId)));

        public async Task<Dictionary<int, Exercise>> GetExerciseMapAsync(CancellationToken cancellationToken = default) =>
            await _context.Exercises.AsNoTracking().Include(e => e.Equipment).ToDictionaryAsync(e => e.ExerciseId, cancellationToken);

        public string BuildRoutineText(PlanDocument doc, IReadOnlyDictionary<int, Exercise> exercises)
        {
            var sb = new StringBuilder();
            for (var wd = 1; wd <= 7; wd++)
            {
                var day = doc.Days.FirstOrDefault(d => d.Weekday == wd);
                if (day == null)
                {
                    sb.Append("- ").Append(PlanOptions.WeekdayLabel(wd)).Append(": Nghỉ phục hồi.").Append('\n');
                    continue;
                }
                var items = day.Exercises.Select(x =>
                    $"{(exercises.TryGetValue(x.ExerciseId, out var e) ? e.Name : "Bài #" + x.ExerciseId)} {x.Sets}x{x.Reps}");
                sb.Append("- ").Append(PlanOptions.WeekdayLabel(wd)).Append(" (").Append(day.Focus).Append("): ")
                  .Append(string.Join(", ", items));
                if (!string.IsNullOrWhiteSpace(day.Cardio)) sb.Append(". Cardio: ").Append(day.Cardio);
                sb.Append('.').Append('\n');
            }
            return sb.ToString().TrimEnd('\n');
        }

        public string BuildNutritionText(PlanDocument doc)
        {
            var t = doc.Targets;
            var sb = new StringBuilder();
            sb.Append($"- Calo mục tiêu: {FitnessCalculator.N(t.TargetKcal)} kcal/ngày.\n");
            sb.Append($"- Đạm {t.ProteinG}g, tinh bột {t.CarbG}g, chất béo {t.FatG}g mỗi ngày.\n");
            sb.Append($"- Nước: khoảng {FitnessCalculator.N(t.WaterMl)} ml/ngày, ngày tập {FitnessCalculator.N(t.WaterMlTrainingDay)} ml.\n");
            foreach (var m in doc.Meals)
                sb.Append($"- Bữa {m.Slot.ToLowerInvariant()}: {m.Dish} (~{m.Kcal} kcal, {m.ProteinG}g đạm).\n");
            return sb.ToString().TrimEnd('\n');
        }

        // ================== Xử lý chính ==================

        private async Task<PlanGenerationResult> GenerateAsyncWithReason(int memberId, PlanSurveyDto survey, double? inBodyBmr, string? reason, CancellationToken ct)
        {
            var today = FitnessCalculator.TodayVn();
            var targets = FitnessCalculator.ComputeTargets(survey, inBodyBmr, today);
            var split = FitnessCalculator.BuildSplit(survey);
            var allowed = await LoadAllowedExercisesAsync(survey, ct);
            var doc = new PlanDocument { Targets = targets, SplitName = split.Name };
            return await FillWithAiAsync(memberId, survey, targets, split, allowed, doc, "all", reason, ct);
        }

        /// <summary>
        /// Gọi AI cho phần cần tạo ("all" | "meals" | "day"), kiểm tra kết quả, gọi lại 1 lần khi sai,
        /// phần nào vẫn sai thì thay bằng giáo án mẫu. Ghi kết quả vào <paramref name="doc"/>.
        /// </summary>
        private async Task<PlanGenerationResult> FillWithAiAsync(int memberId, PlanSurveyDto survey, FitnessTargets targets, SplitPlan split,
            List<Exercise> allowed, PlanDocument doc, string part, string? reason, CancellationToken ct)
        {
            var wantDays = part is "all" or "day";
            var wantMeals = part is "all" or "meals";

            var apiKey = _configuration["AI:ApiKey"] ?? string.Empty;
            var hasKey = !string.IsNullOrWhiteSpace(apiKey) && apiKey != "YOUR_GEMINI_OR_OPENAI_API_KEY";

            if (hasKey && RemainingToday(memberId) <= 0)
                return Fail($"Bạn đã dùng hết {DailyLimit} lượt tạo giáo án bằng AI hôm nay. Hãy quay lại vào ngày mai.");

            List<PlanDay>? days = null;
            List<PlanMeal>? meals = null;
            List<string>? tips = null;
            var usedAi = false;
            var aiFailedNotice = (string?)null;

            if (hasKey)
            {
                using var budget = CancellationTokenSource.CreateLinkedTokenSource(ct);
                budget.CancelAfter(TotalBudget);
                string? feedback = null;

                for (var attempt = 1; attempt <= 2 && !budget.IsCancellationRequested; attempt++)
                {
                    IncrementUsage(memberId);
                    var prompt = BuildUserPrompt(survey, targets, split, allowed, part, reason, feedback);
                    var raw = await CallGeminiAsync(apiKey, prompt, budget.Token);
                    if (raw == null)
                    {
                        aiFailedNotice = "AI chưa phản hồi kịp nên hệ thống dùng giáo án mẫu. Bạn có thể bấm \"Tạo lại\" sau ít phút.";
                        break;
                    }

                    var parsed = Parse(raw);
                    if (parsed == null)
                    {
                        feedback = "Kết quả lần trước không phải JSON hợp lệ theo schema. Hãy trả về đúng JSON.";
                        continue;
                    }

                    usedAi = true;
                    var errors = new List<string>();
                    if (wantDays && days == null)
                    {
                        var dayErrors = ValidateDays(parsed.Days, split, allowed, out var fixedDays);
                        if (dayErrors.Count == 0) days = fixedDays; else errors.AddRange(dayErrors);
                    }
                    if (wantMeals && meals == null)
                    {
                        var mealErrors = ValidateMeals(parsed.Meals, targets, survey, out var fixedMeals);
                        if (mealErrors.Count == 0) meals = fixedMeals; else errors.AddRange(mealErrors);
                    }
                    if (part == "all" && tips == null)
                    {
                        tips = (parsed.Tips ?? new List<string>()).Where(t => !string.IsNullOrWhiteSpace(t))
                            .Select(t => Truncate(t.Trim(), 160)).Take(4).ToList();
                    }

                    if ((!wantDays || days != null) && (!wantMeals || meals != null)) break;
                    feedback = "Kết quả lần trước có lỗi, hãy sửa: " + string.Join(" | ", errors.Take(8));
                    _logger.LogInformation("Giáo án AI chưa hợp lệ (lần {Attempt}): {Errors}", attempt, string.Join(" | ", errors));
                }
            }
            else
            {
                aiFailedNotice = "Chưa cấu hình khoá AI nên hệ thống dùng giáo án mẫu theo công thức.";
            }

            // Phần nào AI chưa làm được thì dùng giáo án mẫu
            var usedTemplate = false;
            if (wantDays && days == null)
            {
                days = PlanTemplates.Days(split, survey, allowed);
                usedTemplate = true;
            }
            if (wantMeals && meals == null)
            {
                meals = PlanTemplates.Meals(targets, survey.Diet, survey.Allergies);
                usedTemplate = true;
            }
            if (part == "all" && (tips == null || tips.Count == 0)) tips = PlanTemplates.Tips(survey.Goal);

            // Ghép vào tài liệu
            if (part == "all")
            {
                doc.Days = days!;
                doc.Meals = meals!;
                doc.Tips = tips!;
            }
            else if (part == "meals")
            {
                doc.Meals = meals!;
            }
            else
            {
                var newDay = days!.First();
                var idx = doc.Days.FindIndex(d => d.Weekday == newDay.Weekday);
                if (idx >= 0) doc.Days[idx] = newDay; else doc.Days.Add(newDay);
                doc.Days = doc.Days.OrderBy(d => d.Weekday).ToList();
            }

            doc.Origin = usedTemplate ? (usedAi ? "mixed" : "template") : "ai";
            if (usedTemplate)
            {
                doc.Notices.Add(aiFailedNotice ?? (usedAi
                    ? "Một phần kết quả AI chưa đạt yêu cầu nên được thay bằng giáo án mẫu."
                    : "Hệ thống dùng giáo án mẫu theo công thức."));
            }

            return new PlanGenerationResult
            {
                Success = true,
                Document = doc,
                UsedAi = usedAi,
                RemainingToday = RemainingToday(memberId)
            };
        }

        /// <summary>
        /// Bài tập được phép: còn dùng, thiết bị "Hoạt động tốt" (hoặc không cần thiết bị),
        /// người mới chỉ bài cơ bản, bỏ bài ảnh hưởng vùng chấn thương.
        /// </summary>
        private async Task<List<Exercise>> LoadAllowedExercisesAsync(PlanSurveyDto survey, CancellationToken ct)
        {
            var all = await _context.Exercises.AsNoTracking().Include(e => e.Equipment)
                .Where(e => e.IsActive)
                .OrderBy(e => e.ExerciseId)
                .ToListAsync(ct);

            var injuries = survey.Injuries ?? new List<string>();
            return all.Where(e =>
                    (e.Equipment == null || e.Equipment.Status == EquipmentOk) &&
                    (survey.Experience == "experienced" || e.Level == ExerciseLevels.Basic) &&
                    !injuries.Any(i => (e.AvoidIfInjury ?? string.Empty).Split(',', StringSplitOptions.TrimEntries).Contains(i)))
                .ToList();
        }

        // ================== Kiểm tra kết quả AI ==================

        private static List<string> ValidateDays(List<PlanDay>? aiDays, SplitPlan split, List<Exercise> allowed, out List<PlanDay> fixedDays)
        {
            var errors = new List<string>();
            fixedDays = new List<PlanDay>();
            aiDays ??= new List<PlanDay>();

            foreach (var sd in split.Days)
            {
                var label = PlanOptions.WeekdayLabel(sd.Weekday);
                var day = aiDays.FirstOrDefault(d => d.Weekday == sd.Weekday);
                if (day == null)
                {
                    errors.Add($"Thiếu buổi {label} (weekday {sd.Weekday}).");
                    continue;
                }

                var allowedIds = AllowedIdsFor(sd, allowed);
                var list = new List<PlanExercise>();
                foreach (var x in day.Exercises ?? new List<PlanExercise>())
                {
                    if (!allowedIds.Contains(x.ExerciseId))
                    {
                        errors.Add($"{label}: exerciseId {x.ExerciseId} không nằm trong allowedExerciseIds của ngày này.");
                        continue;
                    }
                    if (list.Any(e => e.ExerciseId == x.ExerciseId))
                    {
                        errors.Add($"{label}: bài {x.ExerciseId} bị lặp.");
                        continue;
                    }
                    var reps = string.IsNullOrWhiteSpace(x.Reps) ? "10-12" : Truncate(x.Reps.Trim(), 20);
                    list.Add(new PlanExercise
                    {
                        ExerciseId = x.ExerciseId,
                        Sets = Math.Clamp(x.Sets, 2, 6),
                        Reps = reps,
                        RestSec = Math.Clamp(x.RestSec, 30, 180),
                        Note = string.IsNullOrWhiteSpace(x.Note) ? null : Truncate(x.Note.Trim(), 140)
                    });
                }

                if (list.Count < sd.ExerciseCount - 1 || list.Count > sd.ExerciseCount + 1)
                    errors.Add($"{label}: cần {sd.ExerciseCount} bài, đang có {list.Count} bài hợp lệ.");

                fixedDays.Add(new PlanDay
                {
                    Weekday = sd.Weekday,
                    Focus = PlanOptions.Label(PlanOptions.Focuses, sd.Focus),
                    Exercises = list,
                    Cardio = sd.Cardio
                        ? (string.IsNullOrWhiteSpace(day.Cardio) ? PlanTemplates.CardioText(sd, allowed) : Truncate(day.Cardio.Trim(), 160))
                        : null
                });
            }
            return errors;
        }

        private static List<string> ValidateMeals(List<PlanMeal>? aiMeals, FitnessTargets t, PlanSurveyDto survey, out List<PlanMeal> fixedMeals)
        {
            var errors = new List<string>();
            fixedMeals = (aiMeals ?? new List<PlanMeal>())
                .Where(m => !string.IsNullOrWhiteSpace(m.Dish) && m.Kcal > 0)
                .Select(m => new PlanMeal
                {
                    Slot = string.IsNullOrWhiteSpace(m.Slot) ? "Bữa phụ" : Truncate(m.Slot.Trim(), 40),
                    Dish = Truncate(m.Dish.Trim(), 160),
                    Kcal = Math.Clamp(m.Kcal, 50, 2000),
                    ProteinG = Math.Clamp(m.ProteinG, 0, 150)
                })
                .ToList();

            if (fixedMeals.Count < 3 || fixedMeals.Count > 5)
                errors.Add($"Thực đơn cần 4 bữa, đang có {fixedMeals.Count}.");

            var total = fixedMeals.Sum(m => m.Kcal);
            if (t.TargetKcal > 0 && Math.Abs(total - t.TargetKcal) > t.TargetKcal * 0.10)
                errors.Add($"Tổng calo thực đơn {total} kcal lệch quá 10% so với mục tiêu {t.TargetKcal} kcal.");

            foreach (var m in fixedMeals)
            {
                var bad = PlanTemplates.FindViolation(m.Dish, survey.Diet, survey.Allergies);
                if (bad != null) errors.Add($"Món \"{m.Dish}\" chứa \"{bad}\", không hợp chế độ ăn / dị ứng.");
            }
            return errors;
        }

        private static HashSet<int> AllowedIdsFor(SplitDay d, List<Exercise> allowed) =>
            allowed.Where(e => d.CategoryQuota.ContainsKey(e.Category)).Select(e => e.ExerciseId).ToHashSet();

        // ================== Prompt & schema ==================

        private static string BuildSystemPrompt() => string.Join("\n", new[]
        {
            "Bạn là huấn luyện viên thể hình và chuyên gia dinh dưỡng của phòng tập Pulse Gym (Việt Nam).",
            "Nhiệm vụ: điền bài tập vào khung lịch có sẵn và soạn thực đơn món ăn Việt Nam cho một hội viên.",
            "QUY TẮC BẮT BUỘC:",
            "- KHÔNG tự tính lại calo, đạm, chất béo, tinh bột. Dùng đúng các con số trong mục targets.",
            "- Chỉ chọn exerciseId nằm trong allowedExerciseIds của đúng ngày đó. Không bịa bài, không dùng id khác.",
            "- Mỗi ngày chọn đúng exerciseCount bài, phân bổ theo categoryQuota (số bài cho mỗi nhóm Push/Pull/Legs/Core). Không lặp bài trong cùng một ngày. Hai buổi cùng kiểu (ví dụ Toàn thân A lặp lại) nên khác bài nếu đủ bài.",
            "- Sắp xếp bài nặng nhiều khớp trước, bài cô lập và bụng sau.",
            "- sets, reps, restSec theo mục prescription. Bài Plank ghi reps theo giây, ví dụ \"30-45 giây\".",
            "- note: tối đa 15 từ tiếng Việt, mẹo kỹ thuật hoặc an toàn cho bài đó.",
            "- cardio: nếu ngày có cardio=true thì viết 1 câu ngắn gồm số phút cardioMinutes và tên một máy trong cardioMachines; nếu không thì để chuỗi rỗng.",
            "- meals: đúng 4 bữa theo thứ tự slot \"Sáng\", \"Trưa\", \"Xế (trước tập 1 tiếng)\", \"Tối\". Món ăn Việt Nam gần gũi, dễ mua, ghi khẩu phần ước lượng. Tổng kcal 4 bữa bằng targets.targetKcal (sai lệch tối đa 5%), tổng proteinG gần bằng targets.proteinG.",
            "- Tuân thủ tuyệt đối mục foodRestrictions (chế độ ăn và dị ứng).",
            "- tips: 3 lời khuyên ngắn (dưới 25 từ mỗi câu), phù hợp mục tiêu.",
            "- Chỉ trả về JSON đúng schema, không thêm chữ nào khác."
        });

        private static string BuildUserPrompt(PlanSurveyDto s, FitnessTargets t, SplitPlan split, List<Exercise> allowed,
            string part, string? reason, string? feedback)
        {
            var (sets, reps, rest) = FitnessCalculator.Prescription(s.Goal, s.Experience, ExerciseCategories.Push);
            var payload = new
            {
                task = part switch
                {
                    "meals" => "Chỉ soạn lại meals (thực đơn). Trả days là mảng rỗng.",
                    "day" => "Chỉ soạn lại buổi tập trong days (đúng 1 ngày). Trả meals là mảng rỗng.",
                    _ => "Soạn đầy đủ days, meals và tips."
                },
                member = new
                {
                    goal = PlanOptions.Label(PlanOptions.Goals, s.Goal),
                    experience = PlanOptions.Label(PlanOptions.Experiences, s.Experience),
                    gender = s.Gender,
                    age = t.Age,
                    sessionMinutes = s.SessionMinutes,
                    injuries = (s.Injuries ?? new List<string>()).Select(i => PlanOptions.Label(PlanOptions.Injuries, i)).ToList()
                },
                targets = new { targetKcal = t.TargetKcal, proteinG = t.ProteinG, carbG = t.CarbG, fatG = t.FatG },
                prescription = new
                {
                    mainLifts = $"{sets} hiệp x {reps} lần, nghỉ {rest} giây",
                    core = "3 hiệp x 12-15 lần (Plank 30-45 giây), nghỉ 45 giây"
                },
                foodRestrictions = PlanTemplates.ForbiddenSummary(s.Diet, s.Allergies),
                diet = PlanOptions.Label(PlanOptions.Diets, s.Diet),
                days = part == "meals" ? new List<object>() : split.Days.Select(d => (object)new
                {
                    weekday = d.Weekday,
                    weekdayLabel = PlanOptions.WeekdayLabel(d.Weekday),
                    focus = PlanOptions.Label(PlanOptions.Focuses, d.Focus),
                    exerciseCount = d.ExerciseCount,
                    categoryQuota = d.CategoryQuota,
                    cardio = d.Cardio,
                    cardioMinutes = d.CardioMinutes,
                    allowedExerciseIds = AllowedIdsFor(d, allowed).OrderBy(i => i).ToList()
                }).ToList(),
                exerciseCatalog = part == "meals" ? new List<object>() : allowed
                    .Where(e => e.Category != ExerciseCategories.Cardio)
                    .Select(e => (object)new { id = e.ExerciseId, name = e.Name, muscle = e.MuscleGroup, category = e.Category, level = e.Level })
                    .ToList(),
                cardioMachines = allowed.Where(e => e.Category == ExerciseCategories.Cardio).Select(e => e.Name).ToList(),
                memberRequest = string.IsNullOrWhiteSpace(reason) ? null : Truncate(reason, 200),
                fixPreviousErrors = feedback
            };
            return JsonSerializer.Serialize(payload, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }

        private static object ResponseSchema() => new
        {
            type = "OBJECT",
            properties = new
            {
                days = new
                {
                    type = "ARRAY",
                    items = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            weekday = new { type = "INTEGER" },
                            focus = new { type = "STRING" },
                            exercises = new
                            {
                                type = "ARRAY",
                                items = new
                                {
                                    type = "OBJECT",
                                    properties = new
                                    {
                                        exerciseId = new { type = "INTEGER" },
                                        sets = new { type = "INTEGER" },
                                        reps = new { type = "STRING" },
                                        restSec = new { type = "INTEGER" },
                                        note = new { type = "STRING" }
                                    },
                                    required = new[] { "exerciseId", "sets", "reps", "restSec" }
                                }
                            },
                            cardio = new { type = "STRING" }
                        },
                        required = new[] { "weekday", "exercises" }
                    }
                },
                meals = new
                {
                    type = "ARRAY",
                    items = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            slot = new { type = "STRING" },
                            dish = new { type = "STRING" },
                            kcal = new { type = "INTEGER" },
                            proteinG = new { type = "INTEGER" }
                        },
                        required = new[] { "slot", "dish", "kcal", "proteinG" }
                    }
                },
                tips = new { type = "ARRAY", items = new { type = "STRING" } }
            },
            required = new[] { "days", "meals", "tips" }
        };

        private static AiPlanResponse? Parse(string raw)
        {
            var text = raw.Trim();
            if (text.StartsWith("```"))
            {
                var firstNewLine = text.IndexOf('\n');
                if (firstNewLine > 0) text = text[(firstNewLine + 1)..];
                if (text.EndsWith("```")) text = text[..^3];
            }
            try
            {
                return JsonSerializer.Deserialize<AiPlanResponse>(text, JsonOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        // ================== Gọi Gemini ==================

        /// <summary>Gọi lần lượt model chính rồi các model dự phòng. Trả về văn bản JSON, hoặc null khi lỗi / hết giờ.</summary>
        private async Task<string?> CallGeminiAsync(string apiKey, string userPrompt, CancellationToken budgetToken)
        {
            var model = _configuration["AI:PlanModel"];
            if (string.IsNullOrWhiteSpace(model)) model = _configuration["AI:Model"];
            if (string.IsNullOrWhiteSpace(model)) model = DefaultModel;

            var models = new List<string> { model };
            var fbConfig = _configuration["AI:FallbackModels"];
            var fallbacks = string.IsNullOrWhiteSpace(fbConfig)
                ? DefaultFallbackModels
                : fbConfig.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var f in fallbacks)
                if (!models.Contains(f, StringComparer.OrdinalIgnoreCase)) models.Add(f);

            var now = DateTime.UtcNow;
            var ready = models.Where(m => !SkipModelUntil.TryGetValue(m, out var until) || until <= now).ToList();
            if (ready.Count == 0) ready = models;

            var thinkingLevel = (_configuration["AI:ThinkingLevel"] ?? "low").Trim().ToLowerInvariant();
            var sendThinking = thinkingLevel is "minimal" or "low" or "medium" or "high";
            var systemPrompt = BuildSystemPrompt();
            var schema = ResponseSchema();

            string BuildJson(bool withThinking)
            {
                object generationConfig = withThinking
                    ? new { temperature = 0.7, maxOutputTokens = 8192, responseMimeType = "application/json", responseSchema = schema, thinkingConfig = new { thinkingLevel } }
                    : new { temperature = 0.7, maxOutputTokens = 8192, responseMimeType = "application/json", responseSchema = schema };
                return JsonSerializer.Serialize(new
                {
                    systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                    contents = new[] { new { role = "user", parts = new[] { new { text = userPrompt } } } },
                    generationConfig
                });
            }

            var client = _httpClientFactory.CreateClient();
            client.Timeout = Timeout.InfiniteTimeSpan;

            foreach (var m in ready)
            {
                if (budgetToken.IsCancellationRequested) return null;
                var withThinking = sendThinking && !NoThinkingConfig.ContainsKey(m);
                var (status, text) = await SendOnceAsync(client, apiKey, m, BuildJson(withThinking), budgetToken);

                if (status == 400 && withThinking && text.Contains("thinking", StringComparison.OrdinalIgnoreCase))
                {
                    NoThinkingConfig[m] = true;
                    (status, text) = await SendOnceAsync(client, apiKey, m, BuildJson(false), budgetToken);
                }

                if (status is >= 200 and < 300)
                {
                    SkipModelUntil.TryRemove(m, out _);
                    return ExtractText(text);
                }

                if (status == 0)
                {
                    _logger.LogWarning("Giáo án AI: model {Model} quá thời gian chờ", m);
                    SkipModelUntil[m] = DateTime.UtcNow.AddMinutes(2);
                    continue;
                }

                _logger.LogWarning("Giáo án AI: model {Model} trả về {Status}: {Error}", m, status, Truncate(text, 500));
                if (status == 404) { SkipModelUntil[m] = DateTime.UtcNow.AddDays(1); continue; }
                if (status == 503 || status == 429) { SkipModelUntil[m] = DateTime.UtcNow.AddMinutes(2); continue; }
                return null;
            }
            return null;
        }

        private static string? ExtractText(string body)
        {
            try
            {
                using var doc = JsonDocument.Parse(body);
                if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0) return null;
                if (!candidates[0].TryGetProperty("content", out var content) || !content.TryGetProperty("parts", out var parts)) return null;
                var sb = new StringBuilder();
                foreach (var part in parts.EnumerateArray())
                {
                    // Bỏ qua phần "suy nghĩ" nếu model trả về
                    if (part.TryGetProperty("thought", out var thought) && thought.ValueKind == JsonValueKind.True) continue;
                    if (part.TryGetProperty("text", out var t)) sb.Append(t.GetString());
                }
                var text = sb.ToString();
                return string.IsNullOrWhiteSpace(text) ? null : text;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static async Task<(int Status, string Text)> SendOnceAsync(HttpClient client, string apiKey, string model, string json, CancellationToken budgetToken)
        {
            using var attempt = CancellationTokenSource.CreateLinkedTokenSource(budgetToken);
            attempt.CancelAfter(AttemptTimeout);

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-goog-api-key", apiKey);

            try
            {
                using var response = await client.SendAsync(request, attempt.Token);
                var text = await response.Content.ReadAsStringAsync(attempt.Token);
                return ((int)response.StatusCode, text);
            }
            catch (OperationCanceledException)
            {
                return (0, string.Empty);
            }
            catch (HttpRequestException)
            {
                return (503, string.Empty);
            }
        }

        // ================== Tiện ích ==================

        private static string UsageKey(int memberId) => $"aiplan:usage:{memberId}:{FitnessCalculator.TodayVn():yyyyMMdd}";

        private void IncrementUsage(int memberId)
        {
            var key = UsageKey(memberId);
            var count = _cache.Get<int>(key) + 1;
            _cache.Set(key, count, TimeSpan.FromHours(26));
        }

        private static PlanDocument Clone(PlanDocument doc) =>
            JsonSerializer.Deserialize<PlanDocument>(JsonSerializer.Serialize(doc, JsonOptions), JsonOptions) ?? new PlanDocument();

        private static PlanGenerationResult Fail(string message) => new() { Success = false, Error = message };

        private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max];
    }
}
