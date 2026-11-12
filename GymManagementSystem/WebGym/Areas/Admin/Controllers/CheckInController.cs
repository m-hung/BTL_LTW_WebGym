using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class CheckInController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public CheckInController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/CheckIn
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new CheckInAdminViewModel();
            var todayList = await GetTodayCheckInListAsync();
            model.TodayHistory = todayList;
            model.TodayCount = todayList.Count;
            model.ValidCount = todayList.Count(x => x.IsValid);
            model.WarningCount = todayList.Count(x => !x.IsValid);

            return View(model);
        }

        // POST: /Admin/CheckIn/ProcessCheckIn
        // Xử lý Logic Backend 4 Bước theo yêu cầu
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ProcessCheckIn([FromBody] CheckInProcessRequest request)
        {
            // Kiểm tra dữ liệu gửi lên
            if (request == null || string.IsNullOrWhiteSpace(request.Code))
            {
                return Json(new CheckInProcessResult
                {
                    Success = false,
                    IsValid = false,
                    ErrorCode = "EMPTY_CODE",
                    Message = "Vui lòng nhập mã thẻ hoặc đưa mã QR trước camera."
                });
            }

            var rawCode = request.Code.Trim();

            // =========================================================================
            // BƯỚC 1: Tìm thông tin hội viên theo mã thẻ gửi lên
            // =========================================================================
            // 1.1 Khớp trực tiếp CardCode hoặc QrCode (VD: "GYM-MB-0001")
            var member = await _context.Members
                .Include(m => m.User)
                .Include(m => m.Subscriptions)
                    .ThenInclude(s => s.Package)
                .FirstOrDefaultAsync(m => m.CardCode == rawCode || m.QrCode == rawCode);

            // 1.2 Phân tích trích xuất số nếu mã dạng "GYM-MB-0001", "MB-1", hoặc số nguyên MemberId
            if (member == null)
            {
                var match = System.Text.RegularExpressions.Regex.Match(rawCode, @"\d+");
                if (match.Success && int.TryParse(match.Value, out var parsedId))
                {
                    member = await _context.Members
                        .Include(m => m.User)
                        .Include(m => m.Subscriptions)
                            .ThenInclude(s => s.Package)
                        .FirstOrDefaultAsync(m => m.MemberId == parsedId);
                }
            }

            // 1.3 Thử tìm theo Số điện thoại, Email hoặc Username của tài khoản
            if (member == null)
            {
                member = await _context.Members
                    .Include(m => m.User)
                    .Include(m => m.Subscriptions)
                        .ThenInclude(s => s.Package)
                    .FirstOrDefaultAsync(m => m.User.PhoneNumber == rawCode || 
                                              m.User.Email == rawCode || 
                                              m.User.Username == rawCode);
            }

            // Không tìm thấy hội viên
            if (member == null)
            {
                return Json(new CheckInProcessResult
                {
                    Success = false,
                    IsValid = false,
                    ErrorCode = "NOT_FOUND",
                    Message = "Mã thẻ không tồn tại"
                });
            }

            var now = DateTime.Now;
            var avatarUrl = ResolveAvatar(member.AvatarUrl, member.UserId);

            // =========================================================================
            // BƯỚC 2 (Kiểm tra gói tập): Gói tập có đang hoạt động (Active) và ngày kết thúc còn hạn hay không?
            // =========================================================================
            var activeSub = member.Subscriptions
                .Where(s => s.Status == "Active" && s.EndDate >= now)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            // Lấy thông tin gói mới nhất (kể cả khi đã hết hạn) để báo chi tiết
            var latestSub = activeSub ?? member.Subscriptions
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            var packageName = latestSub?.Package?.PackageName ?? "Chưa đăng ký gói tập";
            var expiryDateStr = latestSub?.EndDate.ToString("dd/MM/yyyy") ?? "Chưa có hạn";

            if (activeSub == null)
            {
                // Gói tập đã hết hạn hoặc chưa đăng ký
                return Json(new CheckInProcessResult
                {
                    Success = false,
                    IsValid = false,
                    ErrorCode = "EXPIRED",
                    Message = "Gói tập đã hết hạn, cần gia hạn tại quầy",
                    MemberId = member.MemberId,
                    CardCode = member.MemberCardCode,
                    FullName = member.User.FullName,
                    AvatarUrl = avatarUrl,
                    PackageName = packageName,
                    DaysRemaining = 0,
                    ExpiryDate = expiryDateStr,
                    CheckInTime = now.ToString("dd/MM/yyyy HH:mm:ss"),
                    CheckInHour = now.ToString("HH:mm:ss"),
                    Method = request.Method ?? "Mã QR Code"
                });
            }

            // =========================================================================
            // BƯỚC 3 (Chống quẹt trùng): Kiểm tra xem người này có vừa quẹt thẻ cách đây ít phút hay không
            // =========================================================================
            int duplicateCooldownMinutes = 5; // Cửa sổ kiểm tra chống quẹt trùng: 5 phút
            var recentCheckIn = await _context.CheckIns
                .Where(c => c.MemberId == member.MemberId)
                .OrderByDescending(c => c.CheckInTime)
                .FirstOrDefaultAsync();

            if (recentCheckIn != null && (now - recentCheckIn.CheckInTime).TotalMinutes < duplicateCooldownMinutes)
            {
                var remainingSec = (int)(duplicateCooldownMinutes * 60 - (now - recentCheckIn.CheckInTime).TotalSeconds);
                var recentTimeStr = recentCheckIn.CheckInTime.ToString("HH:mm:ss");

                return Json(new CheckInProcessResult
                {
                    Success = false,
                    IsValid = false,
                    IsDuplicate = true,
                    ErrorCode = "DUPLICATE",
                    Message = $"Hội viên vừa quẹt thẻ lúc {recentTimeStr} (cách đây chưa đầy {duplicateCooldownMinutes} phút). Tránh quẹt 2 lần liên tiếp!",
                    MemberId = member.MemberId,
                    CardCode = member.MemberCardCode,
                    FullName = member.User.FullName,
                    AvatarUrl = avatarUrl,
                    PackageName = packageName,
                    DaysRemaining = (int)Math.Ceiling((activeSub.EndDate - now).TotalDays),
                    ExpiryDate = expiryDateStr,
                    CheckInTime = recentCheckIn.CheckInTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    CheckInHour = recentTimeStr,
                    Method = recentCheckIn.Method ?? "Mã QR Code"
                });
            }

            // =========================================================================
            // BƯỚC 4 (Phản hồi kết quả): Hợp lệ -> Lưu bản ghi vào bảng CheckIns
            // =========================================================================
            var daysRemaining = (int)Math.Ceiling((activeSub.EndDate - now).TotalDays);
            var method = !string.IsNullOrWhiteSpace(request.Method) ? request.Method : "Mã QR Code";

            var checkIn = new CheckIn
            {
                MemberId = member.MemberId,
                CheckInTime = now,
                Method = method,
                Notes = $"Check-in thành công: {packageName} (Còn {daysRemaining} ngày)"
            };

            _context.CheckIns.Add(checkIn);
            await _context.SaveChangesAsync();

            return Json(new CheckInProcessResult
            {
                Success = true,
                IsValid = true,
                ErrorCode = "SUCCESS",
                Message = "Check-in thành công",
                CheckInId = checkIn.CheckInId,
                MemberId = member.MemberId,
                CardCode = member.MemberCardCode,
                FullName = member.User.FullName,
                AvatarUrl = avatarUrl,
                PackageName = packageName,
                DaysRemaining = daysRemaining,
                ExpiryDate = expiryDateStr,
                CheckInTime = now.ToString("dd/MM/yyyy HH:mm:ss"),
                CheckInHour = now.ToString("HH:mm:ss"),
                Method = method
            });
        }

        // GET: /Admin/CheckIn/GetTodayHistory
        [HttpGet]
        public async Task<IActionResult> GetTodayHistory()
        {
            var list = await GetTodayCheckInListAsync();
            return Json(new
            {
                total = list.Count,
                valid = list.Count(x => x.IsValid),
                warning = list.Count(x => !x.IsValid),
                data = list
            });
        }

        // POST: /Admin/CheckIn/DeleteCheckIn
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteCheckIn(int id)
        {
            var item = await _context.CheckIns.FindAsync(id);
            if (item != null)
            {
                _context.CheckIns.Remove(item);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không tìm thấy bản ghi điểm danh." });
        }

        private async Task<List<CheckInHistoryItemDto>> GetTodayCheckInListAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var checkIns = await _context.CheckIns
                .Include(c => c.Member)
                    .ThenInclude(m => m.User)
                .Include(c => c.Member)
                    .ThenInclude(m => m.Subscriptions)
                        .ThenInclude(s => s.Package)
                .Where(c => c.CheckInTime >= today && c.CheckInTime < tomorrow)
                .OrderByDescending(c => c.CheckInTime)
                .ToListAsync();

            var now = DateTime.Now;
            var result = new List<CheckInHistoryItemDto>();

            foreach (var c in checkIns)
            {
                var member = c.Member;
                if (member == null) continue;

                var activeSub = member.Subscriptions
                    .Where(s => s.Status == "Active" && s.EndDate >= c.CheckInTime)
                    .OrderByDescending(s => s.EndDate)
                    .FirstOrDefault();

                var latestSub = activeSub ?? member.Subscriptions
                    .OrderByDescending(s => s.EndDate)
                    .FirstOrDefault();

                var isValid = activeSub != null;
                var daysRemaining = activeSub != null ? (int)Math.Ceiling((activeSub.EndDate - now).TotalDays) : 0;
                var packageName = latestSub?.Package?.PackageName ?? "Chưa đăng ký gói";

                result.Add(new CheckInHistoryItemDto
                {
                    CheckInId = c.CheckInId,
                    MemberId = member.MemberId,
                    CardCode = member.MemberCardCode,
                    FullName = member.User?.FullName ?? "Hội viên #" + member.MemberId,
                    AvatarUrl = ResolveAvatar(member.AvatarUrl, member.UserId),
                    PackageName = packageName,
                    Time = c.CheckInTime.ToString("HH:mm:ss"),
                    FullTime = c.CheckInTime.ToString("dd/MM/yyyy HH:mm"),
                    Method = c.Method ?? "Mã QR Code",
                    IsValid = isValid,
                    DaysRemaining = daysRemaining
                });
            }

            return result;
        }

        private static string ResolveAvatar(string? avatarUrl, int userId)
        {
            if (!string.IsNullOrWhiteSpace(avatarUrl))
            {
                if (avatarUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    return avatarUrl;
                if (avatarUrl.StartsWith("/"))
                    return avatarUrl;
                return "/images/" + avatarUrl;
            }

            var diskFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", $"user_{userId}.jpg");
            if (System.IO.File.Exists(diskFile))
            {
                return $"/uploads/avatars/user_{userId}.jpg";
            }

            return "/images/default-avatar.svg";
        }
    }
}
