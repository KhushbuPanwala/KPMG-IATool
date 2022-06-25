using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InternalAuditSystem.Test.RiskAndControl
{
    [Collection("Register Dependency")]
    public class RcmSubProcessRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRcmSubProcessRepository _rcmSubProcessRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string subProcessId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public RcmSubProcessRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _rcmSubProcessRepository = bootstrap.ServiceProvider.GetService<IRcmSubProcessRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set Rcm Sub Process testing data
        /// </summary>
        /// <returns>List of Ratings data</returns>
        private List<RiskControlMatrixSubProcess> SetRcmSubProcessInitData()
        {
            var rcmSubProcessMappingObj = new List<RiskControlMatrixSubProcess>()
            {
                new RiskControlMatrixSubProcess()
                {
                    Id = new Guid(subProcessId),
                    EntityId=new Guid(),
                    SubProcess="test"
                }
            };
            return rcmSubProcessMappingObj;
        }

        /// <summary>
        /// Set RcmSubProcessAC testing data
        /// </summary>
        /// <returns>List of RcmSubProcessAC data</returns>
        private List<RcmSubProcessAC> SetRcmSubProcessACInitData()
        {
            var rcmSubProcessACMappingObj = new List<RcmSubProcessAC>()
            {
                new RcmSubProcessAC()
                {
                    Id= new Guid(),
                    EntityId=new Guid(),
                    SubProcess="test"
                }
            };
            return rcmSubProcessACMappingObj;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetRcmSubProcessAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSubProcessAsync_VarifyToReturnRcmSectorAc()
        {
            var rcmSubProcessMappingObj = SetRcmSubProcessInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>())).Returns(rcmSubProcessMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _rcmSubProcessRepository.GetRcmSubProcessAsync(page, pageSize, searchString, entityId);
            Assert.Equal(rcmSubProcessMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRcmSubProcessAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSubProcessAsync_VerifyToGetSearchValueOfRcmSubProcess()
        {
            var ratingMappingObj = SetRcmSubProcessInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                     .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.SubProcess == "test").AsQueryable().BuildMock().Object);

            var result = await _rcmSubProcessRepository.GetRcmSubProcessAsync(page, pageSize, "test", entityId);
            Assert.Equal(ratingMappingObj.Count, result.Count);

        }


        /// <summary>
        /// Test case to verify GetRcmSubProcessCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSubProcessCountAsync_VerifyToReturnCount()
        {
            var ratingMappingObj = SetRcmSubProcessInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _rcmSubProcessRepository.GetRcmSubProcessCountAsync(entityId, searchString);
            Assert.Equal(ratingMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetRcmSubProcessCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmSubProcessCountAsync_VerifyToReturnSearchCount()
        {
            var ratingMappingObj = SetRcmSubProcessInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.SubProcess == "test").AsQueryable().BuildMock().Object);

            var result = await _rcmSubProcessRepository.GetRcmSubProcessCountAsync(entityId, "test");
            Assert.Equal(ratingMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify Delete Rcm Subprocess
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmSubProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RiskControlMatrixSubProcess> matrixSubProcesssObj = SetRcmSubProcessInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _rcmSubProcessRepository.DeleteRcmSubProcess(subProcessId));
        }




        /// <summary>
        /// Test case to verify Add RCM SubProcess data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmSubProcess_VerifySubProcessAddition()
        {
            List<RcmSubProcessAC> rcmSubProcessACMappingObj = SetRcmSubProcessACInitData();
            List<RiskControlMatrixSubProcess> matrixSubProcesssObj = SetRcmSubProcessInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                .Returns(matrixSubProcesssObj.Where(x => x.SubProcess == "test").AsQueryable().BuildMock().Object);

            RcmSubProcessAC result = await _rcmSubProcessRepository.AddRcmSubProcess(rcmSubProcessACMappingObj[0]);
            _dataRepository.Verify(v => v.AddAsync(It.IsAny<RiskControlMatrixSubProcess>()), Times.Once);

            Assert.Equal(rcmSubProcessACMappingObj[0].SubProcess, result.SubProcess);
        }

        /// <summary>
        /// Test case to verify Add Rcm Sub-Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmSubProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            List<RcmSubProcessAC> rcmSubProcessACMappingObj = SetRcmSubProcessACInitData();
            rcmSubProcessACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<RiskControlMatrixSubProcess>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmSubProcessRepository.AddRcmSubProcess(rcmSubProcessACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Update RCM SubProcess data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmSubProcess_VerifyUpdatedRcmSubProcessData()
        {
            List<RcmSubProcessAC> rcmSubProcessACMappingObj = SetRcmSubProcessACInitData();
            List<RiskControlMatrixSubProcess> matrixSubProcesssObj = SetRcmSubProcessInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                .Returns(matrixSubProcesssObj.Where(x => x.SubProcess == "test").AsQueryable().BuildMock().Object);

            RcmSubProcessAC result = await _rcmSubProcessRepository.UpdateRcmSubProcess(rcmSubProcessACMappingObj[0]);
            _dataRepository.Verify(v => v.Update(It.IsAny<RiskControlMatrixSubProcess>()), Times.Once);

            Assert.Equal(rcmSubProcessACMappingObj[0].SubProcess, result.SubProcess);
        }

        /// <summary>
        /// Test case to verify Update Rcm Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing           
            List<RcmSubProcessAC> rcmSubProcessACMappingObj = SetRcmSubProcessACInitData();
            rcmSubProcessACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<RiskControlMatrixSubProcess>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmSubProcessRepository.UpdateRcmSubProcess(rcmSubProcessACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Delete SubProcess 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmSubProcess_ValidRcmSubProcessId()
        {
            List<RiskControlMatrixSubProcess> matrixSubProcesssObj = SetRcmSubProcessInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                     .ReturnsAsync(matrixSubProcesssObj.First(x => x.Id.ToString().Equals(subProcessId)));

            IEnumerable<RiskControlMatrixSubProcess> enumerableList = null;
            _dataRepository.Setup(x => x.Remove(enumerableList));

            await _rcmSubProcessRepository.DeleteRcmSubProcess(subProcessId);

        }

        /// <summary>
        /// Test case to verify DeleteSubProcess
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmSubProcess_ThrowDuplicateReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RiskControlMatrixSubProcess> matrixSubProcesssObj = SetRcmSubProcessInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                     .Throws<DuplicateRCMException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _rcmSubProcessRepository.DeleteRcmSubProcess(subProcessId));
        }

        #endregion
    }
}
