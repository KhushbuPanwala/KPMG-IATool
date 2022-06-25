using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ProvinceStateRepository
{
   public interface IStateRepository
    {
        /// <summary>
        /// Get all the state under a country with searchstring wise , without
        /// search string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of states under a country</returns>
        Task<Pagination<EntityStateAC>> GetAllStatePageWiseAndSearchWiseAsync(Pagination<EntityStateAC> pagination);

        /// <summary>
        /// Get data of a particular state under a country
        /// </summary>
        /// <param name="stateId">Id of the state</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular state data</returns>
        Task<EntityStateAC> GetStateByIdAsync(string stateId, string selectedEntityId);

        /// <summary>
        /// Method to add state
        /// </summary>
        /// <param name="stateDetails">Details of state</param>
        /// <returns>Added object of state</returns>
        Task<EntityStateAC> AddStateAsync(EntityStateAC stateDetails);

        /// <summary>
        /// Update state under an auditable entity
        /// </summary>
        /// <param name="updatedStateDetails">Updated details of state</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateStateAsync(EntityStateAC updatedStateDetails);

        /// <summary>
        /// Method to delete state
        /// </summary>
        /// <param name="StateId">Id of state</param>
        /// <param name="selectedEntityId">selected entityId</param>
        /// <returns>Task</returns>
        Task DeleteStateAsync(string StateId, string selectedEntityId);

        /// <summary>
        /// Get all states based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityStateAC</returns>
        Task<EntityStateAC> GetAllStatesBasedOnSearchAsync(string searchString, string selectedEntityId);

        /// <summary>
        /// Method for exporting states
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportStatesAsync(string entityId, int timeOffset);
    }
}
