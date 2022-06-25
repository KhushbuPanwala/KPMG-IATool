using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.LocationRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository
{
    public class PrimaryGeographicalAreaRepository : IPrimaryGeographicalAreaRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public ILocationRepository _locationRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public PrimaryGeographicalAreaRepository(
            IDataRepository dataRepository,
            IMapper mapper, IGlobalRepository globalRepository,
           ILocationRepository locationRepository,
           IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _locationRepository = locationRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId =Guid.Parse( _httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Update PrimaryGeographicalArea
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea AC object</param>
        /// <returns>Void</returns>
        public async Task UpdatePrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC)
        {
            PrimaryGeographicalArea primaryGeographicalArea = new PrimaryGeographicalArea();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    primaryGeographicalArea = _mapper.Map<PrimaryGeographicalArea>(primaryGeographicalAreaAC);

                    primaryGeographicalArea.UpdatedDateTime = DateTime.UtcNow;
                    primaryGeographicalArea.UpdatedBy = currentUserId;

                    _dataRepository.Update(primaryGeographicalArea);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Add PrimaryGeographicalArea to database
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea model to be added</param>
        /// <returns>Added PrimaryGeographicalArea</returns>
        public async Task<PrimaryGeographicalAreaAC> AddPrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    PrimaryGeographicalArea primaryGeographicalArea = new PrimaryGeographicalArea();

                    primaryGeographicalAreaAC.Id = new Guid();
                    primaryGeographicalArea = _mapper.Map<PrimaryGeographicalArea>(primaryGeographicalAreaAC);

                    primaryGeographicalArea.CreatedDateTime = DateTime.UtcNow;
                    primaryGeographicalArea.CreatedBy = currentUserId;

                    await _dataRepository.AddAsync(primaryGeographicalArea);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return _mapper.Map<PrimaryGeographicalAreaAC>(primaryGeographicalArea);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// Get country list by region
        /// </summary>
        /// <param name="regionId">Region Id</param>
        /// <param name="entityId">Entity Id</param>
        /// <returns>List of countries for dropdown</returns>
        public async Task<List<CountryAC>> GetCountryACListByRegionAsync(Guid regionId, Guid entityId)
        {
            return await _dataRepository.Where<EntityCountry>(x => x.EntityId == entityId && x.RegionId == regionId && !x.IsDeleted)
                                                        .Include(x => x.Country)
                                                        .Select(x => new CountryAC
                                                        {
                                                            Id = x.Id,
                                                            EntityCountryId = x.Id,
                                                            Name = x.Country.Name
                                                        }).ToListAsync();
        }

        /// <summary>
        /// Get state list by country selection
        /// </summary>
        /// <param name="countryId">Entity country id</param>
        /// <param name="entityId">Selected entity id</param>
        /// <returns>List of state</returns>
        public async Task<List<ProvinceStateAC>> GetStateACListByCountryAsync(Guid countryId, Guid entityId)
        {
            return await _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => x.EntityId == entityId
                                                        && x.EntityCountryId == countryId && !x.IsDeleted)
                                                        .Include(x => x.ProvinceState)
                                                        .Select(x => new ProvinceStateAC
                                                        {
                                                            Id = x.Id,
                                                            Name = x.ProvinceState.Name
                                                        }).ToListAsync();

        }

        /// <summary>
        /// Get PrimaryGeographicalArea List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        public async Task<Pagination<PrimaryGeographicalAreaAC>> GetPrimaryGeographicalAreaListAsync(Pagination<PrimaryGeographicalAreaAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<PrimaryGeographicalAreaAC> primaryGeographicalAreaList = _dataRepository.Where<PrimaryGeographicalArea>(x =>
                                                                 x.EntityId == pagination.EntityId
                                                                 && (!String.IsNullOrEmpty(pagination.searchText) ? x.Region.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                                                 && !x.IsDeleted).Include(x => x.EntityCountry.Country)
                                                                 .Include(x => x.Region)
                                                                 .Include(x => x.EntityState)
                                                                 .Include(x => x.Location)
                                                                 .Select(x => new PrimaryGeographicalAreaAC
                                                                 {
                                                                     Id = x.Id,
                                                                     RegionId = x.RegionId,
                                                                     RegionString = x.Region.Name,
                                                                     EntityCountryId = x.EntityCountryId,
                                                                     CountryString = x.EntityCountry.Country.Name,
                                                                     EntityStateId = x.EntityStateId,
                                                                     StateString = x.EntityState.ProvinceState.Name,
                                                                     LocationId = x.LocationId,
                                                                     LocationString = x.Location.Name,
                                                                     CreatedDateTime = x.CreatedDateTime
                                                                 });

            //Get total count
            pagination.TotalRecords = primaryGeographicalAreaList.Count();

            if (pagination.PageSize == 0)//Get all records
            {
                pagination.Items = await primaryGeographicalAreaList.OrderByDescending(x => x.CreatedDateTime).Skip(0).Take(pagination.TotalRecords).ToListAsync();
            }
            else
            {
                pagination.Items = await primaryGeographicalAreaList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            }
            return pagination;
        }

        /// <summary>
        /// Get primary geographical area details of edit and add 
        /// </summary>
        /// <param name="id">PrimaryGeographicalAreaid if on edit</param>
        /// <param name="entityId">Entity id</param>
        /// <returns>PrimaryGeographicalAreaAC</returns>
        public async Task<PrimaryGeographicalAreaAC> GetPrimaryGeographicalAreaDetailsAsync(Guid? id, Guid? entityId = null)
        {
            PrimaryGeographicalAreaAC primaryGeographicalAreaAC = new PrimaryGeographicalAreaAC();
            if (id != null)
            {
                PrimaryGeographicalArea primaryGeographicalArea = _dataRepository.FirstOrDefault<PrimaryGeographicalArea>(x => x.Id == id && !x.IsDeleted);
                primaryGeographicalAreaAC = _mapper.Map<PrimaryGeographicalAreaAC>(primaryGeographicalArea);
            }
            if (entityId != null)
            {
                List<Region> regionList = await _dataRepository.Where<Region>(x => x.EntityId == entityId && !x.IsDeleted).ToListAsync();
                primaryGeographicalAreaAC.RegionACList = _mapper.Map<List<RegionAC>>(regionList);

                primaryGeographicalAreaAC.LocationACList = await _locationRepository.GetAllLocationsByEntityIdAsync(entityId ?? new Guid());
            }

            return primaryGeographicalAreaAC;
        }

        /// <summary>
        /// Delete PrimaryGeographicalArea
        /// </summary>
        /// <param name="id">PrimaryGeographicalArea id</param>
        /// <returns>Void</returns>
        public async Task DeletePrimaryGeographicalAreaAync(Guid id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    PrimaryGeographicalArea primaryGeographicalArea = await _dataRepository.FirstOrDefaultAsync<PrimaryGeographicalArea>(x => x.Id == id && !x.IsDeleted);
                    primaryGeographicalArea.IsDeleted = true;
                    primaryGeographicalArea.UpdatedBy = currentUserId;
                    primaryGeographicalArea.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<PrimaryGeographicalArea>(primaryGeographicalArea);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to Add Primary Geographical Area In New Version
        /// </summary>
        /// <param name="primaryGeographicalAreaACList">PrimaryGeographicalAreaAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        public async Task AddPrimaryGeographicalAreaInNewVersionAsync(List<PrimaryGeographicalAreaAC> primaryGeographicalAreaACList, Guid versionId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    List<PrimaryGeographicalArea> primaryGeographicalAreaList = _mapper.Map<List<PrimaryGeographicalArea>>(primaryGeographicalAreaACList);

                    for (var i = 0; i < primaryGeographicalAreaList.Count(); i++)
                    {
                        primaryGeographicalAreaList[i].Id = new Guid();
                        primaryGeographicalAreaList[i].EntityId = versionId;
                        primaryGeographicalAreaList[i].CreatedBy = currentUserId;
                        primaryGeographicalAreaList[i].CreatedDateTime = DateTime.UtcNow;
                    }
                    await _dataRepository.AddRangeAsync(primaryGeographicalAreaList);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }



        #endregion
    }
}
