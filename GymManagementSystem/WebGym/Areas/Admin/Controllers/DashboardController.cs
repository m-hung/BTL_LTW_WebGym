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

            // === 4 WIDGET MỚI (PHÂN HỆ 6) ===
            // Widget 1: Doanh thu Bán hàng Shop (Orders)
            var validOrderStatuses = new[] { "Hoàn thành", "Đã thanh toán" };
            var shopRevenueThisMonth = await _context.Orders
                .Where(o => validOrderStatuses.Contains(o.Status) && o.OrderDate >= startOfMonth)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var shopRevenueOverall = await _context.Orders
                .Where(o => validOrderStatuses.Contains(o.Status))
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var shopOrdersThisMonth = await _context.Orders
                .CountAsync(o => validOrderStatuses.Contains(o.Status) && o.OrderDate >= startOfMonth);

            var shopOrdersOverall = await _context.Orders
                .CountAsync(o => validOrderStatuses.Contains(o.Status));

            // Widget 2: Tỷ lệ lấp đầy Lớp học nhóm
            var activeClasses = await _context.GymClasses
                .Include(c => c.ClassBookings)
                .Where(c => c.Status != "Cancelled")
                .ToListAsync();

            int activeClassesCount = activeClasses.Count;
            int totalClassSlots = activeClasses.Sum(c => c.MaxCapacity);
            int bookedClassSlots = activeClasses.Sum(c => c.ClassBookings.Count(cb => cb.Status != "Cancelled"));
            double classFillRate = totalClassSlots > 0 ? Math.Round(bookedClassSlots * 100.0 / totalClassSlots, 1) : 0;

            // Widget 3: Cảnh báo thiết bị hỏng / cần bảo trì
            var brokenEquipmentCount = await _context.Equipments.CountAsync(e => e.Status == "Đang hỏng");
            var needsMaintenanceCount = await _context.Equipments.CountAsync(e => e.Status == "Cần bảo trì");

            // Widget 4: Cảnh báo hàng tồn kho (< 5 cái)
            var lowStockProductsCount = await _context.Products.CountAsync(p => p.StockQuantity > 0 && p.StockQuantity < 5);
            var outOfStockProductsCount = await _context.Products.CountAsync(p => p.StockQuantity <= 0);

            // 2. Dữ liệu biểu đồ Doanh thu 6 tháng gần nhất (Song song Gói tập vs Shop POS)
            var monthlyRevenueList = new List<MonthlyRevenueItem>();
            for (int i = 5; i >= 0; i--)
            {
                var targetDate = now.AddMonths(-i);
                var monthStart = new DateTime(targetDate.Year, targetDate.Month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var pkgSum = await _context.Payments
                    .Where(p => p.Status == "Success" && p.PaymentDate >= monthStart && p.PaymentDate < monthEnd)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0;

                var shopSum = await _context.Orders
                    .Where(o => validOrderStatuses.Contains(o.Status) && o.OrderDate >= monthStart && o.OrderDate < monthEnd)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

                var maintSum = await _context.MaintenanceLogs
                    .Where(m => m.MaintenanceDate >= monthStart && m.MaintenanceDate < monthEnd)
                    .SumAsync(m => (decimal?)m.Cost) ?? 0;

                monthlyRevenueList.Add(new MonthlyRevenueItem
                {
                    MonthLabel = $"T{targetDate.Month}/{targetDate.Year}",
                    Revenue = pkgSum + shopSum,
                    PackageRevenue = pkgSum,
                    ShopRevenue = shopSum,
                    MaintenanceCost = maintSum
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

            // 5. Giao dịch thanh toán gói tập gần nhất (5 mục)
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

            // 6. Đơn hàng Shop / POS gần nhất (5 mục)
            var recentShopOrders = await _context.Orders
                .Include(o => o.Member).ThenInclude(m => m!.User)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentShopOrderItem
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Member != null
                        ? o.Member.User.FullName
                        : (o.User != null ? o.User.FullName : "Khách vãng lai"),
                    ItemCount = o.OrderDetails.Sum(od => od.Quantity),
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderDate = o.OrderDate,
                    Status = o.Status
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
                ShopRevenueThisMonth = shopRevenueThisMonth,
                ShopRevenueOverall = shopRevenueOverall,
                ShopOrdersThisMonth = shopOrdersThisMonth,
                ShopOrdersOverall = shopOrdersOverall,
                ClassFillRatePercent = classFillRate,
                TotalClassSlots = totalClassSlots,
                BookedClassSlots = bookedClassSlots,
                ActiveClassesCount = activeClassesCount,
                BrokenEquipmentCount = brokenEquipmentCount,
                NeedsMaintenanceEquipmentCount = needsMaintenanceCount,
                LowStockProductsCount = lowStockProductsCount,
                OutOfStockProductsCount = outOfStockProductsCount,
                MonthlyRevenueChart = monthlyRevenueList,
                PackageDistributionChart = packageDistribution,
                RecentBookings = recentBookings,
                RecentPayments = recentPayments,
                RecentShopOrders = recentShopOrders
            };

            return View(stats);
        }
    }
}
