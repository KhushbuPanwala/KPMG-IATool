using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Rename_ReportId_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_Report_ReportId",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "ReprotId",
                table: "ReportObservation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ReportObservation",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_Report_ReportId",
                table: "ReportObservation",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_Report_ReportId",
                table: "ReportObservation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReportId",
                table: "ReportObservation",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ReprotId",
                table: "ReportObservation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_Report_ReportId",
                table: "ReportObservation",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
