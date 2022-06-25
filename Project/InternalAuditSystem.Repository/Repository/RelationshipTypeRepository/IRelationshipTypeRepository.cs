using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RelationshipTypeRepository
{
    public interface IRelationshipTypeRepository
    {
        /// <summary>
        /// Get all the relationship types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of relationship types under an auditable entity</returns>
        Task<List<RelationshipTypeAC>> GetAllRelationshipTypesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get all the relationship types under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all relationship types </returns>
        Task<List<RelationshipTypeAC>> GetAllRelationshipTypeByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular relationship type under an auditable entity
        /// </summary>
        /// <param name="relationshipTypeId">Id of the relationship type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular relationship type data</returns>
        Task<RelationshipTypeAC> GetRelationshipTypeByIdAsync(Guid relationshipTypeId, Guid selectedEntityId);

        /// <summary>
        /// Add new relationship type under an auditable entity
        /// </summary>
        /// <param name="relationshipTypeDetails">Details of relationship type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added relationship type details</returns>
        Task<RelationshipTypeAC> AddRelationshipTypeAsync(RelationshipTypeAC relationshipTypeDetails, Guid selectedEntityId);

        /// <summary>
        /// Update relationship type under an auditable entity
        /// </summary>
        /// <param name="updatedRelationshipTypeDetails">Updated details of relationship type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateRelationshipTypeAsync(RelationshipTypeAC updatedRelationshipTypeDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete relationship type from an auditable entity
        /// </summary>
        /// <param name="relationshipTypeId">Id of the relationship type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteRelationshipTypeAsync(Guid relationshipTypeId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfRelationshipTypeSearchStringWiseAsync(string searchString, Guid selectedEntityId);


        /// <summary>
        /// Method for exporting  auditable entity relationship type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityRelationShipTypeAsync(string entityId, int timeOffset);
    }
}
