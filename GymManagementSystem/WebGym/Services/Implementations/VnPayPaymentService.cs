using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using WebGym.Models.DTOs.Payment;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }

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
                var tmnCode = _configuration["VnPay:TmnCode"] ?? "G8DV0PLE";
                var hashSecret = _configuration["VnPay:HashSecret"] ?? "GYSWQDUHEOIZGEXBTNETQZHNJDFAQLYS";
                var baseUrl = _configuration["VnPay:BaseUrl"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                var returnUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Payment/VnPayCallback";

                // Múi giờ Việt Nam (GMT+7)
                TimeZoneInfo vnTimeZone;
                try
                {
                    vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                }
                catch
                {
                    vnTimeZone = TimeZoneInfo.CreateCustomTimeZone("VN", TimeSpan.FromHours(7), "Vietnam Standard Time", "Vietnam Standard Time");
                }
                var vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

                var orderId = $"ORDER_{DateTime.UtcNow.Ticks}";
                var amountInVnd = ((long)request.Amount * 100).ToString(); // VNPay yêu cầu nhân 100

                // Danh sách tham số theo chuẩn VNPay, sắp xếp bằng VnPayCompare
                var requestData = new SortedList<string, string>(new VnPayCompare())
                {
                    { "vnp_Version", "2.1.0" },
                    { "vnp_Command", "pay" },
                    { "vnp_TmnCode", tmnCode },
                    { "vnp_Amount", amountInVnd },
                    { "vnp_CreateDate", vnNow.ToString("yyyyMMddHHmmss") },
                    { "vnp_CurrCode", "VND" },
                    { "vnp_IpAddr", GetIpAddress(httpContext) },
                    { "vnp_Locale", "vn" },
                    { "vnp_OrderInfo", $"Thanh toan don hang {orderId}" },
                    { "vnp_OrderType", "other" },
                    { "vnp_ReturnUrl", returnUrl },
                    { "vnp_TxnRef", orderId },
                    { "vnp_ExpireDate", vnNow.AddMinutes(15).ToString("yyyyMMddHHmmss") }
                };

                // Xây dựng chuỗi query string và chuỗi ký (hash) bằng WebUtility.UrlEncode chuẩn VNPay
                var data = new StringBuilder();
                foreach (var kv in requestData)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                    {
                        data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                }

                var queryString = data.ToString();
                if (queryString.EndsWith("&"))
                {
                    queryString = queryString.Substring(0, queryString.Length - 1);
                }

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

        private static string GetIpAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1" || ipAddress.Contains(":"))
            {
                return "127.0.0.1";
            }
            return ipAddress;
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
