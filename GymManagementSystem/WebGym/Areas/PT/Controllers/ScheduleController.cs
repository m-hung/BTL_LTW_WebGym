using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
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
    }
}
