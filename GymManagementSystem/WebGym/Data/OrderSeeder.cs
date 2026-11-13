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
                },

                // === BỔ SUNG ĐƠN HÀNG POS TRẢI RỘNG CÁC THÁNG NĂM 2026 ===
                // Tháng 1/2026
                new Order { OrderId = 6, MemberId = 1, UserId = 5, OrderDate = new DateTime(2026, 1, 18, 17, 15, 0), TotalAmount = 2030000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654321", Notes = "Đơn POS đầu năm: Whey Gold + Bình lắc." },
                new Order { OrderId = 7, MemberId = 24, UserId = 24, OrderDate = new DateTime(2026, 1, 27, 19, 0, 0), TotalAmount = 1240000m, Status = "Đã thanh toán", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654329", Notes = "Mua phụ kiện đai lưng + găng tay tại quầy." },
                // Tháng 2/2026
                new Order { OrderId = 8, MemberId = 21, UserId = 21, OrderDate = new DateTime(2026, 2, 14, 10, 30, 0), TotalAmount = 1890000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654326", Notes = "Rule 1 Protein Isolate tại quầy lễ tân." },
                new Order { OrderId = 9, MemberId = 22, UserId = 22, OrderDate = new DateTime(2026, 2, 23, 18, 45, 0), TotalAmount = 1370000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654327", Notes = "Creatine + Pre-Workout C4." },
                // Tháng 3/2026
                new Order { OrderId = 10, MemberId = 3, UserId = 7, OrderDate = new DateTime(2026, 3, 9, 16, 20, 0), TotalAmount = 2330000m, Status = "Hoàn thành", PaymentMethod = "Chuyển khoản", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654323", Notes = "Whey Gold + Creatine." },
                new Order { OrderId = 11, MemberId = 25, UserId = 25, OrderDate = new DateTime(2026, 3, 24, 19, 10, 0), TotalAmount = 950000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654330", Notes = "Áo thun GymTech + Hộp bánh Quest Bar." },
                // Tháng 4/2026
                new Order { OrderId = 12, MemberId = 23, UserId = 23, OrderDate = new DateTime(2026, 4, 11, 11, 15, 0), TotalAmount = 1430000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654328", Notes = "BCAA Xtend + Bình lắc BlenderBottle." },
                new Order { OrderId = 13, MemberId = 27, UserId = 27, OrderDate = new DateTime(2026, 4, 25, 17, 40, 0), TotalAmount = 2470000m, Status = "Đã thanh toán", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654332", Notes = "Rule 1 Isolate + Creatine Platinum." },
                // Tháng 5/2026
                new Order { OrderId = 14, MemberId = 1, UserId = 5, OrderDate = new DateTime(2026, 5, 12, 18, 0, 0), TotalAmount = 2540000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654321", Notes = "Whey Gold + Pre-Workout C4." },
                new Order { OrderId = 15, MemberId = 31, UserId = 31, OrderDate = new DateTime(2026, 5, 28, 15, 50, 0), TotalAmount = 1240000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654336", Notes = "Đai lưng Lever Belt + Găng tay Harbinger." },
                // Tháng 6/2026
                new Order { OrderId = 16, MemberId = 2, UserId = 6, OrderDate = new DateTime(2026, 6, 8, 9, 45, 0), TotalAmount = 1840000m, Status = "Hoàn thành", PaymentMethod = "Chuyển khoản", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654322", Notes = "BCAA Xtend + Hộp Quest Bar." },
                new Order { OrderId = 17, MemberId = 29, UserId = 29, OrderDate = new DateTime(2026, 6, 21, 18, 30, 0), TotalAmount = 2150000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654334", Notes = "Rule 1 Isolate + Áo thun GymTech." },
                // Tháng 7/2026
                new Order { OrderId = 18, MemberId = 22, UserId = 22, OrderDate = new DateTime(2026, 7, 14, 17, 20, 0), TotalAmount = 2330000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654327", Notes = "Whey Gold + Creatine Monohydrate." },
                new Order { OrderId = 19, MemberId = 33, UserId = 33, OrderDate = new DateTime(2026, 7, 29, 19, 15, 0), TotalAmount = 1070000m, Status = "Đã thanh toán", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654338", Notes = "Pre-Workout C4 + Bình lắc BlenderBottle." },
                // Tháng 8/2026
                new Order { OrderId = 20, MemberId = 24, UserId = 24, OrderDate = new DateTime(2026, 8, 10, 16, 10, 0), TotalAmount = 2740000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654329", Notes = "Rule 1 Isolate + Đai lưng Lever Belt." },
                new Order { OrderId = 21, MemberId = 30, UserId = 30, OrderDate = new DateTime(2026, 8, 22, 10, 25, 0), TotalAmount = 1380000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654335", Notes = "2 hộp bánh Protein Quest Bar tại quầy." },
                // Tháng 9/2026
                new Order { OrderId = 22, MemberId = 21, UserId = 21, OrderDate = new DateTime(2026, 9, 12, 17, 0, 0), TotalAmount = 1940000m, Status = "Hoàn thành", PaymentMethod = "Chuyển khoản", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654326", Notes = "BCAA Xtend + Pre-Workout C4." },
                new Order { OrderId = 23, MemberId = 37, UserId = 37, OrderDate = new DateTime(2026, 9, 26, 18, 40, 0), TotalAmount = 2140000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654342", Notes = "Whey Gold + Găng tay Harbinger." },
                // Tháng 11/2026 (đã hoàn thành tại quầy POS)
                new Order { OrderId = 24, MemberId = 1, UserId = 5, OrderDate = new DateTime(2026, 11, 5, 17, 30, 0), TotalAmount = 2470000m, Status = "Hoàn thành", PaymentMethod = "VNPay", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654321", Notes = "Rule 1 Isolate + Creatine Platinum mua tại quầy." },
                new Order { OrderId = 25, MemberId = 3, UserId = 7, OrderDate = new DateTime(2026, 11, 10, 18, 50, 0), TotalAmount = 1780000m, Status = "Hoàn thành", PaymentMethod = "Tiền mặt", ShippingAddress = "Mua trực tiếp tại quầy POS", PhoneNumber = "0987654323", Notes = "Đai lưng Lever Belt + Găng tay + Áo GymTech + Bình lắc." }
            };
        }
    }
}
