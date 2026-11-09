using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("MaintenanceLogs")]
    public class MaintenanceLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        [ForeignKey("EquipmentId")]
        public virtual Equipment Equipment { get; set; } = null!;

        public DateTime MaintenanceDate { get; set; } = DateTime.UtcNow; // Ngày bảo trì / bảo dưỡng

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty; // Nội dung bảo dưỡng hoặc sự cố

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; } = 0m; // Chi phí

        [Required]
        [MaxLength(100)]
        public string PerformedBy { get; set; } = string.Empty; // Người thực hiện / Đơn vị bảo trì

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Hoàn thành"; // 'Hoàn thành', 'Đang bảo dưỡng', 'Chờ linh kiện'

        [MaxLength(500)]
        public string? Notes { get; set; }

        [NotMapped]
        public int Id => LogId;
    }
}
