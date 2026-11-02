using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;

namespace WebGym.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Index()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            // 1. Thống kê cơ bản
            var totalMembers = await _context.Members.CountAsync();
            var totalTrainers = await _context.Trainers.CountAsync();
            var totalActiveBookings = await _context.Bookings.CountAsync(b => b.Status == "Approved" || b.Status == "Pending");
            var totalActiveSubscriptions = await _context.Subscriptions.CountAsync(s => s.Status == "Active" && s.EndDate >= now);

            var revenueThisMonth = await _context.Payments
                .Where(p => p.Status == "Success" && p.PaymentDate >= startOfMonth)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var revenueOverall = await _context.Payments
                .Where(p => p.Status == "Success")
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // 2. Dữ liệu biểu đồ Doanh thu 6 tháng gần nhất
            var monthlyRevenueList = new List<MonthlyRevenueItem>();
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = now.AddMonths(-i);
                var monthStart = new DateTime(targetDate.Year, targetDate.Month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var sumMonth = await _context.Payments
                    .Where(p => p.Status == "Success" && p.PaymentDate >= monthStart && p.PaymentDate < monthEnd)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0;

                monthlyRevenueList.Add(new MonthlyRevenueItem
                {
                    MonthLabel = $"Tháng {targetDate.Month}/{targetDate.Year}",
                    Revenue = sumMonth
                });
            }

            // 3. Dữ liệu biểu đồ phân bổ gói tập
            var packageDistribution = await _context.Subscriptions
                .Include(s => s.Package)
                .GroupBy(s => s.Package.PackageName)
                .Select(g => new PackageDistributionItem
                {
                    PackageName = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // 4. Lịch hẹn tập gần nhất (5 mục)
            var recentBookings = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .Select(b => new RecentBookingItem
                {
                    BookingId = b.BookingId,
                    MemberName = b.Member.User.FullName,
                    TrainerName = b.Trainer.User.FullName,
                    BookingDate = b.BookingDate,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status
                })
                .ToListAsync();

            // 5. Giao dịch thanh toán gần nhất (5 mục)
            var recentPayments = await _context.Payments
                .Include(p => p.Member).ThenInclude(m => m.User)
                .Include(p => p.Package)
                .OrderByDescending(p => p.PaymentDate)
                .Take(5)
                .Select(p => new RecentPaymentItem
                {
                    PaymentId = p.PaymentId,
                    MemberName = p.Member.User.FullName,
                    PackageName = p.Package.PackageName,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    Status = p.Status
                })
                .ToListAsync();

            var stats = new DashboardStatsDto
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                TotalActiveBookings = totalActiveBookings,
                TotalActiveSubscriptions = totalActiveSubscriptions,
                TotalRevenueThisMonth = revenueThisMonth,
                TotalRevenueOverall = revenueOverall,
                MonthlyRevenueChart = monthlyRevenueList,
                PackageDistributionChart = packageDistribution,
                RecentBookings = recentBookings,
                RecentPayments = recentPayments
            };

            return View(stats);
        }
    }
}
