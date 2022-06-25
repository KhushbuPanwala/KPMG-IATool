using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Remove_ForeignKey_Relation_From_AuditPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditType_SelectedTypeId",
                table: "AuditPlan");

            migrationBuilder.DropIndex(
                name: "IX_AuditPlan_SelectCategoryId",
                table: "AuditPlan");

            migrationBuilder.DropIndex(
                name: "IX_AuditPlan_SelectedTypeId",
                table: "AuditPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_SelectCategoryId",
                table: "AuditPlan",
                column: "SelectCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_SelectedTypeId",
                table: "AuditPlan",
                column: "SelectedTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                table: "AuditPlan",
                column: "SelectCategoryId",
                principalTable: "AuditCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_AuditType_SelectedTypeId",
                table: "AuditPlan",
                column: "SelectedTypeId",
                principalTable: "AuditType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
