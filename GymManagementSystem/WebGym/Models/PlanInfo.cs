namespace WebGym.Models
{
    public class PlanInfo
    {
        public string Slug { get; init; } = "";
        public string Name { get; init; } = "";
        public string Description { get; init; } = "";
        public decimal MonthlyPrice { get; init; }          // VNĐ / tháng
        public bool Popular { get; init; }
        public List<string> Features { get; init; } = new();
    }

    // Dữ liệu demo khớp bảng giá ở trang chủ. Sau này có thể lấy từ bảng GymPackages.
    public static class PlanData
    {
        public static PlanInfo? Find(string? slug) =>
            All.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        public static readonly List<PlanInfo> All = new()
        {
            new PlanInfo { Slug="starter", Name="Starter", MonthlyPrice=590000, Description="Bắt đầu hành trình mạnh mẽ hơn.",
                Features=new(){"Tập không giới hạn","Phòng tắm & tủ đồ","Đánh giá thể lực cơ bản"} },
            new PlanInfo { Slug="pro", Name="Pro", MonthlyPrice=990000, Popular=true, Description="Lựa chọn tốt nhất để bứt phá.",
                Features=new(){"Tất cả quyền lợi Starter","Toàn bộ lớp nhóm","2 buổi PT mỗi tháng","Theo dõi chỉ số chuyên sâu"} },
            new PlanInfo { Slug="elite", Name="Elite", MonthlyPrice=1590000, Description="Huấn luyện cá nhân không giới hạn.",
                Features=new(){"Tất cả quyền lợi Pro","8 buổi PT mỗi tháng","Giáo án dinh dưỡng","Khu vực phục hồi riêng"} }
        };
    }
}
