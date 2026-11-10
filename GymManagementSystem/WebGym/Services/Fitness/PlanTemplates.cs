using System.Text.RegularExpressions;
using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;

namespace WebGym.Services.Fitness
{
    /// <summary>
    /// Giáo án mẫu dùng khi AI lỗi / hết giờ / sai định dạng, và bộ lọc thực phẩm theo chế độ ăn, dị ứng.
    /// Không gọi mạng nên luôn trả về kết quả.
    /// </summary>
    public static class PlanTemplates
    {
        public static readonly string[] MealSlots = { "Sáng", "Trưa", "Xế (trước tập 1 tiếng)", "Tối" };
        private static readonly double[] MealShare = { 0.25, 0.35, 0.10, 0.30 };

        // ===== Bộ lọc thực phẩm =====

        private static readonly string[] MeatAndFish =
        {
            "thịt", "bò", "heo", "lợn", "gà", "vịt", "cá", "tôm", "cua", "ghẹ", "mực", "bạch tuộc", "hải sản",
            "sườn", "ba chỉ", "chả", "giò", "nem", "xúc xích", "pate", "nghêu", "sò", "hến", "ốc", "hàu", "lươn", "ếch", "cá hồi", "cá ngừ"
        };

        private static readonly Dictionary<string, string[]> AllergyKeywords = new()
        {
            ["seafood"] = new[] { "tôm", "cua", "ghẹ", "mực", "bạch tuộc", "nghêu", "sò", "hến", "ốc", "hàu", "hải sản", "cá hồi", "cá ngừ", "cá thu" },
            ["dairy"] = new[] { "phô mai", "whey", "kem", "váng sữa", "bơ sữa" }, // "sữa" xử lý riêng bên dưới
            ["peanut"] = new[] { "đậu phộng", "lạc", "bơ đậu phộng" },
            ["egg"] = new[] { "trứng" },
            ["gluten"] = new[] { "bánh mì", "mì", "mỳ", "nui", "bột mì", "lúa mì", "bánh bao", "yến mạch", "mì ống" }
        };

        // "sữa" nhưng không phải sữa thực vật
        private static readonly Regex AnimalMilk = new(@"(?<!\p{L})sữa(?!\s*(đậu nành|hạt|óc chó|hạnh nhân|gạo|yến mạch))(?!\p{L})",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>Trả về từ khoá vi phạm đầu tiên (nếu có) của một món theo chế độ ăn và dị ứng.</summary>
        public static string? FindViolation(string dish, string diet, IEnumerable<string> allergies)
        {
            if (string.IsNullOrWhiteSpace(dish)) return null;
            var text = dish.ToLowerInvariant();

            if (diet == "vegetarian")
            {
                var hit = MeatAndFish.FirstOrDefault(k => ContainsWord(text, k));
                // "đậu hũ", "chả chay", "gà chay"... vẫn hợp lệ nếu có chữ "chay" đi kèm
                if (hit != null && !ContainsWord(text, hit + " chay")) return hit;
            }

            foreach (var a in allergies ?? Enumerable.Empty<string>())
            {
                if (a == "dairy" && AnimalMilk.IsMatch(text)) return "sữa";
                if (AllergyKeywords.TryGetValue(a, out var words))
                {
                    var hit = words.FirstOrDefault(k => ContainsWord(text, k));
                    if (hit != null) return hit;
                }
            }
            return null;
        }

        public static string ForbiddenSummary(string diet, IEnumerable<string> allergies)
        {
            var parts = new List<string>();
            if (diet == "vegetarian") parts.Add("ăn chay: không thịt, cá, hải sản (được dùng trứng, sữa, đậu hũ nếu không dị ứng)");
            foreach (var a in allergies ?? Enumerable.Empty<string>())
            {
                parts.Add(a switch
                {
                    "seafood" => "dị ứng hải sản: không tôm, cua, mực, nghêu, sò, cá biển",
                    "dairy" => "dị ứng sữa: không sữa bò, sữa chua, phô mai, whey (được dùng sữa đậu nành, sữa hạt)",
                    "peanut" => "dị ứng đậu phộng: không đậu phộng/lạc",
                    "egg" => "dị ứng trứng: không trứng",
                    "gluten" => "không gluten: không bánh mì, mì, nui, yến mạch, bánh bao",
                    _ => a
                });
            }
            return parts.Count == 0 ? "không có hạn chế" : string.Join("; ", parts);
        }

        private static bool ContainsWord(string text, string word) =>
            Regex.IsMatch(text, @"(?<!\p{L})" + Regex.Escape(word) + @"(?!\p{L})", RegexOptions.CultureInvariant);

        // ===== Thực đơn mẫu =====

        private static readonly string[][] MealCandidates =
        {
            new[] // Sáng
            {
                "Phở bò ít bánh nhiều thịt",
                "Yến mạch nấu sữa tươi không đường với chuối",
                "Xôi đậu xanh và sữa đậu nành không đường",
                "Bánh mì trứng ốp la và dưa leo",
                "Bún gạo xào rau củ với đậu hũ chiên ít dầu",
                "Khoai lang luộc và sữa đậu nành không đường"
            },
            new[] // Trưa
            {
                "Cơm trắng, ức gà áp chảo, bông cải xanh luộc",
                "Cơm gạo lứt, thịt bò xào cần tây, canh rau ngót",
                "Bún thịt nạc nướng nhiều rau sống",
                "Cơm gạo lứt, đậu hũ sốt cà chua, rau muống luộc",
                "Cơm trắng, nấm xào đậu hũ, canh bí xanh"
            },
            new[] // Xế
            {
                "1 quả chuối và 1 hộp sữa chua không đường",
                "1 quả chuối và 1 ly sữa đậu nành không đường",
                "1 củ khoai lang luộc và 1 quả táo"
            },
            new[] // Tối
            {
                "Cá hồi áp chảo, salad rau củ, nửa chén cơm",
                "Thịt heo nạc luộc, canh bí, rau luộc, nửa chén cơm",
                "Cá basa hấp gừng, rau củ luộc, nửa chén cơm",
                "Đậu hũ non hấp nấm, canh rau dền, nửa chén cơm gạo lứt",
                "Canh nấm đậu hũ, rau cải luộc, nửa chén cơm gạo lứt"
            }
        };

        public static List<PlanMeal> Meals(FitnessTargets t, string diet, IReadOnlyCollection<string> allergies)
        {
            var meals = new List<PlanMeal>();
            for (var i = 0; i < MealSlots.Length; i++)
            {
                var candidates = MealCandidates[i].AsEnumerable();
                // Eat clean: ưu tiên yến mạch, gạo lứt, khoai lang
                if (diet == "clean")
                    candidates = candidates.OrderByDescending(d => d.Contains("gạo lứt") || d.Contains("yến mạch") || d.Contains("khoai lang"));
                var dish = candidates.FirstOrDefault(d => FindViolation(d, diet, allergies) == null)
                           ?? "Cơm trắng, rau củ luộc và đậu hũ";
                meals.Add(new PlanMeal
                {
                    Slot = MealSlots[i],
                    Dish = dish,
                    Kcal = FitnessCalculator.RoundTo(t.TargetKcal * MealShare[i], 10),
                    ProteinG = (int)Math.Round(t.ProteinG * MealShare[i])
                });
            }
            return meals;
        }

        // ===== Lịch tập mẫu =====

        public static List<PlanDay> Days(SplitPlan split, PlanSurveyDto survey, IReadOnlyList<Exercise> allowed)
        {
            var result = new List<PlanDay>();
            var seenFocus = new Dictionary<string, int>();
            foreach (var d in split.Days)
            {
                // Hai buổi cùng kiểu (vd Toàn thân A lần 2) xoay vòng sang bài khác nếu đủ bài
                var occurrence = seenFocus.GetValueOrDefault(d.Focus);
                seenFocus[d.Focus] = occurrence + 1;
                var variant = d.Focus.EndsWith("_b") ? 1 : 0;
                result.Add(Day(d, survey, allowed, variant + occurrence));
            }
            return result;
        }

        public static PlanDay Day(SplitDay d, PlanSurveyDto survey, IReadOnlyList<Exercise> allowed, int offset)
        {
            var day = new PlanDay { Weekday = d.Weekday, Focus = PlanOptions.Label(PlanOptions.Focuses, d.Focus) };
            foreach (var (cat, count) in d.CategoryQuota)
            {
                var pool = allowed.Where(e => e.Category == cat).ToList();
                if (pool.Count == 0) continue;
                for (var i = 0; i < count && i < pool.Count; i++)
                {
                    var ex = pool[(i + offset * count) % pool.Count];
                    if (day.Exercises.Any(x => x.ExerciseId == ex.ExerciseId)) ex = pool.First(p => day.Exercises.All(x => x.ExerciseId != p.ExerciseId));
                    var (sets, reps, rest) = FitnessCalculator.Prescription(survey.Goal, survey.Experience, cat);
                    if (ex.ExerciseId == 35) reps = "30-45 giây"; // Plank
                    day.Exercises.Add(new PlanExercise { ExerciseId = ex.ExerciseId, Sets = sets, Reps = reps, RestSec = rest });
                }
            }
            day.Cardio = CardioText(d, allowed);
            return day;
        }

        public static string? CardioText(SplitDay d, IReadOnlyList<Exercise> allowed)
        {
            if (!d.Cardio) return null;
            var machine = allowed.FirstOrDefault(e => e.Category == ExerciseCategories.Cardio);
            var name = machine?.Name ?? "Đi bộ nhanh";
            return $"{d.CardioMinutes} phút {name}, cường độ vừa (vẫn nói chuyện được)";
        }

        public static List<string> Tips(string goal) => goal switch
        {
            "fat_loss" => new List<string>
            {
                "Ăn đủ đạm mỗi bữa để giữ cơ trong lúc giảm mỡ.",
                "Đi bộ thêm 6.000–8.000 bước vào ngày nghỉ.",
                "Ngủ đủ 7–8 tiếng, thiếu ngủ dễ làm bạn thèm ăn."
            },
            "muscle_gain" => new List<string>
            {
                "Tăng mức tạ từ từ khi làm được đủ số lần ở hiệp cuối.",
                "Ăn một bữa có đạm và tinh bột trong 2 tiếng sau buổi tập.",
                "Ngủ đủ 7–8 tiếng để cơ phục hồi và phát triển."
            },
            "endurance" => new List<string>
            {
                "Tăng thời gian cardio thêm 5 phút mỗi tuần nếu thấy nhẹ.",
                "Uống nước đều trong lúc tập, đừng đợi khát.",
                "Giãn cơ 5–10 phút sau mỗi buổi tập."
            },
            _ => new List<string>
            {
                "Giữ đều số buổi mỗi tuần quan trọng hơn tập thật nặng.",
                "Ăn nhiều rau và uống đủ nước mỗi ngày.",
                "Đo lại InBody sau 4 tuần để theo dõi thay đổi."
            }
        };
    }
}
