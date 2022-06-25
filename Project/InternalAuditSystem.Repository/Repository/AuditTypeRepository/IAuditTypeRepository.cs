using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditTypeRepository
{
    public interface IAuditTypeRepository 
    {
        /// <summary>
        /// Get all the audit types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit types under an auditable entity</returns>
        Task<List<AuditTypeAC>> GetAllAuditTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get alll the audit types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit types </returns>
        Task<List<AuditTypeAC>> GetAllAuditTypeByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular audit type under an auditable entity
        /// </summary>
        /// <param name="auditTypeId">Id of the audit type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit type data</returns>
        Task<AuditTypeAC> GetAuditTypeByIdAsync(string auditTypeId, string selectedEntityId);

        /// <summary>
        /// Add new audit type under an auditable entity
        /// </summary>
        /// <param name="auditTypeDetails">Details of audit type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit type details</returns>
        Task<AuditTypeAC> AddAuditTypeAsync(AuditTypeAC auditTypeDetails, string selectedEntityId);

        /// <summary>
        /// Update audit type under an auditable entity
        /// </summary>
        /// <param name="updatedAuditTypeDetails">Updated details of audit type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateAuditTypeAsync(AuditTypeAC updatedAuditTypeDetails, string selectedEntityId);

        /// <summary>
        /// Delete audit type from an auditable entity
        /// </summary>
        /// <param name="auditTypeId">Id of the audit type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteAuditTypeAsync(string auditTypeId, string selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfAuditTypeSearchStringWiseAsync(string searchString, string selectedEntityId);

        /// <summary>
        /// Method for exporting audit types
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditTypesAsync(string entityId, int timeOffset);
    }
}
