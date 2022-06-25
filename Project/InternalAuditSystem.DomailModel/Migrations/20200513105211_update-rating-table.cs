using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class updateratingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Legand",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "QuatitativeFactors",
                table: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "User",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Legend",
                table: "Rating",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantitativeFactors",
                table: "Rating",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Legend",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "QuantitativeFactors",
                table: "Rating");

            migrationBuilder.AlterColumn<int>(
                name: "Designation",
                table: "User",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Legand",
                table: "Rating",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuatitativeFactors",
                table: "Rating",
                type: "text",
                nullable: true);
        }
    }
}
