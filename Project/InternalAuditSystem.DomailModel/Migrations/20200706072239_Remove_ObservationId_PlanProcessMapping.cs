using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_ObservationId_PlanProcessMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_Observation_ObservationId",
                table: "PlanProcessMapping");

            migrationBuilder.DropIndex(
                name: "IX_PlanProcessMapping_ObservationId",
                table: "PlanProcessMapping");

            migrationBuilder.DropIndex(
                name: "IX_Observation_SubProcessId",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "ObservationId",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "SubProcessId",
                table: "Observation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ObservationId",
                table: "PlanProcessMapping",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubProcessId",
                table: "Observation",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_ObservationId",
                table: "PlanProcessMapping",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_SubProcessId",
                table: "Observation",
                column: "SubProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation",
                column: "SubProcessId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_Observation_ObservationId",
                table: "PlanProcessMapping",
                column: "ObservationId",
                principalTable: "Observation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
