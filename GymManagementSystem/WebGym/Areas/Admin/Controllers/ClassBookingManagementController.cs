using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class ClassBookingManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public ClassBookingManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/ClassBookingManagement
        public async Task<IActionResult> Index(
            int? classId, 
            string? status, 
            string? searchKeyword, 
            DateTime? bookingDate)
        {
            var query = _context.ClassBookings
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.Trainer)
                        .ThenInclude(t => t.User)
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.Room)
                .Include(b => b.Member)
                    .ThenInclude(m => m.User)
                .AsQueryable();

            if (classId.HasValue && classId.Value > 0)
            {
                query = query.Where(b => b.ClassId == classId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(b => b.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(b => b.Member.User.FullName.ToLower().Contains(kw) ||
                                         b.Member.User.Email.ToLower().Contains(kw) ||
                                         (b.Member.User.PhoneNumber != null && b.Member.User.PhoneNumber.Contains(kw)) ||
                                         b.GymClass.ClassName.ToLower().Contains(kw));
            }

            if (bookingDate.HasValue)
            {
                var d = bookingDate.Value.Date;
                query = query.Where(b => b.BookingDate.Date == d);
            }

            var bookings = await query.OrderByDescending(b => b.BookingDate).ToListAsync();

            // Thống kê nhanh
            ViewBag.TotalBookings = await _context.ClassBookings.CountAsync();
            ViewBag.AttendedCount = await _context.ClassBookings.CountAsync(b => b.Status == "Attended");
            ViewBag.ConfirmedCount = await _context.ClassBookings.CountAsync(b => b.Status == "Confirmed");
            ViewBag.CancelledCount = await _context.ClassBookings.CountAsync(b => b.Status == "Cancelled");

            await PopulateDropdowns(classId);

            ViewBag.SelectedClassId = classId;
            ViewBag.SelectedStatus = status;
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.SelectedBookingDate = bookingDate;

            return View(bookings);
        }

        // GET: /Admin/ClassBookingManagement/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? classId)
        {
            await PopulateDropdowns(classId);

            var vm = new ClassBookingCreateViewModel
            {
                ClassId = classId ?? 0,
                BookingDate = DateTime.Today,
                Status = "Confirmed"
            };

            return View(vm);
        }

        // POST: /Admin/ClassBookingManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassBookingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model.ClassId);
                return View(model);
            }

            var gymClass = await _context.GymClasses
                .Include(c => c.ClassBookings)
                .FirstOrDefaultAsync(c => c.ClassId == model.ClassId);

            if (gymClass == null)
            {
                SetErrorMessage("Không tìm thấy lớp học được chọn.");
                await PopulateDropdowns(model.ClassId);
                return View(model);
            }

            // Kiểm tra xem hội viên này đã đăng ký lớp học này chưa (nếu chưa bị hủy)
            var existingBooking = await _context.ClassBookings
                .FirstOrDefaultAsync(b => b.ClassId == model.ClassId && b.MemberId == model.MemberId && b.Status != "Cancelled");

            if (existingBooking != null)
            {
                ModelState.AddModelError("", "Hội viên này đã có tên trong danh sách đăng ký của lớp!");
                await PopulateDropdowns(model.ClassId);
                return View(model);
            }

            // Kiểm tra sĩ số
            int currentActive = gymClass.ClassBookings.Count(b => b.Status != "Cancelled");
            if (currentActive >= gymClass.MaxCapacity)
            {
                SetErrorMessage($"Lớp học '{gymClass.ClassName}' đã đầy sĩ số tối đa ({gymClass.MaxCapacity} học viên)!");
                await PopulateDropdowns(model.ClassId);
                return View(model);
            }

            var booking = new ClassBooking
            {
                ClassId = model.ClassId,
                MemberId = model.MemberId,
                BookingDate = model.BookingDate,
                Status = model.Status,
                Notes = model.Notes?.Trim()
            };

            _context.ClassBookings.Add(booking);

            // Cập nhật sĩ số lớp
            gymClass.CurrentCapacity = currentActive + 1;
            if (gymClass.CurrentCapacity >= gymClass.MaxCapacity)
            {
                gymClass.Status = "Full";
            }

            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã thêm học viên vào lớp thành công tại quầy!");
            return RedirectToAction(nameof(Index), new { classId = model.ClassId });
        }

        // POST: /Admin/ClassBookingManagement/QuickAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAttendance(int id, string newStatus, string? returnUrl)
        {
            var booking = await _context.ClassBookings
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.ClassBookings)
                .FirstOrDefaultAsync(b => b.ClassBookingId == id);

            if (booking == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Không tìm thấy lượt đăng ký." });
                }
                SetErrorMessage("Không tìm thấy lượt đăng ký.");
                return RedirectToAction(nameof(Index));
            }

            var oldStatus = booking.Status;
            booking.Status = newStatus;

            // Đồng bộ lại sĩ số lớp học
            var gymClass = booking.GymClass;
            if (gymClass != null)
            {
                int activeCount = gymClass.ClassBookings.Count(b => (b.ClassBookingId == id ? newStatus : b.Status) != "Cancelled");
                gymClass.CurrentCapacity = activeCount;
                if (gymClass.CurrentCapacity >= gymClass.MaxCapacity)
                {
                    gymClass.Status = "Full";
                }
                else if (gymClass.Status == "Full" && gymClass.CurrentCapacity < gymClass.MaxCapacity)
                {
                    gymClass.Status = "Open";
                }
            }

            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = $"Đã cập nhật trạng thái thành '{newStatus}'!",
                    status = newStatus 
                });
            }

            SetSuccessMessage($"Đã điểm danh / cập nhật trạng thái thành '{newStatus}'!");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index), new { classId = booking.ClassId });
        }

        // POST: /Admin/ClassBookingManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.ClassBookings
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.ClassBookings)
                .FirstOrDefaultAsync(b => b.ClassBookingId == id);

            if (booking == null)
            {
                SetErrorMessage("Không tìm thấy lượt đăng ký.");
                return RedirectToAction(nameof(Index));
            }

            var gymClass = booking.GymClass;
            int classId = booking.ClassId;

            _context.ClassBookings.Remove(booking);

            if (gymClass != null && booking.Status != "Cancelled")
            {
                gymClass.CurrentCapacity = Math.Max(0, gymClass.CurrentCapacity - 1);
                if (gymClass.Status == "Full" && gymClass.CurrentCapacity < gymClass.MaxCapacity)
                {
                    gymClass.Status = "Open";
                }
            }

            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã xóa lượt đăng ký khỏi danh sách!");
            return RedirectToAction(nameof(Index), new { classId });
        }

        private async Task PopulateDropdowns(int? selectedClassId = null)
        {
            var classes = await _context.GymClasses
                .OrderBy(c => c.ClassName)
                .Select(c => new
                {
                    c.ClassId,
                    DisplayName = $"{c.ClassName} ({c.Schedule} - Sĩ số: {c.CurrentCapacity}/{c.MaxCapacity})"
                })
                .ToListAsync();

            var members = await _context.Members
                .Include(m => m.User)
                .OrderBy(m => m.User.FullName)
                .Select(m => new
                {
                    m.MemberId,
                    DisplayName = $"{m.User.FullName} - {m.User.PhoneNumber ?? m.User.Email}"
                })
                .ToListAsync();

            ViewBag.ClassesList = new SelectList(classes, "ClassId", "DisplayName", selectedClassId);
            ViewBag.MembersList = new SelectList(members, "MemberId", "DisplayName");
        }
    }
}

