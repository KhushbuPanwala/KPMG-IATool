using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository
{
    public interface IAuditProcessSubProcessRepository
    {
        #region Audit Process
        /// <summary>
        /// Get all the audit processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit processes under an auditable entity</returns>
        Task<List<ProcessAC>> GetAllAuditProcessesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);
        
        /// <summary>
        /// Get alll the audit processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit processes</returns>
        Task<List<ProcessAC>> GetOnlyProcessesByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfAuditProcessSearchStringWiseAsync(string searchString, string selectedEntityId);
        #endregion

        #region Audit SubProcess
        /// <summary>
        /// Get all the audit sub-processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit sub-processes under an auditable entity</returns>
        Task<List<ProcessAC>> GetAllAuditSubProcessesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get alll the audit sub-processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit sub-processes </returns>
        Task<List<ProcessAC>> GetAllAuditSubProcessesByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfAuditSubProcessSearchStringWiseAsync(string searchString, string selectedEntityId);
        #endregion

        #region Common method for Process & SubProcess
        /// <summary>
        /// Get data of a particular audit process or sub-process under an auditable entity
        /// </summary>
        /// <param name="auditProcessOrSubProcessId">Id of the audit process or subprocess</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit  audit process or sub-process data</returns>
        Task<ProcessAC> GetAuditProcessOrSubProcessByIdAsync(string auditProcessOrSubProcessId, string selectedEntityId);

        /// <summary>
        /// Add new audit process/sub-process under an auditable entity
        /// </summary>
        /// <param name="auditProcessOrSubProcessDetails">Details of audit process/sub-process/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit process/sub-process details</returns>
        Task<ProcessAC> AddAuditProcessOrSubProcessAsync(ProcessAC auditProcessOrSubProcessDetails, string selectedEntityId);

        /// <summary>
        /// Update audit process/sub-process under an auditable entity
        /// </summary>
        /// <param name="updatedProcessOrSubProcessDetails">Updated details of audit process/sub-process</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateAuditProcessOrSubProcessAsync(ProcessAC updatedProcessOrSubProcessDetails, string selectedEntityId);

        /// <summary>
        /// Delete audit process/subprocess from an auditable entity
        /// </summary>
        /// <param name="processORSubProcessId">Id of the audit process/subprocess that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteAuditProcessOrSubProcessAsync(string processORSubProcessId, string selectedEntityId);

        /// <summary>
        /// Get all the audit processes & sub processes under an auditable enitty
        /// </summary>
        /// <param name="entityId">Selected auditable Id</param>
        /// <returns>List of all processes & sub processes</returns>
        Task<List<ProcessAC>> GetAllProcessSubProcessesByEntityIdAsync(Guid entityId);

        /// <summary>
        /// Method for exporting audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditProcessesAsync(string entityId, int timeOffset);

        /// <summary>
        /// Method for exporting audit subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditSubProcessesAsync(string entityId, int timeOffset);
        #endregion

    }
}
