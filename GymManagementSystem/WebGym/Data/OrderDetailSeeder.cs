using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class OrderDetailSeeder
    {
        public static void SeedOrderDetails(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>().HasData(GetData());
        }

        public static List<OrderDetail> GetData()
        {
            return new List<OrderDetail>
            {
                // Order 1: Whey Gold + Shaker
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    UnitPrice = 1750000m,
                    TotalPrice = 1750000m
                },
                new OrderDetail
                {
                    OrderDetailId = 2,
                    OrderId = 1,
                    ProductId = 8,
                    Quantity = 1,
                    UnitPrice = 280000m,
                    TotalPrice = 280000m
                },

                // Order 2: BCAA Xtend
                new OrderDetail
                {
                    OrderDetailId = 3,
                    OrderId = 2,
                    ProductId = 5,
                    Quantity = 1,
                    UnitPrice = 1150000m,
                    TotalPrice = 1150000m
                },

                // Order 3: Lever Belt + Harbinger Gloves
                new OrderDetail
                {
                    OrderDetailId = 4,
                    OrderId = 3,
                    ProductId = 6,
                    Quantity = 1,
                    UnitPrice = 850000m,
                    TotalPrice = 850000m
                },
                new OrderDetail
                {
                    OrderDetailId = 5,
                    OrderId = 3,
                    ProductId = 7,
                    Quantity = 1,
                    UnitPrice = 390000m,
                    TotalPrice = 390000m
                },

                // Order 4: GymTech Shirt + Quest Bar
                new OrderDetail
                {
                    OrderDetailId = 6,
                    OrderId = 4,
                    ProductId = 9,
                    Quantity = 1,
                    UnitPrice = 260000m,
                    TotalPrice = 260000m
                },
                new OrderDetail
                {
                    OrderDetailId = 7,
                    OrderId = 4,
                    ProductId = 10,
                    Quantity = 1,
                    UnitPrice = 690000m,
                    TotalPrice = 690000m
                },

                // Order 5: Rule 1 + Creatine MuscleTech
                new OrderDetail
                {
                    OrderDetailId = 8,
                    OrderId = 5,
                    ProductId = 2,
                    Quantity = 1,
                    UnitPrice = 1890000m,
                    TotalPrice = 1890000m
                },
                new OrderDetail
                {
                    OrderDetailId = 9,
                    OrderId = 5,
                    ProductId = 3,
                    Quantity = 1,
                    UnitPrice = 580000m,
                    TotalPrice = 580000m
                },

                // Order 6: Whey Gold (1750k) + Shaker (280k) = 2030k
                new OrderDetail { OrderDetailId = 10, OrderId = 6, ProductId = 1, Quantity = 1, UnitPrice = 1750000m, TotalPrice = 1750000m },
                new OrderDetail { OrderDetailId = 11, OrderId = 6, ProductId = 8, Quantity = 1, UnitPrice = 280000m, TotalPrice = 280000m },
                // Order 7: Belt (850k) + Gloves (390k) = 1240k
                new OrderDetail { OrderDetailId = 12, OrderId = 7, ProductId = 6, Quantity = 1, UnitPrice = 850000m, TotalPrice = 850000m },
                new OrderDetail { OrderDetailId = 13, OrderId = 7, ProductId = 7, Quantity = 1, UnitPrice = 390000m, TotalPrice = 390000m },
                // Order 8: Rule 1 (1890k) = 1890k
                new OrderDetail { OrderDetailId = 14, OrderId = 8, ProductId = 2, Quantity = 1, UnitPrice = 1890000m, TotalPrice = 1890000m },
                // Order 9: Creatine (580k) + C4 (790k) = 1370k
                new OrderDetail { OrderDetailId = 15, OrderId = 9, ProductId = 3, Quantity = 1, UnitPrice = 580000m, TotalPrice = 580000m },
                new OrderDetail { OrderDetailId = 16, OrderId = 9, ProductId = 4, Quantity = 1, UnitPrice = 790000m, TotalPrice = 790000m },
                // Order 10: Whey Gold (1750k) + Creatine (580k) = 2330k
                new OrderDetail { OrderDetailId = 17, OrderId = 10, ProductId = 1, Quantity = 1, UnitPrice = 1750000m, TotalPrice = 1750000m },
                new OrderDetail { OrderDetailId = 18, OrderId = 10, ProductId = 3, Quantity = 1, UnitPrice = 580000m, TotalPrice = 580000m },
                // Order 11: Shirt (260k) + Quest Bar (690k) = 950k
                new OrderDetail { OrderDetailId = 19, OrderId = 11, ProductId = 9, Quantity = 1, UnitPrice = 260000m, TotalPrice = 260000m },
                new OrderDetail { OrderDetailId = 20, OrderId = 11, ProductId = 10, Quantity = 1, UnitPrice = 690000m, TotalPrice = 690000m },
                // Order 12: BCAA (1150k) + Shaker (280k) = 1430k
                new OrderDetail { OrderDetailId = 21, OrderId = 12, ProductId = 5, Quantity = 1, UnitPrice = 1150000m, TotalPrice = 1150000m },
                new OrderDetail { OrderDetailId = 22, OrderId = 12, ProductId = 8, Quantity = 1, UnitPrice = 280000m, TotalPrice = 280000m },
                // Order 13: Rule 1 (1890k) + Creatine (580k) = 2470k
                new OrderDetail { OrderDetailId = 23, OrderId = 13, ProductId = 2, Quantity = 1, UnitPrice = 1890000m, TotalPrice = 1890000m },
                new OrderDetail { OrderDetailId = 24, OrderId = 13, ProductId = 3, Quantity = 1, UnitPrice = 580000m, TotalPrice = 580000m },
                // Order 14: Whey Gold (1750k) + C4 (790k) = 2540k
                new OrderDetail { OrderDetailId = 25, OrderId = 14, ProductId = 1, Quantity = 1, UnitPrice = 1750000m, TotalPrice = 1750000m },
                new OrderDetail { OrderDetailId = 26, OrderId = 14, ProductId = 4, Quantity = 1, UnitPrice = 790000m, TotalPrice = 790000m },
                // Order 15: Belt (850k) + Gloves (390k) = 1240k
                new OrderDetail { OrderDetailId = 27, OrderId = 15, ProductId = 6, Quantity = 1, UnitPrice = 850000m, TotalPrice = 850000m },
                new OrderDetail { OrderDetailId = 28, OrderId = 15, ProductId = 7, Quantity = 1, UnitPrice = 390000m, TotalPrice = 390000m },
                // Order 16: BCAA (1150k) + Quest Bar (690k) = 1840k
                new OrderDetail { OrderDetailId = 29, OrderId = 16, ProductId = 5, Quantity = 1, UnitPrice = 1150000m, TotalPrice = 1150000m },
                new OrderDetail { OrderDetailId = 30, OrderId = 16, ProductId = 10, Quantity = 1, UnitPrice = 690000m, TotalPrice = 690000m },
                // Order 17: Rule 1 (1890k) + Shirt (260k) = 2150k
                new OrderDetail { OrderDetailId = 31, OrderId = 17, ProductId = 2, Quantity = 1, UnitPrice = 1890000m, TotalPrice = 1890000m },
                new OrderDetail { OrderDetailId = 32, OrderId = 17, ProductId = 9, Quantity = 1, UnitPrice = 260000m, TotalPrice = 260000m },
                // Order 18: Whey Gold (1750k) + Creatine (580k) = 2330k
                new OrderDetail { OrderDetailId = 33, OrderId = 18, ProductId = 1, Quantity = 1, UnitPrice = 1750000m, TotalPrice = 1750000m },
                new OrderDetail { OrderDetailId = 34, OrderId = 18, ProductId = 3, Quantity = 1, UnitPrice = 580000m, TotalPrice = 580000m },
                // Order 19: C4 (790k) + Shaker (280k) = 1070k
                new OrderDetail { OrderDetailId = 35, OrderId = 19, ProductId = 4, Quantity = 1, UnitPrice = 790000m, TotalPrice = 790000m },
                new OrderDetail { OrderDetailId = 36, OrderId = 19, ProductId = 8, Quantity = 1, UnitPrice = 280000m, TotalPrice = 280000m },
                // Order 20: Rule 1 (1890k) + Belt (850k) = 2740k
                new OrderDetail { OrderDetailId = 37, OrderId = 20, ProductId = 2, Quantity = 1, UnitPrice = 1890000m, TotalPrice = 1890000m },
                new OrderDetail { OrderDetailId = 38, OrderId = 20, ProductId = 6, Quantity = 1, UnitPrice = 850000m, TotalPrice = 850000m },
                // Order 21: Quest Bar x 2 (690k * 2) = 1380k
                new OrderDetail { OrderDetailId = 39, OrderId = 21, ProductId = 10, Quantity = 2, UnitPrice = 690000m, TotalPrice = 1380000m },
                // Order 22: BCAA (1150k) + C4 (790k) = 1940k
                new OrderDetail { OrderDetailId = 40, OrderId = 22, ProductId = 5, Quantity = 1, UnitPrice = 1150000m, TotalPrice = 1150000m },
                new OrderDetail { OrderDetailId = 41, OrderId = 22, ProductId = 4, Quantity = 1, UnitPrice = 790000m, TotalPrice = 790000m },
                // Order 23: Whey Gold (1750k) + Gloves (390k) = 2140k
                new OrderDetail { OrderDetailId = 42, OrderId = 23, ProductId = 1, Quantity = 1, UnitPrice = 1750000m, TotalPrice = 1750000m },
                new OrderDetail { OrderDetailId = 43, OrderId = 23, ProductId = 7, Quantity = 1, UnitPrice = 390000m, TotalPrice = 390000m },
                // Order 24: Rule 1 (1890k) + Creatine (580k) = 2470k
                new OrderDetail { OrderDetailId = 44, OrderId = 24, ProductId = 2, Quantity = 1, UnitPrice = 1890000m, TotalPrice = 1890000m },
                new OrderDetail { OrderDetailId = 45, OrderId = 24, ProductId = 3, Quantity = 1, UnitPrice = 580000m, TotalPrice = 580000m },
                // Order 25: Belt (850k) + Gloves (390k) + Shirt (260k) + Shaker (280k) -> Wait: 850+390+260+280 = 1780k -> adjust Order 25: Belt(850)+Gloves(390)+Shirt(260) + Quest? No, use 850+390+260 = 1500k OR 850+390+280+220? Wait: 850+390+280 = 1520, + 260 = 1780! Let's update Order 25 or remove Shaker/use 1780k; wait here let's check: Belt(850)+Gloves(390)+Shaker(280) = 1520k, or add Shirt(260k) = 1780k! Let's fix Order 25 TotalAmount to 1780000m!
                new OrderDetail { OrderDetailId = 46, OrderId = 25, ProductId = 6, Quantity = 1, UnitPrice = 850000m, TotalPrice = 850000m },
                new OrderDetail { OrderDetailId = 47, OrderId = 25, ProductId = 7, Quantity = 1, UnitPrice = 390000m, TotalPrice = 390000m },
                new OrderDetail { OrderDetailId = 48, OrderId = 25, ProductId = 9, Quantity = 1, UnitPrice = 260000m, TotalPrice = 260000m },
                new OrderDetail { OrderDetailId = 49, OrderId = 25, ProductId = 8, Quantity = 1, UnitPrice = 280000m, TotalPrice = 280000m }
            };
        }
    }
}
