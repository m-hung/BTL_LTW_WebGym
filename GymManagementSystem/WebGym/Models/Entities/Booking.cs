using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer Trainer { get; set; } = null!;

        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = "08:00 - 09:30";

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // 'Pending', 'Approved', 'Completed', 'Cancelled'

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Backward compatibility helpers
        [NotMapped]
        public int Id => BookingId;
    }
}
