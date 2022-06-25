using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.RegionRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.CountryRepository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IRegionRepository _regionRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public CountryRepository(IDataRepository dataRepository, IMapper mapper, IGlobalRepository globalRepository,
            IRegionRepository regionRepository, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _regionRepository = regionRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the country under a region with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of countries under a region</returns>
        public async Task<Pagination<EntityCountryAC>> GetAllCountryPageWiseAndSearchWiseAsync(Pagination<EntityCountryAC> pagination)
        {
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            //return list as per search string 

            IQueryable<EntityCountryAC> countryACList = _dataRepository.Where<EntityCountry>(x => !x.IsDeleted && x.EntityId == pagination.EntityId &&
                                                        (!String.IsNullOrEmpty(pagination.searchText) ? x.Country.Name.ToLower().Contains(pagination.searchText.ToLower()) : true))
                                                        .Include(x => x.Region).Include(x => x.Country).Select(x =>
                                                          new EntityCountryAC
                                                          {
                                                              Id = x.Id,
                                                              CountryId = x.CountryId,
                                                              RegionId = x.RegionId,
                                                              CreatedDateTime = x.CreatedDateTime,
                                                              CountryName = x.Country.Name,
                                                              RegionName = x.Region.Name

                                                          });

            pagination.TotalRecords = countryACList.Count();

            pagination.Items = await countryACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            return pagination;

        }

        /// <summary>
        /// Get data of a particular country under an auditable entity
        /// </summary>
        /// <param name="countryId">Id of the country</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular country data</returns>
        public async Task<EntityCountryAC> GetCountryByIdAsync(string countryId, string selectedEntityId)
        {
            var entityObj = new EntityCountry();
            var countryObjList = new List<Country>();
            if (countryId != null)
            {
                entityObj = await _dataRepository.Where<EntityCountry>(x => !x.IsDeleted && x.Id.ToString() == countryId && x.EntityId == Guid.Parse(selectedEntityId)).Include(x => x.Country).FirstOrDefaultAsync();
            }
            var entityCountryAC = _mapper.Map<EntityCountryAC>(entityObj);
            entityCountryAC.RegionACList = await _regionRepository.GetAllRegionsByEntityIdAsync(Guid.Parse(selectedEntityId));
            entityCountryAC.RegionName = entityCountryAC.RegionACList.FirstOrDefault(x => !x.IsDeleted && x.Id == entityObj.RegionId)?.Name;
            entityCountryAC.CountryName = countryId != null ? entityObj.Country.Name : "";
            return entityCountryAC;
        }

        /// <summary>
        /// Method to add country
        /// </summary>
        /// <param name="countryDetails">Details of country</param>
        /// <returns>Added object of country</returns>
        public async Task<EntityCountryAC> AddCountryAsync(EntityCountryAC countryDetails)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var getDbCountryData = await _dataRepository.FirstOrDefaultAsync<Country>(x => !x.IsDeleted && x.Name.ToLower() == countryDetails.CountryName.Trim().ToLower());
                    Country addedNewCountryData = new Country();
                    if (getDbCountryData == null && countryDetails.CountryId == null)
                    {
                        var countryDataToBeAdded = new Country()
                        {
                            Name = countryDetails.CountryName,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId
                        };
                        addedNewCountryData = await _dataRepository.AddAsync<Country>(countryDataToBeAdded);
                        await _dataRepository.SaveChangesAsync();
                    }
                    //check country already exist in db 
                    if (!await _dataRepository.AnyAsync<EntityCountry>(x => !x.IsDeleted

                    && x.Country.Name.ToLower() == countryDetails.CountryName.ToLower() && x.EntityId == countryDetails.EntityId))
                    {


                        var countryData = new EntityCountry()
                        {
                            RegionId = (Guid)countryDetails.RegionId,
                            CountryId = getDbCountryData != null ? getDbCountryData.Id : addedNewCountryData.Id,
                            EntityId = (Guid)countryDetails.EntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            Region = null,
                            Country = null

                        };

                        var addedCountry = await _dataRepository.AddAsync<EntityCountry>(countryData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        var addedCountryAC = _mapper.Map<EntityCountryAC>(addedCountry);
                        addedCountryAC.CountryName = countryDetails.CountryName;
                        addedCountryAC.RegionName = countryDetails.RegionName;
                        return addedCountryAC;


                    }

                    else
                    {
                        throw new DuplicateDataException(StringConstant.CountryModuleName, countryDetails.CountryName);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update country under an auditable entity
        /// </summary>
        /// <param name="updatedCountryDetails">Updated details of country</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateCountryAsync(EntityCountryAC updatedCountryDetails)
        {
            var getDbCountryData = await _dataRepository.FirstOrDefaultAsync<Country>(x => !x.IsDeleted && x.Name.ToLower() == updatedCountryDetails.CountryName.Trim().ToLower());
            Country addedNewCountryData = new Country();
            if (getDbCountryData != null)
                updatedCountryDetails.CountryId = getDbCountryData.Id;
            if (getDbCountryData == null && updatedCountryDetails.CountryId == null)
            {
                var countryDataToBeAdded = new Country()
                {
                    Name = updatedCountryDetails.CountryName,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = currentUserId
                };
                addedNewCountryData = await _dataRepository.AddAsync<Country>(countryDataToBeAdded);
                await _dataRepository.SaveChangesAsync();
                updatedCountryDetails.CountryId = addedNewCountryData.Id;
            }
            else if (getDbCountryData != null && updatedCountryDetails.CountryId == null)
            {
                //throw Country already exists in entity
                throw new DuplicateDataException(StringConstant.CountryModuleName, updatedCountryDetails.CountryName);
            }

            var dbCountryData = await _dataRepository.Where<EntityCountry>(x => !x.IsDeleted && x.Id == updatedCountryDetails.Id).Include(x => x.Country).AsNoTracking().FirstOrDefaultAsync();

            if (dbCountryData.RegionId == updatedCountryDetails.RegionId)
            {
                // check if updated country is in entity or not
                if (!await _dataRepository.AnyAsync<EntityCountry>(x => !x.IsDeleted && x.EntityId == dbCountryData.EntityId && x.CountryId == updatedCountryDetails.CountryId))
                {
                    dbCountryData.CountryId = (Guid)updatedCountryDetails.CountryId;
                }

                else
                {
                    //throw Country already exists in entity
                    throw new DuplicateDataException(StringConstant.CountryModuleName, updatedCountryDetails.CountryName);
                }
            }
            else
            {
                if (!await _dataRepository.AnyAsync<EntityCountry>(x => !x.IsDeleted && x.EntityId == dbCountryData.EntityId &&
                 x.RegionId == updatedCountryDetails.RegionId && x.CountryId == updatedCountryDetails.CountryId))
                {
                    dbCountryData.RegionId = (Guid)updatedCountryDetails.RegionId;
                    dbCountryData.CountryId = (Guid)updatedCountryDetails.CountryId;
                }
                else
                {
                    //throw Country within this region already exist in entity
                    throw new DuplicateDataException(StringConstant.CountryModuleName, updatedCountryDetails.CountryName, updatedCountryDetails.RegionName);
                }
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    dbCountryData.UpdatedDateTime = DateTime.UtcNow;
                    dbCountryData.UpdatedBy = currentUserId;
                    dbCountryData.Region = null;
                    dbCountryData.Country = null;

                    _dataRepository.Update(dbCountryData);
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
        /// Method to delete country
        /// </summary>
        /// <param name="countryId">Id of country</param>
        /// <param name="selectedEntityId">selected entityId</param>
        /// <returns>Task</returns>
        public async Task DeleteCountryAsync(string countryId, string selectedEntityId)
        {

            //check is this EntityState is used in any active auditable entity or not 
            var totalProvinceState = await _dataRepository.CountAsync<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.EntityCountryId.ToString() == countryId && x.EntityId == Guid.Parse(selectedEntityId));

            // if no enitty has this location as selected then do soft delete
            if (totalProvinceState == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbcountryData = await _dataRepository.FirstAsync<EntityCountry>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == countryId);

                        dbcountryData.IsDeleted = true;
                        dbcountryData.UpdatedDateTime = DateTime.UtcNow;
                        dbcountryData.UpdatedBy = currentUserId;

                        await _dataRepository.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            //throw exception if this country is currently selected in any plan under the selected entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.StateModuleName);
            }


        }
        /// <summary>
        /// Get all countries based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityCountryAC</returns>
        public async Task<EntityCountryAC> GetAllCountryBasedOnSearchAsync(string searchString, string selectedEntityId)
        {
            EntityCountryAC entityCountryAC = new EntityCountryAC();
            List<CountryAC> allcountries = new List<CountryAC>();

            allcountries = await _dataRepository.Where<Country>(x => !String.IsNullOrEmpty(searchString) ? x.Name.ToLower().Contains(searchString.ToLower()) || x.Name.StartsWith(searchString) : true)
                         .Select(x => new CountryAC
                         {
                             Name = x.Name,
                             Id = x.Id

                         }).Distinct().ToListAsync();
            entityCountryAC.CountryACList = allcountries;
            entityCountryAC.EntityId = Guid.Parse(selectedEntityId);
            return entityCountryAC;
        }
        /// <summary>
        /// Method for exporting countries
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportCountriesAsync(string entityId, int timeOffset)
        {
            var countryList = await _dataRepository.Where<EntityCountry>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).Include(x => x.Country).Include(c => c.Region).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var countryACList = _mapper.Map<List<EntityCountryAC>>(countryList);
            if (countryACList.Count == 0)
            {
                EntityCountryAC entityCountryAC = new EntityCountryAC();
                countryACList.Add(entityCountryAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            countryACList.ForEach(x =>
            {
                x.CountryName = x.Id != null ? x.CountryName : string.Empty;
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.RegionName = x.Id != null ? x.RegionName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(countryACList, StringConstant.CountryModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

