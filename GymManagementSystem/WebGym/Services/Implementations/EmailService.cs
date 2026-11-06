using System.Net;
using System.Net.Mail;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    /// <summary>
    /// Gửi email qua SMTP (ví dụ Gmail + App Password). Cấu hình bằng user-secrets:
    ///   Email:SmtpHost, Email:SmtpPort (mặc định 587), Email:SmtpUser, Email:SmtpPassword,
    ///   Email:FromAddress (mặc định = SmtpUser), Email:FromName, Email:EnableSsl (mặc định true)
    /// Chưa cấu hình SmtpHost → chỉ ghi log (giả lập như trước), không báo lỗi.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            var host = _configuration["Email:SmtpHost"];
            if (string.IsNullOrWhiteSpace(host))
            {
                _logger.LogInformation("[Email giả lập] Chưa cấu hình Email:SmtpHost. Gửi đến {ToEmail}: {Subject}", toEmail, subject);
                return false;
            }

            try
            {
                var port = int.TryParse(_configuration["Email:SmtpPort"], out var p) ? p : 587;
                var user = _configuration["Email:SmtpUser"];
                var password = _configuration["Email:SmtpPassword"];
                var from = _configuration["Email:FromAddress"];
                if (string.IsNullOrWhiteSpace(from)) from = user;
                var fromName = _configuration["Email:FromName"] ?? "Pulse Gym";
                var enableSsl = !bool.TryParse(_configuration["Email:EnableSsl"], out var ssl) || ssl;

                if (string.IsNullOrWhiteSpace(from))
                {
                    _logger.LogWarning("Thiếu Email:FromAddress / Email:SmtpUser, không gửi được email đến {ToEmail}", toEmail);
                    return false;
                }

                using var message = new MailMessage
                {
                    From = new MailAddress(from, fromName),
                    Subject = subject,
                    Body = bodyHtml,
                    IsBodyHtml = true
                };
                message.To.Add(toEmail);

                using var client = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 15000
                };
                if (!string.IsNullOrWhiteSpace(user))
                {
                    client.Credentials = new NetworkCredential(user, password);
                }

                await client.SendMailAsync(message);
                _logger.LogInformation("Đã gửi email đến {ToEmail}: {Subject}", toEmail, subject);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi email đến {ToEmail}", toEmail);
                return false;
            }
        }

        public Task<bool> SendPaymentConfirmationAsync(string toEmail, string customerName, string packageName, decimal amount, string transactionId)
        {
            var subject = $"[WebGym] Xác nhận thanh toán thành công - Gói {packageName}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; line-height: 1.6;'>
                    <h2 style='color: #28a745;'>Cảm ơn bạn đã đăng ký gói tập tại WebGym!</h2>
                    <p>Xin chào <strong>{WebUtility.HtmlEncode(customerName)}</strong>,</p>
                    <p>Giao dịch của bạn đã được ghi nhận thành công với các thông tin sau:</p>
                    <ul>
                        <li><strong>Gói tập:</strong> {WebUtility.HtmlEncode(packageName)}</li>
                        <li><strong>Số tiền:</strong> {amount:N0} VNĐ</li>
                        <li><strong>Mã giao dịch:</strong> {WebUtility.HtmlEncode(transactionId)}</li>
                        <li><strong>Thời gian:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                    </ul>
                    <p>Chúc bạn có những trải nghiệm tập luyện tuyệt vời!</p>
                </div>";

            return SendEmailAsync(toEmail, subject, body);
        }

        public Task<bool> SendBookingConfirmationAsync(string toEmail, string customerName, string trainerName, DateTime bookingDate, TimeSpan startTime)
        {
            var subject = $"[WebGym] Lịch tập cùng HLV {trainerName} đã được xác nhận";
            var timeStr = startTime != TimeSpan.Zero ? startTime.ToString(@"hh\:mm") : "khung giờ đã chọn";
            var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; line-height: 1.6;'>
                    <h2 style='color: #007bff;'>Lịch hẹn tập luyện thành công</h2>
                    <p>Xin chào <strong>{WebUtility.HtmlEncode(customerName)}</strong>,</p>
                    <p>Bạn có lịch tập cùng Huấn Luyện Viên <strong>{WebUtility.HtmlEncode(trainerName)}</strong> vào ngày <strong>{bookingDate:dd/MM/yyyy}</strong> lúc <strong>{startTime:hh\\:mm}</strong>.</p>
                    <p>Vui lòng đến đúng giờ để buổi tập đạt hiệu quả cao nhất!</p>
                </div>";

            return SendEmailAsync(toEmail, subject, body);
        }

        public Task<bool> SendPasswordResetAsync(string toEmail, string customerName, string resetLink, int validMinutes)
        {
            var subject = "[Pulse Gym] Đặt lại mật khẩu";
            var safeName = WebUtility.HtmlEncode(customerName);
            var safeLink = WebUtility.HtmlEncode(resetLink);
            var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 24px; line-height: 1.6; color: #1a1a1a; max-width: 520px;'>
                    <h2 style='margin: 0 0 12px;'>Đặt lại mật khẩu</h2>
                    <p>Xin chào <strong>{safeName}</strong>,</p>
                    <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản Pulse Gym của bạn. Bấm nút bên dưới để tạo mật khẩu mới:</p>
                    <p style='margin: 24px 0;'>
                        <a href='{safeLink}' style='background: #c8ff2e; color: #0a0a0a; padding: 12px 22px; text-decoration: none; font-weight: bold; display: inline-block;'>ĐẶT LẠI MẬT KHẨU</a>
                    </p>
                    <p>Liên kết có hiệu lực trong <strong>{validMinutes} phút</strong> và chỉ dùng được một lần.</p>
                    <p style='color: #666; font-size: 13px;'>Nếu bạn không yêu cầu, hãy bỏ qua email này. Mật khẩu của bạn sẽ không thay đổi.</p>
                    <p style='color: #666; font-size: 12px; word-break: break-all;'>Không bấm được nút? Sao chép liên kết này vào trình duyệt: {safeLink}</p>
                </div>";

            return SendEmailAsync(toEmail, subject, body);
        }
    }
}
