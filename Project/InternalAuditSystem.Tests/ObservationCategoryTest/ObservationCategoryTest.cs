using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.ObservationCategoryRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using System.Threading.Tasks;
using System.Linq.Expressions;
using InternalAuditSystem.Repository.ApplicationClasses;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.ObservationCategoryTest
{
    [Collection("Register Dependency")]
    public class ObservationCategoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IObservationCategoryRepository _observationCategoryRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string observationCategoryId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string observationCategoryId2 = "dd76a04c-9c28-4672-a845-2c6d903cbc69";
        private string observationCategoryId3 = "b506c052-0372-4961-a5d4-a6ca46cb0b77";
        private string observationId = "3014889d-3a15-4f0a-ae09-282939e490f9";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string entityId2 = "a96d7084-011e-446c-ba9a-157499113c72";
        #endregion

        #region Constructor
        public ObservationCategoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _observationCategoryRepository = bootstrap.ServiceProvider.GetService<IObservationCategoryRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set region testing data
        /// </summary>
        /// <returns>List of regions</returns>
        private List<ObservationCategory> SetObservationCategoryListInitialData()
        {
            List<ObservationCategory> observationCategoryObjList = new List<ObservationCategory>()
            {
                new ObservationCategory()
                {
                    Id= Guid.Parse(observationCategoryId),
                    CategoryName="ObservationCategory-1",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new ObservationCategory()
                {
                    Id=Guid.Parse(observationCategoryId2),
                    CategoryName="ObservationCategory-2",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                 new ObservationCategory()
                {
                    Id=Guid.Parse(observationCategoryId3),
                    CategoryName="ObservationCategory-3",
                    EntityId=new Guid(entityId2),
                    IsDeleted=false,
                }
            };
            return observationCategoryObjList;
        }

        private List<ObservationCategoryAC> SetObservationCategoryACListInitialData()
        {
            List<ObservationCategoryAC> observationCategoryACObjList = new List<ObservationCategoryAC>()
            {
                new ObservationCategoryAC()
                {
                    Id= Guid.Parse(observationCategoryId),
                    CategoryName="ObservationCategory-1",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new ObservationCategoryAC()
                {
                    Id=Guid.Parse(observationCategoryId2),
                    CategoryName="ObservationCategory-2",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                 new ObservationCategoryAC()
                {
                    Id=Guid.Parse(observationCategoryId3),
                    CategoryName="ObservationCategory-3",
                    EntityId=new Guid(entityId2),
                    IsDeleted=false,
                }
            };
            return observationCategoryACObjList;
        }

        private ObservationCategory SetObservationCategoryInitialData()
        {
            var categoryObj = new ObservationCategory()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = "ObservationCategory-1",
                EntityId = new Guid(entityId1),
                IsDeleted = false,
            };
            return categoryObj;
        }

        private ObservationCategoryAC SetObservationCategoryACInitialData()
        {
            var categoryACObj = new ObservationCategoryAC()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = "ObservationCategory-1",
                EntityId = new Guid(entityId1),
                IsDeleted = false,
            };
            return categoryACObj;
        }

        private List<Observation> SetObservationListInitialData()
        {
            List<Observation> observationList = new List<Observation>()
            {
                new Observation()
                {
                    Id=Guid.Parse(observationId),
                    ObservationCategoryId=Guid.Parse(observationCategoryId),
                    EntityId=Guid.Parse(entityId1)
                },
                 new Observation()
                {
                    Id=Guid.NewGuid(),
                    ObservationCategoryId=Guid.Parse(observationCategoryId2),
                     EntityId=Guid.Parse(entityId1)
                },
            };
            return observationList;
        }
        /// <summary>
        /// set pagination testing data
        /// </summary>
        /// <returns>Pagination data</returns>
        private Pagination<ObservationCategoryAC> SetPaginationInitialData()
        {
            Pagination<ObservationCategoryAC> pagination = new Pagination<ObservationCategoryAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10,
                EntityId = new Guid(entityId1),
                Items=SetObservationCategoryACListInitialData()
            };
            return pagination;
        }
        #endregion  

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllObservationCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationCategoryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _observationCategoryRepository.GetAllObservationCategoriesPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(0, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllObservationCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationCategoryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _observationCategoryRepository.GetAllObservationCategoriesPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Empty(result.Items);
        }

        /// <summary>
        /// Test case to verify GetAllObservationCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationCategoryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            var paginationObj = SetPaginationInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _observationCategoryRepository.GetAllObservationCategoriesPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(2, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllObservationCategoryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationCategoryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();
            var paginationObj = SetPaginationInitialData();
            searchString = "ObservationCategory-1";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.CategoryName.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _observationCategoryRepository.GetAllObservationCategoriesPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Single(result.Items);
        }

        /// <summary>
        /// Test case to verify GetAllObservationCategoryByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllObservationCategoryByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _observationCategoryRepository.GetAllObservationCategoryByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetObservationCategoryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetObservationCategoryByIdAsync_ReturnRegionDetails()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();
            var regionId = observationCategoryList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .ReturnsAsync(observationCategoryList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == regionId.ToString()));

            var result = await _observationCategoryRepository.GetObservationCategoryDetailsByIdAsync(regionId.ToString(), passedAuditableEntity.ToString());

            Assert.Equal(result.CategoryName, observationCategoryList[0].CategoryName);
        }
        /// <summary>
        /// Test case to verify GetObservationCategoryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetObservationCategoryByIdAsync_NoRegionFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();
            var observationCategoryId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _observationCategoryRepository.GetObservationCategoryDetailsByIdAsync(observationCategoryId.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationCategoryAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            ObservationCategoryAC observationCategoryACObj = new ObservationCategoryAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _observationCategoryRepository.AddObservationCategoryAsync(observationCategoryACObj));

        }

        /// <summary>
        /// Test case to verify AddObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationCategoryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            ObservationCategoryAC observationCategoryObj = new ObservationCategoryAC()
            {
                CategoryName = "ObservationCategory-1"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                            .ReturnsAsync(observationCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == observationCategoryObj.CategoryName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _observationCategoryRepository.AddObservationCategoryAsync(observationCategoryObj));

        }

        /// <summary>
        /// Test case to verify AddRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddObservationCategoryAsync_AddnewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

             List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            ObservationCategoryAC observationCategoryACObj = new ObservationCategoryAC()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = "ObservationCategory-5",
                EntityId = passedAuditableEntity
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                            .ReturnsAsync(observationCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == observationCategoryACObj.CategoryName.ToLower()));

            var dbData = new ObservationCategory()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = observationCategoryACObj.CategoryName

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<ObservationCategory>())).ReturnsAsync(dbData);

            var result = await _observationCategoryRepository.AddObservationCategoryAsync(observationCategoryACObj);
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<ObservationCategory>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test case to verify UpdateObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateObservationCategoryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            ObservationCategoryAC observationCategoryACObj = new ObservationCategoryAC()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = "ObservationCategory-2",
                EntityId = passedAuditableEntity
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                         .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.Id == observationCategoryList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                            .ReturnsAsync(observationCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == observationCategoryACObj.CategoryName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _observationCategoryRepository.UpdateObservationCategoryAsync(observationCategoryACObj));
        }
        /// <summary>
        /// Test case to verify UpdateObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateObservationCategoryAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            ObservationCategoryAC regionAcObj = new ObservationCategoryAC()
            {
                Id = Guid.Parse(observationCategoryId),
                CategoryName = "ObservationCategory - 8",
                EntityId = passedAuditableEntity
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                            .ReturnsAsync(observationCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.CategoryName.ToLower() == regionAcObj.CategoryName.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                           .Returns(observationCategoryList.Where(x => !x.IsDeleted && x.Id == observationCategoryList[0].Id).AsQueryable().BuildMock().Object);


            await _observationCategoryRepository.UpdateObservationCategoryAsync(regionAcObj);

            _dataRepository.Verify(v => v.Update(It.IsAny<ObservationCategory>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);
        }

        /// <summary>
        /// Test case to verify DeleteObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteObservationCategoryAsync_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();

            var observationList = SetObservationListInitialData();
          
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                            .ReturnsAsync(observationList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.ObservationCategoryId == observationCategoryList[0].Id));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                    .ReturnsAsync(observationCategoryList.FirstOrDefault(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == observationCategoryList[0].Id));
            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _observationCategoryRepository.DeleteObservationCategoryAsync(observationList[0].Id.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteObservationCategoryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteObservationCategoryAsync_DeleteSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId2);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<ObservationCategory> observationCategoryList = SetObservationCategoryListInitialData();
            var observationList = SetObservationListInitialData();

            var observationIdFromList = observationCategoryList[2].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Observation, bool>>>()))
                            .ReturnsAsync(observationList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.ObservationCategoryId == observationIdFromList));

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<ObservationCategory, bool>>>()))
                     .ReturnsAsync(observationCategoryList.FirstOrDefault(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == observationIdFromList));
            await _observationCategoryRepository.DeleteObservationCategoryAsync(observationIdFromList.ToString(), passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);
        }
        #endregion  

    }
}
