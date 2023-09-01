using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddPaymentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_City_CityId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_District_DistrictId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Province_ProvinceId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_City_Province_ProvinceId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Address_AddressId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Province",
                table: "Province");

            migrationBuilder.DropPrimaryKey(
                name: "PK_District",
                table: "District");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Province",
                newName: "Provinces");

            migrationBuilder.RenameTable(
                name: "District",
                newName: "Districts");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_District_CityId",
                table: "Districts",
                newName: "IX_Districts_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_City_ProvinceId",
                table: "Cities",
                newName: "IX_Cities_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_ProvinceId",
                table: "Addresses",
                newName: "IX_Addresses_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_DistrictId",
                table: "Addresses",
                newName: "IX_Addresses_DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CityId",
                table: "Addresses",
                newName: "IX_Addresses_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Districts",
                table: "Districts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Hobbies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hobbies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackPurchases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    PackType = table.Column<int>(type: "int", nullable: false),
                    IsUsePoint = table.Column<bool>(type: "bit", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: true),
                    DestinationAccount = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    PurchaseCode = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    Currency = table.Column<string>(type: "varchar(20)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackPurchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointPurchases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    DestinationAccount = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    PurchaseCode = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointPurchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_UserId",
                table: "Hobbies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PackPurchases_UserId",
                table: "PackPurchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Type_UserId",
                table: "Payments",
                columns: new[] { "Type", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PointHistories_UserId",
                table: "PointHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PointPurchases_UserId",
                table: "PointPurchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Cities_CityId",
                table: "Addresses",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Districts_DistrictId",
                table: "Addresses",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceId",
                table: "Addresses",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Cities_CityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Districts_DistrictId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Hobbies");

            migrationBuilder.DropTable(
                name: "PackPurchases");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PointHistories");

            migrationBuilder.DropTable(
                name: "PointPurchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Districts",
                table: "Districts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Provinces",
                newName: "Province");

            migrationBuilder.RenameTable(
                name: "Districts",
                newName: "District");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_CityId",
                table: "District",
                newName: "IX_District_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceId",
                table: "City",
                newName: "IX_City_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_ProvinceId",
                table: "Address",
                newName: "IX_Address_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_DistrictId",
                table: "Address",
                newName: "IX_Address_DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CityId",
                table: "Address",
                newName: "IX_Address_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Province",
                table: "Province",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_District",
                table: "District",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_City_CityId",
                table: "Address",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_District_DistrictId",
                table: "Address",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Province_ProvinceId",
                table: "Address",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Province_ProvinceId",
                table: "City",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_District_City_CityId",
                table: "District",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Address_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
