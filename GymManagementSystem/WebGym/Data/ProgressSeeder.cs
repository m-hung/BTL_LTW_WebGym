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
                },
                // Thêm dữ liệu về member tên Hải (MemberId 1)
                new Progress
                {
                    ProgressId = 21,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 3, 1, 8, 30, 0, DateTimeKind.Utc),
                    Weight = 83.0,
                    BodyFatPercentage = 28.0,
                    MuscleMass = 30.5,
                    Chest = 96.0,
                    Waist = 93.0,
                    Hips = 98.0,
                    Notes = "Đo InBody khi mới bắt đầu tập luyện. Thể trạng nhiều mỡ thừa vùng eo, thể lực yếu."
                },
                new Progress
                {
                    ProgressId = 22,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 5, 15, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 80.5,
                    BodyFatPercentage = 26.5,
                    MuscleMass = 31.2,
                    Chest = 97.0,
                    Waist = 90.0,
                    Hips = 97.0,
                    Notes = "Tập gói cơ bản 3 tháng, giảm được 2.5kg mỡ, bắt đầu hình thành thói quen vận động."
                },
                new Progress
                {
                    ProgressId = 23,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 11, 5, 9, 15, 0, DateTimeKind.Utc),
                    Weight = 69.5,
                    BodyFatPercentage = 17.8,
                    MuscleMass = 35.0,
                    Chest = 101.5,
                    Waist = 76.5,
                    Hips = 92.0,
                    Notes = "Vượt mục tiêu đề ra! Tỷ lệ mỡ giảm xuống 17.8%, cơ bắp sắc nét, cơ bụng lộ 6 múi."
                },
                // Thêm dữ liệu Progress cho TẤT CẢ các member khác
                // --- Member 2: Đặng Thanh Hương ---
                new Progress
                {
                    ProgressId = 24,
                    MemberId = 2,
                    RecordedDate = new DateTime(2026, 6, 15, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 56.0,
                    BodyFatPercentage = 24.0,
                    MuscleMass = 22.5,
                    Chest = 84.0,
                    Waist = 68.0,
                    Hips = 91.0,
                    Notes = "Đo chỉ số ban đầu. Mục tiêu giảm mỡ bụng dưới, săn chắc vòng eo và cải thiện số đo vòng 3."
                },
                new Progress
                {
                    ProgressId = 25,
                    MemberId = 2,
                    RecordedDate = new DateTime(2026, 8, 15, 9, 30, 0, DateTimeKind.Utc),
                    Weight = 54.5,
                    BodyFatPercentage = 22.2,
                    MuscleMass = 23.0,
                    Chest = 84.5,
                    Waist = 65.0,
                    Hips = 92.0,
                    Notes = "Vòng eo giảm 3cm, mông căng tròn hơn nhờ các bài Hip Thrust và Romanian Deadlift."
                },
                new Progress
                {
                    ProgressId = 26,
                    MemberId = 2,
                    RecordedDate = new DateTime(2026, 10, 20, 10, 0, 0, DateTimeKind.Utc),
                    Weight = 53.0,
                    BodyFatPercentage = 20.5,
                    MuscleMass = 23.8,
                    Chest = 85.0,
                    Waist = 63.0,
                    Hips = 92.5,
                    Notes = "Form dáng đồng hồ cát rất đẹp, cơ bụng số 11 lộ rõ, sức bền tim mạch tăng rõ rệt."
                },
                // --- Member 3: Vũ Đức Thắng ---
                new Progress
                {
                    ProgressId = 27,
                    MemberId = 3,
                    RecordedDate = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc),
                    Weight = 84.0,
                    BodyFatPercentage = 26.0,
                    MuscleMass = 34.5,
                    Chest = 102.0,
                    Waist = 92.0,
                    Hips = 100.0,
                    Notes = "Thể trạng thừa cân, mỡ nội tạng cao, thường xuyên mệt mỏi khi vận động mạnh."
                },
                new Progress
                {
                    ProgressId = 28,
                    MemberId = 3,
                    RecordedDate = new DateTime(2026, 7, 20, 14, 30, 0, DateTimeKind.Utc),
                    Weight = 80.2,
                    BodyFatPercentage = 23.0,
                    MuscleMass = 35.5,
                    Chest = 103.0,
                    Waist = 87.0,
                    Hips = 99.0,
                    Notes = "Giảm 3.8kg mỡ, tăng 1kg cơ nạc. Thể lực cải thiện, hoàn thành tốt các bài tập tạ nặng."
                },
                new Progress
                {
                    ProgressId = 29,
                    MemberId = 3,
                    RecordedDate = new DateTime(2026, 10, 15, 15, 0, 0, DateTimeKind.Utc),
                    Weight = 76.8,
                    BodyFatPercentage = 19.8,
                    MuscleMass = 36.4,
                    Chest = 105.0,
                    Waist = 82.5,
                    Hips = 98.0,
                    Notes = "Eo thon gọn rõ nét, cơ ngực và xô lưng phát triển ấn tượng, mức tạ Bench Press đạt 85kg."
                },
                // --- Member 4: Bùi Phương Anh ---
                new Progress
                {
                    ProgressId = 30,
                    MemberId = 4,
                    RecordedDate = new DateTime(2026, 9, 5, 10, 0, 0, DateTimeKind.Utc),
                    Weight = 49.0,
                    BodyFatPercentage = 21.5,
                    MuscleMass = 20.0,
                    Chest = 82.0,
                    Waist = 64.0,
                    Hips = 88.0,
                    Notes = "Thể trạng gầy, cơ bắp yếu, dễ mệt mỏi. Mục tiêu tăng cơ nạc và tăng cân lành mạnh."
                },
                new Progress
                {
                    ProgressId = 31,
                    MemberId = 4,
                    RecordedDate = new DateTime(2026, 10, 10, 10, 30, 0, DateTimeKind.Utc),
                    Weight = 50.3,
                    BodyFatPercentage = 20.5,
                    MuscleMass = 21.3,
                    Chest = 83.0,
                    Waist = 63.5,
                    Hips = 89.5,
                    Notes = "Tăng 1.3kg chủ yếu là cơ nạc, cơ thể có sức sống hơn, thể lực cải thiện rõ rệt."
                },
                new Progress
                {
                    ProgressId = 32,
                    MemberId = 4,
                    RecordedDate = new DateTime(2026, 11, 2, 11, 0, 0, DateTimeKind.Utc),
                    Weight = 51.2,
                    BodyFatPercentage = 20.0,
                    MuscleMass = 22.1,
                    Chest = 83.5,
                    Waist = 63.0,
                    Hips = 90.5,
                    Notes = "Vóc dáng cân đối, săn chắc, vòng 3 nở nang tự nhiên, hoàn thành giáo án xuất sắc."
                },
                // --- Member 5: Phạm Hoàng Nam ---
                new Progress
                {
                    ProgressId = 33,
                    MemberId = 5,
                    RecordedDate = new DateTime(2026, 7, 20, 16, 30, 0, DateTimeKind.Utc),
                    Weight = 73.0,
                    BodyFatPercentage = 19.0,
                    MuscleMass = 33.0,
                    Chest = 96.0,
                    Waist = 80.0,
                    Hips = 94.0,
                    Notes = "Mục tiêu duy trì thể lực, rèn luyện sức bền và tăng độ nét cơ bắp toàn thân."
                },
                new Progress
                {
                    ProgressId = 34,
                    MemberId = 5,
                    RecordedDate = new DateTime(2026, 9, 15, 17, 0, 0, DateTimeKind.Utc),
                    Weight = 72.4,
                    BodyFatPercentage = 17.2,
                    MuscleMass = 34.2,
                    Chest = 98.0,
                    Waist = 78.0,
                    Hips = 94.0,
                    Notes = "Cơ vai và bắp tay cắt nét rõ, sức bền chạy bộ 5km đạt thời gian 24 phút."
                },
                new Progress
                {
                    ProgressId = 35,
                    MemberId = 5,
                    RecordedDate = new DateTime(2026, 10, 28, 17, 30, 0, DateTimeKind.Utc),
                    Weight = 71.8,
                    BodyFatPercentage = 16.0,
                    MuscleMass = 35.0,
                    Chest = 99.5,
                    Waist = 76.5,
                    Hips = 93.5,
                    Notes = "Chỉ số cơ thể chuẩn vận động viên thể hình, múi bụng săn chắc, sức đẩy cực khỏe."
                },
                // --- Member 21: Trần Bảo Ngọc ---
                new Progress
                {
                    ProgressId = 36,
                    MemberId = 21,
                    RecordedDate = new DateTime(2026, 2, 15, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 54.0,
                    BodyFatPercentage = 23.5,
                    MuscleMass = 21.8,
                    Chest = 85.0,
                    Waist = 65.0,
                    Hips = 90.0,
                    Notes = "Buổi kiểm tra thể lực đầu vào. Cơ địa linh hoạt nhưng cơ lưng và cơ mông còn yếu."
                },
                new Progress
                {
                    ProgressId = 37,
                    MemberId = 21,
                    RecordedDate = new DateTime(2026, 5, 20, 9, 30, 0, DateTimeKind.Utc),
                    Weight = 52.8,
                    BodyFatPercentage = 21.2,
                    MuscleMass = 22.7,
                    Chest = 85.5,
                    Waist = 62.5,
                    Hips = 91.0,
                    Notes = "Vòng eo giảm 2.5cm, mông đẩy cao săn chắc, thích nghi rất tốt với các bài tập kháng lực."
                },
                new Progress
                {
                    ProgressId = 38,
                    MemberId = 21,
                    RecordedDate = new DateTime(2026, 8, 30, 10, 0, 0, DateTimeKind.Utc),
                    Weight = 52.0,
                    BodyFatPercentage = 19.8,
                    MuscleMass = 23.3,
                    Chest = 86.0,
                    Waist = 61.0,
                    Hips = 92.0,
                    Notes = "Hình thể chuẩn người mẫu fitness, hoàn thành xuất sắc giáo án tập luyện kèm PT Nhật."
                },
                // --- Member 22: Lê Hoàng Long ---
                new Progress
                {
                    ProgressId = 39,
                    MemberId = 22,
                    RecordedDate = new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc),
                    Weight = 82.0,
                    BodyFatPercentage = 24.0,
                    MuscleMass = 35.0,
                    Chest = 101.0,
                    Waist = 86.0,
                    Hips = 98.0,
                    Notes = "Mục tiêu siết mỡ bụng và phát triển cơ ngực, vai, lưng xô chữ V."
                },
                new Progress
                {
                    ProgressId = 40,
                    MemberId = 22,
                    RecordedDate = new DateTime(2026, 6, 15, 10, 30, 0, DateTimeKind.Utc),
                    Weight = 78.8,
                    BodyFatPercentage = 20.8,
                    MuscleMass = 36.3,
                    Chest = 103.5,
                    Waist = 82.0,
                    Hips = 97.0,
                    Notes = "Giảm hơn 3kg mỡ, cơ ngực nở rộng, hoàn thành bài Deadlift 120kg đúng kỹ thuật."
                },
                new Progress
                {
                    ProgressId = 41,
                    MemberId = 22,
                    RecordedDate = new DateTime(2026, 10, 5, 11, 0, 0, DateTimeKind.Utc),
                    Weight = 76.2,
                    BodyFatPercentage = 17.5,
                    MuscleMass = 37.6,
                    Chest = 105.5,
                    Waist = 78.5,
                    Hips = 96.5,
                    Notes = "Lưng chữ V cực kỳ rõ nét, bắp tay cuồn cuộn, nâng cấp thành công lên gói Kim Cương."
                },
                // --- Member 23: Đỗ Mai Linh ---
                new Progress
                {
                    ProgressId = 42,
                    MemberId = 23,
                    RecordedDate = new DateTime(2026, 4, 5, 14, 0, 0, DateTimeKind.Utc),
                    Weight = 52.5,
                    BodyFatPercentage = 22.0,
                    MuscleMass = 20.5,
                    Chest = 83.0,
                    Waist = 63.0,
                    Hips = 89.0,
                    Notes = "Dân văn phòng ngồi nhiều, cơ cổ vai gáy căng cứng, bụng dưới tích mỡ nhẹ."
                },
                new Progress
                {
                    ProgressId = 43,
                    MemberId = 23,
                    RecordedDate = new DateTime(2026, 7, 10, 14, 30, 0, DateTimeKind.Utc),
                    Weight = 51.4,
                    BodyFatPercentage = 20.3,
                    MuscleMass = 21.4,
                    Chest = 83.5,
                    Waist = 61.2,
                    Hips = 89.5,
                    Notes = "Hết hẳn triệu chứng đau mỏi vai gáy, tư thế đi đứng thẳng thớm và tự tin hơn."
                },
                new Progress
                {
                    ProgressId = 44,
                    MemberId = 23,
                    RecordedDate = new DateTime(2026, 10, 25, 15, 0, 0, DateTimeKind.Utc),
                    Weight = 50.6,
                    BodyFatPercentage = 19.2,
                    MuscleMass = 22.2,
                    Chest = 84.0,
                    Waist = 60.0,
                    Hips = 90.0,
                    Notes = "Vòng eo 60cm săn chắc thon gọn, sức khỏe dẻo dai, rất yêu thích tập luyện tại phòng gym."
                },
                // --- Member 24: Nguyễn Hữu Đức ---
                new Progress
                {
                    ProgressId = 45,
                    MemberId = 24,
                    RecordedDate = new DateTime(2026, 1, 10, 10, 0, 0, DateTimeKind.Utc),
                    Weight = 75.0,
                    BodyFatPercentage = 21.0,
                    MuscleMass = 33.0,
                    Chest = 97.0,
                    Waist = 82.0,
                    Hips = 95.0,
                    Notes = "Đăng ký gói VVIP Kim Cương 1 năm. Khởi đầu với quyết tâm rèn luyện sức khỏe bền vững."
                },
                new Progress
                {
                    ProgressId = 46,
                    MemberId = 24,
                    RecordedDate = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Utc),
                    Weight = 73.2,
                    BodyFatPercentage = 18.2,
                    MuscleMass = 34.3,
                    Chest = 99.2,
                    Waist = 78.5,
                    Hips = 94.5,
                    Notes = "Giảm 3.5cm vòng eo, cơ ngực săn chắc, sức bền bứt phá trong các buổi tập functional."
                },
                new Progress
                {
                    ProgressId = 47,
                    MemberId = 24,
                    RecordedDate = new DateTime(2026, 9, 22, 11, 0, 0, DateTimeKind.Utc),
                    Weight = 72.0,
                    BodyFatPercentage = 16.2,
                    MuscleMass = 35.2,
                    Chest = 101.0,
                    Waist = 76.5,
                    Hips = 94.0,
                    Notes = "Tập luyện đều đặn suốt 9 tháng, chỉ số InBody tiệm cận chuẩn vận động viên."
                }
            };
        }
    }
}
