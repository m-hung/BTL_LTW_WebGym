using System.Collections.Concurrent;
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

        // Thời gian chờ: mỗi lần gọi một model và tổng cả chuỗi (hết thì trả lời bằng dữ liệu có sẵn)
        private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(12);
        private static readonly TimeSpan TotalBudget = TimeSpan.FromSeconds(20);

        // Ghi nhớ model đang lỗi để các tin nhắn sau không phải chờ thử lại nó:
        // quá tải / giới hạn tốc độ / quá thời gian → bỏ qua 2 phút; model không tồn tại → bỏ qua 1 ngày.
        private static readonly ConcurrentDictionary<string, DateTime> SkipModelUntil = new(StringComparer.OrdinalIgnoreCase);
        // Model không nhận tuỳ chọn thinkingConfig → các lần sau gửi không kèm tuỳ chọn này
        private static readonly ConcurrentDictionary<string, bool> NoThinkingConfig = new(StringComparer.OrdinalIgnoreCase);

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

            // Mức "suy nghĩ" của Gemini 3: càng thấp trả lời càng nhanh. AI:ThinkingLevel = minimal | low | medium | high | none
            var thinkingLevel = (_configuration["AI:ThinkingLevel"] ?? "low").Trim().ToLowerInvariant();
            var sendThinking = thinkingLevel is "minimal" or "low" or "medium" or "high";

            string BuildJson(bool withThinking)
            {
                object generationConfig = withThinking
                    ? new { temperature = 0.6, maxOutputTokens = 1500, thinkingConfig = new { thinkingLevel } }
                    : new { temperature = 0.6, maxOutputTokens = 1500 };
                return JsonSerializer.Serialize(new
                {
                    systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
                    contents,
                    generationConfig
                });
            }

            var client = _httpClientFactory.CreateClient();
            client.Timeout = Timeout.InfiniteTimeSpan; // thời gian chờ do AttemptTimeout / TotalBudget quản lý

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

            // Bỏ qua model vừa lỗi gần đây; nếu tất cả đều đang bị bỏ qua thì vẫn thử lại cả danh sách
            var now = DateTime.UtcNow;
            var ready = models.Where(m => !SkipModelUntil.TryGetValue(m, out var until) || until <= now).ToList();
            if (ready.Count == 0) ready = models;

            using var budget = new CancellationTokenSource(TotalBudget);

            string? body = null;
            foreach (var m in ready)
            {
                var withThinking = sendThinking && !NoThinkingConfig.ContainsKey(m);
                var (status, text) = await SendOnceAsync(client, apiKey, m, BuildJson(withThinking), budget.Token);

                // Model không nhận thinkingConfig → gửi lại ngay không kèm tuỳ chọn đó
                if (status == 400 && withThinking && text.Contains("thinking", StringComparison.OrdinalIgnoreCase))
                {
                    NoThinkingConfig[m] = true;
                    _logger.LogInformation("Model {Model} không hỗ trợ thinkingConfig, gửi lại không kèm tuỳ chọn này", m);
                    (status, text) = await SendOnceAsync(client, apiKey, m, BuildJson(false), budget.Token);
                }

                if (status is >= 200 and < 300)
                {
                    SkipModelUntil.TryRemove(m, out _);
                    if (!string.Equals(m, model, StringComparison.OrdinalIgnoreCase))
                        _logger.LogInformation("AI trả lời bằng model dự phòng {Model}", m);
                    body = text;
                    break;
                }

                if (budget.IsCancellationRequested)
                {
                    _logger.LogWarning("AI hết {Seconds} giây chờ, chuyển sang câu trả lời có sẵn", TotalBudget.TotalSeconds);
                    return null;
                }

                if (status == 0)
                {
                    _logger.LogWarning("AI model {Model} quá {Seconds} giây không phản hồi", m, AttemptTimeout.TotalSeconds);
                    SkipModelUntil[m] = DateTime.UtcNow.AddMinutes(2);
                    continue;
                }

                _logger.LogWarning("AI API model {Model} returned {Status}: {Error}", m, status, text);

                // Quá tải / giới hạn tốc độ / model không có cho key này → thử model kế tiếp; lỗi khác (key sai...) → dừng
                if (status == 404)
                {
                    SkipModelUntil[m] = DateTime.UtcNow.AddDays(1);
                    continue;
                }
                if (status == 503 || status == 429)
                {
                    SkipModelUntil[m] = DateTime.UtcNow.AddMinutes(2);
                    continue;
                }
                return null;
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

        /// <summary>Gọi một model một lần. Trả về (mã HTTP, nội dung); mã 0 nghĩa là quá thời gian chờ.</summary>
        private static async Task<(int Status, string Text)> SendOnceAsync(HttpClient client, string apiKey, string model, string json, CancellationToken budgetToken)
        {
            using var attempt = CancellationTokenSource.CreateLinkedTokenSource(budgetToken);
            attempt.CancelAfter(AttemptTimeout);

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            // Gửi key qua header để không lộ trong URL/log
            httpRequest.Headers.Add("x-goog-api-key", apiKey);

            try
            {
                using var response = await client.SendAsync(httpRequest, attempt.Token);
                var text = await response.Content.ReadAsStringAsync(attempt.Token);
                return ((int)response.StatusCode, text);
            }
            catch (OperationCanceledException)
            {
                return (0, string.Empty);
            }
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
            sb.AppendLine("- Khi nhắc tới gói tập, PT hoặc khu vực, hãy kèm liên kết nội bộ tương ứng có trong dữ liệu. " +
                          "Chỉ dùng đúng các đường dẫn trong mục LIÊN KẾT HỢP LỆ; không tự tạo đường dẫn khác (ví dụ không dùng /Home/Packages).");
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
