using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.EntityCategoryRepository
{
    public interface IEntityCategoryRepository
    {
        /// <summary>
        /// Get all the entity categories under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of entity categories under an auditable entity</returns>
        Task<List<EntityCategoryAC>> GetAllEntityCategoryPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get alll the entity categories under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all entity categories </returns>
        Task<List<EntityCategoryAC>> GetAllEntityCategoryByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular entity catgory under an auditable entity
        /// </summary>
        /// <param name="entityCategoryId">Id of the entity catgory</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity catgory data</returns>
        Task<EntityCategoryAC> GetEntityCategoryByIdAsync(Guid entityCategoryId, Guid selectedEntityId);

        /// <summary>
        /// Add new entity catgory under an auditable entity
        /// </summary>
        /// <param name="entityCategoryDetails">Details of entity catgory</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity catgory details</returns>
        Task<EntityCategoryAC> AddEntityCategoryAsync(EntityCategoryAC entityCategoryDetails, Guid selectedEntityId);

        /// <summary>
        /// Update entity catgory under an auditable entity
        /// </summary>
        /// <param name="updatedEntityCategoryDetails">Updated details of entity catgory</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateEntityCategoryAsync(EntityCategoryAC updatedEntityCategoryDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete entity catgory from an auditable entity
        /// </summary>
        /// <param name="entityCategoryId">Id of the entity catgory that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteEntityCategoryAsync(Guid entityCategoryId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfEntityCategorySearchStringWiseAsync(string searchString, Guid selectedEntityId);

        /// <summary>
        /// Method for exporting  auditable entity category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityCategoriesAsync(string entityId, int timeOffset);
    }
}
