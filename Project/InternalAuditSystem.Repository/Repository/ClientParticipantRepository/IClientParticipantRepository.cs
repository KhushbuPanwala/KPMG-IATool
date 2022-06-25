using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ClientParticipantRepository
{
    public interface IClientParticipantRepository
    {
        /// <summary>
        /// Get all the client participants under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <returns>List of client participants under an auditable entity</returns>
        Task<List<EntityUserMappingAC>> GetAllClientParticipantsAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId);

        /// <summary>
        /// Get data of a particular client participant under an auditable entity
        /// </summary>
        /// <param name="clientParticipantId">Id of the client participant</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular participant Id</returns>
        Task<UserAC> GetClientParticipantByIdAsync(Guid clientParticipantId, Guid selectedEntityId);

        /// <summary>
        /// Add new Client participant under an auditable entity
        /// </summary>
        /// <param name="clientParticipantDetails">Details of client participant</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added client participant details</returns>
        Task<EntityUserMappingAC> AddClientParticipantAsync(UserAC clientParticipantDetails, Guid selectedEntityId);

        /// <summary>
        /// Update Client participant under an auditable entity
        /// </summary>
        /// <param name="updatedClientDetails">Updated details of client participant</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateClientParticipantAsync(UserAC updatedClientDetails, Guid selectedEntityId);

        /// <summary>
        /// Delete Client participant from an auditable entity
        /// </summary>
        /// <param name="clientParticipantId">Id of the client participant that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteClientParticipantAsync(Guid clientParticipantId, Guid selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalClientParticipantsPerSearchStringAsync(string searchString, Guid selectedEntityId);


        /// <summary>
        /// Method for exporting client participant
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditClientParticipantsAsync(string entityId, int timeOffset);

    }
}
