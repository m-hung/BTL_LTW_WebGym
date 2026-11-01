using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.AI
{
    public class ChatbotRequestDto
    {
        [Required(ErrorMessage = "Vui lòng nhập câu hỏi")]
        public string Message { get; set; } = string.Empty;

        public double? HeightCm { get; set; }
        public double? WeightKg { get; set; }
        public string? Goal { get; set; } // Giảm cân, tăng cơ, cải thiện sức bền...
    }

    public class ChatbotResponseDto
    {
        public bool Success { get; set; }
        public string Reply { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
