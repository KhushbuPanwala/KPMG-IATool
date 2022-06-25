using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class UserResponseOptionListAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_OptionId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "UserResponse");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "OptionIds",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "Option",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Option_OptionId",
                table: "Option",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_UserResponse_OptionId",
                table: "Option",
                column: "OptionId",
                principalTable: "UserResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_UserResponse_OptionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_OptionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "OptionIds",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Option");

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "UserResponse",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_OptionId",
                table: "UserResponse",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
