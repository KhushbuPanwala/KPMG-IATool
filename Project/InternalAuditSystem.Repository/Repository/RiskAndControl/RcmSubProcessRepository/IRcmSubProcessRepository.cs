using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository
{
    public interface IRcmSubProcessRepository
    {
        /// <summary>
        /// Get all sub Processes for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all sub Process with its id and name only </returns>
        Task<List<RcmSubProcessAC>> GetAllSubProcessesForDisplayInDropDownAsync(Guid entityId);

        /// <summary>
        /// Get RCM Sub Process
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM Sub Process</returns>
        Task<List<RcmSubProcessAC>> GetRcmSubProcessAsync(int? pageIndex, int pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get count of RCM Sub Process
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM Sub Process</returns>
        Task<int> GetRcmSubProcessCountAsync(string selectedEntityId, string searchString);

        /// <summary>
        /// Get RCM SubProcess details
        /// </summary>
        /// <param name="subProcessId">SubProcess id</param>
        /// <returns>count of RCM SubProcess</returns>
        Task<RcmSubProcessAC> GetRcmSubProcessByIdAsync(string subProcessId);

        /// <summary>
        /// Add SubProcess detail
        /// </summary>
        /// <param name="rcmSubProcessAC">detail of SubProcess</param>
        /// <returns>Task</returns>
        Task<RcmSubProcessAC> AddRcmSubProcess(RcmSubProcessAC rcmSubProcessAC);

        /// <summary>
        /// Update SubProcess detail
        /// </summary>
        /// <param name="rcmSubProcessAC">detail of SubProcess</param>
        /// <returns>Task</returns>
        Task<RcmSubProcessAC> UpdateRcmSubProcess(RcmSubProcessAC rcmSubProcessAC);

        /// <summary>
        /// Delete RCM SubProcess detail
        /// <param name="subProcessId">id of deleted SubProcess</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteRcmSubProcess(string subProcessId);


        /// <summary>
        /// Method for exporting Rcm subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportRcmSubProcessAsync(string entityId, int timeOffset);

    }
}
