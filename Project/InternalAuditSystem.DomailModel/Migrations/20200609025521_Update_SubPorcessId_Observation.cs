using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_SubPorcessId_Observation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubProcessId",
                table: "Observation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation",
                column: "SubProcessId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubProcessId",
                table: "Observation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_Process_SubProcessId",
                table: "Observation",
                column: "SubProcessId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
