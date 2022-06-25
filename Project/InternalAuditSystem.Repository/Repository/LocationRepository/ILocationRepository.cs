using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.LocationRepository
{
    public interface ILocationRepository
    {
        /// <summary>
        /// Get all the locations under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of locations under an auditable entity</returns>
        Task<List<LocationAC>> GetAllLocationPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get alll the locations under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all locations </returns>
        Task<List<LocationAC>> GetAllLocationsByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular location under an auditable entity
        /// </summary>
        /// <param name="locationId">Id of the location</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular location data</returns>
        Task<LocationAC> GetLocationByIdAsync(Guid locationId, Guid selectedEntityId);

        /// <summary>
        /// Add new location under an auditable entity
        /// </summary>
        /// <param name="locationDetails">Details of location</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added location details</returns>
        Task<LocationAC> AddLocationAsync(LocationAC locationDetails, Guid selectedEntityId);

        /// <summary>
        /// Update location under an auditable entity
        /// </summary>
        /// <param name="updatedLocationDetails">Updated details of location</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateLocationAsync(LocationAC updatedLocationDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete location from an auditable entity
        /// </summary>
        /// <param name="locationId">Id of the location that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteLocationAsync(Guid locationId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfLocationSearchStringWiseAsync(string searchString, Guid selectedEntityId);

        /// <summary>
        /// Method for exporting  auditable entity location
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportEntityLocationsAsync(string entityId, int timeOffset);
    }
}
