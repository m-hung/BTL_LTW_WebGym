using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebGym.Data;
using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;
using WebGym.Services.Fitness;
using WebGym.Services.Interfaces;
using MemberEntity = WebGym.Models.Entities.Member;

namespace WebGym.Areas.Member.Controllers
{
    /// <summary>
    /// Giáo án cá nhân (AI gợi ý lộ trình tập):
    /// Create (bước 1) → Generate/Regenerate (bước 2–3, AJAX trả về HTML) → Save (bước 4) → Index / Today (dùng hằng ngày).
    /// Bản nháp giữ ở server (IMemoryCache 2 giờ) nên người dùng không sửa được số liệu trước khi lưu.
    /// </summary>
    public class PlanController : MemberBaseController
    {
        private static readonly TimeSpan DraftLifetime = TimeSpan.FromHours(2);
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };

        private readonly ApplicationDbContext _context;
        private readonly IAiPlanService _planService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PlanController> _logger;

        public PlanController(ApplicationDbContext context, IAiPlanService planService, IMemoryCache cache, ILogger<PlanController> logger)
        {
            _context = context;
            _planService = planService;
            _cache = cache;
            _logger = logger;
        }

        private sealed class PlanDraft
        {
            public int MemberId { get; set; }
            public PlanSurveyDto Survey { get; set; } = new();
            public double? InBodyBmr { get; set; }
            public PlanDocument Doc { get; set; } = new();
        }

        // ================== GET /Member/Plan ==================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var member = await CurrentMemberAsync();
            if (member == null) return NoMemberProfile();

            var plans = await _context.WorkoutPlans.AsNoTracking()
                .Include(p => p.Trainer).ThenInclude(t => t!.User)
                .Where(p => p.MemberId == member.MemberId && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var active = plans.FirstOrDefault(p => p.IsActive && p.Status != WorkoutPlanStatuses.Archived);
            ViewBag.History = plans.Where(p => active == null || p.Id != active.Id).ToList();
            ViewBag.Remaining = _planService.RemainingToday(member.MemberId);

            if (active != null && TryReadDoc(active.PlanJson, out var doc))
            {
                ViewBag.ActiveView = new PlanViewModel
                {
                    Doc = doc,
                    Exercises = await _planService.GetExerciseMapAsync(),
                    PlanId = active.Id,
                    Status = active.Status,
                    Source = active.Source,
                    TrainerName = active.Trainer?.User?.FullName,
                    CreatedAt = active.CreatedAt
                };
            }
            return View(active);
        }

        // ================== GET /Member/Plan/Create ==================

        [HttpGet]
        public async Task<IActionResult> Create(int? from)
        {
            var member = await CurrentMemberAsync(includeProgress: true);
            if (member == null) return NoMemberProfile();

            var vm = new PlanCreateViewModel
            {
                RemainingToday = _planService.RemainingToday(member.MemberId)
            };

            var sub = ActiveSubscription(member);
            vm.HasActiveSubscription = sub != null;
            vm.PackageIncludesPt = sub?.Package?.IncludesPT == true;

            var activePlan = await _context.WorkoutPlans.AsNoTracking()
                .Where(p => p.MemberId == member.MemberId && p.IsActive && p.Status != WorkoutPlanStatuses.Archived && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt).FirstOrDefaultAsync();
            vm.HasActivePlan = activePlan != null;
            vm.ActivePlanFromTrainer = activePlan != null && activePlan.Source == WorkoutPlanSources.Trainer;

            // Điền sẵn từ hồ sơ + số đo InBody mới nhất
            var (survey, measurement) = Prefill(member);
            vm.Survey = survey;
            vm.Measurement = measurement;

            // "Tạo lại" từ một giáo án cũ: dùng lại câu trả lời khảo sát lần đó
            if (from.HasValue)
            {
                var old = await _context.WorkoutPlans.AsNoTracking().FirstOrDefaultAsync(p => p.Id == from && p.MemberId == member.MemberId);
                if (old?.InputJson != null)
                {
                    try
                    {
                        var prev = JsonSerializer.Deserialize<PlanSurveyDto>(old.InputJson, JsonOptions);
                        if (prev != null)
                        {
                            // Giữ số đo mới nhất, chỉ lấy lại phần khảo sát
                            prev.HeightCm = survey.HeightCm ?? prev.HeightCm;
                            prev.WeightKg = survey.WeightKg ?? prev.WeightKg;
                            prev.BodyFatPercent = survey.BodyFatPercent ?? prev.BodyFatPercent;
                            prev.MuscleMassKg = survey.MuscleMassKg ?? prev.MuscleMassKg;
                            prev.DateOfBirth = survey.DateOfBirth ?? prev.DateOfBirth;
                            prev.Gender = survey.Gender ?? prev.Gender;
                            prev.Consent = false;
                            vm.Survey = prev;
                        }
                    }
                    catch (JsonException) { /* bỏ qua, dùng giá trị mặc định */ }
                }
            }

            return View(vm);
        }

        // ================== POST /Member/Plan/Generate (AJAX) ==================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(PlanSurveyDto survey, CancellationToken cancellationToken)
        {
            var member = await CurrentMemberAsync(includeProgress: true);
            if (member == null) return JsonError("Tài khoản này chưa có hồ sơ hội viên.", 403);
            if (ActiveSubscription(member) == null) return JsonError("Tính năng này dành cho hội viên có gói tập còn hạn.", 403);

            // Số thập phân: đọc lại theo dấu chấm (InvariantCulture) để không phụ thuộc ngôn ngữ của máy chủ
            // (máy Windows tiếng Việt hiểu "65.5" thành 655). Sau đó kiểm tra lại toàn bộ model.
            survey.HeightCm = ReadNumber("HeightCm");
            survey.WeightKg = ReadNumber("WeightKg");
            survey.BodyFatPercent = ReadNumber("BodyFatPercent");
            survey.MuscleMassKg = ReadNumber("MuscleMassKg");
            ModelState.Clear();
            NormalizeSurvey(survey);
            TryValidateModel(survey);
            ValidateSurvey(survey);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(e => e.Key, e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray());
                var first = errors.Values.SelectMany(v => v).FirstOrDefault() ?? "Vui lòng kiểm tra lại thông tin.";
                return BadRequest(new { success = false, message = first, errors });
            }

            // Bệnh tim mạch / mang thai: không tạo giáo án tự động
            if (survey.Condition is "cardio" or "pregnant")
            {
                return JsonError("Với tình trạng sức khoẻ bạn đã chọn, giáo án cần được HLV và bác sĩ xây dựng trực tiếp. " +
                                 "Hãy đặt lịch với huấn luyện viên để được tư vấn an toàn.", 422, blocked: true);
            }

            var inBodyBmr = InBodyBmrFor(member, survey);
            var result = await _planService.GenerateAsync(member.MemberId, survey, inBodyBmr, cancellationToken);
            if (!result.Success || result.Document == null) return JsonError(result.Error ?? "Không tạo được giáo án, vui lòng thử lại.", 429);

            var draftId = Guid.NewGuid().ToString("N");
            _cache.Set(DraftKey(draftId), new PlanDraft { MemberId = member.MemberId, Survey = survey, InBodyBmr = inBodyBmr, Doc = result.Document }, DraftLifetime);

            return PartialView("_PlanView", await DraftView(result.Document, draftId, result.RemainingToday));
        }

        // ================== POST /Member/Plan/Regenerate (AJAX) ==================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regenerate(string draftId, string part, int? weekday, string? reason, CancellationToken cancellationToken)
        {
            var member = await CurrentMemberAsync();
            if (member == null) return JsonError("Tài khoản này chưa có hồ sơ hội viên.", 403);
            if (!_cache.TryGetValue(DraftKey(draftId), out PlanDraft? draft) || draft == null || draft.MemberId != member.MemberId)
                return JsonError("Bản nháp đã hết hạn. Vui lòng bấm \"Bắt đầu phân tích\" lại.", 410);

            var result = await _planService.RegenerateAsync(member.MemberId, draft.Survey, draft.InBodyBmr, draft.Doc, part, weekday, reason, cancellationToken);
            if (!result.Success || result.Document == null) return JsonError(result.Error ?? "Không tạo lại được, vui lòng thử lại.", 429);

            draft.Doc = result.Document;
            _cache.Set(DraftKey(draftId), draft, DraftLifetime);
            return PartialView("_PlanView", await DraftView(result.Document, draftId, result.RemainingToday));
        }

        // ================== POST /Member/Plan/Save (AJAX) ==================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(string draftId, bool replaceTrainerPlan = false)
        {
            var member = await CurrentMemberAsync(includeProgress: true);
            if (member == null) return JsonError("Tài khoản này chưa có hồ sơ hội viên.", 403);
            if (!_cache.TryGetValue(DraftKey(draftId), out PlanDraft? draft) || draft == null || draft.MemberId != member.MemberId)
                return JsonError("Bản nháp đã hết hạn. Vui lòng tạo lại giáo án.", 410);

            var current = await _context.WorkoutPlans
                .Where(p => p.MemberId == member.MemberId && p.IsActive && p.Status != WorkoutPlanStatuses.Archived && !p.IsDeleted)
                .ToListAsync();
            if (!replaceTrainerPlan && current.Any(p => p.Source == WorkoutPlanSources.Trainer))
            {
                return Conflict(new
                {
                    success = false,
                    needConfirm = true,
                    message = "Bạn đang có giáo án do HLV soạn. Lưu giáo án mới sẽ chuyển giáo án của HLV vào lịch sử. Bạn có chắc không?"
                });
            }

            var sub = ActiveSubscription(member);
            var needsReview = sub?.Package?.IncludesPT == true;
            var trainerId = needsReview ? await LatestTrainerIdAsync(member.MemberId) : null;
            if (trainerId == null) needsReview = false; // chưa tập với HLV nào thì không có người duyệt

            var doc = draft.Doc;
            var exercises = await _planService.GetExerciseMapAsync();
            var today = FitnessCalculator.TodayVn();

            foreach (var p in current)
            {
                p.IsActive = false;
                p.Status = WorkoutPlanStatuses.Archived;
                p.UpdatedAt = DateTime.UtcNow;
            }

            var plan = new WorkoutPlan
            {
                Title = $"Giáo án AI – {PlanOptions.Label(PlanOptions.Goals, draft.Survey.Goal)} {draft.Survey.SessionsPerWeek} buổi/tuần",
                MemberId = member.MemberId,
                TrainerId = null,
                Goal = PlanOptions.Label(PlanOptions.Goals, draft.Survey.Goal),
                StartDate = today,
                EndDate = today.AddDays(28),
                RoutineDetails = _planService.BuildRoutineText(doc, exercises),
                NutritionAdvice = _planService.BuildNutritionText(doc),
                IsActive = true,
                Source = WorkoutPlanSources.Ai,
                Status = needsReview ? WorkoutPlanStatuses.PendingReview : WorkoutPlanStatuses.Active,
                PlanJson = JsonSerializer.Serialize(doc, JsonOptions),
                InputJson = JsonSerializer.Serialize(draft.Survey, JsonOptions),
                TargetKcal = doc.Targets.TargetKcal,
                ProteinG = doc.Targets.ProteinG,
                CarbG = doc.Targets.CarbG,
                FatG = doc.Targets.FatG,
                WaterMl = doc.Targets.WaterMl,
                CreatedAt = DateTime.UtcNow
            };
            _context.WorkoutPlans.Add(plan);

            // Bổ sung hồ sơ nếu trước đó còn thiếu hoặc đang là số mặc định lúc đăng ký
            if (string.IsNullOrWhiteSpace(member.Gender)) member.Gender = draft.Survey.Gender;
            if (!member.DateOfBirth.HasValue) member.DateOfBirth = draft.Survey.DateOfBirth;
            if (!member.Height.HasValue || IsDefaultProfile(member)) member.Height = draft.Survey.HeightCm;

            if (needsReview && trainerId != null)
            {
                var trainerUserId = await _context.Trainers.Where(t => t.TrainerId == trainerId).Select(t => t.UserId).FirstOrDefaultAsync();
                if (trainerUserId != 0)
                {
                    _context.Notifications.Add(new Notification
                    {
                        UserId = trainerUserId,
                        Title = "Giáo án AI chờ bạn xem",
                        Message = $"Học viên {member.User.FullName} vừa tạo giáo án bằng AI ({plan.Goal}, {draft.Survey.SessionsPerWeek} buổi/tuần). Hãy xem và duyệt giúp học viên.",
                        Type = "System",
                        TargetUrl = "/PT/PlanReview",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
            _cache.Remove(DraftKey(draftId));
            _logger.LogInformation("Hội viên {MemberId} lưu giáo án AI {PlanId} (nguồn {Origin})", member.MemberId, plan.Id, doc.Origin);

            SetSuccessMessage(needsReview
                ? "Đã lưu giáo án. Bạn có thể dùng ngay, HLV sẽ xem lại và có thể điều chỉnh bài tập cho bạn."
                : "Đã lưu giáo án. Mỗi ngày vào mục \"Hôm nay\" để xem bài tập và tích khi tập xong.");
            return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
        }

        // ================== GET /Member/Plan/Today ==================

        [HttpGet]
        public async Task<IActionResult> Today()
        {
            var member = await CurrentMemberAsync();
            if (member == null) return NoMemberProfile();

            var date = FitnessCalculator.TodayVn();
            var vm = new TodayViewModel { Date = date, Weekday = FitnessCalculator.ToWeekday(date) };

            vm.Plan = await _context.WorkoutPlans.AsNoTracking()
                .Where(p => p.MemberId == member.MemberId && p.IsActive && p.Status != WorkoutPlanStatuses.Archived && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt).FirstOrDefaultAsync();

            if (vm.Plan != null && TryReadDoc(vm.Plan.PlanJson, out var doc))
            {
                vm.Doc = doc;
                vm.Exercises = await _planService.GetExerciseMapAsync();
                vm.TodayDay = doc.Days.FirstOrDefault(d => d.Weekday == vm.Weekday);
                vm.NextDay = doc.Days.OrderBy(d => (d.Weekday - vm.Weekday + 7) % 7).FirstOrDefault(d => d.Weekday != vm.Weekday);

                var weekStart = date.AddDays(1 - vm.Weekday);
                var weekLogs = await _context.WorkoutLogs.AsNoTracking()
                    .Where(l => l.PlanId == vm.Plan.Id && l.Date >= weekStart && l.Date <= date)
                    .Select(l => new { l.Date, l.Weekday, l.ExerciseId })
                    .ToListAsync();

                vm.DoneExerciseIds = weekLogs.Where(l => l.Date == date).Select(l => l.ExerciseId).ToHashSet();
                vm.SessionsPlannedThisWeek = doc.Days.Count;
                vm.SessionsDoneThisWeek = weekLogs.GroupBy(l => l.Date).Count(g =>
                {
                    var day = doc.Days.FirstOrDefault(d => d.Weekday == g.First().Weekday);
                    return day != null && day.Exercises.Count > 0 && day.Exercises.All(x => g.Any(l => l.ExerciseId == x.ExerciseId));
                });
            }

            // Dữ liệu check-in mẫu lưu theo giờ Việt Nam nên so theo ngày địa phương
            var nextDate = date.AddDays(1);
            vm.CheckInTime = await _context.CheckIns.AsNoTracking()
                .Where(c => c.MemberId == member.MemberId && c.CheckInTime >= date && c.CheckInTime < nextDate)
                .OrderBy(c => c.CheckInTime).Select(c => (DateTime?)c.CheckInTime).FirstOrDefaultAsync();

            return View(vm);
        }

        // ================== POST /Member/Plan/ToggleLog (AJAX) ==================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLog(int exerciseId, bool done)
        {
            var member = await CurrentMemberAsync();
            if (member == null) return JsonError("Tài khoản này chưa có hồ sơ hội viên.", 403);

            var plan = await _context.WorkoutPlans.AsNoTracking()
                .Where(p => p.MemberId == member.MemberId && p.IsActive && p.Status != WorkoutPlanStatuses.Archived && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt).FirstOrDefaultAsync();
            if (plan == null || !TryReadDoc(plan.PlanJson, out var doc)) return JsonError("Bạn chưa có giáo án đang dùng.", 404);

            var date = FitnessCalculator.TodayVn();
            var weekday = FitnessCalculator.ToWeekday(date);
            var day = doc.Days.FirstOrDefault(d => d.Weekday == weekday);
            if (day == null || day.Exercises.All(x => x.ExerciseId != exerciseId))
                return JsonError("Bài này không có trong buổi tập hôm nay.", 400);

            var log = await _context.WorkoutLogs.FirstOrDefaultAsync(l => l.PlanId == plan.Id && l.Date == date && l.ExerciseId == exerciseId);
            if (done && log == null)
            {
                _context.WorkoutLogs.Add(new WorkoutLog
                {
                    PlanId = plan.Id,
                    MemberId = member.MemberId,
                    Date = date,
                    Weekday = weekday,
                    ExerciseId = exerciseId,
                    CompletedAt = DateTime.UtcNow
                });
            }
            else if (!done && log != null)
            {
                _context.WorkoutLogs.Remove(log);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Bấm hai lần liên tiếp: bản ghi đã tồn tại, coi như đã lưu
            }

            var doneCount = await _context.WorkoutLogs.CountAsync(l => l.PlanId == plan.Id && l.Date == date);
            return Json(new { success = true, done, doneCount, total = day.Exercises.Count });
        }

        // ================== Hàm hỗ trợ ==================

        private async Task<MemberEntity?> CurrentMemberAsync(bool includeProgress = false)
        {
            var userId = CurrentUserId;
            if (userId == null) return null;
            IQueryable<MemberEntity> q = _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions).ThenInclude(s => s.Package);
            if (includeProgress) q = q.Include(m => m.ProgressRecords);
            return await q.FirstOrDefaultAsync(m => m.UserId == userId);
        }

        private static Subscription? ActiveSubscription(MemberEntity member)
        {
            var today = FitnessCalculator.TodayVn();
            return member.Subscriptions
                .Where(s => s.Status == "Active" && s.EndDate.Date >= today)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();
        }

        private static bool IsDefaultProfile(MemberEntity m) =>
            m.Height == 170 && m.Weight == 65 && m.BMR == 1500 && m.ProgressRecords.Count == 0;

        private static (PlanSurveyDto Survey, MeasurementSource Source) Prefill(MemberEntity m)
        {
            var survey = new PlanSurveyDto
            {
                Gender = m.Gender,
                DateOfBirth = m.DateOfBirth,
                Weekdays = new List<int> { 1, 3, 5 }
            };
            var src = new MeasurementSource();

            var latest = m.ProgressRecords.OrderByDescending(p => p.RecordedDate).FirstOrDefault();
            if (IsDefaultProfile(m))
            {
                src.IsDefault = true;
                src.Label = "Hồ sơ đang dùng số đo mặc định lúc đăng ký, vui lòng nhập số đo thật";
                return (survey, src);
            }

            survey.HeightCm = m.Height;
            if (latest != null)
            {
                survey.WeightKg = latest.Weight;
                survey.BodyFatPercent = latest.BodyFatPercentage ?? m.BodyFatPercentage;
                survey.MuscleMassKg = latest.MuscleMass ?? m.MuscleMass;
                var recorded = latest.RecordedDate.AddHours(7);
                src.Label = $"Số đo InBody ngày {recorded:dd/MM/yyyy}";
                src.IsStale = (FitnessCalculator.TodayVn() - recorded.Date).TotalDays > 60;
            }
            else
            {
                survey.WeightKg = m.Weight;
                survey.BodyFatPercent = m.BodyFatPercentage;
                survey.MuscleMassKg = m.MuscleMass;
                src.Label = "Số đo trong hồ sơ hội viên";
                src.IsStale = true;
            }
            src.InBodyBmr = m.BMR;
            return (survey, src);
        }

        /// <summary>
        /// Chỉ dùng BMR trong hồ sơ khi đó không phải số mặc định và cân nặng hội viên xác nhận gần bằng cân nặng trong hồ sơ
        /// (BMR thay đổi theo cân nặng). Ngược lại để null → tính theo Mifflin-St Jeor.
        /// </summary>
        private static double? InBodyBmrFor(MemberEntity m, PlanSurveyDto s)
        {
            if (!m.BMR.HasValue || m.BMR == 1500 || IsDefaultProfile(m)) return null;
            var profileWeight = m.ProgressRecords.OrderByDescending(p => p.RecordedDate).Select(p => (double?)p.Weight).FirstOrDefault() ?? m.Weight;
            if (profileWeight == null || s.WeightKg == null || Math.Abs(profileWeight.Value - s.WeightKg.Value) > 1.0) return null;
            return m.BMR;
        }

        private async Task<int?> LatestTrainerIdAsync(int memberId) =>
            await _context.Bookings.AsNoTracking()
                .Where(b => b.MemberId == memberId && (b.Status == "Approved" || b.Status == "Completed"))
                .OrderByDescending(b => b.BookingDate)
                .Select(b => (int?)b.TrainerId)
                .FirstOrDefaultAsync();

        private static void NormalizeSurvey(PlanSurveyDto s)
        {
            s.Goal = PlanOptions.Goals.ContainsKey(s.Goal ?? "") ? s.Goal! : "fat_loss";
            s.Experience = PlanOptions.Experiences.ContainsKey(s.Experience ?? "") ? s.Experience! : "beginner";
            s.Activity = PlanOptions.Activities.ContainsKey(s.Activity ?? "") ? s.Activity! : "sedentary";
            s.Diet = PlanOptions.Diets.ContainsKey(s.Diet ?? "") ? s.Diet! : "normal";
            s.Condition = PlanOptions.Conditions.ContainsKey(s.Condition ?? "") ? s.Condition! : "none";
            s.Allergies = (s.Allergies ?? new List<string>()).Where(PlanOptions.Allergies.ContainsKey).Distinct().ToList();
            s.Injuries = (s.Injuries ?? new List<string>()).Where(PlanOptions.Injuries.ContainsKey).Distinct().ToList();
            s.Weekdays = (s.Weekdays ?? new List<int>()).Where(d => d >= 1 && d <= 7).Distinct().OrderBy(d => d).ToList();
            if (!PlanOptions.MinuteChoices.Contains(s.SessionMinutes)) s.SessionMinutes = 60;
            s.Gender = s.Gender?.Trim();
        }

        private void ValidateSurvey(PlanSurveyDto s)
        {
            if (s.Gender is not ("Nam" or "Nữ"))
                ModelState.AddModelError(nameof(s.Gender), "Vui lòng chọn giới tính");
            if (s.DateOfBirth.HasValue)
            {
                var age = FitnessCalculator.Age(s.DateOfBirth.Value, FitnessCalculator.TodayVn());
                if (age < 16 || age > 80)
                    ModelState.AddModelError(nameof(s.DateOfBirth), "Tính năng dành cho người từ 16 đến 80 tuổi. Người ngoài độ tuổi này nên tập theo hướng dẫn trực tiếp của HLV.");
            }
            if (s.Goal == "fat_loss" && s.HeightCm.HasValue && s.WeightKg.HasValue &&
                FitnessCalculator.Bmi(s.WeightKg.Value, s.HeightCm.Value) < FitnessCalculator.UnderweightBmi)
                ModelState.AddModelError(nameof(s.Goal), "BMI của bạn đang dưới 18,5 (thiếu cân) nên không nên giảm mỡ. Hãy chọn \"Giữ dáng\" hoặc \"Tăng cơ\".");
            if (s.Weekdays.Count < s.SessionsPerWeek)
                ModelState.AddModelError(nameof(s.Weekdays), $"Hãy chọn ít nhất {s.SessionsPerWeek} ngày có thể tập trong tuần.");
            if (!s.Consent)
                ModelState.AddModelError(nameof(s.Consent), "Vui lòng đồng ý gửi số đo cho AI để tạo gợi ý.");
        }

        private async Task<PlanViewModel> DraftView(PlanDocument doc, string draftId, int remaining) => new()
        {
            Doc = doc,
            Exercises = await _planService.GetExerciseMapAsync(),
            IsDraft = true,
            DraftId = draftId,
            RemainingToday = remaining
        };

        private static bool TryReadDoc(string? json, out PlanDocument doc)
        {
            doc = new PlanDocument();
            if (string.IsNullOrWhiteSpace(json)) return false;
            try
            {
                var parsed = JsonSerializer.Deserialize<PlanDocument>(json, JsonOptions);
                if (parsed == null) return false;
                doc = parsed;
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private double? ReadNumber(string key)
        {
            var raw = Request.HasFormContentType ? Request.Form[key].ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(raw)) return null;
            raw = raw.Trim().Replace(',', '.');
            return double.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : null;
        }

        private static string DraftKey(string draftId) => $"aiplan:draft:{draftId}";

        private IActionResult JsonError(string message, int status, bool blocked = false) =>
            StatusCode(status, new { success = false, message, blocked });

        private IActionResult NoMemberProfile()
        {
            SetErrorMessage("Tài khoản này chưa có hồ sơ hội viên nên chưa dùng được giáo án.");
            return RedirectToAction("Index", "Profile");
        }
    }
}
