using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using InternalAuditSystem.Repository.Repository.Common;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using System.Linq.Expressions;
using System.Linq;
using InternalAuditSystem.Repository.ApplicationClasses;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.DomailModel.Models.UserModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.RiskAssessmentTest
{
    [Collection("Register Dependency")]
    public class RiskAssessmentRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRiskAssessmentRepository _riskAssessmentRepository;
        private IGlobalRepository _globalRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        #endregion

        #region Constructor
        public RiskAssessmentRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _riskAssessmentRepository = bootstrap.ServiceProvider.GetService<IRiskAssessmentRepository>();
            _globalRepository = bootstrap.ServiceProvider.GetService<IGlobalRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methdods
        private List<RiskAssessmentAC> SetRiskAssessmentACInitData()
        {
            return new List<RiskAssessmentAC>()
            {
                new RiskAssessmentAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="Risk Assessment",
                    IsDeleted=false,
                    Status=DomailModel.Enums.RiskAssessmentStatus.Pending,
                    Year=2020,
                    Summary="Participating financial institution",
                    RiskAssessmentDocumentACList=new List<RiskAssessmentDocumentAC>()
                        {
                            new RiskAssessmentDocumentAC()
                            {
                                Id=new Guid(),
                                Path="asdfjk",
                                RiskAssessmentId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),

                            },
                            new RiskAssessmentDocumentAC()
                            {
                                Id = new Guid(),
                                Path = "asdfjkddd",
                                RiskAssessmentId = new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8")
                            }
                       }
                },
                  new RiskAssessmentAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="Risk Assessment 2",
                    IsDeleted=false,
                    Status=DomailModel.Enums.RiskAssessmentStatus.UnderView,
                    Year=2020,
                    Summary="Participating financial institution",
                      RiskAssessmentDocumentACList=new List<RiskAssessmentDocumentAC>()
                        {
                            new RiskAssessmentDocumentAC()
                            {
                                Id=new Guid(),
                                Path="asdfjk",
                                RiskAssessmentId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),

                            },
                            new RiskAssessmentDocumentAC()
                            {
                                Id = new Guid(),
                                Path = "asdfjkddd",
                                RiskAssessmentId = new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9")
                            }
                       }
                }
            };
        }

        private List<RiskAssessment> SetRiskAssessmentInitData()
        {
            return new List<RiskAssessment>()
            {
                new RiskAssessment()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="Risk Assessment",
                    IsDeleted=false,
                    Status=DomailModel.Enums.RiskAssessmentStatus.Pending,
                    Year=2020,
                    Summary="Participating financial institution",
                },
                  new RiskAssessment()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="Risk Assessment 2",
                    IsDeleted=false,
                    Status=DomailModel.Enums.RiskAssessmentStatus.UnderView,
                    Year=2020,
                    Summary="Participating financial institution",

                }
            };
        }
        private List<RiskAssessmentDocument> SetRiskAssessmentDocInitData()
        {
            return new List<RiskAssessmentDocument>()
            {
                new RiskAssessmentDocument()
                {
                    Id=new Guid(),
                    Path="asdfjk",
                    RiskAssessmentId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                },
                new RiskAssessmentDocument()
                {
                    Id = new Guid(),
                    Path = "asdfjkddd",
                    RiskAssessmentId = new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8")
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
        public async Task GetRiskAssessmentDetailsAsync()
        {
            var riskAssessmentObj = SetRiskAssessmentInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<RiskAssessment, bool>>>()))
                    .Returns(() => Task.FromResult(riskAssessmentObj[0]));

            var riskAssessmentDocObj = SetRiskAssessmentDocInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskAssessmentDocument, bool>>>()))
                    .Returns(riskAssessmentDocObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _riskAssessmentRepository.GetRiskAssessmentDetailsAsync(riskAssessmentObj[0].Id);
            Assert.NotNull(result);

        }

        /// <summary>
        /// Test case to verify GetRiskAssessmentListAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRiskAssessmentListAsync()
        {
            var riskAssessmentObj = SetRiskAssessmentInitData();

            Pagination<RiskAssessmentAC> pagination = new Pagination<RiskAssessmentAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10
            };


            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskAssessment, bool>>>()))
                                        .Returns(riskAssessmentObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);


            var riskAssessmentDocObj = SetRiskAssessmentDocInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskAssessmentDocument, bool>>>()))
                    .Returns(riskAssessmentDocObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _riskAssessmentRepository.GetRiskAssessmentListAsync(pagination);

            Assert.Equal(riskAssessmentObj.Count(), result.Items.Count());

        }
        /// <summary>
        /// Test case to verify AddRiskAssessmentAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRiskAssessmentAsync_ReturnVoid()
        {
            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));
            var riskAssessmentACObj = SetRiskAssessmentACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var riskAssessmentObj = SetRiskAssessmentInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<RiskAssessment, bool>>>()))
                   .Returns(() => Task.FromResult(riskAssessmentObj[0]));

            _dataRepository.Setup(x => x.Add(It.IsAny<RiskAssessmentAC>()))
            .Returns((RiskAssessmentAC model) => (EntityEntry<RiskAssessmentAC>)null);

            await _riskAssessmentRepository.AddRiskAssessmentAsync(riskAssessmentACObj[0]);
            _dataRepository.Verify(x => x.Add(It.IsAny<RiskAssessment>()), Times.Never);
        }

        /// <summary>
        /// Test case to verify AddRiskAssessmentAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRiskAssessmentAsync_ReturnVoid()
        {
            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));
            var riskAssessmentACObj = SetRiskAssessmentACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var riskAssessmentObj = SetRiskAssessmentInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<RiskAssessment, bool>>>()))
                   .Returns(() => Task.FromResult(riskAssessmentObj[0]));

            _dataRepository.Setup(x => x.Update(It.IsAny<RiskAssessmentAC>()))
            .Returns((RiskAssessmentAC model) => (EntityEntry<RiskAssessmentAC>)null);

            await _riskAssessmentRepository.UpdateRiskAssessmentAsync(riskAssessmentACObj[0]);
            _dataRepository.Verify(x => x.Update(It.IsAny<RiskAssessment>()), Times.Once);
        }

        /// <summary>
        /// Test case to verify Delete Risk Assessment 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRiskAssessmentAync_ValidRiskAssessmentId()
        {
            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));
            var riskAssessmentACObj = SetRiskAssessmentACInitData();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var riskAssessmentObj = SetRiskAssessmentInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<RiskAssessment, bool>>>()))
                   .Returns(() => Task.FromResult(riskAssessmentObj[0]));

            _dataRepository.Setup(x => x.Update(It.IsAny<RiskAssessmentAC>()))
            .Returns((RiskAssessmentAC model) => (EntityEntry<RiskAssessmentAC>)null);

            await _riskAssessmentRepository.UpdateRiskAssessmentAsync(riskAssessmentACObj[0]);
            _dataRepository.Verify(x => x.Update(It.IsAny<RiskAssessment>()), Times.Once);

        }
        #endregion
    }
}
