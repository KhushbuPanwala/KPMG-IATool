using InternalAuditSystem.DomailModel.DataRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditCategoryRepository;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditPlanModule.Masters.AuditCategoryTest
{
    [Collection("Register Dependency")]
    public class AuditCategoryRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IAuditCategoryRepository _auditCategoryRepository;
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string auditCategoryId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string auditCategoryId2 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string auditCategoryId3 = "a96d7084-011e-446c-ba9a-157499113c72";
        private string enittyId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public AuditCategoryRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _auditCategoryRepository = bootstrap.ServiceProvider.GetService<IAuditCategoryRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set audit types testing data
        /// </summary>
        /// <returns>List of audit types</returns>
        private List<AuditCategory> SetAuditCategoryInitialData()
        {
            List<AuditCategory> auditCategoryObjList = new List<AuditCategory>()
            {
                new AuditCategory()
                {
                    Id=new Guid(auditCategoryId),
                    Name="category-1",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditCategory()
                {
                    Id=new Guid(auditCategoryId2),
                    Name="category-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                 new AuditCategory()
                {
                    Id=new Guid(),
                    Name="category-3",
                    EntityId=new Guid(enittyId2),
                    IsDeleted=false,
                },
                  new AuditCategory()
                {
                    Id=new Guid(auditCategoryId3),
                    Name="category-4",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                }
            };
            return auditCategoryObjList;
        }

        /// <summary>
        /// Set audit plans testing data
        /// </summary>
        /// <returns>List of audit plans</returns>
        private List<AuditPlan> SetAuditPlanInitialData()
        {
            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            List<AuditPlan> auditPlanObjList = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectCategoryId = auditCategoryList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

                new AuditPlan()
                {
                    Id=new Guid(),
                    SelectCategoryId = auditCategoryList[1].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

            };
            return auditPlanObjList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllAuditCategoriesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesPageWiseAndSearchWise_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditCategoriesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesPageWiseAndSearchWise_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllAuditCategoriesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesPageWiseAndSearchWise_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllAuditCategoriesPageWiseAndSearchWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesPageWiseAndSearchWise_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();
            searchString = "category-2";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Single(result);
        }


        /// <summary>
        /// Test case to verify GetAllAuditCategoriesByEntityId
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesByEntityId_ReturnAllCategoriesUnderEntity()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllAuditCategoriesByEntityId
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllAuditCategoriesByEntityId_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _auditCategoryRepository.GetAllAuditCategoriesByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAuditCategoryById
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditCategoryById_ReturnAuditType()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();
            var auditCategoryId = auditCategoryList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .ReturnsAsync(auditCategoryList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == auditCategoryId.ToString()));

            var result = await _auditCategoryRepository.GetAuditCategoryByIdAsync(auditCategoryId.ToString(), passedAuditableEntity.ToString());

            Assert.Equal(result.Name, auditCategoryList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetAuditCategoryById
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAuditCategoryById_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();
            var auditCategoryId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _auditCategoryRepository.GetAuditCategoryByIdAsync(auditCategoryId.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditCategory_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            AuditCategoryAC auditCategoryAcObj = new AuditCategoryAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _auditCategoryRepository.AddAuditCategoryAsync(auditCategoryAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditCategory_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            AuditCategoryAC auditCategoryAcObj = new AuditCategoryAC()
            {
                Name = "category-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditCategoryAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _auditCategoryRepository.AddAuditCategoryAsync(auditCategoryAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddAuditCategory_AddnewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            AuditCategoryAC auditCategorycObj = new AuditCategoryAC()
            {
                Name = "Category-5"
            }; ;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditCategorycObj.Name.ToLower()));

            var dbData = new AuditCategory()
            {
                Id = new Guid(),
                Name = auditCategorycObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<AuditCategory>())).ReturnsAsync(dbData);

            var result = await _auditCategoryRepository.AddAuditCategoryAsync(auditCategorycObj, passedAuditableEntity.ToString());

        }

        /// <summary>
        /// Test case to verify UpdateAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateAuditCategory_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            AuditCategoryAC auditCategorycObj = new AuditCategoryAC()
            {
                Name = "Category-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                        .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.Id == auditCategoryList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditCategorycObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _auditCategoryRepository.UpdateAuditCategoryAsync(auditCategorycObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify UpdateAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateAuditCategory_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            AuditCategoryAC auditCategorycObj = new AuditCategoryAC()
            {
                Name = "Category new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == auditCategorycObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                           .Returns(auditCategoryList.Where(x => !x.IsDeleted && x.Id == auditCategoryList[0].Id).AsQueryable().BuildMock().Object);


            await _auditCategoryRepository.UpdateAuditCategoryAsync(auditCategorycObj, passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.Update(It.IsAny<AuditCategory>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAuditCategory_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            List<AuditPlan> auditPlans = new List<AuditPlan>()
            {
                new AuditPlan()
                {
                    Id=new Guid(),
                    Title= "Plan-1",
                    SelectCategoryId = auditCategoryList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditPlan()
                {
                    Id=new Guid(),
                    Title= "Plan-2",
                    SelectCategoryId = auditCategoryList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new AuditPlan()
                {
                    Id=new Guid(),
                    Title= "Plan-3",
                    SelectCategoryId = auditCategoryList[1].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlans.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.SelectCategoryId == auditCategoryList[0].Id));


            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _auditCategoryRepository.DeleteAuditCategoryAsync(auditCategoryList[0].Id.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteAuditCategory
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteAuditCategory_DeleteSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            List<AuditPlan> auditPlans = SetAuditPlanInitialData();

            var categoryId = auditCategoryList[3].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditPlan, bool>>>()))
                            .ReturnsAsync(auditPlans.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.SelectCategoryId == categoryId));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                     .ReturnsAsync(auditCategoryList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == categoryId.ToString()));


            await _auditCategoryRepository.DeleteAuditCategoryAsync(categoryId.ToString(), passedAuditableEntity.ToString());

        }


        // <summary>
        /// Test case to verify GetTotalCountOfAuditCategorySearchStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditCategorySearchStringWise_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _auditCategoryRepository.GetTotalCountOfAuditCategorySearchStringWiseAsync(searchString, passedAuditableEntity.ToString());

            Assert.Equal(3, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfAuditCategorySearchStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfAuditCategorySearchStringWise_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<AuditCategory> auditCategoryList = SetAuditCategoryInitialData();
            var searchString = "category-1";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<AuditCategory, bool>>>()))
                            .ReturnsAsync(auditCategoryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower().Contains(searchString)));


            var result = await _auditCategoryRepository.GetTotalCountOfAuditCategorySearchStringWiseAsync(searchString, passedAuditableEntity.ToString());

            Assert.Equal(1, result);

        }

        #endregion

    }
}
