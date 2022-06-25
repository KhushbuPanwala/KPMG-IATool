using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Rename_AcmuserTeam_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMUserTeam_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropTable(
                name: "ACMUserTeam");

            migrationBuilder.CreateTable(
                name: "ACMReviewer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ACMPresentationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMReviewer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMReviewer_ACMPresentation_ACMPresentationId",
                        column: x => x.ACMPresentationId,
                        principalTable: "ACMPresentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMReviewer_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReviewer_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMReviewer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewer_ACMPresentationId",
                table: "ACMReviewer",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewer_CreatedBy",
                table: "ACMReviewer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewer_UpdatedBy",
                table: "ACMReviewer",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReviewer_UserId",
                table: "ACMReviewer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMReviewer_UserId",
                table: "ACMReportMapping",
                column: "UserId",
                principalTable: "ACMReviewer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMReviewer_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropTable(
                name: "ACMReviewer");

            migrationBuilder.CreateTable(
                name: "ACMUserTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ACMPresentationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMUserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_ACMPresentation_ACMPresentationId",
                        column: x => x.ACMPresentationId,
                        principalTable: "ACMPresentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_ACMPresentationId",
                table: "ACMUserTeam",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_CreatedBy",
                table: "ACMUserTeam",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_UpdatedBy",
                table: "ACMUserTeam",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_UserId",
                table: "ACMUserTeam",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMUserTeam_UserId",
                table: "ACMReportMapping",
                column: "UserId",
                principalTable: "ACMUserTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
