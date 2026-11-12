using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.AiPlan;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Areas.PT.Controllers
{
    /// <summary>
    /// HLV xem và duyệt giáo án AI của học viên (gói có kèm PT). HLV được sửa số hiệp, số lần, thời gian nghỉ, ghi chú
    /// rồi bấm "Đã duyệt": giáo án được gán HLV, học viên nhận thông báo.
    /// Chỉ thấy giáo án của học viên từng có ca tập đã duyệt / hoàn thành với mình.
    /// </summary>
    public class PlanReviewController : PTBaseController
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };

        private readonly ApplicationDbContext _context;
        private readonly IAiPlanService _planService;

        public PlanReviewController(ApplicationDbContext context, IAiPlanService planService)
        {
            _context = context;
            _planService = planService;
        }

        public class ReviewItem
        {
            public int Weekday { get; set; }
            public int ExerciseId { get; set; }
            public int Sets { get; set; }
            public string? Reps { get; set; }
            public int RestSec { get; set; }
            public string? Note { get; set; }
        }

        // GET: /PT/PlanReview
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trainer = await CurrentTrainerAsync();
            if (trainer == null) return RedirectToAction("Index", "Schedule");

            var plans = await PendingPlansQuery(trainer.TrainerId)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .OrderBy(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
            return View(plans);
        }

        // GET: /PT/PlanReview/Review/5
        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var trainer = await CurrentTrainerAsync();
            if (trainer == null) return RedirectToAction("Index", "Schedule");

            var plan = await PendingPlansQuery(trainer.TrainerId)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (plan == null || !TryReadDoc(plan.PlanJson, out var doc))
            {
                SetErrorMessage("Không tìm thấy giáo án cần duyệt, có thể đã được duyệt rồi.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Plan = plan;
            ViewBag.Survey = TryReadSurvey(plan.InputJson);
            return View(new PlanViewModel
            {
                Doc = doc,
                Exercises = await _planService.GetExerciseMapAsync(),
                PlanId = plan.Id,
                Status = plan.Status,
                Source = plan.Source,
                CreatedAt = plan.CreatedAt
            });
        }

        // POST: /PT/PlanReview/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, List<ReviewItem>? items, string? message)
        {
            var trainer = await CurrentTrainerAsync();
            if (trainer == null) return RedirectToAction("Index", "Schedule");

            var plan = await PendingPlansQuery(trainer.TrainerId)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (plan == null || !TryReadDoc(plan.PlanJson, out var doc))
            {
                SetErrorMessage("Không tìm thấy giáo án cần duyệt, có thể đã được duyệt rồi.");
                return RedirectToAction(nameof(Index));
            }

            // Áp dụng chỉnh sửa của HLV (chỉ các bài đã có trong giáo án)
            foreach (var item in items ?? new List<ReviewItem>())
            {
                var ex = doc.Days.FirstOrDefault(d => d.Weekday == item.Weekday)?.Exercises.FirstOrDefault(x => x.ExerciseId == item.ExerciseId);
                if (ex == null) continue;
                ex.Sets = Math.Clamp(item.Sets, 1, 8);
                if (!string.IsNullOrWhiteSpace(item.Reps)) ex.Reps = item.Reps.Trim().Length > 20 ? item.Reps.Trim()[..20] : item.Reps.Trim();
                ex.RestSec = Math.Clamp(item.RestSec, 0, 300);
                ex.Note = string.IsNullOrWhiteSpace(item.Note) ? null : (item.Note.Trim().Length > 140 ? item.Note.Trim()[..140] : item.Note.Trim());
            }

            var exercises = await _planService.GetExerciseMapAsync();
            plan.PlanJson = JsonSerializer.Serialize(doc, JsonOptions);
            plan.RoutineDetails = _planService.BuildRoutineText(doc, exercises);
            plan.TrainerId = trainer.TrainerId;
            plan.Status = WorkoutPlanStatuses.Active;
            plan.UpdatedAt = DateTime.UtcNow;

            var note = string.IsNullOrWhiteSpace(message) ? null : (message.Trim().Length > 500 ? message.Trim()[..500] : message.Trim());
            _context.Notifications.Add(new Notification
            {
                UserId = plan.Member.UserId,
                Title = "HLV đã duyệt giáo án của bạn",
                Message = $"HLV {trainer.User.FullName} đã xem và duyệt giáo án \"{plan.Title}\"." + (note != null ? $" Lời nhắn: {note}" : string.Empty),
                Type = "System",
                TargetUrl = "/Member/Plan",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            SetSuccessMessage($"Đã duyệt giáo án của {plan.Member.User.FullName}. Học viên sẽ nhận được thông báo.");
            return RedirectToAction(nameof(Index));
        }

        // ================== Hàm hỗ trợ ==================

        private Task<Trainer?> CurrentTrainerAsync()
        {
            var userId = CurrentUserId;
            return _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.UserId == userId);
        }

        /// <summary>Giáo án AI đang chờ duyệt của học viên từng có ca tập đã duyệt / hoàn thành với HLV này.</summary>
        private IQueryable<WorkoutPlan> PendingPlansQuery(int trainerId) =>
            _context.WorkoutPlans.Where(p =>
                p.Status == WorkoutPlanStatuses.PendingReview && p.IsActive && !p.IsDeleted &&
                _context.Bookings.Any(b => b.MemberId == p.MemberId && b.TrainerId == trainerId &&
                                           (b.Status == "Approved" || b.Status == "Completed")));

        private static bool TryReadDoc(string? json, out PlanDocument doc)
        {
            doc = new PlanDocument();
            if (string.IsNullOrWhiteSpace(json)) return false;
            try
            {
                var parsed = JsonSerializer.Deserialize<PlanDocument>(json, JsonOptions);
                if (parsed == null) return false;
                doc = parsed;
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private static PlanSurveyDto? TryReadSurvey(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try { return JsonSerializer.Deserialize<PlanSurveyDto>(json, JsonOptions); }
            catch (JsonException) { return null; }
        }
    }
}
