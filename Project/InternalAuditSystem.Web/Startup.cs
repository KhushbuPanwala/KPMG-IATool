using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Serilog;
using Microsoft.EntityFrameworkCore;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using AutoMapper;
using InternalAuditSystem.DomailModel.DatabaseContext;
using InternalAuditSystem.DomailModel.DataRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using InternalAuditSystem.Repository.SeedData;
using InternalAuditSystem.Core.GlobalCore.Helper;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.MomRepository;
using InternalAuditSystem.Repository.Repository.ClientParticipantRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Utility.FileUtil;
using InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository;
using InternalAuditSystem.Repository.Repository.AuditTypeRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository;
using InternalAuditSystem.Repository.Repository.AuditCategoryRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using InternalAuditSystem.Repository.Repository.EntityTypeReporsitory;
using InternalAuditSystem.Repository.Repository.EntityCategoryRepository;
using InternalAuditSystem.Repository.Repository.RelationshipTypeRepository;
using InternalAuditSystem.Repository.Repository.LocationRepository;
using InternalAuditSystem.Repository.Repository.DivisionRepository;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.Utility.PdfGeneration;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.AuditTeamRepository;
using System.Text;
using InternalAuditSystem.Repository.Repository.ACMRepresentationRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository;
using InternalAuditSystem.Repository.Repository.RegionRepository;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository;
using InternalAuditSystem.Repository.Repository.CountryRepository;
using InternalAuditSystem.Repository.Repository.ProvinceStateRepository;
using InternalAuditSystem.Repository.Repository.ObservationCategoryRepository;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using InternalAuditSystem.Core.ActionFilters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using System;
using Microsoft.Azure.Storage;
using System.Linq;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;

namespace InternalAuditSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">Collection of services</param>
        public void ConfigureServices(IServiceCollection services)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            #region Static Files Setttings
            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = "wwwroot/dist";
            });
            #endregion

            #region Database Settings
            string connectionString = Configuration["PostgreSql:ConnectionString"];
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(connectionString);

            services.AddDbContext<InternalAuditSystemContext>(options => options.UseNpgsql(builder.ConnectionString));
            #endregion

            #region Miniprofiler
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            }).AddEntityFramework();
            #endregion

            #region Controller Settings
            services.AddControllersWithViews();
            #endregion

            #region Azure Ad Settings
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;

            });

            //Azure Active Directory settings
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            .AddAzureAD(options => Configuration.Bind("AzureAd", options));


            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            #region Data Protection Setting
            var storageConnectionString = Configuration["AzureCredentials:StorageConnectionString"];
            CloudStorageAccount cloudStorageAccount;
            if (CloudStorageAccount.TryParse(storageConnectionString.ToString(), out cloudStorageAccount))
            {
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                var storageContainerName = Configuration["AzureCredentials:StorageContainerName"];
                var container = cloudBlobClient.GetContainerReference(storageContainerName);
                var blob = container.GetBlockBlobReference(storageContainerName);

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

                services.AddDataProtection()
                //This blob must already exist before the application is run
                .PersistKeysToAzureBlobStorage(container, blob.Name);
            }
            #endregion
            #endregion

            #region Cors
            if (Configuration.GetValue<bool>("EnableCors"))
            {
                services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            }
            #endregion

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<EntityRestrictionFilter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerDocument();
            services.AddMvc().AddNewtonsoftJson();

            #region Dependencies
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IWorkProgramRepository, WorkProgramRepository>();
            services.AddScoped<IAuditPlanRepository, AuditPlanRepository>();
            services.AddScoped<IRcmSectorRepository, RcmSectorRepository>();
            services.AddScoped<IDistributionRepository, DistributionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStrategicAnalysisRepository, StrategicAnalysisRespository>();
            services.AddScoped<IAzureRepository, AzureRepository>();
            services.AddScoped<IGlobalRepository, GlobalRepository>();
            services.AddScoped<IMomRepository, MomRepository>();
            services.AddScoped<IClientParticipantRepository, ClientParticipantRepository>();
            services.AddScoped<IAuditableEntityRepository, AuditableEntityRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IFileUtility, AzureFileUtility>();
            services.AddScoped<IRiskControlMatrixRepository, RiskControlMatrixRepository>();
            services.AddScoped<IACMRepository, ACMRepository>();
            services.AddScoped<IDynamicTableRepository, DynamicTableRepository>();

            services.AddScoped<IAuditTypeRepository, AuditTypeRepository>();
            services.AddScoped<IRcmSubProcessRepository, RcmSubProcessRepository>();
            services.AddScoped<IRcmProcessRepository, RcmProcessRepository>();
            services.AddScoped<IAuditCategoryRepository, AuditCategoryRepository>();
            services.AddScoped<IAuditProcessSubProcessRepository, AuditProcessSubProcessRepository>();
            services.AddScoped<IEntityTypeRepository, EntityTypeRepository>();
            services.AddScoped<IEntityCategoryRepository, EntityCategoryRepository>();
            services.AddScoped<IRelationshipTypeRepository, RelationshipTypeRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IDivisionRepository, DivisionRepository>();

            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IExportToExcelRepository, ExportToExcelRepository>();
            services.AddScoped<IAuditTeamRepository, AuditTeamRepository>();
            services.AddScoped<IRiskAssessmentRepository, RiskAssessmentRepository>();
            services.AddScoped<IPrimaryGeographicalAreaRepository, PrimaryGeographicalAreaRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IEntityRelationMappingRepository, EntityRelationMappingRepository>();
            services.AddScoped<IEntityDocumentRepository, EntityDocumentRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IObservationCategoryRepository, ObservationCategoryRepository>();
            services.AddScoped<IGeneratePPTRepository, GeneratePPTRepository>();
            #endregion

            #region Form Data Setting
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });
            #endregion

            #region Auto Mapper Configuration

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EntityMapperRegistration());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Application Insights
            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry();
            #endregion
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Instance to configure an application's requestion pipeline</param>
        /// <param name="env">Information about the web hosting where the application is running in</param>
        /// <param name="context">Instance of dbcontext</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, InternalAuditSystemContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            if (Configuration.GetValue<bool>("EnableMiniProfiler"))
            {
                app.UseMiniProfiler();
            }
            app.UseStaticFiles();
            app.UseRouting();
            if (env.IsDevelopment() && Configuration.GetValue<bool>("EnableCors"))
            {
                app.UseCors("AllowAll");
            }
            //configuration for Clickjacking Vulnerability- iframe
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSpaStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToController("Index", "Home");
            });

            // Apply migrations on project startup
            context.Database.Migrate();

            // Seed country and state only 
            SeedData.Initialize(context);


        }
    }
}
