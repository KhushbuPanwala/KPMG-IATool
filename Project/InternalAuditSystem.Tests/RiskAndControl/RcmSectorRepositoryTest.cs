using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.RiskAndControl.RcmSectorTest
{
    [Collection("Register Dependency")]
    public class RcmSectorRepositoryTest : BaseTest
    {
        #region Private Variable
        private Mock<IDataRepository> _dataRepository;
        private IRcmSectorRepository _rcmSectorRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string sectorId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public RcmSectorRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _rcmSectorRepository = bootstrap.ServiceProvider.GetService<IRcmSectorRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set RCM Sector testing data
        /// </summary>
        /// <returns>List of RCM sector data</returns>
        private List<RiskControlMatrixSector> SetRcmSectorData()
        {
            var rcmSectorMappingObj = new List<RiskControlMatrixSector>()
            {
                new RiskControlMatrixSector()
                {
                    Id = new Guid(sectorId),
                    EntityId=new Guid(),
                    Sector="test"
                }
            };
            return rcmSectorMappingObj;
        }

        /// <summary>
        /// Set RcmSectorAC testing data
        /// </summary>
        /// <returns>List of RcmSectorAC data</returns>
        private List<RcmSectorAC> SetRcmSectorACInitData()
        {
            var rcmSectorACMappingObj = new List<RcmSectorAC>()
            {
                new RcmSectorAC()
                {
                    Id= new Guid(),
                    EntityId=new Guid(),
                    Sector="test"
                }
            };
            return rcmSectorACMappingObj;
        }

        #endregion

        # region Testing Methods Completed

        /// <summary>
        /// Test Case to varify PostRcmSectorAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSectorAsync_VarifyToReturnRcmSectorAc()
        {
            var rcmSectorMappingObj = SetRcmSectorData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                     .Returns(rcmSectorMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _rcmSectorRepository.GetRcmSectorAsync(page, pageSize, searchString, entityId);
            Assert.Equal(rcmSectorMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRcmSectorAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSectorAsync_VarifyToSearchRcmSectorAC()
        {
            var rcmSectorMappingObj = SetRcmSectorData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                     .Returns(rcmSectorMappingObj.Where(x => !x.IsDeleted && x.Sector == "test").AsQueryable().BuildMock().Object);

            var result = await _rcmSectorRepository.GetRcmSectorAsync(page, pageSize, "test", entityId);
            Assert.Equal(rcmSectorMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRcmSectorCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSectorCountAsync_VarifyToReturnCount()
        {
            var passedEntityId = new Guid(entityId);
            var rcmSectorMappingObj = SetRcmSectorData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                .Returns(rcmSectorMappingObj.Where(x => !x.IsDeleted && x.EntityId == passedEntityId).AsQueryable().BuildMock().Object);

            var result = await _rcmSectorRepository.GetRcmSectorCountAsync(entityId, searchString);
            //Assert.Equal(rcmSectorMappingObj.Count, result, passedEntityId.ToString());

        }

        /// <summary>
        /// Test case to verify GetRcmSectorCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSectorCountAsync_VarifySearchOfRcmSectorCount()
        {
            var passedEntityId = new Guid(entityId);
            var rcmSectorMappingObj = SetRcmSectorData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                .Returns(rcmSectorMappingObj.Where(x => !x.IsDeleted && x.Sector == "test" && x.EntityId == passedEntityId).AsQueryable().BuildMock().Object);

            var result = await _rcmSectorRepository.GetRcmSectorCountAsync(entityId, "test");
            //Assert.Equal(rcmSectorMappingObj.Count, result, passedEntityId.ToString());

        }


        /// <summary>
        /// Test case to verify Add RCM Sector data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmSector_VerifySectorAddition()
        {
            List<RcmSectorAC> rcmSectorACMappingObj = SetRcmSectorACInitData();
            List<RiskControlMatrixSector> matrixSectorsObj = SetRcmSectorData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                .Returns(matrixSectorsObj.Where(x => x.Sector == "test").AsQueryable().BuildMock().Object);

            RcmSectorAC result = await _rcmSectorRepository.AddRcmSector(rcmSectorACMappingObj[0]);
            _dataRepository.Verify(v => v.AddAsync(It.IsAny<RiskControlMatrixSector>()), Times.Once);

            Assert.Equal(rcmSectorACMappingObj[0].Sector, result.Sector);
        }

        /// <summary>
        /// Test case to verify Add Rcm Sector data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmSector_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            List<RcmSectorAC> rcmSectorACMappingObj = SetRcmSectorACInitData();
            rcmSectorACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<RiskControlMatrixSector>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmSectorRepository.AddRcmSector(rcmSectorACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Update RCM Sector data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmSector_VerifyUpdatedRcmSectorData()
        {
            List<RcmSectorAC> rcmSectorACMappingObj = SetRcmSectorACInitData();
            List<RiskControlMatrixSector> matrixSectorsObj = SetRcmSectorData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                .Returns(matrixSectorsObj.Where(x => x.Sector == "test").AsQueryable().BuildMock().Object);

            RcmSectorAC result = await _rcmSectorRepository.UpdateRcmSector(rcmSectorACMappingObj[0]);
            _dataRepository.Verify(v => v.Update(It.IsAny<RiskControlMatrixSector>()), Times.Once);

            Assert.Equal(rcmSectorACMappingObj[0].Sector, result.Sector);
        }

        /// <summary>
        /// Test case to verify Update Rcm Sector data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmSector_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing           
            List<RcmSectorAC> rcmSectorACMappingObj = SetRcmSectorACInitData();
            rcmSectorACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<RiskControlMatrixSector>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmSectorRepository.UpdateRcmSector(rcmSectorACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Delete Sector 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmSector_ValidRcmSectorId()
        {
            List<RiskControlMatrixSector> matrixSectorsObj = SetRcmSectorData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                     .ReturnsAsync(matrixSectorsObj.First(x => x.Id.ToString().Equals(sectorId)));

            IEnumerable<RiskControlMatrixSector> enumerableList = null;
            _dataRepository.Setup(x => x.Remove(enumerableList));

            await _rcmSectorRepository.DeleteRcmSector(sectorId);

        }

        /// <summary>
        /// Test case to verify DeleteSector
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmSector_ThrowDuplicateReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RiskControlMatrixSector> matrixSectorObj = SetRcmSectorData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                     .Throws<DuplicateRCMException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _rcmSectorRepository.DeleteRcmSector(sectorId));
        }

        #endregion

    }
}
