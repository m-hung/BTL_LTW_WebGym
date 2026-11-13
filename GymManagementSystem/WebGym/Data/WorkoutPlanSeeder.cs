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
                },

                // === GIÁO ÁN AI CHỜ PT NHẬT DUYỆT (Source = "AI", Status = "PendingReview") ===
                new WorkoutPlan
                {
                    Id = 6,
                    Title = "[AI Smart] Giảm mỡ & săn chắc toàn thân 4 buổi/tuần (AI)",
                    MemberId = 2, // Đặng Thanh Hương (đã có ca Approved/Completed với PT Nhật)
                    TrainerId = null,
                    Goal = "AI khuyến nghị: Giảm mỡ vùng bụng đùi về 20% bodyfat, giữ khối lượng cơ nạc với lịch Push/Pull/Legs + Core.",
                    StartDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                    Source = "AI",
                    Status = "PendingReview",
                    InputJson = "{\"heightCm\":162,\"weightKg\":54.5,\"bodyFatPercent\":23.0,\"muscleMassKg\":22.5,\"dateOfBirth\":\"1998-05-12T00:00:00\",\"gender\":\"Nữ\",\"goal\":\"fat_loss\",\"sessionsPerWeek\":4,\"weekdays\":[1,2,4,6],\"sessionMinutes\":60,\"experience\":\"experienced\",\"activity\":\"sedentary\",\"diet\":\"clean\",\"allergies\":[],\"injuries\":[],\"condition\":\"none\",\"consent\":true}",
                    PlanJson = "{\"version\":1,\"targets\":{\"goal\":\"fat_loss\",\"age\":28,\"bmi\":20.8,\"bmr\":1240,\"bmrSource\":\"Mifflin-St Jeor\",\"activityFactor\":1.45,\"tdee\":1798,\"targetKcal\":1498,\"kcalDelta\":-300,\"proteinPerKg\":1.9,\"proteinG\":104,\"fatG\":42,\"carbG\":176,\"proteinPct\":28,\"fatPct\":25,\"carbPct\":47,\"waterMl\":2200,\"waterMlTrainingDay\":2700,\"estimateText\":\"Khoảng 8-10 tuần giảm ~2.5-3 kg mỡ bền vững\",\"explainKcal\":\"Thâm hụt nhẹ 300 kcal/ngày giúp giảm mỡ mà không mất sức.\",\"explainMacro\":\"Đạm 1.9g/kg bảo vệ cơ nạc trong giai đoạn siết.\",\"explainWater\":\"Uống 2.2L ngày nghỉ và 2.7L ngày tập.\",\"explainEstimate\":\"Ước tính dựa trên 4 buổi/tuần kết hợp ăn sạch.\"},\"splitName\":\"Thân trên / Thân dưới 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":4,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":\"Kiểm soát hạ tạ 2 giây\"},{\"exerciseId\":14,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":10,\"sets\":3,\"reps\":\"12-15\",\"restSec\":60,\"note\":null},{\"exerciseId\":35,\"sets\":3,\"reps\":\"45 giây\",\"restSec\":45,\"note\":\"Siết chặt core\"}],\"cardio\":\"20 phút đi bộ dốc trên máy Technogym\"},{\"weekday\":2,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":32,\"sets\":4,\"reps\":\"10-12\",\"restSec\":90,\"note\":\"Giữ đỉnh 1 giây siết mông\"},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":28,\"sets\":3,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":null},{\"weekday\":4,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":5,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null},{\"exerciseId\":15,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":6,\"focus\":\"Thân dưới B\",\"exercises\":[{\"exerciseId\":27,\"sets\":4,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":31,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":90,\"note\":null},{\"exerciseId\":33,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":36,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null}],\"cardio\":null}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Yến mạch sữa hạnh nhân + 2 trứng luộc\",\"kcal\":360,\"proteinG\":22},{\"slot\":\"Trưa\",\"dish\":\"150g ức gà áp chảo + 120g cơm gạo lứt + rau luộc\",\"kcal\":470,\"proteinG\":40},{\"slot\":\"Xế (trước tập)\",\"dish\":\"1 quả chuối + 1 hũ sữa chua Hy Lạp\",\"kcal\":220,\"proteinG\":14},{\"slot\":\"Tối\",\"dish\":\"150g cá hồi nướng + salad bơ rau xanh\",\"kcal\":448,\"proteinG\":32}],\"tips\":[\"Ngủ đủ 7-8 giờ mỗi đêm để phục hồi cơ và ổn định hormone.\",\"Tăng mức tạ 2.5kg khi vượt quá số rep mục tiêu ở cả 3 hiệp.\"],\"origin\":\"ai\",\"notices\":[\"Ưu tiên bài Hip Thrust & kéo lưng cải thiện vóc dáng và cột sống.\"],\"generatedAt\":\"2026-11-10T09:15:00Z\"}",
                    RoutineDetails = "- Thứ 2 (Thân trên A): Đẩy ngực dốc tạ đơn 3x10-12, Lat Pulldown 3x10-12, Lateral Raise 3x12-15, Plank 3x45s + Cardio 20p.\n- Thứ 3 (Thân dưới A): Hip Thrust 4x10-12, Romanian Deadlift 3x10-12, Leg Press 3x12, Dead Bug 3x12.\n- Thứ 5 (Thân trên B): Pec Fly 3x12, Seated Cable Row 3x10-12, Face Pull 3x15, Pallof Press 3x12 + Đạp xe 20p.\n- Thứ 7 (Thân dưới B): Goblet Squat 4x10-12, Bulgarian Split Squat 3x10, Cable Kickback 3x12, Cable Crunch 3x15.",
                    NutritionAdvice = "- Calo mục tiêu: 1.498 kcal/ngày (Thâm hụt 300 kcal).\n- Đạm 104g, Tinh bột 176g, Chất béo 42g/ngày.\n- Uống 2.2L - 2.7L nước mỗi ngày.",
                    TargetKcal = 1498,
                    ProteinG = 104,
                    CarbG = 176,
                    FatG = 42,
                    WaterMl = 2700,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 10, 9, 15, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new WorkoutPlan
                {
                    Id = 7,
                    Title = "[AI Smart] Lộ trình thon gọn eo & tăng cơ mông đùi 3 buổi (AI)",
                    MemberId = 21, // Trần Bảo Ngọc
                    TrainerId = null,
                    Goal = "AI khuyến nghị: Giảm mỡ nhẹ từ 23.5% xuống 20%, tăng độ săn chắc vòng 3 và cơ bụng cốt lõi.",
                    StartDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                    Source = "AI",
                    Status = "PendingReview",
                    InputJson = "{\"heightCm\":164,\"weightKg\":56.0,\"bodyFatPercent\":23.5,\"muscleMassKg\":21.8,\"dateOfBirth\":\"2000-03-14T00:00:00\",\"gender\":\"Nữ\",\"goal\":\"fat_loss\",\"sessionsPerWeek\":3,\"weekdays\":[1,3,5],\"sessionMinutes\":60,\"experience\":\"beginner\",\"activity\":\"moderate\",\"diet\":\"clean\",\"allergies\":[\"seafood\"],\"injuries\":[],\"condition\":\"none\",\"consent\":true}",
                    PlanJson = "{\"version\":1,\"targets\":{\"goal\":\"fat_loss\",\"age\":26,\"bmi\":20.8,\"bmr\":1320,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.5,\"tdee\":1980,\"targetKcal\":1630,\"kcalDelta\":-350,\"proteinPerKg\":1.8,\"proteinG\":101,\"fatG\":45,\"carbG\":205,\"proteinPct\":25,\"fatPct\":25,\"carbPct\":50,\"waterMl\":2300,\"waterMlTrainingDay\":2800,\"estimateText\":\"Khoảng 8 tuần giảm ~3kg mỡ, vòng eo giảm 3-4 cm\",\"explainKcal\":\"Thâm hụt 350 kcal phù hợp lịch 3 buổi/tuần và vận động vừa.\",\"explainMacro\":\"Thực đơn không hải sản theo khai báo dị ứng.\",\"explainWater\":\"Bổ sung thêm 500ml nước trong các buổi tập chân mông.\",\"explainEstimate\":\"Duy trì đủ protein và giấc ngủ sẽ thấy rõ sau 4 tuần.\"},\"splitName\":\"Toàn thân 3 buổi (A - B - A)\",\"days\":[{\"weekday\":1,\"focus\":\"Toàn thân A\",\"exercises\":[{\"exerciseId\":26,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":\"Xuống chậm kiểm soát gối\"},{\"exerciseId\":2,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":14,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":32,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":\"Siết đỉnh 1 giây\"},{\"exerciseId\":35,\"sets\":3,\"reps\":\"45 giây\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút đi bộ dốc 8%\"},{\"weekday\":3,\"focus\":\"Toàn thân B\",\"exercises\":[{\"exerciseId\":29,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":8,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":15,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":33,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":5,\"focus\":\"Chân & Mông + Core\",\"exercises\":[{\"exerciseId\":28,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":30,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":75,\"note\":null},{\"exerciseId\":16,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":75,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút đi bộ dốc trên máy Technogym\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Bánh mì nguyên cám + 2 trứng ốp la + 1/2 quả bơ\",\"kcal\":390,\"proteinG\":20},{\"slot\":\"Trưa\",\"dish\":\"150g ức gà nướng mật ong + 130g cơm + bông cải xanh\",\"kcal\":520,\"proteinG\":40},{\"slot\":\"Xế (trước tập)\",\"dish\":\"100g khoai lang hấp + 1 muỗng Whey Isolate\",\"kcal\":240,\"proteinG\":25},{\"slot\":\"Tối\",\"dish\":\"150g thịt thăn bò xào rau củ + súp lơ\",\"kcal\":480,\"proteinG\":36}],\"tips\":[\"Không ăn hải sản (tôm, cua, mực) theo hồ sơ dị ứng đã ghi nhận.\",\"Giữ lưng thẳng khi thực hiện Romanian Deadlift.\"],\"origin\":\"ai\",\"notices\":[\"Đã loại bỏ hải sản khỏi gợi ý thực đơn.\"],\"generatedAt\":\"2026-11-11T08:40:00Z\"}",
                    RoutineDetails = "- Thứ 2 (Toàn thân A): Squat Smith 3x10-12, Đẩy ngực Smith 3x10-12, Lat Pulldown 3x12, Hip Thrust 4x12, Plank 3x45s.\n- Thứ 4 (Toàn thân B): Romanian Deadlift 3x10-12, Đẩy vai tạ đơn 3x12, Seated Cable Row 3x12, Cable Kickback 3x12, Dead Bug 3x12.\n- Thứ 6 (Chân Mông & Core): Leg Press 4x12, Walking Lunge 3x10, Chèo tạ đơn 1 tay 3x12, Pallof Press 3x12.",
                    NutritionAdvice = "- Calo mục tiêu: 1.630 kcal/ngày (Loại bỏ hải sản).\n- Đạm 101g, Tinh bột 205g, Chất béo 45g/ngày.",
                    TargetKcal = 1630,
                    ProteinG = 101,
                    CarbG = 205,
                    FatG = 45,
                    WaterMl = 2800,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 11, 8, 40, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new WorkoutPlan
                {
                    Id = 8,
                    Title = "[AI Smart] Hypertrophy tăng cơ nạc 4 buổi/tuần (AI)",
                    MemberId = 22, // Lê Hoàng Long
                    TrainerId = null,
                    Goal = "AI khuyến nghị: Dư nhẹ 250 kcal để tăng khối lượng cơ nạc toàn thân, nâng tạ đa khớp làm trọng tâm.",
                    StartDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                    Source = "AI",
                    Status = "PendingReview",
                    InputJson = "{\"heightCm\":178,\"weightKg\":82.0,\"bodyFatPercent\":24.0,\"muscleMassKg\":35.0,\"dateOfBirth\":\"1996-07-25T00:00:00\",\"gender\":\"Nam\",\"goal\":\"muscle_gain\",\"sessionsPerWeek\":4,\"weekdays\":[1,2,4,5],\"sessionMinutes\":90,\"experience\":\"experienced\",\"activity\":\"moderate\",\"diet\":\"normal\",\"allergies\":[],\"injuries\":[],\"condition\":\"none\",\"consent\":true}",
                    PlanJson = "{\"version\":1,\"targets\":{\"goal\":\"muscle_gain\",\"age\":30,\"bmi\":25.9,\"bmr\":1810,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.5,\"tdee\":2715,\"targetKcal\":2965,\"kcalDelta\":250,\"proteinPerKg\":2.1,\"proteinG\":172,\"fatG\":78,\"carbG\":394,\"proteinPct\":23,\"fatPct\":24,\"carbPct\":53,\"waterMl\":2900,\"waterMlTrainingDay\":3500,\"estimateText\":\"Tăng 0.8-1.2 kg cơ nạc sau 8 tuần với áp lực tạ tăng tiến\",\"explainKcal\":\"Dư nhẹ 250 kcal giúp tăng cơ hạn chế tích mỡ bụng.\",\"explainMacro\":\"Đạm 2.1g/kg hỗ trợ tổng hợp protein cơ bắp tối ưu.\",\"explainWater\":\"Uống 3.5L vào ngày tập nặng 90 phút.\",\"explainEstimate\":\"Theo dõi tiến độ tăng tạ ở Bench Press, Squat và Barbell Row.\"},\"splitName\":\"Thân trên / Thân dưới 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":1,\"sets\":4,\"reps\":\"6-8\",\"restSec\":120,\"note\":\"Chạm ngực kiểm soát\"},{\"exerciseId\":17,\"sets\":4,\"reps\":\"8\",\"restSec\":120,\"note\":null},{\"exerciseId\":9,\"sets\":3,\"reps\":\"8-10\",\"restSec\":120,\"note\":null},{\"exerciseId\":11,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null},{\"exerciseId\":22,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":2,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":25,\"sets\":4,\"reps\":\"6-8\",\"restSec\":150,\"note\":\"Phát lực mạnh từ giữa chân\"},{\"exerciseId\":28,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":34,\"sets\":4,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":37,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":4,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":4,\"sets\":4,\"reps\":\"8-10\",\"restSec\":90,\"note\":null},{\"exerciseId\":19,\"sets\":4,\"reps\":\"8-10\",\"restSec\":120,\"note\":null},{\"exerciseId\":10,\"sets\":4,\"reps\":\"12-15\",\"restSec\":60,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":23,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":5,\"focus\":\"Thân dưới B\",\"exercises\":[{\"exerciseId\":18,\"sets\":4,\"reps\":\"5-6\",\"restSec\":150,\"note\":\"Khởi động kỹ lưng dưới\"},{\"exerciseId\":31,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":90,\"note\":null},{\"exerciseId\":32,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":36,\"sets\":4,\"reps\":\"15\",\"restSec\":60,\"note\":null}],\"cardio\":\"15 phút chèo thuyền Concept2\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Phở bò tái chín + 2 trứng chần + 1 ly sữa tươi\",\"kcal\":650,\"proteinG\":42},{\"slot\":\"Trưa\",\"dish\":\"200g ức gà / thăn bò + 2 chén cơm + rau xanh\",\"kcal\":820,\"proteinG\":55},{\"slot\":\"Xế (trước tập)\",\"dish\":\"2 lát bánh mì bơ đậu phộng + 1 muỗng Whey\",\"kcal\":440,\"proteinG\":32},{\"slot\":\"Tối\",\"dish\":\"200g cá hồi / thịt nạc + 1.5 chén cơm + canh rau\",\"kcal\":780,\"proteinG\":48}],\"tips\":[\"Ghi chép mức tạ sau mỗi buổi tập để tăng tiến đều đặn.\",\"Ngủ đủ 8 tiếng giúp tối đa hóa hormone tăng trưởng.\"],\"origin\":\"ai\",\"notices\":[\"Giáo án cường độ cao dành cho người đã có nền tảng tập trên 6 tháng.\"],\"generatedAt\":\"2026-11-11T14:20:00Z\"}",
                    RoutineDetails = "- Thứ 2 (Thân trên A): Bench Press 4x6-8, Barbell Row 4x8, Overhead Press 3x8-10, Tricep Pushdown 3x12, Dumbbell Curl 3x12.\n- Thứ 3 (Thân dưới A): Barbell Squat 4x6-8, Leg Press 3x10-12, Romanian Deadlift 3x10, Calf Raise 4x15, Hanging Leg Raise 3x12.\n- Thứ 5 (Thân trên B): Incline Dumbbell Press 4x8-10, Pull-up 4x8-10, Lateral Raise 4x12-15, Face Pull 3x15, Hammer Curl 3x12.\n- Thứ 6 (Thân dưới B): Deadlift 4x5-6, Bulgarian Split Squat 3x10, Hip Thrust 3x10, Cable Crunch 4x15.",
                    NutritionAdvice = "- Calo mục tiêu: 2.965 kcal/ngày (Surplus +250 kcal).\n- Đạm 172g, Tinh bột 394g, Chất béo 78g/ngày.",
                    TargetKcal = 2965,
                    ProteinG = 172,
                    CarbG = 394,
                    FatG = 78,
                    WaterMl = 3500,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 11, 14, 20, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new WorkoutPlan
                {
                    Id = 9,
                    Title = "[AI Smart] Tăng thể lực & sức mạnh nền tảng 4 buổi (AI)",
                    MemberId = 24, // Nguyễn Hữu Đức
                    TrainerId = null,
                    Goal = "AI khuyến nghị: Duy trì cân nặng hiện tại 75kg, giảm dần tỷ lệ mỡ về 17% và cải thiện VO2Max kết hợp sức mạnh.",
                    StartDate = new DateTime(2026, 11, 12, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 7, 0, 0, 0, DateTimeKind.Utc),
                    Source = "AI",
                    Status = "PendingReview",
                    InputJson = "{\"heightCm\":172,\"weightKg\":75.0,\"bodyFatPercent\":21.0,\"muscleMassKg\":33.0,\"dateOfBirth\":\"1994-09-08T00:00:00\",\"gender\":\"Nam\",\"goal\":\"endurance\",\"sessionsPerWeek\":4,\"weekdays\":[1,3,5,7],\"sessionMinutes\":60,\"experience\":\"experienced\",\"activity\":\"moderate\",\"diet\":\"clean\",\"allergies\":[\"dairy\"],\"injuries\":[],\"condition\":\"none\",\"consent\":true}",
                    PlanJson = "{\"version\":1,\"targets\":{\"goal\":\"endurance\",\"age\":32,\"bmi\":25.4,\"bmr\":1700,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.55,\"tdee\":2635,\"targetKcal\":2635,\"kcalDelta\":0,\"proteinPerKg\":1.9,\"proteinG\":143,\"fatG\":72,\"carbG\":354,\"proteinPct\":22,\"fatPct\":25,\"carbPct\":53,\"waterMl\":2700,\"waterMlTrainingDay\":3300,\"estimateText\":\"Thể lực và nhịp tim phục hồi cải thiện rõ rệt sau 4-6 tuần\",\"explainKcal\":\"Nạp đủ TDEE giúp phục hồi tốt giữa các buổi tạ và cardio.\",\"explainMacro\":\"Thực đơn không sữa bò theo khai báo dị ứng (dùng đạm thịt, cá, trứng, đậu).\",\"explainWater\":\"Bù điện giải sau mỗi buổi chèo thuyền hoặc chạy bộ.\",\"explainEstimate\":\"Kết hợp bài đa khớp với 20-25 phút cardio Zone 2/HIIT.\"},\"splitName\":\"Thân trên / Thân dưới + Cardio 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":3,\"sets\":4,\"reps\":\"10\",\"restSec\":75,\"note\":null},{\"exerciseId\":14,\"sets\":4,\"reps\":\"10\",\"restSec\":75,\"note\":null},{\"exerciseId\":8,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":35,\"sets\":3,\"reps\":\"60 giây\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút chạy bộ trên máy Life Fitness\"},{\"weekday\":3,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":26,\"sets\":4,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":30,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút chèo thuyền Concept2\"},{\"weekday\":5,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":7,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":15,\"sets\":4,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":11,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":7,\"focus\":\"Thân dưới & Core\",\"exercises\":[{\"exerciseId\":28,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":32,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"25 phút đi bộ dốc / chạy bộ trên máy Technogym\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Cháo yến mạch nấu thịt bằm + 2 trứng ốp la\",\"kcal\":560,\"proteinG\":32},{\"slot\":\"Trưa\",\"dish\":\"200g ức gà nướng + 2 chén cơm gạo lứt + rau xanh\",\"kcal\":750,\"proteinG\":48},{\"slot\":\"Xế (trước tập)\",\"dish\":\"2 quả chuối + 30g hạt hạnh nhân\",\"kcal\":360,\"proteinG\":12},{\"slot\":\"Tối\",\"dish\":\"200g cá thu sốt cà + 1.5 chén cơm + canh rau cải\",\"kcal\":765,\"proteinG\":46}],\"tips\":[\"Không sử dụng chế phẩm từ sữa bò (whey cô đặc, phô mai, sữa tươi) theo hồ sơ dị ứng.\",\"Giữ nhịp thở đều trong lúc chèo thuyền Concept2.\"],\"origin\":\"ai\",\"notices\":[\"Đã loại trừ sữa và chế phẩm từ sữa trong thực đơn.\"],\"generatedAt\":\"2026-11-12T07:30:00Z\"}",
                    RoutineDetails = "- Thứ 2 (Thân trên A): Dumbbell Bench Press 4x10, Lat Pulldown 4x10, Seated Dumbbell Press 3x12, Plank 3x60s + Chạy bộ 20p.\n- Thứ 4 (Thân dưới A): Squat Smith 4x10, Romanian Deadlift 3x10, Walking Lunge 3x12, Dead Bug 3x12 + Chèo thuyền 15p.\n- Thứ 6 (Thân trên B): Push-up 3x15, Seated Cable Row 4x12, Face Pull 3x15, Tricep Pushdown 3x12 + Đạp xe 20p.\n- Chủ nhật (Thân dưới & Core): Leg Press 4x12, Hip Thrust 3x12, Pallof Press 3x12 + Cardio 25p.",
                    NutritionAdvice = "- Calo mục tiêu: 2.635 kcal/ngày (Loại bỏ sữa theo hồ sơ dị ứng).\n- Đạm 143g, Tinh bột 354g, Chất béo 72g/ngày.",
                    TargetKcal = 2635,
                    ProteinG = 143,
                    CarbG = 354,
                    FatG = 72,
                    WaterMl = 3300,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 11, 12, 7, 30, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
            };
        }
    }
}
