using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ClientParticipantRepository
{
    public class ClientParticipantRepository : IClientParticipantRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public ClientParticipantRepository(IDataRepository dataRepository, IMapper mapper, IAuditableEntityRepository auditableEntityRepository,
            IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository)
        {
            _auditableEntityRepository = auditableEntityRepository;
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        #region Public Methods
        /// <summary>
        /// Get all the client participants under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <returns>List of client participants under an auditable entity</returns>
        public async Task<List<EntityUserMappingAC>> GetAllClientParticipantsAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId)
        {
            List<EntityUserMapping> clientParticipantsList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                clientParticipantsList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.User.UserType == UserType.External
                                                               && x.User.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.User)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                clientParticipantsList.OrderBy(a => a.User.Name);
            }
            else
            {
                //when only to get client participant data without searchstring 

                clientParticipantsList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.User.UserType == UserType.External)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.User)
                                                              .AsNoTracking().ToListAsync();

            }

            return _mapper.Map<List<EntityUserMappingAC>>(clientParticipantsList);
        }

        /// <summary>
        /// Get data of a particular client participant under an auditable entity
        /// </summary>
        /// <param name="clientParticipantId">Id of the client participant</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular participant Id</returns>
        public async Task<UserAC> GetClientParticipantByIdAsync(Guid clientParticipantId, Guid selectedEntityId)
        {

            var result = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.Id == clientParticipantId)
                                              .AsNoTracking().Include(x => x.User).FirstAsync();
            return _mapper.Map<UserAC>(result.User);
        }

        /// <summary>
        /// Add new Client participant under an auditable entity
        /// </summary>
        /// <param name="clientParticipantDetails">Details of client participant</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added client participant details</returns>
        public async Task<EntityUserMappingAC> AddClientParticipantAsync(UserAC clientParticipantDetails, Guid selectedEntityId)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                var dbUser = await _dataRepository.FirstOrDefaultAsync<User>(x => x.EmailId.ToLower() == clientParticipantDetails.EmailId.ToLower() && !x.IsDeleted);
                //check user already exist in db 
                if (dbUser == null)
                {

                    var userDetails = _mapper.Map<User>(clientParticipantDetails);
                    userDetails.UserType = UserType.External;
                    userDetails.CreatedDateTime = DateTime.UtcNow;
                    userDetails.CreatedBy = currentUserId;
                    userDetails.UserCreatedBy = null;

                    dbUser = await _dataRepository.AddAsync<User>(userDetails);

                }

                //check user already exist in an auditable entity
                if (!await _dataRepository.AnyAsync<EntityUserMapping>(x => x.EntityId == selectedEntityId && x.User.EmailId.ToLower() == clientParticipantDetails.EmailId.ToLower()))
                {
                    //map client participants with auditable entity
                    var entityUserMapping = new EntityUserMapping()
                    {
                        EntityId = selectedEntityId,
                        UserId = dbUser.Id,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        UserCreatedBy =null,
                        AuditableEntity = null
                    };

                    var newUserData = await _dataRepository.AddAsync<EntityUserMapping>(entityUserMapping);

                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<EntityUserMappingAC>(newUserData);
                }
                else
                {
                    throw new DuplicateDataException(StringConstant.EmailIdFieldName, clientParticipantDetails.EmailId);
                }

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }


        }

        /// <summary>
        /// Update Client participant under an auditable entity
        /// </summary>
        /// <param name="updatedClientDetails">Updated details of client participant</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateClientParticipantAsync(UserAC updatedClientDetails, Guid selectedEntityId)
        {
            var dbuser = await _dataRepository.Where<User>(x => x.Id == updatedClientDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateUser = dbuser.EmailId.ToLower() == updatedClientDetails.EmailId.ToLower() ? true :
                !(await _dataRepository.AnyAsync<User>(x => !x.IsDeleted && x.EmailId.ToLower() == updatedClientDetails.EmailId.ToLower()));

            if (isToUpdateUser)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var updatedDetails = _mapper.Map<User>(updatedClientDetails);

                        updatedDetails.UpdatedDateTime = DateTime.UtcNow;
                        updatedDetails.UpdatedBy = currentUserId;
                        updatedDetails.UserUpdatedBy = null;

                        _dataRepository.Update(updatedDetails);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            else
            {
                throw new DuplicateDataException(StringConstant.EmailIdFieldName, updatedClientDetails.EmailId);
            }
        }

        /// <summary>
        /// Delete Client participant from an auditable entity
        /// </summary>
        /// <param name="clientParticipantId">Id of the client participant that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteClientParticipantAsync(Guid clientParticipantId, Guid selectedEntityId)
        {
            var isToDelete = true;
            var isUserExistInWorkProgram = false;
            var isUserExistInMom = false;
            var moduleName = string.Empty;
            //get to be deleted data from table
            var deleteClientParticipant = await _dataRepository.Where<EntityUserMapping>(x => x.EntityId == selectedEntityId && x.Id == clientParticipantId)
                                                               .Include(x => x.User).AsNoTracking().FirstAsync();

            //check whether plan exists for the entity or not 
            var auditPlanIdList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.EntityId == selectedEntityId).AsNoTracking().Select(x => x.Id).ToListAsync();
            if (auditPlanIdList.Count > 0)
            {
                var workprogramIdList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && auditPlanIdList.Contains(x.PlanId) && x.WorkProgramId != null).AsNoTracking().Select(x => x.WorkProgramId).ToListAsync();
                if (workprogramIdList.Count > 0)
                {
                    //check if user exist in workprogram or not 
                    isUserExistInWorkProgram = await _dataRepository.AnyAsync<WorkProgramTeam>(x => workprogramIdList.Contains(x.WorkProgramId) && x.UserId == deleteClientParticipant.UserId);
                    if (!isUserExistInWorkProgram)
                    {
                        var momIdList = await _dataRepository.Where<Mom>(x => !x.IsDeleted && x.EntityId == selectedEntityId).AsNoTracking().Select(x => x.Id).ToListAsync();
                        if (momIdList.Count > 0)
                        {
                            //check if this user is in mom
                            isUserExistInMom = await _dataRepository.AnyAsync<MomUserMapping>(x => !x.IsDeleted && momIdList.Contains(x.MomId) && x.UserId == deleteClientParticipant.UserId);

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

            }
            //if plan, workprgram , mom doesn't contain this user then check in report observation
            if (isToDelete && !isUserExistInWorkProgram && !isUserExistInMom)
            {
                var isExistInObservation = await _dataRepository.AnyAsync<Observation>(x => !x.IsDeleted && x.PersonResponsible == deleteClientParticipant.UserId);
                if (!isExistInObservation)
                {
                    var isExistInDistributorList = await _dataRepository.AnyAsync<Distributors>(x => !x.IsDeleted && x.UserId == deleteClientParticipant.UserId);

                    //if user exist in distributor data should not be deleted
                    isToDelete = !isExistInDistributorList;
                    moduleName = StringConstant.DistributionModuleName;

                }
                else
                {
                    //if user exist in observation data should not be deleted
                    isToDelete = !isExistInObservation;
                    moduleName = StringConstant.ObservationModuleName;

                }
            }


            if (isToDelete)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        _dataRepository.Remove(deleteClientParticipant);

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

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalClientParticipantsPerSearchStringAsync(string searchString, Guid selectedEntityId)
        {

            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted &&
                                                                                x.EntityId == selectedEntityId &&
                                                                                x.User.UserType == UserType.External &&
                                                                                x.User.Name.ToLower().Contains(searchString)).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.User.UserType == UserType.External);
            }

            return totalRecords;
        }
        #region Export client participant
        /// <summary>
        /// Method for exporting client participant
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditClientParticipantsAsync(string entityId, int timeOffset)
        {
            var auditClientParticipantList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.User.UserType == UserType.External)
                                                        .Include(x => x.AuditableEntity).Include(x => x.User).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditClientParticipantACList = _mapper.Map<List<EntityUserMappingAC>>(auditClientParticipantList);

           HashSet<Guid> getUserids=new HashSet<Guid>(auditClientParticipantACList.Select(x => x.UserId));
           var userList= await _dataRepository.Where<User>(x => !x.IsDeleted && getUserids.Contains(x.Id)).OrderByDescending(x=>x.CreatedDateTime).ToListAsync();
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
                x.EntityName = x.Id != null ? entityName : string.Empty;
                x.UserTypeString = (x.Id != null && x.UserType.ToString() == StringConstant.InternalTypeString) ? StringConstant.TeamString : (x.UserType.ToString() == StringConstant.ExternalTypeString) ? StringConstant.ClientString : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(userACList, StringConstant.ClientString + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion  
        #endregion    
    }


}
