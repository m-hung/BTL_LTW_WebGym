using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("GymClasses")]
    public class GymClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ClassName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer Trainer { get; set; } = null!;

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual GymRoom? Room { get; set; }

        [Required]
        [MaxLength(100)]
        public string Schedule { get; set; } = string.Empty; // Ví dụ: "Thứ 2 - 4 - 6 (18:00 - 19:30)"

        public int MaxCapacity { get; set; } = 25;

        public int CurrentCapacity { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open"; // 'Open', 'Full', 'Completed', 'Cancelled'

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        // Navigation Properties
        public virtual ICollection<ClassBooking> ClassBookings { get; set; } = new List<ClassBooking>();

        [NotMapped]
        public int Id => ClassId;
    }
}
