using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainerAvatars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 1,
                column: "AvatarUrl",
                value: "~/images/coach1.jpg");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 2,
                column: "AvatarUrl",
                value: "~/images/coach2.jpg");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 3,
                column: "AvatarUrl",
                value: "~/images/coach3.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 1,
                column: "AvatarUrl",
                value: "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=300");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 2,
                column: "AvatarUrl",
                value: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300");

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerId",
                keyValue: 3,
                column: "AvatarUrl",
                value: "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=300");
        }
    }
}
