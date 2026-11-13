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
                },
                new Product
                {
                    ProductId = 11,
                    CategoryId = 5,
                    ProductName = "Nước khoáng bù điện giải Pocari Sweat (Chai 500ml)",
                    ImageUrl = "~/images/products/pocari-sweat.svg",
                    Price = 18000m,
                    StockQuantity = 120,
                    Description = "Bù nước và ion khoáng tức thì (Na+, K+, Ca2+, Mg2+) giúp cơ thể phục hồi thể lực và giải khát nhanh trong buổi tập.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 12,
                    CategoryId = 5,
                    ProductName = "Nước tăng lực Monster Energy Ultra White Không Đường (Lon 355ml)",
                    ImageUrl = "~/images/products/monster-energy.svg",
                    Price = 38000m,
                    StockQuantity = 85,
                    Description = "0 Calories, 0 Đường, bổ sung Taurine, Nhân sâm, Vitamin B và 140mg Caffeine kích thích năng lượng tập bùng nổ.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 13,
                    CategoryId = 5,
                    ProductName = "Nước uống vận động Revive Isotonic Chanh Muối (Chai 500ml)",
                    ImageUrl = "~/images/products/revive.svg",
                    Price = 15000m,
                    StockQuantity = 150,
                    Description = "Công thức bù khoáng Isotonic kết hợp Vitamin B3, B6, B12 và muối khoáng, xua tan cơn khát sau các bài tập Cardio cường độ cao.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 26, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 14,
                    CategoryId = 5,
                    ProductName = "Sữa lắc Protein uống liền Myprotein RTD vị Chocolate (Chai 330ml)",
                    ImageUrl = "~/images/products/myprotein-rtd.svg",
                    Price = 68000m,
                    StockQuantity = 40,
                    Description = "Nạp ngay 30g Protein siêu tinh khiết sau buổi tập, ít béo, không thêm đường, hương vị cacao socola thơm ngon.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 26, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 15,
                    CategoryId = 5,
                    ProductName = "Bánh Cookie Protein Myprotein Baked Double Chocolate (Bánh 75g)",
                    ImageUrl = "~/images/products/protein-cookie.svg",
                    Price = 48000m,
                    StockQuantity = 60,
                    Description = "Bánh quy protein nướng mềm với hạt socola chip hảo hạng, cung cấp tới 38g Protein thực vật cho bữa phụ giàu dinh dưỡng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 27, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 16,
                    CategoryId = 5,
                    ProductName = "Nước khoáng thiên nhiên ướp lạnh Aquafina (Chai 500ml)",
                    ImageUrl = "~/images/products/aquafina.svg",
                    Price = 10000m,
                    StockQuantity = 200,
                    Description = "Nước tinh khiết tiệt trùng Hydro-7, mát lạnh giải khát sảng khoái tức thì trong lúc tập luyện.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 27, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 17,
                    CategoryId = 3,
                    ProductName = "Dây kéo lưng nâng tạ Lifting Straps Da Bò Thật (Rogue Fitness)",
                    ImageUrl = "~/images/products/lifting-straps.svg",
                    Price = 195000m,
                    StockQuantity = 35,
                    Description = "Chất liệu da bò thật 100% êm ái, bám chắc thanh đòn, hỗ trợ kéo lưng xô Deadlift và Barbell Row an toàn không tuột tay.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 28, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 18,
                    CategoryId = 3,
                    ProductName = "Băng quấn khớp gối Knee Wraps trợ lực Squat chuyên nghiệp 2m (Aolikes)",
                    ImageUrl = "~/images/products/knee-wraps.svg",
                    Price = 230000m,
                    StockQuantity = 25,
                    Description = "Chất liệu thun cotton dày dặn co giãn trợ lực mạnh mẽ, cố định khớp gối chống chấn thương khi gánh tạ mức tạ nặng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 28, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 19,
                    CategoryId = 3,
                    ProductName = "Phấn nước thể thao chống trượt tay Liquid Chalk (Chai 250ml)",
                    ImageUrl = "~/images/products/liquid-chalk.svg",
                    Price = 145000m,
                    StockQuantity = 45,
                    Description = "Khô nhanh trong 5 giây, bám tạ siêu chắc, không bụi bay lung tung, hạn chế mồ hôi tay khi tập xà đơn và tạ nặng.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 29, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 20,
                    CategoryId = 3,
                    ProductName = "Bộ 5 dây kháng lực tập mông đùi Mini Band cao su tự nhiên",
                    ImageUrl = "~/images/products/resistance-bands.svg",
                    Price = 135000m,
                    StockQuantity = 50,
                    Description = "5 cấp độ kháng lực từ Rất nhẹ (X-Light) đến Siêu nặng (X-Heavy), thích hợp khởi động khớp hông và kích hoạt nhóm cơ mông.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 29, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 21,
                    CategoryId = 3,
                    ProductName = "Khăn tập Gym sợi Microfiber thấm hút kháng khuẩn (Pulse Gym Edition)",
                    ImageUrl = "~/images/products/gym-towel.svg",
                    Price = 65000m,
                    StockQuantity = 110,
                    Description = "Kích thước 40x80cm, sợi Microfiber siêu mịn thấm mồ hôi gấp 5 lần cotton, in logo Pulse Gym dạ quang cao cấp.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    ProductId = 22,
                    CategoryId = 5,
                    ProductName = "Gel năng lượng GU Energy Roctane Gel vị Cà phê Bổ sung BCAA (Gói 32g)",
                    ImageUrl = "~/images/products/gu-energy-gel.svg",
                    Price = 45000m,
                    StockQuantity = 80,
                    Description = "Gel năng lượng nạp carb hấp thu nhanh kèm 35mg caffeine và amino acid, chống chuột rút và duy trì sức bền trong buổi tập dài.",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc)
                }
            };
        }
    }
}
