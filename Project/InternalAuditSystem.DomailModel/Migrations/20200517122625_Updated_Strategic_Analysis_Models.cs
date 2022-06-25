using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Updated_Strategic_Analysis_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "StrategicAnalysisId",
                table: "UserResponse",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_StrategicAnalysisId",
                table: "UserResponse",
                column: "StrategicAnalysisId");

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
                name: "FK_UserResponse_StrategyAnalysis_StrategicAnalysisId",
                table: "UserResponse",
                column: "StrategicAnalysisId",
                principalTable: "StrategyAnalysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_RatingScaleQuestion_Question_Id",
                table: "RatingScaleQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectiveQuestion_Question_Id",
                table: "SubjectiveQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_TextboxQuestion_Question_Id",
                table: "TextboxQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_StrategyAnalysis_StrategicAnalysisId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_StrategicAnalysisId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "StrategicAnalysisId",
                table: "UserResponse");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "TextboxQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "SubjectiveQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "RatingScaleQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "MultipleChoiceQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "FileUploadQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "DropdownQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "CheckboxQuestion",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TextboxQuestion_QuestionId",
                table: "TextboxQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveQuestion_QuestionId",
                table: "SubjectiveQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingScaleQuestion_QuestionId",
                table: "RatingScaleQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestion_QuestionId",
                table: "MultipleChoiceQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadQuestion_QuestionId",
                table: "FileUploadQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DropdownQuestion_QuestionId",
                table: "DropdownQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxQuestion_QuestionId",
                table: "CheckboxQuestion",
                column: "QuestionId");

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
        }
    }
}
