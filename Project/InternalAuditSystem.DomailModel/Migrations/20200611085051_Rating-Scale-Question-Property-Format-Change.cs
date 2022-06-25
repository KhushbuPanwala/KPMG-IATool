using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RatingScaleQuestionPropertyFormatChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "resultSmileyList",
                table: "RatingScaleQuestion",
                newName: "ResultSmileyList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultSmileyList",
                table: "RatingScaleQuestion",
                newName: "resultSmileyList");
        }
    }
}
