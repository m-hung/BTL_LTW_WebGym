using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchemaAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymPackages",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    IncludesPT = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymPackages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    BMR = table.Column<double>(type: "float", nullable: true),
                    BodyFatPercentage = table.Column<double>(type: "float", nullable: true),
                    MuscleMass = table.Column<double>(type: "float", nullable: true),
                    Chest = table.Column<double>(type: "float", nullable: true),
                    Waist = table.Column<double>(type: "float", nullable: true),
                    Hips = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_GymPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "GymPackages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    RecordedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    BodyFatPercentage = table.Column<double>(type: "float", nullable: true),
                    MuscleMass = table.Column<double>(type: "float", nullable: true),
                    Chest = table.Column<double>(type: "float", nullable: true),
                    Waist = table.Column<double>(type: "float", nullable: true),
                    Hips = table.Column<double>(type: "float", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_Progress_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_GymPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "GymPackages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GymPackages",
                columns: new[] { "PackageId", "Description", "DurationDays", "IncludesPT", "PackageName", "Price" },
                values: new object[,]
                {
                    { 1, "Gói tập cơ bản 30 ngày, sử dụng toàn bộ trang thiết bị Gym hiện đại, tủ locker và phòng tắm nước nóng.", 30, false, "Gói Đồng (Basic 1 Tháng)", 500000m },
                    { 2, "Gói tập 90 ngày tiết kiệm, không giới hạn khung giờ tập, miễn phí đo chỉ số cơ thể định kỳ (InBody).", 90, false, "Gói Bạc (Standard 3 Tháng)", 1300000m },
                    { 3, "Gói tập 180 ngày cao cấp, tập kèm PT không giới hạn, sử dụng phòng xông hơi Sauna & bể bơi nước ấm.", 180, true, "Gói Vàng (VIP 6 Tháng kèm PT)", 4500000m },
                    { 4, "Gói tập trọn gói 365 ngày đặc quyền tối cao, tập kèm PT không giới hạn, tư vấn dinh dưỡng AI và khăn nước riêng.", 365, true, "Gói Kim Cương (VVIP 1 Năm kèm PT)", 8000000m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gym.com", "System Administrator", "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", "0901234567", "Admin", "admin" },
                    { 2, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "minhnhat.pt@gym.com", "Trần Minh Nhật", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345678", "PT", "pt_minhnhat" },
                    { 3, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), "hoanganh.pt@gym.com", "Lê Hoàng Anh", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345679", "PT", "pt_hoanganh" },
                    { 4, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), "thuytien.pt@gym.com", "Nguyễn Thụy Tiên", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345680", "PT", "pt_thuytien" },
                    { 5, new DateTime(2026, 8, 1, 8, 0, 0, 0, DateTimeKind.Utc), "quanghai@gmail.com", "Nguyễn Quang Hải", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654321", "Member", "member_quanghai" },
                    { 6, new DateTime(2026, 6, 15, 9, 30, 0, 0, DateTimeKind.Utc), "thanhhuong@gmail.com", "Đặng Thanh Hương", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654322", "Member", "member_thanhhuong" },
                    { 7, new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc), "ducthang@gmail.com", "Vũ Đức Thắng", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654323", "Member", "member_ducthang" },
                    { 8, new DateTime(2026, 9, 5, 10, 15, 0, 0, DateTimeKind.Utc), "phuonganh@gmail.com", "Bùi Phương Anh", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654324", "Member", "member_phuonganh" },
                    { 9, new DateTime(2026, 7, 20, 16, 45, 0, 0, DateTimeKind.Utc), "hoangnam@gmail.com", "Phạm Hoàng Nam", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654325", "Member", "member_hoangnam" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "BMR", "BodyFatPercentage", "Chest", "DateOfBirth", "Gender", "Height", "Hips", "MuscleMass", "UserId", "Waist", "Weight" },
                values: new object[,]
                {
                    { 1, 1720.0, 25.5, 98.0, new DateTime(1997, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 170.0, 96.0, 32.0, 5, 88.0, 78.5 },
                    { 2, 1350.0, 24.0, 84.0, new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 162.0, 91.0, 22.5, 6, 68.0, 56.0 },
                    { 3, 1850.0, 26.0, 102.0, new DateTime(1995, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 176.0, 100.0, 34.5, 7, 92.0, 84.0 },
                    { 4, 1280.0, 21.5, 82.0, new DateTime(1999, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 158.0, 88.0, 20.0, 8, 64.0, 49.0 },
                    { 5, 1690.0, 19.0, 96.0, new DateTime(1992, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 174.0, 94.0, 33.0, 9, 80.0, 73.0 }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "AvatarUrl", "Bio", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 1, "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=300", "HLV 6 năm kinh nghiệm, chứng chỉ quốc tế NASM, từng huấn luyện nhiều vận động viên thể hình.", "Tăng cơ, Giảm mỡ nâng cao & Thể hình chuyên nghiệp (Bodybuilding)", 2 },
                    { 2, "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300", "HLV 4 năm kinh nghiệm, chuyên về giảm cân cấp tốc, rèn luyện tim mạch và cải thiện chuyển hóa.", "Cardio cường độ cao (HIIT), Thể lực & Sức bền (Functional Training)", 3 },
                    { 3, "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=300", "Chuyên gia phục hồi tư thế, tăng độ dẻo dai và giảm đau mỏi lưng cổ vai gáy cho dân văn phòng.", "Pilates, Yoga & Điều chỉnh tư thế cột sống (Rehabilitation)", 4 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "MemberId", "Notes", "Status", "TimeSlot", "TrainerId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tập ngực & tay sau (Chest & Triceps focus)", "Completed", "08:00 - 09:30", 1 },
                    { 2, new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Cardio HIIT đốt mỡ & giãn cơ phục hồi", "Completed", "17:30 - 19:00", 1 },
                    { 3, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Chân mông đùi (Leg day - Squat form check)", "Completed", "09:00 - 10:30", 1 },
                    { 4, new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Lưng xô & tay trước (Back & Biceps)", "Completed", "08:00 - 09:30", 1 },
                    { 5, new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Hội viên có việc bận đột xuất xin hoãn ca", "Cancelled", "18:00 - 19:30", 1 },
                    { 6, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Đo lại chỉ số đầu tháng & tập bài đa khớp compound", "Approved", "08:00 - 09:30", 1 },
                    { 7, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tập mông đùi Glutes hypertrophy", "Approved", "17:30 - 19:00", 1 },
                    { 8, new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Hội viên mới đăng ký buổi đầu thẩm định thể lực", "Pending", "15:00 - 16:30", 1 },
                    { 9, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tập vai & cơ bụng core strength", "Pending", "10:00 - 11:30", 1 },
                    { 10, new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Đăng ký buổi tập bù ca tuần trước", "Pending", "18:00 - 19:30", 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "MemberId", "PackageId", "PaymentDate", "PaymentMethod", "Status", "TransactionCode" },
                values: new object[,]
                {
                    { 1, 1300000m, 3, 2, new DateTime(2026, 5, 10, 8, 45, 12, 0, DateTimeKind.Utc), "Momo", "Success", "MOM20260510084512" },
                    { 2, 8000000m, 2, 4, new DateTime(2026, 6, 15, 14, 30, 11, 0, DateTimeKind.Utc), "BankTransfer", "Success", "MBB20260615143011" },
                    { 3, 500000m, 5, 1, new DateTime(2026, 7, 20, 17, 30, 0, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260720173000" },
                    { 4, 4500000m, 1, 3, new DateTime(2026, 8, 1, 9, 15, 23, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260801091523" },
                    { 5, 4500000m, 3, 3, new DateTime(2026, 8, 10, 10, 20, 45, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20260810102045" },
                    { 6, 8000000m, 5, 4, new DateTime(2026, 9, 1, 9, 11, 22, 0, DateTimeKind.Utc), "BankTransfer", "Success", "TCB20260901091122" },
                    { 7, 500000m, 4, 1, new DateTime(2026, 9, 5, 16, 12, 9, 0, DateTimeKind.Utc), "Momo", "Success", "MOM20260905161209" },
                    { 8, 1300000m, 4, 2, new DateTime(2026, 10, 6, 11, 45, 30, 0, DateTimeKind.Utc), "BankTransfer", "Success", "VCB20261006114530" },
                    { 9, 500000m, 1, 1, new DateTime(2026, 10, 25, 19, 0, 11, 0, DateTimeKind.Utc), "Momo", "Success", "MOM20261025190011" },
                    { 10, 1300000m, 2, 2, new DateTime(2026, 11, 1, 8, 30, 15, 0, DateTimeKind.Utc), "VNPay", "Success", "VNP20261101083015" },
                    { 11, 4500000m, 3, 3, new DateTime(2026, 10, 15, 11, 22, 33, 0, DateTimeKind.Utc), "Momo", "Failed", "MOM20261015112233" }
                });

            migrationBuilder.InsertData(
                table: "Progress",
                columns: new[] { "ProgressId", "BodyFatPercentage", "Chest", "Hips", "MemberId", "MuscleMass", "Notes", "RecordedDate", "Waist", "Weight" },
                values: new object[,]
                {
                    { 1, 25.5, 98.0, 96.0, 1, 32.0, "Đo InBody ngày đầu nhập môn. Thể trạng nhiều mỡ nội tạng, cơ địa dễ tích mỡ bụng.", new DateTime(2026, 8, 1, 8, 30, 0, 0, DateTimeKind.Utc), 88.0, 78.5 },
                    { 2, 24.199999999999999, 98.5, 95.0, 1, 32.399999999999999, "Thích nghi tốt với bài tập kháng lực và cardio HIIT. Bắt đầu quen với thực đơn thâm hụt calo.", new DateTime(2026, 8, 20, 9, 0, 0, 0, DateTimeKind.Utc), 85.5, 76.799999999999997 },
                    { 3, 22.800000000000001, 99.0, 94.0, 1, 32.899999999999999, "Vòng eo giảm rõ rệt 5cm, cơ ngực săn chắc, sức bền tim mạch tăng đáng kể.", new DateTime(2026, 9, 10, 8, 45, 0, 0, DateTimeKind.Utc), 83.0, 74.900000000000006 },
                    { 4, 21.199999999999999, 99.5, 93.5, 1, 33.5, "Tăng cơ nạc và siết mỡ liên tục. Mức tạ Squat và Bench Press tăng thêm 15kg.", new DateTime(2026, 9, 30, 9, 15, 0, 0, DateTimeKind.Utc), 81.0, 73.200000000000003 },
                    { 5, 19.800000000000001, 100.0, 93.0, 1, 34.0, "Tỷ lệ mỡ giảm dưới mốc 20%, cơ bụng lộ 4 múi rõ nét, phong độ tập luyện rất cao.", new DateTime(2026, 10, 18, 8, 30, 0, 0, DateTimeKind.Utc), 79.0, 71.599999999999994 },
                    { 6, 18.5, 101.0, 92.5, 1, 34.600000000000001, "Cột mốc 3 tháng: Giảm 8.3kg mỡ, tăng 2.6kg cơ, vòng eo giảm 10.5cm. Chỉ số xuất sắc!", new DateTime(2026, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), 77.5, 70.200000000000003 }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "EndDate", "MemberId", "OriginalPrice", "PackageId", "PaidAmount", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, 1300000m, 2, 1300000m, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 2, new DateTime(2027, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, 8000000m, 4, 8000000m, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 3, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), 5, 500000m, 1, 500000m, new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 4, new DateTime(2027, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4500000m, 3, 4500000m, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 5, new DateTime(2027, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, 4500000m, 3, 4500000m, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 6, new DateTime(2027, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, 8000000m, 4, 8000000m, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Active" },
                    { 7, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, 500000m, 1, 500000m, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Expired" },
                    { 8, new DateTime(2027, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1300000m, 2, 1300000m, new DateTime(2026, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Active" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberId",
                table: "Bookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrainerId",
                table: "Bookings",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MemberId",
                table: "Payments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PackageId",
                table: "Payments",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Progress_MemberId",
                table: "Progress",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MemberId",
                table: "Subscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PackageId",
                table: "Subscriptions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                table: "Trainers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Progress");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "GymPackages");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
