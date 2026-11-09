using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("GymRooms")]
    public class GymRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty; // Ví dụ: "Tầng 1 - Khu A", "Tầng 2 - Studio 1"

        public int Capacity { get; set; } = 30; // Sức chứa tối đa

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Hoạt động"; // 'Hoạt động', 'Bảo trì', 'Tạm đóng'

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
        public virtual ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();

        [NotMapped]
        public int Id => RoomId;
    }
}
