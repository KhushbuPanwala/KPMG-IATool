using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository
{
    public interface IEntityRelationMappingRepository
    {
        /// <summary>
        /// Get EntityRelationMapping List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        Task<Pagination<EntityRelationMappingAC>> GetEntityRelationMappingListAsync(Pagination<EntityRelationMappingAC> pagination);

        /// <summary>
        /// Delete EntityRelationMapping
        /// </summary>
        /// <param name="id">EntityRelationMapping id</param>
        /// <returns>Void</returns>
        Task DeleteEntityRelationMappingAync(Guid id);

        /// <summary>
        /// Update EntityRelationMapping
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping AC object</param>
        /// <returns>Void</returns>
        Task UpdateEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC);

        /// <summary>
        /// Add EntityRelationMapping to database
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping model to be added</param>
        /// <returns>Added EntityRelationMapping</returns>
        Task<EntityRelationMappingAC> AddEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC);

        /// <summary>
        /// Method to Add EntityRelationMapping In New Version
        /// </summary>
        /// <param name="EntityRelationMappingACList">EntityRelationMappingAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        Task AddEntityRelationMappingInNewVersionAsync(List<EntityRelationMappingAC> entityRelationMappingACList, Guid versionId);

    }
}
