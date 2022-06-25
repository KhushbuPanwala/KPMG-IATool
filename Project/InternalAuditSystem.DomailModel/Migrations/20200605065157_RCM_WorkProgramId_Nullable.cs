using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class RCM_WorkProgramId_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_WorkProgram_WorkProgramId",
                table: "RiskControlMatrix");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkProgramId",
                table: "RiskControlMatrix",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_WorkProgram_WorkProgramId",
                table: "RiskControlMatrix",
                column: "WorkProgramId",
                principalTable: "WorkProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_WorkProgram_WorkProgramId",
                table: "RiskControlMatrix");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkProgramId",
                table: "RiskControlMatrix",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_WorkProgram_WorkProgramId",
                table: "RiskControlMatrix",
                column: "WorkProgramId",
                principalTable: "WorkProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
