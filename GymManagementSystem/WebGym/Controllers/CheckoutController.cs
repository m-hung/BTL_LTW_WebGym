using Microsoft.AspNetCore.Mvc;
using WebGym.Models;

namespace WebGym.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: /thanh-toan/pro?cycle=year
        [HttpGet("thanh-toan/{slug}")]
        public IActionResult Index(string slug, string cycle = "month")
        {
            var plan = PlanData.Find(slug);
            if (plan == null) return NotFound();

            return View(new CheckoutViewModel { Plan = plan, Cycle = cycle == "year" ? "year" : "month" });
        }

        // POST: /thanh-toan/pro
        [HttpPost("thanh-toan/{slug}")]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(string slug, CheckoutViewModel model)
        {
            var plan = PlanData.Find(slug);
            if (plan == null) return NotFound();
            model.Plan = plan;
            model.Cycle = model.Cycle == "year" ? "year" : "month";

            if (model.StartDate.HasValue && model.StartDate.Value.Date < DateTime.Today)
                ModelState.AddModelError(nameof(model.StartDate), "Ngày bắt đầu không được ở quá khứ");

            if (!ModelState.IsValid)
                return View("Index", model);

            // TODO: lưu đơn đăng ký vào database (member, gói, chu kỳ, ngày bắt đầu).
            // TODO: nếu model.PaymentMethod == "vnpay", gọi IPaymentService (VnPayPaymentService)
            //       để tạo link thanh toán với model.Total rồi Redirect(urlVnPay).
            // TODO: gửi email xác nhận qua IEmailService.

            TempData["Plan"] = plan.Name;
            TempData["Cycle"] = model.IsYearly ? "Hàng năm" : "Hàng tháng";
            TempData["Total"] = CheckoutViewModel.Vnd(model.Total);
            TempData["Email"] = model.Email;
            TempData["Name"] = model.FullName;
            TempData["Start"] = model.StartDate?.ToString("dd/MM/yyyy");
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
            if (TempData["Plan"] == null) return Redirect("~/");
            return View();
        }
    }
}
