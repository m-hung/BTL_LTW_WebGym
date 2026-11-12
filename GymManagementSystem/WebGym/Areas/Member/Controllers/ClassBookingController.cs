using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Member;
using WebGym.Models.Entities;
using Member = WebGym.Models.Entities.Member;
using System.Text.RegularExpressions;

namespace WebGym.Areas.Member.Controllers
{
    public class ClassBookingController : MemberBaseController
    {
        private readonly ApplicationDbContext _context;

        public ClassBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Member/ClassBooking
        // Tab = "schedule" (Thời khóa biểu đăng ký) hoặc "my-classes" (Lịch tập của tôi)
        [HttpGet]
        public async Task<IActionResult> Index(string tab = "schedule")
        {
            var member = await GetCurrentMemberAsync();
            if (member == null)
            {
                SetErrorMessage("Không tìm thấy thông tin hội viên của tài khoản hiện tại.");
                return RedirectToAction("Index", "Profile");
            }

            // 1. Lấy danh sách tất cả lớp học nhóm trong hệ thống
            var classes = await _context.GymClasses
                .Include(c => c.Trainer)
                    .ThenInclude(t => t.User)
                .Include(c => c.Room)
                .Include(c => c.ClassBookings)
                .Where(c => c.Status != "Cancelled")
                .OrderBy(c => c.ClassId)
                .ToListAsync();

            // 2. Lấy danh sách lớp đã đăng ký của hội viên này
            var myBookings = await _context.ClassBookings
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.Trainer)
                        .ThenInclude(t => t.User)
                .Include(b => b.GymClass)
                    .ThenInclude(c => c.Room)
                .Where(b => b.MemberId == member.MemberId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var myConfirmedBookingMap = myBookings
                .Where(b => b.Status == "Confirmed")
                .ToDictionary(b => b.ClassId, b => b.ClassBookingId);

            // Đồng bộ sĩ số thực tế từ các booking confirmed để đảm bảo dữ liệu luôn chính xác
            var model = new ClassBookingPageViewModel
            {
                MemberId = member.MemberId,
                MemberName = member.User.FullName,
                ActiveTab = tab
            };

            foreach (var c in classes)
            {
                var confirmedCount = c.ClassBookings.Count(b => b.Status == "Confirmed");
                c.CurrentCapacity = confirmedCount;
                if (c.CurrentCapacity >= c.MaxCapacity)
                {
                    c.Status = "Full";
                }
                else if (c.Status == "Full" && c.CurrentCapacity < c.MaxCapacity)
                {
                    c.Status = "Open";
                }

                var isBooked = myConfirmedBookingMap.TryGetValue(c.ClassId, out var userBookingId);

                model.AvailableClasses.Add(new ClassScheduleItemDto
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    Description = c.Description,
                    TrainerId = c.TrainerId,
                    TrainerName = c.Trainer?.User?.FullName ?? "HLV WebGym",
                    TrainerAvatar = ResolveAvatar(c.Trainer?.AvatarUrl, c.TrainerId, c.Trainer?.UserId ?? 0),
                    RoomName = c.Room?.RoomName ?? "Studio Trung Tâm",
                    RoomLocation = c.Room?.Location ?? "Tầng 2",
                    Schedule = c.Schedule,
                    MaxCapacity = c.MaxCapacity,
                    CurrentCapacity = c.CurrentCapacity,
                    Status = c.Status,
                    ImageUrl = ResolveClassImage(c.ImageUrl, c.ClassName),
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    IsBookedByCurrentUser = isBooked,
                    CurrentUserBookingId = isBooked ? userBookingId : null
                });
            }

            // Danh sách lịch tập riêng của tôi
            foreach (var b in myBookings)
            {
                if (b.GymClass == null) continue;

                model.MyBookedClasses.Add(new MyClassBookingItemDto
                {
                    ClassBookingId = b.ClassBookingId,
                    ClassId = b.ClassId,
                    ClassName = b.GymClass.ClassName,
                    Description = b.GymClass.Description,
                    TrainerName = b.GymClass.Trainer?.User?.FullName ?? "HLV WebGym",
                    TrainerAvatar = ResolveAvatar(b.GymClass.Trainer?.AvatarUrl, b.GymClass.TrainerId, b.GymClass.Trainer?.UserId ?? 0),
                    RoomName = b.GymClass.Room?.RoomName ?? "Studio Trung Tâm",
                    RoomLocation = b.GymClass.Room?.Location ?? "Tầng 2",
                    Schedule = b.GymClass.Schedule,
                    ImageUrl = ResolveClassImage(b.GymClass.ImageUrl, b.GymClass.ClassName),
                    BookingDate = b.BookingDate,
                    Status = b.Status
                });
            }

            return View("Index", model);
        }

        // GET: /Member/ClassBooking/MySchedule
        // Xem trực tiếp phần LỊCH TẬP RIÊNG của hội viên
        [HttpGet]
        public async Task<IActionResult> MySchedule()
        {
            return await Index("my-classes");
        }

        // POST: /Member/ClassBooking/Register
        // Đăng ký tham gia lớp tập nhóm (Slot Booking)
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Register(int classId)
        {
            var member = await GetCurrentMemberAsync();
            if (member == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập bằng tài khoản hội viên để đăng ký lớp tập." });
            }

            var gymClass = await _context.GymClasses
                .Include(c => c.ClassBookings)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (gymClass == null)
            {
                return Json(new { success = false, message = "Không tìm thấy lớp học yêu cầu." });
            }

            // 1. Kiểm tra xem hội viên này đã đăng ký lớp này chưa
            var existingBooking = await _context.ClassBookings
                .FirstOrDefaultAsync(b => b.ClassId == classId && b.MemberId == member.MemberId && b.Status == "Confirmed");

            if (existingBooking != null)
            {
                return Json(new { success = false, message = "Bạn đã đăng ký tham gia lớp học này rồi! Vui lòng kiểm tra mục 'Lịch Tập Của Tôi'." });
            }

            // 2. Kiểm tra TRÙNG LỊCH TẬP hoặc 2 LỊCH QUÁ SÁT NHAU (< 2 tiếng nghỉ ngơi giữa các buổi tập)
            var currentConfirmedBookings = await _context.ClassBookings
                .Include(b => b.GymClass)
                .Where(b => b.MemberId == member.MemberId && b.Status == "Confirmed" && b.GymClass != null)
                .Select(b => new { b.GymClass!.ClassName, b.GymClass.Schedule })
                .ToListAsync();

            var conflictCheck = CheckScheduleConflict(
                gymClass.Schedule,
                gymClass.ClassName,
                currentConfirmedBookings.Select(x => (x.ClassName, x.Schedule)));

            if (conflictCheck.HasConflict)
            {
                return Json(new { success = false, message = conflictCheck.Message });
            }

            // 3. Kiểm tra sĩ số lớp học (Chỉ được đăng ký những lớp chưa full)
            var activeCount = await _context.ClassBookings
                .CountAsync(b => b.ClassId == classId && b.Status == "Confirmed");

            if (activeCount >= gymClass.MaxCapacity)
            {
                gymClass.CurrentCapacity = gymClass.MaxCapacity;
                gymClass.Status = "Full";
                await _context.SaveChangesAsync();

                return Json(new { 
                    success = false, 
                    message = "Rất tiếc! Lớp học này ĐÃ ĐẦY (FULL SĨ SỐ), không thể nhận thêm học viên.",
                    isFull = true
                });
            }

            // 4. Tiến hành đặt chỗ
            var newBooking = new ClassBooking
            {
                ClassId = classId,
                MemberId = member.MemberId,
                BookingDate = DateTime.Now,
                Status = "Confirmed",
                Notes = "Đăng ký thành công qua Cổng hội viên Pulse Gym"
            };

            _context.ClassBookings.Add(newBooking);

            gymClass.CurrentCapacity = activeCount + 1;
            if (gymClass.CurrentCapacity >= gymClass.MaxCapacity)
            {
                gymClass.Status = "Full";
            }

            await _context.SaveChangesAsync();

            var available = Math.Max(0, gymClass.MaxCapacity - gymClass.CurrentCapacity);

            return Json(new
            {
                success = true,
                message = $"Chúc mừng bạn đã đăng ký tham gia lớp \"{gymClass.ClassName}\" thành công!",
                bookingId = newBooking.ClassBookingId,
                classId = gymClass.ClassId,
                currentCapacity = gymClass.CurrentCapacity,
                maxCapacity = gymClass.MaxCapacity,
                availableSlots = available,
                isFull = gymClass.CurrentCapacity >= gymClass.MaxCapacity
            });
        }

        // POST: /Member/ClassBooking/Cancel
        // Hủy lịch tập và tự động nhả slot cho người khác
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Cancel(int bookingId)
        {
            var member = await GetCurrentMemberAsync();
            if (member == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thao tác." });
            }

            var booking = await _context.ClassBookings
                .Include(b => b.GymClass)
                .FirstOrDefaultAsync(b => b.ClassBookingId == bookingId && b.MemberId == member.MemberId && b.Status == "Confirmed");

            if (booking == null)
            {
                return Json(new { success = false, message = "Không tìm thấy lịch tập cần hủy hoặc lịch này đã được hủy trước đó." });
            }

            // 1. Cập nhật trạng thái booking thành Cancelled
            booking.Status = "Cancelled";

            // 2. Nhả slot trống cho lớp học
            var activeCount = await _context.ClassBookings
                .CountAsync(b => b.ClassId == booking.ClassId && b.Status == "Confirmed" && b.ClassBookingId != bookingId);

            booking.GymClass.CurrentCapacity = Math.Max(0, activeCount);
            if (booking.GymClass.CurrentCapacity < booking.GymClass.MaxCapacity)
            {
                booking.GymClass.Status = "Open"; // Trạng thái chuyển lại thành MỞ ĐĂNG KÝ
            }

            await _context.SaveChangesAsync();

            var available = Math.Max(0, booking.GymClass.MaxCapacity - booking.GymClass.CurrentCapacity);

            return Json(new
            {
                success = true,
                message = $"Đã hủy lịch tập lớp \"{booking.GymClass.ClassName}\". Chỗ trống đã được nhả cho người khác đăng ký!",
                classId = booking.ClassId,
                currentCapacity = booking.GymClass.CurrentCapacity,
                maxCapacity = booking.GymClass.MaxCapacity,
                availableSlots = available,
                isOpen = true
            });
        }

        private async Task<WebGym.Models.Entities.Member?> GetCurrentMemberAsync()
        {
            if (CurrentUserId == null) return null;
            return await _context.Members
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UserId == CurrentUserId.Value);
        }

        public static string ResolveClassImage(string? imageUrl, string className)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl) && imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return imageUrl;

            var lower = (className ?? "").ToLower();
            if (lower.Contains("yoga")) return "https://images.unsplash.com/photo-1545205597-3d9d02c29597?w=800&auto=format&fit=crop";
            if (lower.Contains("spinning") || lower.Contains("đạp xe") || lower.Contains("cycling") || lower.Contains("rpm")) 
                return "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&auto=format&fit=crop";
            if (lower.Contains("boxing") || lower.Contains("kickfit") || lower.Contains("combat")) 
                return "https://images.unsplash.com/photo-1549719386-74dfcbf7dbed?w=800&auto=format&fit=crop";
            if (lower.Contains("bodypump") || lower.Contains("tạ") || lower.Contains("pump")) 
                return "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=800&auto=format&fit=crop";
            if (lower.Contains("pilates")) 
                return "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=800&auto=format&fit=crop";
            if (lower.Contains("zumba") || lower.Contains("dance")) 
                return "https://images.unsplash.com/photo-1524594152303-9fd13543fe6e?w=800&auto=format&fit=crop";

            return "https://images.unsplash.com/photo-1571902943202-507ec2618e8f?w=800&auto=format&fit=crop";
        }

        public static string ResolveAvatar(string? avatarUrl, int trainerId = 0, int userId = 0)
        {
            if (!string.IsNullOrWhiteSpace(avatarUrl))
            {
                var clean = avatarUrl.Trim();
                if (clean.StartsWith("~"))
                {
                    clean = clean.TrimStart('~');
                }
                if (clean.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    return clean;
                }
                if (clean.StartsWith("/"))
                {
                    return clean;
                }
                if (clean.StartsWith("images/"))
                {
                    return "/" + clean;
                }
                return "/images/" + clean;
            }

            if (trainerId > 0)
            {
                var coachIdx = ((trainerId - 1) % 3) + 1;
                return $"/images/coach{coachIdx}.jpg";
            }

            if (userId > 0)
            {
                var diskFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", $"user_{userId}.jpg");
                if (System.IO.File.Exists(diskFile))
                {
                    return $"/uploads/avatars/user_{userId}.jpg";
                }
            }

            return "/images/coach1.jpg";
        }

        // =========================================================================
        // LOGIC KIỂM TRA TRÙNG LỊCH & KHOẢNG CÁCH NGHỈ NGƠI TỐI THIỂU GIỮA CÁC LỚP
        // =========================================================================
        public class ParsedClassSchedule
        {
            public List<DayOfWeek> Days { get; set; } = new();
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public bool IsValid { get; set; }
        }

        public static ParsedClassSchedule ParseClassSchedule(string? scheduleStr)
        {
            var result = new ParsedClassSchedule();
            if (string.IsNullOrWhiteSpace(scheduleStr)) return result;

            // 1. Tìm khoảng thời gian: ví dụ (06:30 - 07:45) hoặc 06:30 - 07:45
            var timeMatch = Regex.Match(scheduleStr, @"\(?(\d{1,2}):(\d{2})\s*-\s*(\d{1,2}):(\d{2})\)?");
            if (timeMatch.Success)
            {
                var h1 = int.Parse(timeMatch.Groups[1].Value);
                var m1 = int.Parse(timeMatch.Groups[2].Value);
                var h2 = int.Parse(timeMatch.Groups[3].Value);
                var m2 = int.Parse(timeMatch.Groups[4].Value);
                result.StartTime = new TimeSpan(h1, m1, 0);
                result.EndTime = new TimeSpan(h2, m2, 0);
            }
            else
            {
                return result;
            }

            // 2. Tìm các thứ trong tuần
            var lower = scheduleStr.ToLower();
            if (lower.Contains("2") && (lower.Contains("thứ") || lower.Contains("t2") || lower.Contains("- 2") || lower.Contains("2 -")))
                result.Days.Add(DayOfWeek.Monday);
            if (lower.Contains("3") && (lower.Contains("thứ") || lower.Contains("t3") || lower.Contains("- 3") || lower.Contains("3 -")))
                result.Days.Add(DayOfWeek.Tuesday);
            if (lower.Contains("4") && (lower.Contains("thứ") || lower.Contains("t4") || lower.Contains("- 4") || lower.Contains("4 -")))
                result.Days.Add(DayOfWeek.Wednesday);
            if (lower.Contains("5") && (lower.Contains("thứ") || lower.Contains("t5") || lower.Contains("- 5") || lower.Contains("5 -")))
                result.Days.Add(DayOfWeek.Thursday);
            if (lower.Contains("6") && (lower.Contains("thứ") || lower.Contains("t6") || lower.Contains("- 6") || lower.Contains("6 -")))
                result.Days.Add(DayOfWeek.Friday);
            if (lower.Contains("7") && (lower.Contains("thứ") || lower.Contains("t7") || lower.Contains("- 7") || lower.Contains("7 -")))
                result.Days.Add(DayOfWeek.Saturday);
            if (lower.Contains("cn") || lower.Contains("chủ nhật") || lower.Contains("chu nhat"))
                result.Days.Add(DayOfWeek.Sunday);

            result.IsValid = result.Days.Any() && result.EndTime > result.StartTime;
            return result;
        }

        public static (bool HasConflict, string Message) CheckScheduleConflict(
            string newScheduleStr, 
            string newClassName, 
            IEnumerable<(string ClassName, string Schedule)> currentBookings)
        {
            var newParsed = ParseClassSchedule(newScheduleStr);
            if (!newParsed.IsValid) return (false, "");

            // Khoảng nghỉ tối thiểu giữa 2 lớp tập là 2 tiếng (120 phút) để tránh quá tải thể lực
            var minRestGap = TimeSpan.FromHours(2);

            foreach (var booked in currentBookings)
            {
                var bookedParsed = ParseClassSchedule(booked.Schedule);
                if (!bookedParsed.IsValid) continue;

                // Kiểm tra xem có ngày học nào trùng nhau trong tuần không
                var commonDays = newParsed.Days.Intersect(bookedParsed.Days).ToList();
                if (!commonDays.Any()) continue;

                var commonDayNames = string.Join(", ", commonDays.Select(GetVietnameseDayName));

                // Trường hợp 1: Trùng giờ học trực tiếp
                bool isOverlap = (newParsed.StartTime < bookedParsed.EndTime) && (bookedParsed.StartTime < newParsed.EndTime);
                if (isOverlap)
                {
                    return (true, $"Trùng lịch tập! Lớp \"{newClassName}\" trùng khung giờ với lớp \"{booked.ClassName}\" bạn đã đăng ký ({booked.Schedule}) vào các ngày {commonDayNames}. Vui lòng chọn lớp có khung giờ khác.");
                }

                // Trường hợp 2: Hai lớp quá sát nhau (< 2 tiếng nghỉ ngơi giữa các buổi tập)
                TimeSpan gap;
                if (newParsed.StartTime >= bookedParsed.EndTime)
                {
                    gap = newParsed.StartTime - bookedParsed.EndTime;
                }
                else
                {
                    gap = bookedParsed.StartTime - newParsed.EndTime;
                }

                if (gap < minRestGap)
                {
                    var gapMinutes = (int)gap.TotalMinutes;
                    return (true, $"Khoảng cách 2 buổi tập quá sát nhau (chỉ cách {gapMinutes} phút vào {commonDayNames})! Bạn đã có lịch lớp \"{booked.ClassName}\" ({booked.Schedule}). Để đảm bảo an toàn sức khỏe và phục hồi thể lực, học viên cần nghỉ tối thiểu 2 tiếng giữa các buổi tập.");
                }
            }

            return (false, "");
        }

        private static string GetVietnameseDayName(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "Thứ 2",
                DayOfWeek.Tuesday => "Thứ 3",
                DayOfWeek.Wednesday => "Thứ 4",
                DayOfWeek.Thursday => "Thứ 5",
                DayOfWeek.Friday => "Thứ 6",
                DayOfWeek.Saturday => "Thứ 7",
                DayOfWeek.Sunday => "Chủ Nhật",
                _ => day.ToString()
            };
        }
    }
}
