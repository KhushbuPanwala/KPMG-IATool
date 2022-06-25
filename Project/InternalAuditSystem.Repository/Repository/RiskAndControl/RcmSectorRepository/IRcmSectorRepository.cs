using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository
{ 
    public interface IRcmSectorRepository
    {
        #region List RCM Sector

        /// <summary>
        /// Get all sectors for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all sectors with its id and name only </returns>
        Task<List<RcmSectorAC>> GetAllSectorForDisplayInDropDownAsync(Guid entityId);

        /// <summary>
        /// Get RCM Sector data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM Sectors</returns>
        Task<List<RcmSectorAC>> GetRcmSectorAsync(int? page, int pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get count of RCM sectors
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM Sectors</returns>
        Task<int> GetRcmSectorCountAsync(string selectedEntityId, string searchString);

        /// <summary>
        /// Get RCM Sector details
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="sectorId">Sector id</param>
        /// <returns>count of RCM Sector</returns>
        Task<RcmSectorAC> GetRcmSectorByIdAsync(string sectorId);

        /// <summary>
        /// Add Sector detail
        /// </summary>
        /// <param name="rcmSectorAC">detail of Sector</param>
        /// <returns>Task</returns>
        Task<RcmSectorAC> AddRcmSector(RcmSectorAC rcmSectorAC);

        /// <summary>
        /// Update Sector detail
        /// </summary>
        /// <param name="rcmSectorAC">detail of Sector</param>
        /// <returns>Task</returns>
        Task<RcmSectorAC> UpdateRcmSector(RcmSectorAC rcmSectorAC);

        /// <summary>
        /// Delete RCM Sector detail
        /// <param name="sectorId">id of deleted sector</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteRcmSector(string sectorId);


        /// <summary>
        /// Method for exporting Rcm sector
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportRcmSectorAsync(string entityId, int timeOffset);

        #endregion
    }
}
