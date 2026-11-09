using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int? MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Hoàn thành"; // 'Chờ xác nhận', 'Đã thanh toán', 'Đang giao', 'Hoàn thành', 'Đã hủy'

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "Tiền mặt"; // 'Tiền mặt', 'VNPay', 'Momo', 'Chuyển khoản'

        [MaxLength(250)]
        public string? ShippingAddress { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        [NotMapped]
        public int Id => OrderId;
    }
}
