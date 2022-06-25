using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Updatefieldsinacmmoduletables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMDocument_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ACMReportMapping",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ACMReportMappingId",
                table: "ACMDocument",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "ACMDocument",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_ACMReportMappingId",
                table: "ACMDocument",
                column: "ACMReportMappingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument",
                column: "ACMReportMappingId",
                principalTable: "ACMReportMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMDocument_ACMReportMappingId",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ACMReportMappingId",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "ACMDocument");

            migrationBuilder.AddColumn<Guid>(
                name: "ACMPresentationId",
                table: "ACMDocument",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
