using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.EntityTypeReporsitory
{
    public interface IEntityTypeRepository
    {
        /// <summary>
        /// Get all the entity types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of entity types under an auditable entity</returns>
        Task<List<EntityTypeAC>> GetAllEntityTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get alll the entity types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all entity types </returns>
        Task<List<EntityTypeAC>> GetAllEntityTypeByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular entity type under an auditable entity
        /// </summary>
        /// <param name="entityTypeId">Id of the entity type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity type data</returns>
        Task<EntityTypeAC> GetEntityTypeByIdAsync(Guid entityTypeId, Guid selectedEntityId);

        /// <summary>
        /// Add new entity type under an auditable entity
        /// </summary>
        /// <param name="entityTypeDetails">Details of entity type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity type details</returns>
        Task<EntityTypeAC> AddEntityTypeAsync(EntityTypeAC entityTypeDetails, Guid selectedEntityId);

        /// <summary>
        /// Update entity type under an auditable entity
        /// </summary>
        /// <param name="updatedEntityTypeDetails">Updated details of entity type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateEntityTypeAsync(EntityTypeAC updatedEntityTypeDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete entity type from an auditable entity
        /// </summary>
        /// <param name="entityTypeId">Id of the entity type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteEntityTypeAsync(Guid entityTypeId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfEntityTypeSearchStringWiseAsync(string searchString, Guid selectedEntityId);


        /// <summary>
        /// Method for exporting  auditable entity type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityTypesAsync(string entityId, int timeOffset);

    }
}
