using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;

namespace WebGym.Areas.Member.Controllers
{
    public class WorkoutScheduleController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public WorkoutScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/WorkoutSchedule
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var progressList = await _context.Progress
                .Where(p => p.MemberId == member.MemberId)
                .OrderBy(p => p.RecordedDate)
                .ToListAsync();

            return View(progressList);
        }
    }
}
