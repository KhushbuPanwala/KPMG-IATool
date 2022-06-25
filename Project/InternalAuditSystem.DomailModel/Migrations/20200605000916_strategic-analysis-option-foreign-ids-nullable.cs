using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class strategicanalysisoptionforeignidsnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_CheckboxQuestion_CheckboxQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_DropdownQuestion_DropdownQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_MultipleChoiceQuestion_MultipleChoiceQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_RatingScaleQuestion_RatingQuestionId",
                table: "Option");

            migrationBuilder.AlterColumn<Guid>(
                name: "RatingQuestionId",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "MultipleChoiceQuestionId",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "DropdownQuestionId",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CheckboxQuestionId",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_CheckboxQuestion_CheckboxQuestionId",
                table: "Option",
                column: "CheckboxQuestionId",
                principalTable: "CheckboxQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_DropdownQuestion_DropdownQuestionId",
                table: "Option",
                column: "DropdownQuestionId",
                principalTable: "DropdownQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_MultipleChoiceQuestion_MultipleChoiceQuestionId",
                table: "Option",
                column: "MultipleChoiceQuestionId",
                principalTable: "MultipleChoiceQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_RatingScaleQuestion_RatingQuestionId",
                table: "Option",
                column: "RatingQuestionId",
                principalTable: "RatingScaleQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_CheckboxQuestion_CheckboxQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_DropdownQuestion_DropdownQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_MultipleChoiceQuestion_MultipleChoiceQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_RatingScaleQuestion_RatingQuestionId",
                table: "Option");

            migrationBuilder.AlterColumn<Guid>(
                name: "RatingQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MultipleChoiceQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DropdownQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CheckboxQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_CheckboxQuestion_CheckboxQuestionId",
                table: "Option",
                column: "CheckboxQuestionId",
                principalTable: "CheckboxQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_DropdownQuestion_DropdownQuestionId",
                table: "Option",
                column: "DropdownQuestionId",
                principalTable: "DropdownQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_MultipleChoiceQuestion_MultipleChoiceQuestionId",
                table: "Option",
                column: "MultipleChoiceQuestionId",
                principalTable: "MultipleChoiceQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_RatingScaleQuestion_RatingQuestionId",
                table: "Option",
                column: "RatingQuestionId",
                principalTable: "RatingScaleQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
