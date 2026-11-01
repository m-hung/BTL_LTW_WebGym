using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;

namespace WebGym.Areas.Admin.Controllers
{
    public class BookingManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public BookingManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/BookingManagement
        public async Task<IActionResult> Index(string? statusFilter)
        {
            var query = _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(b => b.Status == statusFilter);
            }

            var bookings = await query.OrderByDescending(b => b.BookingDate).ToListAsync();
            ViewBag.StatusFilter = statusFilter;

            return View(bookings);
        }

        // POST: /Admin/BookingManagement/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập.");
                return RedirectToAction(nameof(Index));
            }

            booking.Status = status;
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã cập nhật trạng thái lịch tập #{id} thành {status}.");
            return RedirectToAction(nameof(Index));
        }
    }
}
