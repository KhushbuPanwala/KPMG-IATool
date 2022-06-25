using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RegionRepository;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.RegionTest
{
    [Collection("Register Dependency")]
    public class RegionRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IRegionRepository _regionRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string regionId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string regionId2 = "ae694442-2437-4ed3-8a18-5e39f968c6ce";
        private string regionId3 = "3e1a999b-9b49-47d2-b364-c02591df7423";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string entityId2 = "a96d7084-011e-446c-ba9a-157499113c72";

        #endregion

        #region Constructor
        public RegionRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _regionRepository = bootstrap.ServiceProvider.GetService<IRegionRepository>();
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
        private List<Region> SetRegionInitialData()
        {
            List<Region> regionObjList = new List<Region>()
            {
                new Region()
                {
                    Id= Guid.Parse(regionId),
                    Name="region-1",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new Region()
                {
                    Id=Guid.Parse(regionId2),
                    Name="region-2",
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                 new Region()
                {
                    Id=Guid.Parse(regionId3),
                    Name="region-3",
                    EntityId=new Guid(entityId2),
                    IsDeleted=false,
                }
            };
            return regionObjList;
        }

        /// <summary>
        /// Set PrimaryGeographicalArea testing data
        /// </summary>
        /// <returns>List of PrimaryGeographicalArea</returns>
        private List<PrimaryGeographicalArea> SetPrimaryGeographicalAreaInitialData()
        {
            var regionList = SetRegionInitialData();
            List<PrimaryGeographicalArea> geographicalAreaList = new List<PrimaryGeographicalArea>()
            {
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    RegionId =regionList[0].Id,
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    RegionId = regionList[0].Id,
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },

            };
            return geographicalAreaList;
        }

        /// <summary>
        /// set entity country testing data
        /// </summary>
        /// <returns>List of countries</returns>
        private List<EntityCountry> SetCountryInitialData()
        {
            List<EntityCountry> countryList = new List<EntityCountry>()
            {
                new EntityCountry()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },
                new EntityCountry()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                },

            };
            return countryList;
        }

        /// <summary>
        /// set pagination testing data
        /// </summary>
        /// <returns>Pagination data</returns>
        private Pagination<RegionAC> SetPaginationInitialData()
        {
            Pagination<RegionAC> pagination = new Pagination<RegionAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10,
                EntityId = new Guid(entityId1)
            };
            return pagination;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllRegionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<Region> regionList = SetRegionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _regionRepository.GetAllRegionPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(0, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllRegionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<Region> regionList = SetRegionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _regionRepository.GetAllRegionPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Empty(result.Items);
        }

        /// <summary>
        /// Test case to verify GetAllRegionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<Region> regionList = SetRegionInitialData();
            var paginationObj = SetPaginationInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _regionRepository.GetAllRegionPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(2, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllRegionPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<Region> regionList = SetRegionInitialData();
            var paginationObj = SetPaginationInitialData();
            searchString = "region-1";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);

            var result = await _regionRepository.GetAllRegionPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Single(result.Items);
        }


        /// <summary>
        /// Test case to verify GetAllRegionsByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionsByEntityIdAsync_ReturnAllTypesUnderEntity()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<Region> regionList = SetRegionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _regionRepository.GetAllRegionsByEntityIdAsync(passedAuditableEntity);

            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Test case to verify GetAllRegionsByEntityIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllRegionsByEntityIdAsync_ReturnNoElementUnderEntity()
        {
            var passedAuditableEntity = new Guid();

            List<Region> regionList = SetRegionInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _regionRepository.GetAllRegionsByEntityIdAsync(passedAuditableEntity);

            Assert.Empty(result);
        }

        /// <summary>
        /// Test case to verify GetRegionByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRegionByIdAsync_ReturnRegionDetails()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<Region> regionList = SetRegionInitialData();
            var regionId = regionList[0].Id;

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                     .ReturnsAsync(regionList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == regionId.ToString()));

            var result = await _regionRepository.GetRegionByIdAsync(regionId.ToString(), passedAuditableEntity.ToString());

            Assert.Equal(result.Name, regionList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetRegionByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetRegionByIdAsync_NoRegionFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<Region> regionList = SetRegionInitialData();
            var regionId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _regionRepository.GetRegionByIdAsync(regionId.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRegionAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            RegionAC regionAcObj = new RegionAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Throws<NullReferenceException>();

            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _regionRepository.AddRegionAsync(regionAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRegionAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Region> regionList = SetRegionInitialData();

            RegionAC regionAcObj = new RegionAC()
            {
                Name = "region-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == regionAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _regionRepository.AddRegionAsync(regionAcObj, passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify AddRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddRegionAsync_AddnewData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Region> regionList = SetRegionInitialData();

            RegionAC regionAcObj = new RegionAC()
            {
                Name = "region-3"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == regionAcObj.Name.ToLower()));

            var dbData = new Region()
            {
                Id = Guid.NewGuid(),
                Name = regionAcObj.Name

            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Region>())).ReturnsAsync(dbData);

            var result = await _regionRepository.AddRegionAsync(regionAcObj, passedAuditableEntity.ToString());
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<Region>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test case to verify UpdateRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRegionAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Region> regionList = SetRegionInitialData();

            RegionAC regionAcObj = new RegionAC()
            {
                Name = "region-2"
            };

            //mock db calls
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                         .Returns(regionList.Where(x => !x.IsDeleted && x.Id == regionList[0].Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == regionAcObj.Name.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _regionRepository.UpdateRegionAsync(regionAcObj, passedAuditableEntity.ToString()));


        }

        /// <summary>
        /// Test case to verify UpdateRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateRegionAsync_UpdateData()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<Region> regionList = SetRegionInitialData();

            RegionAC regionAcObj = new RegionAC()
            {
                Name = "region new",
                EntityId = passedAuditableEntity
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Any(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower() == regionAcObj.Name.ToLower()));

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                           .Returns(regionList.Where(x => !x.IsDeleted && x.Id == regionList[0].Id).AsQueryable().BuildMock().Object);


            await _regionRepository.UpdateRegionAsync(regionAcObj, passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.Update(It.IsAny<Region>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }

        /// <summary>
        /// Test case to verify DeleteRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRegionAsync_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Region> regionList = SetRegionInitialData();

            var geographicalAreaList = SetPrimaryGeographicalAreaInitialData();
            var countryList = SetCountryInitialData();
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<PrimaryGeographicalArea, bool>>>()))
                            .ReturnsAsync(geographicalAreaList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.RegionId == regionList[0].Id));
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(countryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));
            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _regionRepository.DeleteRegionAsync(regionList[0].Id.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteRegionAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteRegionAsync_DeleteSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId2);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<Region> regionList = SetRegionInitialData();
            var geographicalAreaList = SetPrimaryGeographicalAreaInitialData();
            var countryList = SetCountryInitialData();


            var regionIdFromList = regionList[2].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<PrimaryGeographicalArea, bool>>>()))
                            .ReturnsAsync(geographicalAreaList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.RegionId == regionIdFromList));

            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                           .ReturnsAsync(countryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.RegionId== regionIdFromList));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                     .ReturnsAsync(regionList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == regionIdFromList));
            await _regionRepository.DeleteRegionAsync(regionIdFromList.ToString(), passedAuditableEntity.ToString());
            
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);
        }

        // <summary>
        /// Test case to verify GetTotalCountOfRegion SearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfLocationSearchStringWiseAsync_ReturnTotalRecordsCount_WithoutSerachString()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<Region> regionList = SetRegionInitialData();

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity));

            var result = await _regionRepository.GetTotalCountOfRegionSearchStringWiseAsync(null, passedAuditableEntity.ToString());

            Assert.Equal(2, result);
        }

        // <summary>
        /// Test case to verify GetTotalCountOfRegion SearchStringWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetTotalCountOfLocationSearchStringWiseAsync_ReturnTotalRecordsCount_SerachStringWise()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            List<Region> regionList = SetRegionInitialData();
            var searchRegionString = "region-";

            //mock db calls
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<Region, bool>>>()))
                            .ReturnsAsync(regionList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Name.ToLower().Contains(searchRegionString)));

            var result = await _regionRepository.GetTotalCountOfRegionSearchStringWiseAsync(searchRegionString, passedAuditableEntity.ToString());

            Assert.Equal(2, result);

        }
        #endregion

    }
}
