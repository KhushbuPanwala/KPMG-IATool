using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.LocationRepository;
using Moq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.LocationTest
{
    [Collection("Register Dependency")]
    public class LocationRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private ILocationRepository _locationRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string locationId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string enittyId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string enittyId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public LocationRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _locationRepository = bootstrap.ServiceProvider.GetService<ILocationRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set location testing data
        /// </summary>
        /// <returns>List of audit types</returns>
        private List<Location> SetLocationInitialData()
        {
            List<Location> auditTypeObjList = new List<Location>()
            {
                new Location()
                {
                    Id=new Guid(locationId),
                    Name="location-1",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new Location()
                {
                    Id=new Guid(),
                    Name="location-2",
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                 new Location()
                {
                    Id=new Guid(),
                    Name="location-3",
                    EntityId=new Guid(enittyId2),
                    IsDeleted=false,
                }
            };
            return auditTypeObjList;
        }

        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllLocationPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = Guid.Empty;

            List<Location> locationList = SetLocationInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllLocationPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid();

            List<Location> locationList = SetLocationInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetAllLocationPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = Guid.Parse(enittyId1);

            List<Location> locationList = SetLocationInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllLocationPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = Guid.Parse(enittyId1);

            List<Location> locationList = SetLocationInitialData();
            searchString = "location-1";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationPageWiseAndSearchWiseAsync(page, pageSize, searchString, passedAuditableEntity);

            Assert.Single(result);
        }


        /// <summary>
        /// Test case to verify GetAllLocationsByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationsByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Location> locationList = SetLocationInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationsByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllLocationsByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllLocationsByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<Location> locationList = SetLocationInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Returns(locationList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _locationRepository.GetAllLocationsByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetLocationByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetLocationByIdAsync_ReturnAuditType()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Location> locationList = SetLocationInitialData();
            var locationId = locationList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                     .ReturnsAsync(locationList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == locationId.ToString()));

            var result = await _locationRepository.GetLocationByIdAsync(locationId, passedAuditableEntity);

            Assert.Equal(result.Name, locationList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetLocationByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetLocationByIdAsync_NoAuditTypeFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(enittyId1);

            List<Location> locationList = SetLocationInitialData();
            var locationId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _locationRepository.GetLocationByIdAsync(locationId, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddLocationAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            LocationAC locationAcObj = new LocationAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _locationRepository.AddLocationAsync(locationAcObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddLocationAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Location> locationList = SetLocationInitialData();

            LocationAC locationAcObj = new LocationAC()
            {
                Name = "location-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == locationAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _locationRepository.AddLocationAsync(locationAcObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify AddLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddLocationAsync_AddnewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Location> locationList = SetLocationInitialData();

            LocationAC locationAcObj = new LocationAC()
            {
                Name = "location-3"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == locationAcObj.Name.ToLower()));

            var dbData = new Location()
            {
                Id = new Guid(),
                Name = locationAcObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Location>())).ReturnsAsync(dbData);

            var result = await _locationRepository.AddLocationAsync(locationAcObj, passedAuditableEntity);

        }

        /// <summary>
        /// Test case to verify UpdateLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateLocationAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Location> locationList = SetLocationInitialData();

            LocationAC locationAcObj = new LocationAC()
            {
                Name = "location-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                         .Returns(locationList.Where(x => !x.IsDeleted && x.Id == locationList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == locationAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _locationRepository.UpdateLocationAsync(locationAcObj, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify UpdateLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateLocationAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Location> locationList = SetLocationInitialData();

            LocationAC locationAcObj = new LocationAC()
            {
                Name = "location new"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == locationAcObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Location, bool>>>()))
                           .Returns(locationList.Where(x => !x.IsDeleted && x.Id == locationList[0].Id).AsQueryable().BuildMock().Object);


            await _locationRepository.UpdateLocationAsync(locationAcObj, passedAuditableEntity);

            _dataRepository.Verify(v => v.Update(It.IsAny<Location>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }


        /// <summary>
        /// Test case to verify DeleteLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteLocationAsync_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Location> locationList = SetLocationInitialData();

            List<PrimaryGeographicalArea> geographicalAreaList = new List<PrimaryGeographicalArea>()
            {
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    LocationId = locationList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    LocationId = locationList[1].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<PrimaryGeographicalArea, bool>>>()))
                            .ReturnsAsync(geographicalAreaList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.LocationId == locationList[0].Id));

            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _locationRepository.DeleteLocationAsync(locationList[0].Id, passedAuditableEntity));

        }

        /// <summary>
        /// Test case to verify DeleteLocationAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteLocationAsync_DeleteTypeSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Location> locationList = SetLocationInitialData();

            List<PrimaryGeographicalArea> geographicalAreaList = new List<PrimaryGeographicalArea>()
            {
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    LocationId = locationList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    LocationId = locationList[0].Id,
                    EntityId=new Guid(enittyId1),
                    IsDeleted=false,
                },

            };

            var locationId = locationList[1].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<PrimaryGeographicalArea, bool>>>()))
                            .ReturnsAsync(geographicalAreaList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.LocationId == locationId));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                     .ReturnsAsync(locationList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == locationId.ToString()));


            await _locationRepository.DeleteLocationAsync(locationId, passedAuditableEntity);

        }


        // <summary>
        /// Test case to verify GetTotalCountOfLocationSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfLocationSearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<Location> locationList = SetLocationInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));



            var result = await _locationRepository.GetTotalCountOfLocationSearchStringWiseAsync(null, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        // <summary>
        /// Test case to verify GetTotalCountOfLocationSearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfLocationSearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(enittyId1);
            List<Location> locationList = SetLocationInitialData();
            var searchString = "location-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                            .ReturnsAsync(locationList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower().Contains(searchString)));


            var result = await _locationRepository.GetTotalCountOfLocationSearchStringWiseAsync(searchString, passedAuditableEntity);

            Assert.Equal(2, result);

        }

        #endregion

    }
}
