using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Helpers;
using WebGym.Models.DTOs.Member;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Areas.Member.Controllers
{
    public class BookingController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public BookingController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: /Member/Booking
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Tự động hoàn thành các ca Approved của hội viên đã quá 24h
            var memberApprovedOverdue = await _context.Bookings
                .Where(b => b.MemberId == member.MemberId && b.Status == "Approved")
                .ToListAsync();

            bool memberHasChanges = false;
            foreach (var b in memberApprovedOverdue)
            {
                if (CoachUi.IsOver24Hours(b.BookingDate, b.TimeSlot))
                {
                    b.Status = "Completed";
                    var autoNote = "Hệ thống tự động xác nhận hoàn thành sau 24h";
                    b.Notes = string.IsNullOrWhiteSpace(b.Notes) ? autoNote : $"{b.Notes} | {autoNote}";
                    memberHasChanges = true;
                }
            }
            if (memberHasChanges)
            {
                await _context.SaveChangesAsync();
            }

            var myBookings = await _context.Bookings
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .Where(b => b.MemberId == member.MemberId)
                .OrderBy(b => b.Status == "Pending" ? 0 : (b.Status == "Approved" ? 1 : 2))
                .ThenByDescending(b => b.BookingId)
                .ToListAsync();

            ViewBag.Trainers = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            // Kiểm tra quyền lợi gói tập của Hội viên (Bước 3)
            var activeSub = await _context.Subscriptions
                .Include(s => s.Package)
                .Where(s => s.MemberId == member.MemberId && s.Status == "Active")
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefaultAsync();

            bool hasActiveSub = activeSub != null && activeSub.EndDate.Date >= DateTime.Today;
            bool hasPTPackage = hasActiveSub && (activeSub?.Package?.IncludesPT == true);

            ViewBag.ActiveSub = activeSub;
            ViewBag.HasActiveSub = hasActiveSub;
            ViewBag.HasPTPackage = hasPTPackage;
            ViewBag.PackageEndDate = activeSub?.EndDate.ToString("yyyy-MM-dd");

            return View(myBookings);
        }

        // POST: /Member/Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingDto model)
        {
            // 1. Kiểm tra tính hợp lệ dữ liệu đầu vào (ModelState)
            if (!ModelState.IsValid)
            {
                var errorMsg = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault(msg => !string.IsNullOrWhiteSpace(msg))
                    ?? "Dữ liệu đặt lịch không hợp lệ. Vui lòng kiểm tra lại.";
                SetErrorMessage(errorMsg);
                return RedirectToAction(nameof(Index));
            }

            // 2. Chặn đặt lịch vào các ngày trong quá khứ
            if (model.BookingDate.Date < DateTime.Today)
            {
                SetErrorMessage("Không thể đặt lịch vào các ngày trong quá khứ. Vui lòng chọn ngày từ hôm nay trở đi.");
                return RedirectToAction(nameof(Index));
            }

            // 3. Chặn đặt ca tập đã qua giờ trong ngày hôm nay
            if (model.BookingDate.Date == DateTime.Today)
            {
                if (TryGetTimeSlotStartTime(model.TimeSlot, out var slotStart))
                {
                    if (DateTime.Now.TimeOfDay >= slotStart)
                    {
                        SetErrorMessage($"Khung giờ {model.TimeSlot} hôm nay đã qua thời gian bắt đầu. Vui lòng chọn khung giờ tiếp theo.");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            var userId = CurrentUserId;
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy thông tin hội viên.");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // 4. KIỂM TRA ĐIỀU KIỆN GÓI TẬP (BƯỚC 3)
            var activeSub = await _context.Subscriptions
                .Include(s => s.Package)
                .Where(s => s.MemberId == member.MemberId && s.Status == "Active")
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefaultAsync();

            // 4.1. Chặn nếu chưa có gói hoặc gói đã hết hạn
            if (activeSub == null || activeSub.EndDate.Date < DateTime.Today)
            {
                SetErrorMessage("Gói tập của bạn đã hết hạn hoặc chưa kích hoạt. Vui lòng đăng ký/gia hạn gói tập để đặt lịch với HLV.");
                return RedirectToAction(nameof(Index));
            }

            // 4.2. Chặn nếu gói không bao gồm quyền lợi PT
            if (activeSub.Package?.IncludesPT != true)
            {
                SetErrorMessage($"Gói tập hiện tại của bạn ({activeSub.Package?.PackageName ?? "Chưa có"}) không bao gồm quyền lợi Huấn luyện viên (PT 1-1). Vui lòng nâng cấp lên Gói Vàng hoặc Gói Kim Cương.");
                return RedirectToAction(nameof(Index));
            }

            // 4.3. Chặn ngày đặt lịch vượt quá thời hạn gói tập
            if (model.BookingDate.Date > activeSub.EndDate.Date)
            {
                SetErrorMessage($"Ngày đặt lịch ({model.BookingDate:dd/MM/yyyy}) vượt quá thời hạn gói tập của bạn (hết hạn ngày {activeSub.EndDate:dd/MM/yyyy}). Vui lòng chọn ngày trong thời hạn gói.");
                return RedirectToAction(nameof(Index));
            }

            var trainer = await _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.TrainerId == model.TrainerId);
            if (trainer == null)
            {
                SetErrorMessage("Huấn luyện viên được chọn không tồn tại hoặc đã ngừng hoạt động.");
                return RedirectToAction(nameof(Index));
            }

            // 5. CHỐNG LỖI TRÙNG LỊCH (OVERBOOKING)

            // 5.1. Chặn Hội viên đặt trùng 2 ca trong cùng một khung giờ
            bool isMemberBooked = await _context.Bookings.AnyAsync(b =>
                b.MemberId == member.MemberId &&
                b.BookingDate.Date == model.BookingDate.Date &&
                b.TimeSlot == model.TimeSlot &&
                (b.Status == "Pending" || b.Status == "Approved" || b.Status == "Completed"));

            if (isMemberBooked)
            {
                SetErrorMessage($"Bạn đã có ca tập (hoặc đang chờ duyệt) vào khung giờ {model.TimeSlot} ngày {model.BookingDate:dd/MM/yyyy}. Không thể đặt trùng ca.");
                return RedirectToAction(nameof(Index));
            }

            // 5.2. Chặn Huấn luyện viên bị đặt trùng ca (mô hình kèm riêng 1-1)
            bool isTrainerBusy = await _context.Bookings.AnyAsync(b =>
                b.TrainerId == model.TrainerId &&
                b.BookingDate.Date == model.BookingDate.Date &&
                b.TimeSlot == model.TimeSlot &&
                (b.Status == "Pending" || b.Status == "Approved" || b.Status == "Completed"));

            if (isTrainerBusy)
            {
                SetErrorMessage($"HLV {trainer.User.FullName} đã có lịch dạy (hoặc đang có học viên khác chờ duyệt) vào khung giờ {model.TimeSlot} ngày {model.BookingDate:dd/MM/yyyy}. Vui lòng chọn khung giờ hoặc HLV khác.");
                return RedirectToAction(nameof(Index));
            }

            var booking = new Booking
            {
                MemberId = member.MemberId,
                TrainerId = model.TrainerId,
                BookingDate = model.BookingDate.Date,
                TimeSlot = model.TimeSlot,
                Status = "Pending",
                Notes = model.Notes
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Gửi email thông báo cho hội viên (an toàn, không làm gián đoạn đặt lịch)
            if (!string.IsNullOrEmpty(member.User.Email))
            {
                TryGetTimeSlotStartTime(model.TimeSlot, out var startTime);

                try
                {
                    _ = _emailService.SendBookingConfirmationAsync(
                        member.User.Email,
                        member.User.FullName,
                        trainer.User.FullName,
                        booking.BookingDate,
                        startTime
                    );
                }
                catch
                {
                    // Tránh lỗi gửi mail làm crash flow đặt lịch của hội viên
                }
            }

            SetSuccessMessage("Đặt lịch tập thành công! Vui lòng chờ HLV duyệt yêu cầu.");
            return RedirectToAction(nameof(Index));
        }

        // Helper: Phân tích thời gian bắt đầu của khung giờ tập
        private static bool TryGetTimeSlotStartTime(string? timeSlot, out TimeSpan startTime)
        {
            startTime = TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(timeSlot)) return false;

            var timePart = timeSlot.Split(new[] { '-', '|' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            return TimeSpan.TryParse(timePart, out startTime);
        }

        // POST: /Member/Booking/Cancel/5 (Bước 4: Hủy lịch hợp lệ)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = CurrentUserId;
            var booking = await _context.Bookings
                .Include(b => b.Member).ThenInclude(m => m.User)
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.Member.UserId == userId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập hoặc bạn không có quyền thao tác.");
                return RedirectToAction(nameof(Index));
            }

            // 1. Chặn hủy nếu ca tập đã hoàn thành
            if (booking.Status == "Completed")
            {
                SetErrorMessage("Không thể hủy ca tập đã hoàn thành.");
                return RedirectToAction(nameof(Index));
            }

            // 2. Chặn hủy nếu ca tập đã bị hủy trước đó
            if (booking.Status == "Cancelled")
            {
                SetErrorMessage("Ca tập này đã ở trạng thái đã hủy trước đó.");
                return RedirectToAction(nameof(Index));
            }

            // 3. Chỉ cho phép hủy ca tập đang ở trạng thái Chờ duyệt (Pending) hoặc Đã duyệt (Approved)
            if (booking.Status != "Pending" && booking.Status != "Approved")
            {
                SetErrorMessage("Chỉ có thể hủy ca tập đang chờ duyệt hoặc đã được duyệt.");
                return RedirectToAction(nameof(Index));
            }

            // 4. Chặn hủy các ca tập đã diễn ra trong quá khứ hoặc đã đến giờ bắt đầu
            var sessionDate = booking.BookingDate.Date;
            if (sessionDate < DateTime.Today)
            {
                SetErrorMessage($"Không thể hủy ca tập đã diễn ra trong quá khứ (ngày {sessionDate:dd/MM/yyyy}).");
                return RedirectToAction(nameof(Index));
            }

            if (sessionDate == DateTime.Today)
            {
                if (TryGetTimeSlotStartTime(booking.TimeSlot, out var startTime))
                {
                    var sessionStartDateTime = sessionDate.Add(startTime);
                    if (DateTime.Now >= sessionStartDateTime)
                    {
                        SetErrorMessage($"Không thể hủy ca tập đã đến hoặc đã qua thời gian bắt đầu ({booking.TimeSlot} hôm nay).");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            // 5. Cập nhật trạng thái ca tập thành Cancelled
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            var trainerName = booking.Trainer?.User?.FullName ?? "HLV";
            SetSuccessMessage($"Đã hủy thành công ca tập với HLV {trainerName} vào ngày {booking.BookingDate:dd/MM/yyyy} ({booking.TimeSlot}). Khung giờ này đã được mở lại.");
            return RedirectToAction(nameof(Index));
        }
    }
}
