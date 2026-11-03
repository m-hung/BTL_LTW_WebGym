using WebGym.Models.Entities;

namespace WebGym.Models
{
    public class PlanInfo
    {
        public int PackageId { get; init; }
        public string Slug { get; init; } = "";
        public string Name { get; init; } = "";
        public string Description { get; init; } = "";
        public decimal Price { get; init; }
        public decimal MonthlyPrice { get; init; }          // VNĐ / tháng
        public int DurationDays { get; init; }
        public bool IncludesPT { get; init; }
        public bool Popular { get; init; }
        public List<string> Features { get; init; } = new();

        public static PlanInfo FromGymPackage(GymPackage p)
        {
            var monthly = p.DurationDays > 0 ? Math.Round((p.Price / p.DurationDays) * 30) : p.Price;

            var feats = new List<string>();
            if (!string.IsNullOrWhiteSpace(p.Description))
            {
                var parts = p.Description.Split(new[] { '.', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var part in parts)
                {
                    if (part.Length > 3)
                    {
                        feats.Add(part);
                    }
                }
            }

            if (feats.Count == 0)
            {
                feats.Add(p.DurationDays >= 365 ? "Thời hạn 1 năm trọn gói" : $"Thời hạn {p.DurationDays} ngày");
                feats.Add(p.IncludesPT ? "HLV cá nhân (PT) kèm 1:1" : "Tập tự do không giới hạn khung giờ");
                feats.Add("Tủ đồ locker & phòng tắm nóng lạnh");
                if (p.IncludesPT || p.DurationDays >= 90) feats.Add("Đo chỉ số thể lực InBody định kỳ");
                if (p.IncludesPT || p.DurationDays >= 180) feats.Add("Phòng xông hơi Sauna & phục hồi riêng");
            }

            return new PlanInfo
            {
                PackageId = p.PackageId,
                Slug = p.PackageId.ToString(),
                Name = p.PackageName,
                Description = p.Description ?? "",
                Price = p.Price,
                MonthlyPrice = monthly,
                DurationDays = p.DurationDays,
                IncludesPT = p.IncludesPT,
                Popular = p.IsPopular,
                Features = feats
            };
        }
    }

    public static class PlanData
    {
        public static PlanInfo? Find(string? slug) => null;
        public static readonly List<PlanInfo> All = new();
    }
}
