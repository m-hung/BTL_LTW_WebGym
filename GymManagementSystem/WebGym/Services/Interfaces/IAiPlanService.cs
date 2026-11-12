using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;

namespace WebGym.Services.Interfaces
{
    public interface IAiPlanService
    {
        /// <summary>Tạo giáo án mới: server tính chỉ số + khung lịch, AI chọn bài và soạn thực đơn, server kiểm tra lại.</summary>
        Task<PlanGenerationResult> GenerateAsync(int memberId, PlanSurveyDto survey, double? inBodyBmr, CancellationToken cancellationToken = default);

        /// <summary>Tạo lại một phần: "all" (toàn bộ), "meals" (chỉ thực đơn) hoặc "day" (một ngày tập).</summary>
        Task<PlanGenerationResult> RegenerateAsync(int memberId, PlanSurveyDto survey, double? inBodyBmr, PlanDocument current,
            string part, int? weekday, string? reason, CancellationToken cancellationToken = default);

        /// <summary>Số lần gọi AI còn lại trong ngày của hội viên.</summary>
        int RemainingToday(int memberId);

        /// <summary>Danh mục bài tập (kèm thiết bị) để hiển thị tên, máy và cách tập.</summary>
        Task<Dictionary<int, Exercise>> GetExerciseMapAsync(CancellationToken cancellationToken = default);

        /// <summary>Sinh bản văn bản của lịch tập / dinh dưỡng để lưu vào RoutineDetails / NutritionAdvice.</summary>
        string BuildRoutineText(PlanDocument doc, IReadOnlyDictionary<int, Exercise> exercises);
        string BuildNutritionText(PlanDocument doc);
    }
}
