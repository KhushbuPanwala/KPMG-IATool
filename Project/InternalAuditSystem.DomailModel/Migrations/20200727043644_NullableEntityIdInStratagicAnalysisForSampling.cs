using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class NullableEntityIdInStratagicAnalysisForSampling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_AuditableEntity_AuditableEntityId",
                table: "StrategyAnalysis");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditableEntityId",
                table: "StrategyAnalysis",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_AuditableEntity_AuditableEntityId",
                table: "StrategyAnalysis",
                column: "AuditableEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_AuditableEntity_AuditableEntityId",
                table: "StrategyAnalysis");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditableEntityId",
                table: "StrategyAnalysis",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_AuditableEntity_AuditableEntityId",
                table: "StrategyAnalysis",
                column: "AuditableEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
