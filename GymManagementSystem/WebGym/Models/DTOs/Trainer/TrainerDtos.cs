using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.Trainer
{
    public class TrainerScheduleDto
    {
        public int BookingId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string? MemberPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class RecordProgressDto
    {
        [Required(ErrorMessage = "Vui lòng chọn học viên")]
        public int MemberId { get; set; }
        public string? MemberName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cân nặng")]
        public double Weight { get; set; }

        public double? BodyFatPercentage { get; set; }
        public double? MuscleMass { get; set; }
        public double? Chest { get; set; }
        public double? Waist { get; set; }
        public double? Hips { get; set; }

        public string? Notes { get; set; }
    }

    public class WorkoutPlanDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        public string RoutineDetails { get; set; } = string.Empty;
        public string NutritionAdvice { get; set; } = string.Empty;
    }

    /// <summary>Trang Thông tin cá nhân của HLV (/PT/Profile).</summary>
    public class TrainerProfileDto
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        // Email đăng nhập: chỉ hiển thị, không cho sửa (giống trang Member)
        [Display(Name = "Email đăng nhập")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chuyên môn")]
        [StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        [Display(Name = "Chuyên môn")]
        public string Specialization { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Tối đa 1000 ký tự")]
        [Display(Name = "Giới thiệu bản thân")]
        public string? Bio { get; set; }

        // Chỉ dùng để hiển thị
        public int TrainerId { get; set; }
        public string? AvatarPath { get; set; }
        public int MemberCount { get; set; }
        public int CompletedSessions { get; set; }
        public int UpcomingSessions { get; set; }
    }
}
