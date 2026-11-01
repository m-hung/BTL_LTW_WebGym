using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.Auth
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
    }
}
