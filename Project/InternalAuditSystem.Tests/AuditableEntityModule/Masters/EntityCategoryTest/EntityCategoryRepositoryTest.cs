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
using InternalAuditSystem.Repository.Repository.EntityCategoryRepository;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.EntityCategoryTest
{
    [Collection("Register Dependency")]
    public class EntityCategoryRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        public IEntityCategoryRepository _entityCategoryRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private readonly string entityCategoryId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private readonly string entityCategory2 = "35479716-5e62-4f55-a197-a223805f3869";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";

        #endregion

        #region Constructor
        public EntityCategoryRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _entityCategoryRepository = bootstrap.ServiceProvider.GetService<IEntityCategoryRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

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
                    Id=new Guid(entityCategoryId),
                    CategoryName="cat-1",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new EntityCategory()
                {
                    Id=new Guid(entityCategoryId),
                    CategoryName="cat-2",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                 new EntityCategory()
                {
                    Id=new Guid(),
                    CategoryName="cat-3",
                    EntityId=new Guid(entityCategory2),
                    IsDeleted=false,
                }
            };
            return entityCategoryeObjList;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllEntityCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllEntityCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                    .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllEntityCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                   .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllEntityCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            searchString = "cat";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.CategoryName.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }


        /// <summary>
        /// Test case to verify GetAllEntityCategoryByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllEntityCategoryByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllEntityCategoryByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _entityCategoryRepository.GetAllEntityCategoryByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetEntityCategoryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetEntityCategoryByIdAsync_ReturnRelationshipType()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            var entityCategoryId = entityCategoryDataList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .ReturnsAsync(entityCategoryDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityCategoryId));

            var result = await _entityCategoryRepository.GetEntityCategoryByIdAsync(entityCategoryId, passedAuditableEntity);

            Assert.Equal(result.CategoryName, entityCategoryDataList[0].CategoryName);
        }

        /// <summary>
        /// Test case to verify GetEntityCategoryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetEntityCategoryByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            var entityCategoryId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _entityCategoryRepository.GetEntityCategoryByIdAsync(entityCategoryId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityCategoryAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            EntityCategoryAC entityCategoryObj = new EntityCategoryAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _entityCategoryRepository.AddEntityCategoryAsync(entityCategoryObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityCategoryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            EntityCategoryAC entityCategoryObj = new EntityCategoryAC()
            {
                CategoryName = "cat-1"
            };
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                            .ReturnsAsync(entityCategoryDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == entityCategoryObj.CategoryName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _entityCategoryRepository.AddEntityCategoryAsync(entityCategoryObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddEntityCategoryAsync_AddNewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            EntityCategoryAC entityCategoryObj = new EntityCategoryAC()
            {
                CategoryName = "cat-253"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(entityCategoryDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == entityCategoryObj.CategoryName.ToLower()));

            var newData = new EntityCategory()
            {
                Id = new Guid(),
                CategoryName = entityCategoryObj.CategoryName

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<EntityCategory>())).ReturnsAsync(newData);

            var result = await _entityCategoryRepository.AddEntityCategoryAsync(entityCategoryObj, passedAuditableEntity);

        }

        /// <summary>
        /// Test case to verify UpdateEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateEntityCategoryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            EntityCategoryAC entityCategoryObj = new EntityCategoryAC()
            {
                CategoryName = "cat-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                         .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.Id == entityCategoryDataList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                            .ReturnsAsync(entityCategoryDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == entityCategoryObj.CategoryName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _entityCategoryRepository.UpdateEntityCategoryAsync(entityCategoryObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify UpdateEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateEntityCategoryAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            EntityCategoryAC entityCategoryObj = new EntityCategoryAC()
            {
                CategoryName = "type new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                            .ReturnsAsync(entityCategoryDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == entityCategoryObj.CategoryName.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                           .Returns(entityCategoryDataList.Where(x => !x.IsDeleted && x.Id == entityCategoryDataList[0].Id).AsQueryable().BuildMock().Object);


            await _entityCategoryRepository.UpdateEntityCategoryAsync(entityCategoryObj, passedAuditableEntity);

            _dataRepository.Verify(v => v.Update(It.IsAny<EntityCategory>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteEntityCategoryAsync_NoDataFound_ThrowInvalidOperationException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = entityId1;
            var entityCategoryId = entityCategory2;

            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                      .Throws<InvalidOperationException>();


            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
             await _entityCategoryRepository.DeleteEntityCategoryAsync(Guid.Parse(entityCategoryId), Guid.Parse(passedAuditableEntity)));

        }

        /// <summary>
        /// Test case to verify DeleteEntityCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteEntityCategoryAsync_DeleteEntityCategorySuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            var entityCategoryId = entityCategoryDataList[1].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                     .ReturnsAsync(entityCategoryDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityCategoryId));


            await _entityCategoryRepository.DeleteEntityCategoryAsync(entityCategoryId, passedAuditableEntity);

        }


        // <summary>
        /// Test case to verify GetTotalCountOfEntityCategorySearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfEntityCategorySearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                            .ReturnsAsync(entityCategoryDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _entityCategoryRepository.GetTotalCountOfEntityCategorySearchStringWiseAsync(null, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfEntityCategorySearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfEntityCategorySearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<EntityCategory> entityCategoryDataList = SetEntityCategoryInitialData();
            var searchString = "cat-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCategory, bool>>>()))
                             .ReturnsAsync(entityCategoryDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));


            var result = await _entityCategoryRepository.GetTotalCountOfEntityCategorySearchStringWiseAsync(searchString, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        #endregion

    }
}
