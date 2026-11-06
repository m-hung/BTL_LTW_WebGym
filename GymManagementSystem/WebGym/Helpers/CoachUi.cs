namespace WebGym.Helpers
{
    /// <summary>
    /// Hàm tiện ích hiển thị cho giao diện Huấn luyện viên (Area PT):
    /// đọc khung giờ "08:00 - 09:30", tên thứ rút gọn, ngày tương đối, chữ viết tắt tên, trạng thái ca tập.
    /// </summary>
    public static class CoachUi
    {
        public static TimeSpan SlotStart(string? slot) => ParsePart(slot, 0);

        public static TimeSpan SlotEnd(string? slot)
        {
            var end = ParsePart(slot, 1);
            return end == TimeSpan.Zero ? SlotStart(slot).Add(TimeSpan.FromMinutes(90)) : end;
        }

        private static TimeSpan ParsePart(string? slot, int index)
        {
            var parts = (slot ?? string.Empty).Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > index && TimeSpan.TryParse(parts[index], out var t) ? t : TimeSpan.Zero;
        }

        /// <summary>Thời điểm bắt đầu / kết thúc ca (giờ địa phương).</summary>
        public static DateTime StartAt(DateTime day, string? slot) => day.Date + SlotStart(slot);
        public static DateTime EndAt(DateTime day, string? slot) => day.Date + SlotEnd(slot);

        /// <summary>Hiển thị khung giờ với gạch nối chuẩn: "08:00–09:30".</summary>
        public static string SlotText(string? slot)
        {
            if (string.IsNullOrWhiteSpace(slot)) return "--";
            var s = SlotStart(slot);
            var e = SlotEnd(slot);
            return $"{s:hh\\:mm}–{e:hh\\:mm}";
        }

        public static string DayShort(DateTime d) => d.DayOfWeek switch
        {
            DayOfWeek.Sunday => "CN",
            _ => "T" + ((int)d.DayOfWeek + 1)
        };

        public static string DayLong(DateTime d) => d.DayOfWeek switch
        {
            DayOfWeek.Sunday => "Chủ nhật",
            DayOfWeek.Monday => "Thứ hai",
            DayOfWeek.Tuesday => "Thứ ba",
            DayOfWeek.Wednesday => "Thứ tư",
            DayOfWeek.Thursday => "Thứ năm",
            DayOfWeek.Friday => "Thứ sáu",
            _ => "Thứ bảy"
        };

        /// <summary>"Hôm nay", "Ngày mai", "Còn 3 ngày", "2 ngày trước"...</summary>
        public static string RelativeDay(DateTime day)
        {
            var diff = (day.Date - DateTime.Today).Days;
            return diff switch
            {
                0 => "Hôm nay",
                1 => "Ngày mai",
                -1 => "Hôm qua",
                > 1 => $"Còn {diff} ngày",
                _ => $"{-diff} ngày trước"
            };
        }

        /// <summary>"Còn 2 giờ 15 phút", "Còn 25 phút".</summary>
        public static string Countdown(DateTime start)
        {
            var span = start - DateTime.Now;
            if (span.TotalMinutes < 1) return "Sắp bắt đầu";
            if (span.TotalHours >= 24)
            {
                return $"Còn {(int)span.TotalDays} ngày {span.Hours} giờ";
            }
            return span.Hours > 0 ? $"Còn {span.Hours} giờ {span.Minutes} phút" : $"Còn {span.Minutes} phút";
        }

        public static string Initials(string? fullName)
        {
            var words = (fullName ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return "?";
            if (words.Length == 1) return words[0][..1].ToUpperInvariant();
            return (words[^2][..1] + words[^1][..1]).ToUpperInvariant();
        }

        public static string FirstName(string? fullName)
        {
            var words = (fullName ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.Length == 0 ? "bạn" : words[^1];
        }

        /// <summary>
        /// Trạng thái hiển thị của một ca, có xét thời gian thực:
        /// ca "Approved" đã qua giờ kết thúc hiển thị là "Đã diễn ra".
        /// Trả về (nhãn, hậu tố CSS, nhóm lọc).
        /// </summary>
        public static (string Label, string Css, string Groups) SessionState(string status, DateTime day, string? slot)
        {
            var ended = EndAt(day, slot) < DateTime.Now;
            return status switch
            {
                "Approved" when !ended => ("Đã xác nhận", "ok", "upcoming"),
                "Approved" => ("Đã diễn ra", "past", "done"),
                "Pending" when !ended => ("Chờ duyệt", "wait", "upcoming pending"),
                "Pending" => ("Quá hạn duyệt", "late", "pending"),
                "Completed" => ("Hoàn thành", "done", "done"),
                _ => ("Đã huỷ", "off", "cancelled")
            };
        }
    }
}
