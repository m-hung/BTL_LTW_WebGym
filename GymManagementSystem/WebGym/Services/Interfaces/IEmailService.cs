namespace WebGym.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string bodyHtml);
        Task<bool> SendPaymentConfirmationAsync(string toEmail, string customerName, string packageName, decimal amount, string transactionId);
        Task<bool> SendBookingConfirmationAsync(string toEmail, string customerName, string trainerName, DateTime bookingDate, TimeSpan startTime);
        Task<bool> SendPasswordResetAsync(string toEmail, string customerName, string resetLink, int validMinutes);
    }
}
