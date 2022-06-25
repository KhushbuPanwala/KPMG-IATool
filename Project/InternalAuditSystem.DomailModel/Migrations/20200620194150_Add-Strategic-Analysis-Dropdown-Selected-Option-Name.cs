using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddStrategicAnalysisDropdownSelectedOptionName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "selectedDropdownOption",
                table: "UserResponse",
                newName: "SelectedDropdownOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SelectedDropdownOption",
                table: "UserResponse",
                newName: "selectedDropdownOption");
        }
    }
}
