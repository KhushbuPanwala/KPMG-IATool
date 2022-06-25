using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Method for getting user list
        /// </summary>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>List of users</returns>
        Task<List<UserAC>> GetAllUsersOfEntityAsync(Guid entityId);

        /// <summary>
        /// Update selected entity id for the current user 
        /// </summary>
        /// <param name="userDetails">User Details</param>
        /// <returns>Task</returns>
        Task UpdateSelectedEntityForCurrentUserAsync(UserAC userDetails);

        /// <summary>
        /// Get current logged in user details on login otherwise add user in system
        /// </summary>
        /// <param name="userDetails">user details</param>
        /// <returns>Detailed user details</returns>
        Task<UserAC> GetCurrentUserDetailsOnLoginAsync(UserAC userDetails);

        /// <summary>
        /// Get current logged in user details by its id
        /// </summary>
        /// <param name="userId">Id of the user model</param>
        /// <returns>Limited user details</returns>
        Task<UserAC> GetCurrentLoggedInUserDetailsById(Guid userId);
    }
}
