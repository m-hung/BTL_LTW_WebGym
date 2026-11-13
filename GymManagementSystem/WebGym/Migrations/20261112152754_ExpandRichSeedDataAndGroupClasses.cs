using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class ExpandRichSeedDataAndGroupClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('dbo.Members', 'AvatarUrl') IS NULL
    ALTER TABLE [dbo].[Members] ADD [AvatarUrl] nvarchar(500) NULL;
IF COL_LENGTH('dbo.Members', 'CardCode') IS NULL
    ALTER TABLE [dbo].[Members] ADD [CardCode] nvarchar(50) NULL;
IF COL_LENGTH('dbo.Members', 'QrCode') IS NULL
    ALTER TABLE [dbo].[Members] ADD [QrCode] nvarchar(500) NULL;
");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 10, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), "elena.pt@gym.com", "Elena Rostova", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345681", "PT", "pt_elena" },
                    { 11, new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), "marcus.pt@gym.com", "Marcus Vance", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345682", "PT", "pt_marcus" },
                    { 12, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "liam.pt@gym.com", "Liam O'Connor", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345683", "PT", "pt_liam" },
                    { 13, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), "devon.pt@gym.com", "Devon Brooks", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345684", "PT", "pt_devon" },
                    { 14, new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), "chloe.pt@gym.com", "Chloe Bennett", "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=", "0912345685", "PT", "pt_chloe" },
                    { 25, new DateTime(2026, 2, 12, 8, 0, 0, 0, DateTimeKind.Utc), "minhan@gmail.com", "Trịnh Minh An", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654330", "Member", "member_minhan" },
                    { 26, new DateTime(2026, 2, 18, 9, 0, 0, 0, DateTimeKind.Utc), "khanhlinh@gmail.com", "Hoàng Khánh Linh", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654331", "Member", "member_khanhlinh" },
                    { 27, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), "tuananh@gmail.com", "Đào Tuấn Anh", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654332", "Member", "member_tuananh" },
                    { 28, new DateTime(2026, 3, 14, 11, 0, 0, 0, DateTimeKind.Utc), "ngocdiep@gmail.com", "Phan Ngọc Diệp", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654333", "Member", "member_ngocdiep" },
                    { 29, new DateTime(2026, 4, 2, 14, 0, 0, 0, DateTimeKind.Utc), "quocbao@gmail.com", "Võ Quốc Bảo", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654334", "Member", "member_quocbao" },
                    { 30, new DateTime(2026, 4, 9, 15, 0, 0, 0, DateTimeKind.Utc), "thuyduong@gmail.com", "Lý Thùy Dương", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654335", "Member", "member_thuyduong" },
                    { 31, new DateTime(2026, 5, 11, 8, 30, 0, 0, DateTimeKind.Utc), "giabao@gmail.com", "Bùi Gia Bảo", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654336", "Member", "member_giabao" },
                    { 32, new DateTime(2026, 5, 19, 9, 30, 0, 0, DateTimeKind.Utc), "hatrang@gmail.com", "Ngô Hà Trang", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654337", "Member", "member_hatrang" },
                    { 33, new DateTime(2026, 6, 4, 10, 30, 0, 0, DateTimeKind.Utc), "trongnghia@gmail.com", "Dương Trọng Nghĩa", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654338", "Member", "member_trongnghia" },
                    { 34, new DateTime(2026, 6, 17, 13, 0, 0, 0, DateTimeKind.Utc), "myduyen@gmail.com", "Tạ Mỹ Duyên", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654339", "Member", "member_myduyen" },
                    { 35, new DateTime(2026, 7, 3, 14, 0, 0, 0, DateTimeKind.Utc), "anhkhoa@gmail.com", "Hồ Anh Khoa", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654340", "Member", "member_anhkhoa" },
                    { 36, new DateTime(2026, 7, 15, 16, 0, 0, 0, DateTimeKind.Utc), "thaovy@gmail.com", "Đinh Thảo Vy", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654341", "Member", "member_thaovy" },
                    { 37, new DateTime(2026, 8, 1, 9, 0, 0, 0, DateTimeKind.Utc), "minhtuan@gmail.com", "Cao Minh Tuấn", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654342", "Member", "member_minhtuan" },
                    { 38, new DateTime(2026, 8, 12, 10, 0, 0, 0, DateTimeKind.Utc), "kimngan@gmail.com", "Mai Kim Ngân", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654343", "Member", "member_kimngan" },
                    { 39, new DateTime(2026, 9, 2, 11, 0, 0, 0, DateTimeKind.Utc), "ductai@gmail.com", "Lương Đức Tài", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654344", "Member", "member_ductai" },
                    { 40, new DateTime(2026, 9, 10, 15, 0, 0, 0, DateTimeKind.Utc), "lanphuong@gmail.com", "Châu Lan Phương", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654345", "Member", "member_lanphuong" },
                    { 41, new DateTime(2026, 9, 22, 16, 30, 0, 0, DateTimeKind.Utc), "quangvinh@gmail.com", "Tô Quang Vinh", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654346", "Member", "member_quangvinh" },
                    { 42, new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Utc), "yennhi@gmail.com", "Vương Yến Nhi", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654347", "Member", "member_yennhi" },
                    { 43, new DateTime(2026, 10, 11, 10, 30, 0, 0, DateTimeKind.Utc), "haiha@gmail.com", "Kiều Hải Hà", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654348", "Member", "member_haiha" },
                    { 44, new DateTime(2026, 10, 19, 14, 0, 0, 0, DateTimeKind.Utc), "tienhung@gmail.com", "Chu Tiến Hưng", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654349", "Member", "member_tienhung" },
                    { 45, new DateTime(2026, 11, 1, 8, 45, 0, 0, DateTimeKind.Utc), "bichthuy@gmail.com", "Nghiêm Bích Thủy", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654350", "Member", "member_bichthuy" },
                    { 46, new DateTime(2026, 11, 4, 17, 15, 0, 0, DateTimeKind.Utc), "hoangphuc@gmail.com", "Đoàn Hoàng Phúc", "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=", "0987654351", "Member", "member_hoangphuc" }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerId", "AvatarUrl", "Bio", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 4, "~/images/coach4.jpg", "HLV 7 năm kinh nghiệm giảng dạy Hatha Yoga, thở chánh niệm và phục hồi cân bằng thân - tâm - trí.", "Hatha Yoga & Thiền Định Phục Hồi (Yoga Alliance RYT-500)", 10 },
                    { 5, "~/images/coach5.jpg", "HLV 6 năm kinh nghiệm dẫn dắt các lớp đạp xe trong nhà cường độ cao và HIIT đốt mỡ.", "Spinning RPM & Sức Bền Tim Mạch (Indoor Cycling Coach)", 11 },
                    { 6, "~/images/coach6.jpg", "HLV 8 năm kinh nghiệm huấn luyện BodyPump với thanh đòn tạ và tăng sức bền cơ bắp toàn thân.", "BodyPump & Sức Mạnh Kháng Lực (Les Mills Certified)", 12 },
                    { 7, "~/images/coach7.jpg", "HLV 5 năm kinh nghiệm thi đấu và giảng dạy Boxing, Kickfit tự vệ rèn phản xạ nhanh nhẹn.", "Kickfit, Muay Thai & Cardio Boxing (Combat Conditioning)", 13 },
                    { 8, "~/images/coach8.jpg", "HLV 6 năm kinh nghiệm trị liệu cột sống, cải thiện tư thế và làm săn chắc cơ lõi Core với Pilates.", "Pilates Reformer & Điều Chỉnh Tư Thế (Core & Mobility)", 14 }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "AvatarUrl", "BMR", "BodyFatPercentage", "CardCode", "Chest", "DateOfBirth", "Gender", "Height", "Hips", "MuscleMass", "QrCode", "UserId", "Waist", "Weight" },
                values: new object[,]
                {
                    { 25, "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", 1690.0, 18.5, "GYM-MB-0025", 96.0, new DateTime(1997, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 175.0, 94.0, 33.200000000000003, "GYM-MB-0025", 25, 79.0, 72.0 },
                    { 26, "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", 1310.0, 22.0, "GYM-MB-0026", 84.0, new DateTime(1999, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 163.0, 91.0, 21.5, "GYM-MB-0026", 26, 64.0, 53.0 },
                    { 27, "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400", 1800.0, 20.0, "GYM-MB-0027", 102.0, new DateTime(1995, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 179.0, 98.0, 36.0, "GYM-MB-0027", 27, 84.0, 80.5 },
                    { 28, "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400", 1280.0, 21.5, "GYM-MB-0028", 82.0, new DateTime(2000, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 161.0, 89.0, 20.199999999999999, "GYM-MB-0028", 28, 62.0, 51.0 },
                    { 29, "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400", 1720.0, 21.0, "GYM-MB-0029", 98.0, new DateTime(1993, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 174.0, 96.0, 33.799999999999997, "GYM-MB-0029", 29, 83.0, 76.0 },
                    { 30, "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400", 1350.0, 23.0, "GYM-MB-0030", 86.0, new DateTime(1998, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 165.0, 92.0, 22.100000000000001, "GYM-MB-0030", 30, 65.0, 55.0 },
                    { 31, "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400", 1740.0, 17.5, "GYM-MB-0031", 97.0, new DateTime(2001, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 177.0, 95.0, 34.5, "GYM-MB-0031", 31, 78.0, 73.5 },
                    { 32, "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400", 1270.0, 21.0, "GYM-MB-0032", 83.0, new DateTime(1996, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 160.0, 88.5, 20.399999999999999, "GYM-MB-0032", 32, 61.5, 50.5 },
                    { 33, "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400", 1840.0, 22.5, "GYM-MB-0033", 104.0, new DateTime(1992, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 180.0, 99.0, 36.799999999999997, "GYM-MB-0033", 33, 87.0, 84.0 },
                    { 34, "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400", 1330.0, 22.5, "GYM-MB-0034", 85.0, new DateTime(2001, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 164.0, 91.0, 21.800000000000001, "GYM-MB-0034", 34, 64.0, 54.0 },
                    { 35, "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", 1680.0, 18.0, "GYM-MB-0035", 95.0, new DateTime(1998, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 173.0, 94.0, 32.899999999999999, "GYM-MB-0035", 35, 79.0, 71.0 },
                    { 36, "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", 1250.0, 20.5, "GYM-MB-0036", 81.0, new DateTime(2002, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 159.0, 87.0, 19.800000000000001, "GYM-MB-0036", 36, 60.0, 49.0 },
                    { 37, "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400", 1760.0, 19.5, "GYM-MB-0037", 100.0, new DateTime(1994, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 176.0, 96.0, 35.100000000000001, "GYM-MB-0037", 37, 82.0, 77.5 },
                    { 38, "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400", 1300.0, 21.800000000000001, "GYM-MB-0038", 84.0, new DateTime(1997, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 162.0, 90.0, 21.0, "GYM-MB-0038", 38, 63.0, 52.0 },
                    { 39, "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400", 1785.0, 19.0, "GYM-MB-0039", 101.0, new DateTime(1996, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 178.0, 97.0, 35.899999999999999, "GYM-MB-0039", 39, 83.0, 79.0 },
                    { 40, "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400", 1370.0, 23.199999999999999, "GYM-MB-0040", 87.0, new DateTime(1999, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 166.0, 93.0, 22.600000000000001, "GYM-MB-0040", 40, 66.0, 56.5 },
                    { 41, "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400", 1715.0, 18.800000000000001, "GYM-MB-0041", 98.0, new DateTime(1995, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 175.0, 95.0, 34.0, "GYM-MB-0041", 41, 80.0, 74.5 },
                    { 42, "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400", 1275.0, 20.800000000000001, "GYM-MB-0042", 82.0, new DateTime(2000, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 161.0, 89.0, 20.300000000000001, "GYM-MB-0042", 42, 61.0, 50.0 },
                    { 43, "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400", 1320.0, 22.399999999999999, "GYM-MB-0043", 85.0, new DateTime(1998, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 163.0, 91.0, 21.600000000000001, "GYM-MB-0043", 43, 64.0, 54.0 },
                    { 44, "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400", 1790.0, 22.0, "GYM-MB-0044", 102.0, new DateTime(1991, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 177.0, 98.0, 35.600000000000001, "GYM-MB-0044", 44, 85.0, 81.0 },
                    { 45, "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", 1305.0, 21.899999999999999, "GYM-MB-0045", 84.0, new DateTime(1997, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nữ", 162.0, 90.0, 21.199999999999999, "GYM-MB-0045", 45, 63.0, 52.5 },
                    { 46, "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", 1675.0, 16.5, "GYM-MB-0046", 95.0, new DateTime(1999, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nam", 174.0, 93.0, 33.399999999999999, "GYM-MB-0046", 46, 77.0, 70.0 }
                });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                column: "BookingDate",
                value: new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                column: "BookingDate",
                value: new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                column: "BookingDate",
                value: new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 29,
                column: "BookingDate",
                value: new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 33,
                column: "BookingDate",
                value: new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "MemberId", "Notes", "Status", "TimeSlot", "TrainerId" },
                values: new object[,]
                {
                    { 35, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tập ngực trên & vai trước (Incline Press)", "Completed", "07:00 - 08:30", 1 },
                    { 36, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Mông đùi & ổn định khớp gối", "Completed", "09:30 - 11:00", 1 },
                    { 37, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Tập kéo lưng xô & tay trước (Pull Day)", "Completed", "17:00 - 18:30", 1 },
                    { 38, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Heavy Squat 5x5 tăng sức mạnh thân dưới", "Completed", "19:00 - 20:30", 1 },
                    { 39, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Deadlift kỹ thuật & sức mạnh cơ dựng lưng", "Completed", "07:00 - 08:30", 1 },
                    { 40, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Siết eo & Cardio HIIT cuối buổi", "Completed", "10:00 - 11:30", 1 },
                    { 41, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Giãn cơ cổ vai gáy & Core Balance", "Completed", "15:30 - 17:00", 1 },
                    { 42, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Tập thân trên toàn diện & đốt mỡ", "Completed", "18:00 - 19:30", 1 },
                    { 43, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chân & Bắp chân (Squat + Leg Press)", "Completed", "08:00 - 09:30", 1 },
                    { 44, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Cải thiện sức bền thân dưới", "Completed", "14:00 - 15:30", 1 },
                    { 45, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Tập lưng trên & tay sau săn chắc", "Approved", "17:30 - 19:00", 1 },
                    { 46, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Vai rộng & cơ cầu vai (Overhead Press)", "Approved", "19:30 - 21:00", 1 },
                    { 47, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Bench Press nặng & phụ trợ tay sau", "Approved", "07:00 - 08:30", 1 },
                    { 48, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Lưng xô & Kéo xà có tải trọng", "Approved", "09:30 - 11:00", 1 },
                    { 49, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Glute Bridge & Romanian Deadlift nhẹ", "Approved", "15:30 - 17:00", 1 },
                    { 50, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Vai, bụng & 20 phút chạy dốc đốt mỡ", "Approved", "18:00 - 19:30", 1 },
                    { 51, new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Hip Thrust & Lunges đốt mỡ đùi", "Approved", "07:00 - 08:30", 1 },
                    { 52, new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Full Body Conditioning & Core", "Approved", "09:00 - 10:30", 1 },
                    { 53, new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Ngực, vai & Cardio đốt năng lượng", "Approved", "16:00 - 17:30", 1 },
                    { 54, new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Xin tập bổ sung kỹ thuật Squat sâu", "Pending", "18:30 - 20:00", 1 },
                    { 55, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tổng kết InBody giữa chu kỳ 12 tuần", "Approved", "08:00 - 09:30", 1 },
                    { 56, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Functional Circuit cuối tuần", "Approved", "10:00 - 11:30", 1 },
                    { 57, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Đặt lịch tập thân trên & tư thế vai", "Pending", "14:30 - 16:00", 1 },
                    { 58, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Phụ trợ tay trước sau & Core", "Approved", "17:00 - 18:30", 1 },
                    { 59, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Phục hồi cột sống & giãn cơ sâu", "Approved", "08:30 - 10:00", 1 },
                    { 60, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Cardio Zone 2 & siết cơ bụng", "Approved", "10:30 - 12:00", 1 },
                    { 62, new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Ngực & Tay sau tuần mới (Progressive Overload)", "Approved", "07:00 - 08:30", 1 },
                    { 63, new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Chân mông tăng mức tạ Hip Thrust", "Approved", "09:30 - 11:00", 1 },
                    { 64, new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Push Day tăng khối lượng tạ", "Approved", "18:00 - 19:30", 1 },
                    { 65, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Deadlift & Lưng giữa", "Approved", "07:00 - 08:30", 1 },
                    { 66, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Cải thiện độ linh hoạt khớp hông", "Approved", "15:30 - 17:00", 1 },
                    { 67, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Thân trên & HIIT đốt mỡ", "Approved", "18:30 - 20:00", 1 },
                    { 68, new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chân & Mông cường độ vừa", "Approved", "08:00 - 09:30", 1 },
                    { 69, new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Chỉnh form Lunge & Squat", "Approved", "10:30 - 12:00", 1 },
                    { 70, new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Lưng xô & tư thế vai", "Approved", "15:00 - 16:30", 1 },
                    { 71, new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Squat & Leg Press", "Approved", "07:00 - 08:30", 1 },
                    { 72, new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Bench Press nặng 5x5", "Approved", "09:30 - 11:00", 1 },
                    { 73, new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Pull Day & Tay trước", "Approved", "18:00 - 19:30", 1 },
                    { 74, new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Vai & Core chống xoay", "Approved", "07:00 - 08:30", 1 },
                    { 75, new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Giảm mỡ toàn thân & Cardio", "Approved", "10:00 - 11:30", 1 },
                    { 77, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Upper Power & Core", "Approved", "08:00 - 09:30", 1 },
                    { 78, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Deadlift 5x3 mục tiêu PR mới", "Approved", "10:30 - 12:00", 1 },
                    { 79, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 5, "HIIT Sprint & Core", "Approved", "16:00 - 17:30", 1 },
                    { 80, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Core Balance & Mobility", "Approved", "08:30 - 10:00", 1 },
                    { 81, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Mông đùi săn chắc", "Approved", "15:00 - 16:30", 1 },
                    { 83, new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Ngực & Tay sau cường độ cao", "Approved", "07:00 - 08:30", 1 },
                    { 84, new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Glutes Hypertrophy", "Approved", "17:00 - 18:30", 1 },
                    { 85, new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Squat & Calf Raise", "Approved", "19:00 - 20:30", 1 },
                    { 86, new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Full Body Functional", "Approved", "07:00 - 08:30", 1 },
                    { 87, new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Đo InBody & chỉnh bài tập eo", "Approved", "10:00 - 11:30", 1 },
                    { 88, new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Lưng xô & Cardio", "Approved", "18:00 - 19:30", 1 },
                    { 89, new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Deadlift & Lưng dày", "Approved", "08:00 - 09:30", 1 },
                    { 90, new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Tập Core & phục hồi vai", "Approved", "15:30 - 17:00", 1 },
                    { 91, new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Squat kỹ thuật & bắp chân", "Approved", "18:30 - 20:00", 1 },
                    { 92, new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Overhead Press & Pull-up", "Approved", "07:00 - 08:30", 1 },
                    { 93, new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Đốt mỡ thân dưới", "Approved", "14:00 - 15:30", 1 },
                    { 94, new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 21, "Thân trên & siết bụng", "Approved", "17:30 - 19:00", 1 },
                    { 95, new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Đo lại InBody cuối tháng 11", "Approved", "08:00 - 09:30", 1 },
                    { 96, new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tổng kết 12 tuần & giãn cơ", "Approved", "10:30 - 12:00", 1 },
                    { 97, new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 22, "Ngực vai tay toàn diện", "Approved", "18:00 - 19:30", 1 },
                    { 98, new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Kiểm tra sức mạnh Big 3 cuối tháng", "Approved", "08:30 - 10:00", 1 },
                    { 99, new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 23, "Pilates kết hợp tạ nhẹ", "Approved", "15:00 - 16:30", 1 },
                    { 101, new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 24, "Điều hòa tim mạch & giãn cơ", "Approved", "09:00 - 10:30", 1 },
                    { 102, new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 5, "Cardio kết hợp Core cuối tuần", "Approved", "16:00 - 17:30", 1 }
                });

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 5,
                columns: new[] { "BookingDate", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), "Xác nhận tham gia lớp BodyPump tối thứ 6.", "Confirmed" });

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 6,
                columns: new[] { "BookingDate", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), "Đã đặt chỗ thành công.", "Confirmed" });

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 7,
                column: "BookingDate",
                value: new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 8,
                column: "BookingDate",
                value: new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 9,
                column: "BookingDate",
                value: new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "ClassBookings",
                columns: new[] { "ClassBookingId", "BookingDate", "ClassId", "MemberId", "Notes", "Status" },
                values: new object[,]
                {
                    { 38, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 2, "Đăng ký lớp BodyPump HLV Liam.", "Confirmed" },
                    { 39, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 3, "Giữ vị trí khu vực trung tâm.", "Confirmed" },
                    { 40, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 4, "Sẵn sàng bộ tạ 15kg.", "Confirmed" },
                    { 41, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 21, "Đăng ký qua cổng hội viên.", "Confirmed" },
                    { 42, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 22, "Tập sức bền thân trên & đùi.", "Confirmed" },
                    { 43, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 23, "Đã xác nhận tham gia.", "Confirmed" },
                    { 44, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 24, "Tăng sức bền toàn thân.", "Confirmed" },
                    { 61, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 1, "Tập phản xạ Boxing cùng HLV Devon.", "Confirmed" },
                    { 62, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 5, "Mượn găng Boxing 12oz của phòng.", "Confirmed" },
                    { 63, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 22, "Đăng ký buổi tối thứ 5/CN.", "Confirmed" },
                    { 64, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 24, "Tăng thể lực tim mạch & đấm bao cát.", "Confirmed" },
                    { 68, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 21, "Xếp máy Reformer số 1.", "Confirmed" },
                    { 69, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 23, "Tập cùng HLV Chloe Bennett.", "Confirmed" }
                });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 1,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 14, "https://images.unsplash.com/photo-1545205597-3d9d02c29597?w=800&auto=format&fit=crop", 4 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 2,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 16, "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&auto=format&fit=crop", 5 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 3,
                columns: new[] { "ImageUrl", "TrainerId" },
                values: new object[] { "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=800&auto=format&fit=crop", 6 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 4,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 8, "https://images.unsplash.com/photo-1549719386-74dfcbf7dbed?w=800&auto=format&fit=crop", 7 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 5,
                columns: new[] { "ImageUrl", "TrainerId" },
                values: new object[] { "https://images.unsplash.com/photo-1518611012118-696072aa579a?w=800&auto=format&fit=crop", 8 });

            migrationBuilder.InsertData(
                table: "MaintenanceLogs",
                columns: new[] { "LogId", "Cost", "Description", "EquipmentId", "MaintenanceDate", "Notes", "PerformedBy", "Status" },
                values: new object[,]
                {
                    { 9, 650000m, "Kiểm tra tổng quát máy chạy bộ Technogym đầu năm, hiệu chuẩn độ nghiêng 15% và vệ sinh quạt tản nhiệt.", 1, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoạt động ổn định.", "Technogym Vietnam Services", "Hoàn thành" },
                    { 10, 450000m, "Vệ sinh cụm cảm biến từ tính kháng lực xe đạp Matrix CXP và siết chặt bàn đạp.", 3, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đo công suất Watt chính xác.", "Bảo trì nội bộ Gym", "Hoàn thành" },
                    { 11, 500000m, "Kiểm tra chốt an toàn J-hooks và thanh chắn trên khung Power Rack, sơn dặm chống gỉ phần đế.", 6, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đảm bảo tải trọng 400kg.", "Xưởng cơ khí Thành Phát", "Hoàn thành" },
                    { 12, 900000m, "Siết bulong giá đỡ giàn tạ đơn, thay mới đệm cao su chống va đập trên 6 cặp tạ từ 20-35kg.", 7, new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Giảm tiếng ồn khi hạ tạ.", "Bảo trì nội bộ Gym", "Hoàn thành" },
                    { 13, 2200000m, "Thay thảm chạy và kiểm tra bo mạch công suất máy chạy bộ Life Fitness sau 6 tháng vận hành cao điểm.", 2, new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thảm chạy mới êm ái, độ bám tốt.", "Kỹ thuật viên phòng máy (Nguyễn Văn Bình)", "Hoàn thành" },
                    { 14, 400000m, "Tra dầu puly ròng rọc máy ép ngực và vệ sinh thanh dẫn trượt chồng tạ 90kg.", 8, new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chuyển động mượt.", "Bảo trì nội bộ Gym", "Hoàn thành" },
                    { 15, 800000m, "Kiểm tra cơ cấu khóa an toàn 2 bên của máy đạp đùi Leg Press 45 độ và thay 2 chốt lò xo.", 9, new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đạt chuẩn an toàn kỹ thuật.", "Xưởng cơ khí & da thể thao Thành Phát", "Hoàn thành" },
                    { 16, 700000m, "Thay mới yên đệm gel thể thao và kiểm tra bộ truyền động dây curoa xe đạp Matrix CXP.", 3, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phục vụ lớp Spinning RPM.", "Bảo trì nội bộ Gym", "Hoàn thành" },
                    { 17, 1100000m, "Hoàn tất thay thế cụm xích mạ niken và hiệu chỉnh màn hình PM5 máy chèo thuyền Concept2.", 4, new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đã nghiệm thu bàn giao đưa vào sử dụng.", "Trung tâm sửa chữa thiết bị thể thao Việt Á", "Hoàn thành" },
                    { 18, 550000m, "Bảo dưỡng định kỳ trước đợt cao điểm cuối năm: vệ sinh motor, bôi trơn băng tải máy chạy Technogym.", 1, new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Máy vận hành ổn định 100%.", "Technogym Vietnam Services", "Hoàn thành" }
                });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 1,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", "GYM-MB-0001", "GYM-MB-0001" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 2,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", "GYM-MB-0002", "GYM-MB-0002" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 3,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400", "GYM-MB-0003", "GYM-MB-0003" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 4,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400", "GYM-MB-0004", "GYM-MB-0004" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 5,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400", "GYM-MB-0005", "GYM-MB-0005" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 21,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400", "GYM-MB-0021", "GYM-MB-0021" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 22,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400", "GYM-MB-0022", "GYM-MB-0022" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 23,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400", "GYM-MB-0023", "GYM-MB-0023" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 24,
                columns: new[] { "AvatarUrl", "CardCode", "QrCode" },
                values: new object[] { "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400", "GYM-MB-0024", "GYM-MB-0024" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "MemberId", "Notes", "OrderDate", "PaymentMethod", "PhoneNumber", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 6, 1, "Đơn POS đầu năm: Whey Gold + Bình lắc.", new DateTime(2026, 1, 18, 17, 15, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654321", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2030000m, 5 },
                    { 7, 24, "Mua phụ kiện đai lưng + găng tay tại quầy.", new DateTime(2026, 1, 27, 19, 0, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654329", "Mua trực tiếp tại quầy POS", "Đã thanh toán", 1240000m, 24 },
                    { 8, 21, "Rule 1 Protein Isolate tại quầy lễ tân.", new DateTime(2026, 2, 14, 10, 30, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654326", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1890000m, 21 },
                    { 9, 22, "Creatine + Pre-Workout C4.", new DateTime(2026, 2, 23, 18, 45, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654327", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1370000m, 22 },
                    { 10, 3, "Whey Gold + Creatine.", new DateTime(2026, 3, 9, 16, 20, 0, 0, DateTimeKind.Unspecified), "Chuyển khoản", "0987654323", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2330000m, 7 },
                    { 12, 23, "BCAA Xtend + Bình lắc BlenderBottle.", new DateTime(2026, 4, 11, 11, 15, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654328", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1430000m, 23 },
                    { 14, 1, "Whey Gold + Pre-Workout C4.", new DateTime(2026, 5, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654321", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2540000m, 5 },
                    { 16, 2, "BCAA Xtend + Hộp Quest Bar.", new DateTime(2026, 6, 8, 9, 45, 0, 0, DateTimeKind.Unspecified), "Chuyển khoản", "0987654322", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1840000m, 6 },
                    { 18, 22, "Whey Gold + Creatine Monohydrate.", new DateTime(2026, 7, 14, 17, 20, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654327", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2330000m, 22 },
                    { 20, 24, "Rule 1 Isolate + Đai lưng Lever Belt.", new DateTime(2026, 8, 10, 16, 10, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654329", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2740000m, 24 },
                    { 22, 21, "BCAA Xtend + Pre-Workout C4.", new DateTime(2026, 9, 12, 17, 0, 0, 0, DateTimeKind.Unspecified), "Chuyển khoản", "0987654326", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1940000m, 21 },
                    { 24, 1, "Rule 1 Isolate + Creatine Platinum mua tại quầy.", new DateTime(2026, 11, 5, 17, 30, 0, 0, DateTimeKind.Unspecified), "VNPay", "0987654321", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2470000m, 5 },
                    { 25, 3, "Đai lưng Lever Belt + Găng tay + Áo GymTech + Bình lắc.", new DateTime(2026, 11, 10, 18, 50, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654323", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1780000m, 7 }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "ImageUrl",
                value: "~/images/products/gymtech-shirt.svg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "ImageUrl",
                value: "~/images/products/quest-bar.svg");

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "CarbG", "CreatedAt", "EndDate", "FatG", "Goal", "InputJson", "IsActive", "IsDeleted", "MemberId", "NutritionAdvice", "PlanJson", "ProteinG", "RoutineDetails", "Source", "StartDate", "Status", "TargetKcal", "Title", "TrainerId", "UpdatedAt", "WaterMl" },
                values: new object[,]
                {
                    { 6, null, new DateTime(2026, 11, 10, 9, 15, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "AI khuyến nghị: Giảm mỡ vùng bụng đùi về 20% bodyfat, giữ khối lượng cơ nạc với lịch Push/Pull/Legs + Core.", "{\"heightCm\":162,\"weightKg\":54.5,\"bodyFatPercent\":23.0,\"muscleMassKg\":22.5,\"dateOfBirth\":\"1998-05-12T00:00:00\",\"gender\":\"Nữ\",\"goal\":\"fat_loss\",\"sessionsPerWeek\":4,\"weekdays\":[1,2,4,6],\"sessionMinutes\":60,\"experience\":\"experienced\",\"activity\":\"sedentary\",\"diet\":\"clean\",\"allergies\":[],\"injuries\":[],\"condition\":\"none\",\"consent\":true}", true, false, 2, "- Calo mục tiêu: 1.498 kcal/ngày (Thâm hụt 300 kcal).\n- Đạm 104g, Tinh bột 176g, Chất béo 42g/ngày.\n- Uống 2.2L - 2.7L nước mỗi ngày.", "{\"version\":1,\"targets\":{\"goal\":\"fat_loss\",\"age\":28,\"bmi\":20.8,\"bmr\":1240,\"bmrSource\":\"Mifflin-St Jeor\",\"activityFactor\":1.45,\"tdee\":1798,\"targetKcal\":1498,\"kcalDelta\":-300,\"proteinPerKg\":1.9,\"proteinG\":104,\"fatG\":42,\"carbG\":176,\"proteinPct\":28,\"fatPct\":25,\"carbPct\":47,\"waterMl\":2200,\"waterMlTrainingDay\":2700,\"estimateText\":\"Khoảng 8-10 tuần giảm ~2.5-3 kg mỡ bền vững\",\"explainKcal\":\"Thâm hụt nhẹ 300 kcal/ngày giúp giảm mỡ mà không mất sức.\",\"explainMacro\":\"Đạm 1.9g/kg bảo vệ cơ nạc trong giai đoạn siết.\",\"explainWater\":\"Uống 2.2L ngày nghỉ và 2.7L ngày tập.\",\"explainEstimate\":\"Ước tính dựa trên 4 buổi/tuần kết hợp ăn sạch.\"},\"splitName\":\"Thân trên / Thân dưới 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":4,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":\"Kiểm soát hạ tạ 2 giây\"},{\"exerciseId\":14,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":10,\"sets\":3,\"reps\":\"12-15\",\"restSec\":60,\"note\":null},{\"exerciseId\":35,\"sets\":3,\"reps\":\"45 giây\",\"restSec\":45,\"note\":\"Siết chặt core\"}],\"cardio\":\"20 phút đi bộ dốc trên máy Technogym\"},{\"weekday\":2,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":32,\"sets\":4,\"reps\":\"10-12\",\"restSec\":90,\"note\":\"Giữ đỉnh 1 giây siết mông\"},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":28,\"sets\":3,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":null},{\"weekday\":4,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":5,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null},{\"exerciseId\":15,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":6,\"focus\":\"Thân dưới B\",\"exercises\":[{\"exerciseId\":27,\"sets\":4,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":31,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":90,\"note\":null},{\"exerciseId\":33,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":36,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null}],\"cardio\":null}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Yến mạch sữa hạnh nhân + 2 trứng luộc\",\"kcal\":360,\"proteinG\":22},{\"slot\":\"Trưa\",\"dish\":\"150g ức gà áp chảo + 120g cơm gạo lứt + rau luộc\",\"kcal\":470,\"proteinG\":40},{\"slot\":\"Xế (trước tập)\",\"dish\":\"1 quả chuối + 1 hũ sữa chua Hy Lạp\",\"kcal\":220,\"proteinG\":14},{\"slot\":\"Tối\",\"dish\":\"150g cá hồi nướng + salad bơ rau xanh\",\"kcal\":448,\"proteinG\":32}],\"tips\":[\"Ngủ đủ 7-8 giờ mỗi đêm để phục hồi cơ và ổn định hormone.\",\"Tăng mức tạ 2.5kg khi vượt quá số rep mục tiêu ở cả 3 hiệp.\"],\"origin\":\"ai\",\"notices\":[\"Ưu tiên bài Hip Thrust & kéo lưng cải thiện vóc dáng và cột sống.\"],\"generatedAt\":\"2026-11-10T09:15:00Z\"}", null, "- Thứ 2 (Thân trên A): Đẩy ngực dốc tạ đơn 3x10-12, Lat Pulldown 3x10-12, Lateral Raise 3x12-15, Plank 3x45s + Cardio 20p.\n- Thứ 3 (Thân dưới A): Hip Thrust 4x10-12, Romanian Deadlift 3x10-12, Leg Press 3x12, Dead Bug 3x12.\n- Thứ 5 (Thân trên B): Pec Fly 3x12, Seated Cable Row 3x10-12, Face Pull 3x15, Pallof Press 3x12 + Đạp xe 20p.\n- Thứ 7 (Thân dưới B): Goblet Squat 4x10-12, Bulgarian Split Squat 3x10, Cable Kickback 3x12, Cable Crunch 3x15.", "AI", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), "PendingReview", null, "[AI Smart] Giảm mỡ & săn chắc toàn thân 4 buổi/tuần (AI)", null, null, null },
                    { 7, null, new DateTime(2026, 11, 11, 8, 40, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "AI khuyến nghị: Giảm mỡ nhẹ từ 23.5% xuống 20%, tăng độ săn chắc vòng 3 và cơ bụng cốt lõi.", "{\"heightCm\":164,\"weightKg\":56.0,\"bodyFatPercent\":23.5,\"muscleMassKg\":21.8,\"dateOfBirth\":\"2000-03-14T00:00:00\",\"gender\":\"Nữ\",\"goal\":\"fat_loss\",\"sessionsPerWeek\":3,\"weekdays\":[1,3,5],\"sessionMinutes\":60,\"experience\":\"beginner\",\"activity\":\"moderate\",\"diet\":\"clean\",\"allergies\":[\"seafood\"],\"injuries\":[],\"condition\":\"none\",\"consent\":true}", true, false, 21, "- Calo mục tiêu: 1.630 kcal/ngày (Loại bỏ hải sản).\n- Đạm 101g, Tinh bột 205g, Chất béo 45g/ngày.", "{\"version\":1,\"targets\":{\"goal\":\"fat_loss\",\"age\":26,\"bmi\":20.8,\"bmr\":1320,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.5,\"tdee\":1980,\"targetKcal\":1630,\"kcalDelta\":-350,\"proteinPerKg\":1.8,\"proteinG\":101,\"fatG\":45,\"carbG\":205,\"proteinPct\":25,\"fatPct\":25,\"carbPct\":50,\"waterMl\":2300,\"waterMlTrainingDay\":2800,\"estimateText\":\"Khoảng 8 tuần giảm ~3kg mỡ, vòng eo giảm 3-4 cm\",\"explainKcal\":\"Thâm hụt 350 kcal phù hợp lịch 3 buổi/tuần và vận động vừa.\",\"explainMacro\":\"Thực đơn không hải sản theo khai báo dị ứng.\",\"explainWater\":\"Bổ sung thêm 500ml nước trong các buổi tập chân mông.\",\"explainEstimate\":\"Duy trì đủ protein và giấc ngủ sẽ thấy rõ sau 4 tuần.\"},\"splitName\":\"Toàn thân 3 buổi (A - B - A)\",\"days\":[{\"weekday\":1,\"focus\":\"Toàn thân A\",\"exercises\":[{\"exerciseId\":26,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":\"Xuống chậm kiểm soát gối\"},{\"exerciseId\":2,\"sets\":3,\"reps\":\"10-12\",\"restSec\":75,\"note\":null},{\"exerciseId\":14,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":32,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":\"Siết đỉnh 1 giây\"},{\"exerciseId\":35,\"sets\":3,\"reps\":\"45 giây\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút đi bộ dốc 8%\"},{\"weekday\":3,\"focus\":\"Toàn thân B\",\"exercises\":[{\"exerciseId\":29,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":8,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":15,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":33,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":5,\"focus\":\"Chân & Mông + Core\",\"exercises\":[{\"exerciseId\":28,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":30,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":75,\"note\":null},{\"exerciseId\":16,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":75,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút đi bộ dốc trên máy Technogym\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Bánh mì nguyên cám + 2 trứng ốp la + 1/2 quả bơ\",\"kcal\":390,\"proteinG\":20},{\"slot\":\"Trưa\",\"dish\":\"150g ức gà nướng mật ong + 130g cơm + bông cải xanh\",\"kcal\":520,\"proteinG\":40},{\"slot\":\"Xế (trước tập)\",\"dish\":\"100g khoai lang hấp + 1 muỗng Whey Isolate\",\"kcal\":240,\"proteinG\":25},{\"slot\":\"Tối\",\"dish\":\"150g thịt thăn bò xào rau củ + súp lơ\",\"kcal\":480,\"proteinG\":36}],\"tips\":[\"Không ăn hải sản (tôm, cua, mực) theo hồ sơ dị ứng đã ghi nhận.\",\"Giữ lưng thẳng khi thực hiện Romanian Deadlift.\"],\"origin\":\"ai\",\"notices\":[\"Đã loại bỏ hải sản khỏi gợi ý thực đơn.\"],\"generatedAt\":\"2026-11-11T08:40:00Z\"}", null, "- Thứ 2 (Toàn thân A): Squat Smith 3x10-12, Đẩy ngực Smith 3x10-12, Lat Pulldown 3x12, Hip Thrust 4x12, Plank 3x45s.\n- Thứ 4 (Toàn thân B): Romanian Deadlift 3x10-12, Đẩy vai tạ đơn 3x12, Seated Cable Row 3x12, Cable Kickback 3x12, Dead Bug 3x12.\n- Thứ 6 (Chân Mông & Core): Leg Press 4x12, Walking Lunge 3x10, Chèo tạ đơn 1 tay 3x12, Pallof Press 3x12.", "AI", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), "PendingReview", null, "[AI Smart] Lộ trình thon gọn eo & tăng cơ mông đùi 3 buổi (AI)", null, null, null },
                    { 8, null, new DateTime(2026, 11, 11, 14, 20, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "AI khuyến nghị: Dư nhẹ 250 kcal để tăng khối lượng cơ nạc toàn thân, nâng tạ đa khớp làm trọng tâm.", "{\"heightCm\":178,\"weightKg\":82.0,\"bodyFatPercent\":24.0,\"muscleMassKg\":35.0,\"dateOfBirth\":\"1996-07-25T00:00:00\",\"gender\":\"Nam\",\"goal\":\"muscle_gain\",\"sessionsPerWeek\":4,\"weekdays\":[1,2,4,5],\"sessionMinutes\":90,\"experience\":\"experienced\",\"activity\":\"moderate\",\"diet\":\"normal\",\"allergies\":[],\"injuries\":[],\"condition\":\"none\",\"consent\":true}", true, false, 22, "- Calo mục tiêu: 2.965 kcal/ngày (Surplus +250 kcal).\n- Đạm 172g, Tinh bột 394g, Chất béo 78g/ngày.", "{\"version\":1,\"targets\":{\"goal\":\"muscle_gain\",\"age\":30,\"bmi\":25.9,\"bmr\":1810,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.5,\"tdee\":2715,\"targetKcal\":2965,\"kcalDelta\":250,\"proteinPerKg\":2.1,\"proteinG\":172,\"fatG\":78,\"carbG\":394,\"proteinPct\":23,\"fatPct\":24,\"carbPct\":53,\"waterMl\":2900,\"waterMlTrainingDay\":3500,\"estimateText\":\"Tăng 0.8-1.2 kg cơ nạc sau 8 tuần với áp lực tạ tăng tiến\",\"explainKcal\":\"Dư nhẹ 250 kcal giúp tăng cơ hạn chế tích mỡ bụng.\",\"explainMacro\":\"Đạm 2.1g/kg hỗ trợ tổng hợp protein cơ bắp tối ưu.\",\"explainWater\":\"Uống 3.5L vào ngày tập nặng 90 phút.\",\"explainEstimate\":\"Theo dõi tiến độ tăng tạ ở Bench Press, Squat và Barbell Row.\"},\"splitName\":\"Thân trên / Thân dưới 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":1,\"sets\":4,\"reps\":\"6-8\",\"restSec\":120,\"note\":\"Chạm ngực kiểm soát\"},{\"exerciseId\":17,\"sets\":4,\"reps\":\"8\",\"restSec\":120,\"note\":null},{\"exerciseId\":9,\"sets\":3,\"reps\":\"8-10\",\"restSec\":120,\"note\":null},{\"exerciseId\":11,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null},{\"exerciseId\":22,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":2,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":25,\"sets\":4,\"reps\":\"6-8\",\"restSec\":150,\"note\":\"Phát lực mạnh từ giữa chân\"},{\"exerciseId\":28,\"sets\":3,\"reps\":\"10-12\",\"restSec\":90,\"note\":null},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":34,\"sets\":4,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":37,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":4,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":4,\"sets\":4,\"reps\":\"8-10\",\"restSec\":90,\"note\":null},{\"exerciseId\":19,\"sets\":4,\"reps\":\"8-10\",\"restSec\":120,\"note\":null},{\"exerciseId\":10,\"sets\":4,\"reps\":\"12-15\",\"restSec\":60,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":23,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":null},{\"weekday\":5,\"focus\":\"Thân dưới B\",\"exercises\":[{\"exerciseId\":18,\"sets\":4,\"reps\":\"5-6\",\"restSec\":150,\"note\":\"Khởi động kỹ lưng dưới\"},{\"exerciseId\":31,\"sets\":3,\"reps\":\"10 mỗi bên\",\"restSec\":90,\"note\":null},{\"exerciseId\":32,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":36,\"sets\":4,\"reps\":\"15\",\"restSec\":60,\"note\":null}],\"cardio\":\"15 phút chèo thuyền Concept2\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Phở bò tái chín + 2 trứng chần + 1 ly sữa tươi\",\"kcal\":650,\"proteinG\":42},{\"slot\":\"Trưa\",\"dish\":\"200g ức gà / thăn bò + 2 chén cơm + rau xanh\",\"kcal\":820,\"proteinG\":55},{\"slot\":\"Xế (trước tập)\",\"dish\":\"2 lát bánh mì bơ đậu phộng + 1 muỗng Whey\",\"kcal\":440,\"proteinG\":32},{\"slot\":\"Tối\",\"dish\":\"200g cá hồi / thịt nạc + 1.5 chén cơm + canh rau\",\"kcal\":780,\"proteinG\":48}],\"tips\":[\"Ghi chép mức tạ sau mỗi buổi tập để tăng tiến đều đặn.\",\"Ngủ đủ 8 tiếng giúp tối đa hóa hormone tăng trưởng.\"],\"origin\":\"ai\",\"notices\":[\"Giáo án cường độ cao dành cho người đã có nền tảng tập trên 6 tháng.\"],\"generatedAt\":\"2026-11-11T14:20:00Z\"}", null, "- Thứ 2 (Thân trên A): Bench Press 4x6-8, Barbell Row 4x8, Overhead Press 3x8-10, Tricep Pushdown 3x12, Dumbbell Curl 3x12.\n- Thứ 3 (Thân dưới A): Barbell Squat 4x6-8, Leg Press 3x10-12, Romanian Deadlift 3x10, Calf Raise 4x15, Hanging Leg Raise 3x12.\n- Thứ 5 (Thân trên B): Incline Dumbbell Press 4x8-10, Pull-up 4x8-10, Lateral Raise 4x12-15, Face Pull 3x15, Hammer Curl 3x12.\n- Thứ 6 (Thân dưới B): Deadlift 4x5-6, Bulgarian Split Squat 3x10, Hip Thrust 3x10, Cable Crunch 4x15.", "AI", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), "PendingReview", null, "[AI Smart] Hypertrophy tăng cơ nạc 4 buổi/tuần (AI)", null, null, null },
                    { 9, null, new DateTime(2026, 11, 12, 7, 30, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "AI khuyến nghị: Duy trì cân nặng hiện tại 75kg, giảm dần tỷ lệ mỡ về 17% và cải thiện VO2Max kết hợp sức mạnh.", "{\"heightCm\":172,\"weightKg\":75.0,\"bodyFatPercent\":21.0,\"muscleMassKg\":33.0,\"dateOfBirth\":\"1994-09-08T00:00:00\",\"gender\":\"Nam\",\"goal\":\"endurance\",\"sessionsPerWeek\":4,\"weekdays\":[1,3,5,7],\"sessionMinutes\":60,\"experience\":\"experienced\",\"activity\":\"moderate\",\"diet\":\"clean\",\"allergies\":[\"dairy\"],\"injuries\":[],\"condition\":\"none\",\"consent\":true}", true, false, 24, "- Calo mục tiêu: 2.635 kcal/ngày (Loại bỏ sữa theo hồ sơ dị ứng).\n- Đạm 143g, Tinh bột 354g, Chất béo 72g/ngày.", "{\"version\":1,\"targets\":{\"goal\":\"endurance\",\"age\":32,\"bmi\":25.4,\"bmr\":1700,\"bmrSource\":\"Katch-McArdle\",\"activityFactor\":1.55,\"tdee\":2635,\"targetKcal\":2635,\"kcalDelta\":0,\"proteinPerKg\":1.9,\"proteinG\":143,\"fatG\":72,\"carbG\":354,\"proteinPct\":22,\"fatPct\":25,\"carbPct\":53,\"waterMl\":2700,\"waterMlTrainingDay\":3300,\"estimateText\":\"Thể lực và nhịp tim phục hồi cải thiện rõ rệt sau 4-6 tuần\",\"explainKcal\":\"Nạp đủ TDEE giúp phục hồi tốt giữa các buổi tạ và cardio.\",\"explainMacro\":\"Thực đơn không sữa bò theo khai báo dị ứng (dùng đạm thịt, cá, trứng, đậu).\",\"explainWater\":\"Bù điện giải sau mỗi buổi chèo thuyền hoặc chạy bộ.\",\"explainEstimate\":\"Kết hợp bài đa khớp với 20-25 phút cardio Zone 2/HIIT.\"},\"splitName\":\"Thân trên / Thân dưới + Cardio 4 buổi\",\"days\":[{\"weekday\":1,\"focus\":\"Thân trên A\",\"exercises\":[{\"exerciseId\":3,\"sets\":4,\"reps\":\"10\",\"restSec\":75,\"note\":null},{\"exerciseId\":14,\"sets\":4,\"reps\":\"10\",\"restSec\":75,\"note\":null},{\"exerciseId\":8,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":35,\"sets\":3,\"reps\":\"60 giây\",\"restSec\":45,\"note\":null}],\"cardio\":\"20 phút chạy bộ trên máy Life Fitness\"},{\"weekday\":3,\"focus\":\"Thân dưới A\",\"exercises\":[{\"exerciseId\":26,\"sets\":4,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":29,\"sets\":3,\"reps\":\"10\",\"restSec\":90,\"note\":null},{\"exerciseId\":30,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":60,\"note\":null},{\"exerciseId\":39,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"15 phút chèo thuyền Concept2\"},{\"weekday\":5,\"focus\":\"Thân trên B\",\"exercises\":[{\"exerciseId\":7,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":15,\"sets\":4,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":20,\"sets\":3,\"reps\":\"15\",\"restSec\":60,\"note\":null},{\"exerciseId\":11,\"sets\":3,\"reps\":\"12\",\"restSec\":60,\"note\":null}],\"cardio\":\"20 phút đạp xe Matrix CXP\"},{\"weekday\":7,\"focus\":\"Thân dưới & Core\",\"exercises\":[{\"exerciseId\":28,\"sets\":4,\"reps\":\"12\",\"restSec\":90,\"note\":null},{\"exerciseId\":32,\"sets\":3,\"reps\":\"12\",\"restSec\":75,\"note\":null},{\"exerciseId\":38,\"sets\":3,\"reps\":\"12 mỗi bên\",\"restSec\":45,\"note\":null}],\"cardio\":\"25 phút đi bộ dốc / chạy bộ trên máy Technogym\"}],\"meals\":[{\"slot\":\"Sáng\",\"dish\":\"Cháo yến mạch nấu thịt bằm + 2 trứng ốp la\",\"kcal\":560,\"proteinG\":32},{\"slot\":\"Trưa\",\"dish\":\"200g ức gà nướng + 2 chén cơm gạo lứt + rau xanh\",\"kcal\":750,\"proteinG\":48},{\"slot\":\"Xế (trước tập)\",\"dish\":\"2 quả chuối + 30g hạt hạnh nhân\",\"kcal\":360,\"proteinG\":12},{\"slot\":\"Tối\",\"dish\":\"200g cá thu sốt cà + 1.5 chén cơm + canh rau cải\",\"kcal\":765,\"proteinG\":46}],\"tips\":[\"Không sử dụng chế phẩm từ sữa bò (whey cô đặc, phô mai, sữa tươi) theo hồ sơ dị ứng.\",\"Giữ nhịp thở đều trong lúc chèo thuyền Concept2.\"],\"origin\":\"ai\",\"notices\":[\"Đã loại trừ sữa và chế phẩm từ sữa trong thực đơn.\"],\"generatedAt\":\"2026-11-12T07:30:00Z\"}", null, "- Thứ 2 (Thân trên A): Dumbbell Bench Press 4x10, Lat Pulldown 4x10, Seated Dumbbell Press 3x12, Plank 3x60s + Chạy bộ 20p.\n- Thứ 4 (Thân dưới A): Squat Smith 4x10, Romanian Deadlift 3x10, Walking Lunge 3x12, Dead Bug 3x12 + Chèo thuyền 15p.\n- Thứ 6 (Thân trên B): Push-up 3x15, Seated Cable Row 4x12, Face Pull 3x15, Tricep Pushdown 3x12 + Đạp xe 20p.\n- Chủ nhật (Thân dưới & Core): Leg Press 4x12, Hip Thrust 3x12, Pallof Press 3x12 + Cardio 25p.", "AI", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Utc), "PendingReview", null, "[AI Smart] Tăng thể lực & sức mạnh nền tảng 4 buổi (AI)", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 10, 6, 1, 1, 1750000m, 1750000m },
                    { 11, 6, 8, 1, 280000m, 280000m },
                    { 12, 7, 6, 1, 850000m, 850000m },
                    { 13, 7, 7, 1, 390000m, 390000m },
                    { 14, 8, 2, 1, 1890000m, 1890000m },
                    { 15, 9, 3, 1, 580000m, 580000m },
                    { 16, 9, 4, 1, 790000m, 790000m },
                    { 17, 10, 1, 1, 1750000m, 1750000m },
                    { 18, 10, 3, 1, 580000m, 580000m },
                    { 21, 12, 5, 1, 1150000m, 1150000m },
                    { 22, 12, 8, 1, 280000m, 280000m },
                    { 25, 14, 1, 1, 1750000m, 1750000m },
                    { 26, 14, 4, 1, 790000m, 790000m },
                    { 29, 16, 5, 1, 1150000m, 1150000m },
                    { 30, 16, 10, 1, 690000m, 690000m },
                    { 33, 18, 1, 1, 1750000m, 1750000m },
                    { 34, 18, 3, 1, 580000m, 580000m },
                    { 37, 20, 2, 1, 1890000m, 1890000m },
                    { 38, 20, 6, 1, 850000m, 850000m },
                    { 40, 22, 5, 1, 1150000m, 1150000m },
                    { 41, 22, 4, 1, 790000m, 790000m },
                    { 44, 24, 2, 1, 1890000m, 1890000m },
                    { 45, 24, 3, 1, 580000m, 580000m },
                    { 46, 25, 6, 1, 850000m, 850000m },
                    { 47, 25, 7, 1, 390000m, 390000m },
                    { 48, 25, 9, 1, 260000m, 260000m },
                    { 49, 25, 8, 1, 280000m, 280000m }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "BookingDate", "MemberId", "Notes", "Status", "TimeSlot", "TrainerId" },
                values: new object[,]
                {
                    { 61, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 25, "Hội viên mới đặt buổi đánh giá thể lực", "Pending", "16:30 - 18:00", 1 },
                    { 76, new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 26, "Yêu cầu tư vấn lộ trình giảm mỡ sau sinh", "Pending", "14:30 - 16:00", 1 },
                    { 82, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 25, "Đặt lịch tập thân trên buổi 2", "Pending", "17:30 - 19:00", 1 },
                    { 100, new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 27, "Đặt lịch PT 1-1 mục tiêu tăng 4kg cơ", "Pending", "17:00 - 18:30", 1 }
                });

            migrationBuilder.InsertData(
                table: "ClassBookings",
                columns: new[] { "ClassBookingId", "BookingDate", "ClassId", "MemberId", "Notes", "Status" },
                values: new object[,]
                {
                    { 11, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 25, "Đăng ký học Yoga buổi sáng.", "Confirmed" },
                    { 12, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 26, "Hội viên mang thảm cá nhân.", "Confirmed" },
                    { 13, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 27, "Giãn cơ phục hồi sau chạy bộ.", "Confirmed" },
                    { 14, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 28, "Đăng ký qua cổng hội viên.", "Confirmed" },
                    { 15, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 29, "Tập trung cải thiện nhịp thở.", "Confirmed" },
                    { 16, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 30, "Đăng ký lớp HLV Elena.", "Confirmed" },
                    { 17, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 31, "Đăng ký giữ chỗ trước.", "Confirmed" },
                    { 18, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 32, "Giữ chỗ lớp sáng.", "Confirmed" },
                    { 19, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 33, "Giảm căng cứng vai gáy.", "Confirmed" },
                    { 20, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 34, "Tham gia cùng nhóm bạn.", "Confirmed" },
                    { 21, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 35, "Đăng ký tại quầy lễ tân.", "Confirmed" },
                    { 22, new DateTime(2026, 11, 13, 6, 30, 0, 0, DateTimeKind.Unspecified), 1, 36, "Đăng ký qua website.", "Confirmed" },
                    { 23, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 25, "Đạp xe đốt mỡ tối thứ 5.", "Confirmed" },
                    { 24, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 26, "Đăng ký giữ xe hàng đầu.", "Confirmed" },
                    { 25, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 27, "Lớp Cardio cùng HLV Marcus.", "Confirmed" },
                    { 26, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 28, "Chuẩn bị sẵn bình nước điện giải.", "Confirmed" },
                    { 27, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 29, "Mục tiêu đốt 700 kcal.", "Confirmed" },
                    { 28, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 30, "Đăng ký online.", "Confirmed" },
                    { 29, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 31, "Đăng ký qua cổng hội viên.", "Confirmed" },
                    { 30, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 32, "Tăng sức bền tim mạch.", "Confirmed" },
                    { 31, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 33, "Giữ chỗ lớp tối.", "Confirmed" },
                    { 32, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 34, "Tập luyện theo nhịp nhạc RPM.", "Confirmed" },
                    { 33, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 35, "Đặt chỗ tại quầy.", "Confirmed" },
                    { 34, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 36, "Đăng ký trực tuyến.", "Confirmed" },
                    { 35, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 37, "Tham gia đều 3 buổi/tuần.", "Confirmed" },
                    { 36, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 38, "Giữ chỗ thành công.", "Confirmed" },
                    { 37, new DateTime(2026, 11, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, 39, "Tập cardio giảm mỡ.", "Confirmed" },
                    { 45, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 25, "Giữ chỗ lớp BodyPump tối.", "Confirmed" },
                    { 46, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 26, "Đặt chỗ thành công.", "Confirmed" },
                    { 47, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 27, "Đăng ký tại quầy lễ tân.", "Confirmed" },
                    { 48, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 28, "Đăng ký online.", "Confirmed" },
                    { 49, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 29, "Tập theo giáo án sức bền.", "Confirmed" },
                    { 50, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 30, "Xác nhận tham gia.", "Confirmed" },
                    { 51, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 31, "Đã đăng ký trước 3 ngày.", "Confirmed" },
                    { 52, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 32, "Tập tạ nhóm buổi tối.", "Confirmed" },
                    { 53, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 33, "Giữ chỗ lớp tối.", "Confirmed" },
                    { 54, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 34, "Đăng ký cùng bạn tập.", "Confirmed" },
                    { 55, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 35, "Đặt chỗ trực tuyến.", "Confirmed" },
                    { 56, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 36, "Xác nhận giữ chỗ.", "Confirmed" },
                    { 57, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 37, "Đặt trước qua App/Web.", "Confirmed" },
                    { 58, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 38, "Đã xác nhận slot.", "Confirmed" },
                    { 59, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 39, "Sẵn sàng buổi BodyPump.", "Confirmed" },
                    { 60, new DateTime(2026, 11, 13, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, 40, "Slot cuối cùng của lớp BodyPump.", "Confirmed" },
                    { 65, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 41, "Đăng ký qua lễ tân.", "Confirmed" },
                    { 66, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 42, "Học kỹ thuật tự vệ cơ bản.", "Confirmed" },
                    { 67, new DateTime(2026, 11, 14, 19, 30, 0, 0, DateTimeKind.Unspecified), 4, 43, "Đặt chỗ thành công.", "Confirmed" },
                    { 70, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 26, "Cải thiện độ linh hoạt lưng dưới.", "Confirmed" },
                    { 71, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 28, "Đăng ký lớp cuối tuần.", "Confirmed" },
                    { 72, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 30, "Giữ chỗ máy Reformer số 5.", "Confirmed" },
                    { 73, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 32, "Tập săn chắc cơ bụng Core.", "Confirmed" },
                    { 74, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 34, "Đăng ký online thành công.", "Confirmed" },
                    { 75, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 36, "Lớp trị liệu tư thế sáng cuối tuần.", "Confirmed" },
                    { 76, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 38, "Đã xác nhận slot.", "Confirmed" },
                    { 77, new DateTime(2026, 11, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 5, 40, "Slot cuối cùng của lớp Pilates.", "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "MemberId", "Notes", "OrderDate", "PaymentMethod", "PhoneNumber", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 11, 25, "Áo thun GymTech + Hộp bánh Quest Bar.", new DateTime(2026, 3, 24, 19, 10, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654330", "Mua trực tiếp tại quầy POS", "Hoàn thành", 950000m, 25 },
                    { 13, 27, "Rule 1 Isolate + Creatine Platinum.", new DateTime(2026, 4, 25, 17, 40, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654332", "Mua trực tiếp tại quầy POS", "Đã thanh toán", 2470000m, 27 },
                    { 15, 31, "Đai lưng Lever Belt + Găng tay Harbinger.", new DateTime(2026, 5, 28, 15, 50, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654336", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1240000m, 31 },
                    { 17, 29, "Rule 1 Isolate + Áo thun GymTech.", new DateTime(2026, 6, 21, 18, 30, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654334", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2150000m, 29 },
                    { 19, 33, "Pre-Workout C4 + Bình lắc BlenderBottle.", new DateTime(2026, 7, 29, 19, 15, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654338", "Mua trực tiếp tại quầy POS", "Đã thanh toán", 1070000m, 33 },
                    { 21, 30, "2 hộp bánh Protein Quest Bar tại quầy.", new DateTime(2026, 8, 22, 10, 25, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654335", "Mua trực tiếp tại quầy POS", "Hoàn thành", 1380000m, 30 },
                    { 23, 37, "Whey Gold + Găng tay Harbinger.", new DateTime(2026, 9, 26, 18, 40, 0, 0, DateTimeKind.Unspecified), "Tiền mặt", "0987654342", "Mua trực tiếp tại quầy POS", "Hoàn thành", 2140000m, 37 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 19, 11, 9, 1, 260000m, 260000m },
                    { 20, 11, 10, 1, 690000m, 690000m },
                    { 23, 13, 2, 1, 1890000m, 1890000m },
                    { 24, 13, 3, 1, 580000m, 580000m },
                    { 27, 15, 6, 1, 850000m, 850000m },
                    { 28, 15, 7, 1, 390000m, 390000m },
                    { 31, 17, 2, 1, 1890000m, 1890000m },
                    { 32, 17, 9, 1, 260000m, 260000m },
                    { 35, 19, 4, 1, 790000m, 790000m },
                    { 36, 19, 8, 1, 280000m, 280000m },
                    { 39, 21, 10, 2, 1380000m, 690000m },
                    { 42, 23, 1, 1, 1750000m, 1750000m },
                    { 43, 23, 7, 1, 390000m, 390000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MaintenanceLogs",
                keyColumn: "LogId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 37);

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CardCode",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Members");

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 8,
                column: "BookingDate",
                value: new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 9,
                column: "BookingDate",
                value: new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 10,
                column: "BookingDate",
                value: new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 29,
                column: "BookingDate",
                value: new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "BookingId",
                keyValue: 33,
                column: "BookingDate",
                value: new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 5,
                columns: new[] { "BookingDate", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 11, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), "Mức tạ gánh 25kg mỗi bên.", "Attended" });

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 6,
                columns: new[] { "BookingDate", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 11, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), "Tập rất sung sức.", "Attended" });

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 7,
                column: "BookingDate",
                value: new DateTime(2026, 11, 5, 19, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 8,
                column: "BookingDate",
                value: new DateTime(2026, 11, 7, 8, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ClassBookings",
                keyColumn: "ClassBookingId",
                keyValue: 9,
                column: "BookingDate",
                value: new DateTime(2026, 11, 7, 8, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 1,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 15, "~/images/classes/class-yoga.jpg", 3 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 2,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 20, "~/images/classes/class-spinning.jpg", 2 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 3,
                columns: new[] { "ImageUrl", "TrainerId" },
                values: new object[] { "~/images/classes/class-bodypump.jpg", 1 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 4,
                columns: new[] { "CurrentCapacity", "ImageUrl", "TrainerId" },
                values: new object[] { 10, "~/images/classes/class-kickfit.jpg", 2 });

            migrationBuilder.UpdateData(
                table: "GymClasses",
                keyColumn: "ClassId",
                keyValue: 5,
                columns: new[] { "ImageUrl", "TrainerId" },
                values: new object[] { "~/images/classes/class-pilates.jpg", 3 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "ImageUrl",
                value: "~/images/products/gymtech-shirt.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "ImageUrl",
                value: "~/images/products/quest-bar.jpg");
        }
    }
}
