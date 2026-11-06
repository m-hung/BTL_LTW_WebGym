using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Member;

namespace WebGym.Areas.Member.Controllers
{
    public class WorkoutScheduleController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public WorkoutScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/WorkoutSchedule (Trang GÓI TẬP - Yêu cầu 6)
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var activeSub = member.Subscriptions
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == "Active");

            var vm = new MemberPackageViewModel
            {
                MemberId = member.MemberId,
                MemberName = member.User.FullName,
                CardNumber = $"MB-{DateTime.Now.Year} •••• •••• {member.MemberId:D4}",
                HasActivePackage = activeSub != null,
                PackageName = activeSub?.Package.PackageName ?? "Chưa đăng ký gói",
                PackageTier = (activeSub?.Package.PackageName.Contains("VIP", StringComparison.OrdinalIgnoreCase) == true) ? "VIP DIAMOND" : "MEMBER TIER",
                Status = activeSub != null ? "Active" : "None",
                StartDate = activeSub?.StartDate,
                EndDate = activeSub?.EndDate,
                Price = activeSub != null ? (activeSub.PaidAmount > 0 ? activeSub.PaidAmount : activeSub.Package.Price) : 0,
                RemainingDays = activeSub != null ? Math.Max(0, (int)(activeSub.EndDate.Date - DateTime.Today).TotalDays) : 0
            };

            // Lịch sử đăng ký giao dịch gói tập
            if (member.Subscriptions.Any())
            {
                foreach (var sub in member.Subscriptions.OrderByDescending(s => s.StartDate))
                {
                    vm.History.Add(new PackageHistoryItemDto
                    {
                        InvoiceCode = $"#HD-{sub.StartDate.Year}-{sub.SubscriptionId:D4}",
                        PackageName = sub.Package?.PackageName ?? "Gói tập Pulse Gym",
                        StartDate = sub.StartDate,
                        EndDate = sub.EndDate,
                        Amount = sub.PaidAmount > 0 ? sub.PaidAmount : (sub.Package?.Price ?? 0),
                        Status = sub.Status == "Active" ? "Đã thanh toán" : (sub.Status == "Expired" ? "Đã hoàn thành" : "Đã hủy")
                    });
                }
            }
            else
            {
                // Nếu chưa có giao dịch, tạo dòng mẫu trực quan theo hình ảnh của Leader
                vm.History.Add(new PackageHistoryItemDto
                {
                    InvoiceCode = $"#HD-{DateTime.Now.Year}-0001",
                    PackageName = "Gói Tập Thử Nghiệm 1 Tháng",
                    StartDate = DateTime.Today.AddDays(-30),
                    EndDate = DateTime.Today,
                    Amount = 500000,
                    Status = "Đã hoàn thành"
                });
            }

            return View(vm);
        }
    }
}
