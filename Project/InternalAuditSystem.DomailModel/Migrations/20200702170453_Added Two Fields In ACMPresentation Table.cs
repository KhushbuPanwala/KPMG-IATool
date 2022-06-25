using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddedTwoFieldsInACMPresentationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagementResponse",
                table: "ACMPresentation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ACMPresentation",
                nullable: true);

            migrationBuilder.DropForeignKey(
               name: "FK_ACMPresentation_Report_ReportId",
               table: "ACMPresentation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ACMPresentation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_Report_ReportId",
                table: "ACMPresentation",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagementResponse",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ACMPresentation");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_Report_ReportId",
                table: "ACMPresentation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ACMPresentation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_Report_ReportId",
                table: "ACMPresentation",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
