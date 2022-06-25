using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Changed_PropertyName_In_PlanProcessMaping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PlanProcessMapping");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "PlanProcessMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "PlanProcessMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "PlanProcessMapping");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PlanProcessMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PlanProcessMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
