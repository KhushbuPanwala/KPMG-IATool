using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.DivisionRepository;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.DivisionTest
{
    [Collection("Register Dependency")]
    public class DivisionRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IDivisionRepository _divisionRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string divisionId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string divisionId2 = "35479716-5e62-4f55-a197-a223805f3869";
        private string enittyId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public DivisionRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _divisionRepository = bootstrap.ServiceProvider.GetService<IDivisionRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set division testing data
        /// </summary>
        /// <returns>List of division</returns>
        private List<Division> SetDivisionInitialData()
        {
            List<Division> divisionObjList = new List<Division>()
            {
                new Division()
                {
                    Id=new Guid(divisionId),
                    Name="div-1",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new Division()
                {
                    Id=new Guid(divisionId),
                    Name="div-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                 new Division()
                {
                    Id=new Guid(),
                    Name="div-3",
                    EntityId=new Guid(enittyId2),
                    IsDeleted=false,
                }
            };
            return divisionObjList;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllDivisionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<Division> divisionDataList = SetDivisionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllDivisionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<Division> divisionDataList = SetDivisionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                    .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllDivisionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<Division> divisionDataList = SetDivisionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                   .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllDivisionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = enittyId1;

            List<Division> divisionDataList = SetDivisionInitialData();
            searchString = "div";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionPageWiseAndSearchWiseAsync(page, pageSize, searchString, Guid.Parse(passedAuditableEntity));

            Assert.Equal(2, result.Count);
        }


        /// <summary>
        /// Test case to verify GetAllDivisionByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Division> divisionDataList = SetDivisionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllDivisionByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllDivisionByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<Division> divisionDataList = SetDivisionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Returns(divisionDataList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _divisionRepository.GetAllDivisionByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetDivisionByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDivisionByIdAsync_ReturnAuditType()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Division> divisionDataList = SetDivisionInitialData();
            var divisionId = divisionDataList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                     .ReturnsAsync(divisionDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == divisionId.ToString()));

            var result = await _divisionRepository.GetDivisionByIdAsync(divisionId, passedAuditableEntity);

            Assert.Equal(result.Name, divisionDataList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetDivisionByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetDivisionByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Division> divisionDataList = SetDivisionInitialData();
            var divisionId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _divisionRepository.GetDivisionByIdAsync(divisionId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDivisionAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            DivisionAC divisionAcObj = new DivisionAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _divisionRepository.AddDivisionAsync(divisionAcObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDivisionAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Division> divisionDataList = SetDivisionInitialData();

            DivisionAC divisionObj = new DivisionAC()
            {
                Name = "div-1"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                            .ReturnsAsync(divisionDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == divisionObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _divisionRepository.AddDivisionAsync(divisionObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddDivisionAsync_AddNewDaat()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Division> divisionDataList = SetDivisionInitialData();

            DivisionAC divisionObj = new DivisionAC()
            {
                Name = "div-3"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                            .ReturnsAsync(divisionDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == divisionObj.Name.ToLower()));

            var newData = new Division()
            {
                Id = new Guid(),
                Name = divisionObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Division>())).ReturnsAsync(newData);

            var result = await _divisionRepository.AddDivisionAsync(divisionObj, passedAuditableEntity);

        }

        /// <summary>
        /// Test case to verify UpdateDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateDivisionAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Division> divisionDataList = SetDivisionInitialData();

            DivisionAC divisionObj = new DivisionAC()
            {
                Name = "div-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                         .Returns(divisionDataList.Where(x => !x.IsDeleted && x.Id == divisionDataList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                            .ReturnsAsync(divisionDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == divisionObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _divisionRepository.UpdateDivisionAsync(divisionObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify UpdateDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateDivisionAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Division> divisionDataList = SetDivisionInitialData();

            DivisionAC divisionObj = new DivisionAC()
            {
                Name = "div new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                            .ReturnsAsync(divisionDataList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == divisionObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Division, bool>>>()))
                           .Returns(divisionDataList.Where(x => !x.IsDeleted && x.Id == divisionDataList[0].Id).AsQueryable().BuildMock().Object);


            await _divisionRepository.UpdateDivisionAsync(divisionObj, passedAuditableEntity);

            _dataRepository.Verify(v => v.Update(It.IsAny<Division>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteDivisionAsync_NoDataFound_ThrowInvalidOperationException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = enittyId1;
            var divisionId = divisionId2;

            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Division> divisionDataList = SetDivisionInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                      .Throws<InvalidOperationException>();


            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
             await _divisionRepository.DeleteDivisionAsync(Guid.Parse(divisionId), Guid.Parse(passedAuditableEntity)));

        }

        /// <summary>
        /// Test case to verify DeleteDivisionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteDivisionAsync_DeleteDivisionSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Division> divisionDataList = SetDivisionInitialData();

            var divisionId = divisionDataList[1].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                     .ReturnsAsync(divisionDataList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == divisionId));


            await _divisionRepository.DeleteDivisionAsync(divisionId, passedAuditableEntity);

        }


        // <summary>
        /// Test case to verify GetTotalCountOfDivisionSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfDivisionSearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<Division> divisionDataList = SetDivisionInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                            .ReturnsAsync(divisionDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _divisionRepository.GetTotalCountOfDivisionSearchStringWiseAsync(null, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfAuditTypeSearchStringWise
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfDivisionSearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<Division> divisionDataList = SetDivisionInitialData();
            var searchString = "div-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Division, bool>>>()))
                             .ReturnsAsync(divisionDataList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));


            var result = await _divisionRepository.GetTotalCountOfDivisionSearchStringWiseAsync(searchString, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        #endregion

    }
}
