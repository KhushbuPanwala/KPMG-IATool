using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternalAuditSystem.DomailModel.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    Designation = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    LastInterectedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Country_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Country_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlMatrixProcess",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Process = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlMatrixProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixProcess_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixProcess_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlMatrixSector",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Sector = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlMatrixSector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixSector_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixSector_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlMatrixSubProcess",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SubProcess = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlMatrixSubProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixSubProcess_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrixSubProcess_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProvinceState",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvinceState_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProvinceState_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProvinceState_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACMDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    ACMPresentationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACMReportMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReprotId = table.Column<Guid>(nullable: false),
                    ACMPresentationId = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMReportMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMReportMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMReportMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACMUserTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ACMPresentationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMUserTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMUserTeam_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditCategory_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditCategory_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SelectedTypeId = table.Column<Guid>(nullable: false),
                    SelectCategoryId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    OverviewBackground = table.Column<string>(nullable: true),
                    TotalBudgetedHours = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPlan_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlan_AuditCategory_SelectCategoryId",
                        column: x => x.SelectCategoryId,
                        principalTable: "AuditCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlan_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditPlanDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlanId = table.Column<Guid>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPlanDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPlanDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlanDocument_AuditPlan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "AuditPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlanDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AuditTitle = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AuditPlanId = table.Column<Guid>(nullable: false),
                    AuditPeriodStartDate = table.Column<DateTime>(nullable: false),
                    AuditPeriodEndDate = table.Column<DateTime>(nullable: false),
                    Scope = table.Column<string>(nullable: true),
                    DraftIssues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkProgram_AuditPlan_AuditPlanId",
                        column: x => x.AuditPlanId,
                        principalTable: "AuditPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkProgram_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkProgram_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MomDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WorkProgramId = table.Column<Guid>(nullable: false),
                    MomDate = table.Column<DateTime>(nullable: false),
                    MomStartTime = table.Column<DateTime>(nullable: false),
                    ClosureMeetingDate = table.Column<DateTime>(nullable: false),
                    Agenda = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MomDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MomDetail_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomDetail_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomDetail_WorkProgram_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "WorkProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskControlMatrix",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WorkProgramId = table.Column<Guid>(nullable: false),
                    RiskName = table.Column<string>(nullable: true),
                    RiskDescription = table.Column<string>(nullable: true),
                    ControlCategory = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    ControlObjective = table.Column<string>(nullable: true),
                    ControlDescription = table.Column<string>(nullable: true),
                    NatureOfControl = table.Column<string>(nullable: true),
                    AntiFraudControl = table.Column<bool>(nullable: false),
                    SectorId = table.Column<Guid>(nullable: false),
                    RcmProcessId = table.Column<Guid>(nullable: false),
                    RCMProcessId = table.Column<Guid>(nullable: true),
                    RcmSubProcessId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskControlMatrix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_RiskControlMatrixProcess_RCMProcessId",
                        column: x => x.RCMProcessId,
                        principalTable: "RiskControlMatrixProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_RiskControlMatrixSubProcess_RcmSubProcess~",
                        column: x => x.RcmSubProcessId,
                        principalTable: "RiskControlMatrixSubProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_RiskControlMatrixSector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "RiskControlMatrixSector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskControlMatrix_WorkProgram_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "WorkProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPaper",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    WorkProgramId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPaper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPaper_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkPaper_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkPaper_WorkProgram_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "WorkProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkProgramTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    WorkProgramId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkProgramTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkProgramTeam_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkProgramTeam_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkProgramTeam_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkProgramTeam_WorkProgram_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "WorkProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainDiscussionPoint",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MomId = table.Column<Guid>(nullable: false),
                    MainPoint = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainDiscussionPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainDiscussionPoint_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MainDiscussionPoint_MomDetail_MomId",
                        column: x => x.MomId,
                        principalTable: "MomDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MainDiscussionPoint_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MomUserMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    MomId = table.Column<Guid>(nullable: false),
                    PointOfDiscussionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MomUserMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MomUserMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomUserMapping_MomDetail_MomId",
                        column: x => x.MomId,
                        principalTable: "MomDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomUserMapping_MainDiscussionPoint_PointOfDiscussionId",
                        column: x => x.PointOfDiscussionId,
                        principalTable: "MainDiscussionPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomUserMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomUserMapping_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubPointOfDiscussion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SubPoint = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MainPointId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubPointOfDiscussion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubPointOfDiscussion_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubPointOfDiscussion_MainDiscussionPoint_MainPointId",
                        column: x => x.MainPointId,
                        principalTable: "MainDiscussionPoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubPointOfDiscussion_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditType_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditableEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsStrategyAnalysisDone = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    SelectedTypeId = table.Column<Guid>(nullable: false),
                    SelectedCategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditableEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditableEntity_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                        column: x => x.SelectedCategoryId,
                        principalTable: "AuditCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                        column: x => x.SelectedTypeId,
                        principalTable: "AuditType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditableEntity_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Division_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Division_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Division_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCategory_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCategory_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityDocument_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TypeName = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityType_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityType_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityUserMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityUserMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityUserMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityUserMapping_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityUserMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityUserMapping_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Location_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Location_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationCategory_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationCategory_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationCategory_ObservationCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ObservationCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationCategory_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ScopeBasedOn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Process_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Process_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Process_Process_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Process_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Ratings = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    QualitativeFactors = table.Column<string>(nullable: true),
                    QuatitativeFactors = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    Legand = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelationshipType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipType_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelationshipType_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskAssessment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAssessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAssessment_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskAssessment_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskAssessment_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskAssessmentDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAssessmentDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAssessmentDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskAssessmentDocument_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskAssessmentDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrategyAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SurveyTitle = table.Column<string>(nullable: true),
                    AuditableEntityName = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    IsSampling = table.Column<bool>(nullable: false),
                    AuditableEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrategyAnalysis_AuditableEntity_AuditableEntityId",
                        column: x => x.AuditableEntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategyAnalysis_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategyAnalysis_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Heading = table.Column<string>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    RcmId = table.Column<Guid>(nullable: false),
                    Observations = table.Column<string>(nullable: true),
                    ObservationType = table.Column<int>(nullable: false),
                    ObservationCategoryId = table.Column<Guid>(nullable: false),
                    IsRepeatObservation = table.Column<bool>(nullable: false),
                    RootCause = table.Column<string>(nullable: true),
                    Implication = table.Column<string>(nullable: true),
                    Disposition = table.Column<int>(nullable: false),
                    ObservationStatus = table.Column<int>(nullable: false),
                    Recommendation = table.Column<string>(nullable: true),
                    ManagementResponse = table.Column<string>(nullable: true),
                    PersonResponsible = table.Column<Guid>(nullable: false),
                    RelatedObservation = table.Column<string>(nullable: true),
                    ProcessId = table.Column<Guid>(nullable: false),
                    SubProcessId = table.Column<Guid>(nullable: false),
                    RiskAndControlId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observation_ObservationCategory_ObservationCategoryId",
                        column: x => x.ObservationCategoryId,
                        principalTable: "ObservationCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observation_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observation_RiskControlMatrix_RiskAndControlId",
                        column: x => x.RiskAndControlId,
                        principalTable: "RiskControlMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Observation_Process_SubProcessId",
                        column: x => x.SubProcessId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observation_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanProcessMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlanId = table.Column<Guid>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkProgramId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanProcessMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanProcessMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcessMapping_AuditPlan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "AuditPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcessMapping_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcessMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcessMapping_WorkProgram_WorkProgramId",
                        column: x => x.WorkProgramId,
                        principalTable: "WorkProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportTitle = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    RatingId = table.Column<Guid>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    AuditPeriodStartDate = table.Column<DateTime>(nullable: false),
                    AuditPeriodEndDate = table.Column<DateTime>(nullable: false),
                    AuditStatus = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_Rating_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Rating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityCountry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RegionId = table.Column<Guid>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityCountry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCountry_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCountry_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCountry_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityCountry_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityRelationMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    RelationTypeId = table.Column<Guid>(nullable: false),
                    RelatedEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityRelationMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityRelationMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityRelationMapping_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityRelationMapping_RelationshipType_RelationTypeId",
                        column: x => x.RelationTypeId,
                        principalTable: "RelationshipType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityRelationMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsOtherToBeShown = table.Column<bool>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    Representation = table.Column<string>(nullable: true),
                    ScaleStart = table.Column<int>(nullable: false),
                    ScaleEnd = table.Column<int>(nullable: false),
                    StartLabel = table.Column<string>(nullable: true),
                    EndLabel = table.Column<string>(nullable: true),
                    CharacterLowerLimit = table.Column<int>(nullable: false),
                    CharacterUpperLimit = table.Column<int>(nullable: false),
                    Guidance = table.Column<string>(nullable: true),
                    IsDocAllowed = table.Column<bool>(nullable: false),
                    IsPdfAllowed = table.Column<bool>(nullable: false),
                    IsPngAllowed = table.Column<bool>(nullable: false),
                    IsPpxAllowed = table.Column<bool>(nullable: false),
                    IsJpegAllowed = table.Column<bool>(nullable: false),
                    IsGifAllowed = table.Column<bool>(nullable: false),
                    StrategyAnalysisId = table.Column<Guid>(nullable: false),
                    RelatedAnswer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsGroup_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrategicAnalysisTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StrategAnalysyId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    StrategyAnalysisId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategicAnalysisTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrategicAnalysisTeam_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategicAnalysisTeam_StrategyAnalysis_StrategyAnalysisId",
                        column: x => x.StrategyAnalysisId,
                        principalTable: "StrategyAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StrategicAnalysisTeam_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategicAnalysisTeam_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObservationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    DocumentFor = table.Column<int>(nullable: false),
                    ObservationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationDocuments_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationDocuments_Observation_ObservationId",
                        column: x => x.ObservationId,
                        principalTable: "Observation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationDocuments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACMPresentation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    Heading = table.Column<string>(nullable: true),
                    Recommendation = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    Implication = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ACMReportTitle = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    ACMReportStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACMPresentation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ACMPresentation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMPresentation_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ACMPresentation_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportObservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReprotId = table.Column<Guid>(nullable: false),
                    ObservationId = table.Column<Guid>(nullable: false),
                    ProcessId = table.Column<Guid>(nullable: false),
                    ObservationCategoryId = table.Column<Guid>(nullable: false),
                    ObservationTitle = table.Column<string>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    Observation = table.Column<string>(nullable: true),
                    ObasevationRating = table.Column<int>(nullable: false),
                    TypeOfObservation = table.Column<int>(nullable: false),
                    IsRepeatObservation = table.Column<bool>(nullable: false),
                    RootCause = table.Column<string>(nullable: true),
                    Implication = table.Column<string>(nullable: true),
                    Disposition = table.Column<int>(nullable: false),
                    ObservationStatus = table.Column<int>(nullable: false),
                    Recommendation = table.Column<string>(nullable: true),
                    ManagementResponse = table.Column<string>(nullable: true),
                    TargetDate = table.Column<DateTime>(nullable: false),
                    LinkedObservation = table.Column<string>(nullable: true),
                    Conclusion = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportObservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportObservation_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservation_ObservationCategory_ObservationCategoryId",
                        column: x => x.ObservationCategoryId,
                        principalTable: "ObservationCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservation_Observation_ObservationId",
                        column: x => x.ObservationId,
                        principalTable: "Observation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservation_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservation_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportObservation_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ReportUserType = table.Column<int>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportUserMapping_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportUserMapping_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportUserMapping_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportUserMapping_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityState",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EntityCountryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: false),
                    StateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityState_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityState_EntityCountry_EntityCountryId",
                        column: x => x.EntityCountryId,
                        principalTable: "EntityCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityState_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityState_ProvinceState_StateId",
                        column: x => x.StateId,
                        principalTable: "ProvinceState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityState_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OptionText = table.Column<string>(nullable: true),
                    QuestionId = table.Column<Guid>(nullable: false),
                    QuestionsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Option_QuestionsGroup_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "QuestionsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Option_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    OptionChosen = table.Column<int>(nullable: false),
                    Other = table.Column<string>(nullable: true),
                    RepresentationNumber = table.Column<int>(nullable: false),
                    AnswerText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResponse_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserResponse_QuestionsGroup_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserResponse_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserResponse_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportObservationDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportObservationId = table.Column<Guid>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    DocumentFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportObservationDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportObservationDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationDocument_ReportObservation_ReportObservati~",
                        column: x => x.ReportObservationId,
                        principalTable: "ReportObservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportObservationMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ReportObservationId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportObservationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportObservationMember_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationMember_ReportObservation_ReportObservation~",
                        column: x => x.ReportObservationId,
                        principalTable: "ReportObservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationMember_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportObservationMember_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewerDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentPath = table.Column<string>(nullable: true),
                    ReportUserMappingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewerDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewerDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewerDocument_ReportUserMapping_ReportUserMappingId",
                        column: x => x.ReportUserMappingId,
                        principalTable: "ReportUserMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewerDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryGeograhcialArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    RegionId = table.Column<Guid>(nullable: false),
                    EntityCountryId = table.Column<Guid>(nullable: false),
                    EntityStateId = table.Column<Guid>(nullable: false),
                    LocationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryGeograhcialArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_EntityCountry_EntityCountryId",
                        column: x => x.EntityCountryId,
                        principalTable: "EntityCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_AuditableEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AuditableEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_EntityState_EntityStateId",
                        column: x => x.EntityStateId,
                        principalTable: "EntityState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryGeograhcialArea_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrategyDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    UserResponseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StrategyDocument_UserResponse_UserResponseId",
                        column: x => x.UserResponseId,
                        principalTable: "UserResponse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_CreatedBy",
                table: "ACMDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMDocument_UpdatedBy",
                table: "ACMDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMPresentation_CreatedBy",
                table: "ACMPresentation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMPresentation_ReportId",
                table: "ACMPresentation",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMPresentation_UpdatedBy",
                table: "ACMPresentation",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ACMPresentationId",
                table: "ACMReportMapping",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_CreatedBy",
                table: "ACMReportMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_ReportId",
                table: "ACMReportMapping",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMReportMapping_UpdatedBy",
                table: "ACMReportMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_ACMPresentationId",
                table: "ACMUserTeam",
                column: "ACMPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_CreatedBy",
                table: "ACMUserTeam",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_UpdatedBy",
                table: "ACMUserTeam",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ACMUserTeam_UserId",
                table: "ACMUserTeam",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditableEntity_CreatedBy",
                table: "AuditableEntity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditableEntity_SelectedCategoryId",
                table: "AuditableEntity",
                column: "SelectedCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditableEntity_SelectedTypeId",
                table: "AuditableEntity",
                column: "SelectedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditableEntity_UpdatedBy",
                table: "AuditableEntity",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditCategory_CreatedBy",
                table: "AuditCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditCategory_EntityId",
                table: "AuditCategory",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditCategory_UpdatedBy",
                table: "AuditCategory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_CreatedBy",
                table: "AuditPlan",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_EntityId",
                table: "AuditPlan",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_SelectCategoryId",
                table: "AuditPlan",
                column: "SelectCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_SelectedTypeId",
                table: "AuditPlan",
                column: "SelectedTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_UpdatedBy",
                table: "AuditPlan",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlanDocument_CreatedBy",
                table: "AuditPlanDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlanDocument_PlanId",
                table: "AuditPlanDocument",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlanDocument_UpdatedBy",
                table: "AuditPlanDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditType_CreatedBy",
                table: "AuditType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditType_EntityId",
                table: "AuditType",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditType_UpdatedBy",
                table: "AuditType",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CreatedBy",
                table: "Country",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Country_UpdatedBy",
                table: "Country",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Division_CreatedBy",
                table: "Division",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Division_EntityId",
                table: "Division",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Division_UpdatedBy",
                table: "Division",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory_CreatedBy",
                table: "EntityCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory_EntityId",
                table: "EntityCategory",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCategory_UpdatedBy",
                table: "EntityCategory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCountry_CountryId",
                table: "EntityCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCountry_CreatedBy",
                table: "EntityCountry",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCountry_EntityId",
                table: "EntityCountry",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCountry_RegionId",
                table: "EntityCountry",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityCountry_UpdatedBy",
                table: "EntityCountry",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument_CreatedBy",
                table: "EntityDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument_EntityId",
                table: "EntityDocument",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityDocument_UpdatedBy",
                table: "EntityDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationMapping_CreatedBy",
                table: "EntityRelationMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationMapping_EntityId",
                table: "EntityRelationMapping",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationMapping_RelationTypeId",
                table: "EntityRelationMapping",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationMapping_UpdatedBy",
                table: "EntityRelationMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityState_CreatedBy",
                table: "EntityState",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityState_EntityCountryId",
                table: "EntityState",
                column: "EntityCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityState_EntityId",
                table: "EntityState",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityState_StateId",
                table: "EntityState",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityState_UpdatedBy",
                table: "EntityState",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityType_CreatedBy",
                table: "EntityType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityType_EntityId",
                table: "EntityType",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityType_UpdatedBy",
                table: "EntityType",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityUserMapping_CreatedBy",
                table: "EntityUserMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityUserMapping_EntityId",
                table: "EntityUserMapping",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityUserMapping_UpdatedBy",
                table: "EntityUserMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityUserMapping_UserId",
                table: "EntityUserMapping",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CreatedBy",
                table: "Location",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Location_EntityId",
                table: "Location",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_UpdatedBy",
                table: "Location",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MainDiscussionPoint_CreatedBy",
                table: "MainDiscussionPoint",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MainDiscussionPoint_MomId",
                table: "MainDiscussionPoint",
                column: "MomId");

            migrationBuilder.CreateIndex(
                name: "IX_MainDiscussionPoint_UpdatedBy",
                table: "MainDiscussionPoint",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MomDetail_CreatedBy",
                table: "MomDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MomDetail_UpdatedBy",
                table: "MomDetail",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MomDetail_WorkProgramId",
                table: "MomDetail",
                column: "WorkProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_CreatedBy",
                table: "MomUserMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_MomId",
                table: "MomUserMapping",
                column: "MomId");

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_PointOfDiscussionId",
                table: "MomUserMapping",
                column: "PointOfDiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_UpdatedBy",
                table: "MomUserMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MomUserMapping_UserId",
                table: "MomUserMapping",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_CreatedBy",
                table: "Observation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ObservationCategoryId",
                table: "Observation",
                column: "ObservationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ProcessId",
                table: "Observation",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_RiskAndControlId",
                table: "Observation",
                column: "RiskAndControlId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_SubProcessId",
                table: "Observation",
                column: "SubProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_UpdatedBy",
                table: "Observation",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationCategory_CreatedBy",
                table: "ObservationCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationCategory_EntityId",
                table: "ObservationCategory",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationCategory_ParentId",
                table: "ObservationCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationCategory_UpdatedBy",
                table: "ObservationCategory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationDocuments_CreatedBy",
                table: "ObservationDocuments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationDocuments_ObservationId",
                table: "ObservationDocuments",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ObservationDocuments_UpdatedBy",
                table: "ObservationDocuments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Option_CreatedBy",
                table: "Option",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Option_QuestionsId",
                table: "Option",
                column: "QuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_UpdatedBy",
                table: "Option",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_CreatedBy",
                table: "PlanProcessMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_PlanId",
                table: "PlanProcessMapping",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_ProcessId",
                table: "PlanProcessMapping",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_UpdatedBy",
                table: "PlanProcessMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcessMapping_WorkProgramId",
                table: "PlanProcessMapping",
                column: "WorkProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_CreatedBy",
                table: "PrimaryGeograhcialArea",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_EntityCountryId",
                table: "PrimaryGeograhcialArea",
                column: "EntityCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_EntityId",
                table: "PrimaryGeograhcialArea",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_EntityStateId",
                table: "PrimaryGeograhcialArea",
                column: "EntityStateId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_LocationId",
                table: "PrimaryGeograhcialArea",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_RegionId",
                table: "PrimaryGeograhcialArea",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryGeograhcialArea_UpdatedBy",
                table: "PrimaryGeograhcialArea",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Process_CreatedBy",
                table: "Process",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Process_EntityId",
                table: "Process",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_ParentId",
                table: "Process",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_UpdatedBy",
                table: "Process",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceState_CountryId",
                table: "ProvinceState",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceState_CreatedBy",
                table: "ProvinceState",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProvinceState_UpdatedBy",
                table: "ProvinceState",
                column: "UpdatedBy");

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
                name: "IX_Rating_CreatedBy",
                table: "Rating",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_EntityId",
                table: "Rating",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UpdatedBy",
                table: "Rating",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CreatedBy",
                table: "Region",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Region_EntityId",
                table: "Region",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_UpdatedBy",
                table: "Region",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_CreatedBy",
                table: "RelationshipType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_EntityId",
                table: "RelationshipType",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipType_UpdatedBy",
                table: "RelationshipType",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Report_CreatedBy",
                table: "Report",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EntityId",
                table: "Report",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_RatingId",
                table: "Report",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_UpdatedBy",
                table: "Report",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_CreatedBy",
                table: "ReportObservation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_ObservationCategoryId",
                table: "ReportObservation",
                column: "ObservationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_ObservationId",
                table: "ReportObservation",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_ProcessId",
                table: "ReportObservation",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_ReportId",
                table: "ReportObservation",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservation_UpdatedBy",
                table: "ReportObservation",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationDocument_CreatedBy",
                table: "ReportObservationDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationDocument_ReportObservationId",
                table: "ReportObservationDocument",
                column: "ReportObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationDocument_UpdatedBy",
                table: "ReportObservationDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationMember_CreatedBy",
                table: "ReportObservationMember",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationMember_ReportObservationId",
                table: "ReportObservationMember",
                column: "ReportObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationMember_UpdatedBy",
                table: "ReportObservationMember",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportObservationMember_UserId",
                table: "ReportObservationMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUserMapping_CreatedBy",
                table: "ReportUserMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUserMapping_ReportId",
                table: "ReportUserMapping",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUserMapping_UpdatedBy",
                table: "ReportUserMapping",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportUserMapping_UserId",
                table: "ReportUserMapping",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerDocument_CreatedBy",
                table: "ReviewerDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerDocument_ReportUserMappingId",
                table: "ReviewerDocument",
                column: "ReportUserMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewerDocument_UpdatedBy",
                table: "ReviewerDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessment_CreatedBy",
                table: "RiskAssessment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessment_EntityId",
                table: "RiskAssessment",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessment_UpdatedBy",
                table: "RiskAssessment",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessmentDocument_CreatedBy",
                table: "RiskAssessmentDocument",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessmentDocument_EntityId",
                table: "RiskAssessmentDocument",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAssessmentDocument_UpdatedBy",
                table: "RiskAssessmentDocument",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_CreatedBy",
                table: "RiskControlMatrix",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_RCMProcessId",
                table: "RiskControlMatrix",
                column: "RCMProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_RcmSubProcessId",
                table: "RiskControlMatrix",
                column: "RcmSubProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_SectorId",
                table: "RiskControlMatrix",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_UpdatedBy",
                table: "RiskControlMatrix",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrix_WorkProgramId",
                table: "RiskControlMatrix",
                column: "WorkProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixProcess_CreatedBy",
                table: "RiskControlMatrixProcess",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixProcess_UpdatedBy",
                table: "RiskControlMatrixProcess",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSector_CreatedBy",
                table: "RiskControlMatrixSector",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSector_UpdatedBy",
                table: "RiskControlMatrixSector",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSubProcess_CreatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RiskControlMatrixSubProcess_UpdatedBy",
                table: "RiskControlMatrixSubProcess",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_CreatedBy",
                table: "StrategicAnalysisTeam",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_StrategyAnalysisId",
                table: "StrategicAnalysisTeam",
                column: "StrategyAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_UpdatedBy",
                table: "StrategicAnalysisTeam",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAnalysisTeam_UserId",
                table: "StrategicAnalysisTeam",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyAnalysis_AuditableEntityId",
                table: "StrategyAnalysis",
                column: "AuditableEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyAnalysis_CreatedBy",
                table: "StrategyAnalysis",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyAnalysis_UpdatedBy",
                table: "StrategyAnalysis",
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

            migrationBuilder.CreateIndex(
                name: "IX_SubPointOfDiscussion_CreatedBy",
                table: "SubPointOfDiscussion",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubPointOfDiscussion_MainPointId",
                table: "SubPointOfDiscussion",
                column: "MainPointId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPointOfDiscussion_UpdatedBy",
                table: "SubPointOfDiscussion",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_CreatedBy",
                table: "UserResponse",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_QuestionId",
                table: "UserResponse",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_UpdatedBy",
                table: "UserResponse",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserResponse_UserId",
                table: "UserResponse",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPaper_CreatedBy",
                table: "WorkPaper",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPaper_UpdatedBy",
                table: "WorkPaper",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPaper_WorkProgramId",
                table: "WorkPaper",
                column: "WorkProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgram_AuditPlanId",
                table: "WorkProgram",
                column: "AuditPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgram_CreatedBy",
                table: "WorkProgram",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgram_UpdatedBy",
                table: "WorkProgram",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgramTeam_CreatedBy",
                table: "WorkProgramTeam",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgramTeam_UpdatedBy",
                table: "WorkProgramTeam",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgramTeam_UserId",
                table: "WorkProgramTeam",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkProgramTeam_WorkProgramId",
                table: "WorkProgramTeam",
                column: "WorkProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ACMDocument_ACMPresentation_ACMPresentationId",
                table: "ACMDocument",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_ACMPresentation_ACMPresentationId",
                table: "ACMReportMapping",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMReportMapping_Report_ReportId",
                table: "ACMReportMapping",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ACMUserTeam_ACMPresentation_ACMPresentationId",
                table: "ACMUserTeam",
                column: "ACMPresentationId",
                principalTable: "ACMPresentation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditCategory_AuditableEntity_EntityId",
                table: "AuditCategory",
                column: "EntityId",
                principalTable: "AuditableEntity",
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
                name: "FK_AuditPlan_AuditableEntity_EntityId",
                table: "AuditPlan",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditType_AuditableEntity_EntityId",
                table: "AuditType",
                column: "EntityId",
                principalTable: "AuditableEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_CreatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_User_UpdatedBy",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_CreatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditCategory_User_UpdatedBy",
                table: "AuditCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_CreatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditType_User_UpdatedBy",
                table: "AuditType");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditCategory_SelectedCategoryId",
                table: "AuditableEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditableEntity_AuditType_SelectedTypeId",
                table: "AuditableEntity");

            migrationBuilder.DropTable(
                name: "ACMDocument");

            migrationBuilder.DropTable(
                name: "ACMReportMapping");

            migrationBuilder.DropTable(
                name: "ACMUserTeam");

            migrationBuilder.DropTable(
                name: "AuditPlanDocument");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropTable(
                name: "EntityCategory");

            migrationBuilder.DropTable(
                name: "EntityDocument");

            migrationBuilder.DropTable(
                name: "EntityRelationMapping");

            migrationBuilder.DropTable(
                name: "EntityType");

            migrationBuilder.DropTable(
                name: "EntityUserMapping");

            migrationBuilder.DropTable(
                name: "MomUserMapping");

            migrationBuilder.DropTable(
                name: "ObservationDocuments");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "PlanProcessMapping");

            migrationBuilder.DropTable(
                name: "PrimaryGeograhcialArea");

            migrationBuilder.DropTable(
                name: "ReportObservationDocument");

            migrationBuilder.DropTable(
                name: "ReportObservationMember");

            migrationBuilder.DropTable(
                name: "ReviewerDocument");

            migrationBuilder.DropTable(
                name: "RiskAssessment");

            migrationBuilder.DropTable(
                name: "RiskAssessmentDocument");

            migrationBuilder.DropTable(
                name: "StrategicAnalysisTeam");

            migrationBuilder.DropTable(
                name: "StrategyDocument");

            migrationBuilder.DropTable(
                name: "SubPointOfDiscussion");

            migrationBuilder.DropTable(
                name: "WorkPaper");

            migrationBuilder.DropTable(
                name: "WorkProgramTeam");

            migrationBuilder.DropTable(
                name: "ACMPresentation");

            migrationBuilder.DropTable(
                name: "RelationshipType");

            migrationBuilder.DropTable(
                name: "EntityState");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "ReportObservation");

            migrationBuilder.DropTable(
                name: "ReportUserMapping");

            migrationBuilder.DropTable(
                name: "UserResponse");

            migrationBuilder.DropTable(
                name: "MainDiscussionPoint");

            migrationBuilder.DropTable(
                name: "EntityCountry");

            migrationBuilder.DropTable(
                name: "ProvinceState");

            migrationBuilder.DropTable(
                name: "Observation");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "QuestionsGroup");

            migrationBuilder.DropTable(
                name: "MomDetail");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "ObservationCategory");

            migrationBuilder.DropTable(
                name: "Process");

            migrationBuilder.DropTable(
                name: "RiskControlMatrix");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "StrategyAnalysis");

            migrationBuilder.DropTable(
                name: "RiskControlMatrixProcess");

            migrationBuilder.DropTable(
                name: "RiskControlMatrixSubProcess");

            migrationBuilder.DropTable(
                name: "RiskControlMatrixSector");

            migrationBuilder.DropTable(
                name: "WorkProgram");

            migrationBuilder.DropTable(
                name: "AuditPlan");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AuditCategory");

            migrationBuilder.DropTable(
                name: "AuditType");

            migrationBuilder.DropTable(
                name: "AuditableEntity");
        }
    }
}
