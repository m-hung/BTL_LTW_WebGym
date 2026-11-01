using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Subscriptions")]
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriptionId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; } = null!;

        [Required]
        public int PackageId { get; set; }

        [ForeignKey("PackageId")]
        public virtual GymPackage Package { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active"; // 'Active', 'Expired', 'Cancelled'
    }
}
