using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Add_Nullable_ACMPresentationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument");

            migrationBuilder.AlterColumn<Guid>(
                name: "ACMReportMappingId",
                table: "ACMDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ACMPresentationId",
                table: "ACMDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument",
                column: "ACMReportMappingId",
                principalTable: "ACMReportMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument");

            migrationBuilder.AlterColumn<Guid>(
                name: "ACMReportMappingId",
                table: "ACMDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ACMPresentationId",
                table: "ACMDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMReportMapping_ACMReportMappingId",
                table: "ACMDocument",
                column: "ACMReportMappingId",
                principalTable: "ACMReportMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
