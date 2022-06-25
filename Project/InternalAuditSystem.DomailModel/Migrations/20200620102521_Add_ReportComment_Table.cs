using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_ReportComment_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "ReportObservationUserType",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Report");

            migrationBuilder.CreateTable(
                name: "ReportComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportComment_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportComment_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportComment_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportObservationReviewer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReportObservationId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportObservationReviewer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportObservationReviewer_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportObservationReviewer_ReportObservation_ReportObservati~",
                        column: x => x.ReportObservationId,
                        principalTable: "ReportObservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationReviewer_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportObservationReviewer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportComment_CreatedBy",
                table: "ReportComment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComment_ReportId",
                table: "ReportComment",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComment_UpdatedBy",
                table: "ReportComment",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationReviewer_CreatedBy",
                table: "ReportObservationReviewer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationReviewer_ReportObservationId",
                table: "ReportObservationReviewer",
                column: "ReportObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationReviewer_UpdatedBy",
                table: "ReportObservationReviewer",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationReviewer_UserId",
                table: "ReportObservationReviewer",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportComment");

            migrationBuilder.DropTable(
                name: "ReportObservationReviewer");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ReportObservationMember",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportObservationUserType",
                table: "ReportObservationMember",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Report",
                type: "text",
                nullable: true);
        }
    }
}
