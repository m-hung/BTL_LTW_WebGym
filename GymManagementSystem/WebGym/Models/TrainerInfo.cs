using System.Text.RegularExpressions;
using WebGym.Models.Entities;

namespace WebGym.Models
{
    public record Achievement(string Title, string Detail);
    public record TrainerSlot(string Day, string Time, string ClassName, int Spots);

    public class TrainerInfo
    {
        public int TrainerId { get; init; }
        public string Slug { get; init; } = "";
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string FullName { get; init; } = "";
        public string Role { get; init; } = "";
        public string Badge { get; init; } = "";
        public string Image { get; init; } = "";
        public string Bio { get; init; } = "";
        public string Quote { get; init; } = "";
        public string Years { get; init; } = "5+";
        public string Clients { get; init; } = "150+";
        public string ClassesPerWeek { get; init; } = "6";
        public List<string> Specialties { get; init; } = new();
        public List<Achievement> Achievements { get; init; } = new();
        public List<string> AreaSlugs { get; init; } = new();
        public List<TrainerSlot> Slots { get; init; } = new();

        public static TrainerInfo FromTrainer(Trainer t)
        {
            var fullName = t.User?.FullName ?? "Huấn luyện viên";
            var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string firstName = nameParts.Length > 1 ? string.Join(" ", nameParts.Take(nameParts.Length - 1)) : fullName;
            string lastName = nameParts.Length > 1 ? nameParts.Last() : "";

            // Trích xuất vai trò rút gọn
            string role = t.Specialization;
            if (role.Contains('('))
            {
                var beforeParen = role.Split('(')[0].Trim();
                if (!string.IsNullOrWhiteSpace(beforeParen))
                {
                    role = beforeParen;
                }
            }
            if (role.Length > 30 && role.Contains('&'))
            {
                role = role.Split('&')[0].Trim();
            }

            // Trích xuất Badge từ Bio hoặc Specialization
            string badge = "HLV Chuyên nghiệp";
            string bio = t.Bio ?? "";
            if (bio.Contains("năm kinh nghiệm", StringComparison.OrdinalIgnoreCase))
            {
                var match = Regex.Match(bio, @"(\d+)\s*năm kinh nghiệm", RegexOptions.IgnoreCase);
                badge = match.Success ? $"{match.Groups[1].Value}+ năm kinh nghiệm" : "5+ năm kinh nghiệm";
            }
            else if (bio.Contains("NASM", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("NASM", StringComparison.OrdinalIgnoreCase))
            {
                badge = "NASM Certified";
            }
            else if (bio.Contains("phục hồi", StringComparison.OrdinalIgnoreCase))
            {
                badge = "Chuyên gia phục hồi";
            }
            else if (bio.Contains("quốc gia", StringComparison.OrdinalIgnoreCase))
            {
                badge = "Vô địch toàn quốc";
            }

            // Ảnh đại diện
            string image = !string.IsNullOrEmpty(t.AvatarUrl) ? t.AvatarUrl : $"coach{t.TrainerId}.jpg";

            // Số năm kinh nghiệm
            string years = "5+";
            var yMatch = Regex.Match(bio, @"(\d+)\s*năm kinh nghiệm", RegexOptions.IgnoreCase);
            if (yMatch.Success)
            {
                years = $"{yMatch.Groups[1].Value}+";
            }

            // Triết lý tập luyện
            string quote = "Sức mạnh và sự bền bỉ được rèn luyện từ sự kiên trì đúng kỹ thuật mỗi ngày.";
            if (t.Specialization.Contains("HIIT", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("Cardio", StringComparison.OrdinalIgnoreCase))
            {
                quote = "Mười lăm phút khó nhất sẽ đem lại cho bạn nguồn năng lượng bứt phá cả ngày.";
            }
            else if (t.Specialization.Contains("Yoga", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("Pilates", StringComparison.OrdinalIgnoreCase))
            {
                quote = "Lắng nghe cơ thể, điều chỉnh từng hơi thở để giải phóng áp lực và tìm lại sự cân bằng.";
            }
            else if (t.Specialization.Contains("Boxing", StringComparison.OrdinalIgnoreCase))
            {
                quote = "Mỗi cú đấm đẹp bắt đầu từ đôi chân vững và một cái đầu tập trung điềm tĩnh.";
            }

            // Danh sách chuyên môn
            var specs = new List<string>();
            var specParts = t.Specialization.Split(new[] { ',', '&', ';', '(' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var sp in specParts)
            {
                var clean = sp.TrimEnd(')').Trim();
                if (clean.Length > 3 && !specs.Contains(clean))
                {
                    specs.Add(clean);
                }
            }
            if (!specs.Contains("Lên giáo án dinh dưỡng cá nhân")) specs.Add("Lên giáo án dinh dưỡng cá nhân");
            if (!specs.Contains("Theo dõi chỉ số InBody")) specs.Add("Theo dõi chỉ số InBody");

            // Danh sách thành tích
            var achievements = new List<Achievement>();
            if (!string.IsNullOrWhiteSpace(t.Bio))
            {
                achievements.Add(new("Kinh nghiệm huấn luyện", t.Bio));
            }
            achievements.Add(new("Chuyên môn chứng nhận", $"Chứng nhận chuyên gia {role} chuẩn quốc tế."));
            achievements.Add(new("Đồng hành học viên", "Đã hướng dẫn và hỗ trợ thành công hơn 150+ học viên đạt mục tiêu thể hình."));

            // Khu vực tập luyện tương ứng
            var areaSlugs = new List<string>();
            if (t.Specialization.Contains("Bodybuilding", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("Tăng cơ", StringComparison.OrdinalIgnoreCase))
            {
                areaSlugs.Add("khu-ta-tu-do");
            }
            if (t.Specialization.Contains("Cardio", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("HIIT", StringComparison.OrdinalIgnoreCase))
            {
                areaSlugs.Add("khu-cardio");
            }
            if (t.Specialization.Contains("Boxing", StringComparison.OrdinalIgnoreCase))
            {
                areaSlugs.Add("phong-boxing");
            }
            if (t.Specialization.Contains("Yoga", StringComparison.OrdinalIgnoreCase) || t.Specialization.Contains("Pilates", StringComparison.OrdinalIgnoreCase))
            {
                areaSlugs.Add("studio-yoga");
            }
            if (areaSlugs.Count == 0) areaSlugs.Add("khu-ta-tu-do");

            // Lịch dạy tuần
            var slots = new List<TrainerSlot>
            {
                new("Thứ 2", "06:30", role, 6),
                new("Thứ 4", "18:00", role, 8),
                new("Thứ 6", "19:30", role, 6)
            };

            return new TrainerInfo
            {
                TrainerId = t.TrainerId,
                Slug = t.TrainerId.ToString(),
                FirstName = firstName,
                LastName = lastName,
                FullName = fullName,
                Role = role,
                Badge = badge,
                Image = image,
                Bio = t.Bio ?? "Huấn luyện viên chuyên nghiệp tại Pulse Gym.",
                Quote = quote,
                Years = years,
                Clients = "180+",
                ClassesPerWeek = "6",
                Specialties = specs,
                Achievements = achievements,
                AreaSlugs = areaSlugs,
                Slots = slots
            };
        }
    }
}
