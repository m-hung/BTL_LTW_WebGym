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
                },
                // 5 PT phương Tây chuyên phụ trách 5 lớp học nhóm
                new Trainer
                {
                    TrainerId = 4,
                    UserId = 10,
                    Specialization = "Hatha Yoga & Thiền Định Phục Hồi (Yoga Alliance RYT-500)",
                    Bio = "HLV 7 năm kinh nghiệm giảng dạy Hatha Yoga, thở chánh niệm và phục hồi cân bằng thân - tâm - trí.",
                    AvatarUrl = "~/images/coach4.jpg"
                },
                new Trainer
                {
                    TrainerId = 5,
                    UserId = 11,
                    Specialization = "Spinning RPM & Sức Bền Tim Mạch (Indoor Cycling Coach)",
                    Bio = "HLV 6 năm kinh nghiệm dẫn dắt các lớp đạp xe trong nhà cường độ cao và HIIT đốt mỡ.",
                    AvatarUrl = "~/images/coach5.jpg"
                },
                new Trainer
                {
                    TrainerId = 6,
                    UserId = 12,
                    Specialization = "BodyPump & Sức Mạnh Kháng Lực (Les Mills Certified)",
                    Bio = "HLV 8 năm kinh nghiệm huấn luyện BodyPump với thanh đòn tạ và tăng sức bền cơ bắp toàn thân.",
                    AvatarUrl = "~/images/coach6.jpg"
                },
                new Trainer
                {
                    TrainerId = 7,
                    UserId = 13,
                    Specialization = "Kickfit, Muay Thai & Cardio Boxing (Combat Conditioning)",
                    Bio = "HLV 5 năm kinh nghiệm thi đấu và giảng dạy Boxing, Kickfit tự vệ rèn phản xạ nhanh nhẹn.",
                    AvatarUrl = "~/images/coach7.jpg"
                },
                new Trainer
                {
                    TrainerId = 8,
                    UserId = 14,
                    Specialization = "Pilates Reformer & Điều Chỉnh Tư Thế (Core & Mobility)",
                    Bio = "HLV 6 năm kinh nghiệm trị liệu cột sống, cải thiện tư thế và làm săn chắc cơ lõi Core với Pilates.",
                    AvatarUrl = "~/images/coach8.jpg"
                }
            };
        }
    }
}
