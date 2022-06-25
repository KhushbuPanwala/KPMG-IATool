using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_AuditableEntity_UserResponse_AnalysisTeam_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuditableEntityId",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AuditableEntityId",
                table: "StrategicAnalysisTeam",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_AuditableEntityId",
                table: "UserResponse",
                column: "AuditableEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_AuditableEntityId",
                table: "StrategicAnalysisTeam",
                column: "AuditableEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_AuditableEntity_AuditableEntityId",
                table: "StrategicAnalysisTeam",
                column: "AuditableEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_AuditableEntity_AuditableEntityId",
                table: "UserResponse",
                column: "AuditableEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_AuditableEntity_AuditableEntityId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_AuditableEntity_AuditableEntityId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_AuditableEntityId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_StrategicAnalysisTeam_AuditableEntityId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "AuditableEntityId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "AuditableEntityId",
                table: "StrategicAnalysisTeam");
        }
    }
}
