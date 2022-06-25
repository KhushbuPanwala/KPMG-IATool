using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_ParentPlanId_In_AuditPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentPlanId",
                table: "AuditPlan",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_ParentPlanId",
                table: "AuditPlan",
                column: "ParentPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_AuditPlan_ParentPlanId",
                table: "AuditPlan",
                column: "ParentPlanId",
                principalTable: "AuditPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditPlan_ParentPlanId",
                table: "AuditPlan");

            migrationBuilder.DropIndex(
                name: "IX_AuditPlan_ParentPlanId",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "ParentPlanId",
                table: "AuditPlan");
        }
    }
}
