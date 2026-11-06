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

    /// <summary>Hồ sơ HLV tự chỉnh sửa (bảng hồ sơ trong trang PT).</summary>
    public class TrainerProfileDto
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Tối đa 20 ký tự")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chuyên môn")]
        [StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        public string Specialization { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Tối đa 1000 ký tự")]
        public string? Bio { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn ảnh tối đa 500 ký tự")]
        public string? AvatarUrl { get; set; }
    }
}
