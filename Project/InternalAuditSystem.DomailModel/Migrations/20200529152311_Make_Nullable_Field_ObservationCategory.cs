using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Make_Nullable_Field_ObservationCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_ObservationCategory_ParentId",
                table: "ObservationCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "ObservationCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_ObservationCategory_ParentId",
                table: "ObservationCategory",
                column: "ParentId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_ObservationCategory_ParentId",
                table: "ObservationCategory");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "ObservationCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_ObservationCategory_ParentId",
                table: "ObservationCategory",
                column: "ParentId",
                principalTable: "ObservationCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
