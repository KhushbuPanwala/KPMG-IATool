using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_AcmReportDetail_AcmReviewerDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportDetail_AcmReportDetailId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMReviewerDocument_AcmReportDetailId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropColumn(
                name: "AcmReportDetailId",
                table: "ACMReviewerDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcmReportDetailId",
                table: "ACMReviewerDocument",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_AcmReportDetailId",
                table: "ACMReviewerDocument",
                column: "AcmReportDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportDetail_AcmReportDetailId",
                table: "ACMReviewerDocument",
                column: "AcmReportDetailId",
                principalTable: "ACMReportDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
