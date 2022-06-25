using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository;
using Microsoft.AspNetCore.Http;
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

namespace InternalAuditSystem.Test.RiskAndControl
{
    [Collection("Register Dependency")]

    public class RcmProcessRepositoryTest : BaseTest
    {

        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRcmProcessRepository _rcmProcessRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        private readonly string ProcessId = "fa25b374-e348-4364-9dce-bb0ff40c08b8";
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public RcmProcessRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _rcmProcessRepository = bootstrap.ServiceProvider.GetService<IRcmProcessRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set Ratings testing data
        /// </summary>
        /// <returns>List of Ratings data</returns>
        private List<RiskControlMatrixProcess> SetRcmProcessData()
        {
            var rcmProcessMappingObj = new List<RiskControlMatrixProcess>()
            {
                new RiskControlMatrixProcess()
                {
                    Id = new Guid(ProcessId),
                    EntityId=new Guid(),
                    Process="test"
                }
            };
            return rcmProcessMappingObj;
        }

        /// <summary>
        /// Set RcmProcessAC testing data
        /// </summary>
        /// <returns>List of RcmProcessAC data</returns>
        private List<RcmProcessAC> SetRcmProcessACInitData()
        {
            var rcmProcessACMappingObj = new List<RcmProcessAC>()
            {
                new RcmProcessAC()
                {
                    Id= new Guid(),
                    EntityId=new Guid(),
                    Process="test"
                }
            };
            return rcmProcessACMappingObj;
        }

        #endregion

        #region Testing Methods Completed

        /// <summary>
        /// Test case to verify GetRcmProcessAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmProcessAsync_VarifyToGetAllProcesses()
        {
            var rcmProcessMappingObj = SetRcmProcessData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                     .Returns(rcmProcessMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _rcmProcessRepository.GetRcmProcessAsync(page, pageSize, searchString, entityId);
            Assert.Equal(rcmProcessMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetRcmProcessAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmProcessAsync_VarifyToSearchProcess()
        {
            var rcmProcessMappingObj = SetRcmProcessData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                     .Returns(rcmProcessMappingObj.Where(x => !x.IsDeleted && x.Process == "test").AsQueryable().BuildMock().Object);

            var result = await _rcmProcessRepository.GetRcmProcessAsync(page, pageSize, "test", entityId);
            Assert.Equal(rcmProcessMappingObj.Count, result.Count);

        }


        /// <summary>
        /// Test case to verify  GetRcmProcessCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmProcessCountAsync_VarifyToGetProcessCount()
        {
            var rcmProcessMappingObj = SetRcmProcessData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                .Returns(rcmProcessMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _rcmProcessRepository.GetRcmProcessCountAsync(searchString);
            Assert.Equal(rcmProcessMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetRcmProcessCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRcmProcessCountAsync_VarifyToSearchProcessCount()
        {
            var rcmProcessMappingObj = SetRcmProcessData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                .Returns(rcmProcessMappingObj.Where(x => !x.IsDeleted && x.Process == "test").AsQueryable().BuildMock().Object);

            var result = await _rcmProcessRepository.GetRcmProcessCountAsync("test");
            Assert.Equal(rcmProcessMappingObj.Count, result);

        }

        /// <summary>
        /// Test case to verify Delete Rcm Process
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RiskControlMatrixProcess> matrixProcesssObj = SetRcmProcessData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _rcmProcessRepository.DeleteRcmProcess(ProcessId));
        }




        /// <summary>
        /// Test case to verify Add RCM Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmProcess_VerifyProcessAddition()
        {
            List<RcmProcessAC> rcmProcessACMappingObj = SetRcmProcessACInitData();
            List<RiskControlMatrixProcess> matrixProcesssObj = SetRcmProcessData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                .Returns(matrixProcesssObj.Where(x => x.Process == "test").AsQueryable().BuildMock().Object);

            RcmProcessAC result = await _rcmProcessRepository.AddRcmProcess(rcmProcessACMappingObj[0]);
            _dataRepository.Verify(v => v.AddAsync(It.IsAny<RiskControlMatrixProcess>()), Times.Once);

            Assert.Equal(rcmProcessACMappingObj[0].Process, result.Process);
        }

        /// <summary>
        /// Test case to verify Add Rcm Sub-Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRcmProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            List<RcmProcessAC> rcmProcessACMappingObj = SetRcmProcessACInitData();
            rcmProcessACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<RiskControlMatrixProcess>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmProcessRepository.AddRcmProcess(rcmProcessACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Update RCM Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmProcess_VerifyUpdatedRcmProcessData()
        {
            List<RcmProcessAC> rcmProcessACMappingObj = SetRcmProcessACInitData();
            List<RiskControlMatrixProcess> matrixProcesssObj = SetRcmProcessData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(s => s.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                .Returns(matrixProcesssObj.Where(x => x.Process == "test").AsQueryable().BuildMock().Object);

            RcmProcessAC result = await _rcmProcessRepository.UpdateRcmProcess(rcmProcessACMappingObj[0]);
            _dataRepository.Verify(v => v.Update(It.IsAny<RiskControlMatrixProcess>()), Times.Once);

            Assert.Equal(rcmProcessACMappingObj[0].Process, result.Process);
        }

        /// <summary>
        /// Test case to verify Update Rcm Process data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRcmProcess_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing           
            List<RcmProcessAC> rcmProcessACMappingObj = SetRcmProcessACInitData();
            rcmProcessACMappingObj[0] = null;

            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<RiskControlMatrixProcess>()))
                .Throws(new NullReferenceException());

            await Assert.ThrowsAsync<NullReferenceException>(async () => await _rcmProcessRepository.UpdateRcmProcess(rcmProcessACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Delete Process 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmProcess_ValidRcmProcessId()
        {
            List<RiskControlMatrixProcess> matrixProcesssObj = SetRcmProcessData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                     .ReturnsAsync(matrixProcesssObj.First(x => x.Id.ToString().Equals(ProcessId)));

            IEnumerable<RiskControlMatrixProcess> enumerableList = null;
            _dataRepository.Setup(x => x.Remove(enumerableList));

            await _rcmProcessRepository.DeleteRcmProcess(ProcessId);

        }

        /// <summary>
        /// Test case to verify DeleteProcess
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcmProcess_ThrowDuplicateReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RiskControlMatrixProcess> matrixProcesssObj = SetRcmProcessData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                     .Throws<DuplicateRCMException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _rcmProcessRepository.DeleteRcmProcess(ProcessId));
        }

        #endregion

    }
}
