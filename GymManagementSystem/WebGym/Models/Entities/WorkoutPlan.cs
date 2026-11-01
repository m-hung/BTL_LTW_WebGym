using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    public class WorkoutPlan : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer Trainer { get; set; } = null!;

        [MaxLength(200)]
        public string Goal { get; set; } = "Weight Loss / Muscle Gain";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string RoutineDetails { get; set; } = string.Empty; // JSON or markdown of weekly exercises
        public string NutritionAdvice { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
