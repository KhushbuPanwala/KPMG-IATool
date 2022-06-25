using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RiskControlMatrix_AddEnumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlCategory",
                table: "RiskControlMatrix",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlType",
                table: "RiskControlMatrix",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NatureOfControl",
                table: "RiskControlMatrix",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlCategory",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "NatureOfControl",
                table: "RiskControlMatrix");
        }
    }
}
