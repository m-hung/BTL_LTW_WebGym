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
                }
            };
        }
    }
}
