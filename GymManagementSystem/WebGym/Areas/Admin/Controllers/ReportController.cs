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
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;
            var selectedMonth = month;

            var query = _context.Payments
                .Where(p => p.Status == "Success" && p.PaymentDate.Year == selectedYear);

            if (selectedMonth.HasValue && selectedMonth > 0)
            {
                query = query.Where(p => p.PaymentDate.Month == selectedMonth.Value);
            }

            var transactions = await query
                .Include(p => p.Package)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            var report = new RevenueReportDto
            {
                Year = selectedYear,
                Month = selectedMonth,
                TotalRevenue = transactions.Sum(p => p.Amount),
                TotalTransactions = transactions.Count
            };

            ViewBag.ReportData = report;
            return View(transactions);
        }
    }
}
