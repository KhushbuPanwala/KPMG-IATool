using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
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
using System.Threading.Tasks;
using Xunit;


namespace InternalAuditSystem.Test.ReportManagement.DistributionRepositoryTest
{
    [Collection("Register Dependency")]

    public class DistributionRepositoryTest : BaseTest
    {

        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IDistributionRepository _distributionRepository;
        private IExportToExcelRepository _exportToExcelRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string distributorId = "715bb406-5105-4688-9ae9-b70d05231e5b";
        private readonly string enityUserMappingId = "fe7fcd1f-f6bd-4863-8bf6-7f028f8db6c8";
        private readonly string userId = "b4d3c0fd-d291-4a63-b405-6aef66f7d7fa";
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public DistributionRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _distributionRepository = bootstrap.ServiceProvider.GetService<IDistributionRepository>();
            _exportToExcelRepository = bootstrap.ServiceProvider.GetService<IExportToExcelRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods
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
                    Id=new Guid(distributorId),
                    IsDeleted=false,
                }
            };
            return userMappingObj;
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
        /// Mocking of DistributorsAC testing data
        /// </summary>
        /// <returns>List of DistributorsAC data</returns>
        private void MockGetDistributorData()
        {

            var distributorMappingObj = SetDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                     .Returns(distributorMappingObj.Where(x => !x.IsDeleted && x.EntityId == new Guid(entityId)).AsQueryable().BuildMock().Object);
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetDistributorsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsAsync_ReturnDistributorsAC()
        {
            MockGetDistributorData();

            List<Distributors> distributorMappingObj = SetDistributorsInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                     .Returns(distributorMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _distributionRepository.GetDistributorsAsync(page, pageSize, searchString, entityId);
            Assert.Equal(distributorMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetDistributorsAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsAsync_SearchString()
        {
            MockGetDistributorData();

            List<DistributorsAC> distributorACMappingObj = SetDistributorsACInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<DistributorsAC, bool>>>()))
                     .Returns(distributorACMappingObj.Where(x => !x.IsDeleted && x.Name == "test").AsQueryable().BuildMock().Object);

            var result = await _distributionRepository.GetDistributorsAsync(page, pageSize, "test", entityId);
            Assert.Empty(result);

        }

        /// <summary>
        /// Test case to verify GetDistributorsAsync for invalid search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsAsync_InvalidSearchDistributors()
        {
            MockGetDistributorData();
            List<DistributorsAC> distributorACMappingObj = SetDistributorsACInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<DistributorsAC, bool>>>()))
                     .Returns(distributorACMappingObj.Where(x => !x.IsDeleted && x.Name == "distributor").AsQueryable().BuildMock().Object);

            List<DistributorsAC> result = await _distributionRepository.GetDistributorsAsync(page, pageSize, "distributor", entityId);
            Assert.NotEqual(distributorACMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetDistributorsCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsCountAsync_ReturnCount()
        {
            MockGetDistributorData();
            List<Distributors> distributorMappingObj = SetDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                .Returns(distributorMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _distributionRepository.GetDistributorsCountAsync(searchString, entityId);
            Assert.Equal(distributorMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify GetDistributorsCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsCountAsync_SearchReturnCount()
        {
            MockGetDistributorData();
            List<DistributorsAC> distributorACMappingObj = SetDistributorsACInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<DistributorsAC, bool>>>()))
                .Returns(distributorACMappingObj.Where(x => !x.IsDeleted && x.Name == "test").AsQueryable().BuildMock().Object);

            int result = await _distributionRepository.GetDistributorsCountAsync("test", entityId);
            Assert.Equal(0, result);

        }

        /// <summary>
        /// Test case to verify GetDistributorsCountAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDistributorsCountAsync_NoReturnCount()
        {
            MockGetDistributorData();
            List<Distributors> distributorMappingObj = SetDistributorsInitData();
            distributorMappingObj[0].IsDeleted = true;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                .Returns(distributorMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            int result = await _distributionRepository.GetDistributorsCountAsync(searchString, entityId);
            Assert.NotEqual(distributorMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify Delete Distributors for valid distributor id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteDistributors_ValidDistributorsId()
        {
            List<Distributors> distributorMappingObj = SetDistributorsInitData();

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Distributors, bool>>>()))
                     .Returns(() => Task.FromResult(distributorMappingObj[0]));

            await _distributionRepository.DeleteDistributorAsync(distributorId);
            _dataRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

        }

        #endregion

        /// <summary>
        /// Test case to verify GetDistributorsAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetUsersAsync()
        {

            List<EntityUserMapping> entityUserMappingObj = SetEntityUserMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Returns(entityUserMappingObj.Where(x => !x.IsDeleted && x.EntityId == entityUserMappingObj[0].EntityId).AsQueryable().BuildMock().Object);

            List<Distributors> distributorsMappingObj = SetDistributorsInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Distributors, bool>>>()))
                     .Returns(distributorsMappingObj.Where(x => !x.IsDeleted && x.EntityId == distributorsMappingObj[0].EntityId).AsQueryable().BuildMock().Object);

            var result = await _distributionRepository.GetUsersAsync(entityId);
            Assert.Equal(entityUserMappingObj.Count, result.Count);

        }


        /// <summary>
        /// Test case to verify Add Distributor for valid data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRating_ValidData()
        {

            List<EntityUserMappingAC> entityUserMappingACObj = SetEntityUserMappingACInitData();

            List<User> userMappingObj = SetUserInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
               .Returns(() => Task.FromResult(userMappingObj[0]));

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);


            var distributorMappingObj = SetDistributorsInitData();
            _dataRepository.Setup(x => x.AddRangeAsync(distributorMappingObj))
                  .Returns((Distributors model) => Task.FromResult((EntityEntry<Distributors>)null));

            await _distributionRepository.AddDistributorsAsync(entityUserMappingACObj);
            _dataRepository.Verify(x => x.AddRangeAsync<Distributors>(It.IsAny<IEnumerable<Distributors>>()), Times.Once);

        }

    }
}
