using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Made_TotalBudgetedHours_Field_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalBudgetedHours",
                table: "AuditPlan",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalBudgetedHours",
                table: "AuditPlan",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
