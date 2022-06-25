using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using InternalAuditSystem.DomailModel.DatabaseContext;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository;
using InternalAuditSystem.Core.GlobalCore.Helper;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.MomRepository;
using InternalAuditSystem.Repository.Repository.ClientParticipantRepository;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Utility.FileUtil;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Repository.AuditTypeRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository;
using InternalAuditSystem.Repository.Repository.AuditCategoryRepository;
using InternalAuditSystem.Repository.Repository.DivisionRepository;
using InternalAuditSystem.Repository.Repository.LocationRepository;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using InternalAuditSystem.Utility.PdfGeneration;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using Microsoft.Extensions.Configuration;
using InternalAuditSystem.Repository.Repository.ACMRepresentationRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using InternalAuditSystem.Repository.Repository.EntityCategoryRepository;
using InternalAuditSystem.Repository.Repository.EntityTypeReporsitory;
using InternalAuditSystem.Repository.Repository.RegionRepository;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using InternalAuditSystem.Repository.Repository.RelationshipTypeRepository;
using InternalAuditSystem.Repository.Repository.CountryRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository;
using InternalAuditSystem.Repository.Repository.ProvinceStateRepository;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.Repository.ObservationCategoryRepository;
using InternalAuditSystem.Repository.Repository.AuditTeamRepository;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using Microsoft.AspNetCore.Hosting;

namespace InternalAuditSystem.Test
{
    //This bootstrap will be used for further testing from 11-03-2020
    // As the previous bootstrap class was using dbcontext
    // And it was not working in any existing test repositories.
    public class Bootstrap
    {
        #region public properties
        public readonly IServiceProvider ServiceProvider;
        #endregion

        #region Constructor
        public Bootstrap()
        {
            var services = new ServiceCollection();

            #region Setup parameters
            services.AddScoped(config => config.GetService<IMapper>());

            #endregion

            #region Dependecy-Injection
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IExportToExcelRepository, ExportToExcelRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IStrategicAnalysisRepository, StrategicAnalysisRespository>();
            services.AddScoped<IFileUtility, AzureFileUtility>();
            services.AddScoped<IAzureRepository, AzureRepository>();
            services.AddScoped<IRcmSectorRepository, RcmSectorRepository>();
            services.AddScoped<IDistributionRepository, DistributionRepository>();
            services.AddScoped<IWorkProgramRepository, WorkProgramRepository>();
            services.AddScoped<IAuditPlanRepository, AuditPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMomRepository, MomRepository>();
            services.AddScoped<IClientParticipantRepository, ClientParticipantRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IAuditableEntityRepository, AuditableEntityRepository>();
            services.AddScoped<IRiskControlMatrixRepository, RiskControlMatrixRepository>();
            services.AddScoped<IGlobalRepository, GlobalRepository>();
            services.AddScoped<IRcmSubProcessRepository, RcmSubProcessRepository>();
            services.AddScoped<IAuditTypeRepository, AuditTypeRepository>();
            services.AddScoped<IRcmProcessRepository, RcmProcessRepository>();
            services.AddScoped<IAuditCategoryRepository, AuditCategoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();
            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<IDynamicTableRepository, DynamicTableRepository>();
            services.AddScoped<IAuditProcessSubProcessRepository, AuditProcessSubProcessRepository>();
            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<IAuditPlanRepository, AuditPlanRepository>();
            services.AddScoped<IACMRepository, ACMRepository>();
            services.AddScoped<IRiskAssessmentRepository, RiskAssessmentRepository>();
            services.AddScoped<IEntityCategoryRepository, EntityCategoryRepository>();
            services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IRelationshipTypeRepository, RelationshipTypeRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPrimaryGeographicalAreaRepository, PrimaryGeographicalAreaRepository>();
            services.AddScoped<IEntityRelationMappingRepository, EntityRelationMappingRepository>();
            services.AddScoped<IEntityDocumentRepository, EntityDocumentRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IObservationCategoryRepository, ObservationCategoryRepository>();
            services.AddScoped<IAuditTeamRepository, AuditTeamRepository>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IGeneratePPTRepository, GeneratePPTRepository>();
            #endregion

            #region Mocks
            //DBContext
            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(x => x.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]).Returns("40d4fde1-f5df-4770-90bd-81b4ea3f621e");

            services.AddScoped(x => httpContextMock);
            services.AddScoped(x => httpContextMock.Object);

            //DBContext
            var dbContextMock = new Mock<InternalAuditSystemContext>();
            services.AddScoped(x => dbContextMock);
            services.AddScoped(x => dbContextMock.Object);

            //WebHostEnvironment
            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            services.AddScoped(x => webHostEnvironment);
            services.AddScoped(x => webHostEnvironment.Object);
            //DataRepository
            var dataRepositoryMock = new Mock<IDataRepository>();
            dataRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(0));
            services.AddSingleton(x => dataRepositoryMock);
            services.AddSingleton(x => dataRepositoryMock.Object);

            //DataRepository
            var dynamicTableRepositoryMock = new Mock<IDynamicTableRepository>();
            services.AddSingleton(x => dynamicTableRepositoryMock);
            services.AddSingleton(x => dynamicTableRepositoryMock.Object);

            //File Utility
            var fileUtilityMock = new Mock<IFileUtility>();
            services.AddSingleton(x => fileUtilityMock);
            services.AddSingleton(x => fileUtilityMock.Object);

            //AzureRepository
            var azureRepositoryMock = new Mock<IAzureRepository>();
            services.AddSingleton(x => azureRepositoryMock);
            services.AddSingleton(x => azureRepositoryMock.Object);

            //ViewRenderService
            var viewRenderServiceMock = new Mock<IViewRenderService>();
            services.AddSingleton(x => viewRenderServiceMock.Object);


            //configuration file
            var configurationMock = new Mock<IConfiguration>();
            services.AddSingleton(x => configurationMock.Object);
            #endregion


            #region Auto Mapper Configuration
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                cfg.AddProfile(new EntityMapperRegistration());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

#pragma warning restore CS0618 // Type or member is obsolete

            ServiceProvider = services.BuildServiceProvider();
        }
        #endregion

    }
}
