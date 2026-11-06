using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models;
using WebGym.Models.DTOs.Payment;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(
            ApplicationDbContext context,
            IPaymentService paymentService,
            IEmailService emailService,
            ILogger<CheckoutController> logger)
        {
            _context = context;
            _paymentService = paymentService;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: /thanh-toan/1 hoặc /thanh-toan/pro hoặc /Checkout?slug=1
        [HttpGet("thanh-toan/{slug?}")]
        [HttpGet("Checkout/{slug?}")]
        [HttpGet("Checkout")]
        public async Task<IActionResult> Index(string? slug, string cycle = "month")
        {
            // Bắt buộc phải đăng nhập trước khi vào trang thanh toán
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                var target = string.IsNullOrEmpty(slug) ? "/thanh-toan" : $"/thanh-toan/{slug}";
                return Redirect($"/?auth=login&returnUrl={Uri.EscapeDataString(target)}");
            }

            var allPackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            if (allPackages.Count == 0)
            {
                return NotFound("Chưa có gói tập nào trong hệ thống.");
            }

            GymPackage? package = null;
            if (int.TryParse(slug, out int pkgId))
            {
                package = allPackages.FirstOrDefault(p => p.PackageId == pkgId);
            }
            else if (!string.IsNullOrEmpty(slug))
            {
                package = allPackages.FirstOrDefault(p =>
                    p.PackageName.Contains(slug, StringComparison.OrdinalIgnoreCase)
                    || (slug.Equals("starter", StringComparison.OrdinalIgnoreCase) && p.PackageId == 1)
                    || (slug.Equals("pro", StringComparison.OrdinalIgnoreCase) && (p.PackageId == 2 || p.PackageId == 3))
                    || (slug.Equals("elite", StringComparison.OrdinalIgnoreCase) && p.PackageId == 4));
            }

            // Mặc định chọn gói phổ biến hoặc gói đầu tiên
            package ??= allPackages.FirstOrDefault(p => p.PackageId == 2) ?? allPackages.First();

            ViewBag.AllPackages = allPackages;
            var allTrainers = await _context.Trainers.Include(t => t.User).ToListAsync();
            ViewBag.AllTrainers = allTrainers.Select(TrainerInfo.FromTrainer).ToList();
            var plan = PlanInfo.FromGymPackage(package);

            var model = new CheckoutViewModel
            {
                PackageId = package.PackageId,
                Plan = plan,
                Cycle = "package"
            };

            // Điền thông tin từ tài khoản đã đăng nhập & kiểm tra gói active để khóa hạ cấp
            Subscription? activeSub = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int uid))
            {
                var user = await _context.Users.FindAsync(uid);
                if (user != null)
                {
                    model.FullName = user.FullName;
                    model.Email = user.Email;
                    model.Phone = user.PhoneNumber ?? "";
                }

                activeSub = await _context.Subscriptions
                    .Include(s => s.Package)
                    .Where(s => s.Member.UserId == uid && s.Status == "Active")
                    .OrderByDescending(s => s.StartDate)
                    .FirstOrDefaultAsync();
            }

            ViewBag.ActiveSub = activeSub;

            // Khóa hạ cấp: Nếu hội viên đã có gói active và gói đang chọn có giá thấp hơn -> Chuyển về gói hiện tại
            if (activeSub != null && activeSub.Package != null && package.Price < activeSub.Package.Price)
            {
                return RedirectToAction("Index", new { slug = activeSub.PackageId });
            }

            return View(model);
        }

        // POST: /thanh-toan/{slug?}
        [HttpPost("thanh-toan/{slug?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string? slug, CheckoutViewModel model)
        {
            // Bắt buộc phải đăng nhập trước khi submit thanh toán
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                var target = string.IsNullOrEmpty(slug) ? "/thanh-toan" : $"/thanh-toan/{slug}";
                return Redirect($"/?auth=login&returnUrl={Uri.EscapeDataString(target)}");
            }

            var allPackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            ViewBag.AllPackages = allPackages;
            var allTrainers = await _context.Trainers.Include(t => t.User).ToListAsync();
            ViewBag.AllTrainers = allTrainers.Select(TrainerInfo.FromTrainer).ToList();

            var package = allPackages.FirstOrDefault(p => p.PackageId == model.PackageId);
            if (package == null && int.TryParse(slug, out int pkgId))
            {
                package = allPackages.FirstOrDefault(p => p.PackageId == pkgId);
            }
            if (package == null && !string.IsNullOrEmpty(slug))
            {
                package = allPackages.FirstOrDefault(p =>
                    p.PackageName.Contains(slug, StringComparison.OrdinalIgnoreCase)
                    || (slug.Equals("starter", StringComparison.OrdinalIgnoreCase) && p.PackageId == 1)
                    || (slug.Equals("pro", StringComparison.OrdinalIgnoreCase) && (p.PackageId == 2 || p.PackageId == 3))
                    || (slug.Equals("elite", StringComparison.OrdinalIgnoreCase) && p.PackageId == 4));
            }

            if (package == null)
            {
                return NotFound("Gói tập không tồn tại.");
            }

            // Gán lại Plan cho ViewModel nếu ModelState có lỗi cần render lại
            model.Plan = PlanInfo.FromGymPackage(package);

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // 1. Tìm hoặc tạo tài khoản Member tương ứng với người dùng
            var userEmail = model.Email.Trim().ToLowerInvariant();
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userEmail);
            Member? member = null;

            if (existingUser != null)
            {
                member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == existingUser.UserId);
                if (member == null)
                {
                    // Đã có User nhưng chưa có Member profile
                    member = new Member
                    {
                        UserId = existingUser.UserId
                    };
                    _context.Members.Add(member);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                // Tạo tài khoản Member mới
                const string defaultPassHash = "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=";
                var emailPrefix = model.Email.Split('@')[0];
                var uniqueUsername = $"{emailPrefix}_{DateTime.UtcNow.Ticks.ToString()[^4..]}";

                var newUser = new User
                {
                    Username = uniqueUsername,
                    PasswordHash = defaultPassHash,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.Phone,
                    Role = "Member",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                member = new Member
                {
                    UserId = newUser.UserId
                };
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
            }

            // Khóa hạ cấp ở phía server: Không cho phép thanh toán gói thấp hơn gói hiện tại
            if (member != null)
            {
                var activeSub = await _context.Subscriptions
                    .Include(s => s.Package)
                    .Where(s => s.MemberId == member.MemberId && s.Status == "Active")
                    .OrderByDescending(s => s.StartDate)
                    .FirstOrDefaultAsync();

                if (activeSub != null && activeSub.Package != null && package.Price < activeSub.Package.Price)
                {
                    return RedirectToAction("Index", new { slug = activeSub.PackageId });
                }
            }

            // 2. Tính tiền & tạo Payment + Subscription
            decimal totalAmount = package.Price;
            int durationDays = package.DurationDays;
            var orderId = $"ORDER_{DateTime.UtcNow.Ticks}";

            var payment = new Payment
            {
                MemberId = member!.MemberId,
                PackageId = package.PackageId,
                Amount = totalAmount,
                PaymentMethod = model.PaymentMethod == "vnpay" ? "VNPay" : "Cash",
                TransactionCode = orderId,
                PaymentDate = DateTime.UtcNow,
                Status = model.PaymentMethod == "vnpay" ? "Pending" : "Completed"
            };
            _context.Payments.Add(payment);

            if (model.PaymentMethod != "vnpay")
            {
                var oldActiveSubs = await _context.Subscriptions
                    .Where(s => s.MemberId == member.MemberId && s.Status == "Active")
                    .ToListAsync();
                foreach (var old in oldActiveSubs)
                {
                    old.Status = "Expired";
                }
            }

            var subscription = new Subscription
            {
                MemberId = member!.MemberId,
                PackageId = package.PackageId,
                OriginalPrice = package.Price,
                PaidAmount = totalAmount,
                StartDate = model.StartDate ?? DateTime.UtcNow,
                EndDate = (model.StartDate ?? DateTime.UtcNow).AddDays(durationDays),
                Status = model.PaymentMethod == "vnpay" ? "PendingPayment" : "Active"
            };
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            // 3. Nếu thanh toán qua cổng VNPay -> Gọi IPaymentService tạo URL thanh toán
            if (model.PaymentMethod == "vnpay")
            {
                var payDto = new PaymentRequestDto
                {
                    PackageId = package.PackageId,
                    PackageName = $"{package.PackageName} ({package.DurationDays} ngày)",
                    Amount = totalAmount,
                    OrderDescription = $"Thanh toan goi {package.PackageName} - {model.FullName}"
                };

                var paymentResult = await _paymentService.CreatePaymentUrlAsync(payDto, HttpContext);
                if (paymentResult.Success && !string.IsNullOrEmpty(paymentResult.PaymentUrl))
                {
                    payment.TransactionCode = paymentResult.OrderId ?? orderId;
                    await _context.SaveChangesAsync();
                    return Redirect(paymentResult.PaymentUrl);
                }
            }

            // 4. Gửi email xác nhận
            try
            {
                _ = _emailService.SendPaymentConfirmationAsync(
                    model.Email,
                    model.FullName,
                    package.PackageName,
                    totalAmount,
                    orderId
                );
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không thể gửi email xác nhận cho {Email}", model.Email);
            }

            // 5. Lưu thông tin hiển thị tại trang thành công
            TempData["Plan"] = package.PackageName;
            TempData["Cycle"] = $"{package.DurationDays} ngày";
            TempData["Total"] = CheckoutViewModel.Vnd(totalAmount);
            TempData["Email"] = model.Email;
            TempData["Name"] = model.FullName;
            TempData["Start"] = (model.StartDate ?? DateTime.UtcNow).ToString("dd/MM/yyyy");
            TempData["Time"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            TempData["TxnCode"] = orderId;
            TempData["Method"] = model.PaymentMethod == "vnpay" ? "VNPay" : "Thanh toán tại quầy";

            return RedirectToAction(nameof(Success));
        }

        // GET: /thanh-toan/thanh-cong
        [HttpGet("thanh-toan/thanh-cong")]
        public IActionResult Success()
        {
            if (TempData["Plan"] == null && TempData["Name"] == null)
            {
                return Redirect("~/");
            }
            TempData.Keep();
            return View();
        }
    }
}
