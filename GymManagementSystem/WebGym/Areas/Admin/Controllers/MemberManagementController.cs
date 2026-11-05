using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;
using Member = WebGym.Models.Entities.Member;

namespace WebGym.Areas.Admin.Controllers
{
    using MemberEntity = WebGym.Models.Entities.Member;

    public class MemberManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public MemberManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/MemberManagement
        public async Task<IActionResult> Index(string? searchKeyword)
        {
            var query = _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(m => m.User.FullName.ToLower().Contains(kw) ||
                                         m.User.Email.ToLower().Contains(kw) ||
                                         (m.User.PhoneNumber != null && m.User.PhoneNumber.Contains(kw)));
            }

            var members = await query.OrderByDescending(m => m.MemberId).ToListAsync();
            ViewBag.SearchKeyword = searchKeyword;

            return View(members);
        }

        // GET: /Admin/MemberManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .Include(m => m.Bookings)
                    .ThenInclude(b => b.Trainer)
                        .ThenInclude(t => t.User)
                .Include(m => m.ProgressRecords.OrderByDescending(p => p.RecordedDate))
                .Include(m => m.Payments)
                    .ThenInclude(p => p.Package)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                SetErrorMessage("Không tìm thấy thông tin hội viên.");
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // GET: /Admin/MemberManagement/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Packages = new SelectList(await _context.GymPackages.ToListAsync(), "PackageId", "PackageName");
            return View(new MemberCreateViewModel());
        }

        // POST: /Admin/MemberManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateViewModel model)
        {
            // Kiểm tra username hoặc email trùng
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
                ViewBag.Packages = new SelectList(await _context.GymPackages.ToListAsync(), "PackageId", "PackageName", model.SelectedPackageId);
                return View(model);
            }

            // 1. Tạo User
            var user = new User
            {
                Username = model.Username.Trim(),
                PasswordHash = PasswordHelper.Hash(model.Password),
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber?.Trim(),
                Role = "Member",
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 2. Tính toán BMR sơ bộ
            double? bmr = null;
            if (model.Weight.HasValue && model.Height.HasValue && model.DateOfBirth.HasValue)
            {
                var age = DateTime.UtcNow.Year - model.DateOfBirth.Value.Year;
                if (model.Gender == "Nữ")
                {
                    bmr = 655 + (9.6 * model.Weight.Value) + (1.8 * model.Height.Value) - (4.7 * age);
                }
                else
                {
                    bmr = 66 + (13.7 * model.Weight.Value) + (5 * model.Height.Value) - (6.8 * age);
                }
            }

            // 3. Tạo Member
            var member = new MemberEntity
            {
                UserId = user.UserId,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Height = model.Height,
                Weight = model.Weight,
                BodyFatPercentage = model.BodyFatPercentage,
                MuscleMass = model.MuscleMass,
                Chest = model.Chest,
                Waist = model.Waist,
                Hips = model.Hips,
                BMR = bmr.HasValue ? Math.Round(bmr.Value, 1) : null
            };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            // 4. Nếu có chọn gói tập, gán luôn Subscription
            if (model.SelectedPackageId.HasValue)
            {
                var package = await _context.GymPackages.FindAsync(model.SelectedPackageId.Value);
                if (package != null)
                {
                    var subscription = new Subscription
                    {
                        MemberId = member.MemberId,
                        PackageId = package.PackageId,
                        OriginalPrice = package.Price,
                        PaidAmount = package.Price,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(package.DurationDays),
                        Status = "Active"
                    };
                    _context.Subscriptions.Add(subscription);

                    // Tạo lịch sử thanh toán tiền mặt trực tiếp tại quầy
                    var payment = new Payment
                    {
                        MemberId = member.MemberId,
                        PackageId = package.PackageId,
                        Amount = package.Price,
                        PaymentMethod = "DirectCash",
                        TransactionCode = $"CASH-{DateTime.UtcNow:yyyyMMddHHmmss}",
                        PaymentDate = DateTime.UtcNow,
                        Status = "Success"
                    };
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                }
            }

            SetSuccessMessage($"Đã thêm mới hội viên '{user.FullName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/MemberManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hội viên cần chỉnh sửa.");
                return RedirectToAction(nameof(Index));
            }

            var model = new MemberEditViewModel
            {
                MemberId = member.MemberId,
                UserId = member.UserId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Height = member.Height,
                Weight = member.Weight,
                BodyFatPercentage = member.BodyFatPercentage,
                MuscleMass = member.MuscleMass,
                Chest = member.Chest,
                Waist = member.Waist,
                Hips = member.Hips
            };

            return View(model);
        }

        // POST: /Admin/MemberManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MemberEditViewModel model)
        {
            if (id != model.MemberId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hội viên.");
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra email trùng với user khác
            if (await _context.Users.AnyAsync(u => u.Email == model.Email.Trim() && u.UserId != member.UserId))
            {
                ModelState.AddModelError("Email", "Địa chỉ Email này đã thuộc về tài khoản khác.");
                return View(model);
            }

            // Cập nhật User
            member.User.FullName = model.FullName.Trim();
            member.User.Email = model.Email.Trim();
            member.User.PhoneNumber = model.PhoneNumber?.Trim();

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                member.User.PasswordHash = PasswordHelper.Hash(model.NewPassword);
            }

            // Cập nhật Member
            member.Gender = model.Gender;
            member.DateOfBirth = model.DateOfBirth;
            member.Height = model.Height;
            member.Weight = model.Weight;
            member.BodyFatPercentage = model.BodyFatPercentage;
            member.MuscleMass = model.MuscleMass;
            member.Chest = model.Chest;
            member.Waist = model.Waist;
            member.Hips = model.Hips;

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã cập nhật thông tin hội viên '{member.User.FullName}'.");
            return RedirectToAction(nameof(Details), new { id = member.MemberId });
        }

        // POST: /Admin/MemberManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                .Include(m => m.Bookings)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hội viên.");
                return RedirectToAction(nameof(Index));
            }

            // Nếu đã có lịch tập hoặc gói tập, thông báo hủy gói thay vì xóa vật lý
            if (member.Subscriptions.Any() || member.Bookings.Any())
            {
                foreach (var sub in member.Subscriptions)
                {
                    sub.Status = "Cancelled";
                }
                await _context.SaveChangesAsync();
                SetSuccessMessage($"Đã vô hiệu hóa và hủy các gói tập đang kích hoạt của hội viên '{member.User.FullName}'.");
                return RedirectToAction(nameof(Index));
            }

            _context.Members.Remove(member);
            _context.Users.Remove(member.User);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa hội viên '{member.User.FullName}' khỏi hệ thống.");
            return RedirectToAction(nameof(Index));
        }
    }
}
