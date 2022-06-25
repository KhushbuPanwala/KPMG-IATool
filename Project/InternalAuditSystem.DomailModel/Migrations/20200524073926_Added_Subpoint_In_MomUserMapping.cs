using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_Subpoint_In_MomUserMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubPointOfDiscussionId",
                table: "MomUserMapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_SubPointOfDiscussionId",
                table: "MomUserMapping",
                column: "SubPointOfDiscussionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_SubPointOfDiscussion_SubPointOfDiscussionId",
                table: "MomUserMapping",
                column: "SubPointOfDiscussionId",
                principalTable: "SubPointOfDiscussion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_SubPointOfDiscussion_SubPointOfDiscussionId",
                table: "MomUserMapping");

            migrationBuilder.DropIndex(
                name: "IX_MomUserMapping_SubPointOfDiscussionId",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "SubPointOfDiscussionId",
                table: "MomUserMapping");
        }
    }
}
