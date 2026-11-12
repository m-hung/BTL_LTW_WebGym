using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class WorkoutPlanSeeder
    {
        public static void SeedWorkoutPlans(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutPlan>().HasData(GetData());
        }

        public static List<WorkoutPlan> GetData()
        {
            return new List<WorkoutPlan>
            {
                new WorkoutPlan
                {
                    Id = 1,
                    Title = "Kế Hoạch Tăng Cơ Giảm Mỡ Cấp Tốc 12 Tuần (Hypertrophy & Shredding)",
                    MemberId = 1, // Nguyễn Quang Hải
                    TrainerId = 1, // HLV Trần Minh Nhật
                    Goal = "Giảm từ 78.5kg xuống 70kg, giảm tỷ lệ mỡ cơ thể về dưới 18%, tăng khối lượng cơ bắp thân trên.",
                    StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                    RoutineDetails = @"- Thứ 2 (Ngực & Tay sau): Bench Press 4x8, Incline Dumbbell Press 4x10, Cable Fly 3x12, Tricep Pushdown 4x12.
- Thứ 3 (Lưng xô & Tay trước): Deadlift 4x6, Lat Pulldown 4x10, Barbell Row 4x8, Bicep Curls 3x12.
- Thứ 4: Nghỉ ngơi tích cực hoặc Cardio nhẹ 30 phút đi bộ dốc.
- Thứ 5 (Chân & Bắp chân): Barbell Squat 4x8, Leg Press 4x12, Leg Curl 3x15, Calf Raise 4x20.
- Thứ 6 (Vai & Bụng): Overhead Press 4x8, Lateral Raise 4x15, Face Pull 3x15, Hanging Leg Raise 4x15.
- Thứ 7: HIIT Cardio chạy bộ máy Technogym 25 phút.
- Chủ nhật: Nghỉ phục hồi toàn diện.".Replace("\r\n", "\n"),
                    NutritionAdvice = @"- Calo mục tiêu: 2.100 kcal/ngày (Deficit 300 kcal).
- Đạm (Protein): 160g - 170g/ngày (~2.2g/kg thể trọng), ưu tiên ức gà, cá ngừ, trứng, Whey Isolate.
- Tinh bột (Carb): 200g/ngày (khoai lang, yến mạch, cơm gạo lứt).
- Chất béo tốt: 50g/ngày (dầu ô liu, quả bơ, hạt hạnh nhân).
- Uống đủ 3 - 3.5 lít nước mỗi ngày, hạn chế tối đa bia rượu và đồ ngọt đóng chai.".Replace("\r\n", "\n"),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new WorkoutPlan
                {
                    Id = 2,
                    Title = "Giáo Án Siết Eo Săn Chắc Vóc Dáng & Yoga Nữ (Tone & Flexibility)",
                    MemberId = 2, // Đặng Thanh Hương
                    TrainerId = 3, // HLV Nguyễn Thụy Tiên
                    Goal = "Giảm mỡ vòng eo từ 68cm xuống 62cm, cải thiện đường cong hông đùi và độ dẻo dai cơ thể.",
                    StartDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                    RoutineDetails = @"- Thứ 2: Tập mông đùi Glute Focus: Hip Thrust 4x12, Romanian Deadlift 4x10, Bulgarian Split Squat 3x12 mỗi bên.
- Thứ 3: Lớp Hatha Yoga phục hồi & giãn cơ chuyên sâu 75 phút.
- Thứ 4: Thân trên nữ & Lưng xô: Lat Pulldown 3x12, Dumbbell Shoulder Press 3x12, Plank 4x45s.
- Thứ 5: Nghỉ ngơi hoặc đạp xe Spinning kháng lực vừa phải 40 phút.
- Thứ 6: Pilates thảm Matwork & Core Cốt Lõi: Động tác The Hundred, Criss Cross, Teaser.
- Thứ 7 & CN: Đi dạo ngoài trời, thiền định.".Replace("\r\n", "\n"),
                    NutritionAdvice = @"- Calo mục tiêu: 1.450 kcal/ngày.
- Ưu tiên rau xanh nhiều chất xơ, cá hồi, tôm, sữa chua Hy Lạp và sinh tố hoa quả mọng (berries).
- Hạn chế gia vị nêm mặn nhằm tránh tích nước dưới da vùng bụng.
- Uống trà hoa cúc hoặc trà xanh ấm buổi tối.".Replace("\r\n", "\n"),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new WorkoutPlan
                {
                    Id = 3,
                    Title = "Chương Trình Sức Mạnh Nền Tảng Powerlifting & Khối Lượng Cơ Lớn",
                    MemberId = 3, // Phạm Đức Thắng
                    TrainerId = 1, // HLV Trần Minh Nhật
                    Goal = "Tăng sức mạnh Big 3 (Squat, Bench, Deadlift) thêm 20%, tăng 3kg khối lượng cơ nạc sạch.",
                    StartDate = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    RoutineDetails = @"- Thứ 2: Heavy Squat 5x5 (80% 1RM), Pause Squat 3x5, Bulgarian Split Squat 3x8.
- Thứ 4: Heavy Bench Press 5x5, Close-Grip Bench 3x8, Incline Dumbbell Bench 3x10.
- Thứ 6: Conventional Deadlift 5x3, Barbell Row 4x6, Farmer Walk 3 hiệp nặng.
- Thứ 7: Phụ trợ vai tay, bài tập bụng chống xoay vòng Paloff Press.".Replace("\r\n", "\n"),
                    NutritionAdvice = @"- Calo mục tiêu: 2.800 - 3.000 kcal/ngày (Surplus calo tăng cơ).
- Bổ sung 5g Creatine Monohydrate mỗi ngày vào buổi sáng hoặc ngay sau tập.
- Chia nhỏ 5 bữa ăn/ngày, giàu đạm thịt bò, ức gà, cơm trắng và sữa tươi thanh trùng.".Replace("\r\n", "\n"),
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            };
        }
    }
}
