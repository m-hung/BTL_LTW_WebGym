using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class CheckInSeeder
    {
        public static void SeedCheckIns(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckIn>().HasData(GetData());
        }

        public static List<CheckIn> GetData()
        {
            return new List<CheckIn>
            {
                new CheckIn
                {
                    CheckInId = 1,
                    MemberId = 1,
                    CheckInTime = new DateTime(2026, 11, 1, 6, 30, 0),
                    CheckOutTime = new DateTime(2026, 11, 1, 8, 0, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR tại cổng kiểm soát. Buổi tập ngực sáng sớm."
                },
                new CheckIn
                {
                    CheckInId = 2,
                    MemberId = 1,
                    CheckInTime = new DateTime(2026, 11, 3, 17, 45, 0),
                    CheckOutTime = new DateTime(2026, 11, 3, 19, 15, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR từ ứng dụng WebGym. Tập cùng HLV Trần Minh Nhật."
                },
                new CheckIn
                {
                    CheckInId = 3,
                    MemberId = 1,
                    CheckInTime = new DateTime(2026, 11, 5, 18, 0, 0),
                    CheckOutTime = new DateTime(2026, 11, 5, 19, 30, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR tại cổng quay tự động. Tập chân và bụng."
                },
                new CheckIn
                {
                    CheckInId = 4,
                    MemberId = 2,
                    CheckInTime = new DateTime(2026, 11, 2, 7, 0, 0),
                    CheckOutTime = new DateTime(2026, 11, 2, 8, 15, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR tham gia lớp Yoga sáng."
                },
                new CheckIn
                {
                    CheckInId = 5,
                    MemberId = 2,
                    CheckInTime = new DateTime(2026, 11, 4, 18, 15, 0),
                    CheckOutTime = new DateTime(2026, 11, 4, 19, 45, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR vào phòng Cardio đạp xe Spinning."
                },
                new CheckIn
                {
                    CheckInId = 6,
                    MemberId = 3,
                    CheckInTime = new DateTime(2026, 11, 2, 16, 30, 0),
                    CheckOutTime = new DateTime(2026, 11, 2, 18, 0, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR check-in cổng số 1. Tập xô và lưng dưới."
                },
                new CheckIn
                {
                    CheckInId = 7,
                    MemberId = 3,
                    CheckInTime = new DateTime(2026, 11, 5, 17, 0, 0),
                    CheckOutTime = new DateTime(2026, 11, 5, 18, 30, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR từ điện thoại cá nhân. Tập vai và bắp tay trước."
                },
                new CheckIn
                {
                    CheckInId = 8,
                    MemberId = 4,
                    CheckInTime = new DateTime(2026, 11, 3, 19, 0, 0),
                    CheckOutTime = new DateTime(2026, 11, 3, 20, 30, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR thành viên hợp lệ trên hệ thống."
                },
                new CheckIn
                {
                    CheckInId = 9,
                    MemberId = 5,
                    CheckInTime = new DateTime(2026, 11, 4, 6, 45, 0),
                    CheckOutTime = new DateTime(2026, 11, 4, 8, 0, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR check-in ca sáng. Tập toàn thân Full Body."
                },
                new CheckIn
                {
                    CheckInId = 10,
                    MemberId = 21,
                    CheckInTime = new DateTime(2026, 11, 6, 17, 30, 0),
                    CheckOutTime = new DateTime(2026, 11, 6, 19, 0, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR cổng vào. Cardio máy chạy và xông hơi."
                },
                new CheckIn
                {
                    CheckInId = 11,
                    MemberId = 1,
                    CheckInTime = new DateTime(2026, 11, 7, 17, 30, 0),
                    CheckOutTime = new DateTime(2026, 11, 7, 19, 0, 0),
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR cổng turnstile. Tập ngực nâng cao."
                },
                new CheckIn
                {
                    CheckInId = 12,
                    MemberId = 2,
                    CheckInTime = new DateTime(2026, 11, 8, 8, 0, 0),
                    CheckOutTime = null, // Đang trong phòng tập
                    Method = "Mã QR Code",
                    Notes = "Quét mã QR vào phòng Yoga, hiện đang trong buổi tập."
                }
            };
        }
    }
}
