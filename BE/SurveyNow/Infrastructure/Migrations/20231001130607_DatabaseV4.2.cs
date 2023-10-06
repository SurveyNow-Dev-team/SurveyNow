using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceAccount",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseCode",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationAccount",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "Surveys",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceAccount",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseCode",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestinationAccount",
                table: "Transactions",
                type: "nvarchar(80)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "Surveys",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1);
        }
    }
}
