using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class remove_acmuserteam_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMReviewer_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ACMReportMapping");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_UserId",
                table: "ACMReportMapping",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMReviewer_UserId",
                table: "ACMReportMapping",
                column: "UserId",
                principalTable: "ACMReviewer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
