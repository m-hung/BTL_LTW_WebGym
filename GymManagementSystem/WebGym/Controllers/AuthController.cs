using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Auth;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Controllers
{
    public class AuthController : BaseController
    {
        // Liên kết đặt lại mật khẩu có hiệu lực 30 phút và chỉ dùng được 1 lần
        private const string ResetTokenPurpose = "WebGym.Auth.ResetPassword.v1";
        private static readonly TimeSpan ResetTokenLifetime = TimeSpan.FromMinutes(30);

        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;
        private readonly IDataProtectionProvider _dataProtection;
        private readonly IWebHostEnvironment _env;

        public AuthController(
            ApplicationDbContext context,
            ILogger<AuthController> logger,
            IEmailService emailService,
            IDataProtectionProvider dataProtection,
            IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _dataProtection = dataProtection;
            _env = env;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToUserDashboard();
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return IsAjaxRequest ? AjaxValidationError() : View(model);
            }

            // Tìm theo email hoặc tên đăng nhập, rồi kiểm tra mật khẩu bằng PasswordHasher (có salt)
            var login = model.Email.Trim();
            var candidates = await _context.Users
                .Where(u => u.Email == login || u.Username == login)
                .ToListAsync();

            User? user = null;
            var verifyResult = PasswordVerificationResult.Failed;
            foreach (var candidate in candidates)
            {
                verifyResult = PasswordHelper.Verify(candidate.PasswordHash, model.Password);
                if (verifyResult != PasswordVerificationResult.Failed)
                {
                    user = candidate;
                    break;
                }
            }

            if (candidates.Count == 0)
            {
                PasswordHelper.VerifyDummy(model.Password); // giữ thời gian phản hồi như khi có tài khoản
            }

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email/Tên đăng nhập hoặc mật khẩu không chính xác.");
                return IsAjaxRequest ? AjaxValidationError() : View(model);
            }

            // Tài khoản còn hash SHA-256 cũ → đổi sang hash mới ngay khi đăng nhập đúng
            if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = PasswordHelper.Hash(model.Password);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Đã nâng cấp hash mật khẩu cho UserId {UserId}", user.UserId);
            }

            // Tạo Claims cho Cookie Authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            var welcome = $"Chào mừng {user.FullName} ({user.Role}) đã quay trở lại!";
            SetSuccessMessage(welcome);

            var redirectUrl = !string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)
                ? model.ReturnUrl
                : GetDashboardUrl(user.Role);

            // Modal đăng nhập gọi bằng fetch → trả JSON để trang tự chuyển hướng
            if (IsAjaxRequest)
            {
                return Json(new { success = true, message = welcome, redirectUrl });
            }

            return Redirect(redirectUrl);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return IsAjaxRequest ? AjaxValidationError() : View(model);
            }

            var existingUser = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (existingUser)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return IsAjaxRequest ? AjaxValidationError() : View(model);
            }

            // Đăng ký công khai chỉ tạo tài khoản Hội viên. Tài khoản HLV (PT) do Admin tạo ở trang Quản lý HLV.
            const string role = "Member";
            var username = model.Email.Split('@')[0] + "_" + new Random().Next(100, 999);

            var newUser = new User
            {
                Username = username,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = PasswordHelper.Hash(model.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Khởi tạo hồ sơ Hội viên
            _context.Members.Add(new Member
            {
                UserId = newUser.UserId,
                Height = 170,
                Weight = 65,
                BMR = 1500
            });

            await _context.SaveChangesAsync();

            const string registered = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";

            // Modal đăng ký: không đổi trang, JS sẽ chuyển sang tab Đăng nhập
            if (IsAjaxRequest)
            {
                return Json(new { success = true, message = registered });
            }

            SetSuccessMessage(registered);
            return RedirectToAction(nameof(Login));
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            SetSuccessMessage("Đã đăng xuất thành công.");
            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/ForgotPassword  (trang dự phòng khi không dùng modal)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        // POST: /Auth/ForgotPassword
        // Luôn trả cùng một thông báo dù email có tồn tại hay không, để không lộ danh sách tài khoản.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return IsAjaxRequest ? AjaxValidationError() : View(model);
            }

            const string sentMessage = "Nếu email này đã đăng ký, chúng tôi đã gửi liên kết đặt lại mật khẩu. " +
                                       "Vui lòng kiểm tra hộp thư (kể cả mục Spam). Liên kết có hiệu lực trong 30 phút.";
            string? devLink = null;

            var email = model.Email.Trim();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                var token = CreateResetToken(user);
                var link = Url.Action(nameof(ResetPassword), "Auth", new { token }, Request.Scheme)!;

                _ = _emailService.SendPasswordResetAsync(user.Email, user.FullName, link, (int)ResetTokenLifetime.TotalMinutes);
                _logger.LogInformation("Đã tạo liên kết đặt lại mật khẩu cho UserId {UserId}", user.UserId);

                // Môi trường Development: hiện luôn liên kết trên màn hình để demo khi chưa cấu hình gửi mail
                if (_env.IsDevelopment())
                {
                    devLink = link;
                }
            }

            if (IsAjaxRequest)
            {
                return Json(new { success = true, message = sentMessage, devLink });
            }

            ViewBag.SentMessage = sentMessage;
            ViewBag.DevLink = devLink;
            return View(model);
        }

        // GET: /Auth/ResetPassword?token=...  (liên kết trong email)
        // Kiểm tra token rồi mở form đặt mật khẩu mới trong modal ở trang chủ.
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string? token)
        {
            var home = Url.Content("~/");
            var user = await FindUserByResetTokenAsync(token);
            if (user == null)
            {
                return Redirect(home + "?auth=forgot&reset=invalid");
            }

            // Đang đăng nhập tài khoản khác → đăng xuất để modal đặt lại mật khẩu hiện ra
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return Redirect(home + "?auth=reset&token=" + Uri.EscapeDataString(token!));
        }

        // POST: /Auth/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var home = Url.Content("~/");
            const string invalidMessage = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn. Vui lòng gửi lại yêu cầu.";

            var user = await FindUserByResetTokenAsync(model.Token);
            if (user == null)
            {
                if (IsAjaxRequest)
                {
                    return BadRequest(new { success = false, expired = true, message = invalidMessage });
                }
                return Redirect(home + "?auth=forgot&reset=invalid");
            }

            if (!ModelState.IsValid)
            {
                if (IsAjaxRequest)
                {
                    return AjaxValidationError();
                }
                return Redirect(home + "?auth=reset&token=" + Uri.EscapeDataString(model.Token));
            }

            user.PasswordHash = PasswordHelper.Hash(model.Password);
            await _context.SaveChangesAsync();
            _logger.LogInformation("UserId {UserId} đã đặt lại mật khẩu", user.UserId);

            const string doneMessage = "Đặt lại mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.";
            if (IsAjaxRequest)
            {
                return Json(new { success = true, message = doneMessage, email = user.Email });
            }

            SetSuccessMessage(doneMessage);
            return RedirectToAction(nameof(Login));
        }

        // GET: /Auth/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Request do modal (fetch) gửi lên có header X-Requested-With
        private bool IsAjaxRequest =>
            string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

        // Trả lỗi ModelState dạng JSON: { success:false, message, errors:{ Field:[msg], "":[msg chung] } }
        private IActionResult AjaxValidationError()
        {
            var errors = ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(e => e.Key, e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

            var message = errors.TryGetValue(string.Empty, out var general)
                ? general[0]
                : "Vui lòng kiểm tra lại thông tin.";

            return BadRequest(new { success = false, message, errors });
        }

        private string GetDashboardUrl(string? role = null)
        {
            var userRole = role ?? CurrentUserRole;

            return (userRole switch
            {
                "Admin" => Url.Action("Index", "Dashboard", new { area = "Admin" }),
                "PT" or "Trainer" => Url.Action("Index", "Schedule", new { area = "PT" }),
                "Member" => Url.Action("Index", "Profile", new { area = "Member" }),
                _ => Url.Action("Index", "Home", new { area = "" })
            }) ?? "/";
        }

        private IActionResult RedirectToUserDashboard(string? role = null)
        {
            return Redirect(GetDashboardUrl(role));
        }

        /* ---------- Token đặt lại mật khẩu (ASP.NET Data Protection, không cần thêm bảng DB) ---------- */

        private ITimeLimitedDataProtector ResetTokenProtector =>
            _dataProtection.CreateProtector(ResetTokenPurpose).ToTimeLimitedDataProtector();

        // "Dấu vân tay" của mật khẩu hiện tại: đổi mật khẩu xong thì mọi token cũ tự mất hiệu lực
        private static string PasswordStamp(string passwordHash)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(passwordHash)))[..16];
        }

        private string CreateResetToken(User user)
        {
            var payload = $"{user.UserId}|{PasswordStamp(user.PasswordHash)}";
            return ResetTokenProtector.Protect(payload, ResetTokenLifetime);
        }

        private async Task<User?> FindUserByResetTokenAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            string payload;
            try
            {
                payload = ResetTokenProtector.Unprotect(token); // tự báo lỗi nếu bị sửa hoặc đã hết hạn
            }
            catch (Exception ex) when (ex is CryptographicException or FormatException)
            {
                return null; // token bị sửa, sai định dạng hoặc hết hạn
            }

            var parts = payload.Split('|');
            if (parts.Length != 2 || !int.TryParse(parts[0], out var userId))
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null || PasswordStamp(user.PasswordHash) != parts[1])
            {
                return null; // mật khẩu đã đổi sau khi tạo liên kết → liên kết đã dùng rồi
            }

            return user;
        }
    }
}
