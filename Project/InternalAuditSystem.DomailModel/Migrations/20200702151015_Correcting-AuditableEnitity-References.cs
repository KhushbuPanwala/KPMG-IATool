using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class CorrectingAuditableEnitityReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_EntityCategory_SelectedCategoryId",
                table: "AuditableEntity",
                column: "SelectedCategoryId",
                principalTable: "EntityCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_EntityType_SelectedTypeId",
                table: "AuditableEntity",
                column: "SelectedTypeId",
                principalTable: "EntityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_EntityCategory_SelectedCategoryId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_EntityType_SelectedTypeId",
                table: "AuditableEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity",
                column: "SelectedCategoryId",
                principalTable: "AuditCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity",
                column: "SelectedTypeId",
                principalTable: "AuditType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
