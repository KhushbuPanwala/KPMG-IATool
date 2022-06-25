using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_ReportObservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportObservationTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportObservationId = table.Column<Guid>(nullable: false),
                    Table = table.Column<JsonDocument>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportObservationTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportObservationTable_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportObservationTable_ReportObservation_ReportObservationId",
                        column: x => x.ReportObservationId,
                        principalTable: "ReportObservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationTable_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationTable_CreatedBy",
                table: "ReportObservationTable",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationTable_ReportObservationId",
                table: "ReportObservationTable",
                column: "ReportObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationTable_UpdatedBy",
                table: "ReportObservationTable",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportObservationTable");
        }
    }
}
