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
                    BookingDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:00 - 16:30",
                    Status = "Pending",
                    Notes = "Hội viên mới đăng ký buổi đầu thẩm định thể lực"
                },
                new Booking
                {
                    BookingId = 9,
                    MemberId = 3,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 17, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "10:00 - 11:30",
                    Status = "Pending",
                    Notes = "Tập vai & cơ bụng core strength"
                },
                new Booking
                {
                    BookingId = 10,
                    MemberId = 5,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 18, 0, 0, 0, DateTimeKind.Utc),
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
                    BookingDate = new DateTime(2026, 11, 19, 0, 0, 0, DateTimeKind.Utc),
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
                    BookingDate = new DateTime(2026, 11, 20, 0, 0, 0, DateTimeKind.Utc),
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
                },

                // === TUẦN HIỆN TẠI (09/11 - 15/11/2026) - Lịch dạy PT Nhật ===
                new Booking { BookingId = 35, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 9, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Completed", Notes = "Tập ngực trên & vai trước (Incline Press)" },
                new Booking { BookingId = 36, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 9, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:30 - 11:00", Status = "Completed", Notes = "Mông đùi & ổn định khớp gối" },
                new Booking { BookingId = 37, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 9, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:00 - 18:30", Status = "Completed", Notes = "Tập kéo lưng xô & tay trước (Pull Day)" },
                new Booking { BookingId = 38, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 9, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "19:00 - 20:30", Status = "Completed", Notes = "Heavy Squat 5x5 tăng sức mạnh thân dưới" },

                new Booking { BookingId = 39, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 10, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Completed", Notes = "Deadlift kỹ thuật & sức mạnh cơ dựng lưng" },
                new Booking { BookingId = 40, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 10, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:00 - 11:30", Status = "Completed", Notes = "Siết eo & Cardio HIIT cuối buổi" },
                new Booking { BookingId = 41, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 10, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:30 - 17:00", Status = "Completed", Notes = "Giãn cơ cổ vai gáy & Core Balance" },
                new Booking { BookingId = 42, MemberId = 5, TrainerId = 1, BookingDate = new DateTime(2026, 11, 10, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Completed", Notes = "Tập thân trên toàn diện & đốt mỡ" },

                new Booking { BookingId = 43, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Completed", Notes = "Chân & Bắp chân (Squat + Leg Press)" },
                new Booking { BookingId = 44, MemberId = 4, TrainerId = 1, BookingDate = new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "14:00 - 15:30", Status = "Completed", Notes = "Cải thiện sức bền thân dưới" },
                new Booking { BookingId = 45, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:30 - 19:00", Status = "Approved", Notes = "Tập lưng trên & tay sau săn chắc" },
                new Booking { BookingId = 46, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 11, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "19:30 - 21:00", Status = "Approved", Notes = "Vai rộng & cơ cầu vai (Overhead Press)" },

                new Booking { BookingId = 47, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Bench Press nặng & phụ trợ tay sau" },
                new Booking { BookingId = 48, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:30 - 11:00", Status = "Approved", Notes = "Lưng xô & Kéo xà có tải trọng" },
                new Booking { BookingId = 49, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:30 - 17:00", Status = "Approved", Notes = "Glute Bridge & Romanian Deadlift nhẹ" },
                new Booking { BookingId = 50, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Approved", Notes = "Vai, bụng & 20 phút chạy dốc đốt mỡ" },

                new Booking { BookingId = 51, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 13, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Hip Thrust & Lunges đốt mỡ đùi" },
                new Booking { BookingId = 52, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 13, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:00 - 10:30", Status = "Approved", Notes = "Full Body Conditioning & Core" },
                new Booking { BookingId = 53, MemberId = 5, TrainerId = 1, BookingDate = new DateTime(2026, 11, 13, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "16:00 - 17:30", Status = "Approved", Notes = "Ngực, vai & Cardio đốt năng lượng" },
                new Booking { BookingId = 54, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 13, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:30 - 20:00", Status = "Pending", Notes = "Xin tập bổ sung kỹ thuật Squat sâu" },

                new Booking { BookingId = 55, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 14, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Approved", Notes = "Tổng kết InBody giữa chu kỳ 12 tuần" },
                new Booking { BookingId = 56, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 14, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:00 - 11:30", Status = "Approved", Notes = "Functional Circuit cuối tuần" },
                new Booking { BookingId = 57, MemberId = 4, TrainerId = 1, BookingDate = new DateTime(2026, 11, 14, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "14:30 - 16:00", Status = "Pending", Notes = "Đặt lịch tập thân trên & tư thế vai" },
                new Booking { BookingId = 58, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 14, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:00 - 18:30", Status = "Approved", Notes = "Phụ trợ tay trước sau & Core" },

                new Booking { BookingId = 59, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 15, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:30 - 10:00", Status = "Approved", Notes = "Phục hồi cột sống & giãn cơ sâu" },
                new Booking { BookingId = 60, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 15, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:30 - 12:00", Status = "Approved", Notes = "Cardio Zone 2 & siết cơ bụng" },
                new Booking { BookingId = 61, MemberId = 25, TrainerId = 1, BookingDate = new DateTime(2026, 11, 15, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "16:30 - 18:00", Status = "Pending", Notes = "Hội viên mới đặt buổi đánh giá thể lực" },

                // === TUẦN TỚI 1 (16/11 - 22/11/2026) ===
                new Booking { BookingId = 62, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Ngực & Tay sau tuần mới (Progressive Overload)" },
                new Booking { BookingId = 63, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:30 - 11:00", Status = "Approved", Notes = "Chân mông tăng mức tạ Hip Thrust" },
                new Booking { BookingId = 64, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 16, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Approved", Notes = "Push Day tăng khối lượng tạ" },

                new Booking { BookingId = 65, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 17, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Deadlift & Lưng giữa" },
                new Booking { BookingId = 66, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 17, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:30 - 17:00", Status = "Approved", Notes = "Cải thiện độ linh hoạt khớp hông" },
                new Booking { BookingId = 67, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 17, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:30 - 20:00", Status = "Approved", Notes = "Thân trên & HIIT đốt mỡ" },

                new Booking { BookingId = 68, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 18, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Approved", Notes = "Chân & Mông cường độ vừa" },
                new Booking { BookingId = 69, MemberId = 4, TrainerId = 1, BookingDate = new DateTime(2026, 11, 18, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:30 - 12:00", Status = "Approved", Notes = "Chỉnh form Lunge & Squat" },
                new Booking { BookingId = 70, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 18, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:00 - 16:30", Status = "Approved", Notes = "Lưng xô & tư thế vai" },

                new Booking { BookingId = 71, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 19, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Squat & Leg Press" },
                new Booking { BookingId = 72, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 19, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:30 - 11:00", Status = "Approved", Notes = "Bench Press nặng 5x5" },
                new Booking { BookingId = 73, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 19, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Approved", Notes = "Pull Day & Tay trước" },

                new Booking { BookingId = 74, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 20, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Vai & Core chống xoay" },
                new Booking { BookingId = 75, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 20, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:00 - 11:30", Status = "Approved", Notes = "Giảm mỡ toàn thân & Cardio" },
                new Booking { BookingId = 76, MemberId = 26, TrainerId = 1, BookingDate = new DateTime(2026, 11, 20, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "14:30 - 16:00", Status = "Pending", Notes = "Yêu cầu tư vấn lộ trình giảm mỡ sau sinh" },

                new Booking { BookingId = 77, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 21, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Approved", Notes = "Upper Power & Core" },
                new Booking { BookingId = 78, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 21, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:30 - 12:00", Status = "Approved", Notes = "Deadlift 5x3 mục tiêu PR mới" },
                new Booking { BookingId = 79, MemberId = 5, TrainerId = 1, BookingDate = new DateTime(2026, 11, 21, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "16:00 - 17:30", Status = "Approved", Notes = "HIIT Sprint & Core" },

                new Booking { BookingId = 80, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 22, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:30 - 10:00", Status = "Approved", Notes = "Core Balance & Mobility" },
                new Booking { BookingId = 81, MemberId = 4, TrainerId = 1, BookingDate = new DateTime(2026, 11, 22, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:00 - 16:30", Status = "Approved", Notes = "Mông đùi săn chắc" },
                new Booking { BookingId = 82, MemberId = 25, TrainerId = 1, BookingDate = new DateTime(2026, 11, 22, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:30 - 19:00", Status = "Pending", Notes = "Đặt lịch tập thân trên buổi 2" },

                // === TUẦN TỚI 2 (23/11 - 29/11/2026) ===
                new Booking { BookingId = 83, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 23, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Ngực & Tay sau cường độ cao" },
                new Booking { BookingId = 84, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 23, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:00 - 18:30", Status = "Approved", Notes = "Glutes Hypertrophy" },
                new Booking { BookingId = 85, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 23, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "19:00 - 20:30", Status = "Approved", Notes = "Squat & Calf Raise" },

                new Booking { BookingId = 86, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 24, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Full Body Functional" },
                new Booking { BookingId = 87, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 24, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:00 - 11:30", Status = "Approved", Notes = "Đo InBody & chỉnh bài tập eo" },
                new Booking { BookingId = 88, MemberId = 5, TrainerId = 1, BookingDate = new DateTime(2026, 11, 24, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Approved", Notes = "Lưng xô & Cardio" },

                new Booking { BookingId = 89, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 25, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Approved", Notes = "Deadlift & Lưng dày" },
                new Booking { BookingId = 90, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 25, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:30 - 17:00", Status = "Approved", Notes = "Tập Core & phục hồi vai" },
                new Booking { BookingId = 91, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 25, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:30 - 20:00", Status = "Approved", Notes = "Squat kỹ thuật & bắp chân" },

                new Booking { BookingId = 92, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 26, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "07:00 - 08:30", Status = "Approved", Notes = "Overhead Press & Pull-up" },
                new Booking { BookingId = 93, MemberId = 4, TrainerId = 1, BookingDate = new DateTime(2026, 11, 26, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "14:00 - 15:30", Status = "Approved", Notes = "Đốt mỡ thân dưới" },
                new Booking { BookingId = 94, MemberId = 21, TrainerId = 1, BookingDate = new DateTime(2026, 11, 26, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:30 - 19:00", Status = "Approved", Notes = "Thân trên & siết bụng" },

                new Booking { BookingId = 95, MemberId = 1, TrainerId = 1, BookingDate = new DateTime(2026, 11, 27, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:00 - 09:30", Status = "Approved", Notes = "Đo lại InBody cuối tháng 11" },
                new Booking { BookingId = 96, MemberId = 2, TrainerId = 1, BookingDate = new DateTime(2026, 11, 27, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "10:30 - 12:00", Status = "Approved", Notes = "Tổng kết 12 tuần & giãn cơ" },
                new Booking { BookingId = 97, MemberId = 22, TrainerId = 1, BookingDate = new DateTime(2026, 11, 27, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "18:00 - 19:30", Status = "Approved", Notes = "Ngực vai tay toàn diện" },

                new Booking { BookingId = 98, MemberId = 3, TrainerId = 1, BookingDate = new DateTime(2026, 11, 28, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "08:30 - 10:00", Status = "Approved", Notes = "Kiểm tra sức mạnh Big 3 cuối tháng" },
                new Booking { BookingId = 99, MemberId = 23, TrainerId = 1, BookingDate = new DateTime(2026, 11, 28, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "15:00 - 16:30", Status = "Approved", Notes = "Pilates kết hợp tạ nhẹ" },
                new Booking { BookingId = 100, MemberId = 27, TrainerId = 1, BookingDate = new DateTime(2026, 11, 28, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "17:00 - 18:30", Status = "Pending", Notes = "Đặt lịch PT 1-1 mục tiêu tăng 4kg cơ" },

                new Booking { BookingId = 101, MemberId = 24, TrainerId = 1, BookingDate = new DateTime(2026, 11, 29, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "09:00 - 10:30", Status = "Approved", Notes = "Điều hòa tim mạch & giãn cơ" },
                new Booking { BookingId = 102, MemberId = 5, TrainerId = 1, BookingDate = new DateTime(2026, 11, 29, 0, 0, 0, DateTimeKind.Utc), TimeSlot = "16:00 - 17:30", Status = "Approved", Notes = "Cardio kết hợp Core cuối tuần" }
            };
        }
    }
}
