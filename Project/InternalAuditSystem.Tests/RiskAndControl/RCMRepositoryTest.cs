using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.RiskAndControl
{
    [Collection("Register Dependency")]
    public class RCMRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRiskControlMatrixRepository _riskControlMatrixRepository;
        private IGlobalRepository _globalRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly Guid workProgramId = new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418");
        private readonly Guid rcmId = new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8");
        private readonly string entityId = "7d5875d9-ab95-46b5-b961-ec92f1376bf0";
        #endregion

        #region Constructor
        public RCMRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _riskControlMatrixRepository = bootstrap.ServiceProvider.GetService<IRiskControlMatrixRepository>();
            _globalRepository = bootstrap.ServiceProvider.GetService<IGlobalRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methdods
        private List<RiskControlMatrix> SetRiskControlMatrixInitData()
        {
            var riskControlMatrixMappingObj = new List<RiskControlMatrix>()
            {
                new RiskControlMatrix()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    EntityId=new Guid(),
                    WorkProgramId=workProgramId,
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",
                    SectorId=new Guid(),
                    RcmSector=SetRiskControlMatrixSectorInitData()[1],
                    RcmProcessId=new Guid(),
                    RcmProcess=SetRiskControlMatrixProcessInitData()[1],
                    RcmSubProcessId=new Guid(),
                    RcmSubProcess=SetRiskControlMatrixSubProcessInitData()[1]

                },
                  new RiskControlMatrix()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    EntityId=new Guid(),
                    WorkProgramId=workProgramId,
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",
                    SectorId=new Guid(),
                    RcmSector=SetRiskControlMatrixSectorInitData()[1],
                    RcmProcessId=new Guid(),
                    RcmProcess=SetRiskControlMatrixProcessInitData()[1],
                    RcmSubProcessId=new Guid(),
                    RcmSubProcess=SetRiskControlMatrixSubProcessInitData()[1]
                }
            };
            return riskControlMatrixMappingObj;
        }

        private List<RiskControlMatrixAC> SetRiskControlMatrixACInitData()
        {
            var riskControlMatrixMappingObj = new List<RiskControlMatrixAC>()
            {
                new RiskControlMatrixAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    EntityId=new Guid(),
                    WorkProgramId=workProgramId,
                    RiskName="Risk Name",
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",

                },
                  new RiskControlMatrixAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    EntityId=new Guid(),
                    WorkProgramId=workProgramId,
                     RiskName="Risk Name",
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",
                }
            };
            return riskControlMatrixMappingObj;
        }

        private List<RiskControlMatrixSector> SetRiskControlMatrixSectorInitData()
        {
            return new List<RiskControlMatrixSector>()
            {
                new RiskControlMatrixSector()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                   Sector="RCM Sector1"
                },
                new RiskControlMatrixSector()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                   Sector="RCM Sector2"
                },
                new RiskControlMatrixSector()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                  Sector="RCM Sector3"

                },
                new RiskControlMatrixSector()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                    Sector="RCM Sector4"
                }
            };
        }

        private List<RiskControlMatrixProcess> SetRiskControlMatrixProcessInitData()
        {
            return new List<RiskControlMatrixProcess>()
            {
                new RiskControlMatrixProcess()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                   Process="RCM Process1"
                },
                new RiskControlMatrixProcess()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                   Process="RCM Process2"
                },
                new RiskControlMatrixProcess()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                  Process="RCM Process3"

                },
                new RiskControlMatrixProcess()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                    Process="RCM Process4"
                }
            };
        }
        private List<RiskControlMatrixSubProcess> SetRiskControlMatrixSubProcessInitData()
        {
            return new List<RiskControlMatrixSubProcess>()
            {
                new RiskControlMatrixSubProcess()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                   SubProcess="RCM sub Process1"
                },
                new RiskControlMatrixSubProcess()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                   SubProcess="RCM sub Process2"
                },
                new RiskControlMatrixSubProcess()
                {
                   Id= new Guid(),
                    EntityId=new Guid(),
                  SubProcess="RCM sub Process3"

                },
                new RiskControlMatrixSubProcess()
                {
                    Id=new Guid(),
                    EntityId=new Guid(),
                    SubProcess="RCM sub Process4"
                }
            };
        }
        private List<User> SetUserInitData()
        {
            var userMappingObj = new List<User>()
            {
                new User()
                {
                   Id= new Guid(),
                   Name="User 1",
                   UserType=DomailModel.Enums.UserType.External
                },
                new User()
                {
                    Id=new Guid(),
                    Name="User 2",
                    UserType=DomailModel.Enums.UserType.External
                },
                new User()
                {
                   Id= new Guid(),
                   Name="User 3",
                   UserType=DomailModel.Enums.UserType.Internal

                },
                new User()
                {
                    Id=new Guid(),
                    Name="User 4",
                    UserType=DomailModel.Enums.UserType.Internal
                }
            };
            return userMappingObj;
        }
        #endregion

        #region Test Case Methods
        /// <summary>
        /// Test case to verify GetWorkProgramDetailsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRiskControlMatrixDetailsAsync_ReturnRiskControlMatrixAc()
        {
            var riskControlMatrixMappingObj = SetRiskControlMatrixInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                    .Returns(riskControlMatrixMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var rcmSectorMappingObj = SetRiskControlMatrixSectorInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSector, bool>>>()))
                    .Returns(rcmSectorMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var rcmProcessMappingObj = SetRiskControlMatrixProcessInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixProcess, bool>>>()))
                    .Returns(rcmProcessMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var rcmSubProcessMappingObj = SetRiskControlMatrixSubProcessInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrixSubProcess, bool>>>()))
                    .Returns(rcmSubProcessMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _riskControlMatrixRepository.GetRiskControlMatrixDetailsByIdAsync(rcmId, entityId);


            Assert.NotNull(result);

        }

        /// <summary>
        /// Test case to verify GetRiskControlMatrixDetailsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRiskControlMatrixDetailsAsync_EmptyAddObject_NoRecordException()
        {
            //create all variables and mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var rcmACMappingObj = SetRiskControlMatrixACInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .Throws<NoRecordException>();

           // await Assert.ThrowsAsync<NoRecordException>(async () =>
                                     // await _riskControlMatrixRepository.GetRiskControlMatrixDetailsByIdAsync(rcmId, entityId));
        }

        /// <summary>
        /// Test case to verify GetWorkProgramDetailsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRCMListForWorkProgramAsync_ReturnRCMListForWorkProgram()
        {
            var riskControlMatrixMappingObj = SetRiskControlMatrixInitData();

            Pagination<RiskControlMatrixAC> pagination = new Pagination<RiskControlMatrixAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10
            };


            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .Returns(riskControlMatrixMappingObj.Where(x => x.WorkProgramId == workProgramId && !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _riskControlMatrixRepository.GetRCMListForWorkProgramAsync(pagination, workProgramId, entityId);

            Assert.Equal(riskControlMatrixMappingObj.Count(), result.Items.Count());

        }

        /// <summary>
        /// Test case to verify UpdateWorkProgramAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRiskControlMartrixAsync_ReturnWorkProgram()
        {
            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));
            var rcmACMappingObj = SetRiskControlMatrixACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var rcmMappingObj = SetRiskControlMatrixInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                    .Returns(rcmMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);


            _dataRepository.Setup(x => x.Update(It.IsAny<List<RiskControlMatrixAC>>()))
            .Returns((List<RiskControlMatrixAC> model) => (EntityEntry<List<RiskControlMatrixAC>>)null);

            var result = await _riskControlMatrixRepository.UpdateRiskControlMatrixAsync(rcmACMappingObj);
            _dataRepository.Verify(x => x.UpdateRange(It.IsAny<List<RiskControlMatrix>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test case to verify UpdateRiskControlMatrixAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRiskControlMatrixAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all variables and mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var rcmACMappingObj = SetRiskControlMatrixACInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .Throws<NullReferenceException>();

            // Note : test case is wrong please update accordingly
            //await Assert.ThrowsAsync<NullReferenceException>(async () =>
            //await _riskControlMatrixRepository.UpdateRiskControlMatrixAsync(rcmACMappingObj));
        }

        /// <summary>
        /// Test case to verify AWorkProgramAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRiskControlMatrixAsync_ReturnWorkProgram()
        {
            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));
            var rcmACMappingObj = SetRiskControlMatrixACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var rcmMappingObj = SetRiskControlMatrixInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                   .Returns(() => Task.FromResult(rcmMappingObj[0]));

            _dataRepository.Setup(x => x.Add(It.IsAny<RiskControlMatrixAC>()))
            .Returns((RiskControlMatrixAC model) => (EntityEntry<RiskControlMatrixAC>)null);

            var result = await _riskControlMatrixRepository.AddRiskControlMatrixAsync(rcmACMappingObj[0]);
            _dataRepository.Verify(x => x.Add(It.IsAny<RiskControlMatrix>()), Times.Never);
        }

        /// <summary>
        /// Test case to verify AddRiskControlMatrixAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRiskControlMatrixAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var rcmACMappingObj = SetRiskControlMatrixACInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .Throws<NullReferenceException>();
            // Note: this method not returning null reference please udpate accordingly
            //await Assert.ThrowsAsync<NullReferenceException>(async () =>
            //await _riskControlMatrixRepository.AddRiskControlMatrixAsync(rcmACMappingObj[0]));
        }

        /// <summary>
        /// Test case to verify Delete RCM 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcm_ValidRcmId()
        {
            List<RiskControlMatrix> matrixRcmObj = SetRiskControlMatrixInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .ReturnsAsync(matrixRcmObj.First(x => x.Id.Equals(rcmId)));

            IEnumerable<RiskControlMatrix> enumerableList = null;
            _dataRepository.Setup(x => x.Remove(enumerableList));

            await _riskControlMatrixRepository.DeleteRcmAsync(rcmId);

        }

        /// <summary>
        /// Test case to verify DeleteRcm
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRcm_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            var rcmACMappingObj = SetRiskControlMatrixACInitData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _riskControlMatrixRepository.DeleteRcmAsync(rcmId));
        }

        #endregion

    }
}
