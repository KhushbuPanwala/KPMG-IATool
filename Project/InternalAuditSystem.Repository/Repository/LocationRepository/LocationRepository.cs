using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.LocationRepository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public LocationRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the locations under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of locations under an auditable entity</returns>
        public async Task<List<LocationAC>> GetAllLocationPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<Location> locationList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                locationList = await _dataRepository.Where<Location>(x => !x.IsDeleted && x.EntityId == selectedEntityId
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                locationList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit types data without searchstring - to show initial data on list page 

                locationList = await _dataRepository.Where<Location>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<LocationAC>>(locationList);

        }


        /// <summary>
        /// Get alll the locations under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all locations </returns>
        public async Task<List<LocationAC>> GetAllLocationsByEntityIdAsync(Guid selectedEntityId)
        {
            var locationList = await _dataRepository.Where<Location>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<LocationAC>>(locationList);
        }


        /// <summary>
        /// Get data of a particular location under an auditable entity
        /// </summary>
        /// <param name="locationId">Id of the location</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular location data</returns>
        public async Task<LocationAC> GetLocationByIdAsync(Guid locationId, Guid selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<Location>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Id == locationId);

            return _mapper.Map<LocationAC>(result);
        }

        /// <summary>
        /// Add new location under an auditable entity
        /// </summary>
        /// <param name="locationDetails">Details of location</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added location details</returns>
        public async Task<LocationAC> AddLocationAsync(LocationAC locationDetails, Guid selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<Location>(x => !x.IsDeleted
            && x.EntityId == selectedEntityId
            && x.Name.ToLower() == locationDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var locationData = new Location()
                        {
                            Name = locationDetails.Name,
                            EntityId = selectedEntityId,
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy=currentUserId,
                            UserCreatedBy= null,
                            AuditableEntity = null
                        };

                        var addedLocation = await _dataRepository.AddAsync<Location>(locationData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<LocationAC>(addedLocation);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                throw new DuplicateDataException(StringConstant.LocationFieldName, locationDetails.Name);
            }

        }

        /// <summary>
        /// Update location under an auditable entity
        /// </summary>
        /// <param name="updatedLocationDetails">Updated details of location</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateLocationAsync(LocationAC updatedLocationDetails, Guid selectedEntityId)
        {

            var dbLocationData = await _dataRepository.Where<Location>(x => !x.IsDeleted && x.Id == updatedLocationDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateLocation = dbLocationData.Name.ToLower() == updatedLocationDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<Location>(x => !x.IsDeleted && x.Name.ToLower() == updatedLocationDetails.Name.ToLower()));

            if (isToUpdateLocation )
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbLocationData = _mapper.Map<Location>(updatedLocationDetails);
                        dbLocationData.UpdatedDateTime = DateTime.UtcNow;
                        dbLocationData.UpdatedBy = currentUserId;
                        dbLocationData.UserUpdatedBy = null;
                        dbLocationData.AuditableEntity = null;

                        _dataRepository.Update(dbLocationData);
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
            else
            {
                throw new DuplicateDataException(StringConstant.LocationFieldName, updatedLocationDetails.Name);
            }
        }

        /// <summary>
        /// Delete location from an auditable entity
        /// </summary>
        /// <param name="locationId">Id of the location that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteLocationAsync(Guid locationId, Guid selectedEntityId)
        {

            //check is this location is used in any active auditable entity or not 
            var totalPrimaryGeographicalAreaIncludeCurrentLocation = await _dataRepository.CountAsync<PrimaryGeographicalArea>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.LocationId == locationId);

            // if no enitty has this location as selected then do soft delete
            if (totalPrimaryGeographicalAreaIncludeCurrentLocation == 0)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbLocationData = await _dataRepository.FirstAsync<Location>(x => x.EntityId == selectedEntityId && x.Id == locationId);

                        dbLocationData.IsDeleted = true;
                        dbLocationData.UpdatedDateTime = DateTime.UtcNow;
                        dbLocationData.UpdatedBy = currentUserId;

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
            //throw exception if this location is currently selected in any plan under the selected entity
            else
            {
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.PrimaryGeopgraphicalAreaModuleName);
            }


        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfLocationSearchStringWiseAsync(string searchString, Guid selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<Location>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Location>(x => !x.IsDeleted && x.EntityId == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity location
        /// <summary>
        /// Method for exporting  auditable entity location
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityLocationsAsync(string entityId, int timeOffset)
        {
            var locationList = await _dataRepository.Where<Location>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var locationACList = _mapper.Map<List<LocationAC>>(locationList);
            if (locationACList.Count == 0)
            {
                LocationAC locationAC = new LocationAC();
                locationACList.Add(locationAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            locationACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });


            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(locationACList, StringConstant.LocationFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
