using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository
{
    public interface IRcmProcessRepository
    {
        #region

        /// <summary>
        /// Get all Process for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all Process with its id and name only </returns>
        Task<List<RcmProcessAC>> GetAllProcessForDisplayInDropDownAsync(Guid entityId);

        /// <summary>
        /// Get RCM Process data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM Process</returns>
        Task<List<RcmProcessAC>> GetRcmProcessAsync(int? page, int pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get count of RCM process
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM Process</returns>
        Task<int> GetRcmProcessCountAsync(string selectedEntityId, string searchString = null);

        /// <summary>
        /// Get RCM Process details
        /// </summary>
        /// <param name="processId">Process id</param>
        /// <returns>count of RCM Process</returns>
        Task<RcmProcessAC> GetRcmProcessByIdAsync(string processId);

        /// <summary>
        /// Add Process detail
        /// </summary>
        /// <param name="rcmProcessAC">detail of Process</param>
        /// <returns>Task</returns>
        Task<RcmProcessAC> AddRcmProcess(RcmProcessAC rcmProcessAC);

        /// <summary>
        /// Update Process detail
        /// </summary>
        /// <param name="rcmProcessAC">detail of Process</param>
        /// <returns>Task</returns>
        Task<RcmProcessAC> UpdateRcmProcess(RcmProcessAC rcmProcessAC);

        /// <summary>
        /// Delete RCM Process detail
        /// <param name="processId">id of deleted Process</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteRcmProcess(string processId);


        /// <summary>
        /// Method for exporting Rcm process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
       Task<Tuple<string, MemoryStream>> ExportRcmProcessAsync(string entityId, int timeOffset);
        #endregion
    }
}
