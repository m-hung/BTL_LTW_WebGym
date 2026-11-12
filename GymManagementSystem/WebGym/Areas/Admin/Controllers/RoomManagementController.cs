using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;
using Equipment = WebGym.Models.Entities.Equipment;

namespace WebGym.Areas.Admin.Controllers
{
    public class RoomManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public RoomManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/RoomManagement
        public async Task<IActionResult> Index(string? searchKeyword, string? statusFilter)
        {
            var query = _context.GymRooms
                .Include(r => r.Equipments)
                .Include(r => r.GymClasses)
                    .ThenInclude(c => c.Trainer)
                        .ThenInclude(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(r => r.RoomName.ToLower().Contains(kw) ||
                                         r.Location.ToLower().Contains(kw) ||
                                         (r.Description != null && r.Description.ToLower().Contains(kw)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(r => r.Status == statusFilter);
            }

            var rooms = await query.OrderBy(r => r.RoomName).ToListAsync();

            // Thống kê nhanh
            ViewBag.TotalRooms = await _context.GymRooms.CountAsync();
            ViewBag.ActiveRooms = await _context.GymRooms.CountAsync(r => r.Status == "Hoạt động");
            ViewBag.MaintenanceRooms = await _context.GymRooms.CountAsync(r => r.Status == "Bảo trì");
            ViewBag.ClosedRooms = await _context.GymRooms.CountAsync(r => r.Status == "Tạm đóng");
            ViewBag.TotalEquipments = await _context.Equipments.CountAsync();

            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.StatusFilter = statusFilter;

            return View(rooms);
        }

        // GET: /Admin/RoomManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.GymRooms
                .Include(r => r.Equipments)
                .Include(r => r.GymClasses)
                    .ThenInclude(c => c.Trainer)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                SetErrorMessage("Không tìm thấy thông tin phòng tập yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            var vm = new RoomDetailsViewModel
            {
                Room = room,
                TotalEquipments = room.Equipments.Count,
                BrokenEquipments = room.Equipments.Count(e => e.Status == "Đang hỏng" || e.Status == "Cần bảo trì"),
                TotalClasses = room.GymClasses.Count,
                Equipments = room.Equipments.OrderBy(e => e.EquipmentName).ToList(),
                Classes = room.GymClasses.OrderBy(c => c.ClassName).ToList()
            };

            return View(vm);
        }

        // GET: /Admin/RoomManagement/Create
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RoomFormViewModel
            {
                Capacity = 30,
                Status = "Hoạt động",
                Location = "Tầng 1 - Khu A"
            };
            return View(vm);
        }

        // POST: /Admin/RoomManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var room = new GymRoom
            {
                RoomName = model.RoomName.Trim(),
                Location = model.Location.Trim(),
                Capacity = model.Capacity,
                Status = model.Status,
                Description = model.Description?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.GymRooms.Add(room);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã thêm phòng tập '{room.RoomName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/RoomManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.GymRooms.FindAsync(id);
            if (room == null)
            {
                SetErrorMessage("Không tìm thấy phòng tập cần chỉnh sửa.");
                return RedirectToAction(nameof(Index));
            }

            var vm = new RoomFormViewModel
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Location = room.Location,
                Capacity = room.Capacity,
                Status = room.Status,
                Description = room.Description
            };

            return View(vm);
        }

        // POST: /Admin/RoomManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomFormViewModel model)
        {
            if (id != model.RoomId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var room = await _context.GymRooms.FindAsync(id);
            if (room == null)
            {
                SetErrorMessage("Không tìm thấy phòng tập.");
                return RedirectToAction(nameof(Index));
            }

            room.RoomName = model.RoomName.Trim();
            room.Location = model.Location.Trim();
            room.Capacity = model.Capacity;
            room.Status = model.Status;
            room.Description = model.Description?.Trim();

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Cập nhật phòng tập '{room.RoomName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/RoomManagement/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id, string status)
        {
            var room = await _context.GymRooms.FindAsync(id);
            if (room == null)
            {
                SetErrorMessage("Không tìm thấy phòng tập.");
                return RedirectToAction(nameof(Index));
            }

            var oldStatus = room.Status;
            room.Status = status;
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã chuyển trạng thái phòng '{room.RoomName}' từ '{oldStatus}' sang '{status}'.");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/RoomManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.GymRooms
                .Include(r => r.Equipments)
                .Include(r => r.GymClasses)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                SetErrorMessage("Không tìm thấy phòng tập.");
                return RedirectToAction(nameof(Index));
            }

            // Gỡ liên kết thiết bị và lớp học sang null an toàn
            foreach (var eq in room.Equipments)
            {
                eq.RoomId = null;
            }
            foreach (var gc in room.GymClasses)
            {
                gc.RoomId = null;
            }

            _context.GymRooms.Remove(room);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa phòng tập '{room.RoomName}' thành công!");
            return RedirectToAction(nameof(Index));
        }
    }
}
