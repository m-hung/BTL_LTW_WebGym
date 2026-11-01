using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;

namespace WebGym.Areas.Admin.Controllers
{
    public class TrainerManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public TrainerManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/TrainerManagement
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Include(t => t.User)
                .Include(t => t.Bookings)
                .OrderByDescending(t => t.TrainerId)
                .ToListAsync();

            return View(trainers);
        }

        // POST: /Admin/TrainerManagement/ToggleAvailability/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var trainer = await _context.Trainers.Include(t => t.User).FirstOrDefaultAsync(t => t.TrainerId == id);
            if (trainer == null)
            {
                SetErrorMessage("Không tìm thấy huấn luyện viên.");
                return RedirectToAction(nameof(Index));
            }

            SetSuccessMessage($"Đã cập nhật trạng thái HLV {trainer.User.FullName}.");
            return RedirectToAction(nameof(Index));
        }
    }
}
