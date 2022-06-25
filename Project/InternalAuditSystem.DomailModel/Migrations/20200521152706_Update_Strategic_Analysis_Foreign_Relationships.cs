using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Update_Strategic_Analysis_Foreign_Relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckboxQuestion_Question_Id",
                table: "CheckboxQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_DropdownQuestion_Question_Id",
                table: "DropdownQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploadQuestion_Question_Id",
                table: "FileUploadQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceQuestion_Question_Id",
                table: "MultipleChoiceQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_FileUploadQuestion_FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_SubjectiveQuestion_SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_TextboxQuestion_TextboxQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingScaleQuestion_Question_Id",
                table: "RatingScaleQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectiveQuestion_Question_Id",
                table: "SubjectiveQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TextboxQuestion_Question_Id",
                table: "TextboxQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_Option_FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_TextboxQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "TextboxQuestionId",
                table: "Option");

            migrationBuilder.AlterColumn<Guid>(
                name: "OptionId",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "TextboxQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "SubjectiveQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "RatingScaleQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "MultipleChoiceQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "FileUploadQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "DropdownQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "CheckboxQuestion",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TextboxQuestion_QuestionId",
                table: "TextboxQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveQuestion_QuestionId",
                table: "SubjectiveQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RatingScaleQuestion_QuestionId",
                table: "RatingScaleQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestion_QuestionId",
                table: "MultipleChoiceQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadQuestion_QuestionId",
                table: "FileUploadQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DropdownQuestion_QuestionId",
                table: "DropdownQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxQuestion_QuestionId",
                table: "CheckboxQuestion",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckboxQuestion_Question_QuestionId",
                table: "CheckboxQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DropdownQuestion_Question_QuestionId",
                table: "DropdownQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploadQuestion_Question_QuestionId",
                table: "FileUploadQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceQuestion_Question_QuestionId",
                table: "MultipleChoiceQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingScaleQuestion_Question_QuestionId",
                table: "RatingScaleQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectiveQuestion_Question_QuestionId",
                table: "SubjectiveQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextboxQuestion_Question_QuestionId",
                table: "TextboxQuestion",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckboxQuestion_Question_QuestionId",
                table: "CheckboxQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_DropdownQuestion_Question_QuestionId",
                table: "DropdownQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploadQuestion_Question_QuestionId",
                table: "FileUploadQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceQuestion_Question_QuestionId",
                table: "MultipleChoiceQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingScaleQuestion_Question_QuestionId",
                table: "RatingScaleQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectiveQuestion_Question_QuestionId",
                table: "SubjectiveQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TextboxQuestion_Question_QuestionId",
                table: "TextboxQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_TextboxQuestion_QuestionId",
                table: "TextboxQuestion");

            migrationBuilder.DropIndex(
                name: "IX_SubjectiveQuestion_QuestionId",
                table: "SubjectiveQuestion");

            migrationBuilder.DropIndex(
                name: "IX_RatingScaleQuestion_QuestionId",
                table: "RatingScaleQuestion");

            migrationBuilder.DropIndex(
                name: "IX_MultipleChoiceQuestion_QuestionId",
                table: "MultipleChoiceQuestion");

            migrationBuilder.DropIndex(
                name: "IX_FileUploadQuestion_QuestionId",
                table: "FileUploadQuestion");

            migrationBuilder.DropIndex(
                name: "IX_DropdownQuestion_QuestionId",
                table: "DropdownQuestion");

            migrationBuilder.DropIndex(
                name: "IX_CheckboxQuestion_QuestionId",
                table: "CheckboxQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "TextboxQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "SubjectiveQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "RatingScaleQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "MultipleChoiceQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "FileUploadQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "DropdownQuestion");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "CheckboxQuestion");

            migrationBuilder.AlterColumn<Guid>(
                name: "OptionId",
                table: "UserResponse",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectiveQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TextboxQuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Option_FileUploadQuestionId",
                table: "Option",
                column: "FileUploadQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_SubjectiveQuestionId",
                table: "Option",
                column: "SubjectiveQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_TextboxQuestionId",
                table: "Option",
                column: "TextboxQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckboxQuestion_Question_Id",
                table: "CheckboxQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DropdownQuestion_Question_Id",
                table: "DropdownQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploadQuestion_Question_Id",
                table: "FileUploadQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceQuestion_Question_Id",
                table: "MultipleChoiceQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_FileUploadQuestion_FileUploadQuestionId",
                table: "Option",
                column: "FileUploadQuestionId",
                principalTable: "FileUploadQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_SubjectiveQuestion_SubjectiveQuestionId",
                table: "Option",
                column: "SubjectiveQuestionId",
                principalTable: "SubjectiveQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_TextboxQuestion_TextboxQuestionId",
                table: "Option",
                column: "TextboxQuestionId",
                principalTable: "TextboxQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingScaleQuestion_Question_Id",
                table: "RatingScaleQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectiveQuestion_Question_Id",
                table: "SubjectiveQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextboxQuestion_Question_Id",
                table: "TextboxQuestion",
                column: "Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
