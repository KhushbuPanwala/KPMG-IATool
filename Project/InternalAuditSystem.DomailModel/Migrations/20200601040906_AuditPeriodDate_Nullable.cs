using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AuditPeriodDate_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlCategory",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "NatureOfControl",
                table: "RiskControlMatrix");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditPeriodStartDate",
                table: "WorkProgram",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditPeriodEndDate",
                table: "WorkProgram",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditPeriodStartDate",
                table: "WorkProgram",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditPeriodEndDate",
                table: "WorkProgram",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControlCategory",
                table: "RiskControlMatrix",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControlType",
                table: "RiskControlMatrix",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NatureOfControl",
                table: "RiskControlMatrix",
                type: "text",
                nullable: true);
        }
    }
}
