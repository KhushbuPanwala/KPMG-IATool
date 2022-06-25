using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_RiskName_Rcm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskName",
                table: "RiskControlMatrix");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskName",
                table: "RiskControlMatrix",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
