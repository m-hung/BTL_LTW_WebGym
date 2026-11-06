using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
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

            var myBookings = await _context.Bookings
                .Include(b => b.Trainer).ThenInclude(t => t.User)
                .Where(b => b.MemberId == member.MemberId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.Trainers = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            return View(myBookings);
        }

        // POST: /Member/Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingDto model)
        {
            var userId = CurrentUserId;
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var trainer = await _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.TrainerId == model.TrainerId);
            if (trainer == null)
            {
                SetErrorMessage("Huấn luyện viên không tồn tại.");
                return RedirectToAction(nameof(Index));
            }

            var booking = new Booking
            {
                MemberId = member.MemberId,
                TrainerId = model.TrainerId,
                BookingDate = model.BookingDate,
                TimeSlot = model.TimeSlot,
                Status = "Pending",
                Notes = model.Notes
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Gửi email thông báo cho hội viên (an toàn, không làm gián đoạn đặt lịch)
            if (!string.IsNullOrEmpty(member.User.Email))
            {
                TimeSpan startTime = TimeSpan.Zero;
                if (!string.IsNullOrEmpty(model.TimeSlot))
                {
                    var timePart = model.TimeSlot.Split(new[] { '-', '|' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    if (TimeSpan.TryParse(timePart, out var parsed))
                    {
                        startTime = parsed;
                    }
                }

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

        // POST: /Member/Booking/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = CurrentUserId;
            var booking = await _context.Bookings
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.Member.UserId == userId);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy thông tin ca tập.");
                return RedirectToAction(nameof(Index));
            }

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã hủy lịch tập thành công.");
            return RedirectToAction(nameof(Index));
        }
    }
}
