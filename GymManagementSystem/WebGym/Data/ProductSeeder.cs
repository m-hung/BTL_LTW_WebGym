using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ProductSeeder
    {
        public static void SeedProducts(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(GetData());
        }

        public static List<Product> GetData()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    CategoryId = 1,
                    ProductName = "Whey Gold Standard 100% Protein (Optimum Nutrition - 5 Lbs)",
                    ImageUrl = "~/images/products/whey-gold-standard.jpg",
                    Price = 1750000m,
                    StockQuantity = 45,
                    Description = "Sữa tăng cơ huyền thoại thế giới với 24g Protein mỗi lần dùng, 5.5g BCAA tự nhiên, ít đường và chất béo.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 2,
                    CategoryId = 1,
                    ProductName = "Rule 1 Protein Isolate & Hydrolyzed (Rule One - 5 Lbs)",
                    ImageUrl = "~/images/products/rule1-isolate.jpg",
                    Price = 1890000m,
                    StockQuantity = 38,
                    Description = "100% Whey Isolate và Hydrolyzed siêu tinh khiết, 0 đường, 0 fat, hấp thu cực nhanh sau khi tập nặng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 3,
                    CategoryId = 2,
                    ProductName = "Creatine Monohydrate Platinum 400g (MuscleTech)",
                    ImageUrl = "~/images/products/muscletech-creatine.jpg",
                    Price = 580000m,
                    StockQuantity = 60,
                    Description = "Bổ sung 5g Creatine siêu mịn Micronized mỗi muỗng, tăng thể tích tế bào cơ, bộc phát sức mạnh tối đa.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 4,
                    CategoryId = 2,
                    ProductName = "C4 Original Pre-Workout Năng Lượng Đỉnh Cao (Cellucor - 60 Servings)",
                    ImageUrl = "~/images/products/c4-preworkout.jpg",
                    Price = 790000m,
                    StockQuantity = 25,
                    Description = "Tăng tỉnh táo tập trung và sung mãn năng lượng với Beta-Alanine, Creatine Nitrate và 150mg Caffeine.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 5,
                    CategoryId = 2,
                    ProductName = "BCAA Xtend Bù Khoáng Điện Giải (Scivation - 90 Servings)",
                    ImageUrl = "~/images/products/xtend-bcaa.jpg",
                    Price = 1150000m,
                    StockQuantity = 30,
                    Description = "7g BCAA tỉ lệ vàng 2:1:1 kết hợp điện giải dừa tươi, chống dị hóa cơ và giảm đau nhức sau tập.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 6,
                    CategoryId = 3,
                    ProductName = "Đai Lưng Tập Gym Da Bò Thật Khóa Đòn Bẩy (Lever Belt 10mm)",
                    ImageUrl = "~/images/products/lever-belt.jpg",
                    Price = 850000m,
                    StockQuantity = 20,
                    Description = "Đai lưng da thật dày 10mm với khóa đòn bẩy thép nguyên khối, bảo vệ cột sống thắt lưng khi gánh tạ nặng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 7,
                    CategoryId = 3,
                    ProductName = "Găng Tay Tập Gym Thoáng Khí Kèm Quấn Cổ Tay (Harbinger Pro)",
                    ImageUrl = "~/images/products/harbinger-gloves.jpg",
                    Price = 390000m,
                    StockQuantity = 50,
                    Description = "Lớp đệm lòng bàn tay chống chai tay, đai quấn cổ tay dài 45cm bảo vệ khớp khi đẩy ngực nặng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 8,
                    CategoryId = 3,
                    ProductName = "Bình Lắc Shaker BlenderBottle Pro45 (Dung tích 1300ml)",
                    ImageUrl = "~/images/products/blender-bottle.jpg",
                    Price = 280000m,
                    StockQuantity = 75,
                    Description = "Chất liệu nhựa Tritan kháng khuẩn không mùi BPA Free, kèm quả cầu xoắn inox đánh tan bột whey siêu mịn.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 9,
                    CategoryId = 4,
                    ProductName = "Áo Thun Tập Gym Nam Co Giãn 4 Chiều GymTech Pro",
                    ImageUrl = "~/images/products/gymtech-shirt.svg",
                    Price = 260000m,
                    StockQuantity = 40,
                    Description = "Vải Spandex pha Polyester thể thao thấm hút mồ hôi nhanh, tôn dáng cơ bắp và chống nhăn co rút.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 10,
                    CategoryId = 5,
                    ProductName = "Hộp Bánh Protein Bar Quest Nutrition (Hộp 12 Thanh x 60g)",
                    ImageUrl = "~/images/products/quest-bar.svg",
                    Price = 690000m,
                    StockQuantity = 35,
                    Description = "21g Protein chất lượng cao mỗi thanh, chỉ 1g đường và dồi dào chất xơ prebiotic, bữa phụ hoàn hảo lành mạnh.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc)
                }
            };
        }
    }
}
