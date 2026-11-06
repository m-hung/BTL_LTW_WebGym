using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Member;

namespace WebGym.Areas.Member.Controllers
{
    public class ProfileController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

                // GET: /Member/Profile
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .Include(m => m.ProgressRecords)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hồ sơ cá nhân.");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Xóa triệt để các dữ liệu tiến trình đo InBody mẫu và chỉ số ảo nếu có trong database
            if (member.ProgressRecords.Any())
            {
                _context.Progress.RemoveRange(member.ProgressRecords);
                member.Height = null;
                member.Weight = null;
                member.BMR = null;
                member.BodyFatPercentage = null;
                member.MuscleMass = null;
                member.Chest = null;
                member.Waist = null;
                member.Hips = null;
                await _context.SaveChangesAsync();
            }

            var activeSub = member.Subscriptions
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == "Active");

            var dto = new MemberProfileDto
            {
                Id = member.MemberId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Height = member.Height,
                Weight = member.Weight,
                BMR = member.BMR,
                BodyFatPercentage = member.BodyFatPercentage,
                MuscleMass = member.MuscleMass,
                Chest = member.Chest,
                Waist = member.Waist,
                Hips = member.Hips,
                CurrentPackageName = activeSub?.Package?.PackageName,
                MembershipEndDate = activeSub?.EndDate
            };

            // Nếu chưa có cân nặng hoặc chiều cao (tài khoản mới/chưa đo InBody thực tế) thì không gửi bản ghi biểu đồ
            var validProgress = (member.Weight > 0)
                ? member.ProgressRecords.OrderBy(p => p.RecordedDate).ToList()
                : new List<WebGym.Models.Entities.Progress>();

            ViewBag.ProgressList = validProgress;

            return View(dto);
        }

        // GET: /Member/Profile/PersonalInfo (Yêu cầu 3)
        [HttpGet]
        public async Task<IActionResult> PersonalInfo()
        {
            var userId = CurrentUserId;
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Không tìm thấy thông tin hội viên.");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var activeSub = member.Subscriptions
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == "Active");

            string? avatarUrl = member.AvatarUrl;
            if (string.IsNullOrEmpty(avatarUrl) && userId.HasValue)
            {
                var diskPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", $"user_{userId.Value}.jpg");
                if (System.IO.File.Exists(diskPath))
                {
                    avatarUrl = $"/uploads/avatars/user_{userId.Value}.jpg?v={DateTime.UtcNow.Ticks}";
                    member.AvatarUrl = $"/uploads/avatars/user_{userId.Value}.jpg";
                    await _context.SaveChangesAsync();
                }
            }

            var dto = new MemberPersonalInfoDto
            {
                MemberId = member.MemberId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                AvatarUrl = avatarUrl,
                CurrentPackageName = activeSub?.Package.PackageName ?? "Chưa đăng ký gói"
            };

            return View(dto);
        }

        // POST: /Member/Profile/UpdatePersonalInfo (Yêu cầu 3)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalInfo(MemberPersonalInfoDto model, IFormFile? avatarFile)
        {
            var userId = CurrentUserId;
            var member = await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Hội viên không tồn tại.");
                return RedirectToAction(nameof(PersonalInfo));
            }

            // Xử lý upload avatar nếu có và lưu trực tiếp đường dẫn vào Database (Bảng Members)
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
                if (allowedExts.Contains(ext))
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                    Directory.CreateDirectory(uploadsDir);
                    var fileName = $"user_{userId!.Value}_{DateTime.UtcNow.Ticks}{ext}";
                    var filePath = Path.Combine(uploadsDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    // Xóa file avatar cũ nếu có
                    if (!string.IsNullOrEmpty(member.AvatarUrl))
                    {
                        var oldDisk = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", member.AvatarUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldDisk))
                        {
                            try { System.IO.File.Delete(oldDisk); } catch { }
                        }
                    }

                    // Lưu trực tiếp vào Database
                    member.AvatarUrl = $"/uploads/avatars/{fileName}";
                }
                else
                {
                    SetErrorMessage("Định dạng ảnh không hợp lệ (chỉ hỗ trợ JPG, PNG, WEBP).");
                    return RedirectToAction(nameof(PersonalInfo));
                }
            }

            // Cập nhật thông tin cá nhân (KHÔNG CHỈNH SỬA CHỈ SỐ CƠ THỂ theo yêu cầu của Leader)
            member.User.FullName = model.FullName;
            member.User.PhoneNumber = model.PhoneNumber;
            member.Gender = model.Gender;
            member.DateOfBirth = model.DateOfBirth;

            await _context.SaveChangesAsync();

            SetSuccessMessage("Cập nhật thông tin cá nhân thành công!");
            return RedirectToAction(nameof(PersonalInfo));
        }

        // POST: /Member/Profile/DeleteAvatar (Xóa ảnh đại diện, trở về mặc định và cập nhật Database)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userId = CurrentUserId;
            if (userId.HasValue)
            {
                var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
                if (member != null)
                {
                    if (!string.IsNullOrEmpty(member.AvatarUrl))
                    {
                        var oldDisk = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", member.AvatarUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldDisk))
                        {
                            try { System.IO.File.Delete(oldDisk); } catch { }
                        }
                    }
                    member.AvatarUrl = null;
                    await _context.SaveChangesAsync();
                }

                var legacyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", $"user_{userId.Value}.jpg");
                if (System.IO.File.Exists(legacyPath))
                {
                    try { System.IO.File.Delete(legacyPath); } catch { }
                }
            }

            SetSuccessMessage("Đã xóa ảnh đại diện và thiết lập lại ảnh mặc định!");
            return RedirectToAction(nameof(PersonalInfo));
        }

        // POST: /Member/Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MemberProfileDto model)
        {
            var userId = CurrentUserId;
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Hội viên không tồn tại.");
                return RedirectToAction(nameof(Index));
            }

            member.User.FullName = model.FullName;
            member.User.PhoneNumber = model.PhoneNumber;
            member.Gender = model.Gender;
            member.DateOfBirth = model.DateOfBirth;
            member.Height = model.Height;
            member.Weight = model.Weight;
            member.BMR = model.BMR;
            member.BodyFatPercentage = model.BodyFatPercentage;
            member.MuscleMass = model.MuscleMass;
            member.Chest = model.Chest;
            member.Waist = model.Waist;
            member.Hips = model.Hips;

            await _context.SaveChangesAsync();

            SetSuccessMessage("Cập nhật chỉ số và thông tin cá nhân thành công!");
            return RedirectToAction(nameof(Index));
        }
    }
}
