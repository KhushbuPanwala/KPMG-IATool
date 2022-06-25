using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RemoveDocumentTypeReportObservationDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentFor",
                table: "ReportObservationDocument");

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "ReportObservationDocument",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "ReportObservationDocument");

            migrationBuilder.AddColumn<int>(
                name: "DocumentFor",
                table: "ReportObservationDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
