using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_ReportObservation_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfObservation",
                table: "ReportObservation");

            migrationBuilder.AddColumn<int>(
                name: "ObservationType",
                table: "ReportObservation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObservationType",
                table: "ReportObservation");

            migrationBuilder.AddColumn<int>(
                name: "TypeOfObservation",
                table: "ReportObservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
