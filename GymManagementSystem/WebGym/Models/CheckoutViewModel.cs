using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebGym.Models
{
    public class CheckoutViewModel
    {
        [BindNever] 
        public PlanInfo? Plan { get; set; }

        public int PackageId { get; set; }

        public string Cycle { get; set; } = "month";   // month | year

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(80, ErrorMessage = "Họ tên tối đa 80 ký tự")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0|\+84)\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        public DateTime? StartDate { get; set; } = DateTime.Today;

        public string? AreaSlug { get; set; }
        public string? TrainerSlug { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán")]
        public string PaymentMethod { get; set; } = "vnpay";   // vnpay | bank | counter

        [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn cần đồng ý với điều khoản sử dụng")]
        public bool AgreeTerms { get; set; }

        // ----- Tính tiền (server là nguồn tin cậy) -----
        public bool IsYearly => Cycle == "year";
        public int Months => IsYearly ? 12 : 1;
        public decimal BaseTotal => (Plan?.MonthlyPrice ?? 0) * Months;
        public decimal Total => IsYearly ? Math.Round((Plan?.MonthlyPrice ?? 0) * 0.8m) * 12 : BaseTotal;
        public decimal Discount => BaseTotal - Total;

        public static string Vnd(decimal v) => v.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + "đ";
    }
}
