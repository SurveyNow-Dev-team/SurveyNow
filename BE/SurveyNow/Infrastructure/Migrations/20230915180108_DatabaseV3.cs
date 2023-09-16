using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointHistories_PointPurchases_PointPurchaseId",
                table: "PointHistories");

            migrationBuilder.DropTable(
                name: "PointPurchases");

            migrationBuilder.DropColumn(
                name: "Account",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "DestinationAccount",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "IsUsePoint",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "PurchaseCode",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PackPurchases");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PointHistories",
                newName: "PointHistoryType");

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "PointHistories",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Point",
                table: "PackPurchases",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    ExpertParticipant = table.Column<bool>(type: "bit", nullable: false),
                    SurveyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criteria_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<decimal>(type: "decimal(6,1)", precision: 6, scale: 1, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "varchar(20)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    SourceAccount = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    DestinationAccount = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    PurchaseCode = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AreaCriterion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    CriterionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCriterion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaCriterion_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AreaCriterion_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldCriterion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<long>(type: "bigint", nullable: false),
                    CriterionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldCriterion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldCriterion_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FieldCriterion_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenderCriterion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CriterionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderCriterion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenderCriterion_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RelationshipCriterion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelationshipStatus = table.Column<int>(type: "int", nullable: false),
                    CriterionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipCriterion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipCriterion_Criteria_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criteria",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaCriterion_CriterionId",
                table: "AreaCriterion",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaCriterion_ProvinceId",
                table: "AreaCriterion",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_SurveyId",
                table: "Criteria",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCriterion_CriterionId",
                table: "FieldCriterion",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCriterion_FieldId",
                table: "FieldCriterion",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_GenderCriterion_CriterionId",
                table: "GenderCriterion",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipCriterion_CriterionId",
                table: "RelationshipCriterion",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointHistories_Transactions_PointPurchaseId",
                table: "PointHistories",
                column: "PointPurchaseId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointHistories_Transactions_PointPurchaseId",
                table: "PointHistories");

            migrationBuilder.DropTable(
                name: "AreaCriterion");

            migrationBuilder.DropTable(
                name: "FieldCriterion");

            migrationBuilder.DropTable(
                name: "GenderCriterion");

            migrationBuilder.DropTable(
                name: "RelationshipCriterion");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.RenameColumn(
                name: "PointHistoryType",
                table: "PointHistories",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "PointHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "PackPurchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1);

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "PackPurchases",
                type: "nvarchar(80)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "PackPurchases",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "PackPurchases",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationAccount",
                table: "PackPurchases",
                type: "nvarchar(80)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsePoint",
                table: "PackPurchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseCode",
                table: "PackPurchases",
                type: "nvarchar(80)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PackPurchases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PackPurchases",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PointPurchases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "varchar(20)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    DestinationAccount = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    PurchaseCode = table.Column<string>(type: "nvarchar(80)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", nullable: false)
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
                name: "IX_PointPurchases_UserId",
                table: "PointPurchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointHistories_PointPurchases_PointPurchaseId",
                table: "PointHistories",
                column: "PointPurchaseId",
                principalTable: "PointPurchases",
                principalColumn: "Id");
        }
    }
}
