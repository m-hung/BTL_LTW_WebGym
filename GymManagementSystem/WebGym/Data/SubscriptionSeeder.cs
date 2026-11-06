using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class SubscriptionSeeder
    {
        public static void SeedSubscriptions(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>().HasData(GetData());
        }

        public static List<Subscription> GetData()
        {
            return new List<Subscription>
            {
                new Subscription
                {
                    SubscriptionId = 1,
                    MemberId = 3, // Thắng
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 8, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 2,
                    MemberId = 2, // Hương
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 3,
                    MemberId = 5, // Nam
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 19, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 4,
                    MemberId = 1, // Hải
                    PackageId = 3, // Gói Vàng 4.5tr kèm PT
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 5,
                    MemberId = 3, // Thắng gia hạn VIP
                    PackageId = 3, // Gói Vàng 4.5tr
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 6,
                    MemberId = 5, // Nam nâng cấp Kim Cương
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 7,
                    MemberId = 4, // Phương Anh
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 8,
                    MemberId = 4, // Phương Anh gia hạn Gói Bạc
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 10, 6, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 4, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                // Dữ liệu bổ sung cho Hội viên Hải (MemberId 1)
                new Subscription
                {
                    SubscriptionId = 21,
                    MemberId = 1, // Hải khởi đầu gói cơ bản
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 2, 14, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 22,
                    MemberId = 1, // Hải nâng cấp gói 3 tháng
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 23,
                    MemberId = 1, // Hải gia hạn trước khi lên VIP kèm PT
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                // Gói tập cho các hội viên mới
                new Subscription
                {
                    SubscriptionId = 24,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 25,
                    MemberId = 21, // Bảo Ngọc nâng cấp VIP kèm PT
                    PackageId = 3, // Gói Vàng 4.5tr
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 5, 18, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 11, 14, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 26,
                    MemberId = 22, // Hoàng Long
                    PackageId = 3, // Gói Vàng 4.5tr
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 9, 8, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 27,
                    MemberId = 22, // Hoàng Long lên Kim Cương 1 năm
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 9, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 9, 10, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 28,
                    MemberId = 23, // Mai Linh
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 29,
                    MemberId = 23, // Mai Linh gia hạn Gói Bạc
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 5, 11, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 9, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 30,
                    MemberId = 23, // Mai Linh nâng cấp Gói Vàng kèm PT
                    PackageId = 3, // Gói Vàng 4.5tr
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 31,
                    MemberId = 24, // Hữu Đức gói Kim Cương trọn năm
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 12, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                }
            };
        }
    }
}
