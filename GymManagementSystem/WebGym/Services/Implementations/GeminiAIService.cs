using System.Text;
using System.Text.Json;
using WebGym.Models.DTOs.AI;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    public class GeminiAIService : IAIService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GeminiAIService> _logger;

        public GeminiAIService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<GeminiAIService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ChatbotResponseDto> GetGymAdviceAsync(ChatbotRequestDto request)
        {
            var apiKey = _configuration["AI:ApiKey"] ?? string.Empty;

            // Context prompt builder
            var systemPrompt = "Bạn là HLV cá nhân và chuyên gia dinh dưỡng thể hình thông minh tại hệ thống WebGym. " +
                               "Hãy đưa ra lời khuyên ngắn gọn, nhiệt tình, khoa học về bài tập, lịch tập và chế độ ăn uống cho học viên. ";

            if (request.HeightCm.HasValue && request.WeightKg.HasValue)
            {
                var bmi = request.WeightKg.Value / Math.Pow(request.HeightCm.Value / 100, 2);
                systemPrompt += $"Thông số học viên: Chiều cao {request.HeightCm}cm, Cân nặng {request.WeightKg}kg, Chỉ số BMI: {bmi:F1}. Mục tiêu: {request.Goal ?? "Cải thiện vóc dáng"}.";
            }

            // Fallback mock answer if API key is not yet set
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "YOUR_GEMINI_OR_OPENAI_API_KEY")
            {
                return new ChatbotResponseDto
                {
                    Success = true,
                    Reply = $"[AI Trợ Lý Gym]: Chào bạn! Với mục tiêu '{request.Goal ?? "tập luyện"}', tôi gợi ý bạn nên phân bổ 3-4 buổi/tuần: " +
                            $"kết hợp các bài tập đa khớp (Squat, Deadlift, Bench Press) và bổ sung 1.6g - 2.0g protein/kg cân nặng mỗi ngày. " +
                            $"(Ghi chú: Vui lòng cấu hình AI:ApiKey trong appsettings.json để kết nối API trực tiếp)."
                };
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = $"{systemPrompt}\n\nNgười dùng hỏi: {request.Message}" }
                            }
                        }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorStr = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("AI API returned error: {Error}", errorStr);
                    return new ChatbotResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Hệ thống AI đang bảo trì. Vui lòng thử lại sau!",
                        Reply = "Xin lỗi bạn, trợ lý AI tạm thời gián đoạn kết nối."
                    };
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseBody);
                var candidateText = jsonDoc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString() ?? "Không có phản hồi.";

                return new ChatbotResponseDto
                {
                    Success = true,
                    Reply = candidateText
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gọi AI Service");
                return new ChatbotResponseDto
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Reply = "Đã xảy ra sự cố khi xử lý câu hỏi. Vui lòng thử lại!"
                };
            }
        }
    }
}
