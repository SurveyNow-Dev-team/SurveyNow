using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SurveyId",
                table: "PackPurchases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PackPurchases_SurveyId",
                table: "PackPurchases",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PackPurchases_Surveys_SurveyId",
                table: "PackPurchases",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackPurchases_Surveys_SurveyId",
                table: "PackPurchases");

            migrationBuilder.DropIndex(
                name: "IX_PackPurchases_SurveyId",
                table: "PackPurchases");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "PackPurchases");
        }
    }
}
