using Microsoft.AspNetCore.Mvc;
using WebGym.Models.DTOs.AI;
using WebGym.Services.Interfaces;

namespace WebGym.Controllers
{
    public class AIController : BaseController
    {
        private readonly IAIService _aiService;
        private readonly ILogger<AIController> _logger;

        public AIController(IAIService aiService, ILogger<AIController> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        // GET: /AI
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /AI/AskAssistant
        [HttpPost]
        public async Task<IActionResult> AskAssistant([FromBody] ChatbotRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
            {
                return Json(new ChatbotResponseDto
                {
                    Success = false,
                    ErrorMessage = "Vui lòng nhập nội dung câu hỏi."
                });
            }

            var result = await _aiService.GetGymAdviceAsync(request);
            return Json(result);
        }
    }
}
