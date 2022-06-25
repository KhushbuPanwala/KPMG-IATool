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
using InternalAuditSystem.Repository.Repository.EntityTypeReporsitory;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.EntityTypeTest
{
    [Collection("Register Dependency")]
    public class EntityTypeRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        public IEntityTypeRepository _entityTypeRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private readonly string entityTypeId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private readonly string entityTypeId2 = "35479716-5e62-4f55-a197-a223805f3869";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";

        #endregion

        #region Constructor
        public EntityTypeRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _entityTypeRepository = bootstrap.ServiceProvider.GetService<IEntityTypeRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

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
                    Id=new Guid(entityTypeId),
                    TypeName="type-1",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new EntityType()
                {
                    Id=new Guid(entityTypeId),
                    TypeName="type-2",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                 new EntityType()
                {
                    Id=new Guid(),
                    TypeName="type-3",
                    EntityId=new Guid(entityTypeId2),
                    IsDeleted=false,
                }
            };
            return entityTypeObjList;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllEntityTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypesPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllEntityTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypesPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                    .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllEntityTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypesPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                   .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllEntityTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypesPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            searchString = "type";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.TypeName.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }


        /// <summary>
        /// Test case to verify GetAllEntityTypeByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypeByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllEntityTypeByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityTypeByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityTypeRepository.GetAllEntityTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetEntityTypeByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetEntityTypeByIdAsync_ReturnRelationshipType()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            var entityTypeId = entityTypeDataList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .ReturnsAsync(entityTypeDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityTypeId));

            var result = await _entityTypeRepository.GetEntityTypeByIdAsync(entityTypeId, passedAuditableEntity);

            Assert.Equal(result.TypeName, entityTypeDataList[0].TypeName);
        }

        /// <summary>
        /// Test case to verify GetEntityTypeByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetEntityTypeByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            var entityTypeId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _entityTypeRepository.GetEntityTypeByIdAsync(entityTypeId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityTypeAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            EntityTypeAC entityTypeObj = new EntityTypeAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _entityTypeRepository.AddEntityTypeAsync(entityTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityTypeAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            EntityTypeAC entityTypeObj = new EntityTypeAC()
            {
                TypeName = "type-1"
            };
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                            .ReturnsAsync(entityTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.TypeName.ToLower() == entityTypeObj.TypeName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _entityTypeRepository.AddEntityTypeAsync(entityTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityTypeAsync_AddNewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            EntityTypeAC entityTypeObj = new EntityTypeAC()
            {
                TypeName = "type-253"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(entityTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.TypeName.ToLower() == entityTypeObj.TypeName.ToLower()));

            var newData = new EntityType()
            {
                Id = new Guid(),
                TypeName = entityTypeObj.TypeName

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<EntityType>())).ReturnsAsync(newData);

            var result = await _entityTypeRepository.AddEntityTypeAsync(entityTypeObj, passedAuditableEntity);

        }

        /// <summary>
        /// Test case to verify UpdateEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateEntityTypeAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            EntityTypeAC entityTypeObj = new EntityTypeAC()
            {
                TypeName = "type-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                         .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.Id == entityTypeDataList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                            .ReturnsAsync(entityTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.TypeName.ToLower() == entityTypeObj.TypeName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _entityTypeRepository.UpdateEntityTypeAsync(entityTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify UpdateEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateEntityTypeAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            EntityTypeAC entityTypeObj = new EntityTypeAC()
            {
                TypeName = "type new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                            .ReturnsAsync(entityTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.TypeName.ToLower() == entityTypeObj.TypeName.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityType, bool>>>()))
                           .Returns(entityTypeDataList.Where(x => !x.IsDeleted && x.Id == entityTypeDataList[0].Id).AsQueryable().BuildMock().Object);


            await _entityTypeRepository.UpdateEntityTypeAsync(entityTypeObj, passedAuditableEntity);

            _dataRepository.Verify(v => v.Update(It.IsAny<EntityType>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteEntityTypeAsync_NoDataFound_ThrowInvalidOperationException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = entityId1;
            var entityTypeId = entityTypeId2;

            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                      .Throws<InvalidOperationException>();


            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
             await _entityTypeRepository.DeleteEntityTypeAsync(Guid.Parse(entityTypeId), Guid.Parse(passedAuditableEntity)));

        }

        /// <summary>
        /// Test case to verify DeleteEntityTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteEntityTypeAsync_DeleteentityTypeSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            var entityTypeId = entityTypeDataList[1].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                     .ReturnsAsync(entityTypeDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityTypeId));


            await _entityTypeRepository.DeleteEntityTypeAsync(entityTypeId, passedAuditableEntity);

        }


        // <summary>
        /// Test case to verify GetTotalCountOfEntityTypeSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfEntityTypeSearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                            .ReturnsAsync(entityTypeDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _entityTypeRepository.GetTotalCountOfEntityTypeSearchStringWiseAsync(null, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfEntityTypeSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfEntityTypeSearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<EntityType> entityTypeDataList = SetEntityTypeInitialData();
            var searchString = "type-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityType, bool>>>()))
                             .ReturnsAsync(entityTypeDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));


            var result = await _entityTypeRepository.GetTotalCountOfEntityTypeSearchStringWiseAsync(searchString, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        #endregion

    }
}
