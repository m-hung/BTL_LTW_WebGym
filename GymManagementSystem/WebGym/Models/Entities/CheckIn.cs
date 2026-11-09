using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("CheckIns")]
    public class CheckIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckInId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;

        public DateTime? CheckOutTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Method { get; set; } = "Mã QR Code"; // Phương thức: Mã QR Code

        [MaxLength(250)]
        public string? Notes { get; set; }

        // Backward compatibility helper
        [NotMapped]
        public int Id => CheckInId;
    }
}
