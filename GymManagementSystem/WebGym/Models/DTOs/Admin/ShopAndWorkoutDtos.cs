using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WebGym.Models.Entities;
using Member = WebGym.Models.Entities.Member;
using Trainer = WebGym.Models.Entities.Trainer;

namespace WebGym.Models.DTOs.Admin
{
    // ==========================================
    // 1. DTOs CHO QUẢN LÝ SẢN PHẨM & KHO HÀNG (PRODUCT)
    // ==========================================
    public class ProductFormViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục sản phẩm")]
        [Display(Name = "Danh mục sản phẩm")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [MaxLength(150, ErrorMessage = "Tên sản phẩm tối đa 150 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, 1000000000, ErrorMessage = "Giá bán không hợp lệ")]
        [Display(Name = "Giá bán (VNĐ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng tồn kho")]
        [Range(0, 100000, ErrorMessage = "Số lượng tồn kho phải >= 0")]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } = 0;

        [Display(Name = "Mô tả sản phẩm")]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Đường dẫn ảnh (URL)")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Tải ảnh từ máy")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Đang kinh doanh")]
        public bool IsActive { get; set; } = true;
    }

    public class ProductFilterViewModel
    {
        public int? CategoryId { get; set; }
        public string? StockStatus { get; set; } // "All", "LowStock", "OutOfStock", "InStock"
        public string? SearchKeyword { get; set; }

        public List<Product> Products { get; set; } = new();
        public List<ProductCategory> Categories { get; set; } = new();

        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; } // < 5
        public int OutOfStockCount { get; set; } // == 0
        public decimal TotalInventoryValue { get; set; }
    }

    public class CategoryFormViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }

    // ==========================================
    // 2. DTOs CHO QUẢY BÁN HÀNG NHANH (POS) & ĐƠN HÀNG (ORDERS)
    // ==========================================
    public class PosCartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal TotalPrice => Price * Quantity;
        public string? ImageUrl { get; set; }
        public int CurrentStock { get; set; }
    }

    public class PosCheckoutDto
    {
        public int? MemberId { get; set; }
        public bool IsWalkInGuest { get; set; } = false;
        public string? GuestName { get; set; }
        public string? GuestPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; } = "Tiền mặt"; // 'Tiền mặt', 'VNPay'

        public string? Notes { get; set; }
        public string CartItemsJson { get; set; } = string.Empty;
    }

    public class PosScreenViewModel
    {
        public List<ProductCategory> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<WebGym.Models.Entities.Member> Members { get; set; } = new();
    }

    public class OrderFilterViewModel
    {
        public string? Status { get; set; } // 'Chờ xác nhận', 'Đã thanh toán', 'Đang giao', 'Hoàn thành', 'Đã hủy'
        public string? PaymentMethod { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SearchKeyword { get; set; }

        public List<Order> Orders { get; set; } = new();

        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CancelledOrders { get; set; }
    }

    // ==========================================
    // 3. DTOs CHO GIÁO ÁN TẬP LUYỆN (WORKOUT PLANS)
    // ==========================================
    public class WorkoutPlanViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string? MemberPhone { get; set; }
        public double? MemberWeight { get; set; }
        public double? MemberHeight { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public string? TrainerSpecialization { get; set; }
        public string Goal { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoutineDetails { get; set; } = string.Empty;
        public string NutritionAdvice { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string PlanSource { get; set; } = "Trainer"; // "AI" hoặc "Trainer"
        public DateTime CreatedAt { get; set; }
    }

    public class WorkoutPlanFilterViewModel
    {
        public string? PlanSource { get; set; } = "All"; // "All", "AI", "Trainer"
        public int? TrainerId { get; set; }
        public int? MemberId { get; set; }
        public string? SearchKeyword { get; set; }

        public List<WorkoutPlanViewModel> Plans { get; set; } = new();
        public List<WebGym.Models.Entities.Trainer> Trainers { get; set; } = new();
        public List<WebGym.Models.Entities.Member> Members { get; set; } = new();

        public int TotalPlans { get; set; }
        public int AiPlansCount { get; set; }
        public int TrainerPlansCount { get; set; }
        public int ActivePlansCount { get; set; }
    }

    public class WorkoutPlanFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên giáo án / lộ trình")]
        [MaxLength(150)]
        [Display(Name = "Tên lộ trình tập")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn hội viên")]
        [Display(Name = "Hội viên áp dụng")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Huấn luyện viên phụ trách")]
        [Display(Name = "HLV phụ trách")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mục tiêu tập luyện")]
        [MaxLength(200)]
        [Display(Name = "Mục tiêu (Goal)")]
        public string Goal { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(3);

        [Required(ErrorMessage = "Vui lòng nhập chi tiết lịch tập & bài tập theo ngày")]
        [Display(Name = "Lịch tập & Bài tập chi tiết")]
        public string RoutineDetails { get; set; } = string.Empty;

        [Display(Name = "Tư vấn dinh dưỡng & Calo")]
        public string? NutritionAdvice { get; set; }

        [Display(Name = "Nguồn giáo án")]
        public string PlanSource { get; set; } = "Trainer"; // "AI" hoặc "Trainer"

        [Display(Name = "Kích hoạt giáo án")]
        public bool IsActive { get; set; } = true;
    }
}
