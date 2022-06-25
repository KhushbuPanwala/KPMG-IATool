using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddUserResponseDetailAndEstimatedValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailsOfOpportunity",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "EstimatedValueOfOpportunity",
                table: "StrategyAnalysis");

            migrationBuilder.AddColumn<string>(
                name: "DetailsOfOpportunity",
                table: "UserResponse",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedValueOfOpportunity",
                table: "UserResponse",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDetailAndEstimatedValueOfOpportunity",
                table: "UserResponse",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailsOfOpportunity",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "EstimatedValueOfOpportunity",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "IsDetailAndEstimatedValueOfOpportunity",
                table: "UserResponse");

            migrationBuilder.AddColumn<string>(
                name: "DetailsOfOpportunity",
                table: "StrategyAnalysis",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedValueOfOpportunity",
                table: "StrategyAnalysis",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
