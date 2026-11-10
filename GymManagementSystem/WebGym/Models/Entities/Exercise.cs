using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    /// <summary>
    /// Danh mục bài tập do phòng tập soạn sẵn. AI chỉ được chọn bài trong danh sách này,
    /// lọc theo thiết bị đang "Hoạt động tốt", cấp độ và chấn thương của hội viên.
    /// </summary>
    [Table("Exercises")]
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string MuscleGroup { get; set; } = string.Empty; // Ngực, Lưng, Vai, Tay, Chân, Mông, Bụng, Cardio

        [Required]
        [MaxLength(20)]
        public string Category { get; set; } = ExerciseCategories.Push; // Push | Pull | Legs | Core | Cardio

        [Required]
        [MaxLength(20)]
        public string Level { get; set; } = ExerciseLevels.Basic; // Basic | Advanced

        // Thiết bị cần dùng; null = bài tự trọng lượng cơ thể / thảm
        public int? EquipmentId { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual Equipment? Equipment { get; set; }

        // Vùng chấn thương nên tránh bài này, cách nhau bởi dấu phẩy: back, knee, shoulder
        [MaxLength(50)]
        public string? AvoidIfInjury { get; set; }

        [Required]
        [MaxLength(500)]
        public string HowTo { get; set; } = string.Empty; // Mô tả ngắn cách thực hiện

        public int DefaultRestSec { get; set; } = 90;

        public bool IsActive { get; set; } = true;

        [NotMapped]
        public int Id => ExerciseId;
    }

    public static class ExerciseCategories
    {
        public const string Push = "Push";
        public const string Pull = "Pull";
        public const string Legs = "Legs";
        public const string Core = "Core";
        public const string Cardio = "Cardio";
    }

    public static class ExerciseLevels
    {
        public const string Basic = "Basic";
        public const string Advanced = "Advanced";
    }
}
