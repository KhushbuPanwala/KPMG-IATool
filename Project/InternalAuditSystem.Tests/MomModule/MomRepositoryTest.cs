using InternalAuditSystem.DomailModel.DataRepository;
using Moq;
using System;
using System.Collections.Generic;

using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using System.Linq;
using InternalAuditSystem.Repository.Repository.MomRepository;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.Models.MomModels;
using System.Linq.Expressions;
using InternalAuditSystem.DomailModel.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Utility.PdfGeneration;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.MomModule
{
    [Collection("Register Dependency")]
    public class MomRepositoryTest : BaseTest
    {
        #region Private Variables
        private readonly Mock<IDataRepository> _dataRepository;
        private IMomRepository _momDataRepository;
        private IUserRepository _userDataRepository;
        private IWorkProgramRepository _workProgramDataRepository;
        private IAuditableEntityRepository _audiableEntityDataRepository;
        private IExportToExcelRepository _exportToExcelRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IViewRenderService> _viewRenderService;
        private readonly Mock<IConfiguration> _iConfig;
        private readonly Guid userId = new Guid("351218dd-6035-45ba-bb9f-c67146644b82");// d1c04eb6-e4db-4b7f-a36c-f96150da1592
        private readonly Guid planProcessId = new Guid("f0f58460-eeac-4390-936d-26cab74e9eb9");
        private readonly Guid momId = new Guid("fe68434c-2ee2-48a3-87a5-dba743732866");
        private readonly Guid mainPointId = new Guid("bb90aeca-3647-4c72-86c4-07d8e2cddd95");
        private readonly Guid subPointId = new Guid("df181f6d-2494-458f-a9f1-ee3d17fa0e7e");
        private readonly Guid momUserMappingId = new Guid("847a67a7-b4db-4bff-b32e-5fdbaa71af42");
        private readonly Guid momUserMappingIdExternal = new Guid("240d8e51-9b19-4ad4-8e08-2975d77f07a1");
        private readonly Guid workProgramId = new Guid("aa5aac53-f83e-4749-b931-1feb417ca85a");
        private readonly Guid auditableEntityId = new Guid("a93c9ac5-be95-4f88-8bcf-d9906248cb44");
        MomAC momDemoObj = new MomAC();
        MomAC momDemoObjException = new MomAC();
        Mom momObj = new Mom();
        List<User> userList = new List<User>();
        List<UserAC> userListForException = new List<UserAC>();
        List<UserAC> userListAc = new List<UserAC>();
        List<SubPointOfDiscussionAC> subPointListAc = new List<SubPointOfDiscussionAC>();
        List<MainDiscussionPointAC> mainPointListAc = new List<MainDiscussionPointAC>();
        List<MomUserMappingAC> momUserMappingListAc = new List<MomUserMappingAC>();
        List<SubPointOfDiscussion> subPointList = new List<SubPointOfDiscussion>();
        List<MainDiscussionPoint> mainPointList = new List<MainDiscussionPoint>();
        List<MomUserMapping> momUserMappingList = new List<MomUserMapping>();
        List<MomAC> momListAc = new List<MomAC>();
        List<Mom> momList = new List<Mom>();
        User user1 = new User();
        UserAC UserAC = new UserAC();
        PlanProcessMapping planProcessMapping = new PlanProcessMapping();
        List<PlanProcessMapping> planProcessMappingList = new List<PlanProcessMapping>();
        WorkProgram workProgram = new WorkProgram();
        List<WorkProgram> workProgramList = new List<WorkProgram>();
        List<EntityUserMapping> entityUserMappingList = new List<EntityUserMapping>();
        EntityUserMapping entityUserMapping = new EntityUserMapping();
        List<AuditableEntity> entityList = new List<AuditableEntity>();
        AuditableEntity auditableEntity = new AuditableEntity();
        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;
        #endregion

        #region Constructor
        public MomRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {

            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _viewRenderService = bootstrap.ServiceProvider.GetService<Mock<IViewRenderService>>();
            _dataRepository.Reset();
            _momDataRepository = bootstrap.ServiceProvider.GetService<IMomRepository>();
            _userDataRepository = bootstrap.ServiceProvider.GetService<IUserRepository>();
            _workProgramDataRepository = bootstrap.ServiceProvider.GetService<IWorkProgramRepository>();
             _audiableEntityDataRepository = bootstrap.ServiceProvider.GetService<IAuditableEntityRepository>();
            _exportToExcelRepository = bootstrap.ServiceProvider.GetService<IExportToExcelRepository>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();


            _iConfig = bootstrap.ServiceProvider.GetService <Mock<IConfiguration>>();
            momDemoObj.Id = momId;
            momDemoObj.WorkProgramId = workProgramId;
            momDemoObj.Agenda = "DemoMom";
            momDemoObj.MomDate = DateTime.UtcNow;
            momDemoObj.MomStartTime = DateTime.UtcNow.ToLocalTime();
            momDemoObj.MomEndTime = DateTime.UtcNow.ToLocalTime();
            momDemoObj.IsDeleted = false;
            momDemoObj.CreatedBy = userId;
            momDemoObj.InternalUserList = momUserMappingListAc.Where(x => x.User.UserType == UserType.Internal).ToList();
            momDemoObj.ExternalUserList = momUserMappingListAc.Where(x => x.User.UserType == UserType.External).ToList();

            momDemoObjException.Id = Guid.Empty;
            momDemoObjException.WorkProgramId = Guid.Empty;
            momDemoObjException.Agenda = "DemoMom";
            momDemoObjException.MomDate = DateTime.UtcNow;
            momDemoObjException.MomStartTime = DateTime.UtcNow.ToLocalTime();
            momDemoObjException.MomEndTime = DateTime.UtcNow.ToLocalTime();
            momDemoObjException.IsDeleted = false;
            momDemoObjException.CreatedBy = userId;

            user1.Id = Guid.NewGuid();
            user1.Name = "Niriksha Soni";
            user1.EmailId = "niriksha@promactinfo.com";
            user1.IsDeleted = false;
            user1.UserType = UserType.Internal;
            user1.CreatedBy = userId;
            user1.CreatedDateTime = DateTime.UtcNow;
            userList.Add(user1);

            User user2 = new User();
            user2.Id = Guid.NewGuid();
            user2.Name = "Moksha Soni";
            user2.EmailId = "moksha@promactinfo.com";
            user2.IsDeleted = false;
            user2.UserType = UserType.External;
            user2.CreatedBy = userId;
            user2.CreatedDateTime = DateTime.UtcNow;
            userList.Add(user2);

            UserAC user3 = new UserAC();
            user3.Id = Guid.Empty;
            user3.Name = "Moksha Soni";
            user3.EmailId = "moksha@promactinfo.com";
            user3.IsDeleted = false;
            user3.UserType = UserType.External;
            user3.CreatedBy = Guid.Empty;
            user3.CreatedDateTime = DateTime.UtcNow;
            user3.UserType = UserType.External;
            userListForException.Add(user3);

            UserAC user4 = new UserAC();
            user4.Id = Guid.NewGuid();
            user4.Name = "ankita Soni";
            user4.EmailId = "ankita@promactinfo.com";
            user4.IsDeleted = false;
            user4.UserType = UserType.Internal;
            user4.CreatedBy = userId;
            user4.CreatedDateTime = DateTime.UtcNow;
            user4.UserType = UserType.Internal;
            userListAc.Add(user4);

            UserAC.Id = Guid.Empty;
            UserAC.Name = "mehul Soni";
            UserAC.EmailId = "mehul@promactinfo.com";
            UserAC.IsDeleted = false;
            UserAC.UserType = UserType.External;
            UserAC.CreatedBy = Guid.Empty;
            UserAC.CreatedDateTime = DateTime.UtcNow;
            userListForException.Add(UserAC);

            SubPointOfDiscussionAC subPointAc = new SubPointOfDiscussionAC();
            subPointAc.Id = subPointId;
            subPointAc.SubPoint = "TestingSubpoint";
            subPointAc.Status = SubPointStatus.InProgress;
            subPointAc.TargetDate = DateTime.UtcNow;

            subPointAc.IsDeleted = false;
            subPointAc.CreatedBy = userId;
            subPointAc.CreatedDateTime = DateTime.UtcNow;
            subPointAc.PersonResponsibleACCollection = momUserMappingListAc;

            MainDiscussionPointAC mainDiscussionPointAc = new MainDiscussionPointAC();
            mainDiscussionPointAc.Id = mainPointId;
            mainDiscussionPointAc.IsDeleted = false;
            mainDiscussionPointAc.MomId = momId;
            mainDiscussionPointAc.SubPointDiscussionACCollection = subPointListAc;
            mainDiscussionPointAc.CreatedBy = userId;
            mainDiscussionPointAc.CreatedDateTime = DateTime.UtcNow;
            mainDiscussionPointAc.MainPoint = "MainPointTesting";

            mainPointListAc.Add(mainDiscussionPointAc);
          
            subPointListAc.Add(subPointAc);

            MomUserMappingAC momUserMappingAc = new MomUserMappingAC();
            momUserMappingAc.Id = momUserMappingId;
            momUserMappingAc.IsDeleted = false;
            momUserMappingAc.MomId = momId;
            momUserMappingAc.CreatedBy = userId;
            momUserMappingAc.CreatedDateTime = DateTime.UtcNow;
            momUserMappingAc.SubPointOfDiscussionId = subPointAc.Id;
            momUserMappingAc.User = user4;
            momUserMappingAc.User.UserType = UserType.Internal;

            MomUserMappingAC momUserMappingAcExternal = new MomUserMappingAC();
            momUserMappingAcExternal.Id = momUserMappingIdExternal;
            momUserMappingAcExternal.IsDeleted = false;
            momUserMappingAcExternal.MomId = momId;
            momUserMappingAcExternal.CreatedBy = userId;
            momUserMappingAcExternal.CreatedDateTime = DateTime.UtcNow;
            momUserMappingAcExternal.SubPointOfDiscussionId = subPointAc.Id;
            momUserMappingAcExternal.User = user3;
            momUserMappingAcExternal.User.UserType = UserType.External;

            SubPointOfDiscussion subPoint = new SubPointOfDiscussion();
            subPoint.Id = subPointId;
            subPoint.SubPoint = "TestingSubpoint";
            subPoint.Status = SubPointStatus.InProgress;
            subPoint.TargetDate = DateTime.UtcNow;
            subPoint.IsDeleted = false;
            subPoint.CreatedBy = userId;
            subPoint.PersonResponsibleCollection = momUserMappingList;

            MainDiscussionPoint mainDiscussionPoint = new MainDiscussionPoint();
            mainDiscussionPoint.Id = mainPointId;
            mainDiscussionPoint.IsDeleted = false;
            mainDiscussionPoint.MomId = momId;
            mainDiscussionPoint.SubPointDiscussionCollection = subPointList;
            mainDiscussionPoint.CreatedBy = userId;
            mainDiscussionPoint.CreatedDateTime = DateTime.UtcNow;
            mainDiscussionPoint.MainPoint = "MainPointTesting";
            mainDiscussionPoint.Mom = momObj;
            mainPointList.Add(mainDiscussionPoint);

            subPoint.MainDiscussionPoint = mainDiscussionPoint;
            subPoint.MainDiscussionPoint.Id = mainPointId;
            subPointList.Add(subPoint);

            MomUserMapping momUserMapping = new MomUserMapping();
            momUserMapping.Id = momUserMappingId;
            momUserMapping.IsDeleted = false;
            momUserMapping.MomId = momId;
            momUserMapping.CreatedBy = userId;
            momUserMapping.CreatedDateTime = DateTime.UtcNow;
            momUserMapping.SubPointOfDiscussionId = subPoint.Id;
            momUserMapping.SubPointOfDiscussion = subPoint;

            workProgram.Id = workProgramId;
            workProgram.IsDeleted = false;
            workProgram.Name = "workprogram-1";
            workProgram.Scope = "scope1";
            workProgram.Status = WorkProgramStatus.Active;
            workProgram.CreatedBy = userId;
            workProgram.CreatedDateTime = DateTime.UtcNow;
            workProgram.DraftIssues = "draft-issues";
            workProgramList.Add(workProgram);

            momObj.Id = momId;
            momObj.WorkProgramId = workProgramId;
            momObj.Agenda = "DemoMom";
            momObj.MomDate = DateTime.UtcNow;
            momObj.MomStartTime = DateTime.UtcNow.ToLocalTime();
            momObj.MomEndTime = DateTime.UtcNow.ToLocalTime();
            momObj.IsDeleted = false;
            momObj.CreatedBy = userId;
            momObj.MainDiscussionPointCollection = mainPointList;
            momObj.SubPointDiscussionCollection = subPointList;
            momObj.WorkProgram = workProgram;
            momObj.EntityId = auditableEntityId;
            momUserMappingListAc.Add(momUserMappingAc);
            momUserMappingListAc.Add(momUserMappingAcExternal);

            momUserMapping.Mom = momObj;
            momUserMapping.User = user1;
            momUserMappingList.Add(momUserMapping);
            momObj.MomUserMappingCollection = momUserMappingList;
            
            momList.Add(momObj);

            momDemoObj.MainDiscussionPointACCollection = mainPointListAc;
            momDemoObj.SubPointDiscussionACCollection = subPointListAc;
            momDemoObj.TeamCollection = userListForException;
            momDemoObj.ClientParticipantCollection = userListForException;
            momDemoObj.EntityId = auditableEntityId;
            momListAc.Add(momDemoObj);



          

            AuditPlan auditPlan = new AuditPlan();
            auditPlan.EntityId = auditableEntityId;
            auditPlan.Title = "auditPlan";

            planProcessMapping.Id = planProcessId;
            planProcessMapping.IsDeleted = false;
            planProcessMapping.CreatedBy = userId;
            planProcessMapping.CreatedDateTime = DateTime.UtcNow;
            planProcessMapping.Status = PlanProcessStatus.InProgress;
            planProcessMapping.PlanId = new Guid("a93c9ac5-be95-4f88-8bcf-d9906248cb44");
            planProcessMapping.ProcessId = new Guid("9052a0d6-be0f-475d-b071-0967708335f3");
            planProcessMapping.WorkProgram = workProgram;
            planProcessMapping.WorkProgramId = workProgram.Id;
            planProcessMapping.AuditPlan = auditPlan;
            planProcessMappingList.Add(planProcessMapping);

            entityUserMapping.User = user2;
            entityUserMappingList.Add(entityUserMapping);

            auditableEntity.Id = auditableEntityId;
            auditableEntity.IsDeleted = false;
            auditableEntity.Name = "auditableEntity-1";
            entityList.Add(auditableEntity);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set Mom testing data
        /// </summary>
        /// <returns>List of mom data</returns>
        private List<Mom> SetMomInitData()
        {
            for(int i=0;i< mainPointList.Count; i++)
            {
                for(int j = 0; j < mainPointList[i].SubPointDiscussionCollection.Count; j++)
                {
                    mainPointList.ToList()[i].SubPointDiscussionCollection.ToList()[j].PersonResponsibleCollection= momUserMappingList;
                }
            }
            var momObj = new List<Mom>()
            {
                new Mom()
                {
                    Id=momId,
                    WorkProgramId=workProgramId,
                    MomDate=DateTime.UtcNow,
                   Agenda="test",
                   ClosureMeetingDate=DateTime.UtcNow,
                   MomStartTime=DateTime.UtcNow.ToLocalTime(),
                   MomEndTime=DateTime.UtcNow.ToLocalTime(),
                    IsDeleted=false,
                    CreatedBy=userId,
                    MainDiscussionPointCollection=mainPointList,
                    SubPointDiscussionCollection=subPointList,
                    MomUserMappingCollection=momUserMappingList,
                    EntityId=auditableEntityId
                    
                }
            };
            return momObj;
        }

        /// <summary>
        /// Set MomAC testing data
        /// </summary>
        /// <returns>List of momAc data</returns>
        private MomAC SetMomACInitData()
        {
            for (int i = 0; i < mainPointListAc.Count; i++)
            {
                for (int j = 0; j < mainPointListAc[i].SubPointDiscussionACCollection.Count; j++)
                {
                    mainPointListAc.ToList()[i].SubPointDiscussionACCollection.ToList()[j].PersonResponsibleACCollection = momUserMappingListAc;
                }
            }
            var momDemoObj = new MomAC()
            {
               
                    Id=momId,
                    WorkProgramId=workProgramId,
                    MomDate=DateTime.UtcNow,
                   Agenda="test",
                   ClosureMeetingDate=DateTime.UtcNow,
                   MomStartTime=DateTime.UtcNow.ToLocalTime(),
                   MomEndTime=DateTime.UtcNow.ToLocalTime(),
                    IsDeleted=false,
                    CreatedBy=userId,
                    EntityId=auditableEntityId,
                    MainDiscussionPointACCollection=mainPointListAc,
                    SubPointDiscussionACCollection=subPointListAc,
                    PersonResposibleACCollection=momUserMappingListAc,
                    TeamCollection= userListForException,
                    ClientParticipantCollection= userListForException,
                InternalUserList = momUserMappingListAc.Where(x => x.User.UserType == UserType.Internal).ToList(),
          ExternalUserList = momUserMappingListAc.Where(x => x.User.UserType == UserType.External).ToList()

        };
            return momDemoObj;
        }

        /// <summary>
        /// Set MomAC SubPointDiscussion data
        /// </summary>
        /// <returns>List of SubPointDiscussion data</returns>
        private List<SubPointOfDiscussion> SubPointDiscussionList()
        {
            var subPointList = new List<SubPointOfDiscussion>()
            {
                new SubPointOfDiscussion()
                {
                    Id=subPointId,
                  SubPoint="TestingSubpoint",
                  Status=SubPointStatus.InProgress,
                  TargetDate=DateTime.UtcNow,
                  MainPointId=mainPointId,
                   IsDeleted=false,
                    CreatedBy=userId,
                    PersonResponsibleCollection=momUserMappingList
                }
            };
            return subPointList;
        }

        /// <summary>
        /// Set MainPointDiscussion data
        /// </summary>
        /// <returns>List of MainPointDiscussion data</returns>
        private List<MainDiscussionPoint> MainPointDiscussionList()
        {
            var mainPointList = new List<MainDiscussionPoint>()
            {
                new MainDiscussionPoint()
                {
                    Id=mainPointId,
                    MomId=momId,
                    MainPoint="TestingMainPoint",
                     IsDeleted=false,
                    CreatedBy=userId,
                    SubPointDiscussionCollection=subPointList
                }
            };
            return mainPointList;
        }

        /// <summary>
        /// Set MomUserMapping data
        /// </summary>
        /// <returns>List of MomUserMapping data</returns>
        private List<MomUserMapping> MomUserMappingList()
        {
            var momMappingList = new List<MomUserMapping>()
            {
                new MomUserMapping()
                {
                    Id=momUserMappingId,
                 UserId=userId,
                 MomId=momId,
                   IsDeleted=false,
                    CreatedBy=userId
                }
            };
            return momMappingList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Method for getting all mom
        /// </summary>
        /// <returns> List of mom</returns>
        [Fact]
        private async Task GetAllMom_ReturnAllMom()
        {
            List<Mom> getInitData = SetMomInitData();
            List<MainDiscussionPoint> mainDiscussionPointList = MainPointDiscussionList();
            List<SubPointOfDiscussion> subPointOfDiscussionList = SubPointDiscussionList();
            List<MomUserMapping> momUserMappingList = MomUserMappingList();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                    .Returns(getInitData.Where(x => !x.IsDeleted && x.CreatedBy == userId && x.WorkProgramId == workProgramId && x.EntityId==auditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MainDiscussionPoint, bool>>>()))
                .Returns(mainDiscussionPointList.Where(x => !x.IsDeleted && x.UserCreatedBy.Id == userId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<SubPointOfDiscussion, bool>>>()))
               .Returns(subPointOfDiscussionList.Where(x => !x.IsDeleted && x.UserCreatedBy.Id == userId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMapping, bool>>>()))
              .Returns(momUserMappingList.Where(x => !x.IsDeleted && x.UserCreatedBy.Id == userId).AsQueryable().BuildMock().Object);
            var result = await _momDataRepository.GetMomAsync(page, pageSize, searchString, auditableEntityId.ToString());
            Assert.NotNull(result);
            Assert.Equal(getInitData.Count, result.Count);
        }
        /// <summary>
        /// Test case to verify GetMomAsync search string
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomAsync_SearchString()
        {
            List<Mom> momMappingObj = SetMomInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momMappingObj.Where(x => !x.IsDeleted && x.Agenda.ToLower() == "test" && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            var result = await _momDataRepository.GetMomAsync(page, pageSize, "test",auditableEntityId.ToString());
            Assert.Equal(momMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetMomAsync search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomAsync_SearchMomAC()
        {
            List<Mom> momMappingObj = SetMomInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momMappingObj.Where(x => !x.IsDeleted && x.Agenda == "test" && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            List<MomAC> result = await _momDataRepository.GetMomAsync(page, pageSize, "test", auditableEntityId.ToString());
            Assert.Equal(momMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetMomAsync for invalid search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomAsync_InvalidSearchRating()
        {
            List<Mom> momMappingObj = SetMomInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momList.Where(x => !x.IsDeleted && x.Agenda == "mom" && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            List<MomAC> result = await _momDataRepository.GetMomAsync(page, pageSize, "mom", auditableEntityId.ToString());
            Assert.NotEqual(momMappingObj.Count, result.Count);

        }

        /// <summary>
        /// Test case to verify GetMomCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomCountAsync_ReturnCount()
        {
            List<Mom> momMappingObj = SetMomInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                .Returns(momList.Where(x => !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            int result = await _momDataRepository.GetMomCountAsync(searchString, auditableEntityId.ToString());
            Assert.Equal(momMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetRatingCountAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomCountAsync_SearchReturnCount()
        {
            List<Mom> ratingMappingObj = SetMomInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                .Returns(ratingMappingObj.Where(x => !x.IsDeleted && x.Agenda == "test" && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            int result = await _momDataRepository.GetMomCountAsync("test", auditableEntityId.ToString());
            Assert.Equal(ratingMappingObj.Count, result);

        }
        /// <summary>
        /// Test case to verify GetMomCountAsync with no data
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetMomCountAsync_NoReturnCount()
        {
            List<Mom> momMappingObj = SetMomInitData();
            momMappingObj[0].IsDeleted = true;
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                .Returns(momMappingObj.Where(x => !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            int result = await _momDataRepository.GetMomCountAsync(searchString, auditableEntityId.ToString());
            Assert.NotEqual(momMappingObj.Count, result);
        }

        /// <summary>
        /// Method for getting data by id
        /// </summary>
        /// <returns>Data of mom of particular id</returns>
        [Fact]
        private async Task GetMomDetailByIdAsync_ReturnValid()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            var momAC = SetMomACInitData();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momList.Where(x => x.Id == momId && !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(Task.FromResult(momList.FirstOrDefault(x => x.Id == momId && !x.IsDeleted && x.EntityId == auditableEntityId)));
           
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MainDiscussionPoint, bool>>>())).Returns(mainPointList.Where(x => x.Id == mainPointId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<SubPointOfDiscussion, bool>>>())).Returns(subPointList.Where(x => x.MainPointId == mainPointId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMapping, bool>>>())).Returns(momUserMappingList.Where(x => x.MomId == momId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>())).Returns(entityUserMappingList.Where(x => !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>())).Returns(planProcessMappingList.Where(x => !x.IsDeleted && !x.AuditPlan.IsDeleted && x.AuditPlan.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                      .Returns(Task.FromResult(entityList.First(x => x.Id == auditableEntityId)));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMappingAC, bool>>>())).Returns(momAC.PersonResposibleACCollection.AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momAC.TeamCollection.Where(x => x.Id == user1.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momAC.ClientParticipantCollection.Where(x => x.Id == user1.Id).AsQueryable().BuildMock().Object);
            var result=await _momDataRepository.GetMomDetailByIdAsync(momId.ToString(), auditableEntityId.ToString());
            Assert.NotNull(result);
            
        }

        /// <summary>
        /// Method for getting mom data by null id
        /// </summary>
        /// <returns>Exception</returns>
        [Fact]
        private async Task GetMomDetailByIdAsync_ReturnInValid()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momList.Where(x => x.Id == null && !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(Task.FromResult(momList.FirstOrDefault(x => x.Id == null && !x.IsDeleted && x.EntityId == auditableEntityId)));
         
            await Assert.ThrowsAsync<System.Reflection.TargetInvocationException>(async () =>
            await _momDataRepository.GetMomDetailByIdAsync(null, auditableEntityId.ToString()));
            

        }

        /// <summary>
        /// Method for adding mom detail
        /// </summary>
        /// <returns>Added mom object</returns>
        [Fact]
        private async Task AddMomAsync_Return_MomDetail()
        {
            var momAC = SetMomACInitData() ;
          
               var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Mom>())).ReturnsAsync(momObj);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user1);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func < MainDiscussionPoint ,bool>>>())).Returns(momObj.MainDiscussionPointCollection.Where(x=>x.MomId==momObj.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<SubPointOfDiscussion, bool>>>())).Returns(momObj.SubPointDiscussionCollection.AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMapping, bool>>>())).Returns(momObj.MomUserMappingCollection.AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MainDiscussionPointAC, bool>>>())).Returns(momAC.MainDiscussionPointACCollection.Where(x => x.MomId == momObj.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<SubPointOfDiscussionAC, bool>>>())).Returns(momAC.SubPointDiscussionACCollection.AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMappingAC, bool>>>())).Returns(momAC.PersonResposibleACCollection.AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momAC.TeamCollection.Where(x => x.Id == user1.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momAC.ClientParticipantCollection.Where(x => x.Id == user1.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<AuditableEntity>(It.IsAny<Expression<Func<AuditableEntity, bool>>>())).Returns(entityList.Where(x=>!x.IsDeleted).AsQueryable().BuildMock().Object);
            var result = await _momDataRepository.AddMomAsync(momAC);
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<Mom>()), Times.AtLeastOnce);
            _dataRepository.Verify(x => x.AddRangeAsync(It.IsAny<List<MomUserMapping>>()), Times.AtLeastOnce);

        }

        /// <summary>
        /// Method for updating mom detail
        /// </summary>
        /// <returns>Updated detail of mom</returns>
        [Fact]
        private async Task UpdateMomAsync_Return_UpdatedMomDetail()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Update(It.IsAny<Mom>()))
                       .Returns((Mom model) => (EntityEntry<Mom>)null);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(userList.First(y => !y.IsDeleted)));
            _dataRepository.Setup(x => x.Where<MainDiscussionPoint>(It.IsAny<Expression<Func<MainDiscussionPoint, bool>>>())).Returns(momObj.MainDiscussionPointCollection.Where(x=>!x.IsDeleted && x.MomId==momObj.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<SubPointOfDiscussion>(It.IsAny<Expression<Func<SubPointOfDiscussion, bool>>>())).Returns(momObj.SubPointDiscussionCollection.Where(x=>x.MainPointId==mainPointId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<MomUserMapping>(It.IsAny<Expression<Func<MomUserMapping, bool>>>())).Returns(momObj.MomUserMappingCollection.Where(m => m.MomId == momId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momDemoObj.TeamCollection.Where(t => t.Id == momId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where<UserAC>(It.IsAny<Expression<Func<UserAC, bool>>>())).Returns(momDemoObj.ClientParticipantCollection.Where(t => t.Id == momId).AsQueryable().BuildMock().Object);
             _dataRepository.Setup(x => x.Where<AuditableEntity>(It.IsAny<Expression<Func<AuditableEntity, bool>>>())).Returns(entityList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            await _momDataRepository.UpdateMomDetailAsync(momDemoObj);
            _dataRepository.Verify(x => x.Update(It.IsAny<Mom>()), Times.Once);
            _dataRepository.Verify(x => x.UpdateRange(It.IsAny<List<MomUserMapping>>()), Times.AtLeastOnce);
            _dataRepository.Verify(x => x.AddRangeAsync(It.IsAny<List<MomUserMapping>>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Method for updating mom detail
        /// </summary>
        /// <returns>Returns Exception</returns>
        [Fact]
        private async Task UpdateMomAsync_Return_exception()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Update(It.IsAny<Mom>()))
                .Throws(new NullReferenceException());
            _dataRepository.Setup(x => x.Where<AuditableEntity>(It.IsAny<Expression<Func<AuditableEntity, bool>>>())).Returns(entityList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                      .Returns(Task.FromResult(userList.FirstOrDefault(y => !y.IsDeleted)));

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
               await _momDataRepository.UpdateMomDetailAsync(momDemoObjException));
        }

        /// <summary>
        /// Method for deleting mom
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        private async Task DeleteMomAsync_ReturnValid()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(momList.Where(x => x.Id == momId && !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Mom, bool>>>()))
                     .Returns(Task.FromResult(momList.FirstOrDefault(x => x.Id == momId && !x.IsDeleted)));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>()))
                     .Returns(Task.FromResult(userList.First(x => !x.IsDeleted)));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MainDiscussionPoint, bool>>>())).Returns(mainPointList.Where(x => x.Id == mainPointId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<SubPointOfDiscussion, bool>>>())).Returns(subPointList.Where(x => x.MainPointId == mainPointId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<MomUserMapping, bool>>>())).Returns(momUserMappingList.Where(x => x.MomId == momId && !x.IsDeleted).AsQueryable().BuildMock().Object);
            
            await _momDataRepository.DeleteMomAsync(momId);
            _dataRepository.Verify(mock => mock.UpdateRange(It.IsAny<List<MainDiscussionPoint>>()), Times.AtLeastOnce);
            _dataRepository.Verify(mock => mock.UpdateRange(It.IsAny<List<SubPointOfDiscussion>>()), Times.AtLeastOnce);
            _dataRepository.Verify(mock => mock.UpdateRange(It.IsAny<List<MomUserMapping>>()), Times.AtLeastOnce);
            _dataRepository.Verify(mock => mock.SaveChangesAsync(), Times.AtLeastOnce);
        }

        /// <summary>
        ///  Method for deleting mom
        /// </summary>
        /// <returns>Returns Exception</returns>
        [Fact]
        private async Task DeleteMomAsync_ReturnInValid()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Mom, bool>>>())).Returns(momList.Where(y => !y.IsDeleted && y.Id == Guid.Empty).AsQueryable().BuildMock().Object);
            
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Mom, bool>>>())).Returns(Task.FromResult(momList.FirstOrDefault(y => !y.IsDeleted && y.Id== Guid.Empty)));
            
            _dataRepository.Setup(x => x.UpdateRange(It.IsAny<List<MainDiscussionPoint>>()))
               .Throws(new NullReferenceException());
            _dataRepository.Setup(x => x.UpdateRange(It.IsAny<List<SubPointOfDiscussion>>()))
              .Throws(new NullReferenceException());
            _dataRepository.Setup(x => x.UpdateRange(It.IsAny<List<MomUserMapping>>()))
             .Throws(new NullReferenceException());
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                   await _momDataRepository.DeleteMomAsync(Guid.Empty));

        }


        /// <summary>
        /// Method for getting predefined data for mom
        /// </summary>
        /// <returns>List of users and workprogram</returns>
        [Fact]
        private async Task TaskGetPredefinedDataForMomAsync__Return_ValidData()
        {
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>())).Returns(entityUserMappingList.Where(x => !x.IsDeleted && x.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<PlanProcessMapping, bool>>>())).Returns(planProcessMappingList.Where(x => !x.IsDeleted && !x.AuditPlan.IsDeleted && x.AuditPlan.EntityId == auditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                      .Returns(Task.FromResult(entityList.First(x => x.Id == auditableEntityId)));
            var result = await _momDataRepository.GetPredefinedDataForMomAsync(auditableEntityId);
            Assert.NotNull(result);
        }
        #endregion
    }
}
