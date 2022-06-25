using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Updated_Start_And_EndDate_AuditPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AuditPlan");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "AuditPlan",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "AuditPlan",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "AuditPlan");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "AuditPlan",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AuditPlan",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
