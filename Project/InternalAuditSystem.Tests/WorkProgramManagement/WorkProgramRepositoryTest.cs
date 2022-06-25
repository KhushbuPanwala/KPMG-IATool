using System;
using System.Collections.Generic;

using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.WorkProgramManagement
{
    [Collection("Register Dependency")]
    public class WorkProgramRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IWorkProgramRepository _workProgramRepository;
        private IAuditPlanRepository _auditPlanRepository;
        private IUserRepository _userRepository;
        private IGlobalRepository _globalRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly Guid workProgramId = new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418");
        private readonly Guid auditPlanId = new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418");
        private readonly Guid enitityId = new Guid();
        #endregion

        #region Constructor
        public WorkProgramRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _workProgramRepository = bootstrap.ServiceProvider.GetService<IWorkProgramRepository>();
            _auditPlanRepository = bootstrap.ServiceProvider.GetService<IAuditPlanRepository>();
            _userRepository = bootstrap.ServiceProvider.GetService<IUserRepository>();
            _globalRepository = bootstrap.ServiceProvider.GetService<IGlobalRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set WorkProgram testing data
        /// </summary>
        /// <returns>List of LiveTestTasks data</returns>
        private List<WorkProgram> SetWorkProgramInitData()
        {
            var workProgramMappingObj = new List<WorkProgram>()
            {
                new WorkProgram()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="Work Program1",
                    IsDeleted=false,
                    AuditTitle="Audit Title",
                    Status= DomailModel.Enums.WorkProgramStatus.Active,
                    AuditPeriodStartDate=DateTime.Now,
                    AuditPeriodEndDate=DateTime.Now.AddDays(1),
                    Scope="Scope",
                    DraftIssues="Draft Issues",
                },
                  new WorkProgram()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    Name="Work Program3",
                    IsDeleted=false
                }
            };
            return workProgramMappingObj;
        }
        private List<WorkProgramAC> SetWorkProgramACInitData()
        {
            var workProgramACMappingObj = new List<WorkProgramAC>()
            {
                new WorkProgramAC()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    Name="Work Program 3",
                    AuditPlanId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    ProcessId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    AuditTitle="Audit Title",
                    Status= DomailModel.Enums.WorkProgramStatus.Active,
                    AuditPeriodStartDate=DateTime.Now,
                    AuditPeriodEndDate=DateTime.Now.AddDays(1),
                    Scope="Scope",
                    WorkProgramTeamACList= new List<WorkProgramTeamAC>()
                        {
                            new WorkProgramTeamAC()
                            {
                               Id=new Guid(),
                              Type=Convert.ToString( DomailModel.Enums.UserType.Internal),
                              Name="Dhaval"
                            },
                            new WorkProgramTeamAC()
                            {
                               Id=new Guid(),
                              Type=Convert.ToString( DomailModel.Enums.UserType.Internal),
                              Name="John"
                            },
                        },
                    WorkProgramClientParticipantsACList=new List<WorkProgramTeamAC>()
                        {
                            new WorkProgramTeamAC()
                            {
                               Id=new Guid(),
                              Type = Convert.ToString(DomailModel.Enums.UserType.External),
                              Name="Tom"
                            },
                            new WorkProgramTeamAC()
                            {
                               Id=new Guid(),
                                Type = Convert.ToString(DomailModel.Enums.UserType.External),
                              Name = "Jerry"
                            },
                        },
                    WorkPaperACList=new List<WorkPaperAC>()
                        {
                            new WorkPaperAC()
                            {
                                Id=new Guid(),
                                DocumentPath="asdfjk",
                                WorkProgramId=new Guid(),

                            },
                            new WorkPaperAC()
                            {
                                Id = new Guid(),
                                DocumentPath = "asdfjkddd",
                                WorkProgramId = new Guid()
                            }
                       }
                }
            };
            return workProgramACMappingObj;
        }
        private List<WorkProgramTeam> SetWorkProgramTeamInitData()
        {
            var workProgramTeamMappingObj = new List<WorkProgramTeam>()
            {
                new WorkProgramTeam()
                {
                   Id=new Guid(),
                   WorkProgramId=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                   UserId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    User =new User()
                    {
                        Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                        Name="Dhaval",
                        UserType=DomailModel.Enums.UserType.Internal
                    },
                    WorkProgram=SetWorkProgramInitData()[1]

                },
                 new WorkProgramTeam()
                {
                   Id=new Guid(),
                   WorkProgramId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                   UserId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    User =new User()
                    {
                        Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                        Name="Kanan",
                        UserType=DomailModel.Enums.UserType.External
                    },
                    WorkProgram=SetWorkProgramInitData()[1]
                }
            };
            return workProgramTeamMappingObj;
        }
        private List<AuditableEntity> SetAuditableEntityInitData()
        {
            var auditableEntityMappingObj = new List<AuditableEntity>()
            {
                new AuditableEntity()
                {
                    Id=new  Guid(),
                    IsDeleted=false
                },
                new AuditableEntity()
                {
                    Id=new Guid(),
                    IsDeleted=false
                }
            };
            return auditableEntityMappingObj;
        }
        private List<AuditPlan> SetAuditPlanInitData()
        {
            var auditPlanMappingObj = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new  Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    Title="Audit Plan (2020-2021)",
                    EntityId=new Guid()
                },
                new AuditPlan()
                {
                    Id=new  Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    Title="Audit Plan (2020-2021)",
                     EntityId=new Guid()
                }
            };
            return auditPlanMappingObj;
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
        private List<WorkPaper> SetWorkPaperInitData()
        {
            var workPaperACMappingObj = new List<WorkPaper>()
            {
                new WorkPaper()
                {
                    Id=new Guid(),
                    DocumentPath="asdfjk",
                    WorkProgramId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    WorkProgram=SetWorkProgramInitData()[0],
                    IsDeleted=false
                },
                new WorkPaper()
                {
                    Id = new Guid(),
                    DocumentPath = "asdfjkddd",
                    WorkProgramId = new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    WorkProgram=SetWorkProgramInitData()[1],

                }
    };
            return workPaperACMappingObj;
        }
        private List<PlanProcessMapping> SetPlanProcessMappingInitData()
        {
            var planProcessMappingMappingObj = new List<PlanProcessMapping>()
            {

                new PlanProcessMapping()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    ProcessId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    PlanId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    WorkProgramId=null,
                    Process=new Process()
                    {
                       Id= new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                       Name="Sub Process 1",
                       ParentId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                       ParentProcess=new Process()
                       {
                            Id= new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                            Name="Process 1",
                       }
                    },
                        AuditPlan=SetAuditPlanInitData()[0]
                    },
                 new PlanProcessMapping()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    ProcessId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    PlanId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                      WorkProgramId=SetWorkProgramACInitData()[0].Id,
                    Process=new Process()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    Name="Sub Process 2",
                     ParentId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                       ParentProcess=new Process()
                       {
                            Id= new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                            Name="Process 1",
                       }
                },
                    WorkProgram=SetWorkProgramInitData()[1],
                    AuditPlan=SetAuditPlanInitData()[0]

                }              ,
                 new PlanProcessMapping()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    ProcessId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    PlanId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                      WorkProgramId=null,
                    Process=new Process()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    Name="Sub Process 2",
                     ParentId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                       ParentProcess=new Process()
                       {
                            Id= new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                            Name="Process 1",
                       }
                },
                    WorkProgram=null,
                    AuditPlan=SetAuditPlanInitData()[0]

                },
                new PlanProcessMapping()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    ProcessId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                    PlanId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                      WorkProgramId=SetWorkProgramInitData()[0].Id,
                    Process=new Process()
                {
                    Id=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    Name="Sub Process 2",
                     ParentId=new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                       ParentProcess=new Process()
                       {
                            Id= new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c418"),
                            Name="Process 1",
                       }
                },
                    WorkProgram=SetWorkProgramInitData()[0],
                    AuditPlan=SetAuditPlanInitData()[0]

                }
            };
            return planProcessMappingMappingObj;
        }

        private List<EntityUserMapping> SetEntityUserMappingData()
        {
            return new List<EntityUserMapping>()
            {
                new EntityUserMapping
                {
                    EntityId=enitityId,
                    UserId=SetUserInitData()[0].Id,
                    User=SetUserInitData()[0]
                },
                new EntityUserMapping
                {
                    EntityId=enitityId,
                    UserId=SetUserInitData()[1].Id,
                    User=SetUserInitData()[1]
                },
                new EntityUserMapping
                {
                    EntityId=enitityId,
                    UserId=SetUserInitData()[2].Id,
                    User=SetUserInitData()[2]
                },
                new EntityUserMapping
                {
                    EntityId=enitityId,
                    UserId=SetUserInitData()[3].Id,
                    User=SetUserInitData()[3]
                }
            };
        }

        private List<RiskControlMatrix> SetRiskControlMatrixInitData()
        {
            var riskControlMatrixMappingObj = new List<RiskControlMatrix>()
            {
                new RiskControlMatrix()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa8"),
                    WorkProgramId=workProgramId,
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",
                    RcmProcessId=new Guid(),
                    RcmSubProcessId=new Guid()

                },
                  new RiskControlMatrix()
                {
                    Id=new Guid("75e9765c-286a-4afd-b4d1-57d6724f9aa9"),
                    WorkProgramId=workProgramId,
                    IsDeleted=false,
                    ControlDescription="ControlDescription",
                    RiskDescription="RiskDescription",
                    RcmProcessId=new Guid(),
                    RcmSubProcessId=new Guid(),
                }
            };
            return riskControlMatrixMappingObj;
        }
        /// <summary>
        /// Set User testing data
        /// </summary>
        /// <returns>List of Users data</returns>
        private List<Mom> SetMomInitData()
        {
            List<Mom> momMappingObj = new List<Mom>()
            {
                new Mom()
                {
                    Id = new Guid(),
                    WorkProgramId = new Guid("a47aaf20-7767-4e5b-9b3a-e9721d27c419"),
                    Agenda = "DemoMom",
                    MomDate = DateTime.UtcNow,
                    MomStartTime = DateTime.UtcNow.ToLocalTime(),
                    MomEndTime = DateTime.UtcNow.ToLocalTime(),
                    IsDeleted = false,
                }
            };
            return momMappingObj;
        }
        #endregion

        #region Test Case Methods
        /// <summary>
        /// Test case to verify GetWorkProgramDetailsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetWorkProgramDetailsAsync_ReturnWorkProgramAc()
        {
            var workProgramMappingObj = SetWorkProgramInitData();


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkProgram, bool>>>()))
                  .Returns(() => Task.FromResult(workProgramMappingObj[0]));

            var planProcessMappingMappingObj = SetPlanProcessMappingInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                     .Returns(() => planProcessMappingMappingObj.Where(x => x.WorkProgramId == workProgramMappingObj[0].Id && !x.IsDeleted).AsQueryable().BuildMock().Object);

            var workProgramTeamMappingObj = SetWorkProgramTeamInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                   .Returns(workProgramTeamMappingObj.Where(x => x.WorkProgramId == workProgramMappingObj[0].Id && !x.IsDeleted).AsQueryable().BuildMock().Object);

            var workPaperMappingObj = SetWorkPaperInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                   .Returns(workPaperMappingObj.Where(x => x.WorkProgramId == workProgramMappingObj[0].Id && !x.IsDeleted).AsQueryable().BuildMock().Object);

            var result = await _workProgramRepository.GetWorkProgramDetailsAsync(Convert.ToString(workProgramMappingObj[0].Id));
            Assert.NotNull(result);

        }

        /// <summary>
        /// Test case to verify GetAuditPlanListAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditPlanListAsync_ReturnAuditPlanACList()
        {
            var auditableEntityMappingObj = SetAuditableEntityInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                     .Returns(() => Task.FromResult(auditableEntityMappingObj[0]));

            var auditPlanMappingObject = SetAuditPlanInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                    .Returns(auditPlanMappingObject.Where(x => x.EntityId == enitityId && !x.IsDeleted)
                    .Select(x => new AuditPlan { Id = x.Id, Title = x.Title })
                    .AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetAllAuditPlansForDisplayInDropDownAsync(enitityId);
            Assert.Equal(auditPlanMappingObject.Count(), result.Count());
        }

        /// <summary>
        /// Test case to verify GetProcessListAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetProcessListAsync_ReturnProcessACList()
        {
            var planProcessMappingObject = SetPlanProcessMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessMappingObject.Where(x => x.PlanId == auditPlanId && !x.IsDeleted)
                    .AsQueryable().BuildMock().Object);

            var result = await _auditPlanRepository.GetPlanWiseAllProcessesByPlanIdAsync(auditPlanId);
            Assert.Equal(planProcessMappingObject.Count(x => x.PlanId == auditPlanId), result.Count());
        }

        /// <summary>
        /// Test case to verify AddWorkProgramAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddWorkProgramAsync_ReturnWorkProgram()
        {
            var workProgramACMappingObject = SetWorkProgramACInitData();
            WorkProgramAC workProgramACObj = workProgramACMappingObject[0];
            workProgramACObj.Id = null;
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            _dataRepository.Setup(x => x.First<WorkProgramAC>(It.IsAny<Expression<Func<WorkProgramAC, bool>>>()))
                                           .Returns(workProgramACMappingObject.First(x => x.Id == workProgramACObj.Id &&
                                            x.Name.Equals(workProgramACObj.Name, StringComparison.InvariantCultureIgnoreCase)));

            var workProgramMappingObj = SetWorkProgramInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgram, bool>>>()))
                    .Returns(workProgramMappingObj.Where(x => !x.IsDeleted && x.Name.Equals(workProgramACObj.Name, StringComparison.InvariantCultureIgnoreCase))
                    .AsQueryable().BuildMock().Object);


            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkProgram, bool>>>()))
                    .Returns(() => Task.FromResult(workProgramMappingObj[0]));


            _dataRepository.Setup(x => x.AddAsync(It.IsAny<WorkProgram>()))
               .Returns((WorkProgram model) => Task.FromResult((WorkProgram)null));

            #region Update plan process mapping table
            var planProcessMappingMappingObj = SetPlanProcessMappingInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                .Returns(planProcessMappingMappingObj.Where(x => !x.IsDeleted && x.ProcessId == workProgramACObj.ProcessId)
                .AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Update(It.IsAny<PlanProcessMappingAC>()))
            .Returns((PlanProcessMappingAC model) => null);

            #endregion

            var workPaperMappingObj = SetWorkPaperInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                       .Returns(() => Task.FromResult(workPaperMappingObj[0]));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<WorkPaper>>()))
               .Returns((List<WorkPaper> model) => Task.FromResult((EntityEntry<WorkPaper>)null));


            var workProgramTeamMappingObj = SetWorkProgramTeamInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                       .Returns(() => Task.FromResult(workProgramTeamMappingObj[0]));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<WorkProgramTeam>>()))
               .Returns((List<WorkProgramTeam> model) => Task.FromResult((EntityEntry<WorkProgramTeam>)null));

            var result = await _workProgramRepository.AddWorkProgramAsync(workProgramACObj);

            _dataRepository.Verify(x => x.Add(It.IsAny<WorkProgram>()), Times.Never);

        }

        /// <summary>
        /// Test case to verify UpdateWorkProgramAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateWorkProgramAsync_ReturnWorkProgram()
        {
            var workProgramACMappingObject = SetWorkProgramACInitData();
            WorkProgramAC workProgramACObj = workProgramACMappingObject[0];
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);

            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            var workProgramMappingObj = SetWorkProgramInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgram, bool>>>()))
                  .Returns(workProgramMappingObj.Where(x => x.Id != workProgramACObj.Id && !x.IsDeleted && x.Name.Equals(workProgramACObj.Name, StringComparison.InvariantCultureIgnoreCase))
                  .AsQueryable().BuildMock().Object);

            #region Update workprogram details
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkProgram, bool>>>()))
                    .Returns(() => Task.FromResult(workProgramMappingObj[0]));

            _dataRepository.Setup(x => x.Update(It.IsAny<WorkProgramAC>()))
            .Returns((WorkProgramAC model) => (EntityEntry<WorkProgramAC>)null);

            #endregion
            var planProcessMappingMappingObj = SetPlanProcessMappingInitData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                   .Returns(planProcessMappingMappingObj.Where(x => !x.IsDeleted && (x.WorkProgramId == workProgramACObj.Id || x.ProcessId == workProgramACObj.ProcessId))
                   .AsQueryable().BuildMock().Object);


            #region delete workpapers and workprogram team while updating
            var workPaperMappingDeleteObj = SetWorkPaperInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                     .Returns(workPaperMappingDeleteObj.Where(x => x.WorkProgramId.ToString().Equals(workProgramACObj.Id)).AsQueryable().BuildMock().Object);
            IEnumerable<WorkPaper> workpaperEnumerableList = null;
            _dataRepository.Setup(x => x.Remove(workpaperEnumerableList));

            var workProgramTeamMappingDeleteObj = SetWorkProgramTeamInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                     .Returns(workProgramTeamMappingDeleteObj.Where(x => x.WorkProgramId.ToString().Equals(workProgramACObj.Id)).AsQueryable().BuildMock().Object);
            IEnumerable<WorkProgramTeam> workProgramTeamEnumerableList = null;
            _dataRepository.Setup(x => x.Remove(workProgramTeamEnumerableList));
            #endregion

            var workPaperMappingObj = SetWorkPaperInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                       .Returns(() => Task.FromResult(workPaperMappingObj[0]));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<WorkPaper>>()))
               .Returns((List<WorkPaper> model) => Task.FromResult((EntityEntry<WorkPaper>)null));

            var workProgramTeamMappingObj = SetWorkProgramTeamInitData();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                       .Returns(() => Task.FromResult(workProgramTeamMappingObj[0]));

            _dataRepository.Setup(x => x.AddRangeAsync(It.IsAny<List<WorkProgramTeam>>()))
               .Returns((List<WorkProgramTeam> model) => Task.FromResult((EntityEntry<WorkProgramTeam>)null));

            var result = await _workProgramRepository.UpdateWorkProgramAsync(workProgramACObj);
            _dataRepository.Verify(x => x.Update(It.IsAny<WorkProgram>()), Times.Once);
        }

        /// <summary>
        /// Test case to verify GetUsersAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetUsersAsync_ReturnUserACList()
        {
            var entityUserMappingObject = SetEntityUserMappingData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                    .Returns(entityUserMappingObject.Where(x => !x.IsDeleted && x.EntityId == enitityId)
                    .AsQueryable().BuildMock().Object);

            var result = await _userRepository.GetAllUsersOfEntityAsync(enitityId);
            Assert.Equal(entityUserMappingObject.Count(), result.Count());
        }
        //TODO unit test will be done in later PR
        /// <summary>
        /// Test case to verify GetWorkProgramList
        /// </summary>
        /// <returns>WorkprogramAC list</returns>
        [Fact]
        public async Task GetWorkProgramList_ReturnWorkProgramACList()
        {
            var auditableEntityMappingObj = SetAuditableEntityInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                     .Returns(() => Task.FromResult(auditableEntityMappingObj[0]));

            Pagination<WorkProgramAC> pagination = new Pagination<WorkProgramAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10,
                EntityId = enitityId
            };
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            var planProcessMappingObject = SetPlanProcessMappingInitData();
            planProcessMappingObject = planProcessMappingObject.Where(x => x.WorkProgramId != null).ToList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                    .Returns(planProcessMappingObject.Where(x => x.AuditPlan.EntityId == pagination.EntityId
                                            && (!String.IsNullOrEmpty(pagination.searchText) ? x.WorkProgram.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                            && !x.AuditPlan.IsDeleted && !x.IsDeleted && x.WorkProgramId != null)
                                            .AsQueryable().BuildMock().Object);

            var workProgramTeamMappingObject = SetWorkProgramTeamInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                    .Returns(workProgramTeamMappingObject.Where(x => !x.IsDeleted)
                                            .AsQueryable().BuildMock().Object);

            var workPaperMappingObject = SetWorkPaperInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                    .Returns(workPaperMappingObject.Where(x => !x.IsDeleted)
                                            .AsQueryable().BuildMock().Object);

            var result = await _workProgramRepository.GetWorkProgramListAsync(pagination);
            Assert.Equal(planProcessMappingObject.Count(), result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify Delete Workprogram for valid workprogram id
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteWorkProgam_ValidWorkProgamId()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);


            var userMappingObj = SetUserInitData();

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .Returns(() => Task.FromResult(userMappingObj[0]));

            List<PlanProcessMapping> planProcessMappingObj = SetPlanProcessMappingInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>()))
                  .Returns(planProcessMappingObj.Where(x => !x.IsDeleted && x.WorkProgramId == SetWorkProgramInitData()[0].Id)
                                          .AsQueryable().BuildMock().Object);

            List<WorkProgramTeam> workProgramteamMappingObj = SetWorkProgramTeamInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkProgramTeam, bool>>>()))
                  .Returns(workProgramteamMappingObj.Where(x => !x.IsDeleted && x.WorkProgramId == SetWorkProgramInitData()[0].Id)
                                          .AsQueryable().BuildMock().Object);

            List<WorkPaper> workPaperMappingObj = SetWorkPaperInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<WorkPaper, bool>>>()))
                  .Returns(workPaperMappingObj.Where(x => !x.IsDeleted && x.WorkProgramId == SetWorkProgramInitData()[0].Id)
                                          .AsQueryable().BuildMock().Object);

            List<RiskControlMatrix> rcmMappingObj = SetRiskControlMatrixInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>()))
                  .Returns(rcmMappingObj.Where(x => !x.IsDeleted && x.WorkProgramId == SetWorkProgramInitData()[0].Id)
                                          .AsQueryable().BuildMock().Object);



            await _workProgramRepository.DeleteWorkProgramAync(SetWorkProgramInitData()[0].Id);
            _dataRepository.Verify(x => x.Update(It.IsAny<WorkProgram>()), Times.Once);

        }

        #endregion

    }
}
