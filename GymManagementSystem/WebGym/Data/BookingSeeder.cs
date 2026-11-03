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
                }
            };
        }
    }
}
