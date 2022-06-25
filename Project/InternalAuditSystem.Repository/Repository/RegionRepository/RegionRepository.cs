using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
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

namespace InternalAuditSystem.Repository.Repository.RegionRepository
{
   public class RegionRepository:IRegionRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid? currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public RegionRepository(IDataRepository dataRepository, IMapper mapper, IGlobalRepository globalRepository, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _httpContextAccessor = httpContextAccessor;

            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get all the region under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of regions under an auditable entity</returns>
        public async Task<Pagination<RegionAC>> GetAllRegionPageWiseAndSearchWiseAsync(Pagination<RegionAC> pagination)
        {
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            //return list as per search string 

            IQueryable<RegionAC> regionACList = _dataRepository.Where<Region>(x => !x.IsDeleted
           && x.EntityId == pagination.EntityId &&
           (!String.IsNullOrEmpty(pagination.searchText) ? x.Name.ToLower().Contains(pagination.searchText.ToLower()) : true))
           .Select(x =>
                                                new RegionAC
                                                {
                                                    Id=x.Id,
                                                    Name=x.Name,
                                                    EntityId=x.EntityId,
                                                    CreatedDateTime =x.CreatedDateTime
                                                });

            pagination.TotalRecords = regionACList.Count();

            pagination.Items = await regionACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            return pagination;

        }


        /// <summary>
        /// Get all the regions under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all regions </returns>
        public async Task<List<RegionAC>> GetAllRegionsByEntityIdAsync(Guid selectedEntityId)
        {
            var regionList = await _dataRepository.Where<Region>(x => !x.IsDeleted && x.EntityId == selectedEntityId).Distinct().OrderByDescending(x=>x.CreatedDateTime)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<RegionAC>>(regionList);
        }


        /// <summary>
        /// Get data of a particular region under an auditable entity
        /// </summary>
        /// <param name="regionId">Id of the region</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular region data</returns>
        public async Task<RegionAC> GetRegionByIdAsync(string regionId, string selectedEntityId)
        {
            var result = await _dataRepository.FirstAsync<Region>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == regionId);

            return _mapper.Map<RegionAC>(result);
        }

        /// <summary>
        /// Add new region under an auditable entity
        /// </summary>
        /// <param name="regionDetails">Details of region</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added region details</returns>
        public async Task<RegionAC> AddRegionAsync(RegionAC regionDetails, string selectedEntityId)
        {

            //check user already exist in db 
            if (!await _dataRepository.AnyAsync<Region>(x => !x.IsDeleted
            && x.EntityId.ToString() == selectedEntityId
            && x.Name.ToLower() == regionDetails.Name.ToLower()))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var regionData = new Region()
                        {
                            Name = regionDetails.Name,
                            EntityId = new Guid(selectedEntityId),
                            CreatedDateTime = DateTime.UtcNow,
                            CreatedBy=currentUserId
                        };

                        var addedregion = await _dataRepository.AddAsync<Region>(regionData);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<RegionAC>(addedregion);
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
                throw new DuplicateDataException(StringConstant.RegionFieldName, regionDetails.Name);
            }

        }

        /// <summary>
        /// Update region under an auditable entity
        /// </summary>
        /// <param name="updatedRegionDetails">Updated details of region</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateRegionAsync(RegionAC updatedRegionDetails, string selectedEntityId)
        {

            var dbRegionData = await _dataRepository.Where<Region>(x => !x.IsDeleted && x.Id == updatedRegionDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateRegion = dbRegionData.Name.ToLower() == updatedRegionDetails.Name.ToLower() ? true :
                !(await _dataRepository.AnyAsync<Region>(x => !x.IsDeleted && x.Name.ToLower() == updatedRegionDetails.Name.ToLower()));

            if (isToUpdateRegion)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        dbRegionData = _mapper.Map<Region>(updatedRegionDetails);
                        dbRegionData.UpdatedDateTime = DateTime.UtcNow;
                        dbRegionData.UpdatedBy = currentUserId;

                        _dataRepository.Update(dbRegionData);
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
                throw new DuplicateDataException(StringConstant.RegionFieldName, updatedRegionDetails.Name);
            }
        }

        /// <summary>
        /// Delete region from an auditable entity
        /// </summary>
        /// <param name="regionId">Id of the region that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteRegionAsync(string regionId, string selectedEntityId)
        {

            //check is this location is used in any active auditable entity or not 
            var totalPrimaryGeographicalAreaIncludeCurrentLocation = await _dataRepository.CountAsync<PrimaryGeographicalArea>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.RegionId.ToString() == regionId);

            var countryIncludeCurrentRegion = await _dataRepository.CountAsync<EntityCountry>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.RegionId==Guid.Parse(regionId));
            // if no enitty has this location as selected then do soft delete
            if (totalPrimaryGeographicalAreaIncludeCurrentLocation == 0 && countryIncludeCurrentRegion==0)
            {
                
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var dbRegionData = await _dataRepository.FirstAsync<Region>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == regionId);

                        dbRegionData.IsDeleted = true;
                        dbRegionData.UpdatedDateTime = DateTime.UtcNow;
                        dbRegionData.UpdatedBy = currentUserId;

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
            //throw exception if this region is currently selected in any plan under the selected entity
            else
            {
                if (totalPrimaryGeographicalAreaIncludeCurrentLocation > 0)
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.PrimaryGeopgraphicalAreaModuleName);
                else
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, StringConstant.CountryModuleName);
            }


        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfRegionSearchStringWiseAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<Region>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Region>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId);
            }

            return totalRecords;
        }

        #region export to excel  auditable entity region
        /// <summary>
        /// Method for exporting  auditable entity region
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportEntityRegionsAsync(string entityId, int timeOffset)
        {
            var regionList = await _dataRepository.Where<Region>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                          .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var regionACList = _mapper.Map<List<RegionAC>>(regionList);
            if (regionACList.Count == 0)
            {
                RegionAC regionAC = new RegionAC();
                regionACList.Add(regionAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            regionACList.ForEach(x =>
            {
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });


            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(regionACList, StringConstant.RegionFieldName + "(" + entityName + ")");
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
