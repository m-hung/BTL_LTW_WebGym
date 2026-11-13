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

        [NotMapped]
        public string DisplayImageUrl => ResolveImageUrl(ImageUrl, EquipmentName, Area);

        public static string ResolveImageUrl(string? url, string? name, string? area)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                var trimmed = url.Trim();
                if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
                    trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed;
                }
                if (trimmed.StartsWith("/"))
                {
                    return trimmed;
                }
                if (trimmed.StartsWith("~/"))
                {
                    var pathWithoutTilde = trimmed.Substring(1);
                    var physicalPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", pathWithoutTilde.TrimStart('/').Replace('/', System.IO.Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        return pathWithoutTilde;
                    }
                }
            }

            var n = name?.ToLowerInvariant() ?? "";
            var a = area?.ToLowerInvariant() ?? "";

            if (n.Contains("chạy bộ") || n.Contains("treadmill") || n.Contains("skillrun"))
                return "https://images.unsplash.com/photo-1576678927484-cc907957088c?w=800&auto=format&fit=crop";

            if (n.Contains("xe đạp") || n.Contains("bike") || n.Contains("spinning") || n.Contains("cxp"))
                return "https://images.unsplash.com/photo-1594737625785-a6cbdabd333c?w=800&auto=format&fit=crop";

            if (n.Contains("chèo thuyền") || n.Contains("rower") || n.Contains("concept2"))
                return "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?w=800&auto=format&fit=crop";

            if (n.Contains("smith") || n.Contains("gánh tạ trợ lực"))
                return "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&auto=format&fit=crop";

            if (n.Contains("power rack") || n.Contains("squat rack") || n.Contains("rogue monster"))
                return "https://images.unsplash.com/photo-1584735935682-2f2b69dff9d2?w=800&auto=format&fit=crop";

            if (n.Contains("tạ tay") || n.Contains("dumbbell") || n.Contains("giàn tạ"))
                return "https://images.unsplash.com/photo-1638803040283-7a5ffd48dad5?w=800&auto=format&fit=crop";

            if (n.Contains("ép ngực") || n.Contains("pec") || n.Contains("fly") || n.Contains("rear delt"))
                return "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=800&auto=format&fit=crop";

            if (n.Contains("đạp đùi") || n.Contains("leg press") || n.Contains("chân"))
                return "https://images.unsplash.com/photo-1540497077202-7c8a3999166f?w=800&auto=format&fit=crop";

            if (n.Contains("cáp") || n.Contains("pulley") || n.Contains("kéo xô"))
                return "https://images.unsplash.com/photo-1571902943202-507ec2618e8f?w=800&auto=format&fit=crop";

            if (a.Contains("cardio"))
                return "https://images.unsplash.com/photo-1576678927484-cc907957088c?w=800&auto=format&fit=crop";

            return "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&auto=format&fit=crop";
        }
    }
}
