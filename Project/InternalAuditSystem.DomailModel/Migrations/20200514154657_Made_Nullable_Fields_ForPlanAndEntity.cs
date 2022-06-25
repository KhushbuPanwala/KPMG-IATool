using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class Made_Nullable_Fields_ForPlanAndEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_User_CreatedBy",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_User_CreatedBy",
                table: "ACMPresentation");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_User_CreatedBy",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMUserTeam_User_CreatedBy",
                table: "ACMUserTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_CreatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_CreatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_User_CreatedBy",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditType_SelectedTypeId",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlanDocument_User_CreatedBy",
                table: "AuditPlanDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_CreatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_User_CreatedBy",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Division_User_CreatedBy",
                table: "Division");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCategory_User_CreatedBy",
                table: "EntityCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCountry_User_CreatedBy",
                table: "EntityCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityDocument_User_CreatedBy",
                table: "EntityDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityRelationMapping_User_CreatedBy",
                table: "EntityRelationMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityState_User_CreatedBy",
                table: "EntityState");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityType_User_CreatedBy",
                table: "EntityType");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityUserMapping_User_CreatedBy",
                table: "EntityUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_CreatedBy",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MainDiscussionPoint_User_CreatedBy",
                table: "MainDiscussionPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MomDetail_User_CreatedBy",
                table: "MomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_User_CreatedBy",
                table: "MomUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Observation_User_CreatedBy",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_User_CreatedBy",
                table: "ObservationCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationDocuments_User_CreatedBy",
                table: "ObservationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_User_CreatedBy",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_User_CreatedBy",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_WorkProgram_WorkProgramId",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_CreatedBy",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_User_CreatedBy",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_Process_ParentId",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvinceState_User_CreatedBy",
                table: "ProvinceState");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsGroup_User_CreatedBy",
                table: "QuestionsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_CreatedBy",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_User_CreatedBy",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_RelationshipType_User_CreatedBy",
                table: "RelationshipType");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_CreatedBy",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_User_CreatedBy",
                table: "ReportObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationDocument_User_CreatedBy",
                table: "ReportObservationDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationMember_User_CreatedBy",
                table: "ReportObservationMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportUserMapping_User_CreatedBy",
                table: "ReportUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerDocument_User_CreatedBy",
                table: "ReviewerDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessment_User_CreatedBy",
                table: "RiskAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_User_CreatedBy",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_User_CreatedBy",
                table: "RiskControlMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixProcess_User_CreatedBy",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSector_User_CreatedBy",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_CreatedBy",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_User_CreatedBy",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_User_CreatedBy",
                table: "StrategyAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyDocument_User_CreatedBy",
                table: "StrategyDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPointOfDiscussion_User_CreatedBy",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_CreatedBy",
                table: "UserResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkPaper_User_CreatedBy",
                table: "WorkPaper");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgram_User_CreatedBy",
                table: "WorkProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgramTeam_User_CreatedBy",
                table: "WorkProgramTeam");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkProgramTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkProgram",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkPaper",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserResponse",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "User",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "SubPointOfDiscussion",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategyDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategyAnalysis",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategicAnalysisTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixSubProcess",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixSector",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixProcess",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrix",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskAssessmentDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskAssessment",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReviewerDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservationMember",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservationDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Report",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RelationshipType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Region",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Rating",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "QuestionsGroup",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProvinceState",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "Process",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Process",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PrimaryGeograhcialArea",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkProgramId",
                table: "PlanProcessMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PlanProcessMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Option",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ObservationDocuments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ObservationCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Observation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MomUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MomDetail",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MainDiscussionPoint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Location",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityUserMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityState",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityRelationMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityCountry",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Division",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Country",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditType",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditPlanDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedTypeId",
                table: "AuditPlan",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectCategoryId",
                table: "AuditPlan",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditPlan",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditCategory",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedTypeId",
                table: "AuditableEntity",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedCategoryId",
                table: "AuditableEntity",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditableEntity",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMUserTeam",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMReportMapping",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMPresentation",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMDocument",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_User_CreatedBy",
                table: "ACMDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_User_CreatedBy",
                table: "ACMPresentation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_User_CreatedBy",
                table: "ACMReportMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMUserTeam_User_CreatedBy",
                table: "ACMUserTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_User_CreatedBy",
                table: "AuditableEntity",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AuditCategory_User_CreatedBy",
                table: "AuditCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_User_CreatedBy",
                table: "AuditPlan",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlanDocument_User_CreatedBy",
                table: "AuditPlanDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditType_User_CreatedBy",
                table: "AuditType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_User_CreatedBy",
                table: "Country",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Division_User_CreatedBy",
                table: "Division",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCategory_User_CreatedBy",
                table: "EntityCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCountry_User_CreatedBy",
                table: "EntityCountry",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityDocument_User_CreatedBy",
                table: "EntityDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityRelationMapping_User_CreatedBy",
                table: "EntityRelationMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityState_User_CreatedBy",
                table: "EntityState",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityType_User_CreatedBy",
                table: "EntityType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityUserMapping_User_CreatedBy",
                table: "EntityUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_CreatedBy",
                table: "Location",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MainDiscussionPoint_User_CreatedBy",
                table: "MainDiscussionPoint",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MomDetail_User_CreatedBy",
                table: "MomDetail",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_User_CreatedBy",
                table: "MomUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_User_CreatedBy",
                table: "Observation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_User_CreatedBy",
                table: "ObservationCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationDocuments_User_CreatedBy",
                table: "ObservationDocuments",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_User_CreatedBy",
                table: "Option",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_User_CreatedBy",
                table: "PlanProcessMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_WorkProgram_WorkProgramId",
                table: "PlanProcessMapping",
                column: "WorkProgramId",
                principalTable: "WorkProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_CreatedBy",
                table: "PrimaryGeograhcialArea",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_User_CreatedBy",
                table: "Process",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Process_ParentId",
                table: "Process",
                column: "ParentId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvinceState_User_CreatedBy",
                table: "ProvinceState",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsGroup_User_CreatedBy",
                table: "QuestionsGroup",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_CreatedBy",
                table: "Rating",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_User_CreatedBy",
                table: "Region",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelationshipType_User_CreatedBy",
                table: "RelationshipType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_CreatedBy",
                table: "Report",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_User_CreatedBy",
                table: "ReportObservation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationDocument_User_CreatedBy",
                table: "ReportObservationDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationMember_User_CreatedBy",
                table: "ReportObservationMember",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportUserMapping_User_CreatedBy",
                table: "ReportUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerDocument_User_CreatedBy",
                table: "ReviewerDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessment_User_CreatedBy",
                table: "RiskAssessment",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_User_CreatedBy",
                table: "RiskAssessmentDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_User_CreatedBy",
                table: "RiskControlMatrix",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixProcess_User_CreatedBy",
                table: "RiskControlMatrixProcess",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSector_User_CreatedBy",
                table: "RiskControlMatrixSector",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_CreatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_User_CreatedBy",
                table: "StrategicAnalysisTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_User_CreatedBy",
                table: "StrategyAnalysis",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyDocument_User_CreatedBy",
                table: "StrategyDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPointOfDiscussion_User_CreatedBy",
                table: "SubPointOfDiscussion",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_CreatedBy",
                table: "UserResponse",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPaper_User_CreatedBy",
                table: "WorkPaper",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgram_User_CreatedBy",
                table: "WorkProgram",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgramTeam_User_CreatedBy",
                table: "WorkProgramTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ACMDocument_User_CreatedBy",
                table: "ACMDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMPresentation_User_CreatedBy",
                table: "ACMPresentation");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMReportMapping_User_CreatedBy",
                table: "ACMReportMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ACMUserTeam_User_CreatedBy",
                table: "ACMUserTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_CreatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_CreatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_User_CreatedBy",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlan_AuditType_SelectedTypeId",
                table: "AuditPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditPlanDocument_User_CreatedBy",
                table: "AuditPlanDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_CreatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_User_CreatedBy",
                table: "Country");

            migrationBuilder.DropForeignKey(
                name: "FK_Division_User_CreatedBy",
                table: "Division");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCategory_User_CreatedBy",
                table: "EntityCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityCountry_User_CreatedBy",
                table: "EntityCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityDocument_User_CreatedBy",
                table: "EntityDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityRelationMapping_User_CreatedBy",
                table: "EntityRelationMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityState_User_CreatedBy",
                table: "EntityState");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityType_User_CreatedBy",
                table: "EntityType");

            migrationBuilder.DropForeignKey(
                name: "FK_EntityUserMapping_User_CreatedBy",
                table: "EntityUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_CreatedBy",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_MainDiscussionPoint_User_CreatedBy",
                table: "MainDiscussionPoint");

            migrationBuilder.DropForeignKey(
                name: "FK_MomDetail_User_CreatedBy",
                table: "MomDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MomUserMapping_User_CreatedBy",
                table: "MomUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_Observation_User_CreatedBy",
                table: "Observation");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationCategory_User_CreatedBy",
                table: "ObservationCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ObservationDocuments_User_CreatedBy",
                table: "ObservationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_User_CreatedBy",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_User_CreatedBy",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanProcessMapping_WorkProgram_WorkProgramId",
                table: "PlanProcessMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_CreatedBy",
                table: "PrimaryGeograhcialArea");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_User_CreatedBy",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_Process_ParentId",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvinceState_User_CreatedBy",
                table: "ProvinceState");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsGroup_User_CreatedBy",
                table: "QuestionsGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_CreatedBy",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_User_CreatedBy",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_RelationshipType_User_CreatedBy",
                table: "RelationshipType");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_CreatedBy",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservation_User_CreatedBy",
                table: "ReportObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationDocument_User_CreatedBy",
                table: "ReportObservationDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportObservationMember_User_CreatedBy",
                table: "ReportObservationMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportUserMapping_User_CreatedBy",
                table: "ReportUserMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewerDocument_User_CreatedBy",
                table: "ReviewerDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessment_User_CreatedBy",
                table: "RiskAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskAssessmentDocument_User_CreatedBy",
                table: "RiskAssessmentDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrix_User_CreatedBy",
                table: "RiskControlMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixProcess_User_CreatedBy",
                table: "RiskControlMatrixProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSector_User_CreatedBy",
                table: "RiskControlMatrixSector");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_CreatedBy",
                table: "RiskControlMatrixSubProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAnalysisTeam_User_CreatedBy",
                table: "StrategicAnalysisTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyAnalysis_User_CreatedBy",
                table: "StrategyAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_StrategyDocument_User_CreatedBy",
                table: "StrategyDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPointOfDiscussion_User_CreatedBy",
                table: "SubPointOfDiscussion");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponse_User_CreatedBy",
                table: "UserResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkPaper_User_CreatedBy",
                table: "WorkPaper");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgram_User_CreatedBy",
                table: "WorkProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkProgramTeam_User_CreatedBy",
                table: "WorkProgramTeam");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkProgramTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkProgram",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "WorkPaper",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UserResponse",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "User",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "SubPointOfDiscussion",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategyDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategyAnalysis",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "StrategicAnalysisTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixSubProcess",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixSector",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrixProcess",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskControlMatrix",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskAssessmentDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RiskAssessment",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReviewerDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservationMember",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservationDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ReportObservation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Report",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "RelationshipType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Region",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Rating",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "QuestionsGroup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ProvinceState",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "Process",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Process",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PrimaryGeograhcialArea",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkProgramId",
                table: "PlanProcessMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "PlanProcessMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Option",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ObservationDocuments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ObservationCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Observation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MomUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MomDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "MainDiscussionPoint",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Location",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityUserMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityState",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityRelationMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityCountry",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "EntityCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Division",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Country",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditPlanDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedTypeId",
                table: "AuditPlan",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectCategoryId",
                table: "AuditPlan",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditPlan",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditCategory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedTypeId",
                table: "AuditableEntity",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SelectedCategoryId",
                table: "AuditableEntity",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "AuditableEntity",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMUserTeam",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMReportMapping",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMPresentation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "ACMDocument",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_User_CreatedBy",
                table: "ACMDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMPresentation_User_CreatedBy",
                table: "ACMPresentation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_User_CreatedBy",
                table: "ACMReportMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMUserTeam_User_CreatedBy",
                table: "ACMUserTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_User_CreatedBy",
                table: "AuditableEntity",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity",
                column: "SelectedCategoryId",
                principalTable: "AuditCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity",
                column: "SelectedTypeId",
                principalTable: "AuditType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditCategory_User_CreatedBy",
                table: "AuditCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_User_CreatedBy",
                table: "AuditPlan",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                table: "AuditPlan",
                column: "SelectCategoryId",
                principalTable: "AuditCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlan_AuditType_SelectedTypeId",
                table: "AuditPlan",
                column: "SelectedTypeId",
                principalTable: "AuditType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditPlanDocument_User_CreatedBy",
                table: "AuditPlanDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditType_User_CreatedBy",
                table: "AuditType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_User_CreatedBy",
                table: "Country",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Division_User_CreatedBy",
                table: "Division",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCategory_User_CreatedBy",
                table: "EntityCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityCountry_User_CreatedBy",
                table: "EntityCountry",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityDocument_User_CreatedBy",
                table: "EntityDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityRelationMapping_User_CreatedBy",
                table: "EntityRelationMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityState_User_CreatedBy",
                table: "EntityState",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityType_User_CreatedBy",
                table: "EntityType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityUserMapping_User_CreatedBy",
                table: "EntityUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_CreatedBy",
                table: "Location",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MainDiscussionPoint_User_CreatedBy",
                table: "MainDiscussionPoint",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MomDetail_User_CreatedBy",
                table: "MomDetail",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MomUserMapping_User_CreatedBy",
                table: "MomUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observation_User_CreatedBy",
                table: "Observation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationCategory_User_CreatedBy",
                table: "ObservationCategory",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationDocuments_User_CreatedBy",
                table: "ObservationDocuments",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_User_CreatedBy",
                table: "Option",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_User_CreatedBy",
                table: "PlanProcessMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanProcessMapping_WorkProgram_WorkProgramId",
                table: "PlanProcessMapping",
                column: "WorkProgramId",
                principalTable: "WorkProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryGeograhcialArea_User_CreatedBy",
                table: "PrimaryGeograhcialArea",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_User_CreatedBy",
                table: "Process",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Process_ParentId",
                table: "Process",
                column: "ParentId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvinceState_User_CreatedBy",
                table: "ProvinceState",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsGroup_User_CreatedBy",
                table: "QuestionsGroup",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_CreatedBy",
                table: "Rating",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_User_CreatedBy",
                table: "Region",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelationshipType_User_CreatedBy",
                table: "RelationshipType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_CreatedBy",
                table: "Report",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservation_User_CreatedBy",
                table: "ReportObservation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationDocument_User_CreatedBy",
                table: "ReportObservationDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportObservationMember_User_CreatedBy",
                table: "ReportObservationMember",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportUserMapping_User_CreatedBy",
                table: "ReportUserMapping",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewerDocument_User_CreatedBy",
                table: "ReviewerDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessment_User_CreatedBy",
                table: "RiskAssessment",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskAssessmentDocument_User_CreatedBy",
                table: "RiskAssessmentDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrix_User_CreatedBy",
                table: "RiskControlMatrix",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixProcess_User_CreatedBy",
                table: "RiskControlMatrixProcess",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSector_User_CreatedBy",
                table: "RiskControlMatrixSector",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskControlMatrixSubProcess_User_CreatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAnalysisTeam_User_CreatedBy",
                table: "StrategicAnalysisTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyAnalysis_User_CreatedBy",
                table: "StrategyAnalysis",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyDocument_User_CreatedBy",
                table: "StrategyDocument",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPointOfDiscussion_User_CreatedBy",
                table: "SubPointOfDiscussion",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponse_User_CreatedBy",
                table: "UserResponse",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPaper_User_CreatedBy",
                table: "WorkPaper",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgram_User_CreatedBy",
                table: "WorkProgram",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkProgramTeam_User_CreatedBy",
                table: "WorkProgramTeam",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
