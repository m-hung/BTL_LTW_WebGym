using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class ExpandSeedDataForMembersAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "MemberId", "Notes", "Status", "TimeSlot", "TrainerId" },
                values: new object[,]
                {
                    { 33, new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tập eo thon và cơ lưng dưới", "Pending", "17:30 - 19:00", 1 },
                    { 34, new DateTime(2026, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tập tăng cơ đùi và điều chỉnh tư thế bước đi", "Approved", "14:00 - 15:30", 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "MemberId", "PackageId", "PaymentDate", "PaymentMethod", "Status", "TransactionCode" },
                values: new object[,]
                {
                    { 21, 500000m, 1, 1, new DateTime(2026, 1, 15, 9, 10, 22, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260115091022" },
                    { 24, 1300000m, 1, 2, new DateTime(2026, 2, 15, 11, 15, 30, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260215111530" },
                    { 26, 500000m, 3, 1, new DateTime(2026, 2, 24, 16, 45, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260224164500" },
                    { 28, 500000m, 2, 1, new DateTime(2026, 3, 18, 9, 5, 14, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260318090514" },
                    { 29, 500000m, 5, 1, new DateTime(2026, 3, 28, 18, 30, 0, 0, DateTimeKind.Utc), "Cash", "Failed", "CSH20260328183000" },
                    { 33, 1300000m, 1, 2, new DateTime(2026, 5, 17, 9, 45, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260517094500" },
                    { 46, 1300000m, 1, 2, new DateTime(2026, 11, 2, 9, 20, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261102092000" }
                });

            migrationBuilder.InsertData(
                table: "Progress",
                columns: new[] { "ProgressId", "BodyFatPercentage", "Chest", "Hips", "MemberId", "MuscleMass", "Notes", "RecordedDate", "Waist", "Weight" },
                values: new object[,]
                {
                    { 21, 28.0, 96.0, 98.0, 1, 30.5, "Đo InBody khi mới bắt đầu tập luyện. Thể trạng nhiều mỡ thừa vùng eo, thể lực yếu.", new DateTime(2026, 3, 1, 8, 30, 0, 0, DateTimeKind.Utc), 93.0, 83.0 },
                    { 22, 26.5, 97.0, 97.0, 1, 31.199999999999999, "Tập gói cơ bản 3 tháng, giảm được 2.5kg mỡ, bắt đầu hình thành thói quen vận động.", new DateTime(2026, 5, 15, 9, 0, 0, 0, DateTimeKind.Utc), 90.0, 80.5 },
                    { 23, 17.800000000000001, 101.5, 92.0, 1, 35.0, "Vượt mục tiêu đề ra! Tỷ lệ mỡ giảm xuống 17.8%, cơ bắp sắc nét, cơ bụng lộ 6 múi.", new DateTime(2026, 11, 5, 9, 15, 0, 0, DateTimeKind.Utc), 76.5, 69.5 },
                    { 24, 24.0, 84.0, 91.0, 2, 22.5, "Đo chỉ số ban đầu. Mục tiêu giảm mỡ bụng dưới, săn chắc vòng eo và cải thiện số đo vòng 3.", new DateTime(2026, 6, 15, 9, 0, 0, 0, DateTimeKind.Utc), 68.0, 56.0 },
                    { 25, 22.199999999999999, 84.5, 92.0, 2, 23.0, "Vòng eo giảm 3cm, mông căng tròn hơn nhờ các bài Hip Thrust và Romanian Deadlift.", new DateTime(2026, 8, 15, 9, 30, 0, 0, DateTimeKind.Utc), 65.0, 54.5 },
                    { 26, 20.5, 85.0, 92.5, 2, 23.800000000000001, "Form dáng đồng hồ cát rất đẹp, cơ bụng số 11 lộ rõ, sức bền tim mạch tăng rõ rệt.", new DateTime(2026, 10, 20, 10, 0, 0, 0, DateTimeKind.Utc), 63.0, 53.0 },
                    { 27, 26.0, 102.0, 100.0, 3, 34.5, "Thể trạng thừa cân, mỡ nội tạng cao, thường xuyên mệt mỏi khi vận động mạnh.", new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc), 92.0, 84.0 },
                    { 28, 23.0, 103.0, 99.0, 3, 35.5, "Giảm 3.8kg mỡ, tăng 1kg cơ nạc. Thể lực cải thiện, hoàn thành tốt các bài tập tạ nặng.", new DateTime(2026, 7, 20, 14, 30, 0, 0, DateTimeKind.Utc), 87.0, 80.200000000000003 },
                    { 29, 19.800000000000001, 105.0, 98.0, 3, 36.399999999999999, "Eo thon gọn rõ nét, cơ ngực và xô lưng phát triển ấn tượng, mức tạ Bench Press đạt 85kg.", new DateTime(2026, 10, 15, 15, 0, 0, 0, DateTimeKind.Utc), 82.5, 76.799999999999997 },
                    { 30, 21.5, 82.0, 88.0, 4, 20.0, "Thể trạng gầy, cơ bắp yếu, dễ mệt mỏi. Mục tiêu tăng cơ nạc và tăng cân lành mạnh.", new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Utc), 64.0, 49.0 },
                    { 31, 20.5, 83.0, 89.5, 4, 21.300000000000001, "Tăng 1.3kg chủ yếu là cơ nạc, cơ thể có sức sống hơn, thể lực cải thiện rõ rệt.", new DateTime(2026, 10, 10, 10, 30, 0, 0, DateTimeKind.Utc), 63.5, 50.299999999999997 },
                    { 32, 20.0, 83.5, 90.5, 4, 22.100000000000001, "Vóc dáng cân đối, săn chắc, vòng 3 nở nang tự nhiên, hoàn thành giáo án xuất sắc.", new DateTime(2026, 11, 2, 11, 0, 0, 0, DateTimeKind.Utc), 63.0, 51.200000000000003 },
                    { 33, 19.0, 96.0, 94.0, 5, 33.0, "Mục tiêu duy trì thể lực, rèn luyện sức bền và tăng độ nét cơ bắp toàn thân.", new DateTime(2026, 7, 20, 16, 30, 0, 0, DateTimeKind.Utc), 80.0, 73.0 },
                    { 34, 17.199999999999999, 98.0, 94.0, 5, 34.200000000000003, "Cơ vai và bắp tay cắt nét rõ, sức bền chạy bộ 5km đạt thời gian 24 phút.", new DateTime(2026, 9, 15, 17, 0, 0, 0, DateTimeKind.Utc), 78.0, 72.400000000000006 },
                    { 35, 16.0, 99.5, 93.5, 5, 35.0, "Chỉ số cơ thể chuẩn vận động viên thể hình, múi bụng săn chắc, sức đẩy cực khỏe.", new DateTime(2026, 10, 28, 17, 30, 0, 0, DateTimeKind.Utc), 76.5, 71.799999999999997 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "EndDate", "MemberId", "OriginalPrice", "PackageId", "PaidAmount", "StartDate", "Status" },
                values: new object[,]
                {
                    { 21, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, 500000m, 1, 500000m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 22, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1300000m, 2, 1300000m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 23, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1300000m, 2, 1300000m, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 21, new DateTime(2026, 2, 10, 8, 30, 0, 0, DateTimeKind.Utc), "baongoc@gmail.com", "Trần Bảo Ngọc", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654326", "Member", "member_baongoc" },
                    { 22, new DateTime(2026, 3, 5, 9, 15, 0, 0, DateTimeKind.Utc), "hoanglong@gmail.com", "Lê Hoàng Long", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654327", "Member", "member_hoanglong" },
                    { 23, new DateTime(2026, 4, 1, 14, 20, 0, 0, DateTimeKind.Utc), "mailinh@gmail.com", "Đỗ Mai Linh", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654328", "Member", "member_mailinh" },
                    { 24, new DateTime(2026, 1, 5, 10, 0, 0, 0, DateTimeKind.Utc), "huuduc@gmail.com", "Nguyễn Hữu Đức", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654329", "Member", "member_huuduc" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "BMR", "BodyFatPercentage", "Chest", "DateOfBirth", "Gender", "Height", "Hips", "MuscleMass", "UserId", "Waist", "Weight" },
                values: new object[,]
                {
                    { 21, 1320.0, 23.5, 85.0, new DateTime(2000, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 165.0, 90.0, 21.800000000000001, 21, 65.0, 54.0 },
                    { 22, 1810.0, 24.0, 101.0, new DateTime(1996, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 178.0, 98.0, 35.0, 22, 86.0, 82.0 },
                    { 23, 1290.0, 22.0, 83.0, new DateTime(2002, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 160.0, 89.0, 20.5, 23, 63.0, 52.5 },
                    { 24, 1700.0, 21.0, 97.0, new DateTime(1994, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 172.0, 95.0, 33.0, 24, 82.0, 75.0 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "MemberId", "Notes", "Status", "TimeSlot", "TrainerId" },
                values: new object[,]
                {
                    { 21, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Tập cơ mông đùi và cơ bụng dưới (Glutes & Core)", "Completed", "07:00 - 08:30", 1 },
                    { 22, new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Cardio HIIT đốt mỡ toàn thân và giãn cơ", "Completed", "17:00 - 18:30", 1 },
                    { 23, new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Tập thân trên và chỉnh form Squat nâng cao", "Approved", "17:00 - 18:30", 1 },
                    { 24, new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Tập ngực nâng cao - Pyramid Bench Press & Incline Dumbbell", "Completed", "18:00 - 19:30", 1 },
                    { 25, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Tập lưng xô - Barbell Row & Lat Pulldown form check", "Completed", "18:00 - 19:30", 1 },
                    { 26, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Tập chân mông - Barbell Squat 100kg & Romanian Deadlift", "Approved", "18:00 - 19:30", 1 },
                    { 27, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Phục hồi tư thế cột sống và giải mỏi cổ vai gáy", "Completed", "15:30 - 17:00", 1 },
                    { 28, new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Pilates tăng độ dẻo dai cơ bụng và săn chắc eo", "Completed", "15:30 - 17:00", 1 },
                    { 29, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Luyện tập thân dưới và thăng bằng cơ thể", "Pending", "15:30 - 17:00", 1 },
                    { 30, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Tập sức mạnh toàn thân (Full Body Functional Training)", "Completed", "06:30 - 08:00", 1 },
                    { 31, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Tập chuỗi bài Deadlift & Overhead Press", "Completed", "06:30 - 08:00", 1 },
                    { 32, new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Kiểm tra thể lực định kỳ và tăng khối lượng tạ", "Approved", "06:30 - 08:00", 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "MemberId", "PackageId", "PaymentDate", "PaymentMethod", "Status", "TransactionCode" },
                values: new object[,]
                {
                    { 22, 8000000m, 24, 4, new DateTime(2026, 1, 12, 10, 30, 45, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260112103045" },
                    { 23, 500000m, 22, 1, new DateTime(2026, 1, 20, 15, 0, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260120150000" },
                    { 25, 1300000m, 21, 2, new DateTime(2026, 2, 15, 8, 40, 10, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260215084010" },
                    { 27, 4500000m, 22, 3, new DateTime(2026, 3, 12, 14, 20, 55, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260312142055" },
                    { 30, 500000m, 23, 1, new DateTime(2026, 4, 10, 10, 15, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260410101500" },
                    { 31, 500000m, 21, 1, new DateTime(2026, 4, 18, 16, 30, 22, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260418163022" },
                    { 32, 1300000m, 24, 2, new DateTime(2026, 4, 25, 11, 20, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260425112000" },
                    { 34, 4500000m, 21, 3, new DateTime(2026, 5, 18, 10, 30, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260518103000" },
                    { 35, 1300000m, 23, 2, new DateTime(2026, 5, 11, 14, 0, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260511140000" },
                    { 36, 1300000m, 22, 2, new DateTime(2026, 6, 20, 16, 40, 10, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260620164010" },
                    { 37, 500000m, 24, 1, new DateTime(2026, 6, 28, 11, 15, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260628111500" },
                    { 38, 500000m, 21, 1, new DateTime(2026, 7, 14, 15, 20, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260714152000" },
                    { 39, 1300000m, 22, 2, new DateTime(2026, 7, 25, 18, 10, 30, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260725181030" },
                    { 40, 4500000m, 23, 3, new DateTime(2026, 8, 10, 14, 15, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260810141500" },
                    { 41, 4500000m, 24, 3, new DateTime(2026, 8, 22, 9, 30, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260822093000" },
                    { 42, 8000000m, 22, 4, new DateTime(2026, 9, 10, 8, 50, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260910085000" },
                    { 43, 1300000m, 21, 2, new DateTime(2026, 9, 20, 17, 25, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20260920172500" },
                    { 44, 500000m, 23, 1, new DateTime(2026, 10, 12, 11, 5, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261012110500" },
                    { 45, 1300000m, 24, 2, new DateTime(2026, 10, 28, 16, 40, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261028164000" },
                    { 47, 4500000m, 21, 3, new DateTime(2026, 11, 3, 14, 15, 0, 0, DateTimeKind.Utc), "Cash", "Success", "CSH20261103141500" },
                    { 48, 1300000m, 22, 2, new DateTime(2026, 11, 4, 10, 30, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261104103000" },
                    { 49, 8000000m, 23, 4, new DateTime(2026, 11, 5, 15, 45, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261105154500" }
                });

            migrationBuilder.InsertData(
                table: "Progress",
                columns: new[] { "ProgressId", "BodyFatPercentage", "Chest", "Hips", "MemberId", "MuscleMass", "Notes", "RecordedDate", "Waist", "Weight" },
                values: new object[,]
                {
                    { 36, 23.5, 85.0, 90.0, 21, 21.800000000000001, "Buổi kiểm tra thể lực đầu vào. Cơ địa linh hoạt nhưng cơ lưng và cơ mông còn yếu.", new DateTime(2026, 2, 15, 9, 0, 0, 0, DateTimeKind.Utc), 65.0, 54.0 },
                    { 37, 21.199999999999999, 85.5, 91.0, 21, 22.699999999999999, "Vòng eo giảm 2.5cm, mông đẩy cao săn chắc, thích nghi rất tốt với các bài tập kháng lực.", new DateTime(2026, 5, 20, 9, 30, 0, 0, DateTimeKind.Utc), 62.5, 52.799999999999997 },
                    { 38, 19.800000000000001, 86.0, 92.0, 21, 23.300000000000001, "Hình thể chuẩn người mẫu fitness, hoàn thành xuất sắc giáo án tập luyện kèm PT Nhật.", new DateTime(2026, 8, 30, 10, 0, 0, 0, DateTimeKind.Utc), 61.0, 52.0 },
                    { 39, 24.0, 101.0, 98.0, 22, 35.0, "Mục tiêu siết mỡ bụng và phát triển cơ ngực, vai, lưng xô chữ V.", new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), 86.0, 82.0 },
                    { 40, 20.800000000000001, 103.5, 97.0, 22, 36.299999999999997, "Giảm hơn 3kg mỡ, cơ ngực nở rộng, hoàn thành bài Deadlift 120kg đúng kỹ thuật.", new DateTime(2026, 6, 15, 10, 30, 0, 0, DateTimeKind.Utc), 82.0, 78.799999999999997 },
                    { 41, 17.5, 105.5, 96.5, 22, 37.600000000000001, "Lưng chữ V cực kỳ rõ nét, bắp tay cuồn cuộn, nâng cấp thành công lên gói Kim Cương.", new DateTime(2026, 10, 5, 11, 0, 0, 0, DateTimeKind.Utc), 78.5, 76.200000000000003 },
                    { 42, 22.0, 83.0, 89.0, 23, 20.5, "Dân văn phòng ngồi nhiều, cơ cổ vai gáy căng cứng, bụng dưới tích mỡ nhẹ.", new DateTime(2026, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), 63.0, 52.5 },
                    { 43, 20.300000000000001, 83.5, 89.5, 23, 21.399999999999999, "Hết hẳn triệu chứng đau mỏi vai gáy, tư thế đi đứng thẳng thớm và tự tin hơn.", new DateTime(2026, 7, 10, 14, 30, 0, 0, DateTimeKind.Utc), 61.200000000000003, 51.399999999999999 },
                    { 44, 19.199999999999999, 84.0, 90.0, 23, 22.199999999999999, "Vòng eo 60cm săn chắc thon gọn, sức khỏe dẻo dai, rất yêu thích tập luyện tại phòng gym.", new DateTime(2026, 10, 25, 15, 0, 0, 0, DateTimeKind.Utc), 60.0, 50.600000000000001 },
                    { 45, 21.0, 97.0, 95.0, 24, 33.0, "Đăng ký gói VVIP Kim Cương 1 năm. Khởi đầu với quyết tâm rèn luyện sức khỏe bền vững.", new DateTime(2026, 1, 10, 10, 0, 0, 0, DateTimeKind.Utc), 82.0, 75.0 },
                    { 46, 18.199999999999999, 99.200000000000003, 94.5, 24, 34.299999999999997, "Giảm 3.5cm vòng eo, cơ ngực săn chắc, sức bền bứt phá trong các buổi tập functional.", new DateTime(2026, 5, 12, 10, 30, 0, 0, DateTimeKind.Utc), 78.5, 73.200000000000003 },
                    { 47, 16.199999999999999, 101.0, 94.0, 24, 35.200000000000003, "Tập luyện đều đặn suốt 9 tháng, chỉ số InBody tiệm cận chuẩn vận động viên.", new DateTime(2026, 9, 22, 11, 0, 0, 0, DateTimeKind.Utc), 76.5, 72.0 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "EndDate", "MemberId", "OriginalPrice", "PackageId", "PaidAmount", "StartDate", "Status" },
                values: new object[,]
                {
                    { 24, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), 21, 1300000m, 2, 1300000m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 25, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 21, 4500000m, 3, 4500000m, new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 26, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 22, 4500000m, 3, 4500000m, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 27, new DateTime(2027, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), 22, 8000000m, 4, 8000000m, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 28, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), 23, 500000m, 1, 500000m, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 29, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), 23, 1300000m, 2, 1300000m, new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 30, new DateTime(2027, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 23, 4500000m, 3, 4500000m, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 31, new DateTime(2027, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 24, 8000000m, 4, 8000000m, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Progress",
                keyColumn: "ProgressId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 1, 8, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
