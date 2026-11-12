using System.ComponentModel.DataAnnotations;
using WebGym.Models.Entities;
using Equipment = WebGym.Models.Entities.Equipment;

namespace WebGym.Models.DTOs.Admin
{
    // ==========================================
    // 1. DTOs CHO QUẢN LÝ PHÒNG TẬP (ROOM)
    // ==========================================
    public class RoomFormViewModel
    {
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phòng/studio")]
        [MaxLength(100, ErrorMessage = "Tên phòng tối đa 100 ký tự")]
        [Display(Name = "Tên phòng / Studio")]
        public string RoomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập vị trí phòng")]
        [MaxLength(100, ErrorMessage = "Vị trí tối đa 100 ký tự")]
        [Display(Name = "Vị trí (Khu vực / Tầng)")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập sức chứa")]
        [Range(1, 500, ErrorMessage = "Sức chứa từ 1 đến 500 người")]
        [Display(Name = "Sức chứa tối đa (người)")]
        public int Capacity { get; set; } = 30;

        [Required]
        [Display(Name = "Trạng thái hoạt động")]
        public string Status { get; set; } = "Hoạt động"; // 'Hoạt động', 'Bảo trì', 'Tạm đóng'

        [Display(Name = "Mô tả / Diện tích / Tiện ích")]
        public string? Description { get; set; }
    }

    public class RoomDetailsViewModel
    {
        public GymRoom Room { get; set; } = null!;
        public int TotalEquipments { get; set; }
        public int BrokenEquipments { get; set; }
        public int TotalClasses { get; set; }
        public List<WebGym.Models.Entities.Equipment> Equipments { get; set; } = new();
        public List<GymClass> Classes { get; set; } = new();
    }

    // ==========================================
    // 2. DTOs CHO QUẢN LÝ THIẾT BỊ (EQUIPMENT)
    // ==========================================
    public class EquipmentFormViewModel
    {
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thiết bị / máy móc")]
        [MaxLength(150, ErrorMessage = "Tên thiết bị tối đa 150 ký tự")]
        [Display(Name = "Tên thiết bị / Máy móc")]
        public string EquipmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn hoặc nhập phân khu")]
        [MaxLength(100)]
        [Display(Name = "Khu vực tập luyện")]
        public string Area { get; set; } = "Khu máy khối";

        [Display(Name = "Phòng tập / Studio")]
        public int? RoomId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập hãng sản xuất")]
        [MaxLength(100)]
        [Display(Name = "Hãng sản xuất (Brand)")]
        public string Brand { get; set; } = "Technogym";

        [Required(ErrorMessage = "Vui lòng chọn ngày mua")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày mua / Đưa vào sử dụng")]
        public DateTime PurchaseDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày bảo dưỡng gần nhất")]
        public DateTime? LastMaintenanceDate { get; set; }

        [Required]
        [Display(Name = "Tình trạng hoạt động")]
        public string Status { get; set; } = "Hoạt động tốt"; // 'Hoạt động tốt', 'Cần bảo trì', 'Đang hỏng'

        [MaxLength(100)]
        [Display(Name = "Mã số Serial máy")]
        public string? SerialNumber { get; set; }

        [Display(Name = "Đường dẫn hình ảnh")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Ghi chú kỹ thuật")]
        public string? Notes { get; set; }
    }

    // ==========================================
    // 3. DTOs CHO NHẬT KÝ BẢO TRÌ (MAINTENANCE)
    // ==========================================
    public class MaintenanceLogFormViewModel
    {
        public int LogId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thiết bị cần bảo trì")]
        [Display(Name = "Thiết bị / Máy móc")]
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày thực hiện")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày bảo trì / Sửa chữa")]
        public DateTime MaintenanceDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng mô tả nội dung bảo dưỡng hoặc sự cố")]
        [MaxLength(500, ErrorMessage = "Nội dung tối đa 500 ký tự")]
        [Display(Name = "Nội dung bảo dưỡng / Sự cố")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập chi phí")]
        [Range(0, 500000000, ErrorMessage = "Chi phí từ 0 đến 500.000.000 VNĐ")]
        [Display(Name = "Chi phí sửa chữa (VNĐ)")]
        public decimal Cost { get; set; } = 0m;

        [Required(ErrorMessage = "Vui lòng nhập đơn vị hoặc người thực hiện")]
        [MaxLength(100)]
        [Display(Name = "Người / Đơn vị thực hiện")]
        public string PerformedBy { get; set; } = "Kỹ thuật nội bộ";

        [Required]
        [Display(Name = "Trạng thái bảo trì")]
        public string Status { get; set; } = "Hoàn thành"; // 'Hoàn thành', 'Đang bảo dưỡng', 'Chờ linh kiện'

        [Display(Name = "Ghi chú linh kiện thay thế & nghiệm thu")]
        public string? Notes { get; set; }

        [Display(Name = "Tự động đồng bộ trạng thái thiết bị tương ứng")]
        public bool SyncEquipmentStatus { get; set; } = true;
    }

    public class MaintenanceStatsSummary
    {
        public decimal TotalCostOverall { get; set; }
        public decimal CostThisMonth { get; set; }
        public decimal CostThisQuarter { get; set; }
        public int TotalLogsCount { get; set; }
        public int PendingLogsCount { get; set; }
        public int CompletedLogsCount { get; set; }
        public List<MonthlyCostItem> MonthlyCosts { get; set; } = new();
    }

    public class MonthlyCostItem
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int Count { get; set; }
    }

    // ==========================================
    // 4. DTOs CHO QUẢN LÝ LỚP HỌC (GYM CLASSES)
    // ==========================================
    public class GymClassFormViewModel
    {
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên lớp học")]
        [MaxLength(150, ErrorMessage = "Tên lớp tối đa 150 ký tự")]
        [Display(Name = "Tên lớp học nhóm")]
        public string ClassName { get; set; } = string.Empty;

        [Display(Name = "Mô tả / Giáo trình lớp")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn huấn luyện viên")]
        [Display(Name = "Huấn luyện viên đứng lớp")]
        public int TrainerId { get; set; }

        [Display(Name = "Phòng học / Studio")]
        public int? RoomId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lịch học")]
        [MaxLength(100)]
        [Display(Name = "Lịch học (Thứ & Khung giờ)")]
        public string Schedule { get; set; } = "Thứ 2 - 4 - 6 (18:00 - 19:30)";

        [Required(ErrorMessage = "Vui lòng nhập giới hạn sĩ số")]
        [Range(1, 200, ErrorMessage = "Sĩ số từ 1 đến 200 người")]
        [Display(Name = "Sĩ số tối đa")]
        public int MaxCapacity { get; set; } = 25;

        [Required]
        [Display(Name = "Trạng thái lớp")]
        public string Status { get; set; } = "Open"; // 'Open', 'Full', 'Completed', 'Cancelled'

        [DataType(DataType.Date)]
        [Display(Name = "Ngày khai giảng")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày bế giảng")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Hình ảnh đại diện")]
        public string? ImageUrl { get; set; }
    }

    // ==========================================
    // 5. DTOs CHO ĐĂNG KÝ & ĐIỂM DANH LỚP
    // ==========================================
    public class ClassBookingCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn lớp học")]
        [Display(Name = "Lớp học")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hội viên")]
        [Display(Name = "Hội viên")]
        public int MemberId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đăng ký")]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Confirmed"; // 'Confirmed', 'Attended', 'Cancelled'

        [Display(Name = "Ghi chú đăng ký")]
        public string? Notes { get; set; }
    }
}
