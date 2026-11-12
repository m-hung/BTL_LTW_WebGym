using System.Globalization;
using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;

namespace WebGym.Services.Fitness
{
    /// <summary>
    /// Tính các chỉ số dinh dưỡng và khung lịch tập bằng công thức cố định (không gọi mạng, không dùng AI),
    /// để cùng một số đo luôn ra cùng một kết quả.
    /// - BMR: số InBody nếu có, ngược lại dùng Mifflin-St Jeor.
    /// - TDEE = BMR × hệ số vận động (1,2 → 1,725) theo số buổi tập và mức vận động ngoài giờ.
    /// - Calo mục tiêu: giảm mỡ −400, tăng cơ +250, còn lại giữ nguyên; không bao giờ thấp hơn BMR.
    /// - Đạm 1,6–2,0 g/kg (theo khuyến nghị 1,4–2,0 g/kg của ISSN), chất béo 25% calo, tinh bột phần còn lại.
    /// - Nước 35 ml/kg, +500 ml ngày có tập (quy ước của nhóm, không phải khuyến nghị y khoa).
    /// </summary>
    public static class FitnessCalculator
    {
        private static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        public const int FatLossDeficit = 400;
        public const int MuscleGainSurplus = 250;
        private const double KcalPerKgFat = 7700;
        public const int MinKcalFemale = 1200;
        public const int MinKcalMale = 1500;
        public const double UnderweightBmi = 18.5;

        public static double Bmi(double weightKg, double heightCm) =>
            heightCm > 0 ? weightKg / Math.Pow(heightCm / 100.0, 2) : 0;

        /// <summary>Ngày hiện tại theo giờ Việt Nam (UTC+7).</summary>
        public static DateTime TodayVn() => DateTime.UtcNow.AddHours(7).Date;

        /// <summary>1 = Thứ 2 ... 7 = Chủ nhật</summary>
        public static int ToWeekday(DateTime date) => date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek;

        public static int Age(DateTime dateOfBirth, DateTime today)
        {
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsMale(string? gender) =>
            !string.IsNullOrWhiteSpace(gender) && (gender.Trim().Equals("Nam", StringComparison.OrdinalIgnoreCase) || gender.Trim().Equals("Male", StringComparison.OrdinalIgnoreCase));

        public static double MifflinStJeor(double weightKg, double heightCm, int age, bool male) =>
            10 * weightKg + 6.25 * heightCm - 5 * age + (male ? 5 : -161);

        /// <summary>Hệ số vận động: 1,2 / 1,375 / 1,55 / 1,725 (có giá trị trung gian khi đi lại vừa phải).</summary>
        public static double ActivityFactor(int sessionsPerWeek, string activity)
        {
            var sessionLevel = sessionsPerWeek >= 4 ? 2.0 : 1.0;
            var lifestyle = activity switch { "manual" => 1.0, "moderate" => 0.5, _ => 0.0 };
            var idx = Math.Min(3.0, sessionLevel + lifestyle);
            return Math.Round(1.2 + 0.175 * idx, 3);
        }

        public static FitnessTargets ComputeTargets(PlanSurveyDto s, double? inBodyBmr, DateTime today)
        {
            var w = s.WeightKg ?? 0;
            var h = s.HeightCm ?? 0;
            var age = s.DateOfBirth.HasValue ? Age(s.DateOfBirth.Value, today) : 25;
            var male = IsMale(s.Gender);
            var heightM = h / 100.0;
            var bmi = heightM > 0 ? w / (heightM * heightM) : 0;

            var useInBody = inBodyBmr.HasValue && inBodyBmr.Value >= 800 && inBodyBmr.Value <= 4000;
            var bmr = useInBody ? inBodyBmr!.Value : MifflinStJeor(w, h, age, male);
            var factor = ActivityFactor(s.SessionsPerWeek, s.Activity);
            var tdee = bmr * factor;

            double target = s.Goal switch
            {
                "fat_loss" => tdee - FatLossDeficit,
                "muscle_gain" => tdee + MuscleGainSurplus,
                _ => tdee
            };
            // Sàn an toàn: không thấp hơn BMR và không dưới 1.200 kcal (nữ) / 1.500 kcal (nam),
            // nhưng khi giảm mỡ cũng không vượt quá mức tiêu hao
            var floor = Math.Max(bmr, male ? MinKcalMale : MinKcalFemale);
            target = Math.Max(target, floor);
            if (s.Goal == "fat_loss") target = Math.Min(target, tdee);
            var targetKcal = RoundTo(target, 10);

            var proteinPerKg = s.Goal switch { "fat_loss" => 2.0, "muscle_gain" => 1.8, _ => 1.6 };
            // Người BMI > 30: tính đạm theo cân nặng tham chiếu (BMI 27,5) để không quá cao
            var refWeight = bmi > 30 ? 27.5 * heightM * heightM : w;
            var proteinG = (int)Math.Round(proteinPerKg * refWeight);
            var fatG = (int)Math.Round(targetKcal * 0.25 / 9);
            var carbG = Math.Max(0, (int)Math.Round((targetKcal - proteinG * 4 - fatG * 9) / 4.0));

            var totalMacroKcal = Math.Max(1, proteinG * 4 + fatG * 9 + carbG * 4);
            var proteinPct = (int)Math.Round(proteinG * 4 * 100.0 / totalMacroKcal);
            var fatPct = (int)Math.Round(fatG * 9 * 100.0 / totalMacroKcal);
            var carbPct = Math.Max(0, 100 - proteinPct - fatPct);

            var water = RoundTo(35 * w, 50);

            var t = new FitnessTargets
            {
                Goal = s.Goal,
                Age = age,
                Bmi = Math.Round(bmi, 1),
                Bmr = (int)Math.Round(bmr),
                BmrSource = useInBody ? "InBody" : "Mifflin-St Jeor",
                ActivityFactor = factor,
                Tdee = RoundTo(tdee, 10),
                TargetKcal = targetKcal,
                KcalDelta = targetKcal - RoundTo(tdee, 10),
                ProteinPerKg = proteinPerKg,
                ProteinG = proteinG,
                FatG = fatG,
                CarbG = carbG,
                ProteinPct = proteinPct,
                FatPct = fatPct,
                CarbPct = carbPct,
                WaterMl = water,
                WaterMlTrainingDay = water + 500
            };

            var bmrText = useInBody
                ? $"BMR {N(t.Bmr)} kcal lấy từ kết quả đo InBody"
                : $"BMR {N(t.Bmr)} kcal tính theo công thức Mifflin-St Jeor ({(male ? "nam" : "nữ")}, {age} tuổi, {N(h)} cm, {N(w, 1)} kg)";
            var goalText = s.Goal switch
            {
                "fat_loss" when t.KcalDelta < -FatLossDeficit / 2 => $"trừ {N(-t.KcalDelta)} kcal để giảm mỡ từ từ",
                "fat_loss" => $"chỉ trừ {N(-t.KcalDelta)} kcal vì không nên ăn thấp hơn BMR hoặc dưới {N(male ? MinKcalMale : MinKcalFemale)} kcal/ngày",
                "muscle_gain" => $"cộng {N(t.KcalDelta)} kcal để có năng lượng xây cơ",
                _ => "giữ bằng mức tiêu hao để duy trì cân nặng"
            };
            t.ExplainKcal = $"{bmrText}. Nhân hệ số vận động {factor.ToString("0.###", Vi)} (theo {s.SessionsPerWeek} buổi/tuần và mức vận động ngoài giờ) ra mức tiêu hao khoảng {N(t.Tdee)} kcal/ngày, sau đó {goalText}.";
            t.ExplainMacro = $"Đạm {proteinPerKg.ToString("0.0", Vi)} g/kg{(bmi > 30 ? " (theo cân nặng tham chiếu)" : string.Empty)} = {proteinG} g, nằm trong khoảng 1,4–2,0 g/kg khuyến nghị cho người tập luyện. Chất béo chiếm 25% tổng calo = {fatG} g. Phần calo còn lại dành cho tinh bột = {carbG} g.";
            t.ExplainWater = $"35 ml cho mỗi kg cân nặng = {N(water)} ml. Ngày có tập uống thêm khoảng 500 ml. Đây là mức gợi ý chung, uống nhiều hơn khi trời nóng hoặc ra nhiều mồ hôi.";

            switch (s.Goal)
            {
                case "fat_loss":
                    var deficit = Math.Max(0, t.Tdee - t.TargetKcal);
                    if (deficit < 150)
                    {
                        t.EstimateText = "Duy trì và tăng vận động";
                        t.ExplainEstimate = "Calo mục tiêu đã chạm mức BMR nên mức thâm hụt rất nhỏ. Kết quả chủ yếu đến từ việc tập đều và tăng vận động trong ngày.";
                    }
                    else
                    {
                        var perWeek = Math.Min(1.0, deficit * 7 / KcalPerKgFat);
                        var low = Math.Round(perWeek * 4 * 0.8, 1);
                        var high = Math.Round(perWeek * 4 * 1.2, 1);
                        t.EstimateText = $"Khoảng {N(low, 1)}–{N(high, 1)} kg sau 4 tuần";
                        t.ExplainEstimate = $"Thâm hụt {N(deficit)} kcal/ngày tương đương khoảng {N(perWeek, 2)} kg mỡ mỗi tuần (1 kg mỡ ≈ 7.700 kcal). Con số thực tế còn phụ thuộc giấc ngủ, nước trong cơ thể và mức độ theo sát thực đơn, nên chỉ là ước tính.";
                    }
                    break;
                case "muscle_gain":
                    t.EstimateText = "Đo lại InBody sau 4 tuần";
                    t.ExplainEstimate = "Tốc độ tăng cơ khác nhau giữa từng người nên hệ thống không hứa số kg. Hãy đo InBody với HLV sau 4 tuần để xem khối lượng cơ thay đổi thế nào.";
                    break;
                case "endurance":
                    t.EstimateText = "Giữ cân, tăng sức bền";
                    t.ExplainEstimate = "Mục tiêu là cải thiện sức bền qua cardio và bài tập toàn thân, cân nặng gần như giữ nguyên. Đo lại sau 4 tuần để so sánh.";
                    break;
                default:
                    t.EstimateText = "Giữ cân, săn chắc hơn";
                    t.ExplainEstimate = "Calo bằng mức tiêu hao nên cân nặng gần như giữ nguyên, cơ thể săn chắc hơn nhờ tập đều. Đo lại sau 4 tuần để so sánh.";
                    break;
            }

            return t;
        }

        /// <summary>Số bài mỗi buổi theo thời lượng: 45 phút → 4, 60 phút → 5, 90 phút → 6.</summary>
        public static int ExercisesPerSession(int minutes) => minutes <= 45 ? 4 : minutes >= 90 ? 6 : 5;

        /// <summary>
        /// Chọn kiểu chia lịch theo số buổi và kinh nghiệm, rồi xếp vào các thứ hội viên rảnh
        /// sao cho các buổi cách nhau đều nhất.
        /// </summary>
        public static SplitPlan BuildSplit(PlanSurveyDto s)
        {
            var sessions = Math.Clamp(s.SessionsPerWeek, 2, 5);
            string name;
            string[] focuses;
            switch (sessions)
            {
                case 2:
                    name = "Toàn thân A/B";
                    focuses = new[] { "full_a", "full_b" };
                    break;
                case 3 when s.Experience != "experienced":
                    name = "Toàn thân A/B luân phiên";
                    focuses = new[] { "full_a", "full_b", "full_a" };
                    break;
                case 3:
                    name = "Đẩy / Kéo / Chân";
                    focuses = new[] { "push", "pull", "legs" };
                    break;
                case 4:
                    name = "Thân trên / Thân dưới × 2";
                    focuses = new[] { "upper_a", "lower_a", "upper_b", "lower_b" };
                    break;
                default:
                    name = "Đẩy / Kéo / Chân / Thân trên / Thân dưới";
                    focuses = new[] { "push", "pull", "legs", "upper", "lower" };
                    break;
            }

            var days = PickDays(s.Weekdays, sessions);
            var n = ExercisesPerSession(s.SessionMinutes);
            var cardio = s.Goal is "fat_loss" or "endurance";
            var cardioMinutes = s.SessionMinutes <= 45 ? 15 : 20;

            var plan = new SplitPlan { Name = name };
            for (var i = 0; i < days.Count; i++)
            {
                var focus = focuses[i];
                plan.Days.Add(new SplitDay
                {
                    Weekday = days[i],
                    Focus = focus,
                    ExerciseCount = n,
                    CategoryQuota = Quota(focus, n),
                    Cardio = cardio,
                    CardioMinutes = cardio ? cardioMinutes : 0
                });
            }
            return plan;
        }

        /// <summary>Số bài cần chọn theo từng nhóm động tác cho một buổi.</summary>
        public static Dictionary<string, int> Quota(string focus, int n)
        {
            var q = new Dictionary<string, int>();
            void Add(string cat, int count) { if (count > 0) q[cat] = q.GetValueOrDefault(cat) + count; }

            switch (focus)
            {
                case "full_a":
                case "full_b":
                    Add(ExerciseCategories.Legs, n >= 5 ? 2 : 1);
                    Add(ExerciseCategories.Push, n >= 6 ? 2 : 1);
                    Add(ExerciseCategories.Pull, 1);
                    Add(ExerciseCategories.Core, 1);
                    break;
                case "push":
                    Add(ExerciseCategories.Push, n - 1);
                    Add(ExerciseCategories.Core, 1);
                    break;
                case "pull":
                    Add(ExerciseCategories.Pull, n - 1);
                    Add(ExerciseCategories.Core, 1);
                    break;
                case "legs":
                case "lower":
                case "lower_a":
                case "lower_b":
                    Add(ExerciseCategories.Legs, n - 1);
                    Add(ExerciseCategories.Core, 1);
                    break;
                default: // upper, upper_a, upper_b
                    Add(ExerciseCategories.Push, (n + 1) / 2);
                    Add(ExerciseCategories.Pull, n / 2);
                    break;
            }
            return q;
        }

        /// <summary>
        /// Chọn đúng <paramref name="sessions"/> ngày trong các thứ hội viên rảnh, ưu tiên khoảng cách đều
        /// (khoảng nghỉ ngắn nhất giữa hai buổi, tính vòng qua tuần sau, là lớn nhất).
        /// </summary>
        public static List<int> PickDays(IEnumerable<int>? available, int sessions)
        {
            var days = (available ?? Enumerable.Empty<int>()).Where(d => d >= 1 && d <= 7).Distinct().OrderBy(d => d).ToList();
            if (days.Count == 0) days = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            if (days.Count <= sessions) return days;

            List<int>? best = null;
            var bestMinGap = -1;
            var bestSpread = double.MaxValue;
            foreach (var combo in Combinations(days, sessions))
            {
                var gaps = new List<int>();
                for (var i = 0; i < combo.Count; i++)
                {
                    var next = i + 1 < combo.Count ? combo[i + 1] : combo[0] + 7;
                    gaps.Add(next - combo[i]);
                }
                var minGap = gaps.Min();
                var avg = gaps.Average();
                var spread = gaps.Sum(g => (g - avg) * (g - avg));
                if (minGap > bestMinGap || (minGap == bestMinGap && spread < bestSpread))
                {
                    best = combo;
                    bestMinGap = minGap;
                    bestSpread = spread;
                }
            }
            return best ?? days.Take(sessions).ToList();
        }

        private static IEnumerable<List<int>> Combinations(List<int> items, int k, int start = 0)
        {
            if (k == 0)
            {
                yield return new List<int>();
                yield break;
            }
            for (var i = start; i <= items.Count - k; i++)
            {
                foreach (var rest in Combinations(items, k - 1, i + 1))
                {
                    rest.Insert(0, items[i]);
                    yield return rest;
                }
            }
        }

        /// <summary>Sets/reps/nghỉ gợi ý theo mục tiêu và kinh nghiệm (dùng cho giáo án mẫu và hướng dẫn AI).</summary>
        public static (int Sets, string Reps, int RestSec) Prescription(string goal, string experience, string category)
        {
            if (category == ExerciseCategories.Core) return (3, "12-15", 45);
            var beginner = experience != "experienced";
            return goal switch
            {
                "muscle_gain" => (beginner ? 3 : 4, "8-10", 90),
                "fat_loss" => (3, "12-15", 60),
                "endurance" => (3, "15-20", 45),
                _ => (3, "10-12", 75)
            };
        }

        public static int RoundTo(double value, int step) => (int)(Math.Round(value / step) * step);

        public static string N(double value, int decimals = 0) => value.ToString(decimals == 0 ? "N0" : "N" + decimals, Vi);
    }
}
