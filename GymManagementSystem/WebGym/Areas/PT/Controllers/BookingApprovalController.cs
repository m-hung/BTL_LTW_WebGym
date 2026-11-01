using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;

namespace WebGym.Areas.PT.Controllers
{
    public class BookingApprovalController : PTBaseController
    {
        private readonly ApplicationDbContext _context;

        public BookingApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /PT/BookingApproval
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var pendingBookings = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Where(b => b.TrainerId == trainer.TrainerId && b.Status == "Pending")
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            return View(pendingBookings);
        }

        // POST: /PT/BookingApproval/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var userId = CurrentUserId;
            var booking = await _context.Bookings
                .Include(b => b.Trainer)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.Trainer.UserId == userId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy ca tập hoặc bạn không có quyền duyệt.");
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Approved";
            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã duyệt lịch hẹn tập thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /PT/BookingApproval/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string? reason)
        {
            var userId = CurrentUserId;
            var booking = await _context.Bookings
                .Include(b => b.Trainer)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.Trainer.UserId == userId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy ca tập.");
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Cancelled";
            booking.Notes = reason ?? "HLV bận vào khung giờ này.";
            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã từ chối lịch hẹn.");
            return RedirectToAction(nameof(Index));
        }
    }
}
