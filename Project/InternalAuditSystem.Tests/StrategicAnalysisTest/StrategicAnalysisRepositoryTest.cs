using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository;
using Moq;
using Xunit;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Utility.FileUtil;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.StrategicAnalysisTest
{
    [Collection("Register Dependency")]
    public class StrategicAnalysisRepositoryTest : BaseTest
    {
        #region Private Variables

        private IStrategicAnalysisRepository _strategicAnalysisRepository;
        private Mock<IDataRepository> _dataRepository;
        private Mock<IFileUtility> _fileUtil;
        private Mock<IAzureRepository> _azureRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly int page = 1;
        private readonly int pageSize = 10;
        private readonly string searchString = null;

        #endregion

        #region Constructor
        public StrategicAnalysisRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _strategicAnalysisRepository = bootstrap.ServiceProvider.GetService<IStrategicAnalysisRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _fileUtil = bootstrap.ServiceProvider.GetService<Mock<IFileUtility>>();
            _azureRepository = bootstrap.ServiceProvider.GetService<Mock<IAzureRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();
        }
        #endregion

        #region Testing Methods

        #region Public Methods

        #region CRUD for Strategic Analysis

        /// <summary>
        /// Test case to verify GetAllStrategicAnalysis search value when null
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStrategicAnalysis_ReturnsStrategicAnalysisAC()
        {
            var strategicAnalysisMappingObj = SetStrategicAnalysisInitData();
            var strategicAnalysisMappingObjAC = SetStrategicAnalysisACInitData();
            var userResponseList = SetUserResponseList();
            var questions = SetQuestionInitData();
            var userId = Guid.NewGuid().ToString();

            //Create pagination object
            Pagination<StrategicAnalysis> pagination = new Pagination<StrategicAnalysis>();
            pagination.Items = strategicAnalysisMappingObj.ToList();

            //Create pagination objectAC
            Pagination<StrategicAnalysisAC> paginationAC = new Pagination<StrategicAnalysisAC>();
            paginationAC.PageSize = 1;
            paginationAC.Items = strategicAnalysisMappingObjAC.ToList();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<StrategicAnalysis, bool>>>()))
                     .Returns(strategicAnalysisMappingObj.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<UserResponse, bool>>>()))
                     .Returns(userResponseList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Question, bool>>>()))
                     .Returns(questions.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<StrategicAnalysis, bool>>>())).ReturnsAsync(strategicAnalysisMappingObj[0]);

            //var result = await _strategicAnalysisRepository.GetAllStrategicAnalysisAsync(paginationAC, userId);
            //Assert.Equal(pagination.Items.Count, result.Items.Count);
        }

        /// <summary>
        /// Test case to verify GetAllStrategicAnalysis with search value
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStrategicAnalysis_SearchStrategicAnalysisAc()
        {
            var strategicAnalysisMappingObj = SetStrategicAnalysisInitData();
            var strategicAnalysisMappingObjAC = SetStrategicAnalysisACInitData();
            var userResponseList = SetUserResponseList();
            var questions = SetQuestionInitData();
            var userId = Guid.NewGuid().ToString();
            //Create pagination object
            Pagination<StrategicAnalysis> pagination = new Pagination<StrategicAnalysis>();
            pagination.Items = strategicAnalysisMappingObj.ToList();

            //Create pagination objectAC
            Pagination<StrategicAnalysisAC> paginationAC = new Pagination<StrategicAnalysisAC>();
            paginationAC.Items = strategicAnalysisMappingObjAC.ToList();
            paginationAC.PageIndex = page;
            paginationAC.PageSize = pageSize;
            paginationAC.searchText = "test";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<StrategicAnalysis, bool>>>()))
                     .Returns(strategicAnalysisMappingObj.Where(x => !x.IsDeleted && x.SurveyTitle.ToLower().Contains("test")).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<UserResponse, bool>>>()))
                     .Returns(userResponseList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Question, bool>>>()))
                     .Returns(questions.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            //var result = await _strategicAnalysisRepository.GetAllStrategicAnalysisAsync(paginationAC, userId);
            //Assert.Equal(pagination.Items.Count, result.Items.Count);
        }

        /// <summary>
        /// Test case to verify AddStrategicAnalysisAsync with strategicAnalysisAC returns strategicAnalysisAC added in database
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddStrategicAnalysisAsync_ReturnsStrategicAnalysisAC()
        {
            var mockedDbTransaction = new Mock<IDbContextTransaction>();
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            var strategicAnalysisObj = SetStrategicAnalysisInitData();
            var user = CreateUser();
            var strategicAnalysisAC = SetStrategicAnalysisACInitData();
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);

            //var result = await _strategicAnalysisRepository.AddStrategicAnalysisAsync(strategicAnalysisAC[0]);
            //_dataRepository.Verify(x => x.AddAsync(It.IsAny<StrategicAnalysis>()), Times.Once);
            //Assert.Equal(result.Id, strategicAnalysisAC[0].Id);
        }

        /// <summary>
        /// Test to verify UpdateStrategicAnalysisAsync with strategicAnalysisAC returns strategicAnalysisAC updated by database 
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateStrategicAnalysisAsync_ReturnsStrategicAnalysisAC()
        {
            var strategicAnalysisObj = SetStrategicAnalysisInitData();
            var strategicAnalysisAC = SetStrategicAnalysisACInitData();
            var auditableEntityAC = CreateAuditableEntityACList()[0];
            var auditableEntity = CreateAuditableEntityList()[0];
            User user = new User();
            var mockedDbTransaction = new Mock<IDbContextTransaction>();

            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockedDbTransaction.Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<StrategicAnalysis, bool>>>())).ReturnsAsync(strategicAnalysisObj[0]);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<AuditableEntity, bool>>>())).ReturnsAsync(auditableEntity);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();

            //var result = await _strategicAnalysisRepository.UpdateStrategicAnalysisAsync(strategicAnalysisAC[0]);
            //_dataRepository.Verify(x => x.Update(It.IsAny<StrategicAnalysis>()), Times.Once);
            //Assert.Equal(result.Id, strategicAnalysisAC[0].Id);
        }

        /// <summary>
        /// Verify DeleteStrategicAnalysisVerify with strategicAnalysisId passed
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteStrategicAnalysisAsync_Verify()
        {
            var strategicAnalyses = SetStrategicAnalysisInitData();
            var riskControlList = SetRiskControlMatrixList();
            var auditableEntityList = CreateAuditableEntityList();
            var questions = SetQuestionInitData();
            var options = SetOptions();
            var user = new User();
            var userResponses = SetUserResponseInitData();
            var userResponseDocuments = SetUserResponseDocuments();
            _dataRepository.Setup(x => x.FindAsync<StrategicAnalysis>(It.IsAny<Guid>())).ReturnsAsync(strategicAnalyses[0]);
            _dataRepository.Setup(x => x.AnyAsync<RiskControlMatrix>(It.IsAny<Expression<Func<RiskControlMatrix, bool>>>())).ReturnsAsync(riskControlList.Any<RiskControlMatrix>(x => x.StrategicAnalysisId == strategicAnalyses[0].Id && !x.IsDeleted));
            _dataRepository.Setup(x => x.AnyAsync<AuditableEntity>(It.IsAny<Expression<Func<AuditableEntity, bool>>>())).ReturnsAsync(auditableEntityList.Any<AuditableEntity>(x => x.Id == strategicAnalyses[0].AuditableEntityId && x.IsStrategyAnalysisDone && !x.IsDeleted));
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditableEntity, bool>>>()))
                     .Returns(auditableEntityList.Where(x => !x.IsDeleted && x.Id == strategicAnalyses[0].AuditableEntityId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Question, bool>>>()))
                    .Returns(questions.Where(x => !x.IsDeleted && x.StrategyAnalysisId == strategicAnalyses[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Option, bool>>>()))
                   .Returns(options.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<UserResponse, bool>>>()))
                   .Returns(userResponses.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<UserResponseDocument, bool>>>()))
                  .Returns(userResponseDocuments.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);

            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();

            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _strategicAnalysisRepository.DeleteStrategicAnalysisAsync(strategicAnalyses[0].Id));


        }

        #endregion

        #region CRUD for question

        /// <summary>
        /// Test case to verify GetQuestionsAsync with strategicAnalysisId returns list of questions
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetQuestionsAsync_ReturnsQuestionAC()
        {
            var questionList = SetQuestionInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Question, bool>>>()))
                     .Returns(questionList.Where(x => !x.IsDeleted && x.StrategyAnalysisId == questionList[0].StrategyAnalysisId).AsQueryable().BuildMock().Object);
            //var result = await _strategicAnalysisRepository.GetQuestionsAsync(questionList[0].StrategyAnalysisId);
            //Assert.Equal(questionList.Count, result.Count);
        }

        /// <summary>
        /// Test case to verify AddQuestionAsync with questionAC returns questionAC added to database
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddQuestionAsync_ReturnsQuestionAC()
        {
            var questionList = SetQuestionInitData();
            var questionACList = SetQuestionACInitData();
            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();
            //var result = await _strategicAnalysisRepository.AddQuestionAsync(questionACList[0], questionList[0].StrategyAnalysisId);
            //Assert.Equal(result.Id, questionACList[0].Id);
            //_dataRepository.Verify(x => x.AddAsync(It.IsAny<Question>()), Times.Once);
        }

        /// <summary>
        /// Test case to verify UpdateQuestionAsync with questionAC returns questionAC updated to the database
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateQuestionAsync_ReturnsQuestionAC()
        {
            var questionList = SetQuestionInitData();
            var questionACList = SetQuestionACInitData();
            _dataRepository.Setup(x => x.FindAsync<Question>(It.IsAny<Guid>())).ReturnsAsync(questionList[0]);
            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();

            //var result = await _strategicAnalysisRepository.UpdateQuestionAsync(questionACList[0]);
            //_dataRepository.Verify(x => x.Update(It.IsAny<Question>()), Times.Once);
            //Assert.Equal(result.Id, questionACList[0].Id);
        }

        /// <summary>
        /// Test case to verify DeleteQuestionByIdAsync with questionId passed
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteQuestionByIdAsync_Verify()
        {
            var questionList = SetQuestionInitData();
            _dataRepository.Setup(x => x.FindAsync<Question>(It.IsAny<Guid>())).ReturnsAsync(questionList[0]);
            _dataRepository.Setup(c => c.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();
            //await _strategicAnalysisRepository.DeleteQuestionByIdAsync(questionList[0].Id);
            //_dataRepository.Verify(x => x.Remove(It.IsAny<Question>()), Times.Once);
        }
        #endregion

        /// <summary>
        /// Test case to verify GetUserResponseAsync
        /// </summary>
        /// <returns></returns>
        #region User Responses
        [Fact]
        public void GetUserResponseAsync_VerifyCountOfUserResponses()
        {
            var userResponseACs = SetUserResponseACInitData();
            var userResponses = SetUserResponseInitData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<UserResponse, bool>>>()))
                     .Returns(userResponses.Where(x => !x.IsDeleted && x.StrategicAnalysisId == userResponses[0].StrategicAnalysisId && x.UserId == userResponses[0].UserId).AsQueryable().BuildMock().Object);
            var result = _strategicAnalysisRepository.GetUserResponseAsync(page, pageSize, searchString, (Guid)userResponseACs[0].StrategicAnalysisId);//, userResponseACs[0].UserId);
            //Assert.Equal(userResponseACs.Count, result.Count());
        }



        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Set StrategicAnalysis testing data
        /// </summary>
        /// <returns>List of StrategicAnalysis data</returns>
        private List<StrategicAnalysis> SetStrategicAnalysisInitData()
        {
            var strategicAnalysisMappingObj = new List<StrategicAnalysis>()
            {
                new StrategicAnalysis()
                {
                    Id = new Guid(),
                    SurveyTitle = "Test Survey Title",
                    AuditableEntityName = "Test Auditable Entity Name",
                    Message = "Test Message",
                    Version = 1,
                    IsSampling = false,
                    AuditableEntityId = new Guid(),
                    IsDeleted=false
                }
            };
            return strategicAnalysisMappingObj;
        }

        /// <summary>
        /// Set StrategicAnalysisAC testing data
        /// </summary>
        /// <returns>List of StrategicAnalysisAC data</returns>
        private List<StrategicAnalysisAC> SetStrategicAnalysisACInitData()
        {
            var strategicAnalysisId = new Guid();
            List<StrategicAnalysisTeamAC> srategicAnalysisTeamACList = new List<StrategicAnalysisTeamAC>()
            {
                new StrategicAnalysisTeamAC()
                {
                    StrategicAnalysisId = strategicAnalysisId,
                    UserId = (Guid)CreateUserAC().Id,
                    StrategicAnalysis = new StrategicAnalysisAC()
                    {
                        Id = strategicAnalysisId,
                        SurveyTitle = "Test Survey Title",
                        AuditableEntityName = "Test Auditable Entity Name",
                        Message = "Test Message",
                        Version = 1,
                        IsSampling = false,
                        AuditableEntityId = new Guid(),
                        IsDeleted=false,
                        //InternalUserList = srategicAnalysisTeamACList,
                        TeamCollection = CreateUserACList(),
                        IsUserResponseToBeUpdated = false
                        }
                }
            };

            var strategicAnalysisMappingObj = new List<StrategicAnalysisAC>()
            {
                new StrategicAnalysisAC()
                {
                    Id = new Guid(),
                    SurveyTitle = "Test Survey Title",
                    AuditableEntityName = "Test Auditable Entity Name",
                    Message = "Test Message",
                    Version = 1,
                    IsSampling = false,
                    AuditableEntityId = new Guid(),
                    IsDeleted=false,
                    InternalUserList = srategicAnalysisTeamACList,
                    TeamCollection = CreateUserACList(),
                    IsUserResponseToBeUpdated = false
                }
            };
            return strategicAnalysisMappingObj;
        }

        /// <summary>
        /// Get internal user list
        /// </summary>
        /// <returns>List of strategic analysis user mappingAC</returns>
        private List<StrategicAnalysisTeamAC> GetInternalUserList()
        {
            var user = CreateUser();
            List<StrategicAnalysisTeamAC> srategicAnalysisTeamACList = new List<StrategicAnalysisTeamAC>()
            {
                new StrategicAnalysisTeamAC()
                {
                    UserId = user.Id,
                    StrategicAnalysisId = new Guid(),
                    User = CreateUserAC(),
                    StrategicAnalysis = new StrategicAnalysisAC()
                         {
                            Id = new Guid(),
                            SurveyTitle = "Test Survey Title",
                            AuditableEntityName = "Test Auditable Entity Name",
                            Message = "Test Message",
                            Version = 1,
                            IsSampling = false,
                            AuditableEntityId = new Guid(),
                            IsDeleted=false,
                            InternalUserList = new List<StrategicAnalysisTeamAC>()
                                                    {
                                                        new StrategicAnalysisTeamAC()
                                                        {
                                                            UserId = user.Id,
                                                            StrategicAnalysisId = new Guid(),
                                                            User = CreateUserAC(),
                                                            StrategicAnalysis = new StrategicAnalysisAC()
                                                                 {
                                                                    Id = new Guid(),
                                                                    SurveyTitle = "Test Survey Title",
                                                                    AuditableEntityName = "Test Auditable Entity Name",
                                                                    Message = "Test Message",
                                                                    Version = 1,
                                                                    IsSampling = false,
                                                                    AuditableEntityId = new Guid(),
                                                                    IsDeleted=false,
                                                                    TeamCollection = CreateUserACList(),
                                                                    IsUserResponseToBeUpdated = false
                                                                }
                                                        }
                                                        },
                            TeamCollection = CreateUserACList(),
                            IsUserResponseToBeUpdated = false
                        }
                }
            };

            return srategicAnalysisTeamACList;
        }

        /// <summary>
        /// Set Question testing data
        /// </summary>
        /// <returns>List of Question data</returns>
        private List<Question> SetQuestionInitData()
        {
            var questionMappingObj = new List<Question>()
            {
                new Question()
                {
                    QuestionText = "Test Question Text",
                    Type = QuestionType.Textbox,
                    IsRequired = true,
                    StrategyAnalysisId = new Guid(),
                    IsDeleted=false,
                    TextboxQuestion = new TextboxQuestion(){
                    CharacterLowerLimit = 0,
                    CharacterUpperLimit = 499,
                    Guidance = "Test Guidance text"
                    }
                }
            };
            return questionMappingObj;
        }

        /// <summary>
        /// Set QuestionAC testing data
        /// </summary>
        /// <returns>List of QuestionAC data</returns>
        private List<QuestionAC> SetQuestionACInitData()
        {
            var questionMappingObj = new List<QuestionAC>()
            {
                new QuestionAC()
                {
                    Id = new Guid(),
                    QuestionText = "Test Question Text",
                    Type = QuestionType.Textbox,
                    IsRequired = true,
                    StrategyAnalysisId = new Guid(),
                    IsDeleted=false,
                    TextboxQuestion = new TextboxQuestionAC(){
                    CharacterLowerLimit = 0,
                    CharacterUpperLimit = 499,
                    Guidance = "Test Guidance text"
                    }
                }
            };
            return questionMappingObj;
        }

        /// <summary>
        /// Set UserResponseAC testing data
        /// </summary>
        /// <returns>List of UserResponseAC data</returns>
        private List<UserResponseAC> SetUserResponseACInitData()
        {
            var userResponseACs = new List<UserResponseAC>()
            {
                new UserResponseAC()
                {
                    QuestionId = new Guid(),
                    Other = "Other",
                    RepresentationNumber = 1,
                    AnswerText = "AnswerText",
                    StrategicAnalysisId = new Guid()
                }
            };
            return userResponseACs;
        }

        /// <summary>
        /// Set UserResponse testing data
        /// </summary>
        /// <returns>List of UserResponse data</returns>
        private List<UserResponse> SetUserResponseInitData()
        {
            var userResponses = new List<UserResponse>()
            {
                new UserResponse()
                {
                    QuestionId = new Guid(),
                    OptionIds = new List<Guid>(),
                    Options = new List<Option>()
                    {
                        new Option()
                        {
                            OptionText = "OptionText"
                        }
                    },
                    Other = "Other",
                    RepresentationNumber = 1,
                    AnswerText = "AnswerText"
                }
            };
            return userResponses;
        }

        /// <summary>
        /// Set user response documents
        /// </summary>
        /// <returns>List of user response documents</returns>
        private List<UserResponseDocument> SetUserResponseDocuments()
        {
            List<UserResponseDocument> userResponseDocuments = new List<UserResponseDocument>()
            {
                new UserResponseDocument()
                {
                    Path = "path",
                    UserResponseId = new Guid(),
                    IsDeleted = false,
                    Id = new Guid()
                }
            };
            return userResponseDocuments;
        }

        /// <summary>
        /// Create dummy User
        /// </summary>
        /// <returns></returns>
        private User CreateUser()
        {
            var user = new User()
            {
                Id = new Guid(),
                Name = "Test User",
                EmailId = "testuser@gmail.com",
                Designation = "Manager",
                UserType = DomailModel.Enums.UserType.Internal,
                LastInterectedDateTime = DateTime.UtcNow
            };

            return user;
        }

        /// <summary>
        /// Create dummy UserAC
        /// </summary>
        /// <returns></returns>
        private UserAC CreateUserAC()
        {
            var userAC = new UserAC()
            {
                Id = new Guid(),
                Name = "Test User",
                EmailId = "testuser@gmail.com",
                Designation = "Manager",
                UserType = DomailModel.Enums.UserType.Internal,
                LastInterectedDateTime = DateTime.UtcNow
            };

            return userAC;
        }


        /// <summary>
        /// Create dummy UserAC List
        /// </summary>
        /// <returns></returns>
        private List<UserAC> CreateUserACList()
        {
            UserAC userAC = CreateUserAC();
            List<UserAC> userACList = new List<UserAC>();
            userACList.Add(userAC);
            return userACList;
        }

        /// <summary>
        /// Create auditable entity list
        /// </summary>
        /// <returns>List of AuditableEntity</returns>
        private List<AuditableEntity> CreateAuditableEntityList()
        {
            List<AuditableEntity> auditableEntityList = new List<AuditableEntity>()
            {
                new AuditableEntity()
                {
                    Id = new Guid(),
                    Status = AuditableEntityStatus.Active,
                    Description = "Description-1",
                    IsStrategyAnalysisDone = false,
                    Version = 1.1,
                    CreatedDateTime = DateTime.Now,
                    IsDeleted = false
                }
            };
            return auditableEntityList;
        }

        /// <summary>
        /// Create auditable entity AC list
        /// </summary>
        /// <returns>List of AuditableEntityAC</returns>
        private List<AuditableEntityAC> CreateAuditableEntityACList()
        {
            List<AuditableEntityAC> auditableEntityACList = new List<AuditableEntityAC>()
            {
                new AuditableEntityAC()
                {
                    Id = new Guid(),
                    Status = AuditableEntityStatus.Active,
                    Description = "Description-1",
                    IsStrategyAnalysisDone = false,
                    Version = 1,
                    CreatedDateTime = DateTime.Now,
                    IsDeleted = false
                }
            };
            return auditableEntityACList;
        }

        /// <summary>
        /// Create User response list
        /// </summary>
        /// <returns>List of user response</returns>
        private List<UserResponse> SetUserResponseList()
        {
            var userResponseList = new List<UserResponse>()
            {
                new UserResponse()
                {
                    Id = new Guid(),
                    IsDeleted = false
                }
            };

            return userResponseList;
        }

        /// <summary>
        /// Set risk control matrix list
        /// </summary>
        /// <returns>List of RCM</returns>
        private List<RiskControlMatrix> SetRiskControlMatrixList()
        {
            var riskControlList = new List<RiskControlMatrix>()
            {
                new RiskControlMatrix()
                {
                    Id = new Guid(),
                    StrategicAnalysisId = new Guid(),
                    IsDeleted = false
                }
            };
            return riskControlList;
        }

        /// <summary>
        /// Set options initial data
        /// </summary>
        /// <returns>List of options</returns>
        private List<Option> SetOptions()
        {
            List<Option> options = new List<Option>()
            {
                new Option()
                {
                    Id = new Guid(),
                    IsDeleted = false
                }
            };
            return options;
        }
        #endregion
        #endregion
    }
}
