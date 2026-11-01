using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Trainers")]
    public class Trainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Specialization { get; set; } = "General Fitness";

        public string? Bio { get; set; }

        public string? AvatarUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Backward compatibility helpers
        [NotMapped]
        public int Id => TrainerId;

        [NotMapped]
        public bool IsAvailable { get; set; } = true;
    }
}
