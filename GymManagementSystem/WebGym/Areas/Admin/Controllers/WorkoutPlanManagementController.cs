using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkoutPlanManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlanManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/WorkoutPlanManagement
        public async Task<IActionResult> Index(
            [FromQuery(Name = "planSourceFilter")] string? planSourceFilter,
            [FromQuery(Name = "trainerFilter")] int? trainerFilter,
            [FromQuery(Name = "memberFilter")] int? memberFilter,
            [FromQuery(Name = "keyword")] string? keyword)
        {
            var query = _context.WorkoutPlans
                .Include(p => p.Member).ThenInclude(m => m.User)
                .Include(p => p.Trainer).ThenInclude(t => t.User)
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (trainerFilter.HasValue && trainerFilter.Value > 0)
            {
                query = query.Where(p => p.TrainerId == trainerFilter.Value);
            }

            if (memberFilter.HasValue && memberFilter.Value > 0)
            {
                query = query.Where(p => p.MemberId == memberFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(kw) ||
                                         p.Goal.ToLower().Contains(kw) ||
                                         p.Member.User.FullName.ToLower().Contains(kw) ||
                                         p.Trainer.User.FullName.ToLower().Contains(kw));
            }

            var plansList = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

            // Ánh xạ sang ViewModel kèm phân loại PlanSource (AI vs Trainer)
            var viewModels = plansList.Select(p =>
            {
                bool isAi = p.Title.Contains("AI", StringComparison.OrdinalIgnoreCase) ||
                            p.Goal.Contains("AI", StringComparison.OrdinalIgnoreCase) ||
                            (!string.IsNullOrEmpty(p.RoutineDetails) && p.RoutineDetails.Contains("AI", StringComparison.OrdinalIgnoreCase));

                return new WorkoutPlanViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    MemberId = p.MemberId,
                    MemberName = p.Member.User.FullName,
                    MemberPhone = p.Member.User.PhoneNumber,
                    MemberWeight = p.Member.Weight,
                    MemberHeight = p.Member.Height,
                    TrainerId = p.TrainerId,
                    TrainerName = p.Trainer.User.FullName,
                    TrainerSpecialization = p.Trainer.Specialization,
                    Goal = p.Goal,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    RoutineDetails = p.RoutineDetails,
                    NutritionAdvice = p.NutritionAdvice,
                    IsActive = p.IsActive,
                    PlanSource = isAi ? "AI" : "Trainer",
                    CreatedAt = p.CreatedAt
                };
            }).ToList();

            // Áp dụng bộ lọc PlanSource nếu có
            if (!string.IsNullOrWhiteSpace(planSourceFilter) && planSourceFilter != "All")
            {
                viewModels = viewModels.Where(p => p.PlanSource.Equals(planSourceFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var allActivePlans = await _context.WorkoutPlans.Where(p => !p.IsDeleted).ToListAsync();
            int totalAi = allActivePlans.Count(p => p.Title.Contains("AI") || p.Goal.Contains("AI") || (p.RoutineDetails != null && p.RoutineDetails.Contains("AI")));
            int totalTrainer = allActivePlans.Count - totalAi;

            var trainers = await _context.Trainers.Include(t => t.User).OrderBy(t => t.User.FullName).ToListAsync();
            var members = await _context.Members.Include(m => m.User).OrderBy(m => m.User.FullName).ToListAsync();

            var resultModel = new WorkoutPlanFilterViewModel
            {
                PlanSource = planSourceFilter ?? "All",
                TrainerId = trainerFilter,
                MemberId = memberFilter,
                SearchKeyword = keyword,
                Plans = viewModels,
                Trainers = trainers,
                Members = members,
                TotalPlans = allActivePlans.Count,
                AiPlansCount = totalAi,
                TrainerPlansCount = totalTrainer,
                ActivePlansCount = allActivePlans.Count(p => p.IsActive)
            };

            return View(resultModel);
        }

        // GET: /Admin/WorkoutPlanManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var p = await _context.WorkoutPlans
                .Include(w => w.Member).ThenInclude(m => m.User)
                .Include(w => w.Trainer).ThenInclude(t => t.User)
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);

            if (p == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy giáo án tập luyện yêu cầu.";
                return RedirectToAction(nameof(Index));
            }

            bool isAi = p.Title.Contains("AI", StringComparison.OrdinalIgnoreCase) ||
                        p.Goal.Contains("AI", StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrEmpty(p.RoutineDetails) && p.RoutineDetails.Contains("AI", StringComparison.OrdinalIgnoreCase));

            var viewModel = new WorkoutPlanViewModel
            {
                Id = p.Id,
                Title = p.Title,
                MemberId = p.MemberId,
                MemberName = p.Member.User.FullName,
                MemberPhone = p.Member.User.PhoneNumber,
                MemberWeight = p.Member.Weight,
                MemberHeight = p.Member.Height,
                TrainerId = p.TrainerId,
                TrainerName = p.Trainer.User.FullName,
                TrainerSpecialization = p.Trainer.Specialization,
                Goal = p.Goal,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                RoutineDetails = p.RoutineDetails,
                NutritionAdvice = p.NutritionAdvice,
                IsActive = p.IsActive,
                PlanSource = isAi ? "AI" : "Trainer",
                CreatedAt = p.CreatedAt
            };

            ViewBag.MemberEntity = p.Member;
            return View(viewModel);
        }

        // GET: /Admin/WorkoutPlanManagement/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownLists();
            return View(new WorkoutPlanFormViewModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3),
                IsActive = true,
                PlanSource = "Trainer"
            });
        }

        // POST: /Admin/WorkoutPlanManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutPlanFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                string title = model.Title.Trim();
                if (model.PlanSource == "AI" && !title.StartsWith("[AI]", StringComparison.OrdinalIgnoreCase))
                {
                    title = $"[AI] {title}";
                }

                var plan = new WorkoutPlan
                {
                    Title = title,
                    MemberId = model.MemberId,
                    TrainerId = model.TrainerId,
                    Goal = model.Goal.Trim(),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    RoutineDetails = model.RoutineDetails.Trim(),
                    NutritionAdvice = model.NutritionAdvice?.Trim() ?? string.Empty,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.WorkoutPlans.Add(plan);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã tạo giáo án '{plan.Title}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDownLists(model.MemberId, model.TrainerId);
            return View(model);
        }

        // GET: /Admin/WorkoutPlanManagement/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan == null || plan.IsDeleted)
            {
                TempData["ErrorMessage"] = "Không tìm thấy giáo án cần sửa.";
                return RedirectToAction(nameof(Index));
            }

            bool isAi = plan.Title.Contains("AI", StringComparison.OrdinalIgnoreCase) ||
                        plan.Goal.Contains("AI", StringComparison.OrdinalIgnoreCase);

            var model = new WorkoutPlanFormViewModel
            {
                Id = plan.Id,
                Title = plan.Title,
                MemberId = plan.MemberId,
                TrainerId = plan.TrainerId,
                Goal = plan.Goal,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                RoutineDetails = plan.RoutineDetails,
                NutritionAdvice = plan.NutritionAdvice,
                PlanSource = isAi ? "AI" : "Trainer",
                IsActive = plan.IsActive
            };

            await PopulateDropDownLists(plan.MemberId, plan.TrainerId);
            return View(model);
        }

        // POST: /Admin/WorkoutPlanManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkoutPlanFormViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var plan = await _context.WorkoutPlans.FindAsync(id);
                if (plan == null || plan.IsDeleted)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy giáo án.";
                    return RedirectToAction(nameof(Index));
                }

                string title = model.Title.Trim();
                if (model.PlanSource == "AI" && !title.StartsWith("[AI]", StringComparison.OrdinalIgnoreCase))
                {
                    title = $"[AI] {title}";
                }
                else if (model.PlanSource == "Trainer" && title.StartsWith("[AI]", StringComparison.OrdinalIgnoreCase))
                {
                    title = title.Replace("[AI]", "").Trim();
                }

                plan.Title = title;
                plan.MemberId = model.MemberId;
                plan.TrainerId = model.TrainerId;
                plan.Goal = model.Goal.Trim();
                plan.StartDate = model.StartDate;
                plan.EndDate = model.EndDate;
                plan.RoutineDetails = model.RoutineDetails.Trim();
                plan.NutritionAdvice = model.NutritionAdvice?.Trim() ?? string.Empty;
                plan.IsActive = model.IsActive;
                plan.UpdatedAt = DateTime.UtcNow;

                _context.Update(plan);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã cập nhật giáo án '{plan.Title}' thành công!";
                return RedirectToAction(nameof(Details), new { id });
            }

            await PopulateDropDownLists(model.MemberId, model.TrainerId);
            return View(model);
        }

        // POST: /Admin/WorkoutPlanManagement/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan != null)
            {
                plan.IsActive = !plan.IsActive;
                plan.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã cập nhật trạng thái của giáo án '{plan.Title}'.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/WorkoutPlanManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan != null)
            {
                plan.IsDeleted = true;
                plan.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã lưu trữ / xóa giáo án '{plan.Title}' thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helpers
        private async Task PopulateDropDownLists(object? selectedMember = null, object? selectedTrainer = null)
        {
            var members = await _context.Members
                .Include(m => m.User)
                .OrderBy(m => m.User.FullName)
                .Select(m => new { m.MemberId, DisplayName = m.User.FullName + " (" + (m.User.PhoneNumber ?? "No phone") + ")" })
                .ToListAsync();

            var trainers = await _context.Trainers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new { t.TrainerId, DisplayName = t.User.FullName + " - " + t.Specialization })
                .ToListAsync();

            ViewBag.MembersList = new SelectList(members, "MemberId", "DisplayName", selectedMember);
            ViewBag.TrainersList = new SelectList(trainers, "TrainerId", "DisplayName", selectedTrainer);
        }
    }
}

