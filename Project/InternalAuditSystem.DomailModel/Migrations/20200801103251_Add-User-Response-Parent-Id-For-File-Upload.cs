using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddUserResponseParentIdForFileUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentFileUploadUserResponseId",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_ParentFileUploadUserResponseId",
                table: "UserResponse",
                column: "ParentFileUploadUserResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_UserResponse_ParentFileUploadUserResponseId",
                table: "UserResponse",
                column: "ParentFileUploadUserResponseId",
                principalTable: "UserResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_UserResponse_ParentFileUploadUserResponseId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_ParentFileUploadUserResponseId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "ParentFileUploadUserResponseId",
                table: "UserResponse");
        }
    }
}
