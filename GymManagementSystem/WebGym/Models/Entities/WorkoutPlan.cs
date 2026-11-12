using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("WorkoutPlans")]
    public class WorkoutPlan : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        // Giáo án do AI tạo có thể chưa có HLV; HLV được gán khi duyệt
        public int? TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer? Trainer { get; set; }

        [MaxLength(200)]
        public string Goal { get; set; } = "Weight Loss / Muscle Gain";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string RoutineDetails { get; set; } = string.Empty; // Lịch tập & chi tiết bài tập theo tuần (dạng văn bản)

        [Column(TypeName = "nvarchar(max)")]
        public string NutritionAdvice { get; set; } = string.Empty; // Lời khuyên chế độ dinh dưỡng (dạng văn bản)

        public bool IsActive { get; set; } = true;

        // ===== Giáo án AI =====

        [Required]
        [MaxLength(20)]
        public string Source { get; set; } = WorkoutPlanSources.Trainer; // 'PT' | 'AI'

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = WorkoutPlanStatuses.Active; // 'Active' | 'PendingReview' | 'Archived'

        [Column(TypeName = "nvarchar(max)")]
        public string? PlanJson { get; set; } // Lịch tập + thực đơn có cấu trúc (hiển thị dạng thẻ)

        [Column(TypeName = "nvarchar(max)")]
        public string? InputJson { get; set; } // Số đo và câu trả lời khảo sát lúc tạo

        public int? TargetKcal { get; set; }
        public int? ProteinG { get; set; }
        public int? CarbG { get; set; }
        public int? FatG { get; set; }
        public int? WaterMl { get; set; }

        public virtual ICollection<WorkoutLog> Logs { get; set; } = new List<WorkoutLog>();

        // Backward compatibility helper
        [NotMapped]
        public int PlanId => Id;
    }

    public static class WorkoutPlanSources
    {
        public const string Trainer = "PT";
        public const string Ai = "AI";
    }

    public static class WorkoutPlanStatuses
    {
        public const string Active = "Active";
        public const string PendingReview = "PendingReview";
        public const string Archived = "Archived";
    }
}
