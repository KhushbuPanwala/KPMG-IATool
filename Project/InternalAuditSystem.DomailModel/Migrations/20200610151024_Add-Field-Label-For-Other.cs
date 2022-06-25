using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddFieldLabelForOther : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldLabel",
                table: "MultipleChoiceQuestion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldLabel",
                table: "CheckboxQuestion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldLabel",
                table: "MultipleChoiceQuestion");

            migrationBuilder.DropColumn(
                name: "FieldLabel",
                table: "CheckboxQuestion");
        }
    }
}
