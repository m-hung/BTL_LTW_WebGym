using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ProgressSeeder
    {
        public static void SeedProgress(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Progress>().HasData(GetData());
        }

        public static List<Progress> GetData()
        {
            return new List<Progress>
            {
                new Progress
                {
                    ProgressId = 1,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 8, 1, 8, 30, 0, DateTimeKind.Utc),
                    Weight = 78.5,
                    BodyFatPercentage = 25.5,
                    MuscleMass = 32.0,
                    Chest = 98.0,
                    Waist = 88.0,
                    Hips = 96.0,
                    Notes = "Đo InBody ngày đầu nhập môn. Thể trạng nhiều mỡ nội tạng, cơ địa dễ tích mỡ bụng."
                },
                new Progress
                {
                    ProgressId = 2,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 8, 20, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 76.8,
                    BodyFatPercentage = 24.2,
                    MuscleMass = 32.4,
                    Chest = 98.5,
                    Waist = 85.5,
                    Hips = 95.0,
                    Notes = "Thích nghi tốt với bài tập kháng lực và cardio HIIT. Bắt đầu quen với thực đơn thâm hụt calo."
                },
                new Progress
                {
                    ProgressId = 3,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 9, 10, 8, 45, 0, DateTimeKind.Utc),
                    Weight = 74.9,
                    BodyFatPercentage = 22.8,
                    MuscleMass = 32.9,
                    Chest = 99.0,
                    Waist = 83.0,
                    Hips = 94.0,
                    Notes = "Vòng eo giảm rõ rệt 5cm, cơ ngực săn chắc, sức bền tim mạch tăng đáng kể."
                },
                new Progress
                {
                    ProgressId = 4,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 9, 30, 9, 15, 0, DateTimeKind.Utc),
                    Weight = 73.2,
                    BodyFatPercentage = 21.2,
                    MuscleMass = 33.5,
                    Chest = 99.5,
                    Waist = 81.0,
                    Hips = 93.5,
                    Notes = "Tăng cơ nạc và siết mỡ liên tục. Mức tạ Squat và Bench Press tăng thêm 15kg."
                },
                new Progress
                {
                    ProgressId = 5,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 10, 18, 8, 30, 0, DateTimeKind.Utc),
                    Weight = 71.6,
                    BodyFatPercentage = 19.8,
                    MuscleMass = 34.0,
                    Chest = 100.0,
                    Waist = 79.0,
                    Hips = 93.0,
                    Notes = "Tỷ lệ mỡ giảm dưới mốc 20%, cơ bụng lộ 4 múi rõ nét, phong độ tập luyện rất cao."
                },
                new Progress
                {
                    ProgressId = 6,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 11, 1, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 70.2,
                    BodyFatPercentage = 18.5,
                    MuscleMass = 34.6,
                    Chest = 101.0,
                    Waist = 77.5,
                    Hips = 92.5,
                    Notes = "Cột mốc 3 tháng: Giảm 8.3kg mỡ, tăng 2.6kg cơ, vòng eo giảm 10.5cm. Chỉ số xuất sắc!"
                }
            };
        }
    }
}
