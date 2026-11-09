using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ClassBookingSeeder
    {
        public static void SeedClassBookings(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassBooking>().HasData(GetData());
        }

        public static List<ClassBooking> GetData()
        {
            return new List<ClassBooking>
            {
                new ClassBooking
                {
                    ClassBookingId = 1,
                    ClassId = 1, // Hatha Yoga
                    MemberId = 2, // Đặng Thanh Hương
                    BookingDate = new DateTime(2026, 11, 2, 6, 0, 0),
                    Status = "Attended",
                    Notes = "Hội viên tham gia đầy đủ buổi tập Yoga sáng."
                },
                new ClassBooking
                {
                    ClassBookingId = 2,
                    ClassId = 1, // Hatha Yoga
                    MemberId = 4, // Vũ Phương Anh
                    BookingDate = new DateTime(2026, 11, 2, 6, 0, 0),
                    Status = "Attended",
                    Notes = "Mượn thảm tập cao su non của phòng."
                },
                new ClassBooking
                {
                    ClassBookingId = 3,
                    ClassId = 2, // Spinning RPM
                    MemberId = 1, // Nguyễn Quang Hải
                    BookingDate = new DateTime(2026, 11, 3, 17, 30, 0),
                    Status = "Attended",
                    Notes = "Đạt mục tiêu đốt 650 kcal."
                },
                new ClassBooking
                {
                    ClassBookingId = 4,
                    ClassId = 2, // Spinning RPM
                    MemberId = 3, // Phạm Đức Thắng
                    BookingDate = new DateTime(2026, 11, 3, 17, 30, 0),
                    Status = "Cancelled",
                    Notes = "Báo bận lịch công tác đột xuất lúc 15:00."
                },
                new ClassBooking
                {
                    ClassBookingId = 5,
                    ClassId = 3, // BodyPump
                    MemberId = 1, // Nguyễn Quang Hải
                    BookingDate = new DateTime(2026, 11, 4, 18, 0, 0),
                    Status = "Attended",
                    Notes = "Mức tạ gánh 25kg mỗi bên."
                },
                new ClassBooking
                {
                    ClassBookingId = 6,
                    ClassId = 3, // BodyPump
                    MemberId = 5, // Lê Hoàng Nam
                    BookingDate = new DateTime(2026, 11, 4, 18, 0, 0),
                    Status = "Attended",
                    Notes = "Tập rất sung sức."
                },
                new ClassBooking
                {
                    ClassBookingId = 7,
                    ClassId = 4, // Kickfit
                    MemberId = 3, // Phạm Đức Thắng
                    BookingDate = new DateTime(2026, 11, 5, 19, 0, 0),
                    Status = "Confirmed",
                    Notes = "Đã xác nhận giữ chỗ lớp đấm bốc."
                },
                new ClassBooking
                {
                    ClassBookingId = 8,
                    ClassId = 5, // Pilates
                    MemberId = 2, // Đặng Thanh Hương
                    BookingDate = new DateTime(2026, 11, 7, 8, 0, 0),
                    Status = "Confirmed",
                    Notes = "Học viên yêu cầu điều chỉnh tư thế thắt lưng."
                },
                new ClassBooking
                {
                    ClassBookingId = 9,
                    ClassId = 5, // Pilates
                    MemberId = 4, // Vũ Phương Anh
                    BookingDate = new DateTime(2026, 11, 7, 8, 0, 0),
                    Status = "Confirmed",
                    Notes = "Đã xếp máy Reformer số 3."
                }
            };
        }
    }
}
