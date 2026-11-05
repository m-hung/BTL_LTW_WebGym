using System.Text;
using System.Text.Json;
using WebGym.Models.DTOs.AI;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    public class GeminiAIService : IAIService
    {
        private const int MaxMessageLength = 500;
        private const int MaxHistoryTurns = 10;
        private static readonly string[] DefaultFallbackModels = { "gemini-3.7-flash", "gemini-flash-latest", "gemini-flash-lite-latest" };
        private const string DefaultModel = "gemini-3.8-flash";

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GeminiAIService> _logger;
        private readonly IGymKnowledgeService _knowledge;

        public GeminiAIService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<GeminiAIService> logger,
            IGymKnowledgeService knowledge)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _knowledge = knowledge;
        }

        public async Task<ChatbotResponseDto> GetGymAdviceAsync(ChatbotRequestDto request)
        {
            var message = (request.Message ?? string.Empty).Trim();
            if (message.Length > MaxMessageLength) message = message[..MaxMessageLength];

            var apiKey = _configuration["AI:ApiKey"] ?? string.Empty;
            var hasKey = !string.IsNullOrWhiteSpace(apiKey) && apiKey != "YOUR_GEMINI_OR_OPENAI_API_KEY";

            if (hasKey)
            {
                try
                {
                    var reply = await CallGeminiAsync(apiKey, message, request);
                    if (!string.IsNullOrWhiteSpace(reply))
                    {
                        return new ChatbotResponseDto
                        {
                            Success = true,
                            Reply = reply.Trim(),
                            Source = "ai",
                            Suggestions = new List<string> { "Có những gói nào?", "Danh sách HLV", "Giờ mở cửa?" }
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gọi AI Service, chuyển sang trả lời dự phòng");
                }
            }

            // Chưa có API key / API lỗi → vẫn trả lời được từ dữ liệu trong hệ thống
            try
            {
                var (offline, suggestions) = await _knowledge.AnswerOfflineAsync(message);
                return new ChatbotResponseDto { Success = true, Reply = offline, Suggestions = suggestions, Source = "offline" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo câu trả lời dự phòng");
                return new ChatbotResponseDto
                {
                    Success = false,
                    ErrorMessage = "Trợ lý đang gặp sự cố. Vui lòng thử lại sau!",
                    Reply = "Xin lỗi bạn, trợ lý tạm thời gián đoạn. Bạn thử lại sau ít phút nhé!"
                };
            }
        }

        private async Task<string?> CallGeminiAsync(string apiKey, string message, ChatbotRequestDto request)
        {
            // gemini-1.5-* đã ngừng, gemini-2.5-flash không còn mở cho key mới → mặc định dùng gemini-3.8-flash.
            // Muốn dùng model khác: đặt AI:Model (user-secrets hoặc appsettings).
            var model = _configuration["AI:Model"];
            if (string.IsNullOrWhiteSpace(model) || model.StartsWith("gemini-1.5", StringComparison.OrdinalIgnoreCase))
                model = DefaultModel;

            var systemPrompt = await BuildSystemPromptAsync(request);

            // Lịch sử hội thoại: lấy N lượt gần nhất, phải bắt đầu bằng lượt của user
            var contents = new List<object>();
            var history = (request.History ?? new List<ChatTurnDto>())
                .Where(h => !string.IsNullOrWhiteSpace(h.Text))
                .TakeLast(MaxHistoryTurns)
                .Select(h => (Role: NormalizeRole(h.Role), Text: Truncate(h.Text.Trim(), 1000)))
                .SkipWhile(h => h.Role != "user")
                .ToList();

            foreach (var h in history)
            {
                contents.Add(new { role = h.Role, parts = new[] { new { text = h.Text } } });
            }
            contents.Add(new { role = "user", parts = new[] { new { text = message } } });

            var payload = new
            {
                systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                contents,
                generationConfig = new { temperature = 0.6, maxOutputTokens = 1500 }
            };

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(20);
            var json = JsonSerializer.Serialize(payload);

            // Danh sách model thử lần lượt: model chính → các model dự phòng (AI:FallbackModels, cách nhau bởi dấu phẩy)
            var models = new List<string> { model };
            var fbConfig = _configuration["AI:FallbackModels"];
            var fallbacks = string.IsNullOrWhiteSpace(fbConfig)
                ? DefaultFallbackModels
                : fbConfig.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var f in fallbacks)
            {
                if (!models.Contains(f, StringComparer.OrdinalIgnoreCase)) models.Add(f);
            }

            // Tổng thời gian chờ tối đa cho cả chuỗi thử, tránh để khách đợi quá lâu
            using var budget = new CancellationTokenSource(TimeSpan.FromSeconds(35));

            string? body = null;
            foreach (var m in models)
            {
                var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{m}:generateContent";
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                // Gửi key qua header để không lộ trong URL/log
                httpRequest.Headers.Add("x-goog-api-key", apiKey);

                using var response = await client.SendAsync(httpRequest, budget.Token);
                var text = await response.Content.ReadAsStringAsync(budget.Token);

                if (response.IsSuccessStatusCode)
                {
                    if (m != model) _logger.LogInformation("AI trả lời bằng model dự phòng {Model}", m);
                    body = text;
                    break;
                }

                var status = (int)response.StatusCode;
                _logger.LogWarning("AI API model {Model} returned {Status}: {Error}", m, status, text);

                // Quá tải / giới hạn tốc độ / model không có cho key này → thử model kế tiếp; lỗi khác (key sai...) → dừng
                var tryNext = status == 503 || status == 429 || status == 404;
                if (!tryNext) return null;
            }

            if (body == null) return null;

            using var doc = JsonDocument.Parse(body);
            if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                return null;

            if (!candidates[0].TryGetProperty("content", out var content) ||
                !content.TryGetProperty("parts", out var parts))
                return null;

            var sb = new StringBuilder();
            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var t)) sb.Append(t.GetString());
            }
            return sb.ToString();
        }

        private async Task<string> BuildSystemPromptAsync(ChatbotRequestDto request)
        {
            string context;
            try { context = await _knowledge.BuildContextAsync(); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không nạp được dữ liệu phòng tập cho chatbot");
                context = "(Không tải được dữ liệu hệ thống — hãy hướng dẫn khách xem trang Gói tập hoặc hỏi lễ tân.)";
            }

            var sb = new StringBuilder();
            sb.AppendLine("Bạn là **Pulse AI**, trợ lý ảo của phòng tập Pulse Gym (hệ thống WebGym).");
            sb.AppendLine("Nhiệm vụ: giải đáp về gói tập, huấn luyện viên (PT), khu vực tập, giờ mở cửa, cách đăng ký/thanh toán, " +
                          "và đưa lời khuyên cơ bản về tập luyện, dinh dưỡng thể hình.");
            sb.AppendLine();
            sb.AppendLine("QUY TẮC:");
            sb.AppendLine("- Luôn trả lời bằng tiếng Việt, thân thiện, ngắn gọn (tối đa khoảng 120 từ), đi thẳng vào ý chính.");
            sb.AppendLine("- Về giá, tên gói, tên PT, giờ mở cửa: CHỈ dùng dữ liệu bên dưới. Không bịa. Nếu không có thông tin, nói rõ và gợi ý hỏi lễ tân.");
            sb.AppendLine("- Định dạng cho phép: **in đậm**, danh sách gạch đầu dòng bắt đầu bằng \"- \", và liên kết dạng [chữ](/đường-dẫn). Không dùng bảng, tiêu đề (#) hay HTML.");
            sb.AppendLine("- Khi nhắc tới gói tập, PT hoặc khu vực, hãy kèm liên kết nội bộ tương ứng có trong dữ liệu.");
            sb.AppendLine("- Tiền tệ viết dạng 1.300.000đ.");
            sb.AppendLine("- Không chẩn đoán bệnh hay kê thuốc; với vấn đề sức khỏe/chấn thương, khuyên gặp bác sĩ.");
            sb.AppendLine("- Câu hỏi ngoài lĩnh vực phòng tập/thể hình: từ chối lịch sự và quay lại chủ đề của phòng tập.");
            sb.AppendLine();
            sb.AppendLine(context);

            if (request.HeightCm.HasValue && request.WeightKg.HasValue && request.HeightCm.Value > 0)
            {
                var bmi = request.WeightKg.Value / Math.Pow(request.HeightCm.Value / 100, 2);
                sb.AppendLine();
                sb.AppendLine($"Thông số học viên: cao {request.HeightCm}cm, nặng {request.WeightKg}kg, BMI {bmi:F1}. Mục tiêu: {request.Goal ?? "Cải thiện vóc dáng"}.");
            }

            return sb.ToString();
        }

        private static string NormalizeRole(string? role) =>
            string.Equals(role, "user", StringComparison.OrdinalIgnoreCase) ? "user" : "model";

        private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max];
    }
}
