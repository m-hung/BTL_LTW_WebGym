using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAiPlanNutritionSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { 176, 42, 104, 1498, 2700 });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { 205, 45, 101, 1630, 2800 });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { 394, 78, 172, 2965, 3500 });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { 354, 72, 143, 2635, 3300 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CarbG", "FatG", "ProteinG", "TargetKcal", "WaterMl" },
                values: new object[] { null, null, null, null, null });
        }
    }
}
