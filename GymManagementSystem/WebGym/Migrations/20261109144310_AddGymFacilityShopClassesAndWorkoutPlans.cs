using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class AddGymFacilityShopClassesAndWorkoutPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    CheckInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.CheckInId);
                    table.ForeignKey(
                        name: "FK_CheckIns_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymRooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymRooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoutineDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NutritionAdvice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_GymRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "GymRooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GymClasses",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    CurrentCapacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymClasses", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_GymClasses_GymRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "GymRooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GymClasses_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassBookings",
                columns: table => new
                {
                    ClassBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassBookings", x => x.ClassBookingId);
                    table.ForeignKey(
                        name: "FK_ClassBookings_GymClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "GymClasses",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassBookings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CheckIns",
                columns: new[] { "CheckInId", "CheckInTime", "CheckOutTime", "MemberId", "Method", "Notes" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 1, 6, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, "Thẻ từ RFID", "Buổi tập ngực & tay sau sáng sớm." },
                    { 2, new DateTime(2026, 11, 3, 17, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 3, 19, 15, 0, 0, DateTimeKind.Unspecified), 1, "Mã QR Code", "Tập cùng HLV Trần Minh Nhật." },
                    { 3, new DateTime(2026, 11, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 19, 30, 0, 0, DateTimeKind.Unspecified), 1, "Nhận diện khuôn mặt", "Tập chân và bụng." },
                    { 4, new DateTime(2026, 11, 2, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 2, 8, 15, 0, 0, DateTimeKind.Unspecified), 2, "Mã QR Code", "Tham gia lớp Yoga buổi sáng." },
                    { 5, new DateTime(2026, 11, 4, 18, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 4, 19, 45, 0, 0, DateTimeKind.Unspecified), 2, "Mã QR Code", "Tập Cardio đạp xe Spinning." },
                    { 6, new DateTime(2026, 11, 2, 16, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, "Thẻ từ RFID", "Tập xô và lưng dưới." },
                    { 7, new DateTime(2026, 11, 5, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, "Thẻ từ RFID", "Tập vai và bắp tay trước." },
                    { 8, new DateTime(2026, 11, 3, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 3, 20, 30, 0, 0, DateTimeKind.Unspecified), 4, "Lễ tân check-in", "Quên thẻ thành viên, xác minh qua số điện thoại." },
                    { 9, new DateTime(2026, 11, 4, 6, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 4, 8, 0, 0, 0, DateTimeKind.Unspecified), 5, "Nhận diện khuôn mặt", "Tập toàn thân Full Body." },
                    { 10, new DateTime(2026, 11, 6, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 6, 19, 0, 0, 0, DateTimeKind.Unspecified), 21, "Mã QR Code", "Cardio máy chạy và xông hơi." },
                    { 11, new DateTime(2026, 11, 7, 17, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 7, 19, 0, 0, 0, DateTimeKind.Unspecified), 1, "Nhận diện khuôn mặt", "Tập ngực nâng cao." },
                    { 12, new DateTime(2026, 11, 8, 8, 0, 0, 0, DateTimeKind.Unspecified), null, 2, "Mã QR Code", "Đang tập luyện tại phòng Yoga." }
                });

            migrationBuilder.InsertData(
                table: "GymRooms",
                columns: new[] { "RoomId", "Capacity", "CreatedAt", "Description", "Location", "RoomName", "Status" },
                values: new object[,]
                {
                    { 1, 50, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Khu vực tập luyện chính gồm giàn tạ rời (Free Weights), rack gánh tạ và các máy tập khối chuyên dụng.", "Tầng 1 - Khu A", "Phòng Gym Trung Tâm (Khu Tạ & Kháng Lực)", "Hoạt động" },
                    { 2, 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Trang bị dàn máy chạy bộ thông minh, xe đạp địa hình spinning và máy trượt tuyết elliptical.", "Tầng 2 - Khu B", "Phòng Cardio & Đạp Xe Trong Nhà (Spinning Studio)", "Hoạt động" },
                    { 3, 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Không gian yên tĩnh, sàn gỗ thông tự nhiên, gương ốp tường toàn cảnh và hệ thống thảm tập cao cấp.", "Tầng 3 - Studio 1", "Phòng Tập Nhóm Yoga & Pilates Studio", "Hoạt động" },
                    { 4, 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Khu vực chuyên sâu boxing, kickfit với bao cát treo, thảm giảm chấn và găng đấm tiêu chuẩn thi đấu.", "Tầng 3 - Studio 2", "Phòng Huấn Luyện Boxing & Kickfit (Combat Zone)", "Hoạt động" },
                    { 5, 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Không gian riêng tư cao cấp dành riêng cho học viên tập 1-1 cùng Huấn luyện viên thể hình chuyên nghiệp.", "Tầng 2 - Phòng VIP", "Phòng Huấn Luyện Cá Nhân VIP (PT Private Studio)", "Hoạt động" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "CreatedAt", "IsRead", "Message", "TargetUrl", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "Máy chèo thuyền nước Concept2 (Khu cardio - Tầng 2) đang gặp sự cố kẹt xích kéo, đã tạo phiếu bảo trì.", "/Admin/Equipment", "Thông báo sự cố thiết bị cần xử lý", "Maintenance", 1 },
                    { 2, new DateTime(2026, 11, 6, 18, 25, 0, 0, DateTimeKind.Unspecified), true, "Hội viên Lê Hoàng Nam vừa đặt mua Đơn hàng #5 trị giá 2,470,000đ cần kiểm tra xuất kho.", "/Admin/Order", "Đơn hàng sản phẩm mới phát sinh", "System", 1 },
                    { 3, new DateTime(2026, 11, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), true, "Hội viên Nguyễn Quang Hải đã đặt lịch tập ngực & thể lực vào ngày mai lúc 18:00.", "/Trainer/Bookings", "Lịch đặt tập PT 1:1 mới", "Booking", 2 },
                    { 4, new DateTime(2026, 11, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), false, "Lớp BodyPump Thứ 4 (18:30) của bạn đã đạt tối đa 25/25 học viên đăng ký giữ chỗ.", "/Trainer/Classes", "Lớp học BodyPump đã đủ sĩ số tối đa", "Class", 2 },
                    { 5, new DateTime(2026, 10, 30, 20, 0, 0, 0, DateTimeKind.Unspecified), true, "Chỉ số mỡ cơ thể của bạn đã giảm xuống 18.5%, giảm 7.0% so với ngày bắt đầu. Hãy tiếp tục phát huy!", "/Member/Progress", "Chúc mừng! Bạn đã hoàn thành mốc InBody mới", "System", 5 },
                    { 6, new DateTime(2026, 11, 5, 8, 30, 0, 0, DateTimeKind.Unspecified), false, "HLV Trần Minh Nhật vừa cập nhật giáo án tập luyện 4 buổi/tuần và thực đơn tăng cơ nạc cho bạn.", "/Member/WorkoutPlan", "Kế hoạch tập luyện tuần mới sẵn sàng", "System", 5 },
                    { 7, new DateTime(2026, 11, 7, 20, 0, 0, 0, DateTimeKind.Unspecified), false, "Bạn có lịch tham gia lớp Hatha Yoga & Thiền Định lúc 06:30 sáng mai tại Studio 1.", "/Member/ClassBooking", "Nhắc nhở lịch học Yoga ngày mai", "Class", 6 },
                    { 8, new DateTime(2026, 11, 5, 15, 0, 0, 0, DateTimeKind.Unspecified), true, "Bạn đã đăng ký thành công lớp Kickfit lúc 19:30 Thứ 5. Vui lòng mang găng tay và khăn tập cá nhân.", "/Member/ClassBooking", "Đăng ký thành công lớp Kickfit Combat", "Class", 7 },
                    { 9, new DateTime(2026, 11, 3, 11, 0, 0, 0, DateTimeKind.Unspecified), false, "Đơn hàng phụ kiện Gym của bạn đã được xuất kho và đang trên đường giao tới địa chỉ đăng ký.", "/Member/Orders", "Đơn hàng #4 đang được vận chuyển", "System", 8 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "MemberId", "Notes", "OrderDate", "PaymentMethod", "PhoneNumber", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Đã thanh toán qua cổng VNPay thành công.", new DateTime(2026, 10, 15, 14, 30, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654321", "Nhận trực tiếp tại quầy Lễ tân Gym", "Hoàn thành", 2030000m, 5 },
                    { 2, 2, "Giao hàng giờ hành chính.", new DateTime(2026, 10, 20, 9, 15, 0, 0, DateTimeKind.Unspecified), "Chuyển khoản", "0987654322", "123 Hoàng Quốc Việt, Cầu Giấy, Hà Nội", "Hoàn thành", 1150000m, 6 },
                    { 3, 3, "Thanh toán tiền mặt cho thu ngân.", new DateTime(2026, 10, 28, 16, 45, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654323", "Nhận trực tiếp tại quầy Lễ tân Gym", "Hoàn thành", 1240000m, 7 },
                    { 4, 4, "Đóng gói bọc xốp cẩn thận.", new DateTime(2026, 11, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), "Momo", "0987654324", "Số 45 Trần Phú, Ba Đình, Hà Nội", "Đang giao", 950000m, 8 },
                    { 5, 5, "Hội viên đặt mua combo tăng cơ nhanh.", new DateTime(2026, 11, 6, 18, 20, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654325", "Số 88 Lê Duẩn, Hoàn Kiếm, Hà Nội", "Chờ xác nhận", 2470000m, 9 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "CategoryName", "Description", "IsActive" },
                values: new object[,]
                {
                    { 1, "Thực phẩm bổ sung & Whey Protein", "Sữa tăng cơ Whey Isolate, Hydrolyzed, Mass Gainer hỗ trợ phục hồi và phát triển cơ bắp tối ưu.", true },
                    { 2, "Tăng sức mạnh & Năng lượng (Pre-Workout & BCAA)", "Thức uống bùng nổ sức mạnh Pre-Workout, Creatine Monohydrate và điện giải BCAA chống mỏi cơ.", true },
                    { 3, "Dụng cụ & Phụ kiện tập Gym", "Găng tay, đai lưng nâng tạ bọc da, dây kéo lưng Lifting Straps, bình lắc Shaker và băng quấn cổ tay.", true },
                    { 4, "Trang phục thể thao Gym & Athleisure", "Áo thun co giãn thoáng khí, quần legging, áo Tanktop khoét nách và trang phục tập luyện cao cấp.", true },
                    { 5, "Nước giải khát thể thao & Bữa phụ dinh dưỡng", "Thanh Protein Bar ít đường, nước bù khoáng điện giải Isotonic và hạt sấy khô bổ sung năng lượng nhanh.", true }
                });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "CreatedAt", "EndDate", "Goal", "IsActive", "IsDeleted", "MemberId", "NutritionAdvice", "RoutineDetails", "StartDate", "Title", "TrainerId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Giảm từ 78.5kg xuống 70kg, giảm tỷ lệ mỡ cơ thể về dưới 18%, tăng khối lượng cơ bắp thân trên.", true, false, 1, "- Calo mục tiêu: 2.100 kcal/ngày (Deficit 300 kcal).\n- Đạm (Protein): 160g - 170g/ngày (~2.2g/kg thể trọng), ưu tiên ức gà, cá ngừ, trứng, Whey Isolate.\n- Tinh bột (Carb): 200g/ngày (khoai lang, yến mạch, cơm gạo lứt).\n- Chất béo tốt: 50g/ngày (dầu ô liu, quả bơ, hạt hạnh nhân).\n- Uống đủ 3 - 3.5 lít nước mỗi ngày, hạn chế tối đa bia rượu và đồ ngọt đóng chai.", "- Thứ 2 (Ngực & Tay sau): Bench Press 4x8, Incline Dumbbell Press 4x10, Cable Fly 3x12, Tricep Pushdown 4x12.\n- Thứ 3 (Lưng xô & Tay trước): Deadlift 4x6, Lat Pulldown 4x10, Barbell Row 4x8, Bicep Curls 3x12.\n- Thứ 4: Nghỉ ngơi tích cực hoặc Cardio nhẹ 30 phút đi bộ dốc.\n- Thứ 5 (Chân & Bắp chân): Barbell Squat 4x8, Leg Press 4x12, Leg Curl 3x15, Calf Raise 4x20.\n- Thứ 6 (Vai & Bụng): Overhead Press 4x8, Lateral Raise 4x15, Face Pull 3x15, Hanging Leg Raise 4x15.\n- Thứ 7: HIIT Cardio chạy bộ máy Technogym 25 phút.\n- Chủ nhật: Nghỉ phục hồi toàn diện.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kế Hoạch Tăng Cơ Giảm Mỡ Cấp Tốc 12 Tuần (Hypertrophy & Shredding)", 1, null },
                    { 2, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Giảm mỡ vòng eo từ 68cm xuống 62cm, cải thiện đường cong hông đùi và độ dẻo dai cơ thể.", true, false, 2, "- Calo mục tiêu: 1.450 kcal/ngày.\n- Ưu tiên rau xanh nhiều chất xơ, cá hồi, tôm, sữa chua Hy Lạp và sinh tố hoa quả mọng (berries).\n- Hạn chế gia vị nêm mặn nhằm tránh tích nước dưới da vùng bụng.\n- Uống trà hoa cúc hoặc trà xanh ấm buổi tối.", "- Thứ 2: Tập mông đùi Glute Focus: Hip Thrust 4x12, Romanian Deadlift 4x10, Bulgarian Split Squat 3x12 mỗi bên.\n- Thứ 3: Lớp Hatha Yoga phục hồi & giãn cơ chuyên sâu 75 phút.\n- Thứ 4: Thân trên nữ & Lưng xô: Lat Pulldown 3x12, Dumbbell Shoulder Press 3x12, Plank 4x45s.\n- Thứ 5: Nghỉ ngơi hoặc đạp xe Spinning kháng lực vừa phải 40 phút.\n- Thứ 6: Pilates thảm Matwork & Core Cốt Lõi: Động tác The Hundred, Criss Cross, Teaser.\n- Thứ 7 & CN: Đi dạo ngoài trời, thiền định.", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Giáo Án Siết Eo Săn Chắc Vóc Dáng & Yoga Nữ (Tone & Flexibility)", 3, null },
                    { 3, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Tăng sức mạnh Big 3 (Squat, Bench, Deadlift) thêm 20%, tăng 3kg khối lượng cơ nạc sạch.", true, false, 3, "- Calo mục tiêu: 2.800 - 3.000 kcal/ngày (Surplus calo tăng cơ).\n- Bổ sung 5g Creatine Monohydrate mỗi ngày vào buổi sáng hoặc ngay sau tập.\n- Chia nhỏ 5 bữa ăn/ngày, giàu đạm thịt bò, ức gà, cơm trắng và sữa tươi thanh trùng.", "- Thứ 2: Heavy Squat 5x5 (80% 1RM), Pause Squat 3x5, Bulgarian Split Squat 3x8.\n- Thứ 4: Heavy Bench Press 5x5, Close-Grip Bench 3x8, Incline Dumbbell Bench 3x10.\n- Thứ 6: Conventional Deadlift 5x3, Barbell Row 4x6, Farmer Walk 3 hiệp nặng.\n- Thứ 7: Phụ trợ vai tay, bài tập bụng chống xoay vòng Paloff Press.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chương Trình Sức Mạnh Nền Tảng Powerlifting & Khối Lượng Cơ Lớn", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentId", "Area", "Brand", "EquipmentName", "ImageUrl", "LastMaintenanceDate", "Notes", "PurchaseDate", "RoomId", "SerialNumber", "Status" },
                values: new object[,]
                {
                    { 1, "Khu cardio", "Technogym", "Máy chạy bộ Technogym Skillrun Unity 7000", "~/images/equipment/treadmill-technogym.jpg", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Màn hình cảm ứng 19 inch, tích hợp bài tập leo dốc và chạy sức cản.", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "TG-SR7000-01", "Hoạt động tốt" },
                    { 2, "Khu cardio", "Life Fitness", "Máy chạy bộ Life Fitness Elevation Series Discover SE3", "~/images/equipment/treadmill-lifefitness.jpg", new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Băng chuyền phát tiếng kêu nhỏ khi nâng độ dốc trên 8%.", new DateTime(2024, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "LF-SE3-02", "Cần bảo trì" },
                    { 3, "Khu cardio", "Matrix Fitness", "Xe đạp tập địa hình Matrix Fitness CXP Target Training", "~/images/equipment/spinning-bike.jpg", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Xe đạp bánh đà kháng lực từ tính êm ái, đo công suất watt.", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "MX-CXP-03", "Hoạt động tốt" },
                    { 4, "Khu cardio", "Concept2", "Máy chèo thuyền nước Concept2 Model D Rower", "~/images/equipment/rower-concept2.jpg", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dây kéo xích bị kẹt puly trục cuốn, đang chờ linh kiện thay thế.", new DateTime(2024, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "C2-ROW-04", "Đang hỏng" },
                    { 5, "Khu tạ tự do", "Impulse Fitness", "Khung gánh tạ trợ lực 3D Smith Machine", "~/images/equipment/smith-machine.jpg", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trục dẫn hướng chuyển động mượt mà, chốt khóa an toàn đa điểm.", new DateTime(2024, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "IMP-SM3D-05", "Hoạt động tốt" },
                    { 6, "Khu tạ tự do", "Rogue Fitness", "Giàn tập Power Rack & Tạ đòn Olympic Rogue Monster Lite", "~/images/equipment/power-rack.jpg", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thép chịu lực tải trọng 500kg, kèm thanh xà đơn và chốt an toàn J-Cups.", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ROG-PR-06", "Hoạt động tốt" },
                    { 7, "Khu tạ tự do", "Rogue Fitness", "Bộ giàn tạ tay Dumbbell Rogue Urethane 2.5kg - 50kg", "~/images/equipment/dumbbells-set.jpg", new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bọc Urethane chống ồn, tay cầm mạ chrome vân kim cương chống trượt.", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "ROG-DB-07", "Hoạt động tốt" },
                    { 8, "Khu máy khối", "Technogym", "Máy ép ngực & Kéo vai sau Pectoral Fly / Rear Deltoid", "~/images/equipment/pec-fly.jpg", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hệ thống tạ tháp Selection Pro, chuyển đổi linh hoạt giữa ép ngực và kéo vai.", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "TG-SEL-08", "Hoạt động tốt" },
                    { 9, "Khu máy khối", "Life Fitness", "Máy đạp đùi nghiêng 45 độ Linear Leg Press", "~/images/equipment/leg-press.jpg", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đệm lưng bị trầy rách góc dưới, ray trượt cần tra thêm mỡ bôi trơn công nghiệp.", new DateTime(2024, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "LF-LP45-09", "Cần bảo trì" },
                    { 10, "Khu chức năng", "Matrix Fitness", "Giàn cáp kéo xô đôi đa năng Dual Adjustable Pulley", "~/images/equipment/dual-pulley.jpg", new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hai cột tạ độc lập, ròng rọc xoay 360 độ điều chỉnh độ cao nhiều mức.", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "MX-DAP-10", "Hoạt động tốt" }
                });

            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "ClassId", "ClassName", "CurrentCapacity", "Description", "EndDate", "ImageUrl", "MaxCapacity", "RoomId", "Schedule", "StartDate", "Status", "TrainerId" },
                values: new object[,]
                {
                    { 1, "Hatha Yoga & Thiền Định Phục Hồi Năng Lượng", 15, "Lớp học giúp cân bằng thân - tâm - trí, kéo giãn cơ bắp, giải tỏa căng thẳng và cải thiện hệ hô hấp cho người bận rộn.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "~/images/classes/class-yoga.jpg", 20, 3, "Thứ 2 - 4 - 6 (06:30 - 07:45)", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Open", 3 },
                    { 2, "Spinning RPM - Đua Xe Đạp Trong Nhà Đốt Mỡ Cấp Tốc", 20, "Âm nhạc bùng nổ kết hợp các bài leo dốc, chạy nước rút cường độ cao giúp tiêu hao 600 - 800 kcal mỗi buổi tập.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "~/images/classes/class-spinning.jpg", 25, 2, "Thứ 3 - 5 - 7 (18:00 - 19:15)", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Open", 2 },
                    { 3, "BodyPump - Rèn Luyện Sức Bền Cơ Bắp Toàn Thân", 25, "Tập luyện với thanh đòn tạ theo giai điệu bài hát sôi động, tăng mật độ xương và săn chắc toàn bộ nhóm cơ.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "~/images/classes/class-bodypump.jpg", 25, 1, "Thứ 2 - 4 - 6 (18:30 - 19:45)", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Full", 1 },
                    { 4, "Kickfit Tự Vệ & Cardio Boxing (Combat Zone)", 10, "Sự kết hợp hoàn hảo giữa đấm bốc boxing, cùi chỏ muay thai và bài tập thể lực rèn luyện phản xạ nhanh nhẹn.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "~/images/classes/class-kickfit.jpg", 15, 4, "Thứ 3 - 5 - CN (19:30 - 20:45)", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Open", 2 },
                    { 5, "Pilates Trị Liệu Tư Thế & Giảm Đau Cột Sống", 12, "Phương pháp căn chỉnh khung xương chậu, cải thiện gù lưng cổ rùa và làm săn chắc cơ bụng cốt lõi Core.", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "~/images/classes/class-pilates.jpg", 12, 3, "Thứ 7 - Chủ Nhật (08:30 - 10:00)", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Full", 3 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Sữa tăng cơ huyền thoại thế giới với 24g Protein mỗi lần dùng, 5.5g BCAA tự nhiên, ít đường và chất béo.", "~/images/products/whey-gold-standard.jpg", true, 1750000m, "Whey Gold Standard 100% Protein (Optimum Nutrition - 5 Lbs)", 45 },
                    { 2, 1, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "100% Whey Isolate và Hydrolyzed siêu tinh khiết, 0 đường, 0 fat, hấp thu cực nhanh sau khi tập nặng.", "~/images/products/rule1-isolate.jpg", true, 1890000m, "Rule 1 Protein Isolate & Hydrolyzed (Rule One - 5 Lbs)", 38 },
                    { 3, 2, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bổ sung 5g Creatine siêu mịn Micronized mỗi muỗng, tăng thể tích tế bào cơ, bộc phát sức mạnh tối đa.", "~/images/products/muscletech-creatine.jpg", true, 580000m, "Creatine Monohydrate Platinum 400g (MuscleTech)", 60 },
                    { 4, 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Tăng tỉnh táo tập trung và sung mãn năng lượng với Beta-Alanine, Creatine Nitrate và 150mg Caffeine.", "~/images/products/c4-preworkout.jpg", true, 790000m, "C4 Original Pre-Workout Năng Lượng Đỉnh Cao (Cellucor - 60 Servings)", 25 },
                    { 5, 2, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "7g BCAA tỉ lệ vàng 2:1:1 kết hợp điện giải dừa tươi, chống dị hóa cơ và giảm đau nhức sau tập.", "~/images/products/xtend-bcaa.jpg", true, 1150000m, "BCAA Xtend Bù Khoáng Điện Giải (Scivation - 90 Servings)", 30 },
                    { 6, 3, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đai lưng da thật dày 10mm với khóa đòn bẩy thép nguyên khối, bảo vệ cột sống thắt lưng khi gánh tạ nặng.", "~/images/products/lever-belt.jpg", true, 850000m, "Đai Lưng Tập Gym Da Bò Thật Khóa Đòn Bẩy (Lever Belt 10mm)", 20 },
                    { 7, 3, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Lớp đệm lòng bàn tay chống chai tay, đai quấn cổ tay dài 45cm bảo vệ khớp khi đẩy ngực nặng.", "~/images/products/harbinger-gloves.jpg", true, 390000m, "Găng Tay Tập Gym Thoáng Khí Kèm Quấn Cổ Tay (Harbinger Pro)", 50 },
                    { 8, 3, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Chất liệu nhựa Tritan kháng khuẩn không mùi BPA Free, kèm quả cầu xoắn inox đánh tan bột whey siêu mịn.", "~/images/products/blender-bottle.jpg", true, 280000m, "Bình Lắc Shaker BlenderBottle Pro45 (Dung tích 1300ml)", 75 },
                    { 9, 4, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Vải Spandex pha Polyester thể thao thấm hút mồ hôi nhanh, tôn dáng cơ bắp và chống nhăn co rút.", "~/images/products/gymtech-shirt.jpg", true, 260000m, "Áo Thun Tập Gym Nam Co Giãn 4 Chiều GymTech Pro", 40 },
                    { 10, 5, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "21g Protein chất lượng cao mỗi thanh, chỉ 1g đường và dồi dào chất xơ prebiotic, bữa phụ hoàn hảo lành mạnh.", "~/images/products/quest-bar.jpg", true, 690000m, "Hộp Bánh Protein Bar Quest Nutrition (Hộp 12 Thanh x 60g)", 35 }
                });

            migrationBuilder.InsertData(
                table: "ClassBookings",
                columns: new[] { "ClassBookingId", "BookingDate", "ClassId", "MemberId", "Notes", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 2, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Hội viên tham gia đầy đủ buổi tập Yoga sáng.", "Attended" },
                    { 2, new DateTime(2026, 11, 2, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, "Mượn thảm tập cao su non của phòng.", "Attended" },
                    { 3, new DateTime(2026, 11, 3, 17, 30, 0, 0, DateTimeKind.Unspecified), 2, 1, "Đạt mục tiêu đốt 650 kcal.", "Attended" },
                    { 4, new DateTime(2026, 11, 3, 17, 30, 0, 0, DateTimeKind.Unspecified), 2, 3, "Báo bận lịch công tác đột xuất lúc 15:00.", "Cancelled" },
                    { 5, new DateTime(2026, 11, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Mức tạ gánh 25kg mỗi bên.", "Attended" },
                    { 6, new DateTime(2026, 11, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, 5, "Tập rất sung sức.", "Attended" },
                    { 7, new DateTime(2026, 11, 5, 19, 0, 0, 0, DateTimeKind.Unspecified), 4, 3, "Đã xác nhận giữ chỗ lớp đấm bốc.", "Confirmed" },
                    { 8, new DateTime(2026, 11, 7, 8, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Học viên yêu cầu điều chỉnh tư thế thắt lưng.", "Confirmed" },
                    { 9, new DateTime(2026, 11, 7, 8, 0, 0, 0, DateTimeKind.Unspecified), 5, 4, "Đã xếp máy Reformer số 3.", "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceLogs",
                columns: new[] { "LogId", "Cost", "Description", "EquipmentId", "MaintenanceDate", "Notes", "PerformedBy", "Status" },
                values: new object[,]
                {
                    { 1, 850000m, "Bảo dưỡng định kỳ 6 tháng: kiểm tra motor 4.0 HP, vệ sinh bo mạch và tra dầu silicon chuyên dụng băng tải.", 1, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thiết bị vận hành êm ái, đạt chuẩn thông số kỹ thuật xuất xưởng.", "Technogym Vietnam Services", "Hoàn thành" },
                    { 2, 600000m, "Kiểm tra độ chùng thảm chạy, cân chỉnh lực căng 2 trục con lăn và vệ sinh cảm biến nhịp tim cầm tay.", 2, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khuyến nghị thay băng tải trong đợt bảo dưỡng tới do mài mòn lớp cao su.", "Kỹ thuật viên phòng máy (Nguyễn Văn Bình)", "Hoàn thành" },
                    { 3, 1450000m, "Sự cố kẹt xích kéo máy chèo thuyền Concept2: tháo dỡ cụm bánh đà quạt gió, kiểm tra lò xo hồi xích.", 4, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã đặt mua lò xo cuộn và xích mạ niken chính hãng từ Mỹ, dự kiến hoàn tất trong 5 ngày.", "Trung tâm sửa chữa thiết bị thể thao Việt Á", "Đang bảo dưỡng" },
                    { 4, 350000m, "Bôi trơn 2 thanh ray trục Smith bằng mỡ trắng lithium, kiểm tra các chốt hãm lò xo an toàn chống rơi.", 5, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Không có hư hỏng cơ khí. Cân bằng trọng lực đòn tạ chuẩn.", "Bảo trì nội bộ Gym", "Hoàn thành" },
                    { 5, 750000m, "Thay thế dây cáp bọc nhựa đường kính 5mm của tháp tạ ép ngực do bị xước lớp vỏ bảo vệ bên ngoài.", 8, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dây cáp mới chịu tải 1500kg, đã test tải trọng tối đa 100kg an toàn.", "Kỹ thuật viên Lê Quốc Dũng", "Hoàn thành" },
                    { 6, 1200000m, "Bọc lại lớp da simili cao cấp cho đệm tựa lưng máy đạp đùi và thay thế 2 bạc đạn trượt linear bearing.", 9, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mặt đệm êm, đường may đôi chắc chắn, khả năng chịu mồ hôi tốt.", "Xưởng cơ khí & da thể thao Thành Phát", "Hoàn thành" },
                    { 7, 400000m, "Cân chỉnh rãnh xoay ròng rọc 360 độ, kiểm tra chốt chọn tạ từ tính và siết ốc cố định chân đế sàn bê tông.", 10, new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tất cả các nấc nạp cáp trượt trơn tru.", "Bảo trì nội bộ Gym", "Hoàn thành" }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 1750000m, 1750000m },
                    { 2, 1, 8, 1, 280000m, 280000m },
                    { 3, 2, 5, 1, 1150000m, 1150000m },
                    { 4, 3, 6, 1, 850000m, 850000m },
                    { 5, 3, 7, 1, 390000m, 390000m },
                    { 6, 4, 9, 1, 260000m, 260000m },
                    { 7, 4, 10, 1, 690000m, 690000m },
                    { 8, 5, 2, 1, 1890000m, 1890000m },
                    { 9, 5, 3, 1, 580000m, 580000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberId",
                table: "CheckIns",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBookings_ClassId",
                table: "ClassBookings",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassBookings_MemberId",
                table: "ClassBookings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_RoomId",
                table: "Equipments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GymClasses_RoomId",
                table: "GymClasses",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_GymClasses_TrainerId",
                table: "GymClasses",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_EquipmentId",
                table: "MaintenanceLogs",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MemberId",
                table: "Orders",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_MemberId",
                table: "WorkoutPlans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_TrainerId",
                table: "WorkoutPlans",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "ClassBookings");

            migrationBuilder.DropTable(
                name: "MaintenanceLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "GymClasses");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "GymRooms");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
