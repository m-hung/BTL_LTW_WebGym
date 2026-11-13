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
                }
            }

            var cardCode = member.MemberCardCode;
            var qrCode = member.MemberQrData;

            // Kiểm tra xem tài khoản này đã từng đo InBody thực tế chưa hay chỉ là tài khoản mới
            bool hasRealMeasurements = member.ProgressRecords.Any() || 
                                       (member.BodyFatPercentage.HasValue && member.BodyFatPercentage > 0) || 
                                       (member.Chest.HasValue && member.Chest > 0);

            // Nếu tài khoản mới chưa đo InBody và chỉ có giá trị mặc định lúc tạo (170cm, 65kg, 1500 kcal), hiển thị rỗng để PT kiểm tra
            bool isOldDefaultProfile = !hasRealMeasurements && member.Height == 170 && member.Weight == 65 && member.BMR == 1500;

            var displayHeight = isOldDefaultProfile ? null : member.Height;
            var displayWeight = isOldDefaultProfile ? null : member.Weight;
            var displayBMR = isOldDefaultProfile ? null : member.BMR;

            var dto = new MemberProfileDto
            {
                Id = member.MemberId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                AvatarUrl = avatarUrl,
                CardCode = cardCode,
                QrCode = qrCode,
                Height = displayHeight,
                Weight = displayWeight,
                BMR = displayBMR,
                BodyFatPercentage = member.BodyFatPercentage,
                MuscleMass = member.MuscleMass,
                Chest = member.Chest,
                Waist = member.Waist,
                Hips = member.Hips,
                CurrentPackageName = activeSub?.Package.PackageName,
                PackageTier = activeSub?.Package.PackageName?.Contains("Kim Cương", StringComparison.OrdinalIgnoreCase) == true ? "VVIP" : "VIP",
                MembershipStartDate = activeSub?.StartDate,
                MembershipEndDate = activeSub?.EndDate
            };

            // Nếu chưa có cân nặng hoặc chiều cao (tài khoản mới/chưa đo InBody thực tế) thì không gửi bản ghi biểu đồ
            var validProgress = (hasRealMeasurements && displayWeight > 0)
                ? member.ProgressRecords.OrderBy(p => p.RecordedDate).ToList()
                : new List<WebGym.Models.Entities.Progress>();

            ViewBag.ProgressList = validProgress;

            return View(dto);
        }

        // GET: /Member/Profile/PersonalInfo
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

            var cardCode = member.MemberCardCode;
            var qrCode = member.MemberQrData;

            var dto = new MemberPersonalInfoDto
            {
                MemberId = member.MemberId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                AvatarUrl = avatarUrl,
                CardCode = cardCode,
                QrCode = qrCode,
                CurrentPackageName = activeSub?.Package.PackageName ?? "Chưa đăng ký gói",
                PackageTier = activeSub?.Package.PackageName?.Contains("Kim Cương", StringComparison.OrdinalIgnoreCase) == true ? "VVIP" : "VIP",
                MembershipStartDate = activeSub?.StartDate,
                MembershipEndDate = activeSub?.EndDate
            };

            return View(dto);
        }

        // POST: /Member/Profile/UpdatePersonalInfo
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
                    member.AvatarUrl = $"/uploads/avatars/user_{userId!.Value}.jpg";
                }
                else
                {
                    SetErrorMessage("Định dạng ảnh không hợp lệ (chỉ hỗ trợ JPG, PNG, WEBP).");
                    return RedirectToAction(nameof(PersonalInfo));
                }
            }

            // Cập nhật thông tin cá nhân
            member.User.FullName = model.FullName;
            member.User.PhoneNumber = model.PhoneNumber;
            member.Gender = model.Gender;
            member.DateOfBirth = model.DateOfBirth;

            await _context.SaveChangesAsync();

            SetSuccessMessage("Cập nhật thông tin cá nhân thành công!");
            return RedirectToAction(nameof(PersonalInfo));
        }

        // POST: /Member/Profile/UploadAvatarAjax (Hỗ trợ học viên tải ảnh trực tiếp không cần reload)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatarAjax(IFormFile? avatarFile)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue || avatarFile == null || avatarFile.Length == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn file ảnh hợp lệ." });
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin hội viên." });
            }

            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();
            if (!allowedExts.Contains(ext))
            {
                return Json(new { success = false, message = "Định dạng ảnh không hợp lệ (chỉ hỗ trợ JPG, PNG, WEBP)." });
            }

            if (avatarFile.Length > 5 * 1024 * 1024)
            {
                return Json(new { success = false, message = "Dung lượng file ảnh không được vượt quá 5MB." });
            }

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(uploadsDir);
            var fileName = $"user_{userId.Value}_{DateTime.UtcNow.Ticks}{ext}";
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

            var newUrl = $"/uploads/avatars/{fileName}";
            member.AvatarUrl = newUrl;
            await _context.SaveChangesAsync();

            return Json(new { success = true, avatarUrl = newUrl, message = "Tải ảnh đại diện lên thành công!" });
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
