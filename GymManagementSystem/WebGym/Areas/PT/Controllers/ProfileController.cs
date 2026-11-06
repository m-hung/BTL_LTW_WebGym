using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Trainer;
using WebGym.Models.Entities;

namespace WebGym.Areas.PT.Controllers
{
    /// <summary>
    /// Trang Thông tin cá nhân của HLV: sửa họ tên, số điện thoại, chuyên môn, giới thiệu
    /// và tải ảnh đại diện. Email đăng nhập cố định (giống trang Member).
    /// Ảnh lưu ở wwwroot/images/avatars/trainer_{id}.{jpg|png|webp}, AvatarUrl = "~/images/avatars/..."
    /// nên trang chủ, trang chi tiết HLV và trang Admin đều hiển thị được.
    /// </summary>
    public class ProfileController : PTBaseController
    {
        private const long MaxAvatarBytes = 5 * 1024 * 1024; // 5 MB
        private const string AvatarFolder = "images/avatars";

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context, IWebHostEnvironment env, ILogger<ProfileController> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        // GET: /PT/Profile
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trainer = await FindCurrentTrainerAsync();
            if (trainer == null)
            {
                SetErrorMessage("Tài khoản này chưa có hồ sơ huấn luyện viên.");
                return RedirectToAction("Index", "Schedule", new { area = "PT" });
            }

            var model = new TrainerProfileDto
            {
                FullName = trainer.User.FullName,
                Email = trainer.User.Email,
                PhoneNumber = trainer.User.PhoneNumber,
                Specialization = trainer.Specialization,
                Bio = trainer.Bio
            };
            await FillDisplayAsync(model, trainer);
            return View(model);
        }

        // POST: /PT/Profile  (form multipart: thông tin + ảnh đại diện nếu có)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Index(TrainerProfileDto model, IFormFile? avatarFile)
        {
            var trainer = await FindCurrentTrainerAsync();
            if (trainer == null)
            {
                SetErrorMessage("Tài khoản này chưa có hồ sơ huấn luyện viên.");
                return RedirectToAction("Index", "Schedule", new { area = "PT" });
            }

            ModelState.Remove(nameof(TrainerProfileDto.Email)); // email không sửa ở trang này

            string? newAvatarExt = null;
            if (avatarFile != null && avatarFile.Length > 0)
            {
                newAvatarExt = await DetectImageExtensionAsync(avatarFile);
                if (avatarFile.Length > MaxAvatarBytes)
                {
                    ModelState.AddModelError("avatarFile", "Ảnh tối đa 5 MB. Hãy chọn ảnh nhỏ hơn.");
                }
                else if (newAvatarExt == null)
                {
                    ModelState.AddModelError("avatarFile", "Chỉ nhận ảnh JPG, PNG hoặc WEBP.");
                }
            }

            if (!ModelState.IsValid)
            {
                model.Email = trainer.User.Email;
                await FillDisplayAsync(model, trainer);
                return View(model);
            }

            var oldName = trainer.User.FullName;
            trainer.User.FullName = model.FullName.Trim();
            trainer.User.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim();
            trainer.Specialization = model.Specialization.Trim();
            trainer.Bio = string.IsNullOrWhiteSpace(model.Bio) ? null : model.Bio.Trim();

            if (newAvatarExt != null)
            {
                trainer.AvatarUrl = await SaveAvatarAsync(trainer.TrainerId, avatarFile!, newAvatarExt);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("HLV TrainerId {TrainerId} đã cập nhật hồ sơ", trainer.TrainerId);

            // Họ tên nằm trong cookie đăng nhập → cấp lại để menu hiện tên mới ngay
            if (oldName != trainer.User.FullName)
            {
                await RefreshNameClaimAsync(trainer.User.FullName);
            }

            SetSuccessMessage("Đã lưu thông tin cá nhân. Học viên sẽ thấy hồ sơ mới ở trang Huấn luyện viên.");
            return RedirectToAction(nameof(Index));
        }

        // POST: /PT/Profile/DeleteAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAvatar()
        {
            var trainer = await FindCurrentTrainerAsync();
            if (trainer == null)
            {
                return RedirectToAction("Index", "Schedule", new { area = "PT" });
            }

            DeleteUploadedAvatars(trainer.TrainerId);
            trainer.AvatarUrl = null;
            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã xoá ảnh đại diện. Hồ sơ sẽ hiện chữ viết tắt tên của bạn.");
            return RedirectToAction(nameof(Index));
        }

        /* ---------------- Hàm hỗ trợ ---------------- */

        private Task<Trainer?> FindCurrentTrainerAsync()
        {
            var userId = CurrentUserId;
            return _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.UserId == userId);
        }

        private async Task FillDisplayAsync(TrainerProfileDto model, Trainer trainer)
        {
            model.TrainerId = trainer.TrainerId;
            model.AvatarPath = CoachUi.AvatarPath(trainer.AvatarUrl);

            var bookings = _context.Bookings.Where(b => b.TrainerId == trainer.TrainerId);
            model.MemberCount = await bookings.Select(b => b.MemberId).Distinct().CountAsync();
            model.CompletedSessions = await bookings.CountAsync(b => b.Status == "Completed");
            model.UpcomingSessions = await bookings.CountAsync(b => b.Status == "Approved" && b.BookingDate >= DateTime.Today);
        }

        // Nhận diện ảnh theo nội dung file (không tin vào đuôi file hay Content-Type do trình duyệt gửi)
        private static async Task<string?> DetectImageExtensionAsync(IFormFile file)
        {
            var header = new byte[12];
            await using var stream = file.OpenReadStream();
            var read = await stream.ReadAsync(header.AsMemory(0, header.Length));
            if (read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF) return ".jpg";
            if (read >= 8 && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47) return ".png";
            if (read >= 12 && header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46
                && header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50) return ".webp";
            return null;
        }

        private async Task<string> SaveAvatarAsync(int trainerId, IFormFile file, string ext)
        {
            var dir = Path.Combine(_env.WebRootPath, AvatarFolder);
            Directory.CreateDirectory(dir);
            DeleteUploadedAvatars(trainerId); // bỏ ảnh cũ (có thể khác đuôi)

            var fileName = $"trainer_{trainerId}{ext}";
            await using (var output = new FileStream(Path.Combine(dir, fileName), FileMode.Create))
            {
                await file.CopyToAsync(output);
            }
            return $"~/{AvatarFolder}/{fileName}";
        }

        private void DeleteUploadedAvatars(int trainerId)
        {
            var dir = Path.Combine(_env.WebRootPath, AvatarFolder);
            if (!Directory.Exists(dir)) return;
            foreach (var path in Directory.GetFiles(dir, $"trainer_{trainerId}.*"))
            {
                try { System.IO.File.Delete(path); }
                catch (IOException ex) { _logger.LogWarning(ex, "Không xoá được ảnh cũ {Path}", path); }
            }
        }

        private async Task RefreshNameClaimAsync(string fullName)
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Principal?.Identity is not ClaimsIdentity oldIdentity) return;

            var identity = new ClaimsIdentity(oldIdentity.AuthenticationType ?? CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var claim in oldIdentity.Claims)
            {
                if (claim.Type != ClaimTypes.Name) identity.AddClaim(claim);
            }
            identity.AddClaim(new Claim(ClaimTypes.Name, fullName));

            // Giữ nguyên "ghi nhớ đăng nhập" và thời hạn cũ
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), auth.Properties);
        }
    }
}
