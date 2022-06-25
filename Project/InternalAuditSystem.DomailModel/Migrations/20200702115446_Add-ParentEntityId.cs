using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class AddParentEntityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentEntityId",
                table: "AuditableEntity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditableEntity_ParentEntityId",
                table: "AuditableEntity",
                column: "ParentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_AuditableEntity_ParentEntityId",
                table: "AuditableEntity",
                column: "ParentEntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditableEntity_ParentEntityId",
                table: "AuditableEntity");

            migrationBuilder.DropIndex(
                name: "IX_AuditableEntity_ParentEntityId",
                table: "AuditableEntity");

            migrationBuilder.DropColumn(
                name: "ParentEntityId",
                table: "AuditableEntity");
        }
    }
}
