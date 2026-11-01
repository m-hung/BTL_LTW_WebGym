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

            var stats = new DashboardStatsDto
            {
                TotalMembers = await _context.Members.CountAsync(),
                TotalTrainers = await _context.Trainers.CountAsync(),
                TotalActiveBookings = await _context.Bookings.CountAsync(b => b.Status == "Approved"),
                TotalRevenueThisMonth = await _context.Payments
                    .Where(p => p.Status == "Success" && p.PaymentDate >= startOfMonth)
                    .SumAsync(p => (decimal?)p.Amount) ?? 0,
                TotalRevenueOverall = await _context.Payments
                    .Where(p => p.Status == "Success")
                    .SumAsync(p => (decimal?)p.Amount) ?? 0,
                RecentBookings = await _context.Bookings
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
                        Status = b.Status
                    })
                    .ToListAsync()
            };

            return View(stats);
        }
    }
}
