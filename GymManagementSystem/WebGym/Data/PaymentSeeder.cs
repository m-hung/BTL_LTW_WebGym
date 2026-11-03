using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class PaymentSeeder
    {
        public static void SeedPayments(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasData(GetData());
        }

        public static List<Payment> GetData()
        {
            return new List<Payment>
            {
                new Payment
                {
                    PaymentId = 1,
                    MemberId = 3,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260510084512",
                    PaymentDate = new DateTime(2026, 5, 10, 8, 45, 12, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 2,
                    MemberId = 2,
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260615143011",
                    PaymentDate = new DateTime(2026, 6, 15, 14, 30, 11, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 3,
                    MemberId = 5,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260720173000",
                    PaymentDate = new DateTime(2026, 7, 20, 17, 30, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 4,
                    MemberId = 1,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260801091523",
                    PaymentDate = new DateTime(2026, 8, 1, 9, 15, 23, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 5,
                    MemberId = 3,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260810102045",
                    PaymentDate = new DateTime(2026, 8, 10, 10, 20, 45, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 6,
                    MemberId = 5,
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260901091122",
                    PaymentDate = new DateTime(2026, 9, 1, 9, 11, 22, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 7,
                    MemberId = 4,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260905161209",
                    PaymentDate = new DateTime(2026, 9, 5, 16, 12, 9, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 8,
                    MemberId = 4,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261006114530",
                    PaymentDate = new DateTime(2026, 10, 6, 11, 45, 30, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 9,
                    MemberId = 1,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20261025190011",
                    PaymentDate = new DateTime(2026, 10, 25, 19, 0, 11, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 10,
                    MemberId = 2,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261101083015",
                    PaymentDate = new DateTime(2026, 11, 1, 8, 30, 15, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 11,
                    MemberId = 3,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20261015112233",
                    PaymentDate = new DateTime(2026, 10, 15, 11, 22, 33, DateTimeKind.Utc),
                    Status = "Failed"
                }
            };
        }
    }
}
