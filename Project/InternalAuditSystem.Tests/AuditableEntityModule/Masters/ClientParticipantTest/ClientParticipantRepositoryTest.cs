using InternalAuditSystem.DomailModel.DataRepository;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ClientParticipantRepository;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.ClientParticipantTest
{
    [Collection("Register Dependency")]
    public class ClientParticipantRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        public IClientParticipantRepository _clientParticipantRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private readonly string entityCategoryId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private readonly string entityCategory2 = "35479716-5e62-4f55-a197-a223805f3869";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";

        #endregion

        #region Constructor
        public ClientParticipantRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _clientParticipantRepository = bootstrap.ServiceProvider.GetService<IClientParticipantRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set client participant db testing data
        /// </summary>
        /// <returns>List of client participant</returns>
        private List<EntityUserMapping> SetClientParticipantInitialData()
        {
            List<EntityUserMapping> clientParticipantList = new List<EntityUserMapping>()
            {
                new EntityUserMapping()
                {
                    Id=new Guid(entityCategoryId),
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                    User = new User
                    {
                        Id = new Guid(),
                        Name = "Suparna",
                        EmailId = "supu@gmail.com",
                        UserType = UserType.External
                    }
                },
                new EntityUserMapping()
                {
                    Id=new Guid(entityCategoryId),
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                    User = new User
                    {
                        Id = new Guid(),
                        Name = "Ritu",
                        EmailId = "ritu@gmail.com",
                        UserType = UserType.External
                    }
                },
                 new EntityUserMapping()
                {
                    Id=new Guid(),
                    EntityId=new Guid(entityCategory2),
                    IsDeleted=false,
                    User =  new User
                    {
                        Id = new Guid(),
                        Name = "titli",
                        EmailId = "titli@gmail.com",
                        UserType = UserType.Internal
                    }
                }
            };
            return clientParticipantList;
        }

        /// <summary>
        /// Set client participant db testing data
        /// </summary>
        /// <returns>List of client participant</returns>
        private List<User> SetUsersInitialData()
        {
            List<User> userList = new List<User>()
            {
                 new User
                  {
                        Id = new Guid(),
                        Name = "Suparna",
                        EmailId = "supu@gmail.com",
                        UserType = UserType.External
                  },
                 new User
                  {
                        Id = new Guid(),
                        Name = "Ritu",
                        EmailId = "ritu@gmail.com",
                        UserType = UserType.External

                 },
                 new User
                  {
                        Id = new Guid(),
                        Name = "titli",
                        EmailId = "titli@gmail.com",
                        UserType = UserType.Internal

                  }
            };
            return userList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllClientParticipantsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllClientParticipantsAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Returns(clientParticipantDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.User.UserType == UserType.External).AsQueryable().BuildMock().Object);

            var result = await _clientParticipantRepository.GetAllClientParticipantsAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllClientParticipantsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllClientParticipantsAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                    .Returns(clientParticipantDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.User.UserType == UserType.External).AsQueryable().BuildMock().Object);

            var result = await _clientParticipantRepository.GetAllClientParticipantsAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllClientParticipantsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllClientParticipantsAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                   .Returns(clientParticipantDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.User.UserType == UserType.External).AsQueryable().BuildMock().Object);

            var result = await _clientParticipantRepository.GetAllClientParticipantsAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllClientParticipantsAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllClientParticipantsAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();
            searchString = "Suparna";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Returns(clientParticipantDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.User.UserType == UserType.External
                     && x.User.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _clientParticipantRepository.GetAllClientParticipantsAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Single(result);
        }


        /// <summary>
        /// Test case to verify GetClientParticipantByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetClientParticipantByIdAsync_ReturnRelationshipType()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();
            var clientParticipantId = clientParticipantDataList[0].Id;

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Returns(clientParticipantDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == clientParticipantId)
                     .AsQueryable().BuildMock().Object);

            var result = await _clientParticipantRepository.GetClientParticipantByIdAsync(clientParticipantId, passedAuditableEntity);

            Assert.Equal(result.Name, clientParticipantDataList[0].User.Name);
        }

        /// <summary>
        /// Test case to verify GetClientParticipantByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetClientParticipantByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityUserMapping> clientParticipantDataList = SetClientParticipantInitialData();
            var clientParticipantId = new Guid();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityUserMapping, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _clientParticipantRepository.GetClientParticipantByIdAsync(clientParticipantId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddClientParticipantAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddClientParticipantAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            UserAC clientParticipantObj = new UserAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _clientParticipantRepository.AddClientParticipantAsync(clientParticipantObj, passedAuditableEntity));

        }

        #endregion

    }
}

