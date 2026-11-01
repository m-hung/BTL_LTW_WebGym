using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [Required]
        public int PackageId { get; set; }

        [ForeignKey("PackageId")]
        public virtual GymPackage Package { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "VNPay"; // 'Momo', 'VNPay', 'BankTransfer'

        [Required]
        [MaxLength(100)]
        public string TransactionCode { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Success"; // 'Success', 'Failed'

        // Backward compatibility helpers
        [NotMapped]
        public int Id => PaymentId;

        [NotMapped]
        public string? TransactionId => TransactionCode;
    }
}
