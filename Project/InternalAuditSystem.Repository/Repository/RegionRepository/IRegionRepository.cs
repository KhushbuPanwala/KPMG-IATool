using InternalAuditSystem.Repository.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RegionRepository
{
   public interface IRegionRepository
    {
        /// <summary>
        /// Get all the region under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of regions under an auditable entity</returns>
        Task<Pagination<RegionAC>> GetAllRegionPageWiseAndSearchWiseAsync(Pagination<RegionAC> pagination);

        /// <summary>
        /// Get all the regions under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all regions </returns>
        Task<List<RegionAC>> GetAllRegionsByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular region under an auditable entity
        /// </summary>
        /// <param name="regionId">Id of the region</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular region data</returns>
        Task<RegionAC> GetRegionByIdAsync(string regionId, string selectedEntityId);

        /// <summary>
        /// Add new region under an auditable entity
        /// </summary>
        /// <param name="regionDetails">Details of region</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added region details</returns>
        Task<RegionAC> AddRegionAsync(RegionAC regionDetails, string selectedEntityId);

        /// <summary>
        /// Update region under an auditable entity
        /// </summary>
        /// <param name="updatedRegionDetails">Updated details of region</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateRegionAsync(RegionAC updatedRegionDetails, string selectedEntityId);

        /// <summary>
        /// Delete region from an auditable entity
        /// </summary>
        /// <param name="regionId">Id of the region that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteRegionAsync(string regionId, string selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfRegionSearchStringWiseAsync(string searchString, string selectedEntityId);

        /// <summary>
        /// Method for exporting  auditable entity region
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityRegionsAsync(string entityId, int timeOffset);
    }
}
