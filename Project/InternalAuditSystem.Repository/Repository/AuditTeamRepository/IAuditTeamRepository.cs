using InternalAuditSystem.DomainModel.Models;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditTeamRepository
{
    public interface IAuditTeamRepository
    {
        /// <summary>
        /// Get all users based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="updatedToken">Updated token</param>
        /// <returns>List of users with ad information</returns>
        Task<List<UserAC>> GetAllUserFromAdBasedOnSearchAsync(string searchString, AzureAccessTokenAC updatedToken= null);

        /// <summary>
        /// Get logged in user details from azure ad
        /// </summary>
        /// <param name="emailId">email id of the logged in user</param>
        /// <param name="updatedToken">UPdated token details</param>
        /// <returns>User Details</returns>
        Task<UserAC> GetLoggedInUserDetailsFromAdAsync(string emailId, AzureAccessTokenAC updatedToken = null);

        /// <summary>
        /// Get all the audit team members under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>List of audit team member under an auditable entity</returns>
        Task<List<EntityUserMappingAC>> GetAllAuditTeamMemebersAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);
        
        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalAuditTeamMembersPerSearchString(string searchString, string selectedEntityId);

        /// <summary>
        /// Add new audit team member under an auditable entity
        /// </summary>
        /// <param name="auditTeamMemberDetails">Details of audit team member</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit team member details</returns>
        Task<EntityUserMappingAC> AddNewAuditTeamMemberAsync(UserAC auditTeamMemberDetails, string selectedEntityId);

        /// <summary>
        /// Add new list of audit team members under an auditable entity
        /// </summary>
        /// <param name="auditTeamMembersDetailsAC">Details of list of audit team members</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit team members detail</returns>
        Task<List<UserAC>> AddListOfAuditTeamMembersAsync(List<UserAC> auditTeamMembersDetailsAC, string selectedEntityId);

        /// <summary>
        /// Delete  from an auditable entity
        /// </summary>
        /// <param name="auditTeamMemberId">Id of the audit team member that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteAuditTeamMemberAsync(string auditTeamMemberId, string selectedEntityId);


        /// <summary>
        /// Method for exporting audit team
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditTeamAsync(string entityId, int timeOffset);

        /// <summary>
        /// Sycn Audit team Member data with Azure directive
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="selectedEntityId">Selected Entity id</param>
        /// <returns>Updated synced data</returns>
        Task<Pagination<EntityUserMappingAC>> SyncDataInDbForAuditTeamMembersAsync(int? pageIndex, int? pageSize, Guid entityId);
    }
}
