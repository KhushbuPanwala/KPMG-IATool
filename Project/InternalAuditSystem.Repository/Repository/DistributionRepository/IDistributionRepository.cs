using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;

namespace InternalAuditSystem.Repository.Repository.DistributionRepository
{
    public interface IDistributionRepository
    {
        /// <summary>
        /// Get Distributors data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of Distributors</returns>
        Task<List<DistributorsAC>> GetDistributorsAsync(int? page, int pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get users data
        /// </summary>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of Users</returns>
        Task<List<EntityUserMappingAC>> GetUsersAsync(string selectedEntityId);

        /// <summary>
        /// Get count of Distributor
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>count of Distributor</returns>
        Task<int> GetDistributorsCountAsync(string searchValue, string selectedEntityId);

        /// <summary>
        /// Delete Distributors detail
        /// <param name="id">id of delete Distributor</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteDistributorAsync(string id);

        /// <summary>
        /// Add Distributors detail
        /// <param name="Distributors">List of Distributors detail to add</param>
        /// </summary>
        /// <returns>Task</returns>
        Task AddDistributorsAsync(List<EntityUserMappingAC> Distributors);

        /// <summary>
        /// Get all Distributors 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>List of distributors</returns>
        Task<List<DistributorsAC>> GetAllDistributorsAsync(string entityId);

        /// <summary>
        /// Export Distributors to excel file
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportDistributorsAsync(string selectedEntityId, int timeOffset);

    }
}
