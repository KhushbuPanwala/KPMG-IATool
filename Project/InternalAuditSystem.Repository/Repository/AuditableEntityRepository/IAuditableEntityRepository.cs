using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository
{
    public interface IAuditableEntityRepository
    {
        /// <summary>
        /// Get Auditable Entity List
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <returns>Pagination object</returns>
        Task<Pagination<AuditableEntityAC>> GetAuditableEntityListAsync(Pagination<AuditableEntityAC> pagination);

        /// <summary>
        /// Get auditable entity details
        /// </summary>
        /// <param name="id">Auditable entity id</param>
        /// <param name="stepNo">Step number</param>
        /// <returns>AuditableEntityAC object</returns>
        Task<AuditableEntityAC> GetAuditableEntityDetailsAsync(Guid id, int stepNo);

        /// <summary>
        /// Update Auditable Entity
        /// </summary>
        /// <param name="auditableEntityAC">Auditable entity AC object</param>
        Task UpdateAuditableEntityAsync(AuditableEntityAC auditableEntityAC);

        /// <summary>
        /// Add Auditable Entity to database
        /// </summary>
        /// <param name="auditableEntityAC">Auditable Entity model to be added</param>
        /// <returns>Added Auditable Entity</returns>
        Task<AuditableEntityAC> AddAuditableEntityAsync(AuditableEntityAC auditableEntityAC, bool isFromStrategicAnalysis = false);

        /// <summary>
        /// Delete AuditableEntity
        /// </summary>
        /// <param name="id">AuditableEntity id</param>
        /// <returns>Void</returns>
        Task DeleteAuditableEntityAync(Guid id);

        /// <summary>
        /// Create new version from auditable entity
        /// </summary>
        /// <param name="auditableEntityId">Parent auditable entity</param>
        /// <returns></returns>
        Task CreateNewVersionAsync(Guid auditableEntityId);

        /// <summary>
        /// Export Workprogram to Excel
        /// </summary>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportToExcelAsync(int timeOffset);

        #region Common Method
        ///// <summary>
        ///// Get all the auditable entity whose strategy analysis is done along with current user details
        ///// </summary>
        ///// <returns>List of auditable entity</returns>
        ///// 

        /// <summary>
        /// Get all the auditable entity whose strategy analysis is done and user has access along with user details
        /// </summary>
        /// <param name="currentLoggedInUserId">Logged User Id</param>
        /// <param name="permittedEntityIdList">Permitted entity id list of a current user</param>
        /// <returns>Logged in user details</returns>
        Task<LoggedInUserDetails> GetPermittedEntitiesOfLoggedInUserAsync(Guid currentLoggedInUserId, List<Guid> permittedEntityIdList = null);

        /// <summary>
        /// Get name of selected entity
        /// </summary>
        /// <param name="entityId">Selected entity id<param>
        /// <returns>Name of auditable Entity</returns>
        Task<string> GetEntityNameById(string entityId);
        #endregion

    }
}
