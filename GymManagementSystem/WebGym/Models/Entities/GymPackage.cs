using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("GymPackages")]
    public class GymPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }

        [Required]
        [MaxLength(150)]
        public string PackageName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public bool IncludesPT { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Backward compatibility helpers
        [NotMapped]
        public int Id => PackageId;

        [NotMapped]
        public string Name => PackageName;
    }
}
