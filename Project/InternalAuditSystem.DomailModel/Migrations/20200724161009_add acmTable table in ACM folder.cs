using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class addacmTabletableinACMfolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACMTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ACMId = table.Column<Guid>(nullable: false),
                    Table = table.Column<JsonDocument>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMTable_ACMPresentation_ACMId",
                        column: x => x.ACMId,
                        principalTable: "ACMPresentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMTable_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACMTable_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMTable_ACMId",
                table: "ACMTable",
                column: "ACMId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMTable_CreatedBy",
                table: "ACMTable",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMTable_UpdatedBy",
                table: "ACMTable",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACMTable");
        }
    }
}
