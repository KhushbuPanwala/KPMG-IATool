using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddRcmIdinSampling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RiskControlMatrixId",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_RiskControlMatrixId",
                table: "UserResponse",
                column: "RiskControlMatrixId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_RiskControlMatrix_RiskControlMatrixId",
                table: "UserResponse",
                column: "RiskControlMatrixId",
                principalTable: "RiskControlMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_RiskControlMatrix_RiskControlMatrixId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_RiskControlMatrixId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "RiskControlMatrixId",
                table: "UserResponse");
        }
    }
}
