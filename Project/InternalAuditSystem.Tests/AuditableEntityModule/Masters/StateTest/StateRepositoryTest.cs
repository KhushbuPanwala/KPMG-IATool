using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.ProvinceStateRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.StateTest
{
    [Collection("Register Dependency")]
    public class StateRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private IStateRepository _stateRepository;
        public Mock<IHttpContextAccessor> _httpContextAccessor;

        private readonly int page = 1;
        private readonly int pageSize = 10;
        private string searchString = null;
        private string regionId = "0e66c9dc-b4a3-40de-b7c3-99f279b8e1d7";
        private string regionId2 = "8dfff796-941e-4a4a-b1e3-9c55512baf12";
        private string countryId = "77ccc450-cd4e-4318-a6ef-bb087457046f";
        private string countryId2 = "4cd60a41-041a-4723-ac5b-cca2b654c2b1";
        private string countryId3 = "f5911a37-db36-48f5-a8cb-1f893f5702aa";
        private string entityId1 = "40d4fde1-f5df-4770-90bd-81b4ea3f621e";
        private string entityCountryId = "ec2e1bc6-3034-4e6b-877c-ca40488a08c6";
        private string entityStateId = "a60d50c8-5129-4857-9a94-3fa2c395b79b";
        private string stateId = "cb968329-db45-4b73-95ba-a7c043c4d118";
        #endregion

        #region Constructor
        public StateRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _stateRepository = bootstrap.ServiceProvider.GetService<IStateRepository>();
            _dataRepository = bootstrap.ServiceProvider.GetService<Mock<IDataRepository>>();
            _httpContextAccessor = bootstrap.ServiceProvider.GetService<Mock<IHttpContextAccessor>>();
            _dataRepository.Reset();

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Set EntityCountry testing data
        /// </summary>
        /// <returns>List of EntityCountry</returns>
        private List<EntityCountry> SetEntityCountryListInitialData()
        {
            List<EntityCountry> countryObjList = new List<EntityCountry>()
            {
                new EntityCountry()
                {
                    Id=Guid.Parse(entityCountryId),
                    EntityId=new Guid(entityId1),
                    RegionId=new Guid(regionId),
                    CountryId=new Guid(countryId),
                    Country=SetCountryInitialData(),
                    Region=SetRegionInitialData(),
                    IsDeleted=false,
                },
                new EntityCountry()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                     RegionId=new Guid(regionId),
                     CountryId=new Guid(countryId2),
                       Country=SetCountryInitialData(),
                       Region=SetRegionInitialData(),
                    IsDeleted=false,
                },
                 new EntityCountry()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                     RegionId=new Guid(regionId2),
                     CountryId=new Guid(countryId3),
                       Country=SetCountryInitialData(),
                       Region=SetRegionInitialData(),
                    IsDeleted=false,
                }
            };
            return countryObjList;
        }
        private EntityCountry SetEntityCountryInitialData()
        {

            var entityCountryObj=   new EntityCountry()
            {
                Id = Guid.Parse(entityCountryId),
                EntityId = new Guid(entityId1),
                RegionId = new Guid(regionId),
                CountryId = new Guid(countryId),
                Country = SetCountryInitialData(),
                Region = SetRegionInitialData(),
                IsDeleted = false,
            };
            
            return entityCountryObj;
        }
        private List<EntityCountryAC> SetEntityCountryACInitialData()
        {
            List<EntityCountryAC> countryObjACList = new List<EntityCountryAC>()
            {
                new EntityCountryAC()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    RegionId=new Guid(regionId),
                    CountryId=new Guid(countryId),
                    CountryName="Peru",
                    RegionName="region-1",
                    IsDeleted=false,
                },
                new EntityCountryAC()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                     RegionId=new Guid(regionId),
                     CountryId=new Guid(countryId2),
                    IsDeleted=false,
                },
                 new EntityCountryAC()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                     RegionId=new Guid(regionId2),
                     CountryId=new Guid(countryId3),
                    IsDeleted=false,
                }
            };
            return countryObjACList;
        }
        /// <summary>
        /// set pagination testing data
        /// </summary>
        /// <returns>Pagination data</returns>
        private Pagination<EntityCountryAC> SetCountryPaginationInitialData()
        {
            Pagination<EntityCountryAC> pagination = new Pagination<EntityCountryAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10,
                EntityId = new Guid(entityId1)
            };
            return pagination;
        }

        private Pagination<EntityStateAC> SetStatePaginationInitialData()
        {
            Pagination<EntityStateAC> pagination = new Pagination<EntityStateAC>()
            {
                searchText = string.Empty,
                PageIndex = 1,
                PageSize = 10,
                EntityId = new Guid(entityId1),
                Items = SetEntityStateACListInitialData(),
                TotalRecords = 3
            };
            return pagination;
        }
        private Country SetCountryInitialData()
        {
            Country countryObj = new Country()
            {
                Id = Guid.Parse(countryId),
                Name = "Peru",
                CreatedDateTime = DateTime.UtcNow
            };
            return countryObj;
        }

        private List<Country> SetCountryListInitialData()
        {
            List<Country> countryListObj = new List<Country>()
            {
                new Country(){
                Id = Guid.Parse(countryId),
                Name = "Peru",
                CreatedDateTime = DateTime.UtcNow
                },
                new Country(){
                Id = Guid.NewGuid(),
                Name = "Poland",
                CreatedDateTime = DateTime.UtcNow
                },
                new Country(){
                Id = Guid.NewGuid(),
                Name = "brazil",
                CreatedDateTime = DateTime.UtcNow
                },
            };
            return countryListObj;
        }
        private Region SetRegionInitialData()
        {
            Region regionObj = new Region()
            {
                Id = Guid.NewGuid(),
                Name = "region-1",
                CreatedDateTime = DateTime.UtcNow
            };
            return regionObj;
        }

        private List<Region> SetRegionListInitialData()
        {
            List<Region> regionListObj = new List<Region>()
            {
                new Region(){
                Id =Guid.Parse( regionId),
                Name = "region-1",
                EntityId=Guid.Parse(entityId1),
                CreatedDateTime = DateTime.UtcNow
                },
                new Region(){
                Id = Guid.NewGuid(),
                Name = "region-2",
                CreatedDateTime = DateTime.UtcNow
                },
                new Region(){
                Id = Guid.NewGuid(),
                Name = "region-3",
                CreatedDateTime = DateTime.UtcNow
                },
            };
            return regionListObj;
        }

        private List<ProvinceState> SetProvinceStateListInitialData()
        {
            List<ProvinceState> provinceStateListObj = new List<ProvinceState>()
            {
                new ProvinceState(){
                Id =Guid.Parse(stateId),
                Name = "Porto",
                CountryId=Guid.Parse(countryId),
                CreatedDateTime = DateTime.UtcNow
                },
                new ProvinceState(){
                Id = Guid.NewGuid(),
                Name = "South Cotabato",
                CountryId=Guid.Parse(countryId2),
                CreatedDateTime = DateTime.UtcNow
                }
            };
            return provinceStateListObj;
        }

        private ProvinceState SetProvinceStateInitialData()
        {
            ProvinceState stateObj = new ProvinceState()
            {
                Id = Guid.Parse(stateId),
                Name = "state-1",
                CountryId = Guid.Parse(countryId),
                CreatedDateTime = DateTime.UtcNow
            };
            return stateObj;
        }

        /// <summary>
        /// Set EntityState testing data
        /// </summary>
        /// <returns>List of EntityState</returns>
        private List<EntityState> SetEntityStateListInitialData()
        {
            List<EntityState> stateObjList = new List<EntityState>()
            {
                new EntityState()
                {
                    Id=Guid.Parse(entityStateId),
                    EntityId=new Guid(entityId1),
                    StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    IsDeleted=false,
                    ProvinceState=SetProvinceStateInitialData(),
                    EntityCountry=SetEntityCountryInitialData()
                },
                new EntityState()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    IsDeleted=false,
                     ProvinceState=SetProvinceStateInitialData(),
                      EntityCountry=SetEntityCountryInitialData()
                },
                 new EntityState()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                     StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    IsDeleted=false,
                      ProvinceState=SetProvinceStateInitialData(),
                       EntityCountry=SetEntityCountryInitialData()
                }
            };
            return stateObjList;
        }

        private List<EntityStateAC> SetEntityStateACListInitialData()
        {
            List<EntityStateAC> stateObjACList = new List<EntityStateAC>()
            {
                new EntityStateAC()
                {
                    Id=Guid.Parse(entityStateId),
                    EntityId=new Guid(entityId1),
                    StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    CountryName="Peru",
                    RegionName="region-1",
                    StateName="state-1",
                    IsDeleted=false,

                },
                new EntityStateAC()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    CountryName="Mongolia",
                    RegionName="region-2",
                     StateName="state-2",
                    IsDeleted=false,
                },
                 new EntityStateAC()
                {
                    Id=Guid.NewGuid(),
                    EntityId=new Guid(entityId1),
                    StateId=Guid.Parse(stateId),
                    EntityCountryId=Guid.Parse(entityCountryId),
                    CountryName="USA",
                    RegionName="region-1",
                     StateName="state-3",
                    IsDeleted=false,
                }
            };
            return stateObjACList;
        }

        private EntityState  SetEntityStateInitialData()
        {
            var stateObj = new EntityState()
            {
                Id = Guid.Parse(entityStateId),
                EntityId = new Guid(entityId1),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                IsDeleted = false,
            };
          return stateObj;

        }
        /// <summary>
        /// Set PrimaryGeographicalArea testing data
        /// </summary>
        /// <returns>List of PrimaryGeographicalArea</returns>
        private List<PrimaryGeographicalArea> SetPrimaryGeographicalAreaInitialData()
        {
            var regionList = SetRegionListInitialData();
            List<PrimaryGeographicalArea> geographicalAreaList = new List<PrimaryGeographicalArea>()
            {
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    RegionId =regionList[0].Id,
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                    EntityStateId=Guid.Parse(entityStateId),
                    EntityState=SetEntityStateInitialData()
                },
                new PrimaryGeographicalArea()
                {
                    Id=new Guid(),
                    RegionId = regionList[0].Id,
                    EntityId=new Guid(entityId1),
                    IsDeleted=false,
                     EntityStateId=Guid.Parse(entityStateId),
                      EntityState=SetEntityStateInitialData()
                },

            };
            return geographicalAreaList;
        }
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllStatePageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStatePageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<EntityState> stateList = SetEntityStateListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Returns(stateList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetStatePaginationInitialData();
            var result = await _stateRepository.GetAllStatePageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(0, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllStatePageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStatePageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<EntityState> stateList = SetEntityStateListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Returns(stateList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetStatePaginationInitialData();
            var result = await _stateRepository.GetAllStatePageWiseAndSearchWiseAsync(paginationObj);

            Assert.Empty(result.Items);
        }

        /// <summary>
        /// Test case to verify GetAllStatePageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStatePageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityState> stateList = SetEntityStateListInitialData();
            var paginationObj = SetStatePaginationInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Returns(stateList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _stateRepository.GetAllStatePageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(3, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllStatePageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllStatePageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityState> stateList = SetEntityStateListInitialData();
            List<EntityStateAC> stateACList = SetEntityStateACListInitialData();
            var paginationObj = SetStatePaginationInitialData();
            searchString = "state-1";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Returns(stateList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.ProvinceState.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityStateAC, bool>>>()))
                    .Returns(stateACList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.StateName.ToLower().Contains(searchString.ToLower()))
                    .AsQueryable().BuildMock().Object);
            var result = await _stateRepository.GetAllStatePageWiseAndSearchWiseAsync(paginationObj);

            Assert.NotNull(result.Items);
        }
        /// <summary>
        /// Test case to verify GetStateByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetStateByIdAsync_ReturnCountryDetails()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityState> entityStateList = SetEntityStateListInitialData();
            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();
            List<Country> countryList = SetCountryListInitialData();
            List<Region> regionList = SetRegionListInitialData();
            var stateId = entityStateList[0].Id;

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Returns(entityStateList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == stateId.ToString()).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(entityCountryList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == stateId.ToString()).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Country, bool>>>()))
                    .Returns(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == stateId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).Distinct().OrderByDescending(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            var result = await _stateRepository.GetStateByIdAsync(stateId.ToString(), passedAuditableEntity.ToString());

            Assert.Equal(result.StateName, entityStateList[0].ProvinceState.Name);
        }

        /// <summary>
        /// Test case to verify GetStateByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetStateByIdAsync_NoRegionFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);

            var stateId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _stateRepository.GetStateByIdAsync(stateId.ToString(), passedAuditableEntity.ToString()));

        }
        /// <summary>
        /// Test case to verify AddStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddStateAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            EntityStateAC entityStateACObj = new EntityStateAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Throws<NullReferenceException>();
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                 .Throws<NullReferenceException>();
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _stateRepository.AddStateAsync(entityStateACObj));

        }
        /// <summary>
        /// Test case to verify AddStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddStateAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing

            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityState> stateList = SetEntityStateListInitialData();

            EntityStateAC entityStateAcObj = new EntityStateAC()
            {
                Id = Guid.NewGuid(),
                StateName = "state-1",
                EntityId = Guid.Parse(entityId1),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                CountryName = "Peru",
                RegionName = "region-1"

            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(stateList.Any(x => !x.IsDeleted && x.ProvinceState.Name.ToLower() == entityStateAcObj.StateName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _stateRepository.AddStateAsync(entityStateAcObj));

        }

        /// <summary>
        /// Test case to verify AddStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddStateAsync_AddnewData()
        {
            //create all mock objects for testing

            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityState> entityStateList = SetEntityStateListInitialData();
            var regionList = SetRegionListInitialData();
            var countryList = SetCountryListInitialData();
            var entityCountryList = SetEntityCountryListInitialData();
            EntityStateAC entityStateAcObj = new EntityStateAC()
            {
                Id = Guid.NewGuid(),
                StateName = "state-2",
                EntityId = Guid.Parse(entityId1),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                CountryName = "daresalam",
                RegionName = "region-1"

            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(entityStateList.Any(x => !x.IsDeleted && x.ProvinceState.Name.ToLower() == entityStateAcObj.StateName.ToLower()));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                            .Returns(regionList.Where(x => !x.IsDeleted && x.Id == Guid.Parse(regionId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                           .Returns(countryList.Where(x => !x.IsDeleted && x.Id == Guid.Parse(countryId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                           .ReturnsAsync(entityCountryList.FirstOrDefault(x => !x.IsDeleted && x.Id == Guid.Parse(countryId)));
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<ProvinceState>())).ReturnsAsync(SetProvinceStateInitialData());
            var dbData = new EntityState()
            {
                Id = Guid.NewGuid(),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                EntityId = (Guid)entityStateAcObj.EntityId,
                CreatedDateTime = DateTime.UtcNow,
                EntityCountry = null,
                ProvinceState = null
            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<EntityState>())).ReturnsAsync(dbData);


            var result = await _stateRepository.AddStateAsync(entityStateAcObj);
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<EntityState>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test case to verify UpdateStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateStateAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing

            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityState> entityStateList = SetEntityStateListInitialData();
            List<Country> countryList = SetCountryListInitialData();
            EntityStateAC entityStateACObj = new EntityStateAC()
            {
                Id = Guid.NewGuid(),
                StateName = "state-1",
                EntityId = Guid.Parse(entityId1),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                CountryName = "daresalam",
                RegionName = "region-1"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Country>())).ReturnsAsync(SetCountryInitialData());
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(entityStateList.Any(x => !x.IsDeleted && x.ProvinceState.Name.ToLower() == entityStateACObj.StateName.ToLower()));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                            .Returns(countryList.Where(x => !x.IsDeleted && x.Id == entityStateACObj.EntityCountryId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                            .ReturnsAsync(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == entityStateACObj.EntityCountryId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                           .Returns(entityStateList.Where(x => !x.IsDeleted && x.Id == entityStateList[0].Id).AsQueryable().BuildMock().Object);
            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _stateRepository.UpdateStateAsync(entityStateACObj));

        }

        /// <summary>
        /// Test case to verify UpdateStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateStateAsync_UpdateData()
        {
            //create all mock objects for testing

            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityState> entityStateList = SetEntityStateListInitialData();
            List<Country> countryList = SetCountryListInitialData();
            EntityStateAC entityStateACObj = new EntityStateAC()
            {
                Id = Guid.NewGuid(),
                StateName = "state-2",
                EntityId = Guid.Parse(entityId1),
                StateId = Guid.Parse(stateId),
                EntityCountryId = Guid.Parse(entityCountryId),
                CountryName = "daresalam",
                RegionName = "region-1"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                           .ReturnsAsync(entityStateList.Any(x => !x.IsDeleted && x.ProvinceState.Name.ToLower() == entityStateACObj.StateName.ToLower()));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                            .Returns(countryList.Where(x => !x.IsDeleted && x.Id == entityStateACObj.EntityCountryId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                            .ReturnsAsync(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == entityStateACObj.EntityCountryId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityState, bool>>>()))
                           .Returns(entityStateList.Where(x => !x.IsDeleted && x.Id == entityStateList[0].Id).AsQueryable().BuildMock().Object);

            await _stateRepository.UpdateStateAsync(entityStateACObj);

            _dataRepository.Verify(v => v.Update(It.IsAny<EntityState>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }

        /// <summary>
        /// Test case to verify DeleteStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteStateAsync_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityState> entityStateList = SetEntityStateListInitialData();
            var primaryGeographicalAreaList=SetPrimaryGeographicalAreaInitialData();
            var countryList = SetCountryInitialData();
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<PrimaryGeographicalArea, bool>>>()))
                            .ReturnsAsync(primaryGeographicalAreaList.Count(x => !x.IsDeleted && x.EntityState.StateId == entityStateList[0].StateId));
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(entityStateList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityStateList[0].Id));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(entityStateList.FirstOrDefault(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityStateList[0].Id));
            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _stateRepository.DeleteStateAsync(entityStateList[0].StateId.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteStateAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteStateAsync_DeleteTypeSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityState> entityStateList = SetEntityStateListInitialData();

            var provinceStateList = SetProvinceStateListInitialData();

            var countryList = SetCountryInitialData();


            var entityStateIdFromList = entityStateList[2].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<ProvinceState, bool>>>()))
                            .ReturnsAsync(provinceStateList.Count(x => !x.IsDeleted && x.Id == entityStateIdFromList));

            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                           .ReturnsAsync(entityStateList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityStateIdFromList));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .ReturnsAsync(entityStateList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityStateIdFromList));
            await _stateRepository.DeleteStateAsync(entityStateIdFromList.ToString(), passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);
        }
        #endregion
    }
}
