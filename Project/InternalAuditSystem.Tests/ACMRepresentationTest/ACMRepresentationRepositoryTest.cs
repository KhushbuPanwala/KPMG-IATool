using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ACMRepresentationRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
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
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace InternalAuditSystem.Test.ACMPresentationTest
{
    [Collection("Register Dependency")]
    public class ACMRepresentationRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IACMRepository _acmPresentationRepository;
        private IAuditableEntityRepository _auditableEntityRepository;
        private readonly Mock<IDynamicTableRepository> _dynamicTableRepository;
        private IGeneratePPTRepository _generatePPTRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;
        public Mock<IWebHostEnvironment> _webHostEnvironment;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string AcmId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string reportId = "a47aaf20-7767-4e5b-9b3a-e9721d27c418";
        private readonly string entityId = "c94d4237-359e-431e-a50a-5b619871ca84";
        private readonly string personResposibleId = "1c5c59f2-b26a-47cc-96d9-13f86ec926ff";
        private readonly int fromYear = 2019;
        private readonly int toYear = 2020;
        private readonly Guid userId = new Guid("1c5c59f2-b26a-47cc-96d9-13f86ec926ff");
        #endregion

        #region Constructor
        public ACMRepresentationRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _acmPresentationRepository = bootstrap.ServiceProvider.GetService<IACMRepository>();
            _auditableEntityRepository = bootstrap.ServiceProvider.GetService<IAuditableEntityRepository>();
            _dynamicTableRepository = bootstrap.ServiceProvider.GetService<Mock<IDynamicTableRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _generatePPTRepository = bootstrap.ServiceProvider.GetService<IGeneratePPTRepository>();
            _webHostEnvironment = bootstrap.ServiceProvider.GetService<Mock<IWebHostEnvironment>>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set ACM testing data
        /// </summary>
        /// <returns>List of ACM data</returns>
        private List<ACMPresentation> SetACMPresentationInitData()
        {
            var AcmPresentationMappingObj = new List<ACMPresentation>()
            {
                new ACMPresentation()
                {
                    Id = new Guid(AcmId),
                    Heading="test",
                    Recommendation = "test",
                    Implication = "test",
                    Rating = SetRatingInitData()[1],
                    RatingId = new Guid(),
                    ManagementResponse = "Test",
                    Observation = "test",
                    Status = DomailModel.Enums.ACMStatus.Open,
                    EntityId = Guid.Parse(entityId),
                    CreatedBy=userId,
                    CreatedDateTime=DateTime.UtcNow,
                    IsDeleted=false
                }
            };
            return AcmPresentationMappingObj;
        }
        private List<Report> SetACMPresentationReportInitData()
        {
            return new List<Report>()
            {
                new Report()
                {
                    Id = new Guid(reportId),
                    ReportTitle = "Report title"
                }
            };
        }

        /// <summary>
        /// Set ACMPresentationAC testing data
        /// </summary>
        /// <returns>List of ACMPresentationAC data</returns>
        private List<ACMPresentationAC> SetACMPresentationACInitData()
        {
            var acmPresentationACMappingObj = new List<ACMPresentationAC>()
            {
                new ACMPresentationAC()
                {
                    Id = new Guid(),
                    Heading="test",
                    ReportId = new Guid(),
                    Recommendation = "test",
                    Implication = "test",
                    ACMReportTitle = "test",
                    RatingId = new Guid(),
                    ManagementResponse = "Test",
                    Observation = "test",
                    Status = DomailModel.Enums.ACMStatus.Open,
                    ACMReportStatus = DomailModel.Enums.ACMReportStatus.Complete,
                    EntityId = Guid.Parse(entityId),
                    //Report = SetACMPresentationReportInitData()[1],
                    CreatedBy=userId,
                    CreatedDateTime=DateTime.UtcNow,
                    IsDeleted=false,
                    ReviewerList=SetReviewerInitData()
                }
            };
            return acmPresentationACMappingObj;
        }

        private List<ACMReportMappingAC> SetReviewerInitData()
        {
            return new List<ACMReportMappingAC>()
            {
                new ACMReportMappingAC()
                {
                    Id=userId,
                    ACMId=Guid.Parse(AcmId),
                   Name="Niriksha",
                   Designation="Enagement manager"
                }
            };
        }
        private List<Rating> SetRatingInitData()
        {
            return new List<Rating>()
            {
                new Rating()
                {
                    Id = new Guid(),
                    EntityId=new Guid(),
                    Ratings = "Rating 1"
                },
                new Rating()
                {
                    Id = new Guid(),
                    EntityId=new Guid(),
                    Ratings = "Rating 2"
                }
            };
        }

        private List<ACMDocument> SetACMDocumentInitData()
        {
            return new List<ACMDocument>()
            {
                new ACMDocument()
                {
                    Id = new Guid(),
                    ACMPresentationId=new Guid(),
                },
                new ACMDocument()
                {
                    Id = new Guid(),
                    ACMPresentationId=new Guid(),
                }
            };
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
        /// Method to initialize acm table data
        /// </summary>
        /// <returns>List of acm table</returns>
        private List<ACMTable> SetACMTableListInitData()
        {
            var jsonDoc = "{\"tableId\":\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"columnNames\":[\"rowId\",\"ColumnName1\",\"ColumnName2\"],\"data\":[{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]},{\"RowData\":[\"" +
                          Guid.NewGuid().ToString() +
                          "\",\"Data1\",\"Data2\"]}]}";
            var parsedDocument = JsonDocument.Parse(jsonDoc);
            var acmTableObj = new List<ACMTable>()
            {
                new ACMTable()
                {
                   Id=new Guid(AcmId),
                   IsDeleted=false,
                   CreatedBy=userId,
                   CreatedDateTime=DateTime.UtcNow,
                   ACMId = SetACMPresentationInitData()[0].Id,
                   Table = parsedDocument
                }
            };
            return acmTableObj;
        }

        #endregion

        #region Testing Methods Completed

        #region 
        /// <summary>
        /// Test case to verify GetACMAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetACMAsync_VarifyToGetAllAcmPresentation()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .Returns(acmPresentationMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _acmPresentationRepository.GetACMDataAsync(page, pageSize, searchString, entityId, 0, 0);
            Assert.Equal(acmPresentationMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetAcmPresentationAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAcmPresentationAsync_VarifySearch()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .Returns(acmPresentationMappingObj.Where(x => !x.IsDeleted && x.Heading == "test").AsQueryable().BuildMock().Object);

            var result = await _acmPresentationRepository.GetACMDataAsync(page, pageSize, "test", entityId, 0, 0);
            Assert.Equal(acmPresentationMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetAcmPresentationAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAcmPresentationACAsync_VarifySearchAC()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .Returns(acmPresentationMappingObj.Where(x => !x.IsDeleted && x.Heading == "test").AsQueryable().BuildMock().Object);

            List<ACMPresentationAC> result = await _acmPresentationRepository.GetACMDataAsync(page, pageSize, "test", entityId, 0, 0);
            Assert.Equal(acmPresentationMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify  GetAcmPresentationCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAcmPresentationCountAsync_VarifyToGetACMCount()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                .Returns(acmPresentationMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _acmPresentationRepository.GetACMCountAsync(searchString, entityId);
            Assert.Equal(acmPresentationMappingObj.Count, result);
        }

        /// <summary>
        /// Test case to verify GetAcmPresentationCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAcmPresentationCountAsync_VarifyToSearchACMCount()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                .Returns(acmPresentationMappingObj.Where(x => !x.IsDeleted && x.Heading == "test").AsQueryable().BuildMock().Object);

            var result = await _acmPresentationRepository.GetACMCountAsync("test", entityId);
            Assert.Equal(acmPresentationMappingObj.Count, result);
        }

        /// <summary>
        /// Test case to verify GetACMDetailsByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetACMDetailsByIdAsync_ReturnRiskControlMatrixAc()
        {
            var acmPresentationMappingObj = SetACMPresentationInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                    .Returns(() => Task.FromResult(acmPresentationMappingObj[0]));

            var ratingMappingObj = SetRatingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                    .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.EntityId == new Guid(entityId)).AsQueryable().BuildMock().Object);

            var acmDocumentMappingObj = SetACMDocumentInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMDocument, bool>>>()))
                    .Returns(acmDocumentMappingObj.Where(x => !x.IsDeleted && x.ACMPresentationId == new Guid()).AsQueryable().BuildMock().Object);


            var result = await _acmPresentationRepository.GetACMDetailsByIdAsync(new Guid(AcmId), entityId);

            Assert.NotNull(result);
        }

        /// <summary>
        /// Test case to verify GetACMDetailsByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetACMDetailsByIdAsync_EmptyAddObject_NoRecordException()
        {
            //create all variables and mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var acmPresentationMappingObj = SetACMPresentationInitData();
            List<Rating> ratingsMappingObj = SetRatingInitData();

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Rating, bool>>>()))
                    .Returns(ratingsMappingObj.Where(a => !a.IsDeleted && a.EntityId == new Guid(entityId))
                    .AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NoRecordException>(async () =>
                                      await _acmPresentationRepository.GetACMDetailsByIdAsync(new Guid(AcmId), entityId));
        }

        /// <summary>
        /// Test case to verify Add ACM data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddACMAsync_VerifyACMAddition()
        {
            List<ACMPresentationAC> acmPresentationACObj = SetACMPresentationACInitData();
            var reviewerList = SetReviewerInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(acmPresentationACObj[0]))
                    .Returns((ACMPresentationAC model) => Task.FromResult((ACMPresentationAC)null));

            ACMPresentationAC result = await _acmPresentationRepository.AddACMAsync(acmPresentationACObj[0]);
            _dataRepository.Verify(v => v.AddAsync(It.IsAny<ACMPresentation>()), Times.Once);

            Assert.Equal(acmPresentationACObj[0].Heading, result.Heading);
        }

        /// <summary>
        /// Test case to verify Delete ACM 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAcmAsync_ValidACMId()
        {
            List<ACMPresentation> acmPresentationMappingObj = SetACMPresentationInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .ReturnsAsync(acmPresentationMappingObj.First(x => x.Id.ToString().Equals(AcmId)));

            IEnumerable<ACMPresentation> enumerableList = null;
            _dataRepository.Setup(x => x.Remove(enumerableList));

            await _acmPresentationRepository.DeleteAcmAsync(new Guid(AcmId));

        }

        /// <summary>
        /// Test case to verify Delete ACM
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAcmAsync_ThrowDuplicateReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var acmPresentationMappingObj = SetACMPresentationInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _acmPresentationRepository.DeleteAcmAsync(new Guid(AcmId)));
        }
        #endregion

        #region Dynamic table methods unit tests

        /// <summary>
        /// Test case to get all table data and verify GetACMTableAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetACMTableAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(Task.FromResult(acmTables.FirstOrDefault(x => x.IsDeleted))); // return null since null acm table is required

            _dynamicTableRepository.Setup(x => x.AddDefaultJsonDocument()).Returns(acmTables[0].Table);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<ACMTable>())).ReturnsAsync(acmTables.FirstOrDefault(x => !x.IsDeleted));

            await _acmPresentationRepository.GetACMTableAsync(SetACMPresentationInitData()[0].Id.ToString());
            _dataRepository.Verify(mock => mock.SaveChangesAsync(), Times.AtLeastOnce);

        }

        /// <summary>
        /// Test case to verify UpdateJsonDocumentAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateJsonDocumentAsync_ReturnJsonDocString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();
            var tableId = acmTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                     .Returns(acmTables.Where(x => x.ACMId.ToString() == AcmId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(acmTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ACMTable>()))
                       .Returns((ACMTable model) => (EntityEntry<ACMTable>)null);

            await _acmPresentationRepository.UpdateJsonDocumentAsync(JsonSerializer.Serialize(acmTables[0].Table), acmTables[0].ACMId.ToString(), tableId);
            _dataRepository.Verify(x => x.Update(It.IsAny<ACMTable>()), Times.Once);
        }

        /// <summary>
        /// Test case to add new column and verify AddColumnAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();
            var tableId = acmTables[0].Table.RootElement.GetString("tableId");

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                     .Returns(acmTables.Where(x => x.ACMId.ToString() == AcmId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(acmTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ACMTable>()))
                       .Returns((ACMTable model) => (EntityEntry<ACMTable>)null);
            _dynamicTableRepository.Setup(x => x.AddColumn(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(acmTables[0].Table));

            await _acmPresentationRepository.AddColumnAsync(tableId, SetACMPresentationInitData()[0].Id.ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ACMTable>()), Times.Once);
        }

        /// <summary>
        /// Test case to add new row and verify AddRowAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();
            var tableId = acmTables[0].Table.RootElement.GetString("tableId");
            var acmId = SetACMPresentationInitData()[0].Id.ToString();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                     .Returns(acmTables.Where(x => x.ACMId.ToString() == AcmId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(acmTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ACMTable>()))
                       .Returns((ACMTable model) => (EntityEntry<ACMTable>)null);
            _dynamicTableRepository.Setup(x => x.AddRow(It.IsAny<JsonElement>())).Returns(JsonSerializer.Serialize(acmTables[0].Table.RootElement));

            await _acmPresentationRepository.AddRowAsync(tableId, SetACMPresentationInitData()[0].Id.ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ACMTable>()), Times.Once);
        }

        /// <summary>
        /// Test case to delete row and verify DeleteRowAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRowAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();
            var tableId = acmTables[0].Table.RootElement.GetString("tableId");
            var ACMId = SetACMPresentationInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                     .Returns(acmTables.Where(x => x.ACMId.ToString() == ACMId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(acmTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ACMTable>()))
                       .Returns((ACMTable model) => (EntityEntry<ACMTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteRow(It.IsAny<JsonElement>(), It.IsAny<string>())).Returns(JsonSerializer.Serialize(acmTables[0].Table.RootElement));

            await _acmPresentationRepository.DeleteRowAsync(ACMId, tableId, Guid.NewGuid().ToString());
            _dataRepository.Verify(x => x.Update(It.IsAny<ACMTable>()), Times.Once);
        }

        /// <summary>
        /// Test case to delete column and verify DeleteColumnAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteColumnAsync_ReturnJsonDocumentString()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var userList = SetUserListInitData();
            var acmTables = SetACMTableListInitData();
            var tableId = acmTables[0].Table.RootElement.GetString("tableId");
            var ACMId = SetACMPresentationInitData()[0].Id.ToString();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                     .Returns(acmTables.Where(x => x.ACMId.ToString() == ACMId
                    && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<ACMTable, bool>>>()))
                   .Returns(acmTables.FirstOrDefault(x => x.Table.RootElement.GetString("tableId") == tableId));
            _dataRepository.Setup(x => x.Update(It.IsAny<ACMTable>()))
                       .Returns((ACMTable model) => (EntityEntry<ACMTable>)null);
            _dynamicTableRepository.Setup(x => x.DeleteColumn(It.IsAny<JsonElement>(), It.IsAny<int>())).Returns(JsonSerializer.Serialize(acmTables[0].Table.RootElement));

            await _acmPresentationRepository.DeleteColumnAsync(ACMId, tableId, 1);
            _dataRepository.Verify(x => x.Update(It.IsAny<ACMTable>()), Times.Once);
        }
        #endregion

        #endregion
    }
}