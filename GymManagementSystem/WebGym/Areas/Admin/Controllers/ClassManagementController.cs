using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class ClassManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ClassManagementController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Admin/ClassManagement
        public async Task<IActionResult> Index(
            string? searchKeyword, 
            string? status, 
            int? trainerId, 
            int? roomId)
        {
            var query = _context.GymClasses
                .Include(c => c.Trainer)
                    .ThenInclude(t => t.User)
                .Include(c => c.Room)
                .Include(c => c.ClassBookings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(c => c.ClassName.ToLower().Contains(kw) ||
                                         (c.Description != null && c.Description.ToLower().Contains(kw)) ||
                                         c.Schedule.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            if (trainerId.HasValue && trainerId.Value > 0)
            {
                query = query.Where(c => c.TrainerId == trainerId.Value);
            }

            if (roomId.HasValue && roomId.Value > 0)
            {
                query = query.Where(c => c.RoomId == roomId.Value);
            }

            var classes = await query.OrderByDescending(c => c.ClassId).ToListAsync();

            // Cập nhật lại CurrentCapacity chính xác theo số booking active
            foreach (var item in classes)
            {
                int activeBookings = item.ClassBookings.Count(b => b.Status != "Cancelled");
                if (item.CurrentCapacity != activeBookings)
                {
                    item.CurrentCapacity = activeBookings;
                    if (item.CurrentCapacity >= item.MaxCapacity && item.Status == "Open")
                    {
                        item.Status = "Full";
                    }
                }
            }
            await _context.SaveChangesAsync();

            // Thống kê nhanh
            ViewBag.TotalClasses = await _context.GymClasses.CountAsync();
            ViewBag.OpenClasses = await _context.GymClasses.CountAsync(c => c.Status == "Open");
            ViewBag.FullClasses = await _context.GymClasses.CountAsync(c => c.Status == "Full");
            ViewBag.CompletedClasses = await _context.GymClasses.CountAsync(c => c.Status == "Completed" || c.Status == "Cancelled");

            await PopulateDropdowns(trainerId, roomId);

            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedTrainerId = trainerId;
            ViewBag.SelectedRoomId = roomId;

            return View(classes);
        }

        // GET: /Admin/ClassManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var gymClass = await _context.GymClasses
                .Include(c => c.Trainer)
                    .ThenInclude(t => t.User)
                .Include(c => c.Room)
                .Include(c => c.ClassBookings)
                    .ThenInclude(b => b.Member)
                        .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
        }

        // GET: /Admin/ClassManagement/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();

            var vm = new GymClassFormViewModel
            {
                MaxCapacity = 25,
                Status = "Open",
                Schedule = "Thứ 2 - 4 - 6 (18:00 - 19:30)",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddMonths(1)
            };

            return View(vm);
        }

        // POST: /Admin/ClassManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymClassFormViewModel model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model.TrainerId, model.RoomId);
                return View(model);
            }

            string? finalImageUrl = model.ImageUrl;

            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (allowedExts.Contains(ext))
                {
                    var fileName = $"class_{Guid.NewGuid():N}{ext}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "classes", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    finalImageUrl = $"/images/classes/{fileName}";
                }
            }

            var gymClass = new GymClass
            {
                ClassName = model.ClassName.Trim(),
                Description = model.Description?.Trim(),
                TrainerId = model.TrainerId,
                RoomId = model.RoomId,
                Schedule = model.Schedule.Trim(),
                MaxCapacity = model.MaxCapacity,
                CurrentCapacity = 0,
                Status = model.Status,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ImageUrl = finalImageUrl
            };

            _context.GymClasses.Add(gymClass);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã tạo lớp học nhóm '{gymClass.ClassName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/ClassManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học cần sửa.");
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(gymClass.TrainerId, gymClass.RoomId);

            var vm = new GymClassFormViewModel
            {
                ClassId = gymClass.ClassId,
                ClassName = gymClass.ClassName,
                Description = gymClass.Description,
                TrainerId = gymClass.TrainerId,
                RoomId = gymClass.RoomId,
                Schedule = gymClass.Schedule,
                MaxCapacity = gymClass.MaxCapacity,
                Status = gymClass.Status,
                StartDate = gymClass.StartDate,
                EndDate = gymClass.EndDate,
                ImageUrl = gymClass.ImageUrl
            };

            return View(vm);
        }

        // POST: /Admin/ClassManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GymClassFormViewModel model, IFormFile? imageFile)
        {
            if (id != model.ClassId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model.TrainerId, model.RoomId);
                return View(model);
            }

            var gymClass = await _context.GymClasses
                .Include(c => c.ClassBookings)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học.");
                return RedirectToAction(nameof(Index));
            }

            string? finalImageUrl = model.ImageUrl;

            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (allowedExts.Contains(ext))
                {
                    var fileName = $"class_{Guid.NewGuid():N}{ext}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "classes", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    finalImageUrl = $"/images/classes/{fileName}";
                }
            }

            gymClass.ClassName = model.ClassName.Trim();
            gymClass.Description = model.Description?.Trim();
            gymClass.TrainerId = model.TrainerId;
            gymClass.RoomId = model.RoomId;
            gymClass.Schedule = model.Schedule.Trim();
            gymClass.MaxCapacity = model.MaxCapacity;
            gymClass.Status = model.Status;
            gymClass.StartDate = model.StartDate;
            gymClass.EndDate = model.EndDate;
            gymClass.ImageUrl = finalImageUrl;

            // Kiểm tra sĩ số thực tế
            int activeCount = gymClass.ClassBookings.Count(b => b.Status != "Cancelled");
            gymClass.CurrentCapacity = activeCount;
            if (gymClass.CurrentCapacity >= gymClass.MaxCapacity && gymClass.Status == "Open")
            {
                gymClass.Status = "Full";
            }

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Cập nhật lớp học '{gymClass.ClassName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/ClassManagement/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string newStatus)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học.");
                return RedirectToAction(nameof(Index));
            }

            var oldStatus = gymClass.Status;
            gymClass.Status = newStatus;
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã chuyển trạng thái lớp '{gymClass.ClassName}' sang '{newStatus}'.");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/ClassManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var gymClass = await _context.GymClasses
                .Include(c => c.ClassBookings)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học.");
                return RedirectToAction(nameof(Index));
            }

            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa lớp học '{gymClass.ClassName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns(int? selectedTrainerId = null, int? selectedRoomId = null)
        {
            var trainers = await _context.Trainers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.TrainerId,
                    DisplayName = $"{t.User.FullName} ({t.Specialization})"
                })
                .ToListAsync();

            var rooms = await _context.GymRooms
                .OrderBy(r => r.RoomName)
                .Select(r => new
                {
                    r.RoomId,
                    DisplayName = $"{r.RoomName} ({r.Location} - Sức chứa: {r.Capacity})"
                })
                .ToListAsync();

            ViewBag.TrainersList = new SelectList(trainers, "TrainerId", "DisplayName", selectedTrainerId);
            ViewBag.RoomsList = new SelectList(rooms, "RoomId", "DisplayName", selectedRoomId);
        }
    }
}

