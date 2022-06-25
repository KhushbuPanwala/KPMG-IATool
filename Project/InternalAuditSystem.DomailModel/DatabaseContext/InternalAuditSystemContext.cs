using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomainModel.Models.AzureAdModel;
using InternalAuditSystem.DomainModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using Microsoft.EntityFrameworkCore;

namespace InternalAuditSystem.DomailModel.DatabaseContext
{
    public class InternalAuditSystemContext : DbContext
    {
        public InternalAuditSystemContext(DbContextOptions options) : base(options)
        {

        }

        #region AuditableEntity
        public DbSet<AuditableEntity> AuditableEntity { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<EntityCategory> EntityCategory { get; set; }
        public DbSet<EntityCountry> EntityCountry { get; set; }
        public DbSet<EntityDocument> EntityDocument { get; set; }
        public DbSet<EntityRelationMapping> EntityRelationMapping { get; set; }
        public DbSet<Models.AuditableEntityModels.EntityState> EntityState { get; set; }
        public DbSet<EntityType> EntityType { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<PrimaryGeographicalArea> PrimaryGeograhcialArea { get; set; }
        public DbSet<ProvinceState> ProvinceState { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<RelationshipType> RelationshipType { get; set; }
        public DbSet<RiskAssessment> RiskAssessment { get; set; }
        public DbSet<RiskAssessmentDocument> RiskAssessmentDocument { get; set; }
        public DbSet<EntityUserMapping> EntityUserMapping { get; set; }
        #endregion

        #region AuditPlan
        public DbSet<AuditCategory> AuditCategory { get; set; }
        public DbSet<AuditPlan> AuditPlan { get; set; }
        public DbSet<AuditPlanDocument> AuditPlanDocument { get; set; }
        public DbSet<AuditType> AuditType { get; set; }
        public DbSet<PlanProcessMapping> PlanProcessMapping { get; set; }
        public DbSet<Process> Process { get; set; }
        #endregion

        #region Mom
        public DbSet<MainDiscussionPoint> MainDiscussionPoint { get; set; }
        public DbSet<Mom> MomDetail { get; set; }
        public DbSet<MomUserMapping> MomUserMapping { get; set; }
        public DbSet<SubPointOfDiscussion> SubPointOfDiscussion { get; set; }

        #endregion

        #region Observation
        public DbSet<Observation> Observation { get; set; }
        public DbSet<ObservationCategory> ObservationCategory { get; set; }
        public DbSet<ObservationDocument> ObservationDocuments { get; set; }

        #endregion

        #region Report
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Report> Report { get; set; }

        public DbSet<ReportObservation> ReportObservation { get; set; }
        public DbSet<ReportObservationsDocument> ReportObservationDocument { get; set; }
        public DbSet<ReportObservationsMember> ReportObservationMember { get; set; }
        public DbSet<ReportUserMapping> ReportUserMapping { get; set; }
        public DbSet<ReviewerDocument> ReviewerDocument { get; set; }
        public DbSet<Distributors> Distributor { get; set; }
        public DbSet<ReportComment> ReportComment { get; set; }
        public DbSet<ReportObservationReviewer> ReportObservationReviewer { get; set; }
        public DbSet<ReportObservationTable> ReportObservationTable { get; set; }
        #endregion

        #region RiskControlMatrix
        public DbSet<RiskControlMatrix> RiskControlMatrix { get; set; }
        public DbSet<RiskControlMatrixSector> RiskControlMatrixSector { get; set; }
        public DbSet<RiskControlMatrixProcess> RiskControlMatrixProcess { get; set; }
        public DbSet<RiskControlMatrixSubProcess> RiskControlMatrixSubProcess { get; set; }

        #endregion

        #region StrategicAnalysis
        public DbSet<Option> Option { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<StrategicAnalysis> StrategyAnalysis { get; set; }
        public DbSet<StrategicAnalysisTeam> StrategicAnalysisTeam { get; set; }
        public DbSet<UserResponseDocument> UserResponseDocument { get; set; }
        public DbSet<UserResponse> UserResponse { get; set; }
        public DbSet<CheckboxQuestion> CheckboxQuestion { get; set; }
        public DbSet<DropdownQuestion> DropdownQuestion { get; set; }
        public DbSet<FileUploadQuestion> FileUploadQuestion { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestion { get; set; }
        public DbSet<RatingScaleQuestion> RatingScaleQuestion { get; set; }
        public DbSet<SubjectiveQuestion> SubjectiveQuestion { get; set; }
        public DbSet<TextboxQuestion> TextboxQuestion { get; set; }


        #endregion

        #region User
        public DbSet<User> User { get; set; }

        #endregion

        #region WorkProgram
        public DbSet<WorkProgramTeam> WorkProgramTeam { get; set; }
        public DbSet<WorkProgram> WorkProgram { get; set; }
        public DbSet<WorkPaper> WorkPaper { get; set; }
        #endregion

        #region ACM Presentation
        public DbSet<ACMPresentation> ACMPresentation { get; set; }
        public DbSet<ACMDocument> ACMDocument { get; set; }
        public DbSet<ACMReportMapping> ACMReportMapping { get; set; }
        public DbSet<ACMReviewer> ACMReviewer { get; set; }
        public DbSet<ACMTable> ACMTable { get; set; }
        public DbSet<ACMReviewerDocument> ACMReviewerDocument { get; set; }

        public DbSet<ACMReportDetail> ACMReportDetail { get; set; }
        #endregion

        #region Azure Ad
        public DbSet<AzureAccessToken> AzureAccessToken { get; set; }
        #endregion
    }
}
