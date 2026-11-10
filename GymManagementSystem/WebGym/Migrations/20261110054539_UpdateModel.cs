using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Members",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardCode",
                table: "Members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "Members",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // Cập nhật dữ liệu cho các Member đã có
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CardCode",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Members");
        }
    }
}
