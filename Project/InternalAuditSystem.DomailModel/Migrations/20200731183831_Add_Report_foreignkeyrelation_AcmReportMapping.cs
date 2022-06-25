using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_Report_foreignkeyrelation_AcmReportMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "ACMReportMapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ReportId",
                table: "ACMReportMapping",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "ACMReportMapping");
        }
    }
}
