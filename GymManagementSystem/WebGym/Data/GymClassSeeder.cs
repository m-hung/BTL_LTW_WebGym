using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class GymClassSeeder
    {
        public static void SeedGymClasses(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymClass>().HasData(GetData());
        }

        public static List<GymClass> GetData()
        {
            return new List<GymClass>
            {
                new GymClass
                {
                    ClassId = 1,
                    ClassName = "Hatha Yoga & Thiền Định Phục Hồi Năng Lượng",
                    Description = "Lớp học giúp cân bằng thân - tâm - trí, kéo giãn cơ bắp, giải tỏa căng thẳng và cải thiện hệ hô hấp cho người bận rộn.",
                    TrainerId = 3, // HLV Nguyễn Thụy Tiên
                    RoomId = 3, // Phòng Yoga & Pilates Studio
                    Schedule = "Thứ 2 - 4 - 6 (06:30 - 07:45)",
                    MaxCapacity = 20,
                    CurrentCapacity = 15,
                    Status = "Open",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    ImageUrl = "~/images/classes/class-yoga.jpg"
                },
                new GymClass
                {
                    ClassId = 2,
                    ClassName = "Spinning RPM - Đua Xe Đạp Trong Nhà Đốt Mỡ Cấp Tốc",
                    Description = "Âm nhạc bùng nổ kết hợp các bài leo dốc, chạy nước rút cường độ cao giúp tiêu hao 600 - 800 kcal mỗi buổi tập.",
                    TrainerId = 2, // HLV Lê Hoàng Anh
                    RoomId = 2, // Phòng Cardio & Spinning
                    Schedule = "Thứ 3 - 5 - 7 (18:00 - 19:15)",
                    MaxCapacity = 25,
                    CurrentCapacity = 20,
                    Status = "Open",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    ImageUrl = "~/images/classes/class-spinning.jpg"
                },
                new GymClass
                {
                    ClassId = 3,
                    ClassName = "BodyPump - Rèn Luyện Sức Bền Cơ Bắp Toàn Thân",
                    Description = "Tập luyện với thanh đòn tạ theo giai điệu bài hát sôi động, tăng mật độ xương và săn chắc toàn bộ nhóm cơ.",
                    TrainerId = 1, // HLV Trần Minh Nhật
                    RoomId = 1, // Phòng Gym Trung Tâm
                    Schedule = "Thứ 2 - 4 - 6 (18:30 - 19:45)",
                    MaxCapacity = 25,
                    CurrentCapacity = 25,
                    Status = "Full",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    ImageUrl = "~/images/classes/class-bodypump.jpg"
                },
                new GymClass
                {
                    ClassId = 4,
                    ClassName = "Kickfit Tự Vệ & Cardio Boxing (Combat Zone)",
                    Description = "Sự kết hợp hoàn hảo giữa đấm bốc boxing, cùi chỏ muay thai và bài tập thể lực rèn luyện phản xạ nhanh nhẹn.",
                    TrainerId = 2, // HLV Lê Hoàng Anh
                    RoomId = 4, // Phòng Boxing & Kickfit
                    Schedule = "Thứ 3 - 5 - CN (19:30 - 20:45)",
                    MaxCapacity = 15,
                    CurrentCapacity = 10,
                    Status = "Open",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    ImageUrl = "~/images/classes/class-kickfit.jpg"
                },
                new GymClass
                {
                    ClassId = 5,
                    ClassName = "Pilates Trị Liệu Tư Thế & Giảm Đau Cột Sống",
                    Description = "Phương pháp căn chỉnh khung xương chậu, cải thiện gù lưng cổ rùa và làm săn chắc cơ bụng cốt lõi Core.",
                    TrainerId = 3, // HLV Nguyễn Thụy Tiên
                    RoomId = 3, // Phòng Yoga & Pilates Studio
                    Schedule = "Thứ 7 - Chủ Nhật (08:30 - 10:00)",
                    MaxCapacity = 12,
                    CurrentCapacity = 12,
                    Status = "Full",
                    StartDate = new DateTime(2026, 10, 1),
                    EndDate = new DateTime(2026, 12, 31),
                    ImageUrl = "~/images/classes/class-pilates.jpg"
                }
            };
        }
    }
}
