using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckInMethodToQRCodeOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 1,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR tại cổng kiểm soát. Buổi tập ngực sáng sớm." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 2,
                column: "Notes",
                value: "Quét mã QR từ ứng dụng WebGym. Tập cùng HLV Trần Minh Nhật.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 3,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR tại cổng quay tự động. Tập chân và bụng." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 4,
                column: "Notes",
                value: "Quét mã QR tham gia lớp Yoga sáng.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 5,
                column: "Notes",
                value: "Quét mã QR vào phòng Cardio đạp xe Spinning.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 6,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR check-in cổng số 1. Tập xô và lưng dưới." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 7,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR từ điện thoại cá nhân. Tập vai và bắp tay trước." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 8,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR thành viên hợp lệ trên hệ thống." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 9,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR check-in ca sáng. Tập toàn thân Full Body." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 10,
                column: "Notes",
                value: "Quét mã QR cổng vào. Cardio máy chạy và xông hơi.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 11,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Mã QR Code", "Quét mã QR cổng turnstile. Tập ngực nâng cao." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 12,
                column: "Notes",
                value: "Quét mã QR vào phòng Yoga, hiện đang trong buổi tập.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 1,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Thẻ từ RFID", "Buổi tập ngực & tay sau sáng sớm." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 2,
                column: "Notes",
                value: "Tập cùng HLV Trần Minh Nhật.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 3,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Nhận diện khuôn mặt", "Tập chân và bụng." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 4,
                column: "Notes",
                value: "Tham gia lớp Yoga buổi sáng.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 5,
                column: "Notes",
                value: "Tập Cardio đạp xe Spinning.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 6,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Thẻ từ RFID", "Tập xô và lưng dưới." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 7,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Thẻ từ RFID", "Tập vai và bắp tay trước." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 8,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Lễ tân check-in", "Quên thẻ thành viên, xác minh qua số điện thoại." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 9,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Nhận diện khuôn mặt", "Tập toàn thân Full Body." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 10,
                column: "Notes",
                value: "Cardio máy chạy và xông hơi.");

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 11,
                columns: new[] { "Method", "Notes" },
                values: new object[] { "Nhận diện khuôn mặt", "Tập ngực nâng cao." });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInId",
                keyValue: 12,
                column: "Notes",
                value: "Đang tập luyện tại phòng Yoga.");
        }
    }
}
