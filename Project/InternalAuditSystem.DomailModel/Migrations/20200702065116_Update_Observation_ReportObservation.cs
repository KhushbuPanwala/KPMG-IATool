using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_Observation_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ReportObservation");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "ReportObservation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observations",
                table: "ReportObservation");

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ReportObservation",
                type: "text",
                nullable: true);
        }
    }
}
