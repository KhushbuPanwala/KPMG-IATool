using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditCategoryRepository
{
    public interface IAuditCategoryRepository
    {
        /// <summary>
        /// Get all the audit categories under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit types under an auditable entity</returns>
        Task<List<AuditCategoryAC>> GetAllAuditCategoriesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get alll the audit categories under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit categories </returns>
        Task<List<AuditCategoryAC>> GetAllAuditCategoriesByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular audit category under an auditable entity
        /// </summary>
        /// <param name="auditCategoryId">Id of the audit category</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit category data</returns>
        Task<AuditCategoryAC> GetAuditCategoryByIdAsync(string auditCategoryId, string selectedEntityId);

        /// <summary>
        /// Add new audit category under an auditable entity
        /// </summary>
        /// <param name="auditCategoryDetails">Details of audit category/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit category details</returns>
        Task<AuditCategoryAC> AddAuditCategoryAsync(AuditCategoryAC auditCategoryDetails, string selectedEntityId);

        /// <summary>
        /// Update audit category under an auditable entity
        /// </summary>
        /// <param name="updatedAuditCategoryDetails">Updated details of audit category</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateAuditCategoryAsync(AuditCategoryAC updatedAuditCategoryDetails, string selectedEntityId);

        /// <summary>
        /// Delete audit category from an auditable entity
        /// </summary>
        /// <param name="auditCategoryId">Id of the audit category that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteAuditCategoryAsync(string auditCategoryId, string selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfAuditCategorySearchStringWiseAsync(string searchString, string selectedEntityId);

        /// <summary>
        /// Method for exporting audit categories
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditCategoriesAsync(string entityId, int timeOffset);
    }
}
