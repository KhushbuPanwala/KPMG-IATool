using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_AuditPlan_Observation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuditPlanId",
                table: "Observation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observation_AuditPlanId",
                table: "Observation",
                column: "AuditPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_AuditPlan_AuditPlanId",
                table: "Observation",
                column: "AuditPlanId",
                principalTable: "AuditPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_AuditPlan_AuditPlanId",
                table: "Observation");

            migrationBuilder.DropIndex(
                name: "IX_Observation_AuditPlanId",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "AuditPlanId",
                table: "Observation");
        }
    }
}
