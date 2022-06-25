using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddedStrategicAnalysisIdInRCM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StrategicAnalysisId",
                table: "RiskControlMatrix",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_StrategicAnalysisId",
                table: "RiskControlMatrix",
                column: "StrategicAnalysisId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_StrategyAnalysis_StrategicAnalysisId",
                table: "RiskControlMatrix",
                column: "StrategicAnalysisId",
                principalTable: "StrategyAnalysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_StrategyAnalysis_StrategicAnalysisId",
                table: "RiskControlMatrix");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrix_StrategicAnalysisId",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "StrategicAnalysisId",
                table: "RiskControlMatrix");
        }
    }
}
