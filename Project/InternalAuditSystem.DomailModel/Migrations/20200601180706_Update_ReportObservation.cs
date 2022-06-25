using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObasevationRating",
                table: "ReportObservation");

            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "ReportObservation",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_RatingId",
                table: "ReportObservation",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_Rating_RatingId",
                table: "ReportObservation",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_Rating_RatingId",
                table: "ReportObservation");

            migrationBuilder.DropIndex(
                name: "IX_ReportObservation_RatingId",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "ReportObservation");

            migrationBuilder.AddColumn<int>(
                name: "ObasevationRating",
                table: "ReportObservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
