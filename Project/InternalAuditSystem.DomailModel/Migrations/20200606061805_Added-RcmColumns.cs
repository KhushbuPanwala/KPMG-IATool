using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddedRcmColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskCategory",
                table: "RiskControlMatrix",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestSteps",
                table: "RiskControlMatrix",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskCategory",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "TestSteps",
                table: "RiskControlMatrix");
        }
    }
}
