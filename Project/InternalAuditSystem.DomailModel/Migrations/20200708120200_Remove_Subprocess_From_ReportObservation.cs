using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_Subprocess_From_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "AuditPlanId",
                table: "ReportObservation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_AuditPlanId",
                table: "ReportObservation",
                column: "AuditPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_AuditPlan_AuditPlanId",
                table: "ReportObservation",
                column: "AuditPlanId",
                principalTable: "AuditPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_AuditPlan_AuditPlanId",
                table: "ReportObservation");

            migrationBuilder.DropIndex(
                name: "IX_ReportObservation_AuditPlanId",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "AuditPlanId",
                table: "ReportObservation");

            migrationBuilder.AddColumn<Guid>(
                name: "SubProcessId",
                table: "ReportObservation",
                type: "uuid",
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
    }
}
