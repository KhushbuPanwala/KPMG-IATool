using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_AcmReviewer_AcmReviewerDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcmReportDetailId",
                table: "ACMReviewerDocument",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcmReviewerId",
                table: "ACMReviewerDocument",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_AcmReportDetailId",
                table: "ACMReviewerDocument",
                column: "AcmReportDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_AcmReviewerId",
                table: "ACMReviewerDocument",
                column: "AcmReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportDetail_AcmReportDetailId",
                table: "ACMReviewerDocument",
                column: "AcmReportDetailId",
                principalTable: "ACMReportDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReviewerDocument_ACMReviewer_AcmReviewerId",
                table: "ACMReviewerDocument",
                column: "AcmReviewerId",
                principalTable: "ACMReviewer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportDetail_AcmReportDetailId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReviewerDocument_ACMReviewer_AcmReviewerId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMReviewerDocument_AcmReportDetailId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMReviewerDocument_AcmReviewerId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropColumn(
                name: "AcmReportDetailId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropColumn(
                name: "AcmReviewerId",
                table: "ACMReviewerDocument");
        }
    }
}
