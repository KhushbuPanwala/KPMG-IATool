using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomainModel.Models;
using InternalAuditSystem.DomainModel.Models.AzureAdModel;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditTeamRepository
{
    public class AuditTeamRepository : IAuditTeamRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public AuditTeamRepository(IDataRepository dataRepository, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _exportToExcelRepository = exportToExcelRepository;


        }

        #region Public methods
        /// <summary>
        /// Get all the audit team members under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <returns>List of audit team member under an auditable entity</returns>
        public async Task<List<EntityUserMappingAC>> GetAllAuditTeamMemebersAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<EntityUserMapping> auditTeamMemebrsList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditTeamMemebrsList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                               && x.User.UserType == UserType.Internal
                                                               && x.User.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.User)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditTeamMemebrsList.OrderBy(a => a.User.Name);
            }
            else
            {
                //when only to get all audit team members without page wise
                if (pageIndex == null && pageSize == null)
                {
                    auditTeamMemebrsList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                                   && x.User.UserType == UserType.Internal)
                                                                  .Include(x => x.User)
                                                                  .AsNoTracking().ToListAsync();
                }
                //when only to get audit team members data without searchstring 
                else
                {
                    auditTeamMemebrsList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                                  && x.User.UserType == UserType.Internal)
                                                                  .OrderByDescending(x => x.CreatedDateTime)
                                                                  .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                                  .Take((int)pageSize)
                                                                  .Include(x => x.User)
                                                                  .AsNoTracking().ToListAsync();


                }
            }

            return _mapper.Map<List<EntityUserMappingAC>>(auditTeamMemebrsList);
        }


        /// <summary>
        /// Get all users based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="updatedToken">Updated token</param>
        /// <returns>List of users with ad information</returns>
        public async Task<List<UserAC>> GetAllUsersOfAdToSyncData(AzureAccessTokenAC updatedToken = null)
        {

            List<UserAC> allUsers = new List<UserAC>();
            var adUserList = new List<UserAC>();
            var tokenRequest = await GetTokenFromDbAsync(updatedToken);
            try
            {

                var user = await InitializeGraphClient(tokenRequest)
                    .Users
                    .Request().Select(adUSer => new
                    {
                        adUSer.DisplayName,
                        adUSer.UserPrincipalName,
                        adUSer.JobTitle,
                        adUSer.Department,
                        adUSer.UserType,
                        adUSer.Mail
                    }).GetAsync();

                if (user.CurrentPage.Count > 0)
                {
                    user.CurrentPage.ToList().ForEach(x =>
                    {
                        UserAC us = new UserAC
                        {
                            Name = x.DisplayName,
                            EmailId = x.UserType == "Guest" ? x.Mail : x.UserPrincipalName,
                            Designation = x.JobTitle,
                            Department = x.Department
                        };
                        allUsers.Add(us);
                    });
                }

            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                if (ex.Error.Code == StringConstant.InvalidTokenCode)
                {
                    tokenRequest = await UpdateDbToeknAsync();
                    return await GetAllUsersOfAdToSyncData(tokenRequest);
                }

            }
            return allUsers;
        }

        /// <summary>
        /// Get all users based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="updatedToken">Updated token</param>
        /// <returns>List of users with ad information</returns>
        public async Task<List<UserAC>> GetAllUserFromAdBasedOnSearchAsync(string searchString, AzureAccessTokenAC updatedToken = null)
        {

            List<UserAC> allUsers = new List<UserAC>();
            var adUserList = new List<UserAC>();
            var tokenRequest = await GetTokenFromDbAsync(updatedToken);
            try
            {
                var searchText = StringConstant.DisplayNameFilterKey + searchString + StringConstant.OrConditionKey +
                                 StringConstant.FirstNameFilterKey + searchString + StringConstant.OrConditionKey +
                                 StringConstant.SurnameFilterKey + searchString + StringConstant.EndFilertString;

                var user = await InitializeGraphClient(tokenRequest)
                    .Users
                    .Request().Select(adUSer => new
                    {
                        adUSer.DisplayName,
                        adUSer.UserPrincipalName,
                        adUSer.JobTitle,
                        adUSer.Department,
                        adUSer.UserType,
                        adUSer.Mail
                    }).Filter(searchText)
                    .GetAsync();

                if (user.CurrentPage.Count > 0)
                {
                    user.CurrentPage.ToList().ForEach(x =>
                    {
                        UserAC us = new UserAC
                        {
                            Name = x.DisplayName,
                            EmailId = x.UserType == "Guest" ? x.Mail : x.UserPrincipalName,
                            Designation = x.JobTitle,
                            Department = x.Department
                        };
                        allUsers.Add(us);
                    });
                }

            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                if (ex.Error.Code == StringConstant.InvalidTokenCode)
                {
                    tokenRequest = await UpdateDbToeknAsync();
                    return await GetAllUserFromAdBasedOnSearchAsync(searchString, tokenRequest);
                }

            }
            return allUsers;
        }

        /// <summary>
        /// Get logged in user details from azure ad
        /// </summary>
        /// <param name="emailId">email id of the logged in user</param>
        /// <param name="updatedToken">UPdated token details</param>
        /// <returns>User Details</returns>
        public async Task<UserAC> GetLoggedInUserDetailsFromAdAsync(string emailId, AzureAccessTokenAC updatedToken = null)
        {

            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                UserAC adUser = new UserAC();
                var tokenRequest = await GetTokenFromDbAsync(updatedToken);
                try
                {

                    var user = await InitializeGraphClient(tokenRequest)
                        .Users[emailId]
                        .Request().Select(adUSer => new
                        {
                            adUSer.DisplayName,
                            adUSer.UserPrincipalName,
                            adUSer.JobTitle,
                            adUSer.Department,
                            adUSer.UserType
                        })
                        .GetAsync();

                    adUser = new UserAC
                    {
                        Name = user.DisplayName,
                        EmailId = user.UserPrincipalName,
                        Designation = user.JobTitle,
                        Department = user.Department
                    };
                }
                catch (Microsoft.Graph.ServiceException ex)
                {
                    if (ex.Error.Code == StringConstant.InvalidTokenCode)
                    {
                        tokenRequest = await UpdateDbToeknAsync();
                        transaction.Commit();
                        return await GetLoggedInUserDetailsFromAdAsync(emailId, tokenRequest);
                    }
                    if (ex.Error.Code == StringConstant.RequestResourceCode)
                    {

                        return await GetGuestUserDetailsFromAdAsync(emailId, tokenRequest);
                    }
                    else
                    {
                        throw;
                    }
                }
                transaction.Commit();
                return adUser;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Get guest user details based on the email id 
        /// </summary>
        /// <param name="emailId">email id of the guest</param>
        /// <param name="tokenRequest">Access token details</param>
        /// <returns>Guest User Details</returns>
        public async Task<UserAC> GetGuestUserDetailsFromAdAsync(string emailId, AzureAccessTokenAC tokenRequest)
        {
            UserAC guestUserDetails = new UserAC();
            var searchText = StringConstant.GuestMailIdFilterKey + emailId + StringConstant.GuestQueryEndString;
            try
            {
                var user = await InitializeGraphClient(tokenRequest)
                .Users
                .Request().Select(adUSer => new
                {
                    adUSer.DisplayName,
                    adUSer.JobTitle,
                    adUSer.Department,
                    adUSer.UserType,
                    adUSer.Mail
                }).Filter(searchText)
                .GetAsync();

                if (user.CurrentPage.Count > 0)
                {
                    guestUserDetails = new UserAC
                    {
                        Name = user.CurrentPage.First().DisplayName,
                        EmailId = user.CurrentPage.First().Mail,
                        Designation = user.CurrentPage.First().JobTitle,
                        Department = user.CurrentPage.First().Department
                    };


                }
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                if (ex.Error.Code == StringConstant.InvalidTokenCode)
                {
                    tokenRequest = await UpdateDbToeknAsync();
                    return await GetGuestUserDetailsFromAdAsync(emailId, tokenRequest);
                }
                else
                {
                    throw;
                }
            }
            return guestUserDetails;
        }


        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalAuditTeamMembersPerSearchString(string searchString, string selectedEntityId)
        {

            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId &&
                                                                                x.User.UserType == UserType.Internal &&
                                                                                x.User.Name.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<EntityUserMapping>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                                                    && x.User.UserType == UserType.Internal);
            }

            return totalRecords;
        }

        /// <summary>
        /// Add new audit team member under an auditable entity
        /// </summary>
        /// <param name="auditTeamMemberDetails">Details of audit team member</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit team member details</returns>
        public async Task<EntityUserMappingAC> AddNewAuditTeamMemberAsync(UserAC auditTeamMemberDetails, string selectedEntityId)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
                var dbUser = await _dataRepository.FirstOrDefaultAsync<User>(x => x.EmailId.ToLower() == auditTeamMemberDetails.AuditMemberEmailId.ToLower() && !x.IsDeleted);
                //check user already exist in db 
                if (dbUser == null)
                {

                    var userDetails = _mapper.Map<User>(auditTeamMemberDetails);
                    userDetails.EmailId = auditTeamMemberDetails.AuditMemberEmailId;
                    userDetails.UserType = UserType.Internal;
                    userDetails.CreatedDateTime = DateTime.UtcNow;
                    userDetails.CreatedBy = currentUserId;
                    userDetails.UserCreatedBy = null;

                    dbUser = await _dataRepository.AddAsync<User>(userDetails);
                }

                //check user already exist in an auditable entity
                if (dbUser != null && !await _dataRepository.AnyAsync<EntityUserMapping>(x => x.EntityId.ToString() == selectedEntityId && x.UserId == dbUser.Id))
                {
                    //map client participants with auditable entity
                    var entityUserMapping = new EntityUserMapping()
                    {
                        EntityId = new Guid(selectedEntityId),
                        UserId = dbUser.Id,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        UserCreatedBy = null
                    };

                    var newUserData = await _dataRepository.AddAsync<EntityUserMapping>(entityUserMapping);

                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<EntityUserMappingAC>(newUserData);
                }
                else
                {
                    throw new DuplicateDataException(StringConstant.EmailIdFieldName, auditTeamMemberDetails.AuditMemberEmailId);
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Add new list of audit team members under an auditable entity
        /// </summary>
        /// <param name="auditTeamMembersDetailsAC">Details of list of audit team members</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit team members detail</returns>
        public async Task<List<UserAC>> AddListOfAuditTeamMembersAsync(List<UserAC> auditTeamMembersDetailsAC, string selectedEntityId)
        {

            try
            {
                currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);

                var auditTeamMembersEmailIds = auditTeamMembersDetailsAC.Select(x => x.EmailId.ToLower()).ToList();

                var existingDbUserTeamMembers = await _dataRepository.Where<User>(x => auditTeamMembersEmailIds.Contains(x.EmailId.ToLower()) && !x.IsDeleted).AsNoTracking().ToListAsync();

                var existingDbUserTeamMembersAC = _mapper.Map<List<UserAC>>(existingDbUserTeamMembers);

                var teamMembersACToBeAdded = auditTeamMembersDetailsAC.Where(x => !existingDbUserTeamMembersAC.Any(e => x.EmailId == e.EmailId)).ToList();

                var teamMembersToBeAdded = _mapper.Map<List<UserAC>, List<User>>(teamMembersACToBeAdded);

                var existingTeamMembersAC = _mapper.Map<List<UserAC>, List<User>>(auditTeamMembersDetailsAC.Where(x => !teamMembersACToBeAdded.Any(e => x.EmailId == e.EmailId)).ToList());

                //check user already exist in db 
                if (teamMembersToBeAdded.Count() > 0)
                {

                    for (int i = 0; i < teamMembersToBeAdded.Count(); i++)
                    {

                        teamMembersToBeAdded[i].EmailId = teamMembersACToBeAdded[i].EmailId;
                        teamMembersToBeAdded[i].UserType = UserType.Internal;
                        teamMembersToBeAdded[i].CreatedDateTime = DateTime.UtcNow;
                        teamMembersToBeAdded[i].CreatedBy = currentUserId;
                        teamMembersToBeAdded[i].UserCreatedBy = null;
                    }

                    await _dataRepository.AddRangeAsync<User>(teamMembersToBeAdded);
                    await _dataRepository.SaveChangesAsync();
                }

                var allTeamMembers = teamMembersToBeAdded.Concat(existingDbUserTeamMembers);



                return _mapper.Map<List<UserAC>>(allTeamMembers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sycn Audit team Member data with Azure directive
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="selectedEntityId">Selected Entity id</param>
        /// <returns>Updated synced data</returns>
        public async Task<Pagination<EntityUserMappingAC>> SyncDataInDbForAuditTeamMembersAsync(int? pageIndex, int? pageSize, Guid selectedEntityId)
        {
            var addUsers = await GetAllUsersOfAdToSyncData();

            List<EntityUserMapping> entityMembers = await _dataRepository.Where<EntityUserMapping>(x => x.EntityId == selectedEntityId && !x.IsDeleted
                                                                                        && x.User.UserType == UserType.Internal).AsNoTracking()
                                                    .OrderByDescending(x => x.CreatedDateTime).Include(x => x.User).ToListAsync();

            var userList = entityMembers.Select(x => x.User).ToList();

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);

                    userList.ForEach(dbUser =>
                    {
                        var temp = addUsers.FirstOrDefault(adUser => adUser.EmailId == dbUser.EmailId);
                        if (temp != null)
                        {
                            dbUser.Name = temp.Name;
                            dbUser.Department = temp.Department;
                            dbUser.Designation = temp.Designation;
                            dbUser.UpdatedBy = currentUserId;
                            dbUser.UserUpdatedBy = null;
                            dbUser.UpdatedDateTime = DateTime.UtcNow;
                        }
                    });


                    _dataRepository.UpdateRange<User>(userList);
                    await _dataRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var finalUserList = new Pagination<EntityUserMappingAC>
                    {

                        TotalRecords = entityMembers.Count,
                        PageIndex = pageIndex ?? 1,
                        PageSize = (int)pageSize,
                    };

                    finalUserList.Items = _mapper.Map<List<EntityUserMappingAC>>(entityMembers.Skip((pageIndex - 1 ?? 0) * (int)pageSize).Take((int)pageSize).ToList());

                    return finalUserList;
                }

                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }


        }

        /// <summary>
        /// Delete  from an auditable entity
        /// </summary>
        /// <param name="auditTeamMemberId">Id of the audit team member that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteAuditTeamMemberAsync(string auditTeamMemberId, string selectedEntityId)
        {
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            var isToDelete = true;
            var isUserExistInWorkProgram = false;
            var isUserExistInMom = false;
            var isExistInDistributorList = false;
            var isUserAddedAsReviewer = false;
            var isUserIsAuthorOfAnyReportObservation = false;
            var isUserIsAcmReviewer = false;
            var moduleName = string.Empty;
            //get to be deleted data from table
            var deleteAuditTeamMember = await _dataRepository.Where<EntityUserMapping>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditTeamMemberId).AsNoTracking().FirstAsync();

            // check if present in acm
            var allAcmUnderCurrentEntity = await _dataRepository.Where<ACMPresentation>(x => x.EntityId.ToString() == selectedEntityId && !x.IsDeleted).AsNoTracking().Select(x => x.Id).ToListAsync();
            if (allAcmUnderCurrentEntity.Count > 0)
            {
                isUserIsAcmReviewer = await _dataRepository.AnyAsync<ACMReviewer>(x => allAcmUnderCurrentEntity.Contains(x.ACMPresentationId) && x.UserId == deleteAuditTeamMember.UserId && !x.IsDeleted);
                if (isUserIsAcmReviewer)
                {
                    moduleName = StringConstant.AcmReportReviewerModuleName;
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, moduleName);
                }
            }

            //check whether plan exists for the entity or not 
            var auditPlanIdList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId).AsNoTracking().Select(x => x.Id).ToListAsync();
            if (auditPlanIdList.Count > 0)
            {
                #region checking workprogram & MOM
                var workprogramIdList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && auditPlanIdList.Contains(x.PlanId) && x.WorkProgramId != null).AsNoTracking().Select(x => x.WorkProgramId).ToListAsync();
                if (workprogramIdList.Count > 0)
                {
                    //check if user exist in workprogram or not 
                    isUserExistInWorkProgram = await _dataRepository.AnyAsync<WorkProgramTeam>(x => workprogramIdList.Contains(x.WorkProgramId) && x.UserId == deleteAuditTeamMember.UserId);
                    if (!isUserExistInWorkProgram)
                    {
                        var momIdList = await _dataRepository.Where<Mom>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId).AsNoTracking().Select(x => x.Id).ToListAsync();
                        if (momIdList.Count > 0)
                        {
                            //check if this user is in mom
                            isUserExistInMom = await _dataRepository.AnyAsync<MomUserMapping>(x => !x.IsDeleted && momIdList.Contains(x.MomId) && x.UserId == deleteAuditTeamMember.UserId);

                            //if user exist in mom ,data should not be deleted
                            isToDelete = !isUserExistInMom;
                            moduleName = StringConstant.MomModuleName;
                        }
                    }
                    else
                    {
                        //if user exist in work program data should not be deleted
                        isToDelete = !isUserExistInWorkProgram;
                        moduleName = StringConstant.WorkprogrameModuleName;

                    }

                }
                #endregion
            }
            //if plan, workprgram , mom doesn't contain this user then check in report observation
            #region Checking report modules linkning 
            if (isToDelete && !isUserExistInWorkProgram && !isUserExistInMom)
            {
                isExistInDistributorList = await _dataRepository.AnyAsync<Distributors>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.UserId == deleteAuditTeamMember.UserId);
                if (!isExistInDistributorList)
                {
                    var reportIdList = await _dataRepository.Where<Report>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId).AsNoTracking().Select(x => x.Id).ToListAsync();
                    if (reportIdList.Count > 0)
                    {
                        //check  if user is in reviewer list 
                        isUserAddedAsReviewer = await _dataRepository.AnyAsync<ReportUserMapping>(x => !x.IsDeleted && reportIdList.Contains(x.ReportId) && x.UserId == deleteAuditTeamMember.UserId);

                        if (!isUserAddedAsReviewer)
                        {
                            var reportObservationList = await _dataRepository.Where<ReportObservation>(x => !x.IsDeleted && reportIdList.Contains(x.ReportId)).AsNoTracking().Select(x => new ReportObservationAC { Id = x.Id, AuditorId = x.AuditorId }).ToListAsync();

                            //if report observation exist then only check 
                            if (reportObservationList.Count > 0)
                            {
                                //if user exist as author of the report observation 
                                isUserIsAuthorOfAnyReportObservation = reportObservationList.Any(x => x.AuditorId == deleteAuditTeamMember.UserId);
                                if (!isUserIsAuthorOfAnyReportObservation)
                                {
                                    var reportObservationIdList = reportObservationList.Select(x => x.Id).ToList();
                                    var isUserExistInReportObservation = await _dataRepository.AnyAsync<ReportObservationsMember>(x => reportObservationIdList.Contains(x.ReportObservationId)
                                                                                                                                    && x.UserId == deleteAuditTeamMember.UserId);
                                    //if user exist in report observation as first person responsible,data should not be deleted
                                    isToDelete = !isUserExistInReportObservation;
                                    moduleName = StringConstant.ReportObservationFirstPersonModuleName;
                                }
                                else
                                {
                                    isToDelete = !isUserIsAuthorOfAnyReportObservation;
                                    moduleName = StringConstant.ReportObservationAuthortModuleName;
                                }
                            }
                        }
                        else
                        {
                            //if user exist in reviewer list data should not be deleted
                            isToDelete = !isUserAddedAsReviewer;
                            moduleName = StringConstant.ReportReviewerListModuleName;
                        }
                    }
                }
                else
                {
                    //if user exist in distributor data should not be deleted
                    isToDelete = !isExistInDistributorList;
                    moduleName = StringConstant.DistributionModuleName;
                }

            }
            #endregion


            if (isToDelete)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        //remove the last selected entity id from the current member if he selected this entity 
                        var userData = await _dataRepository.FirstOrDefaultAsync<User>(x => x.Id == deleteAuditTeamMember.UserId && x.CurrentSelectedEntityId.ToString() == selectedEntityId);
                        if (userData != null)
                        {
                            userData.CurrentSelectedEntityId = null;
                            userData.UpdatedDateTime = DateTime.UtcNow;
                            userData.UpdatedBy = currentUserId;
                            userData.UserUpdatedBy = null;

                            _dataRepository.Update(userData);
                            await _dataRepository.SaveChangesAsync();
                        }

                        //remove from entity team
                        _dataRepository.Remove(deleteAuditTeamMember);

                        await _dataRepository.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            else
            {
                //customize exception message by passing module name that is linked
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, moduleName);
            }

        }

        #region Export audit team
        /// <summary>
        /// Method for exporting audit team
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditTeamAsync(string entityId, int timeOffset)
        {
            var auditTeamList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.User.UserType == UserType.Internal)
                                                        .Include(x => x.AuditableEntity).Include(x => x.User).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditTeamACList = _mapper.Map<List<EntityUserMappingAC>>(auditTeamList);
            HashSet<Guid> getUserids = new HashSet<Guid>(auditTeamACList.Select(x => x.UserId));
            var userList = await _dataRepository.Where<User>(x => !x.IsDeleted && getUserids.Contains(x.Id)).OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            var userACList = _mapper.Map<List<UserAC>>(userList);
            if (userACList.Count == 0)
            {
                UserAC userAC = new UserAC();
                userACList.Add(userAC);
            }


            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            userACList.ForEach(x =>
            {
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.Designation = x.Id != null ? x.Designation : string.Empty;
                x.EmailId = x.Id != null ? x.EmailId : string.Empty;
                x.Department = x.Id != null ? x.Department : string.Empty;
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.UserTypeString = (x.Id != null && x.UserType.ToString() == StringConstant.InternalTypeString) ? StringConstant.TeamString : (x.UserType.ToString() == StringConstant.ExternalTypeString) ? StringConstant.ClientString : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(userACList, StringConstant.TeamString + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion  
        #endregion

        #region Private methods
        /// <summary>
        /// Get token from database or create new token
        /// </summary>
        /// <param name="updatedToken">existing token</param>
        /// <returns>New updated token</returns>
        private async Task<AzureAccessTokenAC> GetTokenFromDbAsync(AzureAccessTokenAC updatedToken)
        {
            //if no token exist
            var dbToken = await _dataRepository.GetAll<AzureAccessToken>().AsNoTracking().ToListAsync();
            var tokenRequest = updatedToken ?? new AzureAccessTokenAC();
            if (updatedToken == null)
            {
                tokenRequest = dbToken.Count == 0 ? await SaveNewTokenAsync() : _mapper.Map<AzureAccessTokenAC>(dbToken.First());
            }
            return tokenRequest;
        }

        /// <summary>
        /// Initialize graph client object
        /// </summary>
        /// <param name="tokenRequest">Details of token</param>
        /// <returns>graph client object</returns>
        private Microsoft.Graph.GraphServiceClient InitializeGraphClient(AzureAccessTokenAC tokenRequest)
        {
            Microsoft.Graph.GraphServiceClient graphClient = new Microsoft.Graph.GraphServiceClient(new Microsoft.Graph.DelegateAuthenticationProvider(
                       async (requestMessage) =>
                       {
                           await Task.Run(() =>
                           {
                               requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                                   tokenRequest.TokenType,
                                   tokenRequest.AccessToken);
                           });
                       }));

            return graphClient;
        }

        /// <summary>
        /// Add new token in db if no token is present
        /// </summary>
        /// <returns>New token details</returns>
        private async Task<AzureAccessTokenAC> SaveNewTokenAsync()
        {
            var tokenRequest = GetAccessToken();
            var newToken = _mapper.Map<AzureAccessToken>(tokenRequest);
            newToken.CreatedDateTime = DateTime.UtcNow;
            await _dataRepository.AddAsync(newToken);
            await _dataRepository.SaveChangesAsync();
            return tokenRequest;
        }

        /// <summary>
        /// Update new token in the db 
        /// </summary>
        /// <returns>New token details</returns>
        private async Task<AzureAccessTokenAC> UpdateDbToeknAsync()
        {
            var tokenRequest = GetAccessToken();

            var existingTokenInfo = await _dataRepository.GetAll<AzureAccessToken>().FirstAsync();
            existingTokenInfo.AccessToken = tokenRequest.AccessToken;
            existingTokenInfo.TokenType = tokenRequest.TokenType;
            existingTokenInfo.UpdatedDateTime = DateTime.UtcNow;

            _dataRepository.Update(existingTokenInfo);
            await _dataRepository.SaveChangesAsync();
            return tokenRequest;

        }

        /// <summary>
        /// Get Access token from azure ad to do api request
        /// </summary>
        /// <returns>access token with it's type</returns>
        private AzureAccessTokenAC GetAccessToken()
        {
            var azureCredentials = _configuration.GetSection(StringConstant.AzureAdSelectionKeyName);

            //create request url
            var tenantID = azureCredentials.GetSection(StringConstant.TenantIdSelectionKeyName).Value;
            var requestUri = azureCredentials.GetSection(StringConstant.InstanceSelectionKeyName).Value + tenantID + StringConstant.authTokenUrl;

            //create params
            var parameters = new Dictionary<string, string>
            {
                { StringConstant.ClientIdParamName, azureCredentials.GetSection(StringConstant.ClientIdSelectionKeyName).Value },
                { StringConstant.ClientSecretParamName, azureCredentials.GetSection(StringConstant.ClientSecretSelectionKeyName).Value },
                { StringConstant.ScopeParamName, azureCredentials.GetSection(StringConstant.ScopeSelectionKeyName).Value },
                { StringConstant.GrantTypeParamName, StringConstant.GrantType }
            };

            // request for access token.
            var response = RequestTokenFromAd(requestUri, parameters).Result;

            return JsonConvert.DeserializeObject<AzureAccessTokenAC>(response);
        }

        /// <summary>
        /// Request new acess token from ad 
        /// </summary>
        /// <param name="uri">Url of the request</param>
        /// <param name="parameters">Request related parameters</param>
        /// <returns>Return reponse content from ad</returns>
        private async Task<string> RequestTokenFromAd(string uri, Dictionary<string, string> parameters)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    response = await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters));
                    if (!response.IsSuccessStatusCode)
                        throw new AzureAdException(StringConstant.TokenRequestFailureMessage);

                    var content = response.Content.ReadAsStringAsync().Result;

                    if (string.IsNullOrWhiteSpace(content))
                        throw new AzureAdException(StringConstant.EmptyTokenContentMessage);

                    return content;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        #endregion

    }
}
