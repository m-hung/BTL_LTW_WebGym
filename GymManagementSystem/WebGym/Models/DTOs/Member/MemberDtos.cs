using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.Member
{
    public class MemberProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Các chỉ số thể hình
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? BMR { get; set; }
        public double? BodyFatPercentage { get; set; }
        public double? MuscleMass { get; set; }
        public double? Chest { get; set; }
        public double? Waist { get; set; }
        public double? Hips { get; set; }

        public string? CurrentPackageName { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public bool IsMembershipActive => MembershipEndDate.HasValue && MembershipEndDate.Value >= DateTime.UtcNow;
    }

    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Vui lòng chọn huấn luyện viên")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày tập")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Today.AddDays(1);

        [Required(ErrorMessage = "Vui lòng chọn khung giờ tập")]
        public string TimeSlot { get; set; } = "08:00 - 09:30";

        [MaxLength(300)]
        public string? Notes { get; set; }
    }

    public class MemberPersonalInfoDto
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string? CurrentPackageName { get; set; }
    }

    public class MemberPackageViewModel
    {
        public int MemberId { get; set; }
        public int? PackageId { get; set; }
        public int DurationDays { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public bool HasActivePackage { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string PackageTier { get; set; } = "VIP";
        public string Status { get; set; } = "Active";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public int RemainingDays { get; set; }

        public List<PackageHistoryItemDto> History { get; set; } = new();
    }

    public class PackageHistoryItemDto
    {
        public string InvoiceCode { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Đã thanh toán";
    }
}
