using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.Common;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
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
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Enums;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;

namespace InternalAuditSystem.Test.AuditableEntityModule.AuditableEntityTest
{
    [Collection("Register Dependency")]
    public class AuditableEntityRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IAuditableEntityRepository _auditableEntityRepository;
        private IGlobalRepository _globalRepository;
        private IRiskAssessmentRepository _riskAssessmentRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        #endregion

        #region Constructor
        public AuditableEntityRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _auditableEntityRepository = bootstrap.ServiceProvider.GetService<IAuditableEntityRepository>();
            _riskAssessmentRepository = bootstrap.ServiceProvider.GetService<IRiskAssessmentRepository>();
            _globalRepository = bootstrap.ServiceProvider.GetService<IGlobalRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methdods

        private List<User> SetUserInitData()
        {
            var userMappingObj = new List<User>()
            {
                new User()
                {
                   Id= new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                   Name="User 1",
                   UserType=UserType.External,
                   Designation="Engagement Manager"
                },
                new User()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="User 2",
                    UserType=UserType.External,
                   Designation="Engagement Manager"
                },
                new User()
                {
                   Id= new Guid(),
                   Name="User 3",
                   UserType=UserType.Internal

                },
                new User()
                {
                    Id=new Guid(),
                    Name="User 4",
                    UserType=UserType.Internal
                }
            };
            return userMappingObj;
        }

        private List<AuditPlan> SetAuditPlanInitData()
        {
            return new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Title="Audit plan 1",
                    IsDeleted=false,
                    EntityId=new Guid(),

                },
            };
        }

        private List<AuditableEntityAC> SetAuditableEntityACInitData()
        {
            return new List<AuditableEntityAC>()
            {
                new AuditableEntityAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="AuditableEntity 1",
                    IsDeleted=false,
                     ParentEntityId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),

                },
                 new AuditableEntityAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa7"),
                    Name="AuditableEntity 2",
                    IsDeleted=false,
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),

                },
            };
        }
        private List<AuditableEntity> SetAuditableEntityInitData()
        {
            return new List<AuditableEntity>()
            {
                new AuditableEntity()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="AuditableEntity 1",
                    IsDeleted=false,
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    EntityType=new EntityType()
                    {
                        Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                        TypeName="Entity Type 1"
                    },
                    Version=1,
                    Status=AuditableEntityStatus.Active,
                    IsStrategyAnalysisDone=true

                },
                new AuditableEntity()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="AuditableEntity 2",
                    IsDeleted=false,
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    EntityType=new EntityType()
                    {
                        Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                        TypeName="Entity Type 2"
                    },
                    Version=1,
                    Status=AuditableEntityStatus.Closed,
                    IsStrategyAnalysisDone=true
                },
                 new AuditableEntity()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="AuditableEntity 1.1",
                    IsDeleted=false,
                    SelectedTypeId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    EntityType=new EntityType()
                    {
                        Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                        TypeName="Entity Type 2"
                    },
                    Version=2,
                    Status=AuditableEntityStatus.Closed,
                    IsStrategyAnalysisDone=true,
                    ParentEntityId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                },
            };
        }
        private List<EntityUserMapping> SetEntityUserMappingInitData()
        {
            return new List<EntityUserMapping>()
            {
                new EntityUserMapping()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    EntityId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    IsDeleted=false,
                    UserId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    User=SetUserInitData()[0],
                },
                new EntityUserMapping()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    EntityId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    IsDeleted=false,
                    UserId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    User=SetUserInitData()[1],
                },
                new EntityUserMapping()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa7"),
                    EntityId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    IsDeleted=false,
                    UserId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    User=SetUserInitData()[0],
                },
            };
        }
        /// <summary>
        /// Set entity type testing data
        /// </summary>
        /// <returns>List of entity category</returns>
        private List<EntityCategory> SetEntityCategoryInitialData()
        {
            List<EntityCategory> entityCategoryeObjList = new List<EntityCategory>()
            {
                new EntityCategory()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    CategoryName="cat-1",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                },
                new EntityCategory()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    CategoryName="cat-2",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                },
                 new EntityCategory()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa5"),
                    CategoryName="cat-3",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                }
            };
            return entityCategoryeObjList;
        }
        /// <summary>
        /// Set entity type testing data
        /// </summary>
        /// <returns>List of entity type</returns>
        private List<EntityType> SetEntityTypeInitialData()
        {
            List<EntityType> entityTypeObjList = new List<EntityType>()
            {
                new EntityType()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa5"),
                    TypeName="type-1",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                },
                new EntityType()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa4"),
                    TypeName="type-2",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                },
                 new EntityType()
                {
                    Id=new Guid(),
                    TypeName="type-3",
                    EntityId=SetAuditableEntityInitData()[0].Id,
                    IsDeleted=false,
                }
            };
            return entityTypeObjList;
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
                    Status=RiskAssessmentStatus.Pending,
                    Year=2020,
                    Summary="Participating financial institution",
                },
                  new RiskAssessment()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="Risk Assessment 2",
                    IsDeleted=false,
                    Status=RiskAssessmentStatus.UnderView,
                    Year=2020,
                    Summary="Participating financial institution",

                }
            };
        }
        /// <summary>
        /// Set Risk Assessment Doc Init Data
        /// </summary>
        /// <returns>List of RiskAssessmentDocument</returns>
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

        /// <summary>
        /// Set Reports testing data
        /// </summary>
        /// <returns>List of Reports data</returns>
        private List<Report> SetReportsInitData()
        {
            List<Report> reportMappingObj = new List<Report>()
            {
                new Report()
                {
                    Id=new Guid(),
                    ReportTitle="Report",
                    EntityId=new Guid(),
                    RatingId =new Guid(),
                    Stage=ReportStage.Draft,
                    AuditPeriodStartDate= DateTime.UtcNow,
                    AuditPeriodEndDate= DateTime.UtcNow,
                    AuditStatus=ReportStatus.Initial,
                    IsDeleted=false,
                }
            };
            return reportMappingObj;
        }
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
                    Id = new Guid(),
                    Heading="test",
                    Recommendation = "test",
                    Implication = "test",
                    IsDeleted=false,
                    EntityId=new Guid()
                }
            };
            return AcmPresentationMappingObj;
        }
        #endregion

        #region Test Case Methods
        /// <summary>
        /// Test case to verify GetRiskAssessmentListAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditableEntityListAsync_ReturnPaginationOfAuditableEntityAC()
        {
            Pagination<AuditableEntityAC> pagination = new Pagination<AuditableEntityAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10
            };

            var auditableEntityListObj = SetAuditableEntityInitData();
            var userList = new List<User>();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                                        .Returns(auditableEntityListObj.Where(x => x.IsStrategyAnalysisDone
                                        && (!string.IsNullOrEmpty(pagination.searchText) ? x.Name.ToLower().Contains(pagination.searchText.ToLower()) : true) && !x.IsDeleted).AsQueryable().BuildMock().Object);


            var entityUserMappingObj = SetEntityUserMappingInitData();
            List<Guid> entityIdList = auditableEntityListObj.Select(x => x.Id).ToList();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                    .Returns(entityUserMappingObj.Where(x => entityIdList.Contains(x.EntityId)
                                         && x.User.Designation.ToLower() == StringConstant.EngagementManager.ToLower()
                                         && !x.IsDeleted && !x.User.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<User, bool>>>()))
                   .Returns(userList.Where(x => x.Id == new Guid())
                   .AsQueryable().BuildMock().Object);

            var result = await _auditableEntityRepository.GetAuditableEntityListAsync(pagination);

            Assert.Equal(auditableEntityListObj.Count(), result.Items.Count());

        }
        /// <summary>
        /// Test case to verify GetAuditableEntityDetailsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditableEntityDetailsAsync_ReturnAuditableEntityAc()
        {
            var AuditableEntityMappingObj = SetAuditableEntityInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                  .Returns(() => Task.FromResult(AuditableEntityMappingObj[0]));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                 .Returns(AuditableEntityMappingObj.Where(x => x.ParentEntityId == AuditableEntityMappingObj[0].Id
                                      && !x.IsDeleted).AsQueryable().BuildMock().Object);
            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                    .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId == AuditableEntityMappingObj[0].Id).AsQueryable().BuildMock().Object);

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                              .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId == AuditableEntityMappingObj[0].Id).AsQueryable().BuildMock().Object);

            var result = await _auditableEntityRepository.GetAuditableEntityDetailsAsync(AuditableEntityMappingObj[0].Id, 1);
            Assert.NotNull(result);

            var result2 = await _auditableEntityRepository.GetAuditableEntityDetailsAsync(AuditableEntityMappingObj[0].Id, 2);
            Assert.NotNull(result2);

        }

        /// <summary>
        /// Test case to verify UpdateAuditableEntityAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateAuditableEntityAsync_ReturnAuditableEntity()
        {
            var auditableEntityACMappingObject = SetAuditableEntityACInitData();
            AuditableEntityAC auditableEntityACObj = auditableEntityACMappingObject[0];
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            var auditableEntityMappingObj = SetAuditableEntityInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                  .Returns(auditableEntityMappingObj.Where(x => x.Id != auditableEntityACObj.Id && x.ParentEntityId != auditableEntityACObj.ParentEntityId && x.Id != auditableEntityACObj.ParentEntityId && !x.IsDeleted && x.Name.ToLower() == auditableEntityACObj.Name.ToLower())
                  .AsQueryable().BuildMock().Object);

            #region Update AuditableEntity details
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                    .Returns(() => Task.FromResult(auditableEntityMappingObj[0]));

            _dataRepository.Setup(x => x.Update(It.IsAny<AuditableEntityAC>()))
            .Returns((AuditableEntityAC model) => null);

            #endregion


            await _auditableEntityRepository.UpdateAuditableEntityAsync(auditableEntityACObj);
            _dataRepository.Verify(x => x.Update(It.IsAny<AuditableEntity>()), Times.Once);
        }

        /// <summary>
        /// Test case to verify AddAuditableEntityAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditableEntityAsync_ReturnAuditableEntity()
        {
            var auditableEntityACMappingObject = SetAuditableEntityACInitData();
            AuditableEntityAC auditableEntityACObj = auditableEntityACMappingObject[0];
            auditableEntityACObj.Id = null;
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var userMappingObj = SetUserInitData();



            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userMappingObj[0]);

            var auditableEntityMappingObj = SetAuditableEntityInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                  .Returns(auditableEntityMappingObj.Where(x => !x.IsDeleted && x.Name.ToLower() == auditableEntityACObj.Name.ToLower())
                  .AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<AuditableEntity>()))
               .Returns((AuditableEntity model) => Task.FromResult((AuditableEntity)null));



            var result = await _auditableEntityRepository.AddAuditableEntityAsync(auditableEntityACObj);

            _dataRepository.Verify(x => x.Add(It.IsAny<AuditableEntity>()), Times.Never);

        }

        /// <summary>
        /// Test case to verify DeleteAuditableEntityAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAuditableEntityAsync_ReturnTask()
        {
            //NOTE: Uncomment this code when test cases will be updated in later pr
            //{
            //    var auditableEntityACMappingObject = SetAuditableEntityACInitData();
            //    AuditableEntityAC auditableEntityACObj = auditableEntityACMappingObject[0];
            //    var mockedDbTransaction = new Mock<IDbContextTransaction>();
            //    _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            //    var userList = new List<User>();
            //    var userMappingObj = SetUserInitData();

            //    _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
            //         .Returns(() => Task.FromResult(userMappingObj[0]));


            //    var auditableEntityMappingObj = SetAuditableEntityInitData();
            //    _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
            //          .Returns(SetAuditPlanInitData().Where(x => x.EntityId == auditableEntityACObj.Id && !x.IsDeleted)
            //          .AsQueryable().BuildMock().Object);

            //    var reportMappingObj = SetReportsInitData();
            //    _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Report, bool>>>()))
            //          .Returns(SetReportsInitData().Where(x => x.EntityId == auditableEntityACObj.Id && !x.IsDeleted)
            //          .AsQueryable().BuildMock().Object);
            //    var aCMPresentationMappingObj = SetACMPresentationInitData();
            //    _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ACMPresentation, bool>>>()))
            //          .Returns(SetACMPresentationInitData().Where(x => x.EntityId == auditableEntityACObj.Id && !x.IsDeleted)
            //          .AsQueryable().BuildMock().Object);
            //    _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<User, bool>>>()))
            //           .Returns(userList.Where(x=>x.CurrentSelectedEntityId == auditableEntityACObj.Id)
            //           .AsQueryable().BuildMock().Object);

            //    #region Update AuditableEntity details
            //    _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
            //            .Returns(() => Task.FromResult(auditableEntityMappingObj[0]));

            //    _dataRepository.Setup(x => x.Update(It.IsAny<AuditableEntityAC>()))
            //    .Returns((AuditableEntityAC model) => (EntityEntry<AuditableEntityAC>)null);

            //    #endregion


            //    await _auditableEntityRepository.DeleteAuditableEntityAync(auditableEntityACObj.Id ?? new Guid());
            //    _dataRepository.Verify(x => x.Update(It.IsAny<AuditableEntity>()), Times.Once);

        }

        /// <summary>
        /// Test case to verify AddAuditableEntityAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task CreateNewVersionAsync_ReturnVoid()
        {
            var auditableEntityACMappingObject = SetAuditableEntityACInitData();
            AuditableEntityAC auditableEntityACObj = auditableEntityACMappingObject[0];
            auditableEntityACObj.Id = null;
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userMappingObj[0]);
            var auditableEntityMappingObj = SetAuditableEntityInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                  .Returns(auditableEntityMappingObj.Where(x => !x.IsDeleted && x.Name.ToLower() == auditableEntityACObj.Name.ToLower())
                  .AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<AuditableEntity>()))
               .Returns((AuditableEntity model) => Task.FromResult((AuditableEntity)null));
            var result = await _auditableEntityRepository.AddAuditableEntityAsync(auditableEntityACObj);

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

            var result1 = await _riskAssessmentRepository.GetRiskAssessmentListAsync(pagination);


            _dataRepository.Verify(x => x.Add(It.IsAny<AuditableEntity>()), Times.Never);

        }
        #endregion
    }
}
