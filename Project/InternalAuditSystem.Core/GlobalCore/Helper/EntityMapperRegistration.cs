
using AutoMapper;
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
using InternalAuditSystem.DomainModel.Models;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomainModel.Models.AzureAdModel;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;
using InternalAuditSystem.DomainModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternalAuditSystem.Core.GlobalCore.Helper
{
    public class EntityMapperRegistration : Profile
    {
        public EntityMapperRegistration()
        {
            CreateMap<BaseModel, BaseModelAC>().ReverseMap();

            CreateMap<StrategicAnalysis, StrategicAnalysisAC>()
            .ForMember(cv => cv.UserName, m => m.MapFrom(s => s.UserCreatedBy.Name.ToString()))
            .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.UserCreatedBy.Designation.ToString()))
            .ForMember(cv => cv.Email, m => m.MapFrom(s => s.UserCreatedBy.EmailId.ToString()))
            .ReverseMap();
            CreateMap<StrategicAnalysisTeam, StrategicAnalysisTeamAC>().ReverseMap();
            CreateMap<StrategicAnalysisTeam, StrategicAnalysisTeamAC>()
            .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString()))
            .ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
            .ForMember(cv => cv.EmailId, m => m.MapFrom(s => s.User.EmailId.ToString()))
            .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime));

            CreateMap<Question, QuestionAC>()
                .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime))
                .ReverseMap();
            CreateMap<DropdownQuestion, DropdownQuestionAC>().ReverseMap();
            CreateMap<TextboxQuestion, TextboxQuestionAC>().ReverseMap();
            CreateMap<MultipleChoiceQuestionAC, MultipleChoiceQuestion>().ReverseMap();
            CreateMap<RatingScaleQuestionAC, RatingScaleQuestion>().ReverseMap();
            CreateMap<SubjectiveQuestionAC, SubjectiveQuestion>().ReverseMap();
            CreateMap<TextboxQuestionAC, TextboxQuestion>().ReverseMap();
            CreateMap<FileUploadQuestionAC, FileUploadQuestion>().ReverseMap();
            CreateMap<CheckboxQuestionAC, CheckboxQuestion>().ReverseMap();
            CreateMap<UserResponse, UserResponseAC>()
                .ForMember(cv => cv.AuditableEntityName, m => m.MapFrom(s => s.AuditableEntity.Name.ToString()))
                 .ForMember(cv => cv.UserResponseStatusString, m => m.MapFrom(s => s.SamplingResponseStatus.ToString()))
                 .ReverseMap();
            CreateMap<UserResponse, UserResponseForDetailsAndEstimatedValueOfOpportunity>().ReverseMap();
            CreateMap<UserResponseDocument, UserResponseDocumentAC>().ReverseMap();
            CreateMap<Option, OptionAC>().ReverseMap();
            CreateMap<IQueryable<UserResponse>, IQueryable<UserResponseAC>>().ReverseMap();
            CreateMap<WorkProgram, WorkProgramAC>()
                .ForMember(cv => cv.StatusString, m => m.MapFrom(s => s.Status.ToString())).ReverseMap();
            CreateMap<WorkProgramTeam, WorkProgramTeamAC>()
                .ForMember(cv => cv.WorkProgramName, m => m.MapFrom(s => s.WorkProgram.Name.ToString()))
                .ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString())).ReverseMap();
            CreateMap<WorkPaper, WorkPaperAC>().ReverseMap();
            CreateMap<User, UserAC>()
                .ForMember(cv => cv.UserType, m => m.MapFrom(s => s.UserType.ToString())).ReverseMap();
            CreateMap<RiskControlMatrixSector, RcmSectorAC>().ReverseMap();
            CreateMap<RiskControlMatrixSubProcess, RcmSubProcessAC>().ReverseMap();
            CreateMap<RiskControlMatrixProcess, RcmProcessAC>().ReverseMap();
            CreateMap<Distributors, DistributorsAC>().ReverseMap();

            CreateMap<ACMReportMapping, ACMReportMappingAC>().ReverseMap();
            CreateMap<ACMReportDetail, ACMReportDetailAC>().ForMember(cv => cv.acmReportTitle, m => m.MapFrom(s => s.ReportTitle.ToString())).ReverseMap();

            CreateMap<ACMReviewerDocument, ACMReviewerDocumentAC>().ReverseMap();
            CreateMap<ACMReviewer, ACMReviewerAC>()
                .ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString()))
                .ReverseMap();
            CreateMap<EntityUserMapping, EntityUserMappingAC>().
                ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
                .ForMember(cv => cv.EmailId, m => m.MapFrom(s => s.User.EmailId.ToString()))
                .ForMember(cv => cv.UserType, m => m.MapFrom(s => s.User.UserType.ToString()))
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString())).ReverseMap();


            CreateMap<Mom, MomAC>()
               .ForMember(cv => cv.WorkProgramAc, m => m.MapFrom(s => s.WorkProgram))
               .ForMember(cv => cv.AuditableEntityAc, m => m.MapFrom(s => s.AuditableEntity))
              .ForMember(cv => cv.MomDate, m => m.MapFrom(s => s.MomDate))
              .ForMember(cv => cv.MomStartTime, m => m.MapFrom(s => s.MomStartTime))
              .ForMember(cv => cv.MomEndTime, m => m.MapFrom(s => s.MomEndTime))
               .ForMember(cv => cv.ClosureMeetingDate, m => m.MapFrom(s => s.ClosureMeetingDate))
                .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime))
              .ForMember(cv => cv.MainDiscussionPointACCollection, m => m.MapFrom(s => s.MainDiscussionPointCollection));

            CreateMap<MomAC, Mom>()
            .ForMember(cv => cv.AuditableEntity, m => m.MapFrom(s => s.AuditableEntityAc))
            .ForMember(cv => cv.MomDate, m => m.MapFrom(s => s.MomDate))
             .ForMember(cv => cv.MomStartTime, m => m.MapFrom(s => s.MomStartTime))
             .ForMember(cv => cv.MomEndTime, m => m.MapFrom(s => s.MomEndTime))
              .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime))
              .ForMember(cv => cv.ClosureMeetingDate, m => m.MapFrom(s => s.ClosureMeetingDate));


            CreateMap<MainDiscussionPoint, MainDiscussionPointAC>()
              .ForMember(cv => cv.SubPointDiscussionACCollection, m => m.MapFrom(s => s.SubPointDiscussionCollection))
               .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime))
                .ForMember(cv => cv.UpdatedDateTime, m => m.MapFrom(s => s.UpdatedDateTime))
              .ReverseMap();


            CreateMap<SubPointOfDiscussion, SubPointOfDiscussionAC>()
                .ForMember(cv => cv.PersonResponsibleACCollection, m => m.MapFrom(s => s.PersonResponsibleCollection))
                 .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime))
                .ForMember(cv => cv.TargetDate, m => m.MapFrom(s => s.TargetDate))
                .ForMember(cv => cv.UpdatedDateTime, m => m.MapFrom(s => s.UpdatedDateTime))
                .ReverseMap();




            CreateMap<MomUserMapping, MomUserMappingAC>()
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString()))
                 .ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
             .ForMember(cv => cv.CreatedDateTime, m => m.MapFrom(s => s.CreatedDateTime));
            CreateMap<MomUserMappingAC, MomUserMapping>();



            #region Audit Report 
            CreateMap<Rating, RatingAC>().ReverseMap();
            CreateMap<Report, ReportAC>().ForMember(cv => cv.Ratings, m => m.MapFrom(s => s.Rating.Ratings.ToString())).ReverseMap();
            CreateMap<Distributors, DistributorsAC>().ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
             .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString())).ReverseMap();
            CreateMap<ReportUserMapping, ReportUserMappingAC>().ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString()))
                .ForMember(cv => cv.Email, m => m.MapFrom(s => s.User.EmailId.ToString()))
                    .ForMember(cv => cv.ReportTitle, m => m.MapFrom(s => s.Report.ReportTitle.ToString())).ReverseMap();
            CreateMap<ReportObservationsMember, ReportObservationsMemberAC>().ForMember(cv => cv.Name, m => m.MapFrom(s => s.User.Name.ToString()))
                .ForMember(cv => cv.Designation, m => m.MapFrom(s => s.User.Designation.ToString()))
                .ForMember(cv => cv.EmailId, m => m.MapFrom(s => s.User.EmailId.ToString()))
                    .ForMember(cv => cv.ObservationTitle, m => m.MapFrom(s => s.ReportObservation.Heading.ToString()))
                .ReverseMap();

            CreateMap<Observation, ObservationAC>()
                .ForMember(cv => cv.ObservationTypeToString, m => m.MapFrom(s => s.ObservationType.ToString()))
                .ReverseMap();
            CreateMap<ObservationCategory, ObservationCategoryAC>().ReverseMap();
            CreateMap<ObservationDocument, ObservationDocumentAC>().ReverseMap();
            CreateMap<ReportObservation, ReportObservationAC>().ReverseMap();
            CreateMap<ReviewerDocument, ReviewerDocumentAC>().ReverseMap();
            CreateMap<ReportObservationsDocument, ReportObservationsDocumentAC>().ReverseMap();
            CreateMap<ReportObservation, Observation>().ReverseMap();
            CreateMap<ReportObservation, Observation>().ForMember(cv => cv.Id, m => m.MapFrom(s => s.ObservationId)).ReverseMap();
            CreateMap<ReportObservationAC, ObservationAC>()
                .ForMember(cv => cv.Id, m => m.MapFrom(s => s.ObservationId))
                .ForMember(cv => cv.ObservationTableList, m => m.MapFrom(s => s.ReportObservationTableList))
                .ReverseMap();
            CreateMap<ReportComment, ReportCommentAC>().ForMember(cv => cv.Name, m => m.MapFrom(s => s.UserCreatedBy.Name.ToString()))
            .ForMember(cv => cv.ReportTitle, m => m.MapFrom(s => s.Report.ReportTitle.ToString())).ReverseMap();
            CreateMap<ReportObservationReviewer, ReportObservationReviewerAC>()
                .ForMember(cv => cv.Name, m => m.MapFrom(s => s.UserCreatedBy.Name.ToString()))
                .ForMember(cv => cv.ReviewerName, m => m.MapFrom(s => s.User.Name.ToString()))
                .ReverseMap();

            CreateMap<ReportObservationTable, ReportObservationTableAC>().ReverseMap();
            CreateMap<ObservationTable, ObservationTableAC>()
                  .ReverseMap();
            CreateMap<ReportObservationTable, ObservationTable>().ReverseMap();
            CreateMap<ReportObservationTableAC, ObservationTableAC>()

                .ReverseMap();

            #region Export Report
            CreateMap<ReportObservation, ReportObservationAC>()
                .ForMember(cv => cv.ReportTitle, m => m.MapFrom(s => s.Report.ReportTitle.ToString()))
                .ForMember(cv => cv.ProcessName, m => m.MapFrom(s => s.Process.Name.ToString()))
                .ForMember(cv => cv.ObservationCategory, m => m.MapFrom(s => s.ObservationCategory.CategoryName.ToString()))
               .ForMember(cv => cv.Auditor, m => m.MapFrom(s => s.User.Name.ToString()))
               .ForMember(cv => cv.Rating, m => m.MapFrom(s => s.Rating.Ratings.ToString()))
              .ReverseMap();
            #endregion
            #endregion


            CreateMap<RiskControlMatrix, RiskControlMatrixAC>()
                .ForMember(cv => cv.RcmProcessName, m => m.MapFrom(s => s.RcmProcess.Process.ToString()))
                .ForMember(cv => cv.RcmSubProcessName, m => m.MapFrom(s => s.RcmSubProcess.SubProcess.ToString()))
                .ForMember(cv => cv.ControlCategoryString, m => m.MapFrom(s => s.ControlCategory.ToString()))
                .ForMember(cv => cv.ControlTypeString, m => m.MapFrom(s => s.ControlType.ToString()))
                .ForMember(cv => cv.AntiFraudControlString, m => m.MapFrom(s => s.AntiFraudControl == true ? StringConstant.Yes : StringConstant.No))
                .ForMember(cv => cv.NatureOfControlString, m => m.MapFrom(s => s.NatureOfControl.ToString())).ReverseMap();

            CreateMap<RiskControlMatrix, RiskControlMatrixACForExcel>()
                .ForMember(cv => cv.RcmSectorName, m => m.MapFrom(s => s.RcmSector.Sector.ToString()))
              .ForMember(cv => cv.RcmProcessName, m => m.MapFrom(s => s.RcmProcess.Process.ToString()))
              .ForMember(cv => cv.RcmSubProcessName, m => m.MapFrom(s => s.RcmSubProcess.SubProcess.ToString()))
              .ForMember(cv => cv.ControlCategoryString, m => m.MapFrom(s => s.ControlCategory.ToString()))
              .ForMember(cv => cv.ControlTypeString, m => m.MapFrom(s => s.ControlType.ToString()))
              .ForMember(cv => cv.AntiFraudControlString, m => m.MapFrom(s => s.AntiFraudControl == true ? StringConstant.Yes : StringConstant.No))
              .ForMember(cv => cv.NatureOfControlString, m => m.MapFrom(s => s.NatureOfControl.ToString())).ReverseMap();

            CreateMap<ACMPresentation, ACMPresentationAC>()
                .ForMember(cv => cv.Ratings, m => m.MapFrom(s => s.Rating.Ratings.ToString()))
                .ForMember(cv => cv.StatusString, m => m.MapFrom(s => s.Status.ToString()))
                .ReverseMap();
            CreateMap<ACMDocument, ACMDocumentAC>().ReverseMap();

            #region Audit Plan
            CreateMap<AuditPlan, AuditPlanAC>()
             .ForMember(cv => cv.StatusString, m => m.MapFrom(s => s.Status.ToString())).ReverseMap();
            CreateMap<AuditType, AuditTypeAC>().ReverseMap();
            CreateMap<AuditCategory, AuditCategoryAC>().ReverseMap();
            CreateMap<Process, ProcessAC>().ReverseMap();
            CreateMap<PlanProcessMapping, PlanProcessMappingAC>().ReverseMap();
            CreateMap<AuditPlanDocument, AuditPlanDocumentAC>().ReverseMap();
            #endregion


            #region Auditable Entity
            CreateMap<AuditableEntity, AuditableEntityAC>()
                .ForMember(cv => cv.SelectedTypeString, m => m.MapFrom(s => s.EntityType.TypeName.ToString()))
                .ForMember(cv => cv.StatusString, m => m.MapFrom(s => s.Status.ToString()))
                .ReverseMap();
            CreateMap<EntityType, EntityTypeAC>().ReverseMap();
            CreateMap<EntityCategory, EntityCategoryAC>().ReverseMap();
            CreateMap<RelationshipType, RelationshipTypeAC>().ReverseMap();
            CreateMap<Division, DivisionAC>().ReverseMap();
            CreateMap<Location, LocationAC>().ReverseMap();
            CreateMap<AzureAccessToken, AzureAccessTokenAC>().ReverseMap();
            CreateMap<RiskAssessment, RiskAssessmentAC>().ReverseMap();
            CreateMap<RiskAssessmentDocument, RiskAssessmentDocumentAC>().ReverseMap();
            CreateMap<Country, CountryAC>().ReverseMap();
            CreateMap<EntityCountry, EntityCountryAC>().ReverseMap();
            CreateMap<Location, LocationAC>().ReverseMap();
            CreateMap<Region, RegionAC>().ReverseMap();
            CreateMap<ProvinceState, ProvinceStateAC>().ReverseMap();
            CreateMap<EntityState, EntityStateAC>().ReverseMap();
            CreateMap<PrimaryGeographicalArea, PrimaryGeographicalAreaAC>().ReverseMap();
            CreateMap<EntityRelationMapping, EntityRelationMappingAC>().ReverseMap();
            CreateMap<EntityDocument, EntityDocumentAC>().ReverseMap();

            #endregion


        }
    }
}
