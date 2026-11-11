using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;

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
        public async Task<IActionResult> Index(string? statusFilter, int? trainerId, DateTime? bookingDate)
        {
            // Tự động chuyển các ca Approved đã quá 24h thành Completed
            var approvedOverdue = await _context.Bookings
                .Where(b => b.Status == "Approved")
                .ToListAsync();

            bool hasChanges = false;
            foreach (var b in approvedOverdue)
            {
                if (CoachUi.IsOver24Hours(b.BookingDate, b.TimeSlot))
                {
                    b.Status = "Completed";
                    var autoNote = "Hệ thống tự động xác nhận hoàn thành sau 24h";
                    b.Notes = string.IsNullOrWhiteSpace(b.Notes) ? autoNote : $"{b.Notes} | {autoNote}";
                    hasChanges = true;
                }
            }
            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }

            var query = _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(b => b.Status == statusFilter);
            }

            if (trainerId.HasValue && trainerId > 0)
            {
                query = query.Where(b => b.TrainerId == trainerId.Value);
            }

            if (bookingDate.HasValue)
            {
                var d = bookingDate.Value.Date;
                query = query.Where(b => b.BookingDate.Date == d);
            }

            var bookings = await query.OrderByDescending(b => b.BookingDate).ToListAsync();

            // Thống kê nhanh
            ViewBag.TotalPending = await _context.Bookings.CountAsync(b => b.Status == "Pending");
            ViewBag.TotalApproved = await _context.Bookings.CountAsync(b => b.Status == "Approved");
            ViewBag.TotalCompleted = await _context.Bookings.CountAsync(b => b.Status == "Completed");

            ViewBag.Trainers = new SelectList(await _context.Trainers.Include(t => t.User).Select(t => new { t.TrainerId, t.User.FullName }).ToListAsync(), "TrainerId", "FullName", trainerId);
            ViewBag.StatusFilter = statusFilter;
            ViewBag.SelectedDate = bookingDate?.ToString("yyyy-MM-dd");

            return View(bookings);
        }

        // POST: /Admin/BookingManagement/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var booking = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập.");
                return RedirectToAction(nameof(Index));
            }

            // Chống trùng lịch: Nếu Admin duyệt ca tập, kiểm tra HLV đã có ca nào khác đã duyệt chưa
            if (status == "Approved")
            {
                bool hasConflict = await _context.Bookings.AnyAsync(b =>
                    b.BookingId != id &&
                    b.TrainerId == booking.TrainerId &&
                    b.BookingDate.Date == booking.BookingDate.Date &&
                    b.TimeSlot == booking.TimeSlot &&
                    b.Status == "Approved");

                if (hasConflict)
                {
                    SetErrorMessage($"HLV này đã có ca dạy khác đã duyệt vào khung giờ {booking.TimeSlot} ngày {booking.BookingDate:dd/MM/yyyy}. Không thể duyệt trùng.");
                    return RedirectToAction(nameof(Index));
                }
            }

            // Chặn hủy ca tập đã hoàn thành
            if (status == "Cancelled" && booking.Status == "Completed")
            {
                SetErrorMessage("Không thể hủy ca tập đã hoàn thành.");
                return RedirectToAction(nameof(Index));
            }

            booking.Status = status;
            await _context.SaveChangesAsync();

            var statusText = status switch
            {
                "Approved" => "Đã duyệt",
                "Cancelled" => "Đã hủy",
                "Completed" => "Đã hoàn thành",
                _ => status
            };

            SetSuccessMessage($"Đã cập nhật lịch tập của {booking.Member.User.FullName} sang trạng thái: {statusText}.");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/BookingManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy lịch tập cần xóa.");
                return RedirectToAction(nameof(Index));
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa lịch tập #{id} thành công.");
            return RedirectToAction(nameof(Index));
        }
    }
}
