using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.UserModels;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;
using System.Text.Json;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using Microsoft.AspNetCore.Hosting;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;

namespace InternalAuditSystem.Test.ObservationTest
{
    [Collection("Register Dependency")]
    public class ObservationTest : BaseTest
    {
        #region Private Variables
        private readonly Mock<IDataRepository> _dataRepository;
        private readonly Mock<IDynamicTableRepository> _dynamicTableRepository;
        private IObservationRepository _observationDataRepository;
        private IAuditableEntityRepository _auditableEntityRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;
        private IGeneratePPTRepository _generatePPTRepository;
        public Mock<IWebHostEnvironment> _webHostEnvironment;
        private readonly Guid userId = new Guid("1c5c59f2-b26a-47cc-96d9-13f86ec926ff");
        private readonly string observationId = "715bb406-5105-4688-9ae9-b70d05231e5b";
        private readonly string entityId = "5453b4af-71d1-4718-8b6c-3ddabd6f1ff9";
        private readonly string processId = "fbde814f-25ef-4b75-b7a7-2a96e0afd4aa";
        private readonly string subProcessId = "49ed3494-9a6e-4bce-a67e-abe3a32eb9b5";
        private readonly string personResposibleId = "1c5c59f2-b26a-47cc-96d9-13f86ec926ff";
        private readonly string rcmId = "5e0ef327-29c2-439c-a7df-e270014a651a";
        private readonly string auditPlanId = "49a44777-3427-4b3f-9b68-2f713c269462";
        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly int fromYear = 2019;
        private readonly int toYear = 2020;
        #endregion

        public ObservationTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _dynamicTableRepository = bootstrap.ServiceProvider.GetService<Mock<IDynamicTableRepository>>();
            _dataRepository.Reset();
            _observationDataRepository = bootstrap.ServiceProvider.GetService<IObservationRepository>();
            _generatePPTRepository = bootstrap.ServiceProvider.GetService<IGeneratePPTRepository>();
            _auditableEntityRepository = bootstrap.ServiceProvider.GetService<IAuditableEntityRepository>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _webHostEnvironment = bootstrap.ServiceProvider.GetService<Mock<IWebHostEnvironment>>();
        }

        #region Private Methods

        /// <summary>
        /// Method for initialize observation data
        /// </summary>
        /// <returns>List of observation</returns>
        private List<Observation> SetObservationListInitData()
        {

            var observationObj = new List<Observation>()
            {
                new Observation()
                {
                   Id=new Guid(observationId),
                   Heading="heading 1",
                   Background="background-1",
                   Observations="observation-1",
                   ObservationType=DomailModel.Enums.ObservationType.Compliance,
                   ObservationStatus=DomailModel.Enums.ObservationStatus.Open,
                   IsRepeatObservation=true,
                   RootCause="missing analysis",
                   RiskAndControlId=Guid.Parse(rcmId),
                   IsDeleted=false,
                   CreatedBy=userId,
                   CreatedDateTime=DateTime.UtcNow,
                   Implication="Implication 3",
                   Disposition=DomailModel.Enums.Disposition.NonReportable,
                   Recommendation="Recommendation 3",
                   ManagementResponse="ManagementResponse 3",
                   PersonResponsible=Guid.Parse(personResposibleId),
                   ProcessId=Guid.Parse(processId),
                   EntityId= Guid.Parse(entityId)
                }
            };
            return observationObj;
        }

        /// <summary>
        /// Method to initialize observation table data
        /// </summary>
        /// <returns>List of observation table</returns>
        private List<ObservationTable> SetObservationTableListInitData()
        {
            var jsonDoc = "{\"tableId\":\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"columnNames\":[\"rowId\",\"ColumnName1\",\"ColumnName2\"],\"data\":[{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]},{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]}]}";
            var parsedDocument = JsonDocument.Parse(jsonDoc);
            var observationTableObj = new List<ObservationTable>()
            {
                new ObservationTable()
                {
                   Id=new Guid(observationId),
                   IsDeleted=false,
                   CreatedBy=userId,
                   CreatedDateTime=DateTime.UtcNow,
                   ObservationId = SetObservationListInitData()[0].Id,
                   Table = parsedDocument
                }
            };
            return observationTableObj;
        }

        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private UserAC SetUserACInitData()
        {
            UserAC userACMappingObj = new UserAC
            {

                Id = userId,
                IsDeleted = false,
                Name = "suparna",
                EmailId = "suparna@gmail.com",
                Designation = "General manager",
                UserType = DomailModel.Enums.UserType.Internal
            };
            return userACMappingObj;
        }

        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private User SetUserInitData()
        {
            User userMappingObj = new User
            {

                Id = userId,
                IsDeleted = false,
                Name = "suparna",
                EmailId = "suparna@gmail.com",
                Designation = "General manager",
                UserType = DomailModel.Enums.UserType.Internal
            };
            return userMappingObj;
        }

        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private List<User> SetUserListInitData()
        {
            List<User> userMappingObj = new List<User>
            {
                 new User()
                {

                Id = userId,
                IsDeleted = false,
                Name = "suparna",
                EmailId = "suparna@gmail.com",
                Designation = "General manager",
                UserType = DomailModel.Enums.UserType.Internal
                }
            };
            return userMappingObj;
        }

        /// <summary>
        /// Method for initializing observationAC data
        /// </summary>
        /// <returns>List of observationAC</returns>
        private List<ObservationAC> SetObservationACListInitData()
        {
            List<ObservationAC> observationACMappingObj = new List<ObservationAC>()
            {
                new ObservationAC()
                {
                    Id=new Guid(observationId),
                    Heading="heading 1",
                   Background="background-1",
                   Observations="observation-1",
                   ObservationType=DomailModel.Enums.ObservationType.Compliance,
                   ObservationStatus=DomailModel.Enums.ObservationStatus.Open,
                   IsRepeatObservation=true,
                   RootCause="missing analysis",
                   RiskAndControlId=Guid.Parse(rcmId),
                   IsDeleted=false,
                   CreatedBy=userId,
                   CreatedDateTime=DateTime.UtcNow,
                   Implication="Implication 3",
                   Disposition=DomailModel.Enums.Disposition.NonReportable,
                   Recommendation="Recommendation 3",
                   ManagementResponse="ManagementResponse 3",
                   PersonResponsible=Guid.Parse(personResposibleId),
                   ProcessId=Guid.Parse(subProcessId),
                   ParentProcessId=Guid.Parse(processId),
                   UserAC=SetUserACInitData(),
                   EntityId=Guid.Parse(entityId),
                   AuditPlanId=Guid.Parse(auditPlanId),
                   StatusString="Pending",
                   ProcessName="process-1",
                   ObservationDocuments = SetObservationDocumentACs()
                }
            };
            return observationACMappingObj;
        }

        private List<ObservationDocumentAC> SetObservationDocumentACs()
        {
            List<ObservationDocumentAC> observationDocumentACs = new List<ObservationDocumentAC>();
            var observationDocument = new ObservationDocumentAC()
            {
                Id = Guid.NewGuid(),
                DocumentPath = "DocumentPath",
                FileName = "FileName",
                ObservationId = Guid.Parse(observationId)
            };
            observationDocumentACs.Add(observationDocument);
            return observationDocumentACs;
        }

        private List<ObservationDocument> SetObservationDocuments()
        {
            List<ObservationDocument> observationDocuments = new List<ObservationDocument>();
            var observationDocument = new ObservationDocument()
            {
                Id = Guid.NewGuid(),
                DocumentPath = "DocumentPath",
                ObservationId = Guid.Parse(observationId)
            };
            observationDocuments.Add(observationDocument);
            return observationDocuments;
        }

        private ObservationAC SetObservationACInitData()
        {

            var observationObj = new ObservationAC()
            {
                Id = Guid.Parse(observationId),
                Heading = "heading 1",
                Background = "background-1",
                Observations = "observation-1",
                ObservationType = DomailModel.Enums.ObservationType.Compliance,
                ObservationStatus = DomailModel.Enums.ObservationStatus.Open,
                IsRepeatObservation = true,
                RootCause = "missing analysis",
                RiskAndControlId = Guid.Parse(rcmId),
                IsDeleted = false,
                CreatedBy = userId,
                CreatedDateTime = DateTime.UtcNow,
                Implication = "Implication 3",
                Disposition = DomailModel.Enums.Disposition.NonReportable,
                Recommendation = "Recommendation 3",
                ManagementResponse = "ManagementResponse 3",
                PersonResponsible = Guid.Parse(personResposibleId),
                ProcessId = Guid.Parse(subProcessId),
                ParentProcessId = Guid.Parse(processId),
                UserAC = SetUserACInitData(),
                EntityId = Guid.Parse(entityId),
                AuditPlanId = Guid.Parse(auditPlanId),
                StatusString = "Pending",
                ProcessName = "process-1"
            };
            return observationObj;
        }
        private List<EntityUserMapping> SetEntityUserMappingInitData()
        {

            var entityUserMapping = new List<EntityUserMapping>()
            {
                new EntityUserMapping()
                {
                    UserId=userId,
                    EntityId=Guid.Parse(entityId),
                    User=SetUserInitData()
                }
            };
            return entityUserMapping;
        }

        private List<PlanProcessMapping> SetPlanProcessMappingInitData()
        {

            var planProcessMapping = new List<PlanProcessMapping>()
            {
                new PlanProcessMapping()
                {
                 PlanId = Guid.Parse(auditPlanId),
                 ProcessId= Guid.Parse(subProcessId),
                 Process=SetProcessInitData()

                }
            };
            return planProcessMapping;
        }


        private List<ProcessAC> SetProcessACListInitData()
        {
            List<ProcessAC> processACMappingObj = new List<ProcessAC>
            {
                new ProcessAC{
                Id = Guid.Parse(processId),
                IsDeleted = false,
                Name = "process-1",
                AuditPlanId=Guid.Parse(auditPlanId),
                EntityId=Guid.Parse(entityId),
                ParentId=Guid.Parse(processId)
                }
            };
            return processACMappingObj;
        }

        private Process SetProcessInitData()
        {
            Process processMappingObj = new Process
            {
                Id = Guid.Parse(processId),
                IsDeleted = false,
                Name = "process-1",
                EntityId = Guid.Parse(entityId),
                ParentId = Guid.Parse(processId)
            };
            return processMappingObj;
        }
        /// <summary>
        /// Method for getting all observations
        /// </summary>
        /// <returns> List of observations</returns>
        [Fact]
        private async Task GetAllObservations_ReturnAllObservations()
        {
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();
            var observationData = SetObservationListInitData();
            var userData = SetEntityUserMappingInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var observationACData = SetObservationACListInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationData.Where(x => !x.IsDeleted && x.CreatedBy == userId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                    .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACData.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                     .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationData[0].Id }).AsQueryable().BuildMock().Object);

            var result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, searchString, entityId, 0, 0);
            Assert.NotNull(result);
            Assert.Equal(observationData.Count, result.Count);
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Test case to verify GetAllObservationAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_SearchString()
        {
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();
            List<Observation> observationMappingObj = SetObservationListInitData();
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading.ToLower() == "heading 1" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                  .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                   .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);


            var result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, "heading 1", entityId, 0, 0);
            Assert.Equal(observationMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_SearchObservationAC()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var userData = SetEntityUserMappingInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 1" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading.ToLower() == "heading 1" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                  .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);

            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);

            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, "heading 1", entityId, 0, 0);
            Assert.Equal(observationACMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync for invalid search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_InvalidSearchObservation()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();

            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);


            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, "heading 2", entityId, 0, 0);
            Assert.NotEqual(observationMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync for year filter with search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_YearFilterWithSearchString()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();

            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);

            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, "heading 2", entityId, fromYear, toYear);
            Assert.NotNull(result);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync for year filter without search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_YearFilterWithoutSearchString()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();

            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == null && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == null && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);

            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, null, entityId, fromYear, toYear);
            Assert.NotNull(result);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync for year filter without search string ,returns null
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationAsync_YearFilterWithoutSearchString_ReturnsNull()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var processData = SetProcessACListInitData();
            var planProcessData = SetPlanProcessMappingInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();

            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == null && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == null && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);

            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, null, entityId, 2018, 2019);
            Assert.Empty(result);

        }

        /// <summary>
        /// Test case to verify GetAllObservationAsync for year filter with search string,Retunrs null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllObservationAsync_YearFilterWithSearchString_ReturnsNull()
        {
            List<ObservationAC> observationACMappingObj = SetObservationACListInitData();
            List<Observation> observationMappingObj = SetObservationListInitData();
            var observationDocumentACs = SetObservationDocumentACs();
            var observationDocuments = SetObservationDocuments();
            var planProcessData = SetPlanProcessMappingInitData();

            var userData = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationAC, bool>>>()))
                     .Returns(observationACMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading-2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                     .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading-2" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                 .Returns(userData.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACMappingObj.Select(s => s.AuditPlanId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.PlanId)).OrderBy(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ProcessAC, bool>>>()))
                    .Returns(planProcessData.Where(x => !x.IsDeleted).Select(x => new ProcessAC { Id = x.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>()))
                    .Returns(observationDocuments.Where(x => !x.IsDeleted).Select(x => new ObservationDocument { Id = x.Id, DocumentPath = "DocumentPath", ObservationId = (Guid)observationACMappingObj[0].Id }).AsQueryable().BuildMock().Object);

            List<ObservationAC> result = await _observationDataRepository.GetAllObservationsAsync(page, pageSize, "heading-2", entityId, 2018, 2019);
            Assert.Empty(result);

        }
        /// <summary>
        /// Test case to verify GetTotalObservationsPerSearchStringAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalObservationsPerSearchStringAsync_ReturnCount()
        {
            List<Observation> observationMappingObj = SetObservationListInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                .ReturnsAsync(1);

            int result = await _observationDataRepository.GetTotalObservationsPerSearchStringAsync(searchString, entityId);
            Assert.Equal(observationMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetTotalObservationsPerSearchStringAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalObservationsPerSearchString_SearchReturnCount()
        {
            List<Observation> observationMappingObj = SetObservationListInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.Heading == "heading 1" && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);

            int result = await _observationDataRepository.GetTotalObservationsPerSearchStringAsync("heading 1", entityId);
            Assert.Equal(observationMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetTotalObservationsPerSearchStringAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalObservationsPerSearchString_NoReturnCount()
        {
            List<Observation> observationMappingObj = SetObservationListInitData();
            observationMappingObj[0].IsDeleted = true;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>()))
                .Returns(observationMappingObj.Where(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).AsQueryable().BuildMock().Object);

            int result = await _observationDataRepository.GetTotalObservationsPerSearchStringAsync(searchString, entityId);
            Assert.NotEqual(observationMappingObj.Count, result);
        }

        /// <summary>
        /// Method for adding observation return valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationAsync_ReturnData()
        {
            var observationList = SetObservationListInitData();
            var userList = SetUserListInitData();
            var observationAC = SetObservationACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Observation>())).ReturnsAsync(observationList.FirstOrDefault(x => !x.IsDeleted));

            var result = await _observationDataRepository.AddObservationAsync(observationAC);
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<Observation>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Method for adding observation returns exception
        /// </summary>
        /// <returns>NullReference Exception</returns>
        [Fact]
        public async Task AddObservationAsync_ReturnNull()
        {
            var planProcessMappingList = SetPlanProcessMappingInitData();
            var userList = SetUserListInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Observation>()))
                .Throws(new NullReferenceException());


            await Assert.ThrowsAsync<NullReferenceException>(async () =>
               await _observationDataRepository.AddObservationAsync(null));

        }

        /// <summary>
        /// Method for updating observation return valida data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateObservationAsync_ReturnData()
        {
            var userList = SetUserListInitData();
            var observationAC = SetObservationACInitData();
            var planProcessMappingList = SetPlanProcessMappingInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Update(It.IsAny<Observation>()))
                       .Returns((Observation model) => (EntityEntry<Observation>)null);


            var observationDocList = new List<ObservationDocument> { 
            new ObservationDocument()
            {
                DocumentPath = "DocumentPath",
                Id = new Guid(),
                ObservationId = new Guid(),
                IsDeleted = false,
            }
            };

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationDocument, bool>>>())).Returns(observationDocList.Where(x => x.Id == Guid.Parse(observationId) && !x.IsDeleted).AsQueryable().BuildMock().Object);

            await _observationDataRepository.UpdateObservationAsync(observationAC);
            _dataRepository.Verify(x => x.Update(It.IsAny<Observation>()), Times.Once);
        }

        /// <summary>
        /// Method for updating observation data returns null exceptions
        /// </summary>
        /// <returns>NullReference Exception</returns>
        [Fact]
        public async Task UpdateObservationAsync_ReturnNull()
        {
            var planProcessMappingList = SetPlanProcessMappingInitData();
            var userList = SetUserListInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Update(It.IsAny<Observation>()))
                .Throws(new NullReferenceException());

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                     .Returns(Task.FromResult(planProcessMappingList.FirstOrDefault(x => x.PlanId == Guid.Parse(auditPlanId)
                                                              && x.ProcessId == Guid.Parse(processId))));

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
               await _observationDataRepository.UpdateObservationAsync(null));

        }

        /// <summary>
        /// Method for deleting observation 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteObservationAync_ReturnValid()
        {
            var observationList = SetObservationListInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>())).Returns(observationList.Where(x => x.Id == Guid.Parse(observationId) && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Update(It.IsAny<Observation>()))
                       .Returns((Observation model) => (EntityEntry<Observation>)null);
            await _observationDataRepository.DeleteObservationAync(Guid.Parse(observationId));
            _dataRepository.Verify(mock => mock.SaveChangesAsync(), Times.AtLeastOnce);
        }

        /// <summary>
        /// Method for deleting observation
        /// </summary>
        /// <returns>NullReference Exception</returns>
        [Fact]
        private async Task DeleteObservationAync_ReturnInValid()
        {
            var observationList = SetObservationListInitData();
            var userList = SetUserListInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Observation, bool>>>())).Returns(observationList.Where(x => x.Id == Guid.Parse(observationId) && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Observation, bool>>>())).ReturnsAsync(observationList.First(x => x.Id == Guid.Parse(observationId) && !x.IsDeleted));
            _dataRepository.Setup(x => x.Update(It.IsAny<Observation>()))
              .Throws(new NullReferenceException());
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                   await _observationDataRepository.DeleteObservationAync(Guid.Empty));
        }

        #region Dynamic table methods unit tests
        [Fact]
        public async Task GetObservationTableAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(Task.FromResult(observationTables.FirstOrDefault(x => x.IsDeleted))); // return null since null observation table is required

            _dynamicTableRepository.Setup(x => x.AddDefaultJsonDocument()).Returns(observationTables[0].Table);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<ObservationTable>())).ReturnsAsync(observationTables.FirstOrDefault(x => !x.IsDeleted));

            await _observationDataRepository.GetObservationTableAsync(SetObservationListInitData()[0].Id.ToString());
            _dataRepository.Verify(mock => mock.SaveChangesAsync(), Times.AtLeastOnce);

        }

        [Fact]
        public async Task UpdateJsonDocumentAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ObservationTable>()))
                       .Returns((ObservationTable model) => (EntityEntry<ObservationTable>)null);

            await _observationDataRepository.UpdateJsonDocumentAsync(JsonSerializer.Serialize(observationTables[0].Table), observationTables[0].ObservationId.ToString(), tableId);
            _dataRepository.Verify(x => x.Update(It.IsAny<ObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task AddColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ObservationTable>()))
                       .Returns((ObservationTable model) => (EntityEntry<ObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.AddColumn(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(observationTables[0].Table));

            await _observationDataRepository.AddColumnAsync(tableId, SetObservationListInitData()[0].Id.ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task AddRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var observationId = SetObservationListInitData()[0].Id.ToString();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ObservationTable>()))
                       .Returns((ObservationTable model) => (EntityEntry<ObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.AddRow(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _observationDataRepository.AddRowAsync(tableId, SetObservationListInitData()[0].Id.ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var observationId = SetObservationListInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ObservationTable>()))
                       .Returns((ObservationTable model) => (EntityEntry<ObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteRow(It.IsAny<JsonElement>(), It.IsAny<string>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _observationDataRepository.DeleteRowAsync(observationId, tableId, Guid.NewGuid().ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ObservationTable>()), Times.Once);
        }

        [Fact]
        public async Task DeleteColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var observationTables = SetObservationTableListInitData();
            var tableId = observationTables[0].Table.RootElement.GetString("tableId");
            var observationId = SetObservationListInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                     .Returns(observationTables.Where(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ObservationTable, bool>>>()))
                   .Returns(observationTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ObservationTable>()))
                       .Returns((ObservationTable model) => (EntityEntry<ObservationTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteColumn(It.IsAny<JsonElement>(), It.IsAny<int>())).Returns(JsonSerializer.Serialize(observationTables[0].Table.RootElement));

            await _observationDataRepository.DeleteColumnAsync(observationId, tableId, 1);
            _dataRepository.Verify(x => x.Update(It.IsAny<ObservationTable>()), Times.Once);
        }
        #endregion

        #endregion
    }
}
