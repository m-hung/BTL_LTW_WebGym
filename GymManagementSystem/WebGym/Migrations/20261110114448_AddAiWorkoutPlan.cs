using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class AddAiWorkoutPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "WorkoutPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CarbG",
                table: "WorkoutPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FatG",
                table: "WorkoutPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InputJson",
                table: "WorkoutPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanJson",
                table: "WorkoutPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProteinG",
                table: "WorkoutPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "WorkoutPlans",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WorkoutPlans",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TargetKcal",
                table: "WorkoutPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaterMl",
                table: "WorkoutPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: true),
                    AvoidIfInjury = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HowTo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DefaultRestSec = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                    table.ForeignKey(
                        name: "FK_Exercises_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Weekday = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_WorkoutPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "ExerciseId", "AvoidIfInjury", "Category", "DefaultRestSec", "EquipmentId", "HowTo", "IsActive", "Level", "MuscleGroup", "Name" },
                values: new object[,]
                {
                    { 1, "shoulder", "Push", 120, 6, "Nằm ghế phẳng, hạ tạ chạm giữa ngực, khuỷu tay mở khoảng 45 độ, đẩy thẳng lên. Luôn tập có người hỗ trợ hoặc chốt an toàn của Power Rack.", true, "Advanced", "Ngực", "Đẩy ngực tạ đòn (Barbell Bench Press)" },
                    { 2, "shoulder", "Push", 90, 5, "Đặt ghế dưới thanh Smith, hạ thanh chậm tới ngang ngực rồi đẩy lên. Thanh chạy theo ray nên an toàn cho người mới.", true, "Basic", "Ngực", "Đẩy ngực trên Smith Machine" },
                    { 3, "shoulder", "Push", 90, 7, "Nằm ghế phẳng, hai tạ ngang ngực, đẩy lên và khép nhẹ ở đỉnh. Hạ chậm 2-3 giây, không thả rơi tạ.", true, "Basic", "Ngực", "Đẩy ngực tạ đơn (Dumbbell Bench Press)" },
                    { 4, "shoulder", "Push", 90, 7, "Ghế dốc 30-45 độ, đẩy tạ từ ngực trên lên cao. Giữ lưng áp sát ghế, không ưỡn quá mức.", true, "Basic", "Ngực", "Đẩy ngực dốc tạ đơn (Incline Dumbbell Press)" },
                    { 5, null, "Push", 60, 8, "Ngồi thẳng lưng, tay hơi cong, ép hai tay vào trước ngực và giữ 1 giây rồi mở ra chậm.", true, "Basic", "Ngực", "Ép ngực trên máy (Pec Fly)" },
                    { 6, null, "Push", 60, 10, "Đứng giữa giàn cáp, bước một chân lên trước, kéo hai tay chéo xuống trước bụng, cảm nhận ngực siết lại.", true, "Basic", "Ngực", "Ép ngực cáp chéo (Cable Crossover)" },
                    { 7, null, "Push", 60, null, "Hai tay rộng hơn vai, thân thẳng từ đầu tới gót. Người mới có thể chống gối. Hạ ngực gần sàn rồi đẩy lên.", true, "Basic", "Ngực", "Hít đất (Push-up)" },
                    { 8, "shoulder", "Push", 90, 7, "Ngồi ghế dựng thẳng, đẩy hai tạ từ ngang tai lên đỉnh đầu, không khoá cứng khuỷu tay.", true, "Basic", "Vai", "Đẩy vai tạ đơn ngồi (Seated Dumbbell Press)" },
                    { 9, "shoulder,back", "Push", 120, 6, "Đứng thẳng, siết bụng và mông, đẩy thanh đòn từ ngực trên lên quá đầu. Không ngả lưng ra sau.", true, "Advanced", "Vai", "Đẩy vai tạ đòn đứng (Overhead Press)" },
                    { 10, null, "Push", 60, 7, "Đứng thẳng, nâng hai tạ nhẹ sang ngang tới ngang vai, khuỷu hơi cong, hạ chậm.", true, "Basic", "Vai", "Nâng tạ đơn sang ngang (Lateral Raise)" },
                    { 11, null, "Push", 60, 10, "Đứng trước giàn cáp, khuỷu tay ép sát người, đẩy tay cầm xuống tới khi duỗi thẳng tay rồi nâng lên chậm.", true, "Basic", "Tay sau", "Kéo cáp tay sau (Tricep Pushdown)" },
                    { 12, "shoulder", "Push", 60, 7, "Ngồi thẳng, hai tay giữ một tạ sau đầu, duỗi thẳng tay lên trên. Giữ khuỷu tay hướng về phía trước.", true, "Basic", "Tay sau", "Duỗi tay sau qua đầu tạ đơn (Overhead Extension)" },
                    { 13, "shoulder", "Push", 60, null, "Chống tay lên mép ghế sau lưng, hạ người xuống tới khi khuỷu tay vuông góc rồi đẩy lên.", true, "Basic", "Tay sau", "Chống đẩy ghế tay sau (Bench Dip)" },
                    { 14, null, "Pull", 90, 10, "Ngồi thẳng, kéo thanh cáp xuống ngực trên, ép hai bả vai về sau. Không giật người ra sau.", true, "Basic", "Lưng", "Kéo cáp xuống trước ngực (Lat Pulldown)" },
                    { 15, null, "Pull", 90, 10, "Ngồi lưng thẳng, kéo tay cầm về bụng, ép bả vai rồi duỗi tay ra chậm. Không cong lưng.", true, "Basic", "Lưng", "Kéo cáp ngồi (Seated Cable Row)" },
                    { 16, null, "Pull", 75, 7, "Một tay và một gối chống ghế, lưng song song sàn, kéo tạ về hông, giữ 1 giây rồi hạ.", true, "Basic", "Lưng", "Chèo tạ đơn một tay (One-arm Dumbbell Row)" },
                    { 17, "back", "Pull", 120, 6, "Gập người khoảng 45 độ, lưng thẳng, kéo thanh đòn về bụng dưới. Cần giữ lưng trung tính suốt hiệp.", true, "Advanced", "Lưng", "Chèo tạ đòn (Barbell Row)" },
                    { 18, "back", "Pull", 150, 6, "Thanh đòn sát ống chân, lưng thẳng, đẩy hông về trước để đứng dậy. Bắt đầu với mức tạ nhẹ để học kỹ thuật.", true, "Advanced", "Lưng dưới", "Deadlift tạ đòn" },
                    { 19, "shoulder", "Pull", 120, 6, "Nắm xà rộng hơn vai, kéo người lên tới khi cằm qua xà, hạ chậm tới khi duỗi tay.", true, "Advanced", "Lưng", "Hít xà đơn (Pull-up) trên Power Rack" },
                    { 20, null, "Pull", 60, 10, "Đặt cáp ngang mặt, kéo dây thừng về phía trán, mở hai khuỷu tay sang hai bên.", true, "Basic", "Vai sau", "Kéo cáp về mặt (Face Pull)" },
                    { 21, null, "Pull", 60, 8, "Ngồi quay mặt vào máy, mở hai tay về sau ngang vai, giữ 1 giây rồi trở về chậm.", true, "Basic", "Vai sau", "Bay vai sau trên máy (Rear Delt Fly)" },
                    { 22, null, "Pull", 60, 7, "Đứng thẳng, khuỷu tay sát người, cuốn tạ lên tới vai rồi hạ chậm. Không đung đưa người.", true, "Basic", "Tay trước", "Cuốn tạ đơn tay trước (Dumbbell Curl)" },
                    { 23, null, "Pull", 60, 7, "Cầm tạ lòng bàn tay hướng vào nhau, cuốn lên tới vai, hạ chậm.", true, "Basic", "Tay trước", "Cuốn tạ búa (Hammer Curl)" },
                    { 24, null, "Pull", 60, 10, "Đứng trước giàn cáp, cuốn thanh từ dưới lên ngực, khuỷu tay cố định.", true, "Basic", "Tay trước", "Cuốn cáp tay trước (Cable Curl)" },
                    { 25, "knee,back", "Legs", 150, 6, "Đặt thanh trên vai sau trong Power Rack, ngồi xuống tới khi đùi song song sàn, gối cùng hướng mũi chân.", true, "Advanced", "Chân", "Squat tạ đòn (Barbell Back Squat)" },
                    { 26, "knee", "Legs", 120, 5, "Đứng dưới thanh Smith, chân hơi bước lên trước, ngồi xuống tới đùi song song sàn rồi đứng dậy.", true, "Basic", "Chân", "Squat trên Smith Machine" },
                    { 27, "knee", "Legs", 90, 7, "Ôm một tạ đơn trước ngực, ngồi xuống sâu, lưng thẳng, gót chân không nhấc khỏi sàn.", true, "Basic", "Chân", "Goblet Squat tạ đơn" },
                    { 28, "knee", "Legs", 90, 9, "Ngồi tựa lưng, đặt chân rộng bằng vai, hạ bàn đạp tới khi gối vuông góc rồi đẩy lên, không khoá gối.", true, "Basic", "Chân", "Đạp đùi 45 độ (Leg Press)" },
                    { 29, "back", "Legs", 90, 7, "Cầm hai tạ trước đùi, gối hơi chùng, đẩy hông ra sau cho tới khi căng đùi sau rồi đứng dậy.", true, "Basic", "Đùi sau", "Romanian Deadlift tạ đơn" },
                    { 30, "knee", "Legs", 75, 7, "Cầm hai tạ, bước dài lên trước, hạ gối sau gần sàn rồi bước tiếp chân kia.", true, "Basic", "Chân", "Chùng chân bước tới (Walking Lunge)" },
                    { 31, "knee", "Legs", 90, 7, "Gác mu bàn chân sau lên ghế, hạ người bằng chân trước tới khi đùi song song sàn. Làm đủ số lần mỗi bên.", true, "Advanced", "Chân", "Bulgarian Split Squat" },
                    { 32, null, "Legs", 90, 6, "Tựa lưng trên vào ghế, thanh đòn trên hông, đẩy hông lên tới khi thân thẳng, siết mông 1 giây.", true, "Basic", "Mông", "Đẩy hông với tạ đòn (Hip Thrust)" },
                    { 33, null, "Legs", 60, 10, "Đeo dây cáp vào cổ chân, đá chân thẳng ra sau, siết mông rồi đưa về chậm.", true, "Basic", "Mông", "Đá chân sau với cáp (Cable Kickback)" },
                    { 34, null, "Legs", 60, 5, "Đứng dưới thanh Smith, nhón gót lên cao nhất, giữ 1 giây rồi hạ xuống chậm.", true, "Basic", "Bắp chân", "Nhón bắp chân trên Smith (Calf Raise)" },
                    { 35, null, "Core", 45, null, "Chống khuỷu tay và mũi chân, thân thẳng từ đầu tới gót, siết bụng. Số lần ghi theo giây.", true, "Basic", "Bụng", "Plank" },
                    { 36, "back", "Core", 60, 10, "Quỳ trước giàn cáp, giữ dây thừng cạnh đầu, gập bụng kéo khuỷu tay về phía gối.", true, "Basic", "Bụng", "Gập bụng với cáp (Cable Crunch)" },
                    { 37, "shoulder", "Core", 60, 6, "Treo người trên xà, nâng hai chân lên ngang hông, hạ chậm và không đung đưa.", true, "Advanced", "Bụng", "Nâng chân treo xà (Hanging Leg Raise)" },
                    { 38, null, "Core", 45, 10, "Đứng nghiêng với giàn cáp, đẩy tay cầm thẳng ra trước ngực và giữ người không bị xoay.", true, "Basic", "Bụng", "Chống xoay với cáp (Pallof Press)" },
                    { 39, null, "Core", 45, null, "Nằm ngửa, tay chân giơ lên, duỗi tay và chân đối diện ra xa, lưng dưới luôn áp sàn.", true, "Basic", "Bụng", "Dead Bug" },
                    { 40, "knee", "Cardio", 0, 1, "Khởi động 3 phút đi bộ, sau đó đi dốc 8-12% hoặc chạy nhẹ ở mức vẫn nói chuyện được.", true, "Basic", "Cardio", "Đi bộ dốc / chạy bộ trên máy Technogym" },
                    { 41, "knee", "Cardio", 0, 2, "Khởi động 3 phút đi bộ, sau đó chạy nhẹ hoặc xen kẽ 1 phút nhanh, 2 phút chậm.", true, "Basic", "Cardio", "Chạy bộ trên máy Life Fitness" },
                    { 42, null, "Cardio", 0, 3, "Chỉnh yên ngang hông, đạp đều ở mức vừa, có thể xen kẽ 30 giây đạp nhanh.", true, "Basic", "Cardio", "Đạp xe Matrix CXP" },
                    { 43, "back", "Cardio", 0, 4, "Đẩy chân trước, ngả người nhẹ rồi mới kéo tay. Giữ lưng thẳng suốt động tác.", true, "Basic", "Cardio", "Chèo thuyền Concept2" }
                });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CarbG", "FatG", "InputJson", "PlanJson", "ProteinG", "Source", "Status", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null, "PT", "Active", null, null });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CarbG", "FatG", "InputJson", "PlanJson", "ProteinG", "Source", "Status", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null, "PT", "Active", null, null });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CarbG", "FatG", "InputJson", "PlanJson", "ProteinG", "Source", "Status", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null, "PT", "Active", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_EquipmentId",
                table: "Exercises",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ExerciseId",
                table: "WorkoutLogs",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_MemberId",
                table: "WorkoutLogs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_PlanId_Date_ExerciseId",
                table: "WorkoutLogs",
                columns: new[] { "PlanId", "Date", "ExerciseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutLogs");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropColumn(
                name: "CarbG",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "FatG",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "InputJson",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "PlanJson",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "ProteinG",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "TargetKcal",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "WaterMl",
                table: "WorkoutPlans");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "WorkoutPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
