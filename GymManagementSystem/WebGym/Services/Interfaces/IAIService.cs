using WebGym.Models.DTOs.AI;

namespace WebGym.Services.Interfaces
{
    public interface IAIService
    {
        Task<ChatbotResponseDto> GetGymAdviceAsync(ChatbotRequestDto request);
    }
}
