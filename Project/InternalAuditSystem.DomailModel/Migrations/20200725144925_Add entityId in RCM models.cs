using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddentityIdinRCMmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "RiskControlMatrixSubProcess",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "RiskControlMatrixSector",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "RiskControlMatrixProcess",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "RiskControlMatrix",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSubProcess_EntityId",
                table: "RiskControlMatrixSubProcess",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSector_EntityId",
                table: "RiskControlMatrixSector",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixProcess_EntityId",
                table: "RiskControlMatrixProcess",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_EntityId",
                table: "RiskControlMatrix",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_AuditableEntity_EntityId",
                table: "RiskControlMatrix",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixProcess_AuditableEntity_EntityId",
                table: "RiskControlMatrixProcess",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSector_AuditableEntity_EntityId",
                table: "RiskControlMatrixSector",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSubProcess_AuditableEntity_EntityId",
                table: "RiskControlMatrixSubProcess",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_AuditableEntity_EntityId",
                table: "RiskControlMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixProcess_AuditableEntity_EntityId",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSector_AuditableEntity_EntityId",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSubProcess_AuditableEntity_EntityId",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrixSubProcess_EntityId",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrixSector_EntityId",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrixProcess_EntityId",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrix_EntityId",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "RiskControlMatrix");
        }
    }
}
