using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Repository.AuditTeamRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IAuditTeamRepository _auditTeamRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public UserRepository(IDataRepository dataRepository, IMapper mapper, IAuditTeamRepository auditTeamRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditTeamRepository = auditTeamRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Method for getting user list
        /// </summary>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>List of users</returns>

        public async Task<List<UserAC>> GetAllUsersOfEntityAsync(Guid entityId)
        {
            List<UserAC> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == entityId)
                                                                       .Include(y => y.User)
                                                                       .Select(x => new UserAC {
                                                                           Id = x.User.Id, 
                                                                           Name = x.User.Name, 
                                                                           Designation = x.User.Designation,
                                                                           UserType = x.User.UserType,
                                                                           EmailId = x.User.EmailId }).ToListAsync();
            return entityUserMappingList;
        }

       /// <summary>
       /// Update selected entity id for the current user 
       /// </summary>
       /// <param name="userDetails">User Details</param>
       /// <returns>Task</returns>
        public async Task UpdateSelectedEntityForCurrentUserAsync(UserAC userDetails)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var dbUser = await _dataRepository.FirstOrDefaultAsync<User>(x => x.Id == userDetails.Id);
                    if (dbUser != null)
                    {
                        dbUser.CurrentSelectedEntityId = userDetails.CurrentSelectedEntityId;
                        _dataRepository.Update(dbUser);
                        await _dataRepository.SaveChangesAsync();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }

        /// <summary>
        /// Get current logged in user details on login otherwise add user in system
        /// </summary>
        /// <param name="userDetails">user details</param>
        /// <returns>Detailed user details</returns>
        public async Task<UserAC> GetCurrentUserDetailsOnLoginAsync(UserAC userDetails)
        {
           
            var  dbUser = await _dataRepository.FirstOrDefaultAsync<User>(x => x.EmailId == userDetails.EmailId);
            if (dbUser == null)
            {
                // if user is logged in for first time add user in system
                var adUserDetails = await _auditTeamRepository.GetLoggedInUserDetailsFromAdAsync(userDetails.EmailId);
                userDetails = await AddNewUserInIATool(adUserDetails);

            }
            else
            {
                // TODO Note :remove updating user details after Sync of audit team implementation
                if(dbUser.Designation != StringConstant.PartnerDesignation && dbUser.Designation != StringConstant.CodHeadDesignation &&
                    dbUser.Designation != StringConstant.DirectorDesignation && dbUser.Designation != StringConstant.AsscDirectorDesignation &&
                    dbUser.Designation != StringConstant.ManagerDesignation && dbUser.Designation != StringConstant.AsstMangagerDesignation &&
                    dbUser.Designation != StringConstant.ConsultantDesignation && dbUser.Designation != StringConstant.AsscConsultantDesignation &&
                    dbUser.Designation != StringConstant.AnalystDesignation && dbUser.Designation != StringConstant.ExecutiveDesignation &&
                    dbUser.Designation != StringConstant.StaffAccountantDesignation)
                {
                    var adUserDetails = await _auditTeamRepository.GetLoggedInUserDetailsFromAdAsync(userDetails.EmailId);
                    userDetails = await UpdateUserInIATool(dbUser, adUserDetails);

                }
                else
                {
                    userDetails = _mapper.Map<UserAC>(dbUser);
                }
            }


            return userDetails;
        }

        /// <summary>
        /// Get current logged in user details by its id
        /// </summary>
        /// <param name="userId">Id of the user model</param>
        /// <returns>Limited user details</returns>
        public async Task<UserAC> GetCurrentLoggedInUserDetailsById(Guid userId)
        {         
            var dbUser = await _dataRepository.Where<User>(x => x.Id == userId)
                .Select(x=> new UserAC { Id = x.Id, CurrentSelectedEntityId = x.CurrentSelectedEntityId , Designation =x.Designation, EmailId = x.EmailId}).FirstOrDefaultAsync();

            if(dbUser != null)
            {
                // asign user role based on its designation
                if (dbUser.Designation == StringConstant.CodHeadDesignation || dbUser.Designation == StringConstant.PartnerDesignation ||
                   dbUser.Designation == StringConstant.DirectorDesignation || dbUser.Designation == StringConstant.TechDirectorDesignation)
                {
                    dbUser.UserRole = UserRole.EngagementPartner;
                }
                else if (dbUser.Designation == StringConstant.AsscDirectorDesignation || dbUser.Designation == StringConstant.ManagerDesignation ||
                    dbUser.Designation == StringConstant.AsstMangagerDesignation)
                {
                    dbUser.UserRole = UserRole.EngagementManager;
                }
                else
                {
                    dbUser.UserRole = UserRole.TeamMember;
                }
            }           

            return dbUser;
        }

        /// <summary>
        /// Add new user details in tool
        /// </summary>
        /// <param name="userDetails">user details</param>
        /// <returns>Added user details</returns>
        public async Task<UserAC> AddNewUserInIATool(UserAC userDetails)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var newUser = _mapper.Map<User>(userDetails);
                    newUser.CreatedDateTime = DateTime.UtcNow;
                    newUser.UserType = UserType.Internal;
                    newUser = await _dataRepository.AddAsync<User>(newUser);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<UserAC>(newUser);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        /// Add new user details in tool
        /// </summary>
        /// <param name="userDetails">user details</param>
        /// <returns>Added user details</returns>
        public async Task<UserAC> UpdateUserInIATool(User userDetails, UserAC updatedAdDetails)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    userDetails.EmailId = updatedAdDetails.EmailId;
                    userDetails.Name = updatedAdDetails.Name;
                    userDetails.Department = updatedAdDetails.Department;
                    userDetails.Designation = updatedAdDetails.Designation;
                    userDetails.UpdatedDateTime = DateTime.UtcNow;
                    userDetails.UserType = UserType.Internal;
                    _dataRepository.Update<User>(userDetails);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<UserAC>(userDetails);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        #endregion
    }
}
