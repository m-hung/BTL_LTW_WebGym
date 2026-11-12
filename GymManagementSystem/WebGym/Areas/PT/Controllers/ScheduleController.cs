using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Trainer;

namespace WebGym.Areas.PT.Controllers
{
    public class ScheduleController : PTBaseController
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /PT/Schedule
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy thông tin HLV.");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Bước 5: Tự động chuyển các ca Approved đã quá 24h kể từ thời gian tập thành Hoàn thành (Completed)
            await AutoCompleteOverdueSessionsAsync(trainer.TrainerId);

            var schedules = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Where(b => b.TrainerId == trainer.TrainerId)
                .OrderBy(b => b.BookingDate)
                .Select(b => new TrainerScheduleDto
                {
                    BookingId = b.BookingId,
                    MemberName = b.Member.User.FullName,
                    MemberPhone = b.Member.User.PhoneNumber,
                    BookingDate = b.BookingDate,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    Notes = b.Notes
                })
                .ToListAsync();

            return View(schedules);
        }

        // POST: /PT/Schedule/Complete/5 (Bước 5: PT xác nhận hoàn thành ca dạy trong cửa sổ 24h)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy thông tin HLV.");
                return RedirectToAction(nameof(Index));
            }

            var booking = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.TrainerId == trainer.TrainerId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập hoặc bạn không có quyền thao tác.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status == "Completed")
            {
                SetErrorMessage("Ca tập này đã được xác nhận hoàn thành trước đó.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status == "Cancelled")
            {
                SetErrorMessage("Không thể cập nhật hoàn thành cho ca tập đã bị hủy.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status != "Approved")
            {
                SetErrorMessage("Chỉ có thể xác nhận hoàn thành cho các ca tập đã được duyệt.");
                return RedirectToAction(nameof(Index));
            }

            var startAt = CoachUi.StartAt(booking.BookingDate, booking.TimeSlot);
            var now = DateTime.Now;

            // 1. Chặn nếu ca tập chưa tới thời gian tập
            if (now < startAt)
            {
                SetErrorMessage($"Ca tập chưa tới thời gian bắt đầu (dự kiến lúc {startAt:HH:mm dd/MM/yyyy}). Vui lòng chỉ xác nhận sau khi ca tập bắt đầu.");
                return RedirectToAction(nameof(Index));
            }

            // 2. Kiểm tra nếu đã quá hạn 24h -> Tự động chốt
            if (CoachUi.IsOver24Hours(booking.BookingDate, booking.TimeSlot))
            {
                booking.Status = "Completed";
                var autoNote = "Hệ thống tự động xác nhận hoàn thành sau 24h";
                booking.Notes = string.IsNullOrWhiteSpace(booking.Notes) ? autoNote : $"{booking.Notes} | {autoNote}";
                await _context.SaveChangesAsync();

                SetErrorMessage("Đã quá thời hạn 24h kể từ khi bắt đầu ca tập. Hệ thống đã tự động ghi nhận hoàn thành.");
                return RedirectToAction(nameof(Index));
            }

            // 3. Hợp lệ trong cửa sổ 24h: Chuyển trạng thái sang Completed
            booking.Status = "Completed";
            await _context.SaveChangesAsync();

            var memberName = booking.Member?.User?.FullName ?? "Học viên";
            SetSuccessMessage($"Đã xác nhận hoàn thành ca tập với học viên {memberName} ({booking.BookingDate:dd/MM/yyyy} {CoachUi.SlotText(booking.TimeSlot)}) thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /PT/Schedule/Cancel/5 (Bước 5: PT báo vắng học viên hoặc hủy ca dạy)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? reason)
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy thông tin HLV.");
                return RedirectToAction(nameof(Index));
            }

            var booking = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.TrainerId == trainer.TrainerId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập hoặc bạn không có quyền thao tác.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status == "Completed")
            {
                SetErrorMessage("Không thể hủy ca tập đã hoàn thành.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status == "Cancelled")
            {
                SetErrorMessage("Ca tập này đã ở trạng thái đã hủy trước đó.");
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status != "Approved")
            {
                SetErrorMessage("Chỉ có thể thao tác hủy/báo vắng với các ca tập đã được duyệt.");
                return RedirectToAction(nameof(Index));
            }

            var startAt = CoachUi.StartAt(booking.BookingDate, booking.TimeSlot);
            var now = DateTime.Now;

            // Kiểm tra quá hạn 24h
            if (CoachUi.IsOver24Hours(booking.BookingDate, booking.TimeSlot))
            {
                booking.Status = "Completed";
                var autoNote = "Hệ thống tự động xác nhận hoàn thành sau 24h";
                booking.Notes = string.IsNullOrWhiteSpace(booking.Notes) ? autoNote : $"{booking.Notes} | {autoNote}";
                await _context.SaveChangesAsync();

                SetErrorMessage("Đã quá thời hạn 24h kể từ thời gian tập. Ca tập đã được hệ thống tự động chốt hoàn thành, không thể hủy.");
                return RedirectToAction(nameof(Index));
            }

            // Cập nhật trạng thái thành Cancelled kèm ghi chú lý do
            booking.Status = "Cancelled";
            var cleanReason = string.IsNullOrWhiteSpace(reason)
                ? (now >= startAt ? "Học viên vắng mặt (HLV báo vắng)" : "HLV hủy ca tập")
                : (now >= startAt ? $"HLV báo vắng: {reason.Trim()}" : $"HLV hủy ca: {reason.Trim()}");

            booking.Notes = string.IsNullOrWhiteSpace(booking.Notes)
                ? cleanReason
                : $"{booking.Notes} | {cleanReason}";

            await _context.SaveChangesAsync();

            var memberName = booking.Member?.User?.FullName ?? "Học viên";
            SetSuccessMessage($"Đã cập nhật hủy ca tập / báo vắng đối với học viên {memberName} ({booking.BookingDate:dd/MM/yyyy} {CoachUi.SlotText(booking.TimeSlot)}).");
            return RedirectToAction(nameof(Index));
        }

        // Helper: Tự động quét và hoàn thành các ca Approved đã quá 24h
        private async Task AutoCompleteOverdueSessionsAsync(int trainerId)
        {
            var approvedList = await _context.Bookings
                .Where(b => b.TrainerId == trainerId && b.Status == "Approved")
                .ToListAsync();

            bool hasChanges = false;
            foreach (var b in approvedList)
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
        }
    }
}