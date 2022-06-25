using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RemoveDuplicateRCMProcessId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_RiskControlMatrixProcess_RCMProcessId",
                table: "RiskControlMatrix");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrix_RCMProcessId",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "RCMProcessId",
                table: "RiskControlMatrix");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_RcmProcessId",
                table: "RiskControlMatrix",
                column: "RcmProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_RiskControlMatrixProcess_RcmProcessId",
                table: "RiskControlMatrix",
                column: "RcmProcessId",
                principalTable: "RiskControlMatrixProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_RiskControlMatrixProcess_RcmProcessId",
                table: "RiskControlMatrix");

            migrationBuilder.DropIndex(
                name: "IX_RiskControlMatrix_RcmProcessId",
                table: "RiskControlMatrix");

            migrationBuilder.AddColumn<Guid>(
                name: "RCMProcessId",
                table: "RiskControlMatrix",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_RCMProcessId",
                table: "RiskControlMatrix",
                column: "RCMProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_RiskControlMatrixProcess_RCMProcessId",
                table: "RiskControlMatrix",
                column: "RCMProcessId",
                principalTable: "RiskControlMatrixProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
