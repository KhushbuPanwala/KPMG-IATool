using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddRatingmasterfieldinACMPresentationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RatingId",
                table: "ACMPresentation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMPresentation_RatingId",
                table: "ACMPresentation",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_Rating_RatingId",
                table: "ACMPresentation",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_Rating_RatingId",
                table: "ACMPresentation");

            migrationBuilder.DropIndex(
                name: "IX_ACMPresentation_RatingId",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "ACMPresentation");
        }
    }
}
