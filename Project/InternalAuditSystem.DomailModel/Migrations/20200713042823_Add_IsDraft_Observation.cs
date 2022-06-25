using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_IsDraft_Observation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Observation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Observation");
        }
    }
}
