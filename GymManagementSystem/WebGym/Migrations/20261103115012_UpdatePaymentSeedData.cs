using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Cash", "CSH20260510084512" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "VNPay", "VNP20260615143011" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 6,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Cash", "CSH20260901091122" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 7,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Cash", "CSH20260905161209" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 8,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "VNPay", "VNP20261006114530" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 9,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Cash", "CSH20261025190011" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 11,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Cash", "CSH20261015112233" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Momo", "MOM20260510084512" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "BankTransfer", "MBB20260615143011" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 6,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "BankTransfer", "TCB20260901091122" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 7,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Momo", "MOM20260905161209" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 8,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "BankTransfer", "VCB20261006114530" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 9,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Momo", "MOM20261025190011" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 11,
                columns: new[] { "PaymentMethod", "TransactionCode" },
                values: new object[] { "Momo", "MOM20261015112233" });
        }
    }
}
