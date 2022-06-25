using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_CategoryId_Observation_ReportObservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_ObservationCategory_ObservationCategoryId",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_ObservationCategory_ObservationCategoryId",
                table: "ReportObservation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ObservationCategoryId",
                table: "ReportObservation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ObservationCategoryId",
                table: "Observation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_ObservationCategory_ObservationCategoryId",
                table: "Observation",
                column: "ObservationCategoryId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_ObservationCategory_ObservationCategoryId",
                table: "ReportObservation",
                column: "ObservationCategoryId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Observation_ObservationCategory_ObservationCategoryId",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_ObservationCategory_ObservationCategoryId",
                table: "ReportObservation");

            migrationBuilder.AlterColumn<Guid>(
                name: "ObservationCategoryId",
                table: "ReportObservation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ObservationCategoryId",
                table: "Observation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_ObservationCategory_ObservationCategoryId",
                table: "Observation",
                column: "ObservationCategoryId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_ObservationCategory_ObservationCategoryId",
                table: "ReportObservation",
                column: "ObservationCategoryId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
