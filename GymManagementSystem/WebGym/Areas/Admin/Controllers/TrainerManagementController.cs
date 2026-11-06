using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class TrainerManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public TrainerManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/TrainerManagement
        public async Task<IActionResult> Index(string? searchKeyword)
        {
            var query = _context.Trainers
                .Include(t => t.User)
                .Include(t => t.Bookings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(t => t.User.FullName.ToLower().Contains(kw) ||
                                         t.User.Email.ToLower().Contains(kw) ||
                                         t.Specialization.ToLower().Contains(kw));
            }

            var trainers = await query.OrderByDescending(t => t.TrainerId).ToListAsync();
            ViewBag.SearchKeyword = searchKeyword;

            return View(trainers);
        }

        // GET: /Admin/TrainerManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.User)
                .Include(t => t.Bookings)
                    .ThenInclude(b => b.Member)
                        .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(t => t.TrainerId == id);

            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy thông tin huấn luyện viên.");
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        // GET: /Admin/TrainerManagement/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TrainerCreateViewModel());
        }

        // POST: /Admin/TrainerManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerCreateViewModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username.Trim()))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng.");
            }
            if (await _context.Users.AnyAsync(u => u.Email == model.Email.Trim()))
            {
                ModelState.AddModelError("Email", "Địa chỉ Email này đã được đăng ký.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Tạo User với Role = PT
            var user = new User
            {
                Username = model.Username.Trim(),
                PasswordHash = PasswordHelper.Hash(model.Password),
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber?.Trim(),
                Role = "PT",
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 2. Tạo Trainer
            var trainer = new Trainer
            {
                UserId = user.UserId,
                Specialization = model.Specialization.Trim(),
                Bio = model.Bio?.Trim(),
                AvatarUrl = model.AvatarUrl?.Trim()
            };
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã thêm mới Huấn luyện viên '{user.FullName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/TrainerManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.TrainerId == id);
            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy HLV yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            var model = new TrainerEditViewModel
            {
                TrainerId = trainer.TrainerId,
                UserId = trainer.UserId,
                FullName = trainer.User.FullName,
                Email = trainer.User.Email,
                PhoneNumber = trainer.User.PhoneNumber,
                Specialization = trainer.Specialization,
                Bio = trainer.Bio,
                AvatarUrl = trainer.AvatarUrl
            };

            return View(model);
        }

        // POST: /Admin/TrainerManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainerEditViewModel model)
        {
            if (id != model.TrainerId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var trainer = await _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.TrainerId == id);
            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy HLV.");
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra trùng email
            if (await _context.Users.AnyAsync(u => u.Email == model.Email.Trim() && u.UserId != trainer.UserId))
            {
                ModelState.AddModelError("Email", "Địa chỉ Email này đã thuộc về người khác.");
                return View(model);
            }

            trainer.User.FullName = model.FullName.Trim();
            trainer.User.Email = model.Email.Trim();
            trainer.User.PhoneNumber = model.PhoneNumber?.Trim();

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                trainer.User.PasswordHash = PasswordHelper.Hash(model.NewPassword);
            }

            trainer.Specialization = model.Specialization.Trim();
            trainer.Bio = model.Bio?.Trim();
            trainer.AvatarUrl = model.AvatarUrl?.Trim();

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã cập nhật thông tin HLV '{trainer.User.FullName}'.");
            return RedirectToAction(nameof(Details), new { id = trainer.TrainerId });
        }

        // POST: /Admin/TrainerManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.User)
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.TrainerId == id);

            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy huấn luyện viên.");
                return RedirectToAction(nameof(Index));
            }

            if (trainer.Bookings.Any(b => b.Status == "Pending" || b.Status == "Approved"))
            {
                SetErrorMessage($"HLV '{trainer.User.FullName}' đang có các lịch tập đang chờ hoặc đã duyệt, không thể xóa!");
                return RedirectToAction(nameof(Index));
            }

            _context.Trainers.Remove(trainer);
            _context.Users.Remove(trainer.User);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa HLV '{trainer.User.FullName}' khỏi hệ thống.");
            return RedirectToAction(nameof(Index));
        }
    }
}
