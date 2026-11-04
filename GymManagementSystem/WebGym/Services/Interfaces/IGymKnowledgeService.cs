namespace WebGym.Services.Interfaces
{
    /// <summary>
    /// Cung cấp "kiến thức" về phòng tập (gói tập, HLV, khu vực...) lấy từ database
    /// để chatbot trả lời chính xác thay vì bịa số liệu.
    /// </summary>
    public interface IGymKnowledgeService
    {
        /// <summary>Đoạn văn bản ngữ cảnh đưa vào system prompt của LLM.</summary>
        Task<string> BuildContextAsync();

        /// <summary>
        /// Trả lời không cần LLM (dùng khi chưa có API key hoặc API lỗi).
        /// Luôn trả về một câu trả lời (câu hỏi lạ → hướng dẫn những gì bot có thể giúp).
        /// </summary>
        Task<(string Reply, List<string> Suggestions)> AnswerOfflineAsync(string message);
    }
}
