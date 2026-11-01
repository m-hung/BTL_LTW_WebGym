using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Payment;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            ApplicationDbContext context,
            IPaymentService paymentService,
            IEmailService emailService,
            ILogger<PaymentController> logger)
        {
            _context = context;
            _paymentService = paymentService;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: /Payment/Checkout/5
        public async Task<IActionResult> Checkout(int packageId)
        {
            var package = await _context.GymPackages.FindAsync(packageId);
            if (package == null)
            {
                SetErrorMessage("Gói tập không tồn tại");
                return RedirectToAction("Packages", "Home");
            }

            var dto = new PaymentRequestDto
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Amount = package.Price,
                OrderDescription = $"Đăng ký gói tập {package.PackageName} tại WebGym"
            };

            return View(dto);
        }

        // POST: /Payment/CreateVnPayPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVnPayPayment(PaymentRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            var currentUserId = CurrentUserId;
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == currentUserId);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy thông tin hội viên");
                return RedirectToAction("Index", "Home");
            }

            // Tạo link thanh toán VNPay
            var paymentResult = await _paymentService.CreatePaymentUrlAsync(model, HttpContext);

            if (!paymentResult.Success)
            {
                SetErrorMessage(paymentResult.Message);
                return View("Checkout", model);
            }

            // Lưu Payment Record ở trạng thái Pending/Initiated
            var payment = new Payment
            {
                MemberId = member.MemberId,
                PackageId = model.PackageId,
                Amount = model.Amount,
                PaymentMethod = "VNPay",
                TransactionCode = paymentResult.OrderId ?? $"VNP_{DateTime.UtcNow.Ticks}",
                PaymentDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Redirect(paymentResult.PaymentUrl);
        }

        // GET: /Payment/VnPayCallback
        [AllowAnonymous]
        public async Task<IActionResult> VnPayCallback(VnPayCallbackDto callbackDto)
        {
            var isSuccess = await _paymentService.ProcessCallbackAsync(callbackDto);

            var payment = await _context.Payments
                .Include(p => p.Package)
                .Include(p => p.Member)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(p => p.TransactionCode == callbackDto.vnp_TxnRef);

            if (payment == null)
            {
                ViewBag.Message = "Không tìm thấy thông tin đơn hàng!";
                return View("PaymentFailed");
            }

            if (isSuccess)
            {
                payment.Status = "Success";
                payment.TransactionCode = callbackDto.vnp_TransactionNo ?? payment.TransactionCode;
                payment.PaymentDate = DateTime.UtcNow;

                // Tạo mới hoặc kích hoạt Subscription cho hội viên
                var subscription = new Subscription
                {
                    MemberId = payment.MemberId,
                    PackageId = payment.PackageId,
                    OriginalPrice = payment.Package.Price,
                    PaidAmount = payment.Amount,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(payment.Package.DurationDays),
                    Status = "Active"
                };
                _context.Subscriptions.Add(subscription);

                await _context.SaveChangesAsync();

                // Gửi email xác nhận
                if (!string.IsNullOrEmpty(payment.Member.User.Email))
                {
                    _ = _emailService.SendPaymentConfirmationAsync(
                        payment.Member.User.Email,
                        payment.Member.User.FullName,
                        payment.Package.PackageName,
                        payment.Amount,
                        payment.TransactionCode
                    );
                }

                return View("PaymentSuccess", payment);
            }
            else
            {
                payment.Status = "Failed";
                await _context.SaveChangesAsync();
                return View("PaymentFailed");
            }
        }
    }
}
