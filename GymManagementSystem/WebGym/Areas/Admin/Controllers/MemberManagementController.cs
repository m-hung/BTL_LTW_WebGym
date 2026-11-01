using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;

namespace WebGym.Areas.Admin.Controllers
{
    public class MemberManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public MemberManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/MemberManagement
        public async Task<IActionResult> Index(string? searchKeyword)
        {
            var query = _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                query = query.Where(m => m.User.FullName.Contains(searchKeyword) ||
                                         m.User.Email.Contains(searchKeyword) ||
                                         (m.User.PhoneNumber != null && m.User.PhoneNumber.Contains(searchKeyword)));
            }

            var members = await query.OrderByDescending(m => m.MemberId).ToListAsync();
            ViewBag.SearchKeyword = searchKeyword;

            return View(members);
        }

        // POST: /Admin/MemberManagement/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy hội viên");
                return RedirectToAction(nameof(Index));
            }

            // Có thể mở rộng cờ IsActive hoặc xóa mềm
            SetSuccessMessage($"Đã cập nhật trạng thái hội viên {member.User.FullName}.");
            return RedirectToAction(nameof(Index));
        }
    }
}
