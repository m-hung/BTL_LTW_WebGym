using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ProductCategory Category { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; } = string.Empty; // Tên sản phẩm

        [MaxLength(255)]
        public string? ImageUrl { get; set; } // Ảnh sản phẩm

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá

        public int StockQuantity { get; set; } = 0; // Tồn kho

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        [NotMapped]
        public int Id => ProductId;
    }
}
