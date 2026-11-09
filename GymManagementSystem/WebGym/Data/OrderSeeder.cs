using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class OrderSeeder
    {
        public static void SeedOrders(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(GetData());
        }

        public static List<Order> GetData()
        {
            return new List<Order>
            {
                new Order
                {
                    OrderId = 1,
                    MemberId = 1,
                    UserId = 5,
                    OrderDate = new DateTime(2026, 10, 15, 14, 30, 0),
                    TotalAmount = 2030000m,
                    Status = "Hoàn thành",
                    PaymentMethod = "VNPay",
                    ShippingAddress = "Nhận trực tiếp tại quầy Lễ tân Gym",
                    PhoneNumber = "0987654321",
                    Notes = "Đã thanh toán qua cổng VNPay thành công."
                },
                new Order
                {
                    OrderId = 2,
                    MemberId = 2,
                    UserId = 6,
                    OrderDate = new DateTime(2026, 10, 20, 9, 15, 0),
                    TotalAmount = 1150000m,
                    Status = "Hoàn thành",
                    PaymentMethod = "Chuyển khoản",
                    ShippingAddress = "123 Hoàng Quốc Việt, Cầu Giấy, Hà Nội",
                    PhoneNumber = "0987654322",
                    Notes = "Giao hàng giờ hành chính."
                },
                new Order
                {
                    OrderId = 3,
                    MemberId = 3,
                    UserId = 7,
                    OrderDate = new DateTime(2026, 10, 28, 16, 45, 0),
                    TotalAmount = 1240000m,
                    Status = "Hoàn thành",
                    PaymentMethod = "Tiền mặt",
                    ShippingAddress = "Nhận trực tiếp tại quầy Lễ tân Gym",
                    PhoneNumber = "0987654323",
                    Notes = "Thanh toán tiền mặt cho thu ngân."
                },
                new Order
                {
                    OrderId = 4,
                    MemberId = 4,
                    UserId = 8,
                    OrderDate = new DateTime(2026, 11, 2, 10, 0, 0),
                    TotalAmount = 950000m,
                    Status = "Đang giao",
                    PaymentMethod = "Momo",
                    ShippingAddress = "Số 45 Trần Phú, Ba Đình, Hà Nội",
                    PhoneNumber = "0987654324",
                    Notes = "Đóng gói bọc xốp cẩn thận."
                },
                new Order
                {
                    OrderId = 5,
                    MemberId = 5,
                    UserId = 9,
                    OrderDate = new DateTime(2026, 11, 6, 18, 20, 0),
                    TotalAmount = 2470000m,
                    Status = "Chờ xác nhận",
                    PaymentMethod = "VNPay",
                    ShippingAddress = "Số 88 Lê Duẩn, Hoàn Kiếm, Hà Nội",
                    PhoneNumber = "0987654325",
                    Notes = "Hội viên đặt mua combo tăng cơ nhanh."
                }
            };
        }
    }
}
