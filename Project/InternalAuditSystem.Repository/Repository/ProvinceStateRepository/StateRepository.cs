using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Repository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Repository.CountryRepository;
using Microsoft.AspNetCore.Http;
using System.IO;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;

namespace InternalAuditSystem.Repository.Repository.ProvinceStateRepository
{
    public class StateRepository : IStateRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        private ICountryRepository _countryRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public StateRepository(IDataRepository dataRepository, IMapper mapper, IGlobalRepository globalRepository, ICountryRepository countryRepository,
            IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _countryRepository = countryRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the state under a country with searchstring wise , without
        /// search string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of states under a country</returns>
        public async Task<Pagination<EntityStateAC>> GetAllStatePageWiseAndSearchWiseAsync(Pagination<EntityStateAC> pagination)
        {
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            //return list as per search string 

            IQueryable<EntityStateAC> stateACList = _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted
           && x.EntityId == pagination.EntityId &&
           (!String.IsNullOrEmpty(pagination.searchText) ? x.ProvinceState.Name.ToLower().Contains(pagination.searchText.ToLower()) : true))
           .Include(x => x.EntityCountry).Include(x => x.ProvinceState).Select(x =>
                                                    new EntityStateAC
                                                    {
                                                        Id = x.Id,
                                                        EntityId = x.EntityId,
                                                        EntityCountryId = x.EntityCountryId,
                                                        StateId = x.StateId,
                                                        StateName = x.ProvinceState.Name,
                                                        CreatedDateTime = x.CreatedDateTime,
                                                        CountryName = x.EntityCountry.Country.Name,
                                                        RegionName = x.EntityCountry.Region.Name
                                                    });

            pagination.TotalRecords = stateACList.Count();

            pagination.Items = await stateACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            return pagination;

        }

        /// <summary>
        /// Get data of a particular state under a country
        /// </summary>
        /// <param name="stateId">Id of the state</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular state data</returns>
        public async Task<EntityStateAC> GetStateByIdAsync(string stateId, string selectedEntityId)
        {
            try
            {
                var entityObj = new DomailModel.Models.AuditableEntityModels.EntityState();
                var stateObjList = new List<ProvinceState>();
                var countryList = new List<EntityCountry>();
                if (stateId != null)
                {
                    entityObj = await _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.Id.ToString() == stateId && x.EntityId == Guid.Parse(selectedEntityId)).Include(x => x.ProvinceState).FirstOrDefaultAsync();
                }

                countryList = await _dataRepository.Where<EntityCountry>(x => !x.IsDeleted && x.EntityId == Guid.Parse(selectedEntityId)).Include(x => x.Region).Include(x => x.Country).Distinct().ToListAsync();
                var entityStateAC = _mapper.Map<EntityStateAC>(entityObj);
                entityStateAC.CountryACList = _mapper.Map<List<EntityCountryAC>>(countryList);
                entityStateAC.StateName = stateId != null ? entityObj.ProvinceState.Name : "";
                return entityStateAC;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to add state
        /// </summary>
        /// <param name="stateDetails">Details of state</param>
        /// <returns>Added object of state</returns>
        public async Task<EntityStateAC> AddStateAsync(EntityStateAC stateDetails)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var getDbStateData = await _dataRepository.FirstOrDefaultAsync<ProvinceState>(x => !x.IsDeleted && x.Name.ToLower() == stateDetails.StateName.Trim().ToLower());
                    var getData = await _dataRepository.FirstOrDefaultAsync<EntityCountry>(x => x.Id == stateDetails.EntityCountryId);
                    ProvinceState addedNewStateData = new ProvinceState();
                    if (getDbStateData == null && stateDetails.StateId == null)
                    {
                        var countryDataToBeAdded = new ProvinceState()
                        {
                            Name = stateDetails.StateName,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            CountryId = getData.CountryId,
                            Country =null
                        };
                        addedNewStateData = await _dataRepository.AddAsync<ProvinceState>(countryDataToBeAdded);
                        await _dataRepository.SaveChangesAsync();
                    }
                    //check state already exist in db 
                    if (!await _dataRepository.AnyAsync<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted

                    && x.ProvinceState.Name.ToLower() == stateDetails.StateName.ToLower() && x.EntityId == stateDetails.EntityId))
                    {



                        var stateData = new DomailModel.Models.AuditableEntityModels.EntityState()
                        {
                            EntityId = (Guid)stateDetails.EntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            EntityCountryId = (Guid)stateDetails.EntityCountryId,
                            StateId = getDbStateData != null ? getDbStateData.Id : addedNewStateData.Id,
                            EntityCountry = null,
                            ProvinceState = null
                        };

                        var addedState = await _dataRepository.AddAsync<DomailModel.Models.AuditableEntityModels.EntityState>(stateData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        var addedStateAC = _mapper.Map<EntityStateAC>(addedState);
                        return addedStateAC;
                    }
                    else
                    {
                        throw new DuplicateDataException(StringConstant.StateModuleName, stateDetails.StateName);
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
        /// Update state under an auditable entity
        /// </summary>
        /// <param name="updatedStateDetails">Updated details of state</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateStateAsync(EntityStateAC updatedStateDetails)
        {
            var getDbStateData = await _dataRepository.FirstOrDefaultAsync<ProvinceState>(x => !x.IsDeleted && x.Name.ToLower() == updatedStateDetails.StateName.Trim().ToLower());
            if (getDbStateData != null)
                updatedStateDetails.StateId = getDbStateData.Id;
            var getData = await _dataRepository.FirstOrDefaultAsync<EntityCountry>(x => x.Id == updatedStateDetails.EntityCountryId);
            ProvinceState addedNewStateData = new ProvinceState();
            if (getDbStateData == null && updatedStateDetails.StateId == null)
            {
                var countryDataToBeAdded = new ProvinceState()
                {
                    Name = updatedStateDetails.StateName,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = currentUserId,
                    CountryId = getData.CountryId,
                    Country = null

                };
                addedNewStateData = await _dataRepository.AddAsync<ProvinceState>(countryDataToBeAdded);
                await _dataRepository.SaveChangesAsync();
                updatedStateDetails.StateId = addedNewStateData.Id;
            }
            else if (getDbStateData != null && updatedStateDetails.StateId == null)
            {
                //throw Country already exists in entity
                throw new DuplicateDataException(StringConstant.StateModuleName, updatedStateDetails.StateName);
            }

            var dbStateData = await _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.Id == updatedStateDetails.Id).Include(x => x.EntityCountry).Include(x => x.ProvinceState).AsNoTracking().FirstOrDefaultAsync();

            if (dbStateData.EntityCountryId == updatedStateDetails.EntityCountryId)
            {
                // check if updated state is in entity or not
                if (!await _dataRepository.AnyAsync<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.EntityId == dbStateData.EntityId && x.StateId == updatedStateDetails.StateId))
                {
                    dbStateData.StateId = (Guid)updatedStateDetails.StateId;
                }
                else
                {
                    //throw Country already exists in entity
                    throw new DuplicateDataException(StringConstant.StateModuleName, updatedStateDetails.StateName);
                }
            }
            else
            {
                if (!await _dataRepository.AnyAsync<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.EntityId == dbStateData.EntityId &&
                 x.EntityCountryId == updatedStateDetails.EntityCountryId && x.StateId == updatedStateDetails.StateId))
                {
                    dbStateData.EntityCountryId = (Guid)updatedStateDetails.EntityCountryId;
                    dbStateData.StateId = (Guid)updatedStateDetails.StateId;
                }
                else
                {
                    //throw Country within this region already exist in entity
                    throw new DuplicateDataException(StringConstant.StateModuleName, updatedStateDetails.StateName, updatedStateDetails.RegionName);
                }
            }
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    dbStateData.UpdatedDateTime = DateTime.UtcNow;
                    dbStateData.UpdatedBy = currentUserId;
                    dbStateData.EntityCountry = null;
                    dbStateData.ProvinceState = null;

                    _dataRepository.Update(dbStateData);
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
        /// Method to delete state
        /// </summary>
        /// <param name="StateId">Id of state</param>
        /// <param name="selectedEntityId">selected entityId</param>
        /// <returns>Task</returns>
        public async Task DeleteStateAsync(string StateId, string selectedEntityId)
        {

            //check is this PrimaryGeographicalArea is used in any active auditable entity or not 
            var totalProvinceStateInPrimaryGeographicalArea = await _dataRepository.CountAsync<PrimaryGeographicalArea>(x => !x.IsDeleted && x.EntityStateId.ToString() == StateId && x.EntityId == Guid.Parse(selectedEntityId));

            // if no enitty has this location as selected then do soft delete
            if (totalProvinceStateInPrimaryGeographicalArea == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbStateData = await _dataRepository.FirstAsync<DomailModel.Models.AuditableEntityModels.EntityState>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == StateId);

                        dbStateData.IsDeleted = true;
                        dbStateData.UpdatedDateTime = DateTime.UtcNow;
                        dbStateData.UpdatedBy = currentUserId;
                        dbStateData.UserUpdatedBy = null;
                        dbStateData.EntityCountry = null;
                        dbStateData.AuditableEntity = null;

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
            //throw exception if this state is currently selected in any plan under the selected entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.PrimaryGeopgraphicalAreaModuleName);
            }


        }

        /// <summary>
        /// Get all states based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityStateAC</returns>
        public async Task<EntityStateAC> GetAllStatesBasedOnSearchAsync(string searchString, string selectedEntityId)
        {
            EntityStateAC entityStateAC = new EntityStateAC();
            List<ProvinceStateAC> allStates = new List<ProvinceStateAC>();

            allStates = await _dataRepository.Where<ProvinceState>(x => !String.IsNullOrEmpty(searchString) ? x.Name.ToLower().Contains(searchString.ToLower()) || x.Name.StartsWith(searchString.ToLower()) : true)
                         .Select(x => new ProvinceStateAC
                         {
                             Name = x.Name,
                             Id = x.Id

                         }).Distinct().ToListAsync();
            entityStateAC.StateACList = allStates;
            entityStateAC.EntityId = Guid.Parse(selectedEntityId);
            return entityStateAC;
        }

        /// <summary>
        /// Method for exporting states
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportStatesAsync(string entityId, int timeOffset)
        {
            var stateList = await _dataRepository.Where<DomailModel.Models.AuditableEntityModels.EntityState>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).Include(c => c.EntityCountry).ThenInclude(x => x.Country).Include(c => c.ProvinceState).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var stateACList = _mapper.Map<List<EntityStateAC>>(stateList);
            if (stateACList.Count == 0)
            {
                EntityStateAC entityStateAC = new EntityStateAC();
                stateACList.Add(entityStateAC);
            }
            HashSet<Guid> regionIds = new HashSet<Guid>(stateList.Select(a => a.EntityCountry.RegionId));
            var getRegionList = await _dataRepository.Where<Region>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && regionIds.Contains(x.Id)).ToListAsync();
            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            stateACList.ForEach(x =>
            {
                x.CountryName = x.Id != null ? stateList.FirstOrDefault(a => a.StateId == x.StateId)?.EntityCountry.Country.Name : string.Empty;
                x.EntityName = x.Id != null ? stateList.FirstOrDefault(a => a.StateId == x.StateId)?.AuditableEntity.Name : string.Empty;
                x.StateName = x.Id != null ? stateList.FirstOrDefault(a => a.StateId == x.StateId)?.ProvinceState.Name : string.Empty;
                x.RegionName = x.Id != null && getRegionList.Count > 0 ? getRegionList.FirstOrDefault(a => a.Id == stateList.FirstOrDefault(a => a.StateId == x.StateId)?.EntityCountry.RegionId)?.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(stateACList, StringConstant.StateModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
