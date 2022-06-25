using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_AcmReportDetail_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportTitle",
                table: "ACMReportMapping");

            migrationBuilder.AddColumn<Guid>(
                name: "AcmReportId",
                table: "ACMReviewer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcmReportId",
                table: "ACMReportMapping",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ACMReportDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportTitle = table.Column<string>(nullable: true),
                    AcmId = table.Column<Guid>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMReportDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMReportDetail_ACMPresentation_AcmId",
                        column: x => x.AcmId,
                        principalTable: "ACMPresentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReportDetail_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReportDetail_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReportDetail_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewer_AcmReportId",
                table: "ACMReviewer",
                column: "AcmReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_AcmReportId",
                table: "ACMReportMapping",
                column: "AcmReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportDetail_AcmId",
                table: "ACMReportDetail",
                column: "AcmId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportDetail_CreatedBy",
                table: "ACMReportDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportDetail_EntityId",
                table: "ACMReportDetail",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportDetail_UpdatedBy",
                table: "ACMReportDetail",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMReportDetail_AcmReportId",
                table: "ACMReportMapping",
                column: "AcmReportId",
                principalTable: "ACMReportDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReviewer_ACMReportDetail_AcmReportId",
                table: "ACMReviewer",
                column: "AcmReportId",
                principalTable: "ACMReportDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMReportDetail_AcmReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReviewer_ACMReportDetail_AcmReportId",
                table: "ACMReviewer");

            migrationBuilder.DropTable(
                name: "ACMReportDetail");

            migrationBuilder.DropIndex(
                name: "IX_ACMReviewer_AcmReportId",
                table: "ACMReviewer");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_AcmReportId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "AcmReportId",
                table: "ACMReviewer");

            migrationBuilder.DropColumn(
                name: "AcmReportId",
                table: "ACMReportMapping");

            migrationBuilder.AddColumn<string>(
                name: "ReportTitle",
                table: "ACMReportMapping",
                type: "text",
                nullable: true);
        }
    }
}
