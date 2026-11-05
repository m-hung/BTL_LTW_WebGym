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

        // GET: /thanh-toan/1?cycle=year hoặc /thanh-toan/pro
        [HttpGet("thanh-toan/{slug?}")]
        public async Task<IActionResult> Index(string? slug, string cycle = "month")
        {
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
                Cycle = cycle == "year" ? "year" : "month"
            };

            // Nếu người dùng đã đăng nhập, tự động điền thông tin
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
            }

            return View(model);
        }

        // POST: /thanh-toan/{slug?}
        [HttpPost("thanh-toan/{slug?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string? slug, CheckoutViewModel model)
        {
            var allPackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            GymPackage? package = null;

            if (model.PackageId > 0)
            {
                package = allPackages.FirstOrDefault(p => p.PackageId == model.PackageId);
            }
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

            package ??= allPackages.FirstOrDefault();
            if (package == null)
            {
                return NotFound("Gói tập không tồn tại.");
            }

            var plan = PlanInfo.FromGymPackage(package);
            model.Plan = plan;
            model.PackageId = package.PackageId;
            model.Cycle = model.Cycle == "year" ? "year" : "month";

            if (model.StartDate.HasValue && model.StartDate.Value.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.StartDate), "Ngày bắt đầu không được ở quá khứ");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AllPackages = allPackages;
                var allTrainers = await _context.Trainers.Include(t => t.User).ToListAsync();
                ViewBag.AllTrainers = allTrainers.Select(TrainerInfo.FromTrainer).ToList();
                return View("Index", model);
            }

            // 1. Tìm hoặc tạo hội viên (Member) trong Database
            Member? member = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int uid))
            {
                member = await _context.Members
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(m => m.UserId == uid);
            }

            if (member == null)
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email || u.PhoneNumber == model.Phone);

                if (existingUser != null)
                {
                    member = await _context.Members
                        .Include(m => m.User)
                        .FirstOrDefaultAsync(m => m.UserId == existingUser.UserId);

                    if (member == null)
                    {
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
                    // Tạo tài khoản User mới với mật khẩu mặc định (Member@123)
                    const string defaultPassHash = "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=";
                    var emailPrefix = model.Email.Split('@')[0];
                    var uniqueUsername = $"{emailPrefix}_{DateTime.UtcNow.Ticks.ToString()[^4..]}";

                    var newUser = new User
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                        Username = uniqueUsername,
                        PasswordHash = defaultPassHash,
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
            }

            // 2. Tạo bản ghi Thanh toán (Payment) và Gói đăng ký (Subscription)
            var durationDays = package.DurationDays;
            var totalAmount = package.Price;
            var orderId = $"ORDER_{DateTime.UtcNow.Ticks}";

            var payment = new Payment
            {
                MemberId = member.MemberId,
                PackageId = package.PackageId,
                Amount = totalAmount,
                PaymentMethod = model.PaymentMethod switch
                {
                    "vnpay" => "VNPay",
                    "bank" => "BankTransfer",
                    _ => "Cash"
                },
                TransactionCode = orderId,
                PaymentDate = DateTime.UtcNow,
                Status = model.PaymentMethod == "vnpay" ? "Pending" : "Completed"
            };
            _context.Payments.Add(payment);

            var subscription = new Subscription
            {
                MemberId = member.MemberId,
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
            TempData["Method"] = model.PaymentMethod switch
            {
                "vnpay" => "VNPay",
                "bank" => "Chuyển khoản ngân hàng",
                _ => "Thanh toán tại quầy"
            };

            return RedirectToAction(nameof(Success));
        }

        // GET: /thanh-toan/thanh-cong
        [HttpGet("thanh-toan/thanh-cong")]
        public IActionResult Success()
        {
            if (TempData["Plan"] == null)
            {
                return Redirect("~/");
            }
            return View();
        }
    }
}
