using System.Security.Cryptography;
using System.Text;
using WebGym.Models.DTOs.Payment;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    public class VnPayPaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VnPayPaymentService> _logger;

        public VnPayPaymentService(IConfiguration configuration, ILogger<VnPayPaymentService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task<PaymentResponseDto> CreatePaymentUrlAsync(PaymentRequestDto request, HttpContext httpContext)
        {
            try
            {
                // Boilerplate: Configured via appsettings.json: "VnPay:TmnCode", "VnPay:HashSecret", etc.
                var tmnCode = _configuration["VnPay:TmnCode"] ?? "GYMTEST1";
                var hashSecret = _configuration["VnPay:HashSecret"] ?? "SECRET_HASH_KEY_SAMPLE";
                var baseUrl = _configuration["VnPay:BaseUrl"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                var returnUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Payment/VnPayCallback";

                var orderId = $"ORDER_{DateTime.UtcNow.Ticks}";
                var amountInVnd = ((long)request.Amount * 100).ToString(); // VNPay requires amount * 100

                // Standard VNPay parameters
                var queryParams = new SortedDictionary<string, string>
                {
                    { "vnp_Version", "2.1.0" },
                    { "vnp_Command", "pay" },
                    { "vnp_TmnCode", tmnCode },
                    { "vnp_Amount", amountInVnd },
                    { "vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss") },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_IpAddr", httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
                    { "vnp_Locale", "vn" },
                    { "vnp_OrderInfo", $"Thanh toan goi {request.PackageName}" },
                    { "vnp_OrderType", "other" },
                    { "vnp_ReturnUrl", returnUrl },
                    { "vnp_TxnRef", orderId }
                };

                var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
                var secureHash = HmacSHA512(hashSecret, queryString);
                var paymentUrl = $"{baseUrl}?{queryString}&vnp_SecureHash={secureHash}";

                return Task.FromResult(new PaymentResponseDto
                {
                    Success = true,
                    PaymentUrl = paymentUrl,
                    OrderId = orderId,
                    Message = "Tạo link thanh toán VNPay thành công"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo URL VNPay");
                return Task.FromResult(new PaymentResponseDto
                {
                    Success = false,
                    Message = "Không thể tạo liên kết thanh toán: " + ex.Message
                });
            }
        }

        public Task<bool> ProcessCallbackAsync(VnPayCallbackDto callbackData)
        {
            // VNPay ResponseCode "00" = Giao dịch thành công
            if (callbackData.vnp_ResponseCode == "00")
            {
                _logger.LogInformation("VNPay transaction success: {TxnRef}", callbackData.vnp_TxnRef);
                return Task.FromResult(true);
            }

            _logger.LogWarning("VNPay transaction failed with code: {Code}", callbackData.vnp_ResponseCode);
            return Task.FromResult(false);
        }

        private static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using var hmac = new HMACSHA512(keyBytes);
            var hashValue = hmac.ComputeHash(inputBytes);
            foreach (var b in hashValue)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
