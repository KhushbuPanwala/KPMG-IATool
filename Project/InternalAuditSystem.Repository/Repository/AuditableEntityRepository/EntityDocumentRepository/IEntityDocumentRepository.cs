using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository
{
    public interface IEntityDocumentRepository
    {
        /// <summary>
        /// Get EntityDocument List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        Task<Pagination<EntityDocumentAC>> GetEntityDocumentListAsync(Pagination<EntityDocumentAC> pagination);

        /// <summary>
        /// Get EntityDocument details
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>EntityDocumentAC</returns>
        Task<EntityDocumentAC> GetEntityDocumentDetailsAsync(Guid id);

        /// <summary>
        /// Delete EntityDocument
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>Void</returns>
        Task DeleteEntityDocumentAync(Guid id);

        /// <summary>
        /// Update EntityDocument
        /// </summary>
        /// <param name="EntityDocumentAC">EntityDocument AC object</param>
        /// <returns>Void</returns>
        Task UpdateEntityDocumentAsync(EntityDocumentAC entityDocumentAC);

        /// <summary>
        /// Add EntityDocument to database
        /// </summary>
        /// <param name="EntityDocumentAC">EntityDocument model to be added</param>
        /// <returns>Added EntityDocument</returns>
        Task<EntityDocumentAC> AddEntityDocumentAsync(EntityDocumentAC entityDocumentAC);

        /// <summary>
        /// Method to download EntityDocument
        /// </summary>
        /// <param name="id">EntityDocument Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadEntityDocumentAsync(Guid id);

        /// <summary>
        /// Method to Add EntityDocument In New Version
        /// </summary>
        /// <param name="EntityDocumentACList">EntityDocumentAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        Task AddEntityDocumentInNewVersionAsync(List<EntityDocumentAC> entityDocumentACList, Guid versionId);
    }
}
