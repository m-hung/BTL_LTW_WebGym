using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Progress")]
    public class Progress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        public DateTime RecordedDate { get; set; } = DateTime.UtcNow;

        public double Weight { get; set; } // kg

        public double? BodyFatPercentage { get; set; } // %

        public double? MuscleMass { get; set; } // kg

        public double? Chest { get; set; } // cm

        public double? Waist { get; set; } // cm

        public double? Hips { get; set; } // cm

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Backward compatibility helper
        [NotMapped]
        public int Id => ProgressId;
    }
}
