using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Trainer;

namespace WebGym.Areas.PT.Controllers
{
    /// <summary>
    /// Hồ sơ cá nhân của HLV. Giao diện là bảng trượt mở từ ảnh đại diện ở góc dưới menu
    /// (_PTLayout), gửi bằng fetch nên không chuyển trang.
    /// </summary>
    public class ProfileController : PTBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /PT/Profile → về Lịch dạy và mở sẵn bảng hồ sơ
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect(Url.Action("Index", "Schedule", new { area = "PT" }) + "?profile=open");
        }

        // POST: /PT/Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TrainerProfileDto model)
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                const string notFound = "Không tìm thấy hồ sơ huấn luyện viên của tài khoản này.";
                if (IsAjaxRequest) return BadRequest(new { success = false, message = notFound });
                SetErrorMessage(notFound);
                return RedirectBack();
            }

            model.FullName = model.FullName?.Trim() ?? string.Empty;
            model.Email = model.Email?.Trim() ?? string.Empty;
            model.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim();
            model.Specialization = model.Specialization?.Trim() ?? string.Empty;
            model.Bio = string.IsNullOrWhiteSpace(model.Bio) ? null : model.Bio.Trim();
            model.AvatarUrl = string.IsNullOrWhiteSpace(model.AvatarUrl) ? null : model.AvatarUrl.Trim();

            if (model.AvatarUrl != null &&
                !(Uri.TryCreate(model.AvatarUrl, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)))
            {
                ModelState.AddModelError(nameof(model.AvatarUrl), "Đường dẫn ảnh phải bắt đầu bằng http:// hoặc https://");
            }

            if (ModelState.IsValid &&
                await _context.Users.AnyAsync(u => u.Email == model.Email && u.UserId != trainer.UserId))
            {
                ModelState.AddModelError(nameof(model.Email), "Email này đã thuộc về tài khoản khác.");
            }

            if (!ModelState.IsValid)
            {
                if (IsAjaxRequest) return AjaxValidationError();
                SetErrorMessage(ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage);
                return RedirectBack();
            }

            var nameOrEmailChanged = trainer.User.FullName != model.FullName || trainer.User.Email != model.Email;

            trainer.User.FullName = model.FullName;
            trainer.User.Email = model.Email;
            trainer.User.PhoneNumber = model.PhoneNumber;
            trainer.Specialization = model.Specialization;
            trainer.Bio = model.Bio;
            trainer.AvatarUrl = model.AvatarUrl;

            await _context.SaveChangesAsync();
            _logger.LogInformation("HLV UserId {UserId} đã cập nhật hồ sơ", trainer.UserId);

            // Tên và email nằm trong cookie đăng nhập → cấp lại cookie để menu hiện đúng tên mới
            if (nameOrEmailChanged)
            {
                await RefreshSignInAsync(trainer.User.FullName, trainer.User.Email);
            }

            const string saved = "Đã lưu hồ sơ. Học viên sẽ thấy thông tin mới ở trang Huấn luyện viên.";
            if (IsAjaxRequest)
            {
                return Json(new
                {
                    success = true,
                    message = saved,
                    profile = new
                    {
                        fullName = trainer.User.FullName,
                        email = trainer.User.Email,
                        phoneNumber = trainer.User.PhoneNumber,
                        specialization = trainer.Specialization,
                        bio = trainer.Bio,
                        avatarUrl = trainer.AvatarUrl,
                        initials = WebGym.Helpers.CoachUi.Initials(trainer.User.FullName)
                    }
                });
            }

            SetSuccessMessage(saved);
            return RedirectBack();
        }

        private async Task RefreshSignInAsync(string fullName, string email)
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Principal?.Identity is not ClaimsIdentity oldIdentity) return;

            var identity = new ClaimsIdentity(oldIdentity.AuthenticationType ?? CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var claim in oldIdentity.Claims)
            {
                if (claim.Type == ClaimTypes.Name || claim.Type == ClaimTypes.Email) continue;
                identity.AddClaim(claim);
            }
            identity.AddClaim(new Claim(ClaimTypes.Name, fullName));
            identity.AddClaim(new Claim(ClaimTypes.Email, email));

            // Giữ nguyên "ghi nhớ đăng nhập" và thời hạn cũ
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                auth.Properties);
        }

        private bool IsAjaxRequest =>
            string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

        private IActionResult AjaxValidationError()
        {
            var errors = ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(e => e.Key, e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray());
            return BadRequest(new { success = false, message = "Vui lòng kiểm tra lại thông tin.", errors });
        }

        // Không dùng JavaScript: quay lại trang trước (chỉ trong site), mặc định là Lịch dạy
        private IActionResult RedirectBack()
        {
            var referer = Request.Headers.Referer.ToString();
            if (Uri.TryCreate(referer, UriKind.Absolute, out var r) && r.Host == Request.Host.Host && Url.IsLocalUrl(r.PathAndQuery))
            {
                return Redirect(r.PathAndQuery);
            }
            return RedirectToAction("Index", "Schedule", new { area = "PT" });
        }
    }
}
