using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.DivisionRepository
{
    public interface IDivisionRepository
    {
        /// <summary>
        /// Get all the divisions under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of divisions under an auditable entity</returns>
        Task<List<DivisionAC>> GetAllDivisionPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get alll the divisions under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all divisions</returns>
        Task<List<DivisionAC>> GetAllDivisionByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular division under an auditable entity
        /// </summary>
        /// <param name="divisionId">Id of the division</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular division data</returns>
        Task<DivisionAC> GetDivisionByIdAsync(Guid divisionId, Guid selectedEntityId);

        /// <summary>
        /// Add new division under an auditable entity
        /// </summary>
        /// <param name="divisionDetails">Details of division</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added division details</returns>
        Task<DivisionAC> AddDivisionAsync(DivisionAC divisionDetails, Guid selectedEntityId);

        /// <summary>
        /// Update division under an auditable entity
        /// </summary>
        /// <param name="updatedDivisionDetails">Updated details of division</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateDivisionAsync(DivisionAC updatedDivisionDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete division from an auditable entity
        /// </summary>
        /// <param name="divisionId">Id of the division that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteDivisionAsync(Guid divisionId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfDivisionSearchStringWiseAsync(string searchString, Guid selectedEntityId);

        /// <summary>
        /// Method for exporting  auditable entity division
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityDivisionsAsync(string entityId, int timeOffset);
    }
}
