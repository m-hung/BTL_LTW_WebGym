using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class GymRoomSeeder
    {
        public static void SeedGymRooms(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymRoom>().HasData(GetData());
        }

        public static List<GymRoom> GetData()
        {
            return new List<GymRoom>
            {
                new GymRoom
                {
                    RoomId = 1,
                    RoomName = "Phòng Gym Trung Tâm (Khu Tạ & Kháng Lực)",
                    Location = "Tầng 1 - Khu A",
                    Capacity = 50,
                    Status = "Hoạt động",
                    Description = "Khu vực tập luyện chính gồm giàn tạ rời (Free Weights), rack gánh tạ và các máy tập khối chuyên dụng.",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new GymRoom
                {
                    RoomId = 2,
                    RoomName = "Phòng Cardio & Đạp Xe Trong Nhà (Spinning Studio)",
                    Location = "Tầng 2 - Khu B",
                    Capacity = 35,
                    Status = "Hoạt động",
                    Description = "Trang bị dàn máy chạy bộ thông minh, xe đạp địa hình spinning và máy trượt tuyết elliptical.",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new GymRoom
                {
                    RoomId = 3,
                    RoomName = "Phòng Tập Nhóm Yoga & Pilates Studio",
                    Location = "Tầng 3 - Studio 1",
                    Capacity = 25,
                    Status = "Hoạt động",
                    Description = "Không gian yên tĩnh, sàn gỗ thông tự nhiên, gương ốp tường toàn cảnh và hệ thống thảm tập cao cấp.",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new GymRoom
                {
                    RoomId = 4,
                    RoomName = "Phòng Huấn Luyện Boxing & Kickfit (Combat Zone)",
                    Location = "Tầng 3 - Studio 2",
                    Capacity = 20,
                    Status = "Hoạt động",
                    Description = "Khu vực chuyên sâu boxing, kickfit với bao cát treo, thảm giảm chấn và găng đấm tiêu chuẩn thi đấu.",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new GymRoom
                {
                    RoomId = 5,
                    RoomName = "Phòng Huấn Luyện Cá Nhân VIP (PT Private Studio)",
                    Location = "Tầng 2 - Phòng VIP",
                    Capacity = 10,
                    Status = "Hoạt động",
                    Description = "Không gian riêng tư cao cấp dành riêng cho học viên tập 1-1 cùng Huấn luyện viên thể hình chuyên nghiệp.",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            };
        }
    }
}
