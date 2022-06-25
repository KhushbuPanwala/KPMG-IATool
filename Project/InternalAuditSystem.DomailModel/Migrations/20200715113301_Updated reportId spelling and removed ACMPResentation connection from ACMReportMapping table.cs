using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class UpdatedreportIdspellingandremovedACMPResentationconnectionfromACMReportMappingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMPresentation_ACMPresentationId",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_ACMPresentationId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ACMPresentationId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ReprotId",
                table: "ACMReportMapping");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ACMReportMapping",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ACMPresentationId",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReprotId",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ACMPresentationId",
                table: "ACMReportMapping",
                column: "ACMPresentationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMPresentation_ACMPresentationId",
                table: "ACMReportMapping",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
