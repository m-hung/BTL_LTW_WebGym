using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models;
using WebGym.Models.Entities;
using WebGym.Services.Interfaces;

namespace WebGym.Services.Implementations
{
    public class GymKnowledgeService : IGymKnowledgeService
    {
        private const string OpeningHours = "05:00 – 23:00 hằng ngày";
        private static readonly CultureInfo Vi = new("vi-VN");

        private readonly ApplicationDbContext _context;

        public GymKnowledgeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------------
        // Ngữ cảnh cho LLM
        // ------------------------------------------------------------------
        public async Task<string> BuildContextAsync()
        {
            var packages = await LoadPackagesAsync();
            var trainers = await LoadTrainersAsync();

            var sb = new StringBuilder();
            sb.AppendLine("=== DỮ LIỆU CHÍNH THỨC CỦA PULSE GYM (chỉ dùng số liệu trong đây) ===");
            sb.AppendLine($"Giờ mở cửa: {OpeningHours}.");
            sb.AppendLine();

            sb.AppendLine("GÓI TẬP:");
            foreach (var p in packages)
            {
                sb.AppendLine($"- {p.PackageName}: {Vnd(p.Price)} / {DurationLabel(p.DurationDays)} " +
                              $"(~{Vnd(MonthlyPrice(p))}/tháng); {(p.IncludesPT ? "CÓ kèm PT" : "KHÔNG kèm PT")}. {p.Description}");
            }
            sb.AppendLine("Xem & mua gói: /Home/Packages");
            sb.AppendLine();

            sb.AppendLine("HUẤN LUYỆN VIÊN (PT):");
            foreach (var t in trainers)
            {
                sb.AppendLine($"- {t.User?.FullName ?? "HLV"} (link /huan-luyen-vien/{t.TrainerId}): chuyên môn: {t.Specialization}. {t.Bio}");
            }
            sb.AppendLine();

            sb.AppendLine("KHU VỰC TẬP:");
            foreach (var f in FacilityData.All)
            {
                sb.AppendLine($"- {f.FullName} (link /khu-vuc/{f.Slug}): {f.Tagline} Diện tích {f.Area}, sức chứa {f.Capacity}, {f.Location}, mở {f.Hours}.");
            }
            sb.AppendLine();

            sb.AppendLine("QUY TRÌNH: Đăng ký tài khoản → chọn gói tại /Home/Packages → thanh toán online (VNPay, Momo hoặc chuyển khoản). " +
                          "Hội viên đăng nhập có thể đặt lịch tập với PT và theo dõi chỉ số InBody trong khu vực Hội viên.");
            sb.AppendLine("Lịch lớp nhóm theo tuần: xem mục Lịch tập ở trang chủ (/#lich-tap).");
            return sb.ToString();
        }

        // ------------------------------------------------------------------
        // Trả lời dự phòng (không cần LLM)
        // ------------------------------------------------------------------
        public async Task<(string Reply, List<string> Suggestions)> AnswerOfflineAsync(string message)
        {
            var norm = Norm(message);
            var tokens = new HashSet<string>(norm.Split(
                new[] { ' ', ',', '.', '?', '!', ';', ':', '-', '/', '(', ')', '"', '\'' },
                StringSplitOptions.RemoveEmptyEntries));

            var packages = await LoadPackagesAsync();
            var trainers = await LoadTrainersAsync();

            // 1) Hỏi đích danh một HLV
            foreach (var t in trainers)
            {
                var full = Norm(t.User?.FullName ?? "");
                var parts = full.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var lastTwo = parts.Length >= 2 ? string.Join(' ', parts.TakeLast(2)) : full;
                if (full.Length > 0 && (norm.Contains(full) || norm.Contains(lastTwo)))
                {
                    return (TrainerDetail(t), S("Xem tất cả HLV", "Gói nào có kèm PT?", "Đặt lịch tập thế nào?"));
                }
            }

            // 2) Hỏi đích danh một gói
            var named = FindPackage(packages, tokens, norm);
            if (named != null)
            {
                return (PackageDetail(named), S("So sánh các gói tập", "Gói nào có kèm PT?", "Thanh toán thế nào?"));
            }

            // 3) Nhóm chủ đề chung
            if (Has(tokens, norm, "re nhat", "tiet kiem", "gia mem", "binh dan"))
            {
                var cheapest = packages.OrderBy(MonthlyPrice).FirstOrDefault();
                if (cheapest != null)
                    return ($"Gói **rẻ nhất** theo tháng là **{cheapest.PackageName}**: {Vnd(cheapest.Price)} cho {DurationLabel(cheapest.DurationDays)} " +
                            $"(~{Vnd(MonthlyPrice(cheapest))}/tháng).\n\nXem chi tiết các gói tại [Bảng giá](/Home/Packages).",
                            S("Gói nào có kèm PT?", "So sánh các gói tập"));
            }

            if (Has(tokens, norm, "kem pt", "co pt", "co hlv", "pt rieng", "1 kem 1", "ca nhan") && Has(tokens, norm, "goi", "tap"))
            {
                var withPt = packages.Where(p => p.IncludesPT).ToList();
                var sbPt = new StringBuilder("Các gói **có kèm PT** (tập 1 kèm 1 cùng huấn luyện viên):\n");
                foreach (var p in withPt)
                    sbPt.AppendLine($"- **{p.PackageName}** — {Vnd(p.Price)} / {DurationLabel(p.DurationDays)}");
                sbPt.Append("\nXem chi tiết tại [Bảng giá](/Home/Packages).");
                return (sbPt.ToString(), S("Xem danh sách HLV", "Thanh toán thế nào?"));
            }

            if (Has(tokens, norm, "dang ky", "mua goi", "thanh toan", "momo", "vnpay", "chuyen khoan", "dang ki", "mua"))
            {
                return ("Để đăng ký tập tại Pulse Gym rất đơn giản:\n" +
                        "1. **Tạo tài khoản** (nút *Đăng ký* trên thanh menu).\n" +
                        "2. Chọn gói phù hợp tại [Bảng giá](/Home/Packages).\n" +
                        "3. **Thanh toán online** bằng VNPay, Momo hoặc chuyển khoản.\n\n" +
                        "Sau khi thanh toán, gói tập của bạn được kích hoạt ngay.",
                        S("Có những gói nào?", "Gói nào rẻ nhất?", "Giờ mở cửa?"));
            }

            if (Has(tokens, norm, "goi", "gia", "bao nhieu", "phi", "hoc phi", "bang gia", "price", "tien", "thang", "nam"))
            {
                return (PackageList(packages), S("Gói nào rẻ nhất?", "Gói nào có kèm PT?", "Thanh toán thế nào?"));
            }

            if (Has(tokens, norm, "pt", "hlv", "huan luyen", "coach", "trainer", "thay", "co giao"))
            {
                return (TrainerList(trainers), S("Gói nào có kèm PT?", "Đặt lịch tập thế nào?", "Các khu vực tập?"));
            }

            if (Has(tokens, norm, "gio", "mo cua", "dong cua", "may gio", "open", "hoat dong"))
            {
                return ($"Pulse Gym mở cửa **{OpeningHours}**.\n\nBạn có thể xem mức độ đông/vắng theo khung giờ ở từng [khu vực tập](/Home/Facilities) để chọn giờ tập thoải mái nhất.",
                        S("Các khu vực tập?", "Có những gói nào?"));
            }

            if (Has(tokens, norm, "khu", "phong", "co so vat chat", "thiet bi", "may tap", "boxing", "yoga", "cardio", "ta", "sauna", "locker", "dia chi", "o dau"))
            {
                return (FacilityList(), S("Giờ mở cửa?", "Có những gói nào?", "Danh sách HLV"));
            }

            if (Has(tokens, norm, "lich", "dat lich", "booking", "lop", "class", "buoi tap"))
            {
                return ("Bạn có thể:\n" +
                        "- Xem **lịch lớp nhóm** theo tuần ở mục [Lịch tập](/#lich-tap) trên trang chủ.\n" +
                        "- **Đặt lịch tập với PT**: đăng nhập tài khoản hội viên rồi chọn HLV và khung giờ phù hợp; HLV sẽ xác nhận lịch cho bạn.\n\n" +
                        "Bạn chưa có tài khoản? Bấm *Đăng ký* trên thanh menu nhé.",
                        S("Danh sách HLV", "Gói nào có kèm PT?"));
            }

            if (Has(tokens, norm, "giam can", "tang can", "tang co", "giam mo", "an uong", "dinh duong", "protein", "calo", "bai tap", "tap gi", "bmi", "body", "dang"))
            {
                return ("Vài gợi ý nhanh để bắt đầu:\n" +
                        "- Tập **3–4 buổi/tuần**, ưu tiên bài đa khớp (squat, deadlift, đẩy ngực, kéo xà).\n" +
                        "- Ăn đủ đạm khoảng **1,6–2 g protein/kg** cân nặng, ngủ đủ 7–8 giờ.\n" +
                        "- Muốn giảm mỡ: thêm cardio/HIIT 2 buổi/tuần và kiểm soát calo.\n\n" +
                        "Để có giáo án cá nhân hoá, bạn nên gặp một [HLV của Pulse Gym](/#huan-luyen-vien).",
                        S("Danh sách HLV", "Gói nào có kèm PT?"));
            }

            if (Has(tokens, norm, "cam on", "thanks", "thank", "ok", "tuyet"))
            {
                return ("Rất vui được giúp bạn! Cần gì thêm cứ hỏi mình nhé 💪", S("Có những gói nào?", "Danh sách HLV"));
            }

            if (Has(tokens, norm, "xin chao", "chao", "hello", "hi", "alo", "hey"))
            {
                return ("Chào bạn! Mình là **Pulse AI** — trợ lý của Pulse Gym. Mình có thể giúp bạn về gói tập, huấn luyện viên, khu vực tập, giờ mở cửa và cách đăng ký.",
                        S("Có những gói nào?", "Danh sách HLV", "Giờ mở cửa?"));
            }

            return ("Mình chưa chắc hiểu ý bạn. Mình có thể giúp về:\n" +
                    "- **Gói tập** & bảng giá\n" +
                    "- **Huấn luyện viên (PT)**\n" +
                    "- **Khu vực tập**, giờ mở cửa\n" +
                    "- Cách **đăng ký & thanh toán**\n\n" +
                    "Bạn thử hỏi lại theo các chủ đề trên nhé!",
                    S("Có những gói nào?", "Danh sách HLV", "Giờ mở cửa?"));
        }

        // ------------------------------------------------------------------
        // Dữ liệu
        // ------------------------------------------------------------------
        private Task<List<GymPackage>> LoadPackagesAsync() =>
            _context.GymPackages.AsNoTracking().OrderBy(p => p.Price).ToListAsync();

        private Task<List<Trainer>> LoadTrainersAsync() =>
            _context.Trainers.AsNoTracking().Include(t => t.User).OrderBy(t => t.TrainerId).ToListAsync();

        // ------------------------------------------------------------------
        // Dựng câu trả lời
        // ------------------------------------------------------------------
        private static string PackageList(List<GymPackage> packages)
        {
            if (packages.Count == 0) return "Hiện chưa có gói tập nào được mở bán. Bạn vui lòng quay lại sau nhé!";
            var sb = new StringBuilder("Pulse Gym hiện có **" + packages.Count + " gói tập**:\n");
            foreach (var p in packages)
            {
                sb.AppendLine($"- **{p.PackageName}** — {Vnd(p.Price)} / {DurationLabel(p.DurationDays)}{(p.IncludesPT ? " · kèm PT" : "")}");
            }
            sb.Append("\nXem chi tiết & đăng ký tại [Bảng giá](/Home/Packages).");
            return sb.ToString();
        }

        private static string PackageDetail(GymPackage p) =>
            $"**{p.PackageName}**\n" +
            $"- Giá: **{Vnd(p.Price)}** / {DurationLabel(p.DurationDays)} (~{Vnd(MonthlyPrice(p))}/tháng)\n" +
            $"- {(p.IncludesPT ? "Có kèm PT cá nhân" : "Không kèm PT")}\n" +
            (string.IsNullOrWhiteSpace(p.Description) ? "" : $"- {p.Description}\n") +
            "\nĐăng ký tại [Bảng giá](/Home/Packages).";

        private static string TrainerList(List<Trainer> trainers)
        {
            if (trainers.Count == 0) return "Danh sách huấn luyện viên đang được cập nhật. Bạn vui lòng quay lại sau nhé!";
            var sb = new StringBuilder("Đội ngũ **huấn luyện viên** của Pulse Gym:\n");
            foreach (var t in trainers)
            {
                sb.AppendLine($"- **[{t.User?.FullName ?? "HLV"}](/huan-luyen-vien/{t.TrainerId})** — {ShortSpec(t.Specialization)}");
            }
            sb.Append("\nBấm vào tên để xem hồ sơ và lịch dạy.");
            return sb.ToString();
        }

        private static string TrainerDetail(Trainer t) =>
            $"**{t.User?.FullName ?? "Huấn luyện viên"}**\n" +
            $"- Chuyên môn: {t.Specialization}\n" +
            (string.IsNullOrWhiteSpace(t.Bio) ? "" : $"- {t.Bio}\n") +
            $"\nXem hồ sơ đầy đủ tại [trang HLV](/huan-luyen-vien/{t.TrainerId}).";

        private static string FacilityList()
        {
            var sb = new StringBuilder("Các **khu vực tập** tại Pulse Gym:\n");
            foreach (var f in FacilityData.All)
            {
                sb.AppendLine($"- **[{f.Name}](/khu-vuc/{f.Slug})** — {f.Area}, {f.Location}, sức chứa {f.Capacity}");
            }
            sb.Append($"\nMở cửa {OpeningHours}.");
            return sb.ToString();
        }

        // ------------------------------------------------------------------
        // Tiện ích
        // ------------------------------------------------------------------
        private static GymPackage? FindPackage(List<GymPackage> packages, HashSet<string> tokens, string norm)
        {
            var mentionsPackage = tokens.Contains("goi") || tokens.Contains("basic") || tokens.Contains("standard")
                                  || tokens.Contains("vip") || tokens.Contains("vvip");
            if (!mentionsPackage) return null;

            foreach (var p in packages)
            {
                var name = Norm(p.PackageName);
                // lấy phần trước dấu "(" ví dụ "goi dong"
                var shortName = name.Split('(')[0].Trim();
                var key = shortName.Replace("goi ", "").Trim(); // "dong", "bac", "vang", "kim cuong"
                if (key.Length > 0 && (norm.Contains(shortName) || (key.Contains(' ') ? norm.Contains(key) : tokens.Contains(key))))
                    return p;
            }
            if (tokens.Contains("vvip")) return packages.FirstOrDefault(p => Norm(p.PackageName).Contains("vvip"));
            return null;
        }

        private static List<string> S(params string[] items) => items.ToList();

        private static bool Has(HashSet<string> tokens, string norm, params string[] keys)
        {
            foreach (var k in keys)
            {
                if (k.Contains(' ') || k.Length >= 6) { if (norm.Contains(k)) return true; }
                else if (tokens.Contains(k)) return true;
            }
            return false;
        }

        private static string Norm(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            var decomposed = s.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(decomposed.Length);
            foreach (var c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) sb.Append(c);
            }
            return sb.ToString().Replace('đ', 'd').Normalize(NormalizationForm.FormC);
        }

        private static string Vnd(decimal v) => v.ToString("N0", Vi) + "đ";

        private static decimal MonthlyPrice(GymPackage p) =>
            p.DurationDays > 0 ? Math.Round(p.Price / p.DurationDays * 30) : p.Price;

        private static string DurationLabel(int days) => days switch
        {
            30 => "1 tháng",
            90 => "3 tháng",
            180 => "6 tháng",
            365 => "1 năm",
            _ => days + " ngày"
        };

        private static string ShortSpec(string spec)
        {
            var s = spec.Split('(')[0].Trim();
            return s.Length > 70 ? s[..70].TrimEnd() + "…" : s;
        }
    }
}
