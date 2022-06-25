using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_PlanFk_From_WorkProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgram_AuditPlan_AuditPlanId",
                table: "WorkProgram");

            migrationBuilder.DropIndex(
                name: "IX_WorkProgram_AuditPlanId",
                table: "WorkProgram");

            migrationBuilder.DropColumn(
                name: "AuditPlanId",
                table: "WorkProgram");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuditPlanId",
                table: "WorkProgram",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgram_AuditPlanId",
                table: "WorkProgram",
                column: "AuditPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgram_AuditPlan_AuditPlanId",
                table: "WorkProgram",
                column: "AuditPlanId",
                principalTable: "AuditPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
