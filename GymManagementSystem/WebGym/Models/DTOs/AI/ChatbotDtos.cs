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

        /// <summary>Các lượt hội thoại trước đó (không gồm câu hỏi hiện tại) để chatbot nhớ ngữ cảnh.</summary>
        public List<ChatTurnDto>? History { get; set; }
    }

    public class ChatTurnDto
    {
        /// <summary>"user" hoặc "model" (chấp nhận thêm "bot"/"assistant").</summary>
        public string Role { get; set; } = "user";
        public string Text { get; set; } = string.Empty;
    }

    public class ChatbotResponseDto
    {
        public bool Success { get; set; }
        public string Reply { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>Gợi ý câu hỏi tiếp theo (hiện dạng nút bấm nhanh).</summary>
        public List<string>? Suggestions { get; set; }

        /// <summary>"ai" = trả lời từ mô hình AI, "offline" = trả lời dự phòng từ dữ liệu hệ thống.</summary>
        public string? Source { get; set; }
    }
}
