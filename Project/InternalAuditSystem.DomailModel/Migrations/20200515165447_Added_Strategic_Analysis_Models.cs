using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_Strategic_Analysis_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_QuestionsGroup_QuestionsId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_StrategyAnalysis_StrategyAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_QuestionsGroup_QuestionId",
                table: "UserResponse");

            migrationBuilder.DropTable(
                name: "QuestionsGroup");

            migrationBuilder.DropTable(
                name: "StrategyDocument");

            migrationBuilder.DropIndex(
                name: "IX_StrategicAnalysisTeam_StrategyAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropIndex(
                name: "IX_Option_QuestionsId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "OptionChosen",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "StrategAnalysyId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "StrategyAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "QuestionsId",
                table: "Option");

            migrationBuilder.AlterColumn<string>(
                name: "Other",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OptionId",
                table: "UserResponse",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StrategicAnalysisId",
                table: "StrategicAnalysisTeam",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "Option",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckboxQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DropdownQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MultipleChoiceQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RatingQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectiveQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TextboxQuestionId",
                table: "Option",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionText = table.Column<string>(maxLength: 256, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    StrategyAnalysisId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Question_StrategyAnalysis_StrategyAnalysisId",
                        column: x => x.StrategyAnalysisId,
                        principalTable: "StrategyAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserResponseDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    UserResponseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResponseDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResponseDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserResponseDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserResponseDocument_UserResponse_UserResponseId",
                        column: x => x.UserResponseId,
                        principalTable: "UserResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckboxQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    IsOtherToBeShown = table.Column<bool>(nullable: false),
                    RelatedAnswer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckboxQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckboxQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckboxQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckboxQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DropdownQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    RelatedAnswer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropdownQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropdownQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DropdownQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DropdownQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Guidance = table.Column<string>(maxLength: 256, nullable: true),
                    IsDocAllowed = table.Column<bool>(nullable: false),
                    IsGifAllowed = table.Column<bool>(nullable: false),
                    IsJpegAllowed = table.Column<bool>(nullable: false),
                    IsPpxAllowed = table.Column<bool>(nullable: false),
                    IsPngAllowed = table.Column<bool>(nullable: false),
                    IsPdfAllowed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUploadQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileUploadQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUploadQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    RelatedAnswer = table.Column<int>(nullable: false),
                    IsOtherToBeShown = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RatingScaleQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    ScaleStart = table.Column<int>(nullable: false),
                    ScaleEnd = table.Column<int>(nullable: false),
                    StartLabel = table.Column<string>(nullable: true),
                    EndLabel = table.Column<string>(nullable: true),
                    Representation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingScaleQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingScaleQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingScaleQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingScaleQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectiveQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    CharacterLowerLimit = table.Column<int>(nullable: false),
                    CharacterUpperLimit = table.Column<int>(nullable: false),
                    Guidance = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectiveQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectiveQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectiveQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectiveQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TextboxQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    CharacterLowerLimit = table.Column<int>(nullable: false),
                    CharacterUpperLimit = table.Column<int>(nullable: false),
                    Guidance = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextboxQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextboxQuestion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TextboxQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextboxQuestion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_OptionId",
                table: "UserResponse",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_StrategicAnalysisId",
                table: "StrategicAnalysisTeam",
                column: "StrategicAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CheckboxQuestionId",
                table: "Option",
                column: "CheckboxQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_DropdownQuestionId",
                table: "Option",
                column: "DropdownQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_FileUploadQuestionId",
                table: "Option",
                column: "FileUploadQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_MultipleChoiceQuestionId",
                table: "Option",
                column: "MultipleChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_RatingQuestionId",
                table: "Option",
                column: "RatingQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_SubjectiveQuestionId",
                table: "Option",
                column: "SubjectiveQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_TextboxQuestionId",
                table: "Option",
                column: "TextboxQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxQuestion_CreatedBy",
                table: "CheckboxQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxQuestion_QuestionId",
                table: "CheckboxQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckboxQuestion_UpdatedBy",
                table: "CheckboxQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DropdownQuestion_CreatedBy",
                table: "DropdownQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DropdownQuestion_QuestionId",
                table: "DropdownQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_DropdownQuestion_UpdatedBy",
                table: "DropdownQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadQuestion_CreatedBy",
                table: "FileUploadQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadQuestion_QuestionId",
                table: "FileUploadQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadQuestion_UpdatedBy",
                table: "FileUploadQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestion_CreatedBy",
                table: "MultipleChoiceQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestion_QuestionId",
                table: "MultipleChoiceQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestion_UpdatedBy",
                table: "MultipleChoiceQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Question_CreatedBy",
                table: "Question",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Question_StrategyAnalysisId",
                table: "Question",
                column: "StrategyAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_UpdatedBy",
                table: "Question",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RatingScaleQuestion_CreatedBy",
                table: "RatingScaleQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RatingScaleQuestion_QuestionId",
                table: "RatingScaleQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingScaleQuestion_UpdatedBy",
                table: "RatingScaleQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveQuestion_CreatedBy",
                table: "SubjectiveQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveQuestion_QuestionId",
                table: "SubjectiveQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectiveQuestion_UpdatedBy",
                table: "SubjectiveQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TextboxQuestion_CreatedBy",
                table: "TextboxQuestion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TextboxQuestion_QuestionId",
                table: "TextboxQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TextboxQuestion_UpdatedBy",
                table: "TextboxQuestion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponseDocument_CreatedBy",
                table: "UserResponseDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponseDocument_UpdatedBy",
                table: "UserResponseDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponseDocument_UserResponseId",
                table: "UserResponseDocument",
                column: "UserResponseId");

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
                name: "FK_Option_FileUploadQuestion_FileUploadQuestionId",
                table: "Option",
                column: "FileUploadQuestionId",
                principalTable: "FileUploadQuestion",
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
                name: "FK_StrategicAnalysisTeam_StrategyAnalysis_StrategicAnalysisId",
                table: "StrategicAnalysisTeam",
                column: "StrategicAnalysisId",
                principalTable: "StrategyAnalysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse",
                column: "OptionId",
                principalTable: "Option",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_Question_QuestionId",
                table: "UserResponse",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                name: "FK_Option_FileUploadQuestion_FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_MultipleChoiceQuestion_MultipleChoiceQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_RatingScaleQuestion_RatingQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_SubjectiveQuestion_SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_TextboxQuestion_TextboxQuestionId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_StrategyAnalysis_StrategicAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_Option_OptionId",
                table: "UserResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_Question_QuestionId",
                table: "UserResponse");

            migrationBuilder.DropTable(
                name: "CheckboxQuestion");

            migrationBuilder.DropTable(
                name: "DropdownQuestion");

            migrationBuilder.DropTable(
                name: "FileUploadQuestion");

            migrationBuilder.DropTable(
                name: "MultipleChoiceQuestion");

            migrationBuilder.DropTable(
                name: "RatingScaleQuestion");

            migrationBuilder.DropTable(
                name: "SubjectiveQuestion");

            migrationBuilder.DropTable(
                name: "TextboxQuestion");

            migrationBuilder.DropTable(
                name: "UserResponseDocument");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropIndex(
                name: "IX_UserResponse_OptionId",
                table: "UserResponse");

            migrationBuilder.DropIndex(
                name: "IX_StrategicAnalysisTeam_StrategicAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropIndex(
                name: "IX_Option_CheckboxQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_DropdownQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_MultipleChoiceQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_RatingQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Option_TextboxQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "StrategicAnalysisId",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "CheckboxQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "DropdownQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "FileUploadQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "MultipleChoiceQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "RatingQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "SubjectiveQuestionId",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "TextboxQuestionId",
                table: "Option");

            migrationBuilder.AlterColumn<string>(
                name: "Other",
                table: "UserResponse",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "UserResponse",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OptionChosen",
                table: "UserResponse",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StrategAnalysyId",
                table: "StrategicAnalysisTeam",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StrategyAnalysisId",
                table: "StrategicAnalysisTeam",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "Option",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionId",
                table: "Option",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionsId",
                table: "Option",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionsGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterLowerLimit = table.Column<int>(type: "integer", nullable: false),
                    CharacterUpperLimit = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndLabel = table.Column<string>(type: "text", nullable: true),
                    Guidance = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDocAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsGifAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsJpegAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsOtherToBeShown = table.Column<bool>(type: "boolean", nullable: false),
                    IsPdfAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsPngAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsPpxAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Question = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    RelatedAnswer = table.Column<int>(type: "integer", nullable: false),
                    Representation = table.Column<string>(type: "text", nullable: true),
                    ScaleEnd = table.Column<int>(type: "integer", nullable: false),
                    ScaleStart = table.Column<int>(type: "integer", nullable: false),
                    StartLabel = table.Column<string>(type: "text", nullable: true),
                    StrategyAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsGroup_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionsGroup_StrategyAnalysis_StrategyAnalysisId",
                        column: x => x.StrategyAnalysisId,
                        principalTable: "StrategyAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsGroup_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StrategyDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserResponseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_UserResponse_UserResponseId",
                        column: x => x.UserResponseId,
                        principalTable: "UserResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_StrategyAnalysisId",
                table: "StrategicAnalysisTeam",
                column: "StrategyAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_QuestionsId",
                table: "Option",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsGroup_CreatedBy",
                table: "QuestionsGroup",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsGroup_StrategyAnalysisId",
                table: "QuestionsGroup",
                column: "StrategyAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsGroup_UpdatedBy",
                table: "QuestionsGroup",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyDocument_CreatedBy",
                table: "StrategyDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyDocument_UpdatedBy",
                table: "StrategyDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyDocument_UserResponseId",
                table: "StrategyDocument",
                column: "UserResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_QuestionsGroup_QuestionsId",
                table: "Option",
                column: "QuestionsId",
                principalTable: "QuestionsGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_StrategyAnalysis_StrategyAnalysisId",
                table: "StrategicAnalysisTeam",
                column: "StrategyAnalysisId",
                principalTable: "StrategyAnalysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_QuestionsGroup_QuestionId",
                table: "UserResponse",
                column: "QuestionId",
                principalTable: "QuestionsGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
