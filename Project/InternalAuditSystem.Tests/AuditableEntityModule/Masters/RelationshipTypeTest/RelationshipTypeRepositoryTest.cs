using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.RelationshipTypeRepository;
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
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.RelationshipTypeTest
{
    [Collection("Register Dependency")]
    public class RelationshipTypeRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRelationshipTypeRepository _relationshipTypeRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string relationShipTypeId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string relationShipTypeId2 = "35479716-5e62-4f55-a197-a223805f3869";
        private string enittyId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public RelationshipTypeRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _relationshipTypeRepository = bootstrap.ServiceProvider.GetService<IRelationshipTypeRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set relationship type testing data
        /// </summary>
        /// <returns>List of relationship type</returns>
        private List<RelationshipType> SetRelationshipTypeInitialData()
        {
            List<RelationshipType> relationshipTypeObjList = new List<RelationshipType>()
            {
                new RelationshipType()
                {
                    Id=new Guid(relationShipTypeId),
                    Name="rel-1",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new RelationshipType()
                {
                    Id=new Guid(relationShipTypeId),
                    Name="rel-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                 new RelationshipType()
                {
                    Id=new Guid(),
                    Name="rel-3",
                    EntityId=new Guid(enittyId2),
                    IsDeleted=false,
                }
            };
            return relationshipTypeObjList;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllRelationshipTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypesPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllRelationshipTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypesPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                    .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllRelationshipTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypesPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                   .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllRelationshipTypesPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypesPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            searchString = "rel";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypesPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }


        /// <summary>
        /// Test case to verify GetAllRelationshipTypeByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypeByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllRelationshipTypeByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRelationshipTypeByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _relationshipTypeRepository.GetAllRelationshipTypeByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetRelationshipTypeByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRelationshipTypeByIdAsync_ReturnRelationshipType()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            var relationshipTypeId = relationTypeDataList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .ReturnsAsync(relationTypeDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == relationshipTypeId));

            var result = await _relationshipTypeRepository.GetRelationshipTypeByIdAsync(relationshipTypeId, passedAuditableEntity);

            Assert.Equal(result.Name, relationTypeDataList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetRelationshipTypeByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRelationshipTypeByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            var relationshipTypeId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _relationshipTypeRepository.GetRelationshipTypeByIdAsync(relationshipTypeId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRelationshipTypeAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            RelationshipTypeAC relationshipTypeObj = new RelationshipTypeAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _relationshipTypeRepository.AddRelationshipTypeAsync(relationshipTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRelationshipTypeAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            RelationshipTypeAC relationshipTypeObj = new RelationshipTypeAC()
            {
                Name = "rel-1"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(relationTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == relationshipTypeObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _relationshipTypeRepository.AddRelationshipTypeAsync(relationshipTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRelationshipTypeAsync_AddNewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            RelationshipTypeAC relationshipTypeObj = new RelationshipTypeAC()
            {
                Name = "rel-3"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(relationTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == relationshipTypeObj.Name.ToLower()));

            var newData = new RelationshipType()
            {
                Id = new Guid(),
                Name = relationshipTypeObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<RelationshipType>())).ReturnsAsync(newData);

            var result = await _relationshipTypeRepository.AddRelationshipTypeAsync(relationshipTypeObj, passedAuditableEntity);

        }

        /// <summary>
        /// Test case to verify UpdateRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRelationshipTypeAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            RelationshipTypeAC relationshipTypeObj = new RelationshipTypeAC()
            {
                Name = "rel-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                         .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.Id == relationTypeDataList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(relationTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == relationshipTypeObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _relationshipTypeRepository.UpdateRelationshipTypeAsync(relationshipTypeObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify UpdateRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRelationshipTypeAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            RelationshipTypeAC relationshipTypeObj = new RelationshipTypeAC()
            {
                Name = "relation new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(relationTypeDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == relationshipTypeObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                           .Returns(relationTypeDataList.Where(x => !x.IsDeleted && x.Id == relationTypeDataList[0].Id).AsQueryable().BuildMock().Object);


            await _relationshipTypeRepository.UpdateRelationshipTypeAsync(relationshipTypeObj, passedAuditableEntity);

            _dataRepository.Verify(v => v.Update(It.IsAny<RelationshipType>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRelationshipTypeAsync_NoDataFound_ThrowInvalidOperationException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = enittyId1;
            var relationshipTypeId = relationShipTypeId2;

            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                      .Throws<InvalidOperationException>();


            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
             await _relationshipTypeRepository.DeleteRelationshipTypeAsync(Guid.Parse(relationshipTypeId), Guid.Parse(passedAuditableEntity)));

        }

        /// <summary>
        /// Test case to verify DeleteRelationshipTypeAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRelationshipTypeAsync_DeleteRelationshipTypeSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            var relationshipTypeId = relationTypeDataList[1].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                     .ReturnsAsync(relationTypeDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == relationshipTypeId));


            await _relationshipTypeRepository.DeleteRelationshipTypeAsync(relationshipTypeId, passedAuditableEntity);

        }


        // <summary>
        /// Test case to verify GetTotalCountOfRelationshipTypeSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfRelationshipTypeSearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                            .ReturnsAsync(relationTypeDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _relationshipTypeRepository.GetTotalCountOfRelationshipTypeSearchStringWiseAsync(null, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfRelationshipTypeSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfRelationshipTypeSearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<RelationshipType> relationTypeDataList = SetRelationshipTypeInitialData();
            var searchString = "rel-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<RelationshipType, bool>>>()))
                             .ReturnsAsync(relationTypeDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));


            var result = await _relationshipTypeRepository.GetTotalCountOfRelationshipTypeSearchStringWiseAsync(searchString, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        #endregion

    }
}
