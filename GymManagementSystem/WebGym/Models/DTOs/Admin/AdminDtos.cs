using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.Admin
{
    // ================= DTOs CHO DASHBOARD =================
    public class DashboardStatsDto
    {
        public int TotalMembers { get; set; }
        public int TotalTrainers { get; set; }
        public int TotalActiveBookings { get; set; }
        public int TotalActiveSubscriptions { get; set; }
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal TotalRevenueOverall { get; set; }

        // 4 Widget mới (Phân hệ 6)
        // 1. Doanh thu bán hàng Shop & POS
        public decimal ShopRevenueThisMonth { get; set; }
        public decimal ShopRevenueOverall { get; set; }
        public int ShopOrdersThisMonth { get; set; }
        public int ShopOrdersOverall { get; set; }

        // 2. Tỷ lệ lấp đầy Lớp học nhóm
        public double ClassFillRatePercent { get; set; }
        public int TotalClassSlots { get; set; }
        public int BookedClassSlots { get; set; }
        public int ActiveClassesCount { get; set; }

        // 3. Cảnh báo thiết bị hỏng / cần bảo trì
        public int BrokenEquipmentCount { get; set; }
        public int NeedsMaintenanceEquipmentCount { get; set; }
        public int AlertEquipmentTotal => BrokenEquipmentCount + NeedsMaintenanceEquipmentCount;

        // 4. Cảnh báo hàng tồn kho (< 5 cái)
        public int LowStockProductsCount { get; set; }
        public int OutOfStockProductsCount { get; set; }
        public int AlertProductsTotal => LowStockProductsCount + OutOfStockProductsCount;

        public List<MonthlyRevenueItem> MonthlyRevenueChart { get; set; } = new();
        public List<PackageDistributionItem> PackageDistributionChart { get; set; } = new();
        public List<RecentBookingItem> RecentBookings { get; set; } = new();
        public List<RecentPaymentItem> RecentPayments { get; set; } = new();
        public List<RecentShopOrderItem> RecentShopOrders { get; set; } = new();
    }

    public class MonthlyRevenueItem
    {
        public string MonthLabel { get; set; } = string.Empty; // Ví dụ: "Tháng 10"
        public decimal Revenue { get; set; } // Thu Gói tập (giữ tương thích)
        public decimal PackageRevenue { get; set; }
        public decimal ShopRevenue { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal NetRevenue => PackageRevenue + ShopRevenue - MaintenanceCost;
    }

    public class PackageDistributionItem
    {
        public string PackageName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class RecentBookingItem
    {
        public int BookingId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class RecentPaymentItem
    {
        public int PaymentId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RecentShopOrderItem
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ================= DTOs CHO BÁO CÁO DOANH THU =================
    public class RevenueReportDto
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;
        public int? Month { get; set; }
        public string? PaymentMethod { get; set; }
        public string? SourceFilter { get; set; } = "All"; // "All", "Package", "Shop", "Maintenance"

        public decimal TotalRevenue { get; set; } // Tổng thu (Gói + Shop)
        public decimal PackageRevenue { get; set; } // Tiền bán Gói tập
        public decimal ShopRevenue { get; set; } // Tiền bán Hàng Shop / POS
        public decimal MaintenanceCost { get; set; } // Chi phí sửa chữa bảo trì máy
        public decimal NetRevenue => PackageRevenue + ShopRevenue - MaintenanceCost;

        public int TotalTransactions { get; set; }
        public int PackageTransactionsCount { get; set; }
        public int ShopOrdersCount { get; set; }
        public int MaintenanceLogsCount { get; set; }

        public decimal AverageOrderValue => TotalTransactions > 0 ? TotalRevenue / TotalTransactions : 0;
        public string BestSellingPackage { get; set; } = "Chưa có";

        public List<MonthlyRevenueItem> MonthlyBreakdown { get; set; } = new();
        public Dictionary<string, decimal> MethodBreakdown { get; set; } = new();
        public List<ConsolidatedTransactionItem> ConsolidatedItems { get; set; } = new();
    }

    public class ConsolidatedTransactionItem
    {
        public DateTime Date { get; set; }
        public string SourceType { get; set; } = string.Empty; // "Gói tập", "Shop POS", "Bảo trì"
        public string ReferenceCode { get; set; } = string.Empty;
        public string CustomerOrTarget { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsExpense { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ================= DTOs CHO QUẢN LÝ GÓI TẬP =================
    public class GymPackageFormViewModel
    {
        public int PackageId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên gói tập")]
        [MaxLength(150, ErrorMessage = "Tên gói tập tối đa 150 ký tự")]
        [Display(Name = "Tên gói tập")]
        public string PackageName { get; set; } = string.Empty;

        [Display(Name = "Mô tả chi tiết")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá tiền")]
        [Range(0, 1000000000, ErrorMessage = "Giá gói tập không hợp lệ")]
        [Display(Name = "Giá gói (VNĐ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thời hạn")]
        [Range(1, 3650, ErrorMessage = "Thời hạn gói từ 1 đến 3650 ngày")]
        [Display(Name = "Thời hạn (Số ngày)")]
        public int DurationDays { get; set; } = 30;

        [Display(Name = "Bao gồm HLV riêng (PT)")]
        public bool IncludesPT { get; set; } = false;
    }

    // ================= DTOs CHO QUẢN LÝ HỘI VIÊN =================
    public class MemberCreateViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MaxLength(50)]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(150)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; } = "Nam";

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Range(50, 250, ErrorMessage = "Chiều cao không hợp lệ (50-250cm)")]
        [Display(Name = "Chiều cao (cm)")]
        public double? Height { get; set; }

        [Range(20, 300, ErrorMessage = "Cân nặng không hợp lệ (20-300kg)")]
        [Display(Name = "Cân nặng ban đầu (kg)")]
        public double? Weight { get; set; }

        [Display(Name = "Tỷ lệ mỡ (%)")]
        public double? BodyFatPercentage { get; set; }

        [Display(Name = "Khối lượng cơ (kg)")]
        public double? MuscleMass { get; set; }

        [Display(Name = "Vòng ngực (cm)")]
        public double? Chest { get; set; }

        [Display(Name = "Vòng eo (cm)")]
        public double? Waist { get; set; }

        [Display(Name = "Vòng hông (cm)")]
        public double? Hips { get; set; }

        [Display(Name = "Đăng ký gói tập ngay")]
        public int? SelectedPackageId { get; set; }
    }

    public class MemberEditViewModel
    {
        public int MemberId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(150)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Ảnh đại diện (Avatar URL)")]
        public string? AvatarUrl { get; set; }

        [Display(Name = "Chiều cao (cm)")]
        public double? Height { get; set; }

        [Display(Name = "Cân nặng (kg)")]
        public double? Weight { get; set; }

        [Display(Name = "Tỷ lệ mỡ (%)")]
        public double? BodyFatPercentage { get; set; }

        [Display(Name = "Khối lượng cơ (kg)")]
        public double? MuscleMass { get; set; }

        [Display(Name = "Vòng ngực (cm)")]
        public double? Chest { get; set; }

        [Display(Name = "Vòng eo (cm)")]
        public double? Waist { get; set; }

        [Display(Name = "Vòng hông (cm)")]
        public double? Hips { get; set; }

        [Display(Name = "Đổi mật khẩu mới (Bỏ trống nếu không đổi)")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string? NewPassword { get; set; }
    }

    // ================= DTOs CHO QUẢN LÝ HUẤN LUYỆN VIÊN =================
    public class TrainerCreateViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MaxLength(50)]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(150)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chuyên môn")]
        [MaxLength(200)]
        [Display(Name = "Chuyên môn huấn luyện")]
        public string Specialization { get; set; } = "Thể hình & Giảm mỡ";

        [Display(Name = "Tiểu sử / Chứng chỉ")]
        public string? Bio { get; set; }

        [Display(Name = "Link ảnh đại diện")]
        public string? AvatarUrl { get; set; }
    }

    public class TrainerEditViewModel
    {
        public int TrainerId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(150)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chuyên môn")]
        [MaxLength(200)]
        [Display(Name = "Chuyên môn")]
        public string Specialization { get; set; } = string.Empty;

        [Display(Name = "Tiểu sử / Chứng chỉ")]
        public string? Bio { get; set; }

        [Display(Name = "Link ảnh đại diện")]
        public string? AvatarUrl { get; set; }

        [Display(Name = "Đổi mật khẩu mới (Bỏ trống nếu không đổi)")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string? NewPassword { get; set; }
    }
}
