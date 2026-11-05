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
                },
                // Dữ liệu thanh toán phong phú trải dài từ tháng 1 đến tháng 11
                // --- THÁNG 1 ---
                new Payment
                {
                    PaymentId = 21,
                    MemberId = 1, // Hải
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260115091022",
                    PaymentDate = new DateTime(2026, 1, 15, 9, 10, 22, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 22,
                    MemberId = 24, // Hữu Đức
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260112103045",
                    PaymentDate = new DateTime(2026, 1, 12, 10, 30, 45, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 23,
                    MemberId = 22, // Hoàng Long
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260120150000",
                    PaymentDate = new DateTime(2026, 1, 20, 15, 0, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 2 ---
                new Payment
                {
                    PaymentId = 24,
                    MemberId = 1, // Hải
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260215111530",
                    PaymentDate = new DateTime(2026, 2, 15, 11, 15, 30, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 25,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260215084010",
                    PaymentDate = new DateTime(2026, 2, 15, 8, 40, 10, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 26,
                    MemberId = 3, // Thắng
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260224164500",
                    PaymentDate = new DateTime(2026, 2, 24, 16, 45, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 3 ---
                new Payment
                {
                    PaymentId = 27,
                    MemberId = 22, // Hoàng Long
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260312142055",
                    PaymentDate = new DateTime(2026, 3, 12, 14, 20, 55, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 28,
                    MemberId = 2, // Hương
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260318090514",
                    PaymentDate = new DateTime(2026, 3, 18, 9, 5, 14, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 29,
                    MemberId = 5, // Nam
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260328183000",
                    PaymentDate = new DateTime(2026, 3, 28, 18, 30, 0, DateTimeKind.Utc),
                    Status = "Failed"
                },
                // --- THÁNG 4 ---
                new Payment
                {
                    PaymentId = 30,
                    MemberId = 23, // Mai Linh
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260410101500",
                    PaymentDate = new DateTime(2026, 4, 10, 10, 15, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 31,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260418163022",
                    PaymentDate = new DateTime(2026, 4, 18, 16, 30, 22, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 32,
                    MemberId = 24, // Hữu Đức
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260425112000",
                    PaymentDate = new DateTime(2026, 4, 25, 11, 20, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 5 ---
                new Payment
                {
                    PaymentId = 33,
                    MemberId = 1, // Hải
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260517094500",
                    PaymentDate = new DateTime(2026, 5, 17, 9, 45, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 34,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260518103000",
                    PaymentDate = new DateTime(2026, 5, 18, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 35,
                    MemberId = 23, // Mai Linh
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260511140000",
                    PaymentDate = new DateTime(2026, 5, 11, 14, 0, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 6 ---
                new Payment
                {
                    PaymentId = 36,
                    MemberId = 22, // Hoàng Long
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260620164010",
                    PaymentDate = new DateTime(2026, 6, 20, 16, 40, 10, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 37,
                    MemberId = 24, // Hữu Đức
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260628111500",
                    PaymentDate = new DateTime(2026, 6, 28, 11, 15, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 7 ---
                new Payment
                {
                    PaymentId = 38,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260714152000",
                    PaymentDate = new DateTime(2026, 7, 14, 15, 20, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 39,
                    MemberId = 22, // Hoàng Long
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260725181030",
                    PaymentDate = new DateTime(2026, 7, 25, 18, 10, 30, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 8 ---
                new Payment
                {
                    PaymentId = 40,
                    MemberId = 23, // Mai Linh
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260810141500",
                    PaymentDate = new DateTime(2026, 8, 10, 14, 15, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 41,
                    MemberId = 24, // Hữu Đức
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260822093000",
                    PaymentDate = new DateTime(2026, 8, 22, 9, 30, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 9 ---
                new Payment
                {
                    PaymentId = 42,
                    MemberId = 22, // Hoàng Long
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260910085000",
                    PaymentDate = new DateTime(2026, 9, 10, 8, 50, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 43,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20260920172500",
                    PaymentDate = new DateTime(2026, 9, 20, 17, 25, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 10 ---
                new Payment
                {
                    PaymentId = 44,
                    MemberId = 23, // Mai Linh
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261012110500",
                    PaymentDate = new DateTime(2026, 10, 12, 11, 5, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 45,
                    MemberId = 24, // Hữu Đức
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261028164000",
                    PaymentDate = new DateTime(2026, 10, 28, 16, 40, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                // --- THÁNG 11 ---
                new Payment
                {
                    PaymentId = 46,
                    MemberId = 1, // Hải
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261102092000",
                    PaymentDate = new DateTime(2026, 11, 2, 9, 20, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 47,
                    MemberId = 21, // Bảo Ngọc
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "Cash",
                    TransactionCode = "CSH20261103141500",
                    PaymentDate = new DateTime(2026, 11, 3, 14, 15, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 48,
                    MemberId = 22, // Hoàng Long
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261104103000",
                    PaymentDate = new DateTime(2026, 11, 4, 10, 30, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 49,
                    MemberId = 23, // Mai Linh
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261105154500",
                    PaymentDate = new DateTime(2026, 11, 5, 15, 45, 0, DateTimeKind.Utc),
                    Status = "Success"
                }
            };
        }
    }
}
