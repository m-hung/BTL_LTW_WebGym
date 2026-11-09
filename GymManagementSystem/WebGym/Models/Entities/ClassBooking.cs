using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("ClassBookings")]
    public class ClassBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassBookingId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual GymClass GymClass { get; set; } = null!;

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Confirmed"; // 'Confirmed', 'Attended', 'Cancelled'

        [MaxLength(250)]
        public string? Notes { get; set; }

        [NotMapped]
        public int Id => ClassBookingId;
    }
}
