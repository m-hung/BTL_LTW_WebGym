using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Member;

namespace WebGym.Areas.Member.Controllers
{
    public class ProfileController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/Profile
        public async Task<IActionResult> Index()
        {
            var userId = CurrentUserId;
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .Include(m => m.ProgressRecords)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hồ sơ cá nhân.");
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var activeSub = member.Subscriptions
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault(s => s.Status == "Active");

            var dto = new MemberProfileDto
            {
                Id = member.MemberId,
                FullName = member.User.FullName,
                Email = member.User.Email,
                PhoneNumber = member.User.PhoneNumber,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Height = member.Height,
                Weight = member.Weight,
                BMR = member.BMR,
                BodyFatPercentage = member.BodyFatPercentage,
                MuscleMass = member.MuscleMass,
                Chest = member.Chest,
                Waist = member.Waist,
                Hips = member.Hips,
                CurrentPackageName = activeSub?.Package.PackageName,
                MembershipEndDate = activeSub?.EndDate
            };

            ViewBag.ProgressList = member.ProgressRecords.OrderBy(p => p.RecordedDate).ToList();

            return View(dto);
        }

        // POST: /Member/Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MemberProfileDto model)
        {
            var userId = CurrentUserId;
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                SetErrorMessage("Hội viên không tồn tại.");
                return RedirectToAction(nameof(Index));
            }

            member.User.FullName = model.FullName;
            member.User.PhoneNumber = model.PhoneNumber;
            member.Gender = model.Gender;
            member.DateOfBirth = model.DateOfBirth;
            member.Height = model.Height;
            member.Weight = model.Weight;
            member.BMR = model.BMR;
            member.BodyFatPercentage = model.BodyFatPercentage;
            member.MuscleMass = model.MuscleMass;
            member.Chest = model.Chest;
            member.Waist = model.Waist;
            member.Hips = model.Hips;

            await _context.SaveChangesAsync();

            SetSuccessMessage("Cập nhật chỉ số và thông tin cá nhân thành công!");
            return RedirectToAction(nameof(Index));
        }
    }
}
