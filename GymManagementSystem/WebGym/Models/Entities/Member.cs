using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGym.Models.Entities
{
    [Table("Members")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [MaxLength(20)]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [MaxLength(50)]
        public string? CardCode { get; set; } // Mã thẻ hội viên (vd: GYM-MB-0001)

        [MaxLength(500)]
        public string? QrCode { get; set; } // Mã QR hoặc chuỗi mã hóa thẻ hội viên

        public double? Height { get; set; } // Chiều cao (cm)
        public double? Weight { get; set; } // Cân nặng ban đầu (kg)
        public double? BMR { get; set; } // Basal Metabolic Rate (kcal)
        public double? BodyFatPercentage { get; set; } // Tỷ lệ mỡ cơ thể (%)
        public double? MuscleMass { get; set; } // Khối lượng cơ (kg)
        public double? Chest { get; set; } // Vòng ngực (cm)
        public double? Waist { get; set; } // Vòng eo (cm)
        public double? Hips { get; set; } // Vòng hông (cm)

        // Navigation Properties
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Progress> ProgressRecords { get; set; } = new List<Progress>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<ClassBooking> ClassBookings { get; set; } = new List<ClassBooking>();
        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

        // Backward compatibility helpers
        [NotMapped]
        public int Id => MemberId;

        [NotMapped]
        public double? HeightCm => Height;

        [NotMapped]
        public double? WeightKg => Weight;

        [NotMapped]
        public string MemberCardCode => !string.IsNullOrWhiteSpace(CardCode) ? CardCode : $"GYM-MB-{MemberId:D4}";

        [NotMapped]
        public string MemberQrData => !string.IsNullOrWhiteSpace(QrCode) ? QrCode : MemberCardCode;
    }
}
