using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Trainer;
using WebGym.Models.Entities;

namespace WebGym.Areas.PT.Controllers
{
    public class AssignedMembersController : PTBaseController
    {
        private readonly ApplicationDbContext _context;

        public AssignedMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /PT/AssignedMembers
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainer == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            // Danh sách các học viên từng đặt lịch với PT này
            var members = await _context.Bookings
                .Where(b => b.TrainerId == trainer.TrainerId)
                .Select(b => b.Member)
                .Distinct()
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .Include(m => m.ProgressRecords)
                .ToListAsync();

            return View(members);
        }

        // GET: /PT/AssignedMembers/RecordProgress?memberId=1
        [HttpGet]
        public async Task<IActionResult> RecordProgress(int memberId)
        {
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == memberId);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy học viên.");
                return RedirectToAction(nameof(Index));
            }

            var dto = new RecordProgressDto
            {
                MemberId = member.MemberId,
                MemberName = member.User.FullName,
                Weight = member.Weight ?? 70,
                BodyFatPercentage = member.BodyFatPercentage,
                MuscleMass = member.MuscleMass,
                Chest = member.Chest,
                Waist = member.Waist,
                Hips = member.Hips
            };

            return View(dto);
        }

        // POST: /PT/AssignedMembers/RecordProgress
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordProgress(RecordProgressDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var member = await _context.Members.FindAsync(model.MemberId);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy học viên.");
                return RedirectToAction(nameof(Index));
            }

            // Tạo bản ghi Progress mới
            var progress = new Progress
            {
                MemberId = model.MemberId,
                RecordedDate = DateTime.UtcNow,
                Weight = model.Weight,
                BodyFatPercentage = model.BodyFatPercentage,
                MuscleMass = model.MuscleMass,
                Chest = model.Chest,
                Waist = model.Waist,
                Hips = model.Hips,
                Notes = model.Notes
            };

            // Cập nhật lại chỉ số mới nhất vào bảng Member
            member.Weight = model.Weight;
            member.BodyFatPercentage = model.BodyFatPercentage;
            member.MuscleMass = model.MuscleMass;
            member.Chest = model.Chest;
            member.Waist = model.Waist;
            member.Hips = model.Hips;

            _context.Progress.Add(progress);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã cập nhật chỉ số InBody mới cho học viên thành công!");
            return RedirectToAction(nameof(Index));
        }
    }
}
