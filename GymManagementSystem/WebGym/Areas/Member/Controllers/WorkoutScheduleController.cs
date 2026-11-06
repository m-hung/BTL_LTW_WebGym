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

            // Tự động đồng bộ nếu Member 1 đang gán Gói Đồng (Basic) nhưng số tiền hoặc thời gian còn dính Gói Vàng cũ
            if (activeSub != null && activeSub.PackageId == 1 && (activeSub.PaidAmount != 500000m || (activeSub.EndDate - activeSub.StartDate).TotalDays > 35))
            {
                activeSub.OriginalPrice = 500000m;
                activeSub.PaidAmount = 500000m;
                activeSub.StartDate = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc);
                activeSub.EndDate = new DateTime(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc);
                await _context.SaveChangesAsync();
                await _context.Entry(activeSub).Reference(s => s.Package).LoadAsync();
            }

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
                Price = activeSub != null ? (activeSub.PaidAmount > 0 ? activeSub.PaidAmount : (activeSub.Package?.Price ?? 500000m)) : 0,
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
                        Amount = sub.PaidAmount > 0 ? sub.PaidAmount : (sub.Package?.Price ?? 500000m),
                        Status = sub.Status == "Active" ? "Đã thanh toán" : (sub.Status == "Expired" ? "Đã hoàn thành" : "Đã hủy")
                    });
                }
            }
            else
            {
                vm.History.Add(new PackageHistoryItemDto
                {
                    InvoiceCode = $"#HD-{DateTime.Now.Year}-0001",
                    PackageName = "Gói Đồng (Basic 1 Tháng)",
                    StartDate = new DateTime(2026, 11, 1),
                    EndDate = new DateTime(2026, 12, 1),
                    Amount = 500000,
                    Status = "Đã thanh toán"
                });
            }

            ViewBag.AvailablePackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            return View(vm);
        }
    }
}
