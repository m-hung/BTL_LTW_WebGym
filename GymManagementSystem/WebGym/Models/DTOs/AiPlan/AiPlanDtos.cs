using System.ComponentModel.DataAnnotations;
using WebGym.Models.Entities;

namespace WebGym.Models.DTOs.AiPlan
{
    /// <summary>Các lựa chọn của form khảo sát (giá trị gửi lên → nhãn tiếng Việt).</summary>
    public static class PlanOptions
    {
        public static readonly Dictionary<string, string> Goals = new()
        {
            ["fat_loss"] = "Giảm mỡ",
            ["muscle_gain"] = "Tăng cơ",
            ["maintain"] = "Giữ dáng",
            ["endurance"] = "Tăng thể lực"
        };

        public static readonly Dictionary<string, string> Experiences = new()
        {
            ["beginner"] = "Mới bắt đầu (dưới 6 tháng)",
            ["experienced"] = "Đã tập trên 6 tháng"
        };

        public static readonly Dictionary<string, string> Activities = new()
        {
            ["sedentary"] = "Ngồi nhiều (văn phòng, học tập)",
            ["moderate"] = "Đi lại vừa phải",
            ["manual"] = "Lao động chân tay"
        };

        public static readonly Dictionary<string, string> Diets = new()
        {
            ["normal"] = "Ăn bình thường",
            ["clean"] = "Eat clean",
            ["vegetarian"] = "Ăn chay (không thịt, cá, hải sản)"
        };

        public static readonly Dictionary<string, string> Allergies = new()
        {
            ["seafood"] = "Hải sản",
            ["dairy"] = "Sữa",
            ["peanut"] = "Đậu phộng",
            ["egg"] = "Trứng",
            ["gluten"] = "Gluten (lúa mì)"
        };

        public static readonly Dictionary<string, string> Injuries = new()
        {
            ["back"] = "Chấn thương lưng",
            ["knee"] = "Chấn thương gối",
            ["shoulder"] = "Chấn thương vai"
        };

        public static readonly Dictionary<string, string> Conditions = new()
        {
            ["none"] = "Không có",
            ["cardio"] = "Bệnh tim mạch / huyết áp",
            ["pregnant"] = "Đang mang thai"
        };

        public static readonly int[] SessionChoices = { 2, 3, 4, 5 };
        public static readonly int[] MinuteChoices = { 45, 60, 90 };

        public static string Label(Dictionary<string, string> map, string? key) =>
            key != null && map.TryGetValue(key, out var v) ? v : (key ?? string.Empty);

        /// <summary>1 = Thứ 2 ... 6 = Thứ 7, 7 = Chủ nhật</summary>
        public static string WeekdayLabel(int weekday) => weekday switch
        {
            1 => "Thứ 2",
            2 => "Thứ 3",
            3 => "Thứ 4",
            4 => "Thứ 5",
            5 => "Thứ 6",
            6 => "Thứ 7",
            7 => "Chủ nhật",
            _ => "?"
        };

        public static string WeekdayShort(int weekday) => weekday == 7 ? "CN" : $"T{weekday + 1}";

        public static readonly Dictionary<string, string> Focuses = new()
        {
            ["full_a"] = "Toàn thân A",
            ["full_b"] = "Toàn thân B",
            ["push"] = "Đẩy (Ngực, Vai, Tay sau)",
            ["pull"] = "Kéo (Lưng, Tay trước)",
            ["legs"] = "Chân & Mông",
            ["upper"] = "Thân trên",
            ["upper_a"] = "Thân trên A",
            ["upper_b"] = "Thân trên B",
            ["lower"] = "Thân dưới",
            ["lower_a"] = "Thân dưới A",
            ["lower_b"] = "Thân dưới B"
        };
    }

    /// <summary>Bước 1: số đo đã xác nhận + câu trả lời khảo sát.</summary>
    public class PlanSurveyDto
    {
        [Required(ErrorMessage = "Vui lòng nhập chiều cao")]
        [Range(120, 230, ErrorMessage = "Chiều cao từ 120 đến 230 cm")]
        public double? HeightCm { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cân nặng")]
        [Range(30, 250, ErrorMessage = "Cân nặng từ 30 đến 250 kg")]
        public double? WeightKg { get; set; }

        [Range(3, 60, ErrorMessage = "Tỷ lệ mỡ từ 3 đến 60%")]
        public double? BodyFatPercent { get; set; }

        [Range(10, 120, ErrorMessage = "Khối lượng cơ từ 10 đến 120 kg")]
        public double? MuscleMassKg { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public string? Gender { get; set; } // "Nam" | "Nữ"

        [Required]
        public string Goal { get; set; } = "fat_loss";

        [Range(2, 5)]
        public int SessionsPerWeek { get; set; } = 3;

        public List<int> Weekdays { get; set; } = new();

        public int SessionMinutes { get; set; } = 60;

        public string Experience { get; set; } = "beginner";

        public string Activity { get; set; } = "sedentary";

        public string Diet { get; set; } = "normal";

        public List<string> Allergies { get; set; } = new();

        public List<string> Injuries { get; set; } = new();

        public string Condition { get; set; } = "none";

        public bool Consent { get; set; }
    }

    /// <summary>Các chỉ số do server tính (không dùng AI).</summary>
    public class FitnessTargets
    {
        public string Goal { get; set; } = "fat_loss";
        public int Age { get; set; }
        public double Bmi { get; set; }
        public int Bmr { get; set; }
        public string BmrSource { get; set; } = "Mifflin-St Jeor";
        public double ActivityFactor { get; set; }
        public int Tdee { get; set; }
        public int TargetKcal { get; set; }
        public int KcalDelta { get; set; } // âm = thâm hụt, dương = dư
        public double ProteinPerKg { get; set; }
        public int ProteinG { get; set; }
        public int FatG { get; set; }
        public int CarbG { get; set; }
        public int ProteinPct { get; set; }
        public int FatPct { get; set; }
        public int CarbPct { get; set; }
        public int WaterMl { get; set; }
        public int WaterMlTrainingDay { get; set; }
        public string EstimateText { get; set; } = string.Empty;
        public string ExplainKcal { get; set; } = string.Empty;
        public string ExplainMacro { get; set; } = string.Empty;
        public string ExplainWater { get; set; } = string.Empty;
        public string ExplainEstimate { get; set; } = string.Empty;
    }

    /// <summary>Khung một buổi tập do server xếp (AI chỉ điền bài vào khung này).</summary>
    public class SplitDay
    {
        public int Weekday { get; set; }
        public string Focus { get; set; } = string.Empty;
        public int ExerciseCount { get; set; }
        public Dictionary<string, int> CategoryQuota { get; set; } = new(); // Push/Pull/Legs/Core → số bài
        public bool Cardio { get; set; }
        public int CardioMinutes { get; set; }
    }

    public class SplitPlan
    {
        public string Name { get; set; } = string.Empty;
        public List<SplitDay> Days { get; set; } = new();
    }

    // ===== Giáo án có cấu trúc (lưu vào WorkoutPlans.PlanJson) =====

    public class PlanDocument
    {
        public int Version { get; set; } = 1;
        public FitnessTargets Targets { get; set; } = new();
        public string SplitName { get; set; } = string.Empty;
        public List<PlanDay> Days { get; set; } = new();
        public List<PlanMeal> Meals { get; set; } = new();
        public List<string> Tips { get; set; } = new();
        public string Origin { get; set; } = "ai"; // ai | template | mixed
        public List<string> Notices { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class PlanDay
    {
        public int Weekday { get; set; }
        public string Focus { get; set; } = string.Empty;
        public List<PlanExercise> Exercises { get; set; } = new();
        public string? Cardio { get; set; }
    }

    public class PlanExercise
    {
        public int ExerciseId { get; set; }
        public int Sets { get; set; }
        public string Reps { get; set; } = string.Empty;
        public int RestSec { get; set; }
        public string? Note { get; set; }
    }

    public class PlanMeal
    {
        public string Slot { get; set; } = string.Empty; // Sáng | Trưa | Xế (trước tập) | Tối
        public string Dish { get; set; } = string.Empty;
        public int Kcal { get; set; }
        public int ProteinG { get; set; }
    }

    /// <summary>Phần AI trả về (JSON theo responseSchema).</summary>
    public class AiPlanResponse
    {
        public List<PlanDay> Days { get; set; } = new();
        public List<PlanMeal> Meals { get; set; } = new();
        public List<string> Tips { get; set; } = new();
    }

    /// <summary>Kết quả tạo giáo án gửi về controller.</summary>
    public class PlanGenerationResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public PlanDocument? Document { get; set; }
        public bool UsedAi { get; set; }
        public int RemainingToday { get; set; }
    }

    /// <summary>Dữ liệu cho partial hiển thị giáo án (bản nháp hoặc đã lưu).</summary>
    public class PlanViewModel
    {
        public PlanDocument Doc { get; set; } = new();
        public Dictionary<int, Exercise> Exercises { get; set; } = new();
        public bool IsDraft { get; set; }
        public string? DraftId { get; set; }
        public int? PlanId { get; set; }
        public string? Status { get; set; }
        public string? Source { get; set; }
        public string? TrainerName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int RemainingToday { get; set; }
    }

    /// <summary>Thông tin số đo hiện có để điền sẵn form.</summary>
    public class MeasurementSource
    {
        public string Label { get; set; } = string.Empty; // ví dụ "InBody ngày 02/11/2026"
        public bool IsDefault { get; set; } // số đo mặc định lúc đăng ký (170/65/1500)
        public bool IsStale { get; set; } // cũ hơn 60 ngày
        public double? InBodyBmr { get; set; }
    }

    public class PlanCreateViewModel
    {
        public PlanSurveyDto Survey { get; set; } = new();
        public MeasurementSource Measurement { get; set; } = new();
        public bool HasActivePlan { get; set; }
        public bool ActivePlanFromTrainer { get; set; }
        public bool HasActiveSubscription { get; set; }
        public bool PackageIncludesPt { get; set; }
        public int RemainingToday { get; set; }
    }

    public class TodayViewModel
    {
        public WorkoutPlan? Plan { get; set; }
        public PlanDocument? Doc { get; set; }
        public PlanDay? TodayDay { get; set; }
        public PlanDay? NextDay { get; set; }
        public int Weekday { get; set; }
        public DateTime Date { get; set; }
        public HashSet<int> DoneExerciseIds { get; set; } = new();
        public Dictionary<int, Exercise> Exercises { get; set; } = new();
        public DateTime? CheckInTime { get; set; }
        public int SessionsDoneThisWeek { get; set; }
        public int SessionsPlannedThisWeek { get; set; }
    }
}
