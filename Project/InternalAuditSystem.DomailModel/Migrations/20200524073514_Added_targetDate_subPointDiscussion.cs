using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_targetDate_subPointDiscussion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MomId",
                table: "SubPointOfDiscussion",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetDate",
                table: "SubPointOfDiscussion",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "MomDetail",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "MomEndTime",
                table: "MomDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SubPointOfDiscussion_MomId",
                table: "SubPointOfDiscussion",
                column: "MomId");

            migrationBuilder.CreateIndex(
                name: "IX_MomDetail_EntityId",
                table: "MomDetail",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MomDetail_AuditableEntity_EntityId",
                table: "MomDetail",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPointOfDiscussion_MomDetail_MomId",
                table: "SubPointOfDiscussion",
                column: "MomId",
                principalTable: "MomDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MomDetail_AuditableEntity_EntityId",
                table: "MomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPointOfDiscussion_MomDetail_MomId",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropIndex(
                name: "IX_SubPointOfDiscussion_MomId",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropIndex(
                name: "IX_MomDetail_EntityId",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "MomId",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "TargetDate",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "MomEndTime",
                table: "MomDetail");
        }
    }
}
