using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class TrainerSeeder
    {
        public static void SeedTrainers(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>().HasData(GetData());
        }

        public static List<Trainer> GetData()
        {
            return new List<Trainer>
            {
                new Trainer
                {
                    TrainerId = 1,
                    UserId = 2,
                    Specialization = "Tăng cơ, Giảm mỡ nâng cao & Thể hình chuyên nghiệp (Bodybuilding)",
                    Bio = "HLV 6 năm kinh nghiệm, chứng chỉ quốc tế NASM, từng huấn luyện nhiều vận động viên thể hình.",
                    AvatarUrl = "~/images/coach1.jpg"
                },
                new Trainer
                {
                    TrainerId = 2,
                    UserId = 3,
                    Specialization = "Cardio cường độ cao (HIIT), Thể lực & Sức bền (Functional Training)",
                    Bio = "HLV 4 năm kinh nghiệm, chuyên về giảm cân cấp tốc, rèn luyện tim mạch và cải thiện chuyển hóa.",
                    AvatarUrl = "~/images/coach2.jpg"
                },
                new Trainer
                {
                    TrainerId = 3,
                    UserId = 4,
                    Specialization = "Pilates, Yoga & Điều chỉnh tư thế cột sống (Rehabilitation)",
                    Bio = "Chuyên gia phục hồi tư thế, tăng độ dẻo dai và giảm đau mỏi lưng cổ vai gáy cho dân văn phòng.",
                    AvatarUrl = "~/images/coach3.jpg"
                }
            };
        }
    }
}
