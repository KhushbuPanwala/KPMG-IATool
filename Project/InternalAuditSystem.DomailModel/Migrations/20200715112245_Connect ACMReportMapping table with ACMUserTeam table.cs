using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class ConnectACMReportMappingtablewithACMUserTeamtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ACMUserId",
                table: "ACMReportMapping",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ReportUserType",
                table: "ACMReportMapping",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ACMReportMapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_UserId",
                table: "ACMReportMapping",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMUserTeam_UserId",
                table: "ACMReportMapping",
                column: "UserId",
                principalTable: "ACMUserTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_ACMUserTeam_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropIndex(
                name: "IX_ACMReportMapping_UserId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ACMUserId",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "ReportUserType",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ACMReportMapping");
        }
    }
}
