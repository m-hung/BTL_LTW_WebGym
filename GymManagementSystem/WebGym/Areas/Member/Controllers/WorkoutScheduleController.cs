using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Member;
using WebGym.Models.Entities;

namespace WebGym.Areas.Member.Controllers
{
    public class WorkoutScheduleController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public WorkoutScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/WorkoutSchedule (Trang GÓI TẬP)
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

            // Lấy gói tập đang hoạt động mới nhất của hội viên (hoặc gói đã mua gần nhất)
            var activeSub = member.Subscriptions
                .Where(s => s.Status == "Active" && s.EndDate >= DateTime.Today)
                .OrderByDescending(s => s.StartDate)
                .ThenByDescending(s => s.SubscriptionId)
                .FirstOrDefault()
                ?? member.Subscriptions
                .OrderByDescending(s => s.StartDate)
                .ThenByDescending(s => s.SubscriptionId)
                .FirstOrDefault();

            string tier = "MEMBER TIER";
            if (activeSub != null && activeSub.Package != null)
            {
                if (activeSub.Package.PackageName.Contains("Kim Cương", StringComparison.OrdinalIgnoreCase))
                    tier = "VIP DIAMOND";
                else if (activeSub.Package.PackageName.Contains("Vàng", StringComparison.OrdinalIgnoreCase))
                    tier = "VIP GOLD";
                else if (activeSub.Package.PackageName.Contains("Bạc", StringComparison.OrdinalIgnoreCase))
                    tier = "STANDARD";
                else if (activeSub.Package.PackageName.Contains("Đồng", StringComparison.OrdinalIgnoreCase))
                    tier = "BASIC MEMBER";
            }

            var currentPkgPrice = activeSub != null ? (activeSub.PaidAmount > 0 ? activeSub.PaidAmount : (activeSub.Package?.Price ?? 500000m)) : 0m;

            var vm = new MemberPackageViewModel
            {
                MemberId = member.MemberId,
                PackageId = activeSub?.PackageId,
                DurationDays = activeSub?.Package?.DurationDays ?? 30,
                MemberName = member.User.FullName,
                CardNumber = $"MB-{DateTime.Now.Year} •••• •••• {member.MemberId:D4}",
                HasActivePackage = activeSub != null,
                PackageName = activeSub?.Package?.PackageName ?? "Chưa đăng ký gói",
                PackageTier = tier,
                Status = activeSub != null ? "Active" : "None",
                StartDate = activeSub?.StartDate,
                EndDate = activeSub?.EndDate,
                Price = currentPkgPrice,
                RemainingDays = activeSub != null ? Math.Max(0, (int)(activeSub.EndDate.Date - DateTime.Today).TotalDays) : 0
            };

            // Lịch sử đăng ký giao dịch gói tập (chỉ lấy các giao dịch hợp lệ: Đã thanh toán hoặc hoàn thành)
            var validSubs = member.Subscriptions
                .Where(s => s.Status == "Active" || s.Status == "Expired")
                .OrderByDescending(s => s.StartDate);

            foreach (var sub in validSubs)
            {
                vm.History.Add(new PackageHistoryItemDto
                {
                    InvoiceCode = $"#HD-{sub.StartDate.Year}-{sub.SubscriptionId:D4}",
                    PackageName = sub.Package?.PackageName ?? "Gói tập Pulse Gym",
                    StartDate = sub.StartDate,
                    EndDate = sub.EndDate,
                    Amount = sub.PaidAmount > 0 ? sub.PaidAmount : (sub.Package?.Price ?? 500000m),
                    Status = sub.Status == "Active" ? "Đã thanh toán" : "Đã hoàn thành"
                });
            }

            if (!vm.History.Any())
            {
                vm.History.Add(new PackageHistoryItemDto
                {
                    InvoiceCode = $"#HD-2026-0004",
                    PackageName = "Gói Đồng (Basic 1 Tháng)",
                    StartDate = new DateTime(2026, 11, 1),
                    EndDate = new DateTime(2026, 12, 1),
                    Amount = 500000,
                    Status = "Đã thanh toán"
                });
            }

            ViewBag.CurrentPackagePrice = currentPkgPrice;
            ViewBag.ActivePackageId = activeSub?.PackageId ?? 1;
            ViewBag.AvailablePackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            return View(vm);
        }
    }
}
