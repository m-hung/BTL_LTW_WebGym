using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    /// <summary>
    /// Nhật ký tập: mỗi dòng là một bài hội viên đã tích "Đã hoàn thành" trong một ngày.
    /// </summary>
    [Table("WorkoutLogs")]
    public class WorkoutLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public int PlanId { get; set; }

        [ForeignKey("PlanId")]
        public virtual WorkoutPlan Plan { get; set; } = null!;

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime Date { get; set; } // Ngày tập (giờ Việt Nam)

        public int Weekday { get; set; } // 1 = Thứ 2 ... 7 = Chủ nhật

        [Required]
        public int ExerciseId { get; set; }

        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; } = null!;

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public int Id => LogId;
    }
}
