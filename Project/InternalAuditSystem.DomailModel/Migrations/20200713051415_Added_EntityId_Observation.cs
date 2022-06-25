using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_EntityId_Observation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Observation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Observation_EntityId",
                table: "Observation",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_AuditableEntity_EntityId",
                table: "Observation",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_AuditableEntity_EntityId",
                table: "Observation");

            migrationBuilder.DropIndex(
                name: "IX_Observation_EntityId",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Observation");
        }
    }
}
