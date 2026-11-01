using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
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
            try
            {
                // Boilerplate: Configured via Smtp or SendGrid in appsettings.json
                _logger.LogInformation("Gửi Email đến {ToEmail}: Tiêu đề: {Subject}", toEmail, subject);
                
                // TODO (Thành viên TH): Kết nối SmtpClient hoặc MailKit gửi thật
                await Task.Delay(100); // Simulate network latency
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
                    <p>Xin chào <strong>{customerName}</strong>,</p>
                    <p>Giao dịch của bạn đã được ghi nhận thành công với các thông tin sau:</p>
                    <ul>
                        <li><strong>Gói tập:</strong> {packageName}</li>
                        <li><strong>Số tiền:</strong> {amount:N0} VNĐ</li>
                        <li><strong>Mã giao dịch:</strong> {transactionId}</li>
                        <li><strong>Thời gian:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                    </ul>
                    <p>Chúc bạn có những trải nghiệm tập luyện tuyệt vời!</p>
                </div>";

            return SendEmailAsync(toEmail, subject, body);
        }

        public Task<bool> SendBookingConfirmationAsync(string toEmail, string customerName, string trainerName, DateTime bookingDate, TimeSpan startTime)
        {
            var subject = $"[WebGym] Lịch tập cùng HLV {trainerName} đã được xác nhận";
            var body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2 style='color: #007bff;'>Lịch hẹn tập luyện thành công</h2>
                    <p>Xin chào <strong>{customerName}</strong>,</p>
                    <p>Bạn có lịch tập cùng Huấn Luyện Viên <strong>{trainerName}</strong> vào ngày <strong>{bookingDate:dd/MM/yyyy}</strong> lúc <strong>{startTime:hh\\:mm}</strong>.</p>
                    <p>Vui lòng đến đúng giờ để buổi tập đạt hiệu quả cao nhất!</p>
                </div>";

            return SendEmailAsync(toEmail, subject, body);
        }
    }
}
