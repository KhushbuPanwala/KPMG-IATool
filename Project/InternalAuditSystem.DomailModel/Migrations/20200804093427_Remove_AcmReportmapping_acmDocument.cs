using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_AcmReportmapping_acmDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMReviewerDocument_ACMReportMappingId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropColumn(
                name: "ACMReportMappingId",
                table: "ACMReviewerDocument");

            migrationBuilder.DropColumn(
                name: "ReportUserMappingId",
                table: "ACMReviewerDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ACMReportMappingId",
                table: "ACMReviewerDocument",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportUserMappingId",
                table: "ACMReviewerDocument",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_ACMReportMappingId",
                table: "ACMReviewerDocument",
                column: "ACMReportMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReviewerDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMReviewerDocument",
                column: "ACMReportMappingId",
                principalTable: "ACMReportMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
