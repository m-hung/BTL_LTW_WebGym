using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Equipments")]
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipmentId { get; set; }

        [Required]
        [MaxLength(150)]
        public string EquipmentName { get; set; } = string.Empty; // Tên máy

        [Required]
        [MaxLength(100)]
        public string Area { get; set; } = string.Empty; // Khu vực: "Khu tạ tự do", "Khu cardio", "Khu máy kháng lực", "Khu tập cơ chân", "Khu chức năng"

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual GymRoom? GymRoom { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty; // Hãng sản xuất

        public DateTime PurchaseDate { get; set; } // Ngày mua

        public DateTime? LastMaintenanceDate { get; set; } // Ngày bảo dưỡng gần nhất

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Hoạt động tốt"; // Tình trạng: 'Hoạt động tốt', 'Cần bảo trì', 'Đang hỏng'

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();

        [NotMapped]
        public int Id => EquipmentId;
    }
}
