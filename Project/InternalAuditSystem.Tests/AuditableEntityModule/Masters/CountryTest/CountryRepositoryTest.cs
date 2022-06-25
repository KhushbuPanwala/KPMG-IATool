using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.Repository.CountryRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore.Storage;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Test.AuditableEntityModule.Masters.CountryTest
{
    [Collection("Register Dependency")]
    public class CountryRepositoryTest : BaseTest
    {
        #region Private Variables
        private Mock<IDataRepository> _dataRepository;
        private ICountryRepository _countryRepository;
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
        private string stateId = "cb968329-db45-4b73-95ba-a7c043c4d118";
        #endregion

        #region Constructor
        public CountryRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _countryRepository = bootstrap.ServiceProvider.GetService<ICountryRepository>();
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
        private Pagination<EntityCountryAC> SetPaginationInitialData()
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

        private Country SetCountryInitialData()
        {
            Country countryObj = new Country()
            {
                Id= Guid.Parse(countryId),
                Name="Peru",
                CreatedDateTime=DateTime.UtcNow
            };
            return countryObj;
        }

        private List<Country> SetCountryListInitialData()
        {
            List < Country> countryListObj = new List<Country>()
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
            List < Region> regionListObj = new List<Region>()
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
        private EntityCountry SetEntityCountryInitialData()
        {

            var entityCountryObj = new EntityCountry()
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
                    Id=Guid.Parse(entityCountryId),
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
        #endregion

        #region Testing Methods Completed
        /// <summary>
        /// Test case to verify GetAllCountryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllCountryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_AuditableEntitityNull_ReturnNoElement()
        {
            var passedAuditableEntity = "";

            List<EntityCountry> countryList = SetEntityCountryListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _countryRepository.GetAllCountryPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(0, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllCountryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllCountryPageWiseAndSearchWiseAsync_PageIndexNull_PageSizeNull_SearchStringNull_ReturnNoElementfoundForAuditableEnitty()
        {
            var passedAuditableEntity = Guid.NewGuid().ToString();

            List<EntityCountry> countryList = SetEntityCountryListInitialData();

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);
            var paginationObj = SetPaginationInitialData();
            var result = await _countryRepository.GetAllCountryPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Empty(result.Items);
        }

        /// <summary>
        /// Test case to verify GetAllCountryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllCountryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityCountry> countryList = SetEntityCountryListInitialData();
            var paginationObj = SetPaginationInitialData();
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity).AsQueryable().BuildMock().Object);

            var result = await _countryRepository.GetAllCountryPageWiseAndSearchWiseAsync(paginationObj);

            Assert.Equal(3, result.TotalRecords);
        }

        /// <summary>
        /// Test case to verify GetAllCountryPageWiseAndSearchWiseAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetAllCountryPageWiseAndSearchWiseAsync_PageIndexAndPageSizeNotNull_SearchStringNotNull_ReturnsElementForAuditableEntity()
        {
            var passedAuditableEntity = entityId1;

            List<EntityCountry> countryList = SetEntityCountryListInitialData();
            List<EntityCountryAC> countryACList = SetEntityCountryACInitialData();
            var paginationObj = SetPaginationInitialData();
            searchString = "Peru";

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.Country.Name.ToLower().Contains(searchString.ToLower()))
                     .AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountryAC, bool>>>()))
                    .Returns(countryACList.Where(x => !x.IsDeleted && x.EntityId.ToString() == passedAuditableEntity && x.CountryName.ToLower().Contains(searchString.ToLower()))
                    .AsQueryable().BuildMock().Object);
            var result = await _countryRepository.GetAllCountryPageWiseAndSearchWiseAsync(paginationObj);

            Assert.NotNull(result.Items);
        }
        /// <summary>
        /// Test case to verify GetCountryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetCountryByIdAsync_ReturnCountryDetails()
        {
            var passedAuditableEntity = new Guid(entityId1);

            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();

            List<Country> countryList = SetCountryListInitialData();
            List<Region> regionList = SetRegionListInitialData();
            var countryId = entityCountryList[0].Id;

            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Returns(entityCountryList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id.ToString() == countryId.ToString()).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                     .Returns(countryList.Where(x => !x.IsDeleted).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Country, bool>>>()))
                    .Returns(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == countryId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                     .Returns(regionList.Where(x => !x.IsDeleted && x.EntityId == passedAuditableEntity).Distinct().OrderByDescending(x => x.CreatedDateTime).AsQueryable().BuildMock().Object);
            var result = await _countryRepository.GetCountryByIdAsync(countryId.ToString(), passedAuditableEntity.ToString());

            Assert.Equal(result.CountryName, countryList[0].Name);
        }

        /// <summary>
        /// Test case to verify GetCountryByIdAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task GetCountryByIdAsync_NoRegionFound_ThrowInvalidOperationException()
        {
            var passedAuditableEntity = new Guid(entityId1);
            
            var countryId = new Guid();

            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                     .Throws<InvalidOperationException>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                                        await _countryRepository.GetCountryByIdAsync(countryId.ToString(), passedAuditableEntity.ToString()));

        }
        /// <summary>
        /// Test case to verify AddCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddCountryAsync_EmptyAddObject_ThrowNullReferenceException()
        {
            //create all mock objects for testing
            var mockTransaction = new Mock<IDbContextTransaction>();
            EntityCountryAC entityCountryAcObj = new EntityCountryAC();

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .Throws<NullReferenceException>();
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                 .Throws<NullReferenceException>();
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
                                      await _countryRepository.AddCountryAsync(entityCountryAcObj));

        }
        /// <summary>
        /// Test case to verify AddCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddCountryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
           
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCountry> countryList = SetEntityCountryListInitialData();

            EntityCountryAC entityCountryACObj = new EntityCountryAC()
            {
                CountryName = "Peru"
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(countryList.Any(x => !x.IsDeleted && x.Country.Name.ToLower() == entityCountryACObj.CountryName.ToLower()));


            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _countryRepository.AddCountryAsync(entityCountryACObj));

        }

        /// <summary>
        /// Test case to verify AddCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task AddCountryAsync_AddnewData()
        {
            //create all mock objects for testing
            
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();
            var regionList=SetRegionListInitialData();
          var countryList = SetCountryListInitialData();
            EntityCountryAC entityCountryAcObj = new EntityCountryAC()
            {
                CountryName = "mongolia",
                CountryId=Guid.Parse(countryId),
                RegionId=Guid.Parse(regionId),
                EntityId=Guid.Parse(entityId1)
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(entityCountryList.Any(x => !x.IsDeleted &&  x.Country.Name.ToLower() == entityCountryAcObj.CountryName.ToLower()));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Region, bool>>>()))
                            .Returns(regionList.Where(x => !x.IsDeleted && x.Id == Guid.Parse(regionId)).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                           .Returns(countryList.Where(x => !x.IsDeleted && x.Id == Guid.Parse(countryId)).AsQueryable().BuildMock().Object);
            var dbData = new EntityCountry()
            {
                Id = Guid.NewGuid(),
                CountryId =(Guid) entityCountryAcObj.CountryId,
                RegionId= (Guid)entityCountryAcObj.RegionId,
                EntityId= (Guid)entityCountryAcObj.EntityId,
                CreatedDateTime = DateTime.UtcNow,
                Region = null,
                Country = null
            };

            _dataRepository.Setup(x => x.AddAsync(It.IsAny<EntityCountry>())).ReturnsAsync(dbData);

            var result = await _countryRepository.AddCountryAsync(entityCountryAcObj);
            Assert.NotNull(result);
            _dataRepository.Verify(x => x.AddAsync(It.IsAny<EntityCountry>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Test case to verify UpdateCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateCountryAsync_SameEntryAdd_ThrowDuplicateDataException()
        {
            //create all mock objects for testing
            
            var mockTransaction = new Mock<IDbContextTransaction>();

            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();
            List<Country> countryList = SetCountryListInitialData();
            EntityCountryAC entityCountryACObj = new EntityCountryAC()
            {
                CountryName = "Peru",
                Id=Guid.Parse(entityCountryId),
                CountryId= Guid.Parse(countryId),
                EntityId= Guid.Parse(entityId1),
                RegionId = Guid.Parse(regionId2)
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AddAsync(It.IsAny<Country>())).ReturnsAsync(SetCountryInitialData());
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                            .Returns(countryList.Where(x => !x.IsDeleted && x.Id == entityCountryACObj.CountryId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                            .ReturnsAsync(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == entityCountryACObj.CountryId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .Returns(entityCountryList.Where(x => !x.IsDeleted && x.Id == entityCountryACObj.Id).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(entityCountryList.Any(x => !x.IsDeleted && x.Country.Name.ToLower() == entityCountryACObj.CountryName.ToLower()));

            await Assert.ThrowsAsync<DuplicateDataException>(async () =>
                                      await _countryRepository.UpdateCountryAsync(entityCountryACObj));

        }

        /// <summary>
        /// Test case to verify UpdateCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task UpdateCountryAsync_UpdateData()
        {
            //create all mock objects for testing
            
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();
            List<Country> countryList = SetCountryListInitialData();
            EntityCountryAC entityCountryACObj = new EntityCountryAC()
            {
                CountryName = "mongoliya",
                Id = Guid.Parse(entityCountryId),
                CountryId = Guid.Parse(countryId),
                EntityId = Guid.Parse(entityId1),
                RegionId= Guid.Parse(regionId)
            };

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(entityCountryList.Any(x => !x.IsDeleted &&  x.Country.Name.ToLower() == entityCountryACObj.CountryName.ToLower()));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<Country, bool>>>()))
                            .Returns(countryList.Where(x => !x.IsDeleted && x.Id == entityCountryACObj.CountryId).AsQueryable().BuildMock().Object);
            _dataRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>()))
                            .ReturnsAsync(countryList.FirstOrDefault(x => !x.IsDeleted && x.Id == entityCountryACObj.CountryId));
            _dataRepository.Setup(x => x.Where(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                           .Returns(entityCountryList.Where(x => !x.IsDeleted && x.Id == entityCountryList[0].Id).AsQueryable().BuildMock().Object);


            await _countryRepository.UpdateCountryAsync(entityCountryACObj);

            _dataRepository.Verify(v => v.Update(It.IsAny<EntityCountry>()), Times.Once);
            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);


        }

        /// <summary>
        /// Test case to verify DeleteCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteCountryAsync_SameEntryAdd_ThrowDeleteLinkedDataException()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();

            var entityStateList = SetProvinceStateListInitialData();
            var countryList = SetCountryInitialData();
            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityState, bool>>>()))
                            .ReturnsAsync(entityStateList.Count(x => !x.IsDeleted  && x.CountryId == entityCountryList[0].CountryId));
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                            .ReturnsAsync(entityCountryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityCountryList[0].Id));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                           .ReturnsAsync(entityCountryList.FirstOrDefault(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityCountryList[0].Id));
            await Assert.ThrowsAsync<DeleteLinkedDataException>(async () =>
            await _countryRepository.DeleteCountryAsync(entityCountryList[0].Id.ToString(), passedAuditableEntity.ToString()));

        }

        /// <summary>
        /// Test case to verify DeleteCountryAsync
        /// </summary>
        /// <returns>Task</returns>
        [Fact]
        public async Task DeleteCountryAsync_DeleteTypeSuccessfully()
        {
            //create all mock objects for testing
            var passedAuditableEntity = new Guid(entityId1);
            var mockTransaction = new Mock<IDbContextTransaction>();
            List<EntityCountry> entityCountryList = SetEntityCountryListInitialData();

            var provinceStateList = SetProvinceStateListInitialData();
            
            var countryList = SetCountryInitialData();


            var entityCountryIdFromList = entityCountryList[2].Id;

            //mock db calls
            _dataRepository.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<ProvinceState, bool>>>()))
                            .ReturnsAsync(provinceStateList.Count(x => !x.IsDeleted  && x.Id == entityCountryIdFromList));

            _dataRepository.Setup(x => x.CountAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                           .ReturnsAsync(entityCountryList.Count(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id == entityCountryIdFromList));
            _dataRepository.Setup(x => x.FirstAsync(It.IsAny<Expression<Func<EntityCountry, bool>>>()))
                     .ReturnsAsync(entityCountryList.First(x => !x.IsDeleted && x.EntityId == passedAuditableEntity && x.Id== entityCountryIdFromList));
            await _countryRepository.DeleteCountryAsync(entityCountryIdFromList.ToString(), passedAuditableEntity.ToString());

            _dataRepository.Verify(v => v.SaveChangesAsync(), Times.Once);
        }
        #endregion
    }
}
