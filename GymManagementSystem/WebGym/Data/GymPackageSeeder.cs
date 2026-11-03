using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class GymPackageSeeder
    {
        public static void SeedGymPackages(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymPackage>().HasData(GetData());
        }

        public static List<GymPackage> GetData()
        {
            return new List<GymPackage>
            {
                new GymPackage
                {
                    PackageId = 1,
                    PackageName = "Gói Đồng (Basic 1 Tháng)",
                    Description = "Gói tập cơ bản 30 ngày, sử dụng toàn bộ trang thiết bị Gym hiện đại, tủ locker và phòng tắm nước nóng.",
                    Price = 500000m,
                    DurationDays = 30,
                    IncludesPT = false
                },
                new GymPackage
                {
                    PackageId = 2,
                    PackageName = "Gói Bạc (Standard 3 Tháng)",
                    Description = "Gói tập 90 ngày tiết kiệm, không giới hạn khung giờ tập, miễn phí đo chỉ số cơ thể định kỳ (InBody).",
                    Price = 1300000m,
                    DurationDays = 90,
                    IncludesPT = false
                },
                new GymPackage
                {
                    PackageId = 3,
                    PackageName = "Gói Vàng (VIP 6 Tháng kèm PT)",
                    Description = "Gói tập 180 ngày cao cấp, tập kèm PT không giới hạn, sử dụng phòng xông hơi Sauna & bể bơi nước ấm.",
                    Price = 4500000m,
                    DurationDays = 180,
                    IncludesPT = true
                },
                new GymPackage
                {
                    PackageId = 4,
                    PackageName = "Gói Kim Cương (VVIP 1 Năm kèm PT)",
                    Description = "Gói tập trọn gói 365 ngày đặc quyền tối cao, tập kèm PT không giới hạn, tư vấn dinh dưỡng AI và khăn nước riêng.",
                    Price = 8000000m,
                    DurationDays = 365,
                    IncludesPT = true
                }
            };
        }
    }
}
