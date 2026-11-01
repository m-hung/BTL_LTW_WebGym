using System.ComponentModel.DataAnnotations;

namespace WebGym.Models.DTOs.Trainer
{
    public class TrainerScheduleDto
    {
        public int BookingId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string? MemberPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class RecordProgressDto
    {
        [Required(ErrorMessage = "Vui lòng chọn học viên")]
        public int MemberId { get; set; }
        public string? MemberName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cân nặng")]
        public double Weight { get; set; }

        public double? BodyFatPercentage { get; set; }
        public double? MuscleMass { get; set; }
        public double? Chest { get; set; }
        public double? Waist { get; set; }
        public double? Hips { get; set; }

        public string? Notes { get; set; }
    }

    public class WorkoutPlanDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        public string RoutineDetails { get; set; } = string.Empty;
        public string NutritionAdvice { get; set; } = string.Empty;
    }
}
