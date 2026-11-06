using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class BookingSeeder
    {
        public static void SeedBookings(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasData(GetData());
        }

        public static List<Booking> GetData()
        {
            return new List<Booking>
            {
                new Booking
                {
                    BookingId = 1,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 26, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Completed",
                    Notes = "Tập ngực & tay sau (Chest & Triceps focus)"
                },
                new Booking
                {
                    BookingId = 2,
                    MemberId = 2,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 27, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:30 - 19:00",
                    Status = "Completed",
                    Notes = "Cardio HIIT đốt mỡ & giãn cơ phục hồi"
                },
                new Booking
                {
                    BookingId = 3,
                    MemberId = 3,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 28, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "09:00 - 10:30",
                    Status = "Completed",
                    Notes = "Chân mông đùi (Leg day - Squat form check)"
                },
                new Booking
                {
                    BookingId = 4,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 29, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Completed",
                    Notes = "Lưng xô & tay trước (Back & Biceps)"
                },
                new Booking
                {
                    BookingId = 5,
                    MemberId = 5,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 30, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Cancelled",
                    Notes = "Hội viên có việc bận đột xuất xin hoãn ca"
                },
                new Booking
                {
                    BookingId = 6,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 2, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Approved",
                    Notes = "Đo lại chỉ số đầu tháng & tập bài đa khớp compound"
                },
                new Booking
                {
                    BookingId = 7,
                    MemberId = 2,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:30 - 19:00",
                    Status = "Approved",
                    Notes = "Tập mông đùi Glutes hypertrophy"
                },
                new Booking
                {
                    BookingId = 8,
                    MemberId = 4,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 4, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:00 - 16:30",
                    Status = "Pending",
                    Notes = "Hội viên mới đăng ký buổi đầu thẩm định thể lực"
                },
                new Booking
                {
                    BookingId = 9,
                    MemberId = 3,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "10:00 - 11:30",
                    Status = "Pending",
                    Notes = "Tập vai & cơ bụng core strength"
                },
                new Booking
                {
                    BookingId = 10,
                    MemberId = 5,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 6, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Pending",
                    Notes = "Đăng ký buổi tập bù ca tuần trước"
                },
                // Các hội viên khác đặt lịch tập với PT Nhật (TrainerId = 1)
                new Booking
                {
                    BookingId = 21,
                    MemberId = 21, // Bảo Ngọc
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 25, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "07:00 - 08:30",
                    Status = "Completed",
                    Notes = "Tập cơ mông đùi và cơ bụng dưới (Glutes & Core)"
                },
                new Booking
                {
                    BookingId = 22,
                    MemberId = 21, // Bảo Ngọc
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 29, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:00 - 18:30",
                    Status = "Completed",
                    Notes = "Cardio HIIT đốt mỡ toàn thân và giãn cơ"
                },
                new Booking
                {
                    BookingId = 23,
                    MemberId = 21, // Bảo Ngọc
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 4, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:00 - 18:30",
                    Status = "Approved",
                    Notes = "Tập thân trên và chỉnh form Squat nâng cao"
                },
                new Booking
                {
                    BookingId = 24,
                    MemberId = 22, // Hoàng Long
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 24, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Completed",
                    Notes = "Tập ngực nâng cao - Pyramid Bench Press & Incline Dumbbell"
                },
                new Booking
                {
                    BookingId = 25,
                    MemberId = 22, // Hoàng Long
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 28, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Completed",
                    Notes = "Tập lưng xô - Barbell Row & Lat Pulldown form check"
                },
                new Booking
                {
                    BookingId = 26,
                    MemberId = 22, // Hoàng Long
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Approved",
                    Notes = "Tập chân mông - Barbell Squat 100kg & Romanian Deadlift"
                },
                new Booking
                {
                    BookingId = 27,
                    MemberId = 23, // Mai Linh
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 26, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:30 - 17:00",
                    Status = "Completed",
                    Notes = "Phục hồi tư thế cột sống và giải mỏi cổ vai gáy"
                },
                new Booking
                {
                    BookingId = 28,
                    MemberId = 23, // Mai Linh
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:30 - 17:00",
                    Status = "Completed",
                    Notes = "Pilates tăng độ dẻo dai cơ bụng và săn chắc eo"
                },
                new Booking
                {
                    BookingId = 29,
                    MemberId = 23, // Mai Linh
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:30 - 17:00",
                    Status = "Pending",
                    Notes = "Luyện tập thân dưới và thăng bằng cơ thể"
                },
                new Booking
                {
                    BookingId = 30,
                    MemberId = 24, // Hữu Đức
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 27, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "06:30 - 08:00",
                    Status = "Completed",
                    Notes = "Tập sức mạnh toàn thân (Full Body Functional Training)"
                },
                new Booking
                {
                    BookingId = 31,
                    MemberId = 24, // Hữu Đức
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 1, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "06:30 - 08:00",
                    Status = "Completed",
                    Notes = "Tập chuỗi bài Deadlift & Overhead Press"
                },
                new Booking
                {
                    BookingId = 32,
                    MemberId = 24, // Hữu Đức
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 6, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "06:30 - 08:00",
                    Status = "Approved",
                    Notes = "Kiểm tra thể lực định kỳ và tăng khối lượng tạ"
                },
                new Booking
                {
                    BookingId = 33,
                    MemberId = 2, // Thanh Hương
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 7, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:30 - 19:00",
                    Status = "Pending",
                    Notes = "Tập eo thon và cơ lưng dưới"
                },
                new Booking
                {
                    BookingId = 34,
                    MemberId = 4, // Phương Anh
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "14:00 - 15:30",
                    Status = "Approved",
                    Notes = "Tập tăng cơ đùi và điều chỉnh tư thế bước đi"
                }
            };
        }
    }
}
