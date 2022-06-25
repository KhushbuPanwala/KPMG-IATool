using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RemoveacmreportrelatedfieldsfromACMPresentationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_Report_ReportId",
                table: "ACMPresentation");

            migrationBuilder.DropIndex(
                name: "IX_ACMPresentation_ReportId",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "ACMReportStatus",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "ACMReportTitle",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "ACMPresentation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ACMReportStatus",
                table: "ACMPresentation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ACMReportTitle",
                table: "ACMPresentation",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "ACMPresentation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMPresentation_ReportId",
                table: "ACMPresentation",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_Report_ReportId",
                table: "ACMPresentation",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
