using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_AuditorId_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuditorId",
                table: "ReportObservation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_AuditorId",
                table: "ReportObservation",
                column: "AuditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_User_AuditorId",
                table: "ReportObservation",
                column: "AuditorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_User_AuditorId",
                table: "ReportObservation");

            migrationBuilder.DropIndex(
                name: "IX_ReportObservation_AuditorId",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "AuditorId",
                table: "ReportObservation");
        }
    }
}
