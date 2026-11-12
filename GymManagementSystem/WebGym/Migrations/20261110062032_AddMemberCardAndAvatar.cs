using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberCardAndAvatar : Migration
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
