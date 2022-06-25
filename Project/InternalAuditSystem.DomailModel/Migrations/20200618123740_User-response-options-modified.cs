using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Userresponseoptionsmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_UserResponse_OptionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_OptionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Option");

            migrationBuilder.AddColumn<Guid>(
                name: "OptionIds",
                table: "Option",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Option_OptionIds",
                table: "Option",
                column: "OptionIds");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_UserResponse_OptionIds",
                table: "Option",
                column: "OptionIds",
                principalTable: "UserResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_UserResponse_OptionIds",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_OptionIds",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "OptionIds",
                table: "Option");

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "Option",
                type: "uuid",
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
    }
}
