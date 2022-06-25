using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_Observation_in_PlanprocessMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ObservationId",
                table: "PlanProcessMapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_ObservationId",
                table: "PlanProcessMapping",
                column: "ObservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_Observation_ObservationId",
                table: "PlanProcessMapping",
                column: "ObservationId",
                principalTable: "Observation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_Observation_ObservationId",
                table: "PlanProcessMapping");

            migrationBuilder.DropIndex(
                name: "IX_PlanProcessMapping_ObservationId",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "ObservationId",
                table: "PlanProcessMapping");
        }
    }
}
