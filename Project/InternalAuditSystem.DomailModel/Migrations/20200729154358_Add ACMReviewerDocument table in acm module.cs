using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddACMReviewerDocumenttableinacmmodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_ReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "ACMReportMapping");

            migrationBuilder.AddColumn<Guid>(
                name: "ACMId",
                table: "ACMReportMapping",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ACMPresentationId",
                table: "ACMDocument",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ACMReviewerDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentName = table.Column<string>(nullable: true),
                    DocumentPath = table.Column<string>(nullable: true),
                    ReportUserMappingId = table.Column<Guid>(nullable: false),
                    ACMReportMappingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMReviewerDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMReviewerDocument_ACMReportMapping_ACMReportMappingId",
                        column: x => x.ACMReportMappingId,
                        principalTable: "ACMReportMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReviewerDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReviewerDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ACMId",
                table: "ACMReportMapping",
                column: "ACMId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_ACMReportMappingId",
                table: "ACMReviewerDocument",
                column: "ACMReportMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_CreatedBy",
                table: "ACMReviewerDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewerDocument_UpdatedBy",
                table: "ACMReviewerDocument",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMPresentation_ACMId",
                table: "ACMReportMapping",
                column: "ACMId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMPresentation_ACMId",
                table: "ACMReportMapping");

            migrationBuilder.DropTable(
                name: "ACMReviewerDocument");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_ACMId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMDocument_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "ACMId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ReportId",
                table: "ACMReportMapping",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
