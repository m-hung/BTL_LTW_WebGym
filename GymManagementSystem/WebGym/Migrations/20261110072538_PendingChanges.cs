using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 2.100 kcal/ngày (Deficit 300 kcal).\r\n- Đạm (Protein): 160g - 170g/ngày (~2.2g/kg thể trọng), ưu tiên ức gà, cá ngừ, trứng, Whey Isolate.\r\n- Tinh bột (Carb): 200g/ngày (khoai lang, yến mạch, cơm gạo lứt).\r\n- Chất béo tốt: 50g/ngày (dầu ô liu, quả bơ, hạt hạnh nhân).\r\n- Uống đủ 3 - 3.5 lít nước mỗi ngày, hạn chế tối đa bia rượu và đồ ngọt đóng chai.", "- Thứ 2 (Ngực & Tay sau): Bench Press 4x8, Incline Dumbbell Press 4x10, Cable Fly 3x12, Tricep Pushdown 4x12.\r\n- Thứ 3 (Lưng xô & Tay trước): Deadlift 4x6, Lat Pulldown 4x10, Barbell Row 4x8, Bicep Curls 3x12.\r\n- Thứ 4: Nghỉ ngơi tích cực hoặc Cardio nhẹ 30 phút đi bộ dốc.\r\n- Thứ 5 (Chân & Bắp chân): Barbell Squat 4x8, Leg Press 4x12, Leg Curl 3x15, Calf Raise 4x20.\r\n- Thứ 6 (Vai & Bụng): Overhead Press 4x8, Lateral Raise 4x15, Face Pull 3x15, Hanging Leg Raise 4x15.\r\n- Thứ 7: HIIT Cardio chạy bộ máy Technogym 25 phút.\r\n- Chủ nhật: Nghỉ phục hồi toàn diện." });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 1.450 kcal/ngày.\r\n- Ưu tiên rau xanh nhiều chất xơ, cá hồi, tôm, sữa chua Hy Lạp và sinh tố hoa quả mọng (berries).\r\n- Hạn chế gia vị nêm mặn nhằm tránh tích nước dưới da vùng bụng.\r\n- Uống trà hoa cúc hoặc trà xanh ấm buổi tối.", "- Thứ 2: Tập mông đùi Glute Focus: Hip Thrust 4x12, Romanian Deadlift 4x10, Bulgarian Split Squat 3x12 mỗi bên.\r\n- Thứ 3: Lớp Hatha Yoga phục hồi & giãn cơ chuyên sâu 75 phút.\r\n- Thứ 4: Thân trên nữ & Lưng xô: Lat Pulldown 3x12, Dumbbell Shoulder Press 3x12, Plank 4x45s.\r\n- Thứ 5: Nghỉ ngơi hoặc đạp xe Spinning kháng lực vừa phải 40 phút.\r\n- Thứ 6: Pilates thảm Matwork & Core Cốt Lõi: Động tác The Hundred, Criss Cross, Teaser.\r\n- Thứ 7 & CN: Đi dạo ngoài trời, thiền định." });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 2.800 - 3.000 kcal/ngày (Surplus calo tăng cơ).\r\n- Bổ sung 5g Creatine Monohydrate mỗi ngày vào buổi sáng hoặc ngay sau tập.\r\n- Chia nhỏ 5 bữa ăn/ngày, giàu đạm thịt bò, ức gà, cơm trắng và sữa tươi thanh trùng.", "- Thứ 2: Heavy Squat 5x5 (80% 1RM), Pause Squat 3x5, Bulgarian Split Squat 3x8.\r\n- Thứ 4: Heavy Bench Press 5x5, Close-Grip Bench 3x8, Incline Dumbbell Bench 3x10.\r\n- Thứ 6: Conventional Deadlift 5x3, Barbell Row 4x6, Farmer Walk 3 hiệp nặng.\r\n- Thứ 7: Phụ trợ vai tay, bài tập bụng chống xoay vòng Paloff Press." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 2.100 kcal/ngày (Deficit 300 kcal).\n- Đạm (Protein): 160g - 170g/ngày (~2.2g/kg thể trọng), ưu tiên ức gà, cá ngừ, trứng, Whey Isolate.\n- Tinh bột (Carb): 200g/ngày (khoai lang, yến mạch, cơm gạo lứt).\n- Chất béo tốt: 50g/ngày (dầu ô liu, quả bơ, hạt hạnh nhân).\n- Uống đủ 3 - 3.5 lít nước mỗi ngày, hạn chế tối đa bia rượu và đồ ngọt đóng chai.", "- Thứ 2 (Ngực & Tay sau): Bench Press 4x8, Incline Dumbbell Press 4x10, Cable Fly 3x12, Tricep Pushdown 4x12.\n- Thứ 3 (Lưng xô & Tay trước): Deadlift 4x6, Lat Pulldown 4x10, Barbell Row 4x8, Bicep Curls 3x12.\n- Thứ 4: Nghỉ ngơi tích cực hoặc Cardio nhẹ 30 phút đi bộ dốc.\n- Thứ 5 (Chân & Bắp chân): Barbell Squat 4x8, Leg Press 4x12, Leg Curl 3x15, Calf Raise 4x20.\n- Thứ 6 (Vai & Bụng): Overhead Press 4x8, Lateral Raise 4x15, Face Pull 3x15, Hanging Leg Raise 4x15.\n- Thứ 7: HIIT Cardio chạy bộ máy Technogym 25 phút.\n- Chủ nhật: Nghỉ phục hồi toàn diện." });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 1.450 kcal/ngày.\n- Ưu tiên rau xanh nhiều chất xơ, cá hồi, tôm, sữa chua Hy Lạp và sinh tố hoa quả mọng (berries).\n- Hạn chế gia vị nêm mặn nhằm tránh tích nước dưới da vùng bụng.\n- Uống trà hoa cúc hoặc trà xanh ấm buổi tối.", "- Thứ 2: Tập mông đùi Glute Focus: Hip Thrust 4x12, Romanian Deadlift 4x10, Bulgarian Split Squat 3x12 mỗi bên.\n- Thứ 3: Lớp Hatha Yoga phục hồi & giãn cơ chuyên sâu 75 phút.\n- Thứ 4: Thân trên nữ & Lưng xô: Lat Pulldown 3x12, Dumbbell Shoulder Press 3x12, Plank 4x45s.\n- Thứ 5: Nghỉ ngơi hoặc đạp xe Spinning kháng lực vừa phải 40 phút.\n- Thứ 6: Pilates thảm Matwork & Core Cốt Lõi: Động tác The Hundred, Criss Cross, Teaser.\n- Thứ 7 & CN: Đi dạo ngoài trời, thiền định." });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NutritionAdvice", "RoutineDetails" },
                values: new object[] { "- Calo mục tiêu: 2.800 - 3.000 kcal/ngày (Surplus calo tăng cơ).\n- Bổ sung 5g Creatine Monohydrate mỗi ngày vào buổi sáng hoặc ngay sau tập.\n- Chia nhỏ 5 bữa ăn/ngày, giàu đạm thịt bò, ức gà, cơm trắng và sữa tươi thanh trùng.", "- Thứ 2: Heavy Squat 5x5 (80% 1RM), Pause Squat 3x5, Bulgarian Split Squat 3x8.\n- Thứ 4: Heavy Bench Press 5x5, Close-Grip Bench 3x8, Incline Dumbbell Bench 3x10.\n- Thứ 6: Conventional Deadlift 5x3, Barbell Row 4x6, Farmer Walk 3 hiệp nặng.\n- Thứ 7: Phụ trợ vai tay, bài tập bụng chống xoay vòng Paloff Press." });
        }
    }
}
