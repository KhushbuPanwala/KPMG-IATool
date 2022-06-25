using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Added_DataAnnotations_In_AllModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_User_UpdatedBy",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_User_UpdatedBy",
                table: "ACMPresentation");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_User_UpdatedBy",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMUserTeam_User_UpdatedBy",
                table: "ACMUserTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_UpdatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_UpdatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_User_UpdatedBy",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlanDocument_User_UpdatedBy",
                table: "AuditPlanDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_UpdatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_User_UpdatedBy",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Division_User_UpdatedBy",
                table: "Division");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCategory_User_UpdatedBy",
                table: "EntityCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCountry_User_UpdatedBy",
                table: "EntityCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityDocument_User_UpdatedBy",
                table: "EntityDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityRelationMapping_User_UpdatedBy",
                table: "EntityRelationMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityState_User_UpdatedBy",
                table: "EntityState");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityType_User_UpdatedBy",
                table: "EntityType");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityUserMapping_User_UpdatedBy",
                table: "EntityUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_UpdatedBy",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MainDiscussionPoint_User_UpdatedBy",
                table: "MainDiscussionPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MomDetail_User_UpdatedBy",
                table: "MomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_User_UpdatedBy",
                table: "MomUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Observation_User_UpdatedBy",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_User_UpdatedBy",
                table: "ObservationCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationDocuments_User_UpdatedBy",
                table: "ObservationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_User_UpdatedBy",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_User_UpdatedBy",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_UpdatedBy",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_User_UpdatedBy",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvinceState_User_UpdatedBy",
                table: "ProvinceState");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsGroup_User_UpdatedBy",
                table: "QuestionsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_UpdatedBy",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_User_UpdatedBy",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_RelationshipType_User_UpdatedBy",
                table: "RelationshipType");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_UpdatedBy",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_User_UpdatedBy",
                table: "ReportObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationDocument_User_UpdatedBy",
                table: "ReportObservationDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationMember_User_UpdatedBy",
                table: "ReportObservationMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportUserMapping_User_UpdatedBy",
                table: "ReportUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerDocument_User_UpdatedBy",
                table: "ReviewerDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessment_User_UpdatedBy",
                table: "RiskAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_AuditableEntity_EntityId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_User_UpdatedBy",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_User_UpdatedBy",
                table: "RiskControlMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixProcess_User_UpdatedBy",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSector_User_UpdatedBy",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_UpdatedBy",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_User_UpdatedBy",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_User_UpdatedBy",
                table: "StrategyAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyDocument_User_UpdatedBy",
                table: "StrategyDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPointOfDiscussion_User_UpdatedBy",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_UpdatedBy",
                table: "UserResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkPaper_User_UpdatedBy",
                table: "WorkPaper");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgram_User_UpdatedBy",
                table: "WorkProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgramTeam_User_UpdatedBy",
                table: "WorkProgramTeam");

            migrationBuilder.DropIndex(
                name: "IX_RiskAssessmentDocument_EntityId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkProgramTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WorkProgramTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkProgram");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WorkProgram");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkPaper");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WorkPaper");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StrategyDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StrategyDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RiskAssessment");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RiskAssessment");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReviewerDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReviewerDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReportUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReportUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReportObservationDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReportObservationDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RelationshipType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "RelationshipType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "QuestionsGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "QuestionsGroup");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProvinceState");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ProvinceState");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ObservationDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ObservationDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ObservationCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ObservationCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MainDiscussionPoint");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MainDiscussionPoint");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityState");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EntityState");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityState");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityRelationMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityRelationMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityCountry");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EntityCountry");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityCountry");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EntityCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "EntityCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuditType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuditPlanDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditPlanDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuditCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AuditableEntity");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditableEntity");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ACMUserTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ACMUserTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ACMDocument");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkProgramTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "WorkProgramTeam",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "WorkProgramTeam",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkProgram",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "WorkProgram",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkProgram",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DraftIssues",
                table: "WorkProgram",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditTitle",
                table: "WorkProgram",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "WorkProgram",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "WorkProgram",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkPaper",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "WorkPaper",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "WorkPaper",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Other",
                table: "UserResponse",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "UserResponse",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "UserResponse",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "UserResponse",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "User",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "SubPointOfDiscussion",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "SubPoint",
                table: "SubPointOfDiscussion",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "SubPointOfDiscussion",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "SubPointOfDiscussion",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategyDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "StrategyDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "StrategyDocument",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Version",
                table: "StrategyAnalysis",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategyAnalysis",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "SurveyTitle",
                table: "StrategyAnalysis",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditableEntityName",
                table: "StrategyAnalysis",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "StrategyAnalysis",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DetailsOfOpportunity",
                table: "StrategyAnalysis",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedValueOfOpportunity",
                table: "StrategyAnalysis",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "StrategyAnalysis",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategicAnalysisTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "StrategicAnalysisTeam",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "StrategicAnalysisTeam",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixSubProcess",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "SubProcess",
                table: "RiskControlMatrixSubProcess",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskControlMatrixSubProcess",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixSubProcess",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixSector",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "RiskControlMatrixSector",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskControlMatrixSector",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixSector",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixProcess",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Process",
                table: "RiskControlMatrixProcess",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskControlMatrixProcess",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixProcess",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrix",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "RiskName",
                table: "RiskControlMatrix",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskControlMatrix",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskControlMatrix",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskAssessmentDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskAssessmentDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "RiskAssessmentDocument",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RiskAssessmentId",
                table: "RiskAssessmentDocument",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskAssessmentDocument",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskAssessment",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RiskAssessment",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RiskAssessment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RiskAssessment",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReviewerDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ReviewerDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ReviewerDocument",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ReportUserMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ReportUserMapping",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservationMember",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ReportObservationMember",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ReportObservationMember",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservationDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ReportObservationDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ReportObservationDocument",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "ReportObservation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ReportObservation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ReportObservation",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Version",
                table: "Report",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Report",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ReportTitle",
                table: "Report",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Report",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Report",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RelationshipType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RelationshipType",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RelationshipType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RelationshipType",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Region",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Region",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Region",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Region",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Rating",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Ratings",
                table: "Rating",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuantitativeFactors",
                table: "Rating",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualitativeFactors",
                table: "Rating",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Rating",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Rating",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "QuestionsGroup",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "QuestionsGroup",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Guidance",
                table: "QuestionsGroup",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "QuestionsGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "QuestionsGroup",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ProvinceState",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProvinceState",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ProvinceState",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ProvinceState",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Process",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ScopeBasedOn",
                table: "Process",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Process",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Process",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Process",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "PrimaryGeograhcialArea",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PrimaryGeograhcialArea",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PrimaryGeograhcialArea",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "PlanProcessMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PlanProcessMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PlanProcessMapping",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "Option",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Option",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Option",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ObservationDocuments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ObservationDocuments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ObservationDocuments",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ObservationCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "ObservationCategory",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ObservationCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ObservationCategory",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Observation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "RootCause",
                table: "Observation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "Observation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Heading",
                table: "Observation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Observation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Observation",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MomUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MomUserMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "MomUserMapping",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MomDetail",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Agenda",
                table: "MomDetail",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MomDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "MomDetail",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MainDiscussionPoint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "MainPoint",
                table: "MainDiscussionPoint",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MainDiscussionPoint",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "MainDiscussionPoint",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Location",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Location",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Location",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityUserMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityUserMapping",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "EntityType",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityType",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityState",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityState",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityState",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityRelationMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityRelationMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityRelationMapping",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "EntityDocument",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityDocument",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityCountry",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityCountry",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityCountry",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "EntityCategory",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "EntityCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "EntityCategory",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Division",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Division",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Division",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Division",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Country",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Country",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Country",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditType",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AuditType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "AuditType",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditPlanDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "AuditPlanDocument",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AuditPlanDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "AuditPlanDocument",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Version",
                table: "AuditPlan",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditPlan",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AuditPlan",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AuditPlan",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FinancialYear",
                table: "AuditPlan",
                maxLength: 4,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "AuditPlan",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditCategory",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AuditCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "AuditCategory",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Version",
                table: "AuditableEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditableEntity",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditableEntity",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "AuditableEntity",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "AuditableEntity",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMUserTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ACMUserTeam",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ACMUserTeam",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMReportMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ACMReportMapping",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ACMReportMapping",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Version",
                table: "ACMPresentation",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMPresentation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "ACMPresentation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Heading",
                table: "ACMPresentation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ACMReportTitle",
                table: "ACMPresentation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ACMPresentation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ACMPresentation",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "ACMDocument",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "ACMDocument",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessmentDocument_RiskAssessmentId",
                table: "RiskAssessmentDocument",
                column: "RiskAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_User_UpdatedBy",
                table: "ACMDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_User_UpdatedBy",
                table: "ACMPresentation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_User_UpdatedBy",
                table: "ACMReportMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMUserTeam_User_UpdatedBy",
                table: "ACMUserTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_User_UpdatedBy",
                table: "AuditableEntity",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditCategory_User_UpdatedBy",
                table: "AuditCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_User_UpdatedBy",
                table: "AuditPlan",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlanDocument_User_UpdatedBy",
                table: "AuditPlanDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditType_User_UpdatedBy",
                table: "AuditType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_User_UpdatedBy",
                table: "Country",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Division_User_UpdatedBy",
                table: "Division",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCategory_User_UpdatedBy",
                table: "EntityCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCountry_User_UpdatedBy",
                table: "EntityCountry",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityDocument_User_UpdatedBy",
                table: "EntityDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityRelationMapping_User_UpdatedBy",
                table: "EntityRelationMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityState_User_UpdatedBy",
                table: "EntityState",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityType_User_UpdatedBy",
                table: "EntityType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityUserMapping_User_UpdatedBy",
                table: "EntityUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_UpdatedBy",
                table: "Location",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MainDiscussionPoint_User_UpdatedBy",
                table: "MainDiscussionPoint",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MomDetail_User_UpdatedBy",
                table: "MomDetail",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_User_UpdatedBy",
                table: "MomUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_User_UpdatedBy",
                table: "Observation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_User_UpdatedBy",
                table: "ObservationCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationDocuments_User_UpdatedBy",
                table: "ObservationDocuments",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_User_UpdatedBy",
                table: "Option",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_User_UpdatedBy",
                table: "PlanProcessMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_UpdatedBy",
                table: "PrimaryGeograhcialArea",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_User_UpdatedBy",
                table: "Process",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvinceState_User_UpdatedBy",
                table: "ProvinceState",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsGroup_User_UpdatedBy",
                table: "QuestionsGroup",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_UpdatedBy",
                table: "Rating",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_User_UpdatedBy",
                table: "Region",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelationshipType_User_UpdatedBy",
                table: "RelationshipType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_UpdatedBy",
                table: "Report",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_User_UpdatedBy",
                table: "ReportObservation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationDocument_User_UpdatedBy",
                table: "ReportObservationDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationMember_User_UpdatedBy",
                table: "ReportObservationMember",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportUserMapping_User_UpdatedBy",
                table: "ReportUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerDocument_User_UpdatedBy",
                table: "ReviewerDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessment_User_UpdatedBy",
                table: "RiskAssessment",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_RiskAssessment_RiskAssessmentId",
                table: "RiskAssessmentDocument",
                column: "RiskAssessmentId",
                principalTable: "RiskAssessment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_User_UpdatedBy",
                table: "RiskAssessmentDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_User_UpdatedBy",
                table: "RiskControlMatrix",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixProcess_User_UpdatedBy",
                table: "RiskControlMatrixProcess",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSector_User_UpdatedBy",
                table: "RiskControlMatrixSector",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_UpdatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_User_UpdatedBy",
                table: "StrategicAnalysisTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_User_UpdatedBy",
                table: "StrategyAnalysis",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyDocument_User_UpdatedBy",
                table: "StrategyDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPointOfDiscussion_User_UpdatedBy",
                table: "SubPointOfDiscussion",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_UpdatedBy",
                table: "UserResponse",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPaper_User_UpdatedBy",
                table: "WorkPaper",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgram_User_UpdatedBy",
                table: "WorkProgram",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgramTeam_User_UpdatedBy",
                table: "WorkProgramTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_User_UpdatedBy",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_User_UpdatedBy",
                table: "ACMPresentation");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_User_UpdatedBy",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMUserTeam_User_UpdatedBy",
                table: "ACMUserTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_UpdatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_UpdatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_User_UpdatedBy",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlanDocument_User_UpdatedBy",
                table: "AuditPlanDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_UpdatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_User_UpdatedBy",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Division_User_UpdatedBy",
                table: "Division");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCategory_User_UpdatedBy",
                table: "EntityCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCountry_User_UpdatedBy",
                table: "EntityCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityDocument_User_UpdatedBy",
                table: "EntityDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityRelationMapping_User_UpdatedBy",
                table: "EntityRelationMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityState_User_UpdatedBy",
                table: "EntityState");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityType_User_UpdatedBy",
                table: "EntityType");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityUserMapping_User_UpdatedBy",
                table: "EntityUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_UpdatedBy",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MainDiscussionPoint_User_UpdatedBy",
                table: "MainDiscussionPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MomDetail_User_UpdatedBy",
                table: "MomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_User_UpdatedBy",
                table: "MomUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Observation_User_UpdatedBy",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_User_UpdatedBy",
                table: "ObservationCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationDocuments_User_UpdatedBy",
                table: "ObservationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_User_UpdatedBy",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_User_UpdatedBy",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_UpdatedBy",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_User_UpdatedBy",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvinceState_User_UpdatedBy",
                table: "ProvinceState");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsGroup_User_UpdatedBy",
                table: "QuestionsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_UpdatedBy",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_User_UpdatedBy",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_RelationshipType_User_UpdatedBy",
                table: "RelationshipType");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_UpdatedBy",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_User_UpdatedBy",
                table: "ReportObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationDocument_User_UpdatedBy",
                table: "ReportObservationDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationMember_User_UpdatedBy",
                table: "ReportObservationMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportUserMapping_User_UpdatedBy",
                table: "ReportUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerDocument_User_UpdatedBy",
                table: "ReviewerDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessment_User_UpdatedBy",
                table: "RiskAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_RiskAssessment_RiskAssessmentId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_User_UpdatedBy",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_User_UpdatedBy",
                table: "RiskControlMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixProcess_User_UpdatedBy",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSector_User_UpdatedBy",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_UpdatedBy",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_User_UpdatedBy",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_User_UpdatedBy",
                table: "StrategyAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyDocument_User_UpdatedBy",
                table: "StrategyDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPointOfDiscussion_User_UpdatedBy",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_UpdatedBy",
                table: "UserResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkPaper_User_UpdatedBy",
                table: "WorkPaper");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgram_User_UpdatedBy",
                table: "WorkProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgramTeam_User_UpdatedBy",
                table: "WorkProgramTeam");

            migrationBuilder.DropIndex(
                name: "IX_RiskAssessmentDocument_RiskAssessmentId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "WorkProgramTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "WorkProgramTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "WorkProgram");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "WorkProgram");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "WorkPaper");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "WorkPaper");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "UserResponse");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "StrategyDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "StrategyDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "DetailsOfOpportunity",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "EstimatedValueOfOpportunity",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "StrategyAnalysis");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskControlMatrix");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "RiskAssessmentId",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RiskAssessment");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RiskAssessment");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ReviewerDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ReviewerDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ReportUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ReportUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ReportObservationMember");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ReportObservationDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ReportObservationDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ReportObservation");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RelationshipType");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RelationshipType");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "QuestionsGroup");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "QuestionsGroup");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ProvinceState");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ProvinceState");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PlanProcessMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ObservationDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ObservationDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ObservationCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ObservationCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Observation");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "MomUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "MomDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "MainDiscussionPoint");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "MainDiscussionPoint");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityUserMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityUserMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityState");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityState");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityRelationMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityRelationMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityCountry");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityCountry");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "EntityCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "EntityCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AuditType");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "AuditType");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AuditPlanDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "AuditPlanDocument");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "FinancialYear",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "AuditPlan");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AuditCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "AuditCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "AuditableEntity");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "AuditableEntity");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ACMUserTeam");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ACMUserTeam");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ACMReportMapping");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ACMPresentation");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "ACMDocument");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "ACMDocument");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkProgramTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkProgramTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WorkProgramTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkProgram",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "WorkProgram",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkProgram",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DraftIssues",
                table: "WorkProgram",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditTitle",
                table: "WorkProgram",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkProgram",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WorkProgram",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "WorkPaper",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkPaper",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WorkPaper",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "UserResponse",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Other",
                table: "UserResponse",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerText",
                table: "UserResponse",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserResponse",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UserResponse",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "User",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "User",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "User",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "SubPointOfDiscussion",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubPoint",
                table: "SubPointOfDiscussion",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SubPointOfDiscussion",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SubPointOfDiscussion",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategyDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StrategyDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StrategyDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "StrategyAnalysis",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategyAnalysis",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SurveyTitle",
                table: "StrategyAnalysis",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuditableEntityName",
                table: "StrategyAnalysis",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StrategyAnalysis",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StrategyAnalysis",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "StrategicAnalysisTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StrategicAnalysisTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StrategicAnalysisTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixSubProcess",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubProcess",
                table: "RiskControlMatrixSubProcess",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskControlMatrixSubProcess",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskControlMatrixSubProcess",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixSector",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "RiskControlMatrixSector",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskControlMatrixSector",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskControlMatrixSector",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrixProcess",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Process",
                table: "RiskControlMatrixProcess",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskControlMatrixProcess",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskControlMatrixProcess",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskControlMatrix",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RiskName",
                table: "RiskControlMatrix",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskControlMatrix",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskControlMatrix",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskAssessmentDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskAssessmentDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "RiskAssessmentDocument",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RiskAssessmentDocument",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RiskAssessmentDocument",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "RiskAssessmentDocument",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskAssessmentDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "RiskAssessmentDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RiskAssessment",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RiskAssessment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RiskAssessment",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RiskAssessment",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReviewerDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReviewerDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReviewerDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReportUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReportUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservationMember",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReportObservationMember",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReportObservationMember",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservationDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReportObservationDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReportObservationDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ReportObservation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "ReportObservation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReportObservation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ReportObservation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Report",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Report",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportTitle",
                table: "Report",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Report",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Report",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "RelationshipType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RelationshipType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RelationshipType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "RelationshipType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Region",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Region",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Region",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Region",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Rating",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ratings",
                table: "Rating",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuantitativeFactors",
                table: "Rating",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QualitativeFactors",
                table: "Rating",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Rating",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Rating",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "QuestionsGroup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "QuestionsGroup",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Guidance",
                table: "QuestionsGroup",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "QuestionsGroup",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "QuestionsGroup",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ProvinceState",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProvinceState",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProvinceState",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ProvinceState",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Process",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ScopeBasedOn",
                table: "Process",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Process",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Process",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Process",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "PrimaryGeograhcialArea",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PrimaryGeograhcialArea",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PrimaryGeograhcialArea",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "PlanProcessMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PlanProcessMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PlanProcessMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "Option",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Option",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Option",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ObservationDocuments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ObservationDocuments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ObservationDocuments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ObservationCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "ObservationCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ObservationCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ObservationCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Observation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RootCause",
                table: "Observation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "Observation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Heading",
                table: "Observation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Observation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Observation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MomUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MomUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MomUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MomDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Agenda",
                table: "MomDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MomDetail",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MomDetail",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "MainDiscussionPoint",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MainPoint",
                table: "MainDiscussionPoint",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MainDiscussionPoint",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MainDiscussionPoint",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Location",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Location",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Location",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Location",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityUserMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "EntityType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityState",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityState",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EntityState",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityState",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityRelationMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityRelationMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityRelationMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "EntityDocument",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityCountry",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityCountry",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EntityCountry",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityCountry",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "EntityCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "EntityCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EntityCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "EntityCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Division",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Division",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Division",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Division",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Country",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Country",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Country",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuditType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditType",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditPlanDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "AuditPlanDocument",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuditPlanDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditPlanDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "AuditPlan",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditPlan",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AuditPlan",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuditPlan",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditPlan",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuditCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditCategory",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "AuditableEntity",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "AuditableEntity",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuditableEntity",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AuditableEntity",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditableEntity",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMUserTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ACMUserTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ACMUserTeam",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ACMReportMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ACMReportMapping",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "ACMPresentation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMPresentation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Implication",
                table: "ACMPresentation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Heading",
                table: "ACMPresentation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ACMReportTitle",
                table: "ACMPresentation",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ACMPresentation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ACMPresentation",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "ACMDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ACMDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ACMDocument",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessmentDocument_EntityId",
                table: "RiskAssessmentDocument",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_User_UpdatedBy",
                table: "ACMDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_User_UpdatedBy",
                table: "ACMPresentation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_User_UpdatedBy",
                table: "ACMReportMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMUserTeam_User_UpdatedBy",
                table: "ACMUserTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_User_UpdatedBy",
                table: "AuditableEntity",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditCategory_User_UpdatedBy",
                table: "AuditCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_User_UpdatedBy",
                table: "AuditPlan",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlanDocument_User_UpdatedBy",
                table: "AuditPlanDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditType_User_UpdatedBy",
                table: "AuditType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_User_UpdatedBy",
                table: "Country",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Division_User_UpdatedBy",
                table: "Division",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCategory_User_UpdatedBy",
                table: "EntityCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCountry_User_UpdatedBy",
                table: "EntityCountry",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityDocument_User_UpdatedBy",
                table: "EntityDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityRelationMapping_User_UpdatedBy",
                table: "EntityRelationMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityState_User_UpdatedBy",
                table: "EntityState",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityType_User_UpdatedBy",
                table: "EntityType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityUserMapping_User_UpdatedBy",
                table: "EntityUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_UpdatedBy",
                table: "Location",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MainDiscussionPoint_User_UpdatedBy",
                table: "MainDiscussionPoint",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MomDetail_User_UpdatedBy",
                table: "MomDetail",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_User_UpdatedBy",
                table: "MomUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_User_UpdatedBy",
                table: "Observation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_User_UpdatedBy",
                table: "ObservationCategory",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationDocuments_User_UpdatedBy",
                table: "ObservationDocuments",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_User_UpdatedBy",
                table: "Option",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_User_UpdatedBy",
                table: "PlanProcessMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_UpdatedBy",
                table: "PrimaryGeograhcialArea",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_User_UpdatedBy",
                table: "Process",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvinceState_User_UpdatedBy",
                table: "ProvinceState",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsGroup_User_UpdatedBy",
                table: "QuestionsGroup",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_UpdatedBy",
                table: "Rating",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_User_UpdatedBy",
                table: "Region",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelationshipType_User_UpdatedBy",
                table: "RelationshipType",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_UpdatedBy",
                table: "Report",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_User_UpdatedBy",
                table: "ReportObservation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationDocument_User_UpdatedBy",
                table: "ReportObservationDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationMember_User_UpdatedBy",
                table: "ReportObservationMember",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportUserMapping_User_UpdatedBy",
                table: "ReportUserMapping",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerDocument_User_UpdatedBy",
                table: "ReviewerDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessment_User_UpdatedBy",
                table: "RiskAssessment",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_AuditableEntity_EntityId",
                table: "RiskAssessmentDocument",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_User_UpdatedBy",
                table: "RiskAssessmentDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_User_UpdatedBy",
                table: "RiskControlMatrix",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixProcess_User_UpdatedBy",
                table: "RiskControlMatrixProcess",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSector_User_UpdatedBy",
                table: "RiskControlMatrixSector",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_UpdatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_User_UpdatedBy",
                table: "StrategicAnalysisTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_User_UpdatedBy",
                table: "StrategyAnalysis",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyDocument_User_UpdatedBy",
                table: "StrategyDocument",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPointOfDiscussion_User_UpdatedBy",
                table: "SubPointOfDiscussion",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_UpdatedBy",
                table: "UserResponse",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPaper_User_UpdatedBy",
                table: "WorkPaper",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgram_User_UpdatedBy",
                table: "WorkProgram",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgramTeam_User_UpdatedBy",
                table: "WorkProgramTeam",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
