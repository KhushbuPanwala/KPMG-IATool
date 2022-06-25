using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Removed_MainPointDiscussion_From_MomUserMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_MainDiscussionPoint_PointOfDiscussionId",
                table: "MomUserMapping");

            migrationBuilder.DropIndex(
                name: "IX_MomUserMapping_PointOfDiscussionId",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "PointOfDiscussionId",
                table: "MomUserMapping");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PointOfDiscussionId",
                table: "MomUserMapping",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_PointOfDiscussionId",
                table: "MomUserMapping",
                column: "PointOfDiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_MainDiscussionPoint_PointOfDiscussionId",
                table: "MomUserMapping",
                column: "PointOfDiscussionId",
                principalTable: "MainDiscussionPoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
