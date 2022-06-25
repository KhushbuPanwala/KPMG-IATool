using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddRelatedEntityForgienKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationMapping_RelatedEntityId",
                table: "EntityRelationMapping",
                column: "RelatedEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityRelationMapping_AuditableEntity_RelatedEntityId",
                table: "EntityRelationMapping",
                column: "RelatedEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityRelationMapping_AuditableEntity_RelatedEntityId",
                table: "EntityRelationMapping");

            migrationBuilder.DropIndex(
                name: "IX_EntityRelationMapping_RelatedEntityId",
                table: "EntityRelationMapping");
        }
    }
}
