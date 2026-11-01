using WebGym.Models.DTOs.Payment;

namespace WebGym.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentUrlAsync(PaymentRequestDto request, HttpContext httpContext);
        Task<bool> ProcessCallbackAsync(VnPayCallbackDto callbackData);
    }
}
