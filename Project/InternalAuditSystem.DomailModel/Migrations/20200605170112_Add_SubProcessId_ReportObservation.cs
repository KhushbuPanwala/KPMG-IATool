using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_SubProcessId_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubProcessId",
                table: "ReportObservation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_SubProcessId",
                table: "ReportObservation",
                column: "SubProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_Process_SubProcessId",
                table: "ReportObservation",
                column: "SubProcessId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_Process_SubProcessId",
                table: "ReportObservation");

            migrationBuilder.DropIndex(
                name: "IX_ReportObservation_SubProcessId",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "SubProcessId",
                table: "ReportObservation");
        }
    }
}
