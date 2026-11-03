using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;

namespace WebGym.Areas.Admin.Controllers
{
    public class ReportController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Report
        public async Task<IActionResult> Index(int? year, int? month, string? paymentMethod)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;
            var selectedMonth = month;

            var baseQuery = _context.Payments
                .Where(p => p.Status == "Success" && p.PaymentDate.Year == selectedYear);

            if (selectedMonth.HasValue && selectedMonth > 0)
            {
                baseQuery = baseQuery.Where(p => p.PaymentDate.Month == selectedMonth.Value);
            }

            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                baseQuery = baseQuery.Where(p => p.PaymentMethod == paymentMethod);
            }

            var transactions = await baseQuery
                .Include(p => p.Package)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            // 1. Phân tích doanh thu 12 tháng trong năm được chọn
            var monthlyBreakdown = new List<MonthlyRevenueItem>();
            for (int m = 1; m <= 12; m++)
            {
                var monthSum = await _context.Payments
                    .Where(p => p.Status == "Success" && p.PaymentDate.Year == selectedYear && p.PaymentDate.Month == m)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0;

                monthlyBreakdown.Add(new MonthlyRevenueItem
                {
                    MonthLabel = $"Tháng {m}",
                    Revenue = monthSum
                });
            }

            // 2. Phân tích theo phương thức thanh toán
            var methodBreakdown = transactions
                .GroupBy(t => t.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            // 3. Tìm gói tập bán chạy nhất
            var bestPkg = transactions
                .GroupBy(t => t.Package.PackageName)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "Chưa có";

            var report = new RevenueReportDto
            {
                Year = selectedYear,
                Month = selectedMonth,
                PaymentMethod = paymentMethod,
                TotalRevenue = transactions.Sum(p => p.Amount),
                TotalTransactions = transactions.Count,
                BestSellingPackage = bestPkg,
                MonthlyBreakdown = monthlyBreakdown,
                MethodBreakdown = methodBreakdown
            };

            ViewBag.ReportData = report;
            return View(transactions);
        }
    }
}
