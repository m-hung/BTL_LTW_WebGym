using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Auth;
using WebGym.Models.Entities;

namespace WebGym.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
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

            var passwordHash = HashPassword(model.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => (u.Email == model.Email || u.Username == model.Email) && u.PasswordHash == passwordHash);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email/Tên đăng nhập hoặc mật khẩu không chính xác.");
                return IsAjaxRequest ? AjaxValidationError() : View(model);
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

            var role = model.RegisterAsTrainer ? "PT" : "Member";
            var username = model.Email.Split('@')[0] + "_" + new Random().Next(100, 999);

            var newUser = new User
            {
                Username = username,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = HashPassword(model.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Khởi tạo Profile tương ứng (Tài khoản mới tạo để các chỉ số về Null theo yêu cầu)
            if (role == "Member")
            {
                _context.Members.Add(new Member
                {
                    UserId = newUser.UserId
                });
            }
            else
            {
                _context.Trainers.Add(new Trainer
                {
                    UserId = newUser.UserId,
                    Specialization = "Huấn luyện thể hình tổng quát",
                    Bio = "HLV nhiệt tình, chuyên nghiệp."
                });
            }

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

        // GET: /Auth/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
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

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
