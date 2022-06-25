using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddSamplingResponseStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_UserId",
                table: "UserResponse");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "SamplingResponseStatus",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_UserId",
                table: "UserResponse",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_UserId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "SamplingResponseStatus",
                table: "UserResponse");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserResponse",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_UserId",
                table: "UserResponse",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
