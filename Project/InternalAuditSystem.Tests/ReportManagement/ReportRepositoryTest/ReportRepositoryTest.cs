using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;
using InternalAuditSystem.DomainModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


namespace InternalAuditSystem.Test.ReportManagement.ReportRepositoryTest
{
    [Collection("Register Dependency")]

    public class ReportRepositoryTest : BaseTest
    {

        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IReportRepository _reportRepository;
        private IObservationRepository _observationRepository;
        private IDistributionRepository _distributionRepository;
        private IAuditPlanRepository _auditPlanRepository;
        private IAuditableEntityRepository _auditableEntityRepository;
        private IGeneratePPTRepository _generatePPTRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;
        public Mock<IWebHostEnvironment> _webHostEnvironment;
        private IExportToExcelRepository _exportToExcelRepository;
        private readonly Mock<IDynamicTableRepository> _dynamicTableRepository;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string reportId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string reportId2 = "ea25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string reportObservationId = "08639a18-cc6c-484b-8b40-d1346b7ca34a";
        private readonly string rcmId = "c32e8de3-8343-4e47-b5dc-17a838e4da23";
        private readonly string categoryId = "a32e5de3-8343-4e47-b5dc-17a838e4da23";
        private readonly string observationId = "a52e5de3-8343-4e47-b5dc-17a838e4da23";
        private readonly string processId = "c54e5de3-8343-4e47-b5dc-17a838e4da23";
        private readonly string subProcessId = "c54e5fd6-8343-4e47-b5dc-17a838e4da23";
        private readonly string ratingId = "73916e65-4c1a-4142-8740-d1682198f60f";
        private readonly string entityId = "c94d4237-359e-431e-a50a-5b619871ca84";
        private readonly string userId = "e90ea60d-f180-4ef2-90aa-39e92a229575";
        private readonly string enityUserMappingId = "fe7fcd1f-f6bd-4863-8bf6-7f028f8db6c8";
        private readonly string distributorId = "715bb406-5105-4688-9ae9-b70d05231e5b";
        private readonly string planId = "715bb406-5105-4688-9ae9-b70d05231e5b";
        private readonly string auditTypeId = "56be48af-bf4e-48a6-9a18-be81b31b04c8";
        private readonly string observationCategoryId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private readonly string parentProcessId = "ae66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private readonly int timeOffset = -300;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public ReportRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {

            _mapper = bootstrap.ServiceProvider.GetService<IMapper>();
            _dynamicTableRepository = bootstrap.ServiceProvider.GetService<Mock<IDynamicTableRepository>>();
            _reportRepository = bootstrap.ServiceProvider.GetService<IReportRepository>();
            _observationRepository = bootstrap.ServiceProvider.GetService<IObservationRepository>();
            _distributionRepository = bootstrap.ServiceProvider.GetService<IDistributionRepository>();
            _auditPlanRepository = bootstrap.ServiceProvider.GetService<IAuditPlanRepository>();
            _auditableEntityRepository = bootstrap.ServiceProvider.GetService<IAuditableEntityRepository>();
            _generatePPTRepository = bootstrap.ServiceProvider.GetService<IGeneratePPTRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _webHostEnvironment = bootstrap.ServiceProvider.GetService<Mock<IWebHostEnvironment>>();
            _exportToExcelRepository = bootstrap.ServiceProvider.GetService<IExportToExcelRepository>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set Reports testing data
        /// </summary>
        /// <returns>List of Reports data</returns>
        private List<Report> SetReportsInitData()
        {
            List<Report> reportMappingObj = new List<Report>()
            {
                new Report()
                {
                    Id=new Guid(reportId),
                    ReportTitle="Report",
                    EntityId=new Guid(entityId),
                    RatingId =new Guid(ratingId),
                    Stage=ReportStage.Draft,
                    AuditPeriodStartDate= DateTime.UtcNow,
                    AuditPeriodEndDate= DateTime.UtcNow,
                    AuditStatus=ReportStatus.Initial,
                    IsDeleted=false,
                }
            };
            return reportMappingObj;
        }

        /// <summary>
        /// Set RatingAC testing data
        /// </summary>
        /// <returns>List of RatingAC data</returns>
        private List<RatingAC> SetRatingsACInitData()
        {
            List<RatingAC> ratingACMappingObj = new List<RatingAC>()
            {
                new RatingAC()
                {
                    Id= new Guid(),
                    Ratings="test",
                    EntityId=new Guid(),
                    QualitativeFactors="test",
                    QuantitativeFactors="test",
                    Score=1,
                    Legend="red",
                }
            };
            return ratingACMappingObj;
        }

        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private List<User> SetUserInitData()
        {
            List<User> userMappingObj = new List<User>()
            {
                new User()
                {
                    Id=new Guid(reportId),
                    IsDeleted=false,
                }
            };
            return userMappingObj;
        }

        /// <summary>
        /// Set ReportUserMapping testing data
        /// </summary>
        /// <returns>List of ReportUserMapping data</returns>
        private List<ReportUserMapping> SetReportUserMappingInitData()
        {
            List<ReportUserMapping> reportUserMappingObj = new List<ReportUserMapping>()
            {
                new ReportUserMapping()
                {
                    Id=new Guid(),
                    Status= ReviewStatus.Initial,
                    ReportUserType=ReportUserType.Reviewer,
                    ReportId=new Guid(reportId),
                    UserId=new Guid(userId),
                    IsDeleted=false
                }
            };
            return reportUserMappingObj;
        }

        /// <summary>
        /// Set ReportUserMappingAC testing data
        /// </summary>
        /// <returns>List of ReportUserMappingAC data</returns>
        private List<ReportUserMappingAC> SetReportUserMappingACInitData()
        {
            List<ReportUserMappingAC> reportUserACMappingObj = new List<ReportUserMappingAC>()
            {
                new ReportUserMappingAC()
                {
                    Id=new Guid(),
                    Status= ReviewStatus.Initial,
                    ReportUserType=ReportUserType.Reviewer,
                    ReportId=new Guid(reportId),
                    UserId=new Guid(userId),
                    Name="user1",
                    Designation="designation1",
                    IsDeleted=false
                },

                new ReportUserMappingAC()
                {
                    Id=new Guid(),
                    Status= ReviewStatus.Initial,
                    ReportUserType=ReportUserType.Distributor,
                    ReportId=new Guid(reportId2),
                    UserId=new Guid(userId),
                    Name="user2",
                    Designation="designation2",
                    IsDeleted=false
                }
            };
            return reportUserACMappingObj;

        }

        /// <summary>
        /// Set report distributors testing data
        /// </summary>
        /// <returns>List of report distributor data</returns>
        private List<ReportUserMapping> SetReportDistributorsInitData()
        {
            List<ReportUserMapping> reportUserMappingObj = new List<ReportUserMapping>()
            {
               new ReportUserMapping()
                {
                    Id=new Guid(),
                    Status= ReviewStatus.Initial,
                    ReportUserType=ReportUserType.Distributor,
                    ReportId=new Guid(reportId),
                    UserId=new Guid(userId),
                    IsDeleted=false
                }
            };
            return reportUserMappingObj;

        }

        /// <summary>
        /// Set report distributors ac testing data
        /// </summary>
        /// <returns>List of report distributor data</returns>
        private List<ReportUserMappingAC> SetReportDistributorsACInitData()
        {
            List<ReportUserMappingAC> reportUserMappingObj = new List<ReportUserMappingAC>()
            {
               new ReportUserMappingAC()
                {
                    Id=new Guid(),
                    Status= ReviewStatus.Initial,
                    ReportUserType=ReportUserType.Distributor,
                    ReportId=new Guid(reportId),
                    UserId=new Guid(userId),
                    Name="name2",
                    Designation="designation2",
                    IsDeleted=false
                }
            };
            return reportUserMappingObj;

        }

        /// <summary>
        /// Set Ratings testing data
        /// </summary>
        /// <returns>List of Ratings data</returns>
        private List<Rating> SetRatingsInitData()
        {
            List<Rating> ratingMappingObj = new List<Rating>()
            {
                new Rating()
                {
                    Id=new Guid(ratingId),
                    Ratings="test",
                    EntityId=new Guid(),
                    QualitativeFactors="test",
                    QuantitativeFactors="test",
                    Score=1,
                    Legend="red",
                    IsDeleted=false,
                }
            };
            return ratingMappingObj;
        }

        /// <summary>
        /// Set EntityUserMapping testing data
        /// </summary>
        /// <returns>List of EntityUserMapping data</returns>
        private List<EntityUserMapping> SetEntityUserMappingInitData()
        {
            List<User> userMappingObj = SetUserInitData();
            List<EntityUserMapping> entityUserMappingObj = new List<EntityUserMapping>()
            {
                new EntityUserMapping()
                {
                    Id=new Guid(enityUserMappingId),
                    UserId=new Guid(userId),
                    EntityId=new Guid(entityId),
                    User=userMappingObj[0],
                    IsDeleted = false
                }
            };
            return entityUserMappingObj;
        }

        /// <summary>
        /// Set EntityUserMappingAC testing data
        /// </summary>
        /// <returns>List of EntityUserMappingAC data</returns>
        private List<EntityUserMappingAC> SetEntityUserMappingACInitData()
        {
            List<EntityUserMappingAC> entityUserMappingACObj = new List<EntityUserMappingAC>()
            {
                new EntityUserMappingAC()
                {
                    Id=new Guid(enityUserMappingId),
                    UserId=new Guid(userId),
                    EntityId=new Guid(entityId),
                    IsDeleted = false
                }
            };
            return entityUserMappingACObj;
        }
        /// <summary>
        /// Set ReportAC testing data
        /// </summary>
        /// <returns>List of ReportAC data</returns>
        private List<ReportAC> SetReportsACInitData()
        {
            List<ReportAC> reportACMappingObj = new List<ReportAC>()
            {
                new ReportAC()
                {
                    Id=new Guid(reportId),
                    ReportTitle="Report",
                    EntityId=new Guid(entityId),
                    RatingId=new Guid(ratingId),
                    Stage=ReportStage.Draft,
                    AuditPeriodStartDate= DateTime.UtcNow,
                    AuditPeriodEndDate= DateTime.UtcNow,
                    AuditStatus=ReportStatus.Initial,
                    Comment="Comment",
                    StageName ="",
                    Status ="",
                    RatingsList=SetRatingsACInitData(),
                    UserList= SetEntityUserMappingACInitData(),
                    ReviewerList= SetReportUserMappingACInitData(),
                    IsDeleted = false,
               }
             };
            return reportACMappingObj;
        }
        /// <summary>
        /// Set Report Observation testing data
        /// </summary>
        /// <returns>List of Observation data</returns>
        private List<Observation> SetObservationsInitData()
        {
            List<Observation> observationMappingObj = new List<Observation>()
            {
                new Observation()
                {
                            Id= new Guid(),
                            Heading="Heading 1",
                            Background="Background 1",
                            RiskAndControlId=new Guid(rcmId),
                            Observations="Observations 1",
                            ObservationType=ObservationType.Legal,
                            ObservationCategoryId=new Guid(categoryId),
                            IsRepeatObservation=true,
                            RootCause="RootCause 1",
                            Implication="Implication 1",
                            Disposition=Disposition.Reportable,
                            ObservationStatus=ObservationStatus.Open,
                            Recommendation="Recommendation 1",
                            ManagementResponse="ManagementResponse 1",
                            PersonResponsible=new Guid(userId),
                            RelatedObservation="RelatedObservation 1",
                            ProcessId= new Guid(processId),
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = new Guid(userId)
                }
            };
            return observationMappingObj;
        }

        /// <summary>
        /// Set Reports observation testing data
        /// </summary>
        /// <returns>List of Report observations data</returns>
        private List<ReportObservation> SetReportObservationsInitData()
        {
            List<ReportObservation> reportObservationMappingObj = new List<ReportObservation>()
            {
                new ReportObservation()
                {
                    Id=new Guid(reportObservationId),
                    Heading="Heading 1",
                    Background="Background 1",
                    //RcmId=new Guid(rcmId),
                    Observations="Observations 1",
                    ObservationType=ReportObservationType.Legal,
                    ObservationCategoryId=new Guid(categoryId),
                    IsRepeatObservation=true,
                    RootCause="RootCause 1",
                    Implication="Implication 1",
                    Disposition=ReportDisposition .Reportable,
                    ObservationStatus=ReportObservationStatus .Open,
                    Recommendation="Recommendation 1",
                    ManagementResponse="ManagementResponse 1",
                    LinkedObservation ="",
                    ProcessId= new Guid(subProcessId),
                    CreatedDateTime = DateTime.UtcNow,
                    IsDeleted=false,
                    ReportId=Guid.Parse(reportId)
                }
            };
            return reportObservationMappingObj;
        }


        /// <summary>
        /// Set Reports observation AC testing data
        /// </summary>
        /// <returns>List of Report observations data</returns>
        private List<ReportObservationAC> SetReportObservationsACInitData()
        {
            List<ReportObservationAC> reportObservationMappingObj = new List<ReportObservationAC>()
            {
                new ReportObservationAC()
                {
                    Id=new Guid(reportObservationId),
                    Heading="Heading 1",
                    Background="Background 1",
                    //RcmId=new Guid(rcmId),
                    Observations="Observations 1",
                    ObservationType=ReportObservationType.Legal,
                    ObservationCategoryId=new Guid(categoryId),
                    IsRepeatObservation=true,
                    RootCause="RootCause 1",
                    Implication="Implication 1",
                    Disposition=ReportDisposition .Reportable,
                    ObservationStatus=ReportObservationStatus .Open,
                    Recommendation="Recommendation 1",
                    ManagementResponse="ManagementResponse 1",
                    LinkedObservation ="RelatedObservation 1",
                    ProcessId= new Guid(subProcessId),
                    CreatedDateTime = DateTime.UtcNow,
                    IsDeleted=false,
                    ReportId=Guid.Parse(reportId)
                }
            };
            return reportObservationMappingObj;
        }

        /// <summary>
        /// Set Distributors testing data
        /// </summary>
        /// <returns>List of Distributors data</returns>
        private List<Distributors> SetDistributorsInitData()
        {
            List<User> userMappingObj = SetUserInitData();
            List<Distributors> distributorsMappingObj = new List<Distributors>()
            {
                new Distributors()
                {

                    Id=new Guid(distributorId),
                    UserId=new Guid(),
                    EntityId=new Guid(),
                    IsDeleted=false,
                    User=userMappingObj[0]
                }
            };
            return distributorsMappingObj;
        }

        /// <summary>
        /// Set DistributorsAC testing data
        /// </summary>
        /// <returns>List of DistributorsAC data</returns>
        private List<DistributorsAC> SetDistributorsACInitData()
        {
            List<DistributorsAC> distributorsACMappingObj = new List<DistributorsAC>()
            {
                new DistributorsAC()
                {
                    Id=new Guid(distributorId),
                    UserId=new Guid(),
                    EntityId=new Guid(),
                    Name="test",
                    Designation="admin",
                    IsDeleted = false
                }
            };
            return distributorsACMappingObj;
        }

        /// <summary>
        /// Set Reports observation member data
        /// </summary>
        /// <returns>List of Reports observation member</returns>
        private List<ReportObservationsMember> SetReportObservationsMemberInitData()
        {
            List<ReportObservationsMember> reportObservationsMemberMappingObj = new List<ReportObservationsMember>()
            {
                new ReportObservationsMember()
                {
                    UserId=new Guid(userId),
                    ReportObservationId=new Guid(userId),
                    //Comment="Comment1",
                    //ReportObservationUserType=ObservationUserType.Reviewer,
                    IsDeleted=false,
                }
            };
            return reportObservationsMemberMappingObj;
        }

        /// <summary>
        /// Set Reports observation member ac data
        /// </summary>
        /// <returns>List of Reports observation member ac</returns>
        private List<ReportObservationsMemberAC> SetReportObservationsMemberACInitData()
        {
            List<ReportObservationsMemberAC> reportObservationsMemberMappingObj = new List<ReportObservationsMemberAC>()
            {
                new ReportObservationsMemberAC()
                {
                    UserId=new Guid(userId),
                    ReportObservationId=new Guid(userId),
                    IsDeleted=false,
                }
            };
            return reportObservationsMemberMappingObj;
        }

        /// <summary>
        /// Set observation Table data
        /// </summary>
        /// <returns>List of observation table </returns>
        private List<ObservationTable> SetObservationTableInitData()
        {
            List<ObservationTable> observationTableMappingObj = new List<ObservationTable>()
            {
                new ObservationTable()
                {
                    ObservationId=new Guid(userId),
                    IsDeleted=false,
                }
            };
            return observationTableMappingObj;
        }

        /// <summary>
        /// Set Reports observation documents data
        /// </summary>
        /// <returns>List of Reports observation documents </returns>
        private List<ReportObservationsDocument> SetReportObservationsDocumentInitData()

        {
            List<ReportObservationsDocument> reportObservationsDocumentMappingObj = new List<ReportObservationsDocument>()
            {
                new ReportObservationsDocument()
                {
                    ReportObservationId=new Guid(userId),
                    IsDeleted=false,
                }
            };
            return reportObservationsDocumentMappingObj;

        }
        /// <summary>
        /// Set Reports observation reviewer data
        /// </summary>
        /// <returns>List of Reports observation reviewer</returns>
        private List<ReportObservationReviewer> SetReportObservationReviewerInitData()
        {
            List<ReportObservationReviewer> reportObservationReviewerMappingObj = new List<ReportObservationReviewer>()
            {
                new ReportObservationReviewer()
                {
                    Id=new Guid(),
                    UserId = new Guid(userId),
                    ReportObservationId = new Guid(userId),
                    Comment = "Comment1",
                    //ReportObservationUserType=ObservationUserType.Reviewer,
                    IsDeleted = false,
                }
            };
            return reportObservationReviewerMappingObj;
        }

        /// <summary>
        /// Set Reports observation reviewer ac data
        /// </summary>
        /// <returns>List of Reports observation reviewer ac</returns>
        private List<ReportObservationReviewerAC> SetReportObservationReviewerACInitData()
        {
            List<ReportObservationReviewerAC> reportObservationReviewerMappingObj = new List<ReportObservationReviewerAC>()
            {
                new ReportObservationReviewerAC()
                {
                    Id=new Guid(),
                    UserId = new Guid(userId),
                    ReportObservationId = new Guid(userId),
                    Comment = "Comment1",
                    IsDeleted = false,
                }
            };
            return reportObservationReviewerMappingObj;
        }

        /// <summary>
        /// Set ReportComment testing data
        /// </summary>
        /// <returns>List of ReportComment data</returns>
        private List<ReportComment> SetReportCommentInitData()
        {
            List<ReportComment> reportCommentMappingObj = new List<ReportComment>()
            {
                new ReportComment()
                {
                    Id=new Guid(),
                    ReportId=new Guid(reportId),
                    Comment="Comment",
                    IsDeleted = false
                }
            };
            return reportCommentMappingObj;
        }

        /// <summary>
        /// Set ReviewerDocumenttesting data
        /// </summary>
        /// <returns>List of ReviewerDocument data</returns>
        private List<ReviewerDocument> SetReviewerDocumentInitData()
        {
            List<ReviewerDocument> reviewerDocumentMappingObj = new List<ReviewerDocument>()
            {
                new ReviewerDocument()
                {
                    DocumentName ="Document1",
                    DocumentPath ="DocumentPath1",
                    ReportUserMappingId =new Guid()
                }
            };
            return reviewerDocumentMappingObj;
        }


        /// <summary>
        /// Set audit plan testing data
        /// </summary>
        /// <returns>List of audit plan</returns>
        private List<AuditPlan> SetAuditPlanInitialData()
        {
            List<AuditPlan> auditPlanObjList = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(planId),
                    Title="audit-Plan-1",
                    Status= AuditPlanStatus.Active,
                    CreatedDateTime= DateTime.UtcNow,
                    TotalBudgetedHours=0,
                    Version =1.0,
                    SelectedTypeId =new Guid(auditTypeId),
                    EntityId=new Guid(entityId),
                    IsDeleted=false,
                },
            };
            return auditPlanObjList;
        }

        /// <summary>
        /// Set planProcessMapping testing data
        /// </summary>
        /// <returns>List of planProcessMapping</returns>
        private List<PlanProcessMapping> SetPlanProcessMappingInitData()
        {
            List<PlanProcessMapping> planProcessMappingObjList = new List<PlanProcessMapping>()
            {
                new PlanProcessMapping()
                {
                    PlanId= new Guid(planId),
                    ProcessId=Guid.Parse( parentProcessId),
                    StartDateTime=System.DateTime.Now,
                    EndDateTime=System.DateTime.Now,
                    Status=PlanProcessStatus.Closed,
                    WorkProgramId= new Guid(planId),
                    Process =SetProcessInitData().First()
                }
            };
            return planProcessMappingObjList;
        }

        /// <summary>
        /// Set planProcessMapping ac testing data
        /// </summary>
        /// <returns>List of planProcessMapping</returns>
        private List<PlanProcessMappingAC> SetPlanProcessMappingACInitData()
        {
            List<PlanProcessMappingAC> planProcessMappingObjList = new List<PlanProcessMappingAC>()
            {
                new PlanProcessMappingAC()
                {
                    PlanId= new Guid(planId),
                    ProcessId=Guid.Parse( parentProcessId),
                    ParentProcessId=Guid.Parse(parentProcessId),
                    StartDateTime=System.DateTime.Now,
                    EndDateTime=System.DateTime.Now,
                    Status=PlanProcessStatus.Closed,
                    WorkProgramId= new Guid(planId),
                }
            };
            return planProcessMappingObjList;
        }

        /// <summary>
        /// Set process testing data
        /// </summary>
        /// <returns>List of process</returns>
        private List<Process> SetProcessInitData()
        {
            Process parentProcess = new Process()
            {
                Id = new Guid(parentProcessId),
                Name = "Parent Process",
                EntityId = Guid.Parse(entityId),
                ParentId = new Guid(parentProcessId)
            };

            List<Process> processMappingObj = new List<Process>()
                {
                    new Process()
                    {
                        Id = new Guid(),
                        Name = "Process",
                        EntityId = Guid.Parse(entityId),
                        ParentId=new Guid(parentProcessId),
                        ParentProcess=parentProcess
                    }
                };
            return processMappingObj;
        }

        /// <summary>
        /// Set AuditableEntity testing data
        /// </summary>
        /// <returns>List of AuditableEntity</returns>
        private List<AuditableEntity> SetAuditableEntityInitData()
        {
            List<AuditableEntity> auditableEntityList = new List<AuditableEntity>()
            {
                new AuditableEntity()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="AuditableEntity 1",
                    IsDeleted=false,
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    EntityType=new EntityType()
                    {
                        Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                        TypeName="Entity Type 1"
                    },
                    Version=1,
                    Status=DomailModel.Enums.AuditableEntityStatus.Active,
                    IsStrategyAnalysisDone=true

                }
            };
            return auditableEntityList;
        }

        /// <summary>
        /// Set ObservationCategory testing data
        /// </summary>
        /// <returns>List of ObservationCategory</returns>
        private List<ObservationCategory> SetObservationCategoryInitData()
        {
            List<ObservationCategory> observationCategoryObjList = new List<ObservationCategory>()
            {
                new ObservationCategory()
                {
                    Id= Guid.Parse(observationCategoryId),
                    CategoryName="ObservationCategory-1",
                    EntityId=new Guid(entityId),
                    IsDeleted=false,
                }
            };
            return observationCategoryObjList;
        }

        private List<User> SetUserListInitData()
        {
            List<User> userMappingObj = new List<User>
            {
                 new User()
                {

                Id = Guid.Parse( userId),
                IsDeleted = false,
                Name = "abc",
                EmailId = "abc@gmail.com",
                Designation = "General manager",
                UserType = DomailModel.Enums.UserType.Internal
                }
            };
            return userMappingObj;
        }

        /// <summary>
        /// Method to initialize observation table data
        /// </summary>
        /// <returns>List of observation table</returns>
        private List<ReportObservationTable> SetReportObservationTableInitData()
        {
            var jsonDoc = "{\"tableId\":\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"columnNames\":[\"rowId\",\"ColumnName1\",\"ColumnName2\"],\"data\":[{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]},{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]}]}";
            var parsedDocument = JsonDocument.Parse(jsonDoc);
            var observationTableObj = new List<ReportObservationTable>()
            {
                new ReportObservationTable()
                {
                   Id=new Guid(reportObservationId),
                   IsDeleted=false,
                   CreatedBy=Guid.Parse(userId),
                   CreatedDateTime=DateTime.UtcNow,
                   ReportObservationId = Guid.Parse(reportObservationId),
                   Table = parsedDocument
                }
            };
            return observationTableObj;
        }

        /// <summary>
        /// set All Plans And Processes Of All Plans By Entity Id
        /// </summary>
        private void GetAllPlansAndProcessesOfAllPlansByEntityIdAsync()
        {
            List<AuditPlan> allPlansList = SetAuditPlanInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                    .Returns(allPlansList.Where(x => x.EntityId == Guid.Parse(entityId) && !x.IsDeleted).AsQueryable().BuildMock().Object);
            List<Guid> allPlansIdList = allPlansList.Select(x => x.Id).ToList();

            // all processes (i.e: subprocesses added) in audit plan
            List<PlanProcessMapping> allPlanProcesses = SetPlanProcessMappingInitData();
            List<PlanProcessMappingAC> allPlanProcessAC = SetPlanProcessMappingACInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(allPlanProcesses.Where(x => allPlansIdList.Contains(x.PlanId) && !x.IsDeleted)
                    .AsQueryable().BuildMock().Object);

            List<Guid?> allParentProcessIdList = allPlanProcessAC.Select(x => x.ParentProcessId).Distinct().ToList();

            // all parent process details
            var allParentProcessDetails = SetProcessInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Process, bool>>>()))
                    .Returns(allParentProcessDetails.Where(x => !x.IsDeleted && allParentProcessIdList.Contains(x.Id)).AsQueryable().BuildMock().Object);
        }
        #endregion

        #region Testing Methods Completed

        #region Report Methods
        /// <summary>
        /// Test cases to varify report list for acm
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsForACMIdAsync_ValidData()
        {
            List<Report> reportList = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                     .Returns(reportList.Where(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetReportsForACMIdAsync(Guid.Parse(entityId));
            Assert.Equal(reportList.Count, result.Count);
        }

        /// <summary>
        /// Test case to verify GetReportsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsAsync_ReturnReportAC()
        {
            List<Report> reportMappingObj = SetReportsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                     .Returns(reportMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            List<string> reportIds = reportMappingObj.Select(a => a.Id.ToString()).ToList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                     .Returns(reportObservationMappingObj.Where(x => !x.IsDeleted && reportIds.Contains(reportId)).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetReportsAsync(page, pageSize, searchString, entityId, 0, 0);
            Assert.Equal(reportMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetReportsAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsAsync_SearchString()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                     .Returns(reportMappingObj.Where(x => !x.IsDeleted && x.ReportTitle == "Report").AsQueryable().BuildMock().Object);

            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            List<string> reportIds = reportMappingObj.Select(a => a.Id.ToString()).ToList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                     .Returns(reportObservationMappingObj.Where(x => !x.IsDeleted && reportIds.Contains(reportId)).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetReportsAsync(page, pageSize, "Report", entityId, 0, 0);
            Assert.Equal(reportMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetReportsAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsAsync_SearchReportAC()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                     .Returns(reportMappingObj.Where(x => !x.IsDeleted && x.ReportTitle == "Report").AsQueryable().BuildMock().Object);
            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            List<string> reportIds = reportMappingObj.Select(a => a.Id.ToString()).ToList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                     .Returns(reportObservationMappingObj.Where(x => !x.IsDeleted && reportIds.Contains(reportId)).AsQueryable().BuildMock().Object);

            List<ReportAC> result = await _reportRepository.GetReportsAsync(page, pageSize, "Report", entityId, 0, 0);
            Assert.Equal(reportMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetReportsAsync for invalid search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsAsync_InvalidSearchReport()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                     .Returns(reportMappingObj.Where(x => !x.IsDeleted && x.ReportTitle == "report").AsQueryable().BuildMock().Object);

            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            List<string> reportIds = reportMappingObj.Select(a => a.Id.ToString()).ToList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                     .Returns(reportObservationMappingObj.Where(x => !x.IsDeleted && reportIds.Contains(reportId)).AsQueryable().BuildMock().Object);

            List<ReportAC> result = await _reportRepository.GetReportsAsync(page, pageSize, "report", entityId, 0, 0);
            Assert.NotEqual(reportMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetReportCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportCountAsync_ReturnCount()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                .Returns(reportMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _reportRepository.GetReportCountAsync(searchString, entityId);
            Assert.Equal(reportMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetReportCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportCountAsync_SearchReturnCount()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                .Returns(reportMappingObj.Where(x => !x.IsDeleted && x.ReportTitle == "Report").AsQueryable().BuildMock().Object);

            int result = await _reportRepository.GetReportCountAsync("Report", entityId);
            Assert.Equal(reportMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetReportCountAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportCountAsync_NoReturnCount()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            reportMappingObj[0].IsDeleted = true;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
                .Returns(reportMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _reportRepository.GetReportCountAsync(searchString, entityId);
            Assert.NotEqual(reportMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify GetReportsByIdAsync with report id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportsByIdAsync_ValidData()
        {
            List<ReportAC> reportACMappingObj = SetReportsACInitData();

            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Report, bool>>>()))
                 .Returns(() => Task.FromResult(reportMappingObj[0]));


            List<ReportUserMapping> reportUserMappingObj = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                     .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
                           a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Reviewer)
                     .AsQueryable().BuildMock().Object);

            List<Rating> ratingsMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                 .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
            a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Reviewer)
                 .AsQueryable().BuildMock().Object);

            List<EntityUserMapping> entityUserMappingObj = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(entityUserMappingObj.Where(a => !a.IsDeleted &&
                      a.EntityId == new Guid(entityId))
                 .AsQueryable().BuildMock().Object);

            List<ReportComment> reportCommentMappingObj = SetReportCommentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportComment, bool>>>()))
               .Returns(reportCommentMappingObj.Where(a => !a.IsDeleted &&
                    a.ReportId == new Guid(reportId) && a.CreatedBy == new Guid(userId))
               .AsQueryable().BuildMock().Object);

            HashSet<Guid> reportUserMappingIds = new HashSet<Guid>(reportUserMappingObj.Select(s => s.Id));


            List<ReviewerDocument> reviewerDocumentMappingObj = SetReviewerDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReviewerDocument, bool>>>()))
           .Returns(reviewerDocumentMappingObj.Where(a => !a.IsDeleted &&
                    reportUserMappingIds.Contains(a.ReportUserMappingId))
                    .AsQueryable().BuildMock().Object);

            List<Rating> ratingMappingObj = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
          .Returns(ratingMappingObj.Where(a => !a.IsDeleted &&
            a.EntityId.ToString() == entityId)
                   .AsQueryable().BuildMock().Object);


            // to do remove after proper unit testing
            ReportAC result = await _reportRepository.GetReportsByIdAsync(reportId, entityId, 0);
            Assert.Equal(reportACMappingObj[0].ReportTitle, result.ReportTitle);

        }

        /// <summary>
        /// Test case to verify Delete Report for valid report id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteReport_ValidReportId()
        {
            List<Report> reportMappingObj = SetReportsInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Report, bool>>>()))
                   .Returns(() => Task.FromResult(reportMappingObj[0]));

            List<ReportUserMapping> reportUserMappingObj = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                   .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
                         a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.UpdateRange(reportUserMappingObj));


            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                   .Returns(reportObservationMappingObj.Where(a => !a.IsDeleted &&
                         a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.UpdateRange(reportObservationMappingObj));

            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationMappingObj.Select(s => s.Id));
            List<ReportObservationsMember> reportObservationsMemberMappingObj = SetReportObservationsMemberInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                  .Returns(reportObservationsMemberMappingObj.Where(a => !a.IsDeleted &&
                      reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportObservationsMemberMappingObj));

            List<ReportObservationReviewer> reportObservationReviewerMappingObj = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
                  .Returns(reportObservationReviewerMappingObj.Where(a => !a.IsDeleted &&
                      reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportObservationReviewerMappingObj));


            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));


            await _reportRepository.DeleteReportAsync(reportId);
            _dataRepository.Verify(x => x.SaveChangesAsync());

        }

        /// <summary>
        /// Test case to verify Delete Report for invalid report id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteReport_InvalidReportId()
        {
            List<Report> reportMappingObj = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Report, bool>>>()))
                   .Returns(() => Task.FromResult(reportMappingObj[0]));

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                    await _reportRepository.DeleteReportAsync(reportId));

        }

        /// <summary>
        /// Test case to verify Add Report for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddReport_ValidData()
        {
            List<ReportAC> reportACMappingObj = SetReportsACInitData();

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => Task.FromResult(userMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(reportACMappingObj[0]))
               .Returns((ReportAC model) => Task.FromResult((ReportAC)null));

            List<ReportUserMapping> reportUserMappingObj = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.AddRangeAsync(reportUserMappingObj))
            .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            ReportAC result = await _reportRepository.AddReportAsync(reportACMappingObj[0]);
            Assert.Equal(reportACMappingObj[0].ReportTitle, result.ReportTitle);
        }


        /// <summary>
        /// Test case to verify Update Report for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateReport_ValidData()
        {
            List<ReportAC> reportACMappingObj = SetReportsACInitData();

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => Task.FromResult(userMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<ReportAC>()))
                   .Returns((ReportAC model) => (EntityEntry<ReportAC>)null);
            //update reviwer
            List<ReportUserMapping> reportUserMappingObj = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                   .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
                         a.ReportId == reportACMappingObj[0].Id).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportUserMappingObj));
            _dataRepository.Setup(x => x.RemoveRange(reportUserMappingObj));
            _dataRepository.Setup(x => x.AddRangeAsync(reportUserMappingObj))
          .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            ReportAC result = await _reportRepository.UpdateReportAsync(reportACMappingObj[0]);
            Assert.Equal(reportACMappingObj[0].ReportTitle, result.ReportTitle);
        }


        /// <summary>
        /// Test case to verify Add Distributor Report for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDistributors_ValidData()
        {
            List<ReportUserMappingAC> reportUserACMappingData = SetReportUserMappingACInitData();

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => Task.FromResult(userMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<ReportUserMapping> reportUserMappingObj = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                   .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
                         a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportUserMappingObj));

            _dataRepository.Setup(x => x.AddRangeAsync(reportUserMappingObj))
          .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            await _reportRepository.AddDistributorsAsync(reportUserACMappingData);
            _dataRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }


        /// <summary>
        /// Test case to verify Update Distributor Report for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDistributors_UpdateValidData()
        {
            List<ReportUserMappingAC> reportUserACMappingData = SetReportDistributorsACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<ReportUserMapping> reportUserMappingObj = SetReportDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                   .Returns(reportUserMappingObj.Where(a => !a.IsDeleted &&
                         a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportUserMappingObj));

            _dataRepository.Setup(x => x.AddRangeAsync(reportUserMappingObj))
          .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            await _reportRepository.AddDistributorsAsync(reportUserACMappingData);
            _dataRepository.Verify(x => x.AddRangeAsync<ReportUserMapping>(It.IsAny<IEnumerable<ReportUserMapping>>()), Times.Once);
        }

        /// <summary>
        /// Test case to verify Get Distributor by Report Id for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsByReportId_ValidData()
        {
            List<Distributors> distributorsMappingObj = SetDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                .Returns(distributorsMappingObj.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);

            List<ReportUserMapping> reportDistributorsMappingObj = SetReportUserMappingInitData();
            reportDistributorsMappingObj[0].ReportUserType = ReportUserType.Distributor;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                .Returns(reportDistributorsMappingObj.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId
            && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetDistributorsByReportIdAsync(entityId, reportId);
            Assert.Equal(distributorsMappingObj.Count, result.ReportDistributorsList.Count);
        }

        /// <summary>
        /// Test case to verify Get Observationswith process id 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservations_ForSubProcessId()
        {
            List<Observation> observationMappingObj = SetObservationsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .Returns(observationMappingObj.Where(a => !a.IsDeleted
                    && a.AuditPlanId.ToString() == planId && a.ProcessId.ToString() == subProcessId)
            .AsQueryable().BuildMock().Object);

            //get report observation from database
            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                  .Returns(reportObservationMappingObj.Where(a => !a.IsDeleted &&
           a.ReportId.ToString() == reportId && a.ProcessId.ToString() == processId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddRangeAsync(reportObservationMappingObj))
          .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            var result = await _reportRepository.GetAllObservationsAsync(processId, subProcessId, entityId, reportId);
            Assert.Equal(0, result.ReportObservationList.Count);
        }

        /// <summary>
        /// Test case to verify Get Observations with out subprocessid for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservations_ForNullSubProcessId()
        {
            List<Observation> observationMappingObj = SetObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                    .Returns(observationMappingObj.Where(a => !a.IsDeleted && a.ProcessId.ToString() == processId
                    ).AsQueryable().BuildMock().Object);
            //get report observation from database
            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                  .Returns(reportObservationMappingObj.Where(a => !a.IsDeleted &&
           a.ReportId.ToString() == reportId && a.ProcessId.ToString() == processId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddRangeAsync(reportObservationMappingObj))
          .Returns((ReportUserMapping model) => Task.FromResult((EntityEntry<ReportUserMapping>)null));

            var result = await _reportRepository.GetAllObservationsAsync(processId, subProcessId, entityId, reportId);
            Assert.Equal(reportObservationMappingObj.Count, result.ReportObservationList.Count);
        }

        /// <summary>
        /// Test cases to verify DeleteReviewerDocumentAync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteReviewerDocumentAync_ValidData()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var reviewerDocuments = SetReviewerDocumentInitData();
            ReviewerDocument reviewerDocument = reviewerDocuments[0];
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ReviewerDocument, bool>>>()))
                    .Returns(Task.FromResult(reviewerDocument));

            //await _dataRepository.FirstAsync<ReviewerDocument>(x => x.Id == reviewerDocumentId);
            await _reportRepository.DeleteReviewerDocumentAync(reviewerDocument.Id);
        }

        /// <summary>
        /// Test cases to verify DownloadReviewerDocumentAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DownloadReviewerDocumentAsync_ValidData()
        {
            var reviewerDocuments = SetReviewerDocumentInitData();
            ReviewerDocument reviewerDocument = reviewerDocuments[0];
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ReviewerDocument, bool>>>()))
                .Returns(Task.FromResult(reviewerDocument));

            var result = await _reportRepository.DownloadReviewerDocumentAsync(reviewerDocument.Id);

        }


        /// <summary>
        /// Test case verification fot Export Reports 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task ExportReportsAsync_ValidData()
        {
            //Export report table data
            List<Report> reportList = SetReportsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
         .Returns(reportList.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);


            //get added report ids
            HashSet<Guid> getReportIds = new HashSet<Guid>(reportList.Select(s => s.Id));
            //Export report comment
            List<ReportComment> reportCommentList = SetReportCommentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportComment, bool>>>()))
        .Returns(reportCommentList.Where(a => !a.IsDeleted && getReportIds.Contains(a.ReportId)).AsQueryable().BuildMock().Object);

            //Get report reviewer and distributors
            List<ReportUserMapping> reportUserMappingList = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
       .Returns(reportUserMappingList.Where(a => !a.IsDeleted && getReportIds.Contains(a.ReportId)).AsQueryable().BuildMock().Object);


            //Export report observation
            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                  .Returns(reportObservationList.Where(a => !a.IsDeleted && getReportIds.Contains(a.ReportId)).AsQueryable().BuildMock().Object);

            GetAllPlansAndProcessesOfAllPlansByEntityIdAsync();

            HashSet<Guid> getReportObservationsIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));

            List<ReportObservationReviewer> reportObservationReviewerList = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
                  .Returns(reportObservationReviewerList.Where(a => !a.IsDeleted && getReportObservationsIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            List<ReportObservationsMember> reportObservationsMemberList = SetReportObservationsMemberInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                  .Returns(reportObservationsMemberList.Where(a => !a.IsDeleted && getReportObservationsIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);


            List<ReportObservationTable> observationsTable = SetReportObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                  .Returns(observationsTable.Where(a => !a.IsDeleted && getReportObservationsIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            var entity = SetAuditableEntityInitData().First();
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                                .Returns(Task.FromResult(entity));

            var result = await _reportRepository.ExportReportsAsync(entityId, timeOffset);
            Assert.NotNull(result);
        }

        #endregion

        #region Report Distributors
        /// <summary>
        /// Test cases to verify AddDistributorsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDistributorsAsync_ValidData()
        {
            List<ReportUserMappingAC> distributorsMappingObj = SetReportUserMappingACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);


            //get distributor from DB
            List<ReportUserMapping> getReportDistributors = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
               .Returns(getReportDistributors.Where(a => a.ReportId == Guid.Parse(reportId) && !a.IsDeleted
            && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            //get deleted distributors from DB
            List<ReportUserMapping> getDeletedReportDistributors = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
               .Returns(getDeletedReportDistributors.Where(a => a.IsDeleted && a.ReportId == Guid.Parse(reportId)
            && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);


            _dataRepository.Setup(x => x.UpdateRange(getReportDistributors));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<ReportUserMapping>>()))
                .Returns(Task.FromResult((EntityEntry<List<ReportUserMapping>>)null));

            await _reportRepository.AddDistributorsAsync(distributorsMappingObj);
            _dataRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Test case verify for GetDistributorsByReportIdAsync
        /// </summary>
        /// <returns>Task/returns>
        public async Task GetDistributorsByReportIdAsync_ValidData()
        {
            ReportDistributorsAC reportDistributorsAC = new ReportDistributorsAC();
            reportDistributorsAC.DistributorsList = SetDistributorsACInitData();
            List<Distributors> distributors = SetDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
               .Returns(distributors.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);

            List<ReportUserMapping> distributorsList = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
               .Returns(distributorsList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId
            && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetDistributorsByReportIdAsync(entityId, reportId);
            Assert.Equal(reportDistributorsAC.DistributorsList.Count, result.DistributorsList.Count);
        }

        #endregion

        #region Report Observation List

        /// <summary>
        /// Test cases to verify Get Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetPlanProcesessInitDataAsync_ValidData()
        {
            List<AuditPlan> allPlansList = SetAuditPlanInitialData();
            GetAllPlansAndProcessesOfAllPlansByEntityIdAsync();
            var result = await _reportRepository.GetPlanProcesessInitDataAsync(entityId, reportId);
            Assert.Equal(allPlansList.Count, result.Count);
        }

        /// <summary>
        /// test case to verify Get all observation 
        /// </summary>
        /// <returns>Task/returns>
        [Fact]
        public async Task GetAllObservationsAsync_ValidaData()
        {
            ReportDetailAC reportDetailAC = new ReportDetailAC();
            //get plan wise observation list
            List<Observation> observationList = SetObservationsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
               .Returns(observationList.Where(a => !a.IsDeleted && a.AuditPlanId.ToString() == planId && a.ProcessId.ToString() == subProcessId).AsQueryable().BuildMock().Object);

            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
               .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetAllObservationsAsync(null, null, entityId, reportId);
            Assert.Equal(reportObservationList.Count, result.ReportObservationList.Count);
        }


        /// <summary>
        /// test case to verify Get all observation 
        /// </summary>
        /// <returns>Task/returns>
        [Fact]
        public async Task GetAllObservationsAsync_WithSubProcessId()
        {
            ReportDetailAC reportDetailAC = new ReportDetailAC();
            //get plan wise observation list
            List<Observation> observationList = SetObservationsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
               .Returns(observationList.Where(a => !a.IsDeleted && a.AuditPlanId.ToString() == planId && a.ProcessId.ToString() == subProcessId).AsQueryable().BuildMock().Object);

            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
               .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId
               && a.ProcessId.ToString() == subProcessId).AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetAllObservationsAsync(planId, subProcessId, entityId, reportId);

            Assert.Equal(reportObservationList.Count, result.ReportObservationList.Count);
        }

        /// <summary>
        /// Test cases to varify Add and update observation
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationsAsync_ValidData()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                   .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);

            List<ReportObservationAC> reportObservationACList = SetReportObservationsACInitData();
            ReportDetailAC reportObservations = new ReportDetailAC();
            reportObservations.ReportObservationList = reportObservationACList;


            _dataRepository.Setup(x => x.RemoveRange(reportObservationList));

            // Remove report observation reviewer and first person responsible
            List<ReportObservationsMember> reportObservationMembers = SetReportObservationsMemberInitData();
            HashSet<Guid> removedObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.ObservationId));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                  .Returns(reportObservationMembers.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(reportObservationMembers));

            // Remove report observation table --hard delete
            List<ReportObservationTable> removedObservationTables = SetReportObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                  .Returns(removedObservationTables.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(removedObservationTables));

            // Remove report observation document --hard delete
            List<ReportObservationsDocument> removedReportObservationsDocuments = SetReportObservationsDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsDocument, bool>>>()))
                  .Returns(removedReportObservationsDocuments.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(removedReportObservationsDocuments));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<ReportObservation>>())).Returns(Task.FromResult((List<ReportObservation>)null));

            HashSet<Guid> newObservationIds = new HashSet<Guid>(reportObservations.ReportObservationList.Select(s => s.ObservationId));

            List<ObservationTable> observationTableList = SetObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                  .Returns(observationTableList.Where(a => !a.IsDeleted && newObservationIds.Contains(a.ObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.UpdateRange(reportObservationList));

            await _reportRepository.AddObservationsAsync(reportObservations);
            _dataRepository.Verify(x => x.UpdateRange<ReportObservation>(It.IsAny<IEnumerable<ReportObservation>>()), Times.Once);
        }

        /// <summary>
        /// Test cases to varify Add observation
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationsAsync_AddData()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<ReportObservation> reportObservationList = new List<ReportObservation>();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                   .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);

            List<ReportObservationAC> reportObservationACList = SetReportObservationsACInitData();
            ReportDetailAC reportObservations = new ReportDetailAC();
            reportObservations.ReportObservationList = reportObservationACList;


            _dataRepository.Setup(x => x.RemoveRange(reportObservationList));

            // Remove report observation reviewer and first person responsible
            List<ReportObservationsMember> reportObservationMembers = SetReportObservationsMemberInitData();
            HashSet<Guid> removedObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.ObservationId));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                  .Returns(reportObservationMembers.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(reportObservationMembers));

            // Remove report observation table --hard delete
            List<ReportObservationTable> removedObservationTables = SetReportObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                  .Returns(removedObservationTables.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(removedObservationTables));

            // Remove report observation document --hard delete
            List<ReportObservationsDocument> removedReportObservationsDocuments = SetReportObservationsDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsDocument, bool>>>()))
                  .Returns(removedReportObservationsDocuments.Where(a => removedObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.RemoveRange(removedReportObservationsDocuments));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<ReportObservation>>())).Returns(Task.FromResult((List<ReportObservation>)null));

            HashSet<Guid> newObservationIds = new HashSet<Guid>(reportObservations.ReportObservationList.Select(s => s.ObservationId));

            List<ObservationTable> observationTableList = SetObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                  .Returns(observationTableList.Where(a => !a.IsDeleted && newObservationIds.Contains(a.ObservationId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddRangeAsync(reportObservationList));

            reportObservations.ReportId = reportId;
            await _reportRepository.AddObservationsAsync(reportObservations);
            _dataRepository.Verify(x => x.AddRangeAsync<ReportObservation>(It.IsAny<IEnumerable<ReportObservation>>()), Times.Once);
        }

        /// <summary>
        /// Test cases to verify Delete report detail
        /// </summary>
        /// <returns>Task</returns> 
        [Fact]
        public async Task DeleteReportObservationAsync_ValidData()
        {

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var reportObservationToDelete = SetReportObservationsInitData();
            var reportObservation = SetReportObservationsInitData().First();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                .Returns(Task.FromResult(reportObservationToDelete.FirstOrDefault(x => x.ReportId.ToString() == reportId
                    && x.Id.ToString() == reportObservationId)));

            _dataRepository.Setup(x => x.Remove(reportObservation));

            List<ReportObservationsMember> removedObservationMemebers = SetReportObservationsMemberInitData();
            //delete report observation user 
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                .Returns(removedObservationMemebers.Where(a => a.ReportObservationId.ToString() == reportObservationId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.RemoveRange(removedObservationMemebers));
            await _reportRepository.DeleteReportObservationAsync(reportId, reportObservation.Id.ToString());
            _dataRepository.Verify(x => x.SaveChangesAsync());
        }

        #endregion

        #region Observation Tab
        /// <summary>
        /// set observation tab detail test case data
        /// </summary>
        private void SetObservationTabDetail()
        {
            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            //get report observation reviewer adn first person responsible
            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));

            List<ReportObservationsDocument> reportObservationsDocuments = SetReportObservationsDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsDocument, bool>>>()))
              .Returns(reportObservationsDocuments.Where(a => !a.IsDeleted && reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);


            List<ReportObservationTable> reportObservationTables = SetReportObservationTableInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
              .Returns(reportObservationTables.Where(a => !a.IsDeleted && reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            List<ReportObservationsMember> reportObservationMembers = SetReportObservationsMemberInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
              .Returns(reportObservationMembers.Where(a => !a.IsDeleted && reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);


            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
              .Returns(reportObservationReviewerList.Where(a => !a.IsDeleted && reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            List<Rating> ratingsList = SetRatingsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
              .Returns(ratingsList.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);

            //get linked observation list
            List<ReportObservation> linkedObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
              .Returns(linkedObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);

            List<EntityUserMapping> userMappingList = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
              .Returns(userMappingList.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);

            //get observation category
            List<ObservationCategory> observationCategoryList = SetObservationCategoryInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
              .Returns(observationCategoryList.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId)
              .AsQueryable().BuildMock().Object);

            //get report reviewer for observation review
            List<ReportUserMapping> reportUserMappingList = SetReportUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
              .Returns(reportUserMappingList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Reviewer)
              .AsQueryable().BuildMock().Object);
        }

        /// <summary>
        /// Test case varification for GetReportObservationsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]

        public async Task GetReportObservationsAsync_WithReportObservationId()
        {
            List<ReportObservation> reportObservationList = SetReportObservationsInitData();

            ReportObservation reportObservation = SetReportObservationsInitData().First();

            ReportDetailAC reportDetailAC = new ReportDetailAC();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
            .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);
            SetObservationTabDetail();
            var result = await _reportRepository.GetReportObservationsAsync(reportId, reportObservation.Id.ToString(), entityId);
            Assert.Equal(reportObservationList.Count, result.ReportObservationList.Count);
        }

        /// <summary>
        /// Test case varification for GetReportObservationsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetReportObservationsAsync_WithNullReportObservationId()
        {
            List<ReportObservation> reportObservationList = SetReportObservationsInitData();

            ReportDetailAC reportDetailAC = new ReportDetailAC();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
            .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);
            SetObservationTabDetail();
            var result = await _reportRepository.GetReportObservationsAsync(reportId, null, entityId);
            Assert.Equal(reportObservationList.Count, result.ReportObservationList.Count);
        }


        /// <summary>
        /// Test case varification UpdateReportObservationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateReportObservationAsync_ValidData()
        {

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<ReportObservation> reportObservationList = SetReportObservationsInitData();

            ReportObservation reportObservation = reportObservationList[0];
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
            .Returns(reportObservationList.Where(a => a.Id.ToString() == reportObservationId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(a => a.DetachEntityEntry(reportObservation));

            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservation>()))
                   .Returns((ReportObservation model) => (EntityEntry<ReportObservation>)null);


            List<ReportObservationsMember> getReportObservationMembers = SetReportObservationsMemberInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
            .Returns(getReportObservationMembers.Where(a => !a.IsDeleted && a.ReportObservationId.ToString() == reportObservationId).AsQueryable().BuildMock().Object);


            _dataRepository.Setup(x => x.RemoveRange(getReportObservationMembers));

            _dataRepository.Setup(x => x.AddRangeAsync(getReportObservationMembers))
            .Returns((ReportObservationsMember model) => Task.FromResult((EntityEntry<ReportObservationsMember>)null));

            //get observation reviewer from database
            List<ReportObservationReviewer> reportObservationReviewerList = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
            .Returns(reportObservationReviewerList.Where(a => !a.IsDeleted && a.ReportObservationId.ToString() == reportObservationId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddRangeAsync(reportObservationReviewerList))
            .Returns((ReportObservationReviewer model) => Task.FromResult((EntityEntry<ReportObservationReviewer>)null));
            ReportDetailAC reportDetailAC = new ReportDetailAC();
            List<ReportObservationsMemberAC> reportObservationMembers = SetReportObservationsMemberACInitData();
            List<ReportObservationAC> reportObservationACList = SetReportObservationsACInitData();
            reportObservationACList[0].PersonResponsibleList = reportObservationMembers;
            List<ReportObservationReviewerAC> reportObservationReviewerACList = SetReportObservationReviewerACInitData();
            reportObservationACList[0].ObservationReviewerList = reportObservationReviewerACList;
            reportDetailAC.ReportObservationList = reportObservationACList;
            var result = await _reportRepository.UpdateReportObservationAsync(reportDetailAC);
            _dataRepository.Verify(x => x.SaveChangesAsync());
        }

        /// <summary>
        /// Test cases to verify DeleteReportObservationDocumentAync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteReportObservationDocumentAync_ValidData()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            ReportObservationsDocument reportObservationDocument = SetReportObservationsDocumentInitData().First();
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ReportObservationsDocument, bool>>>()))
                        .Returns(Task.FromResult(reportObservationDocument));
            await _reportRepository.DeleteReportObservationDocumentAync(reportObservationDocument.Id);

        }

        #endregion

        #region Comment History
        /// <summary>
        /// Get Reports comment history
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetCommentHistoryAsync_ValidData()
        {
            ReportCommentHistoryAC reportCommentHistory = new ReportCommentHistoryAC();
            //get report Comments
            List<ReportComment> reportComments = SetReportCommentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportComment, bool>>>()))
           .Returns(reportComments.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);

            Report report = SetReportsInitData().First();
            List<Report> reportMappingObj = SetReportsInitData();

            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Report, bool>>>()))
                .Returns(report);

            List<ReportObservation> reportObservationList = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
          .Returns(reportObservationList.Where(a => !a.IsDeleted && a.ReportId.ToString() == reportId).AsQueryable().BuildMock().Object);


            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));
            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
          .Returns(reportObservationReviewerList.Where(a => !a.IsDeleted && reportObservationIds.Contains(a.ReportObservationId))
          .AsQueryable().BuildMock().Object);

            var result = await _reportRepository.GetCommentHistoryAsync(reportId, timeOffset);
            Assert.Equal(reportComments.Count, result.CommentList.Count);
        }
        #endregion

        #region Dynamic table methods unit tests
        [Fact]
        public async Task GetReportObservationTableAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var reportObservationTables = SetReportObservationTableInitData();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(Task.FromResult(reportObservationTables.FirstOrDefault(x => x.IsDeleted))); // return null since null observation table is required

            _dynamicTableRepository.Setup(x => x.AddDefaultJsonDocument()).Returns(reportObservationTables[0].Table);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<ReportObservationTable>())).ReturnsAsync(reportObservationTables.FirstOrDefault(x => !x.IsDeleted));

            await _reportRepository.GetReportObservationTableAsync(SetReportObservationsInitData()[0].Id.ToString());
            _dataRepository.Verify(mock => mock.SaveChangesAsync(), Times.AtLeastOnce);

        }

        [Fact]
        public async Task UpdateJsonDocumentAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetReportObservationTableInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservationTable>()))
                       .Returns((ReportObservationTable model) => (EntityEntry<ReportObservationTable>)null);

            await _reportRepository.UpdateJsonDocumentAsync(JsonSerializer.Serialize(observationTables[0].Table), observationTables[0].ReportObservationId.ToString(), tableId);
            _dataRepository.Verify(x => x.Update(It.IsAny<ReportObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task AddColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetReportObservationTableInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservationTable>()))
                       .Returns((ReportObservationTable model) => (EntityEntry<ReportObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.AddColumn(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(observationTables[0].Table));

            await _reportRepository.AddColumnAsync(tableId, reportObservationId);
            _dataRepository.Verify(x => x.Update(It.IsAny<ReportObservationTable>()), Times.Once);
        }
        [Fact]
        public async Task AddRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetReportObservationTableInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var observationId = SetReportObservationsInitData()[0].Id.ToString();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservationTable>()))
                       .Returns((ReportObservationTable model) => (EntityEntry<ReportObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.AddRow(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _reportRepository.AddRowAsync(tableId, SetReportObservationsInitData()[0].Id.ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ReportObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetReportObservationTableInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var reportObservationId = SetReportObservationsInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservationTable>()))
                       .Returns((ReportObservationTable model) => (EntityEntry<ReportObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteRow(It.IsAny<JsonElement>(), It.IsAny<string>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _reportRepository.DeleteRowAsync(reportObservationId, tableId, Guid.NewGuid().ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ReportObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task DeleteColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetReportObservationTableInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var reportObservationId = SetReportObservationsInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ReportObservationTable>()))
                       .Returns((ReportObservationTable model) => (EntityEntry<ReportObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteColumn(It.IsAny<JsonElement>(), It.IsAny<int>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _reportRepository.DeleteColumnAsync(reportObservationId, tableId, 1);
            _dataRepository.Verify(x => x.Update(It.IsAny<ReportObservationTable>()), Times.Once);
        }
        #endregion

        #region Generate Report PPT
        /// <summary>
        /// Test cases to verify Generate report 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GenerateReportPPTAsync_PathNotFound()
        {

            var entity = SetAuditableEntityInitData().First();
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                                .Returns(Task.FromResult(entity));


            Report report = SetReportsInitData().First();
            List<Report> reportMappingObj = SetReportsInitData();

            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Report, bool>>>()))
                .Returns(report);

            List<string> reportIds = reportMappingObj.Select(a => a.Id.ToString()).ToList();

            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                     .Returns(reportObservationMappingObj.Where(x => !x.IsDeleted && x.ReportId == new Guid(reportId)).AsQueryable().BuildMock().Object);

            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationMappingObj.Select(s => s.Id));

            List<ReportObservationsMember> reportObservationMembers = SetReportObservationsMemberInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                     .Returns(reportObservationMembers.Where(a => !a.IsDeleted
            && reportObservationIds.Contains(a.ReportObservationId)).AsQueryable().BuildMock().Object);

            GetAllPlansAndProcessesOfAllPlansByEntityIdAsync();

            //Tuple<string, string, List<PlanProcessMappingAC>> pptData = await GetProcessSubprocessData(entityId);

            List<ReportUserMapping> distributorsList = SetReportDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportUserMapping, bool>>>()))
                     .Returns(distributorsList.Where(a => !a.IsDeleted && a.ReportId == new Guid(reportId) && a.ReportUserType == ReportUserType.Distributor).AsQueryable().BuildMock().Object);

            List<EntityUserMapping> entityUserMappingList = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Returns(entityUserMappingList.Where(a => !a.IsDeleted && a.EntityId.ToString() == entityId).AsQueryable().BuildMock().Object);

            _webHostEnvironment.Setup(m => m.ContentRootPath).Returns("");

            Assert.ThrowsAsync<Exception>(() => _reportRepository.GenerateReportPPTAsync(reportId, entityId, timeOffset));

        }
        #endregion

        #region Report Observation PPT
        /// <summary>
        /// Test cases to verigy generate report observation 
        /// </summary>
        /// <returns>task</returns>
        [Fact]
        public async Task GenerateReportObservationPPTAsync_ValidData()
        {
            var entity = SetAuditableEntityInitData().First();
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                                .Returns(Task.FromResult(entity));


            List<ReportObservation> reportObservationMappingObj = SetReportObservationsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                 .Returns(reportObservationMappingObj.Where(a => !a.IsDeleted && a.Id == new Guid(reportObservationId)).AsQueryable().BuildMock().Object);


            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservation, bool>>>()))
                .Returns(reportObservationMappingObj.Where(a => !a.IsDeleted && a.ReportId == Guid.Parse(reportId)).AsQueryable().BuildMock().Object);

            //ReportObservationAC reportObservationData = await SetReportObservatonDataAsync(reportObservation, entityId, timeOffset);

            //get add table data
            List<ReportObservationTable> observationTableMappingObj = SetReportObservationTableInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ReportObservationTable, bool>>>()))
                                .Returns(Task.FromResult(observationTableMappingObj[0]));
            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = SetReportObservationReviewerInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationReviewer, bool>>>()))
                 .Returns(reportObservationReviewerList.Where(a => !a.IsDeleted && a.ReportObservationId == Guid.Parse(reportObservationId)).AsQueryable().BuildMock().Object);

            //reportObservationData = await GetCommentHistoryPPTAsync(reportObservationData, timeOffset);

            //get observation document data
            List<ReportObservationsDocument> observationDocuments = SetReportObservationsDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsDocument, bool>>>()))
                 .Returns(observationDocuments.Where(a => !a.IsDeleted && a.ReportObservationId == Guid.Parse(reportObservationId)).AsQueryable().BuildMock().Object);

            List<ReportObservationsMember> reportObservationMembers = SetReportObservationsMemberInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ReportObservationsMember, bool>>>()))
                 .Returns(reportObservationMembers.Where(a => !a.IsDeleted && a.ReportObservationId == Guid.Parse(reportObservationId)).AsQueryable().BuildMock().Object);


            _webHostEnvironment.Setup(m => m.ContentRootPath).Returns("");


            Assert.ThrowsAsync<Exception>(() => _reportRepository.GenerateReportObservationPPTAsync(reportId, entityId, timeOffset));

            //string currentPath = Path.Combine(_environment.ContentRootPath, StringConstant.WWWRootFolder, StringConstant.TemplateFolder);
            //Tuple<string, MemoryStream> fileData = await _generatePPTRepository.CreatePPTFileAsync(entityName, templateFilePath, templateData, 6, tableData);
            //string tempPath = Path.GetTempPath() + StringConstant.ImageFolder;
            //await DeleteTemporaryFilesAsync(tempPath);
            //return fileData;
        }


        #endregion

        #endregion
    }
}
