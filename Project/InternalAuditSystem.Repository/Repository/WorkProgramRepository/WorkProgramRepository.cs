using AutoMapper;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using InternalAuditSystem.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.Utility.FileUtil;
using InternalAuditSystem.Repository.AzureBlobStorage;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using System.Net;

namespace InternalAuditSystem.Repository.Repository.WorkProgramRepository
{
    public class WorkProgramRepository : IWorkProgramRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IUserRepository _userRepository;
        public IAuditPlanRepository _auditPlanRepository;
        public IGlobalRepository _globalRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public WorkProgramRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IAuditPlanRepository auditPlanRepository,
            IGlobalRepository globalRepository,
            IFileUtility fileUtility,
            IAzureRepository azureRepository,
             IExportToExcelRepository exportToExcelRepository,
             IAuditableEntityRepository auditableEntityRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _auditPlanRepository = auditPlanRepository;
            _globalRepository = globalRepository;
            _fileUtility = fileUtility;
            _azureRepository = azureRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get WorkProgram detail by Id
        /// </summary>
        /// <param name="workProgramId">Id of Workprogram</param>
        /// <returns>WorkProgramAC</returns>
        public async Task<WorkProgramAC> GetWorkProgramDetailsAsync(string id)
        {
            Guid workProgramId = new Guid(id);
            WorkProgramAC workProgramAC = new WorkProgramAC();
            WorkProgram workProgram = new WorkProgram();

            workProgram = await _dataRepository.FirstOrDefaultAsync<WorkProgram>(x => x.Id == workProgramId && !x.IsDeleted);

            if (workProgram == null)
            {
                throw new NoRecordException(StringConstant.WorkProgramString);
            }


            workProgramAC = _mapper.Map<WorkProgramAC>(workProgram);

            PlanProcessMapping planProcessMapping = await _dataRepository.Where<PlanProcessMapping>(x =>
                                                    x.WorkProgramId == workProgramId && !x.IsDeleted)
                                                    .Include(x => x.Process).FirstOrDefaultAsync();

            if (planProcessMapping == null)
            {
                throw new NoRecordException(StringConstant.WorkProgramString);
            }
            workProgramAC.AuditPlanId = planProcessMapping.PlanId;
            workProgramAC.ParentProcessId = planProcessMapping.Process.ParentId ?? new Guid();
            workProgramAC.ProcessId = planProcessMapping.ProcessId;

            //Get work program participant users from mapping table
            List<WorkProgramTeam> workProgramUserList = await _dataRepository.Where<WorkProgramTeam>(x =>
                                             x.WorkProgramId == workProgramId && !x.IsDeleted).Include(x => x.WorkProgram)
                                             .Include(x => x.User).ToListAsync();

            workProgramAC.WorkProgramTeamACList = _mapper.Map<List<WorkProgramTeamAC>>(workProgramUserList
                                                    .Where(x => x.User.UserType == DomailModel.Enums.UserType.Internal).ToList());
            workProgramAC.WorkProgramClientParticipantsACList = _mapper.Map<List<WorkProgramTeamAC>>(workProgramUserList
                                                    .Where(x => x.User.UserType == DomailModel.Enums.UserType.External).ToList());

            List<WorkPaper> workPaperList = await _dataRepository.Where<WorkPaper>(x => x.WorkProgramId == workProgramId && !x.IsDeleted).ToListAsync();

            if (workPaperList != null && workPaperList.Count > 0)
            {
                workProgramAC.WorkPaperACList = _mapper.Map<List<WorkPaperAC>>(workPaperList.OrderByDescending(x => x.CreatedDateTime));
                for (var i = 0; i < workProgramAC.WorkPaperACList.Count(); i++)
                {
                    workProgramAC.WorkPaperACList[i].fileName = workProgramAC.WorkPaperACList[i].DocumentPath;
                    workProgramAC.WorkPaperACList[i].DocumentPath = _azureRepository.DownloadFile(workProgramAC.WorkPaperACList[i].DocumentPath);
                }
            }

            return workProgramAC;
        }

        /// <summary>
        /// Add Workprogram to database
        /// </summary>
        /// <param name="workProgram">Work program model to be added</param>
        /// <returns>Added work program</returns>
        public async Task<WorkProgramAC> AddWorkProgramAsync(WorkProgramAC workProgramAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    WorkProgram workProgram = new WorkProgram();

                    //Check if workprogram exists
                    if (CheckIfWorkProgramNameExistAsync(workProgramAC.Name))
                    {
                        throw new DuplicateDataException(StringConstant.WorkProgramString, workProgramAC.Name);
                    }

                    workProgramAC.Id = new Guid();
                    workProgram = _mapper.Map<WorkProgram>(workProgramAC);

                    workProgram.CreatedDateTime = DateTime.UtcNow;
                    workProgram.CreatedBy = currentUserId;

                    //Add Work Program
                    await _dataRepository.AddAsync(workProgram);
                    await _dataRepository.SaveChangesAsync();

                    //Update workprogramId in planprocessmapping
                    List<PlanProcessMapping> planProcessMappingList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && x.ProcessId == workProgramAC.ProcessId)
                                                                        .Include(x => x.Process).ToListAsync();

                    if (planProcessMappingList.Any(x => x.ProcessId == workProgramAC.ProcessId && x.WorkProgramId == workProgramAC.Id))
                    {
                        throw new DuplicateWorkProgramException(workProgramAC.ProcessName);
                    }
                    PlanProcessMapping planProcessMapping = planProcessMappingList.FirstOrDefault(x => x.PlanId == workProgramAC.AuditPlanId
                                                             && x.ProcessId == workProgramAC.ProcessId);
                    if (planProcessMapping.WorkProgramId == null)
                    {
                        planProcessMapping.WorkProgramId = workProgram.Id;
                        _dataRepository.Update<PlanProcessMapping>(planProcessMapping);
                        await _dataRepository.SaveChangesAsync();
                    }
                    else
                    {
                        throw new DuplicateWorkProgramException(workProgramAC.ProcessName);
                    }

                    // Assign workprogram id
                    if (workProgramAC.WorkProgramTeamACList != null && workProgramAC.WorkProgramTeamACList.Count() > 0)
                    {
                        for (var i = 0; i < workProgramAC.WorkProgramTeamACList.Count(); i++)
                        {
                            workProgramAC.WorkProgramTeamACList[i].WorkProgramId = workProgram.Id;
                            workProgramAC.WorkProgramTeamACList[i].CreatedDateTime = DateTime.UtcNow;
                            workProgramAC.WorkProgramTeamACList[i].CreatedBy = currentUserId;
                        }
                    }
                    if (workProgramAC.WorkProgramClientParticipantsACList != null && workProgramAC.WorkProgramClientParticipantsACList.Count() > 0)
                    {
                        for (var i = 0; i < workProgramAC.WorkProgramClientParticipantsACList.Count(); i++)
                        {
                            workProgramAC.WorkProgramClientParticipantsACList[i].WorkProgramId = workProgram.Id;
                            workProgramAC.WorkProgramClientParticipantsACList[i].CreatedDateTime = DateTime.UtcNow;
                            workProgramAC.WorkProgramClientParticipantsACList[i].CreatedBy = currentUserId;
                        }
                    }
                    if (workProgramAC.WorkPaperACList != null && workProgramAC.WorkPaperACList.Count() > 0)
                    {
                        for (var i = 0; i < workProgramAC.WorkPaperACList.Count(); i++)
                        {
                            workProgramAC.WorkPaperACList[i].WorkProgramId = workProgram.Id;
                        }
                    }

                    #region Add WorkProgramTeam list
                    List<WorkProgramTeam> workProgramTeamList = _mapper.Map<List<WorkProgramTeam>>(workProgramAC.WorkProgramTeamACList);

                    workProgramTeamList.AddRange(_mapper.Map<List<WorkProgramTeam>>(workProgramAC.WorkProgramClientParticipantsACList));

                    workProgramTeamList.ForEach(x =>
                    {
                        x.WorkProgram = null;
                    });

                    await _dataRepository.AddRangeAsync(workProgramTeamList);
                    await _dataRepository.SaveChangesAsync();
                    #endregion

                    List<WorkPaperAC> workPaperACList = new List<WorkPaperAC>();
                    #region Work paper add 
                    if (workProgramAC.WorkPaperFiles != null && workProgramAC.WorkPaperFiles.Count() > 0)
                    {
                        workPaperACList = await AddAndUploadWorkPaperFiles(workProgramAC.WorkPaperFiles, workProgram.Id);
                    }
                    #endregion
                    Guid auditPlanId = workProgramAC.AuditPlanId;
                    Guid parentProcessId = workProgramAC.ParentProcessId;
                    Guid processId = workProgramAC.ProcessId;

                    workProgramAC = _mapper.Map<WorkProgramAC>(workProgram);
                    workProgramAC.AuditPlanId = auditPlanId;
                    workProgramAC.ParentProcessId = parentProcessId;
                    workProgramAC.ProcessId = processId;
                    workProgramAC.WorkPaperACList = workPaperACList;
                    workProgramAC.WorkPaperFiles = null;

                    transaction.Commit();
                    return workProgramAC;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        /// <summary>
        /// Update work program
        /// </summary>
        /// <param name="workProgramAC">Work program ac</param>
        /// <returns>Work program ac</returns>
        public async Task<WorkProgramAC> UpdateWorkProgramAsync(WorkProgramAC workProgramAC)
        {
            WorkProgram workProgram = new WorkProgram();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    //Check if workprogram exists
                    if (CheckIfWorkProgramNameExistAsync(workProgramAC.Name, workProgramAC.Id))
                    {
                        throw new DuplicateDataException(StringConstant.WorkProgramString, workProgramAC.Name);
                    }
                    workProgram = _mapper.Map<WorkProgram>(workProgramAC);

                    workProgram.UpdatedDateTime = DateTime.UtcNow;
                    workProgram.UpdatedBy = currentUserId;

                    //Update Work Program
                    _dataRepository.Update(workProgram);
                    await _dataRepository.SaveChangesAsync();

                    List<PlanProcessMapping> planProcessMappingList = await _dataRepository.Where<PlanProcessMapping>(x =>
                                                                   !x.IsDeleted && (x.WorkProgramId == workProgramAC.Id || x.ProcessId == workProgramAC.ProcessId)).Include(x => x.Process).ToListAsync();

                    if (!planProcessMappingList.Any(x => x.PlanId == workProgramAC.AuditPlanId && x.WorkProgramId == workProgramAC.Id && x.ProcessId == workProgramAC.ProcessId))
                    {
                        PlanProcessMapping updatePlanProcessMapping = planProcessMappingList.FirstOrDefault(x => x.PlanId == workProgramAC.AuditPlanId && x.ProcessId == workProgramAC.ProcessId && x.WorkProgramId == null);
                        if (updatePlanProcessMapping != null)
                        {
                            //Delete existing plan process mapping
                            PlanProcessMapping deletePlanProcessMapping = planProcessMappingList.FirstOrDefault(x => x.WorkProgramId == workProgramAC.Id);
                            if (deletePlanProcessMapping != null)
                            {
                                deletePlanProcessMapping.WorkProgramId = null;
                                deletePlanProcessMapping.UpdatedDateTime = DateTime.UtcNow;
                                deletePlanProcessMapping.UpdatedBy = currentUserId;
                                _dataRepository.Update<PlanProcessMapping>(deletePlanProcessMapping);
                                await _dataRepository.SaveChangesAsync();
                            }

                            updatePlanProcessMapping.WorkProgramId = workProgramAC.Id;
                            updatePlanProcessMapping.UpdatedDateTime = DateTime.UtcNow;
                            updatePlanProcessMapping.UpdatedBy = currentUserId;
                            _dataRepository.Update<PlanProcessMapping>(updatePlanProcessMapping);
                            await _dataRepository.SaveChangesAsync();
                        }
                        else
                        {
                            throw new DuplicateWorkProgramException(workProgramAC.ProcessName);
                        }

                    }

                    List<WorkProgramTeam> exisitingWorkProgramTeamList = await _dataRepository.Where<WorkProgramTeam>(x => x.WorkProgramId == workProgramAC.Id && !x.IsDeleted).ToListAsync();
                    List<Guid> existingUserIdList = exisitingWorkProgramTeamList.Select(x => x.UserId).ToList();


                    List<Guid> updatedUserIdList = workProgramAC.WorkProgramTeamACList.Select(x => x.UserId).ToList();
                    if (workProgramAC.WorkProgramClientParticipantsACList != null && workProgramAC.WorkProgramClientParticipantsACList.Count > 0)
                    {
                        updatedUserIdList.AddRange(workProgramAC.WorkProgramClientParticipantsACList.Select(x => x.UserId).ToList());
                    }
                    List<Guid> deleteUserIdList = existingUserIdList.Except(updatedUserIdList).ToList();
                    List<Guid> addUserIdList = updatedUserIdList.Except(existingUserIdList).ToList();


                    #region Delete WorkProgramTeam
                    List<WorkProgramTeam> workProgramTeamDeleteList = exisitingWorkProgramTeamList.Where(x => x.WorkProgramId == workProgramAC.Id && deleteUserIdList.Contains(x.UserId)).ToList();

                    _dataRepository.RemoveRange(workProgramTeamDeleteList);
                    await _dataRepository.SaveChangesAsync();
                    #endregion


                    #region Add WorkProgramTeam list
                    List<WorkProgramTeamAC> workProgramTeamACList = workProgramAC.WorkProgramTeamACList.Where(x => addUserIdList.Contains(x.UserId)).ToList();
                    if (workProgramAC.WorkProgramClientParticipantsACList != null && workProgramAC.WorkProgramClientParticipantsACList.Count > 0)
                    {
                        workProgramTeamACList.AddRange(workProgramAC.WorkProgramClientParticipantsACList.Where(x => addUserIdList.Contains(x.UserId)).ToList());
                    }
                    List<WorkProgramTeam> workProgramTeamAddList = _mapper.Map<List<WorkProgramTeam>>(workProgramTeamACList);

                    workProgramTeamAddList.ForEach(x =>
                    {
                        x.WorkProgram = null;
                        x.CreatedDateTime = DateTime.UtcNow;
                        x.CreatedBy = currentUserId;
                    });
                    await _dataRepository.AddRangeAsync(workProgramTeamAddList);
                    await _dataRepository.SaveChangesAsync();
                    #endregion

                    #region Work paper add 
                    if (workProgramAC.WorkPaperFiles != null && workProgramAC.WorkPaperFiles.Count() > 0)
                    {
                        workProgramAC.WorkPaperACList = await AddAndUploadWorkPaperFiles(workProgramAC.WorkPaperFiles, workProgramAC.Id ?? new Guid());
                        workProgramAC.WorkPaperFiles = null;
                    }
                    else
                    {
                        workProgramAC.WorkPaperACList = new List<WorkPaperAC>();
                    }
                    #endregion

                    transaction.Commit();
                    return workProgramAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Get Details for work program add page
        /// </summary>
        /// <param name="entityId">Auditable EntityId</param>
        /// <returns>User and audit plan list for dropdown</returns>
        public async Task<WorkProgramAC> GetWorkProgramAddDetails(Guid entityId)
        {
            List<UserAC> userList = await _userRepository.GetAllUsersOfEntityAsync(entityId);
            WorkProgramAC workProgramAC = new WorkProgramAC();
            var allPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(entityId);

            workProgramAC.AuditPlanACList = allPlanList;
            workProgramAC.ProcessACList = new List<ProcessAC>();
            workProgramAC.SubProcessACList = new List<PlanProcessMappingAC>();
            for (var i = 0; i < allPlanList.Count(); i++)
            {
                workProgramAC.ProcessACList.AddRange(allPlanList[i].ParentProcessList);
                workProgramAC.SubProcessACList.AddRange(allPlanList[i].PlanProcessList);
            }
            workProgramAC.InternalUserAC = userList.Where(x => x.UserType == DomailModel.Enums.UserType.Internal).ToList();
            workProgramAC.ExternalUserAC = userList.Where(x => x.UserType == DomailModel.Enums.UserType.External).ToList();
            return workProgramAC;
        }

        /// <summary>
        /// Method for fetching WorkProgramAC of auditableEntity
        /// </summary>
        /// <param name="auditableEntityId">Id of auditableEntity</param>
        /// <returns>List of WorkProgramAC</returns>
        public async Task<List<WorkProgramAC>> GetAllWorkProgramsAsync(Guid auditableEntityId)
        {
            List<WorkProgramAC> workProgramList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && !x.AuditPlan.IsDeleted && x.AuditPlan.EntityId == auditableEntityId && x.WorkProgramId != null).Include(x => x.WorkProgram)
            .Select(x => new WorkProgramAC { Id = (Guid)x.WorkProgramId, Name = x.WorkProgram.Name }).ToListAsync();
            return workProgramList;

        }

        /// <summary>
        /// Get Workprogram list for grid in list page
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of workprogram</returns>
        public async Task<Pagination<WorkProgramAC>> GetWorkProgramListAsync(Pagination<WorkProgramAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<WorkProgramAC> workProgramACList = _dataRepository.Where<PlanProcessMapping>(x => x.AuditPlan.EntityId == pagination.EntityId
                                           && (!String.IsNullOrEmpty(pagination.searchText) ? x.WorkProgram.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                           && !x.AuditPlan.IsDeleted && !x.IsDeleted && x.WorkProgramId != null)
                                            .Include(x => x.WorkProgram).Include(x => x.Process).Select(x =>
                                                new WorkProgramAC
                                                {
                                                    Id = x.WorkProgram.Id,
                                                    Name = x.WorkProgram.Name,
                                                    ProcessName = x.Process.ParentProcess.Name,
                                                    AuditTitle = x.WorkProgram.AuditTitle,
                                                    StatusString = Convert.ToString(x.WorkProgram.Status),
                                                    AuditPeriod = x.WorkProgram.AuditPeriodStartDate != null &&
                                                                  x.WorkProgram.AuditPeriodEndDate != null ?
                                                                  string.Format(StringConstant.AuditPeriodString, x.WorkProgram.AuditPeriodStartDate != null ? x.WorkProgram.AuditPeriodStartDate.Value.ToString(StringConstant.DateFormat) : string.Empty,
                                                                                x.WorkProgram.AuditPeriodEndDate != null ? x.WorkProgram.AuditPeriodEndDate.Value.ToString(StringConstant.DateFormat) : string.Empty) :
                                                                  string.Empty,
                                                    Scope = x.WorkProgram.Scope,
                                                    CreatedDateTime = x.WorkProgram.CreatedDateTime
                                                });
            //Get total count
            pagination.TotalRecords = workProgramACList.Count();

            pagination.Items = await workProgramACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();

            List<Guid> workProgramIdList = pagination.Items.Select(x => x.Id ?? new Guid()).ToList();

            List<WorkProgramTeam> workProgramTeamList = await _dataRepository.Where<WorkProgramTeam>(x => workProgramIdList.Contains(x.WorkProgramId) && !x.IsDeleted && x.User.UserType == DomailModel.Enums.UserType.Internal).Include(x => x.User).ToListAsync();

            List<WorkProgramTeamAC> workProgramTeamACList = _mapper.Map<List<WorkProgramTeamAC>>(workProgramTeamList);
            if (workProgramTeamACList != null && workProgramTeamACList.Count() > 0)
            {
                WorkProgramTeamAC workProgramTeamAC = new WorkProgramTeamAC();
                for (var i = 0; i < pagination.Items.Count(); i++)
                {
                    workProgramTeamAC = workProgramTeamACList.FirstOrDefault(x => x.WorkProgramId == pagination.Items[i].Id);
                    if (workProgramTeamAC != null)
                    {
                        pagination.Items[i].TeamFirstName = workProgramTeamAC.Name;

                        pagination.Items[i].WorkProgramTeamACList = workProgramTeamACList.Where(x => x.WorkProgramId == pagination.Items[i].Id && x.Id != workProgramTeamAC.Id).ToList();
                    }
                    else
                    {
                        pagination.Items[i].WorkProgramTeamACList = new List<WorkProgramTeamAC>();
                    }
                }
            }

            List<WorkPaper> workPaperList = await _dataRepository.Where<WorkPaper>(x => workProgramIdList.Contains(x.WorkProgramId) && !x.IsDeleted).ToListAsync();

            List<WorkPaperAC> workPaperACList = _mapper.Map<List<WorkPaperAC>>(workPaperList);
            if (workPaperACList != null && workPaperACList.Count() > 0)
            {
                for (var i = 0; i < pagination.Items.Count(); i++)
                {
                    pagination.Items[i].WorkPaperACList = workPaperACList.Where(x => x.WorkProgramId == pagination.Items[i].Id).OrderByDescending(x => x.CreatedDateTime).ToList();
                }
            }
            return pagination;
        }

        /// <summary>
        /// Delete work program
        /// </summary>
        /// <param name="id">Work program id</param>
        /// <returns>Work</returns>
        public async Task DeleteWorkProgramAync(Guid id)
        {

            //Check whether it has MOM reference
            if (await _dataRepository.AnyAsync<Mom>(x => x.WorkProgramId == id && !x.IsDeleted))
            {
                throw new DeleteValidationException(StringConstant.WorkProgramString, StringConstant.MinutesOfMeeting);
            }
            else if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.WorkProgramId == id && !x.IsDeleted))
            {
                throw new DeleteValidationException(StringConstant.WorkProgramString, StringConstant.RiskControlMatrixString);
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    PlanProcessMapping planProcessMapping = await _dataRepository.Where<PlanProcessMapping>(x =>
                                                                x.WorkProgramId == id && !x.IsDeleted).Include(x => x.WorkProgram).FirstAsync();
                    //Delete plan process mapping data
                    planProcessMapping.IsDeleted = true;
                    planProcessMapping.UpdatedBy = currentUserId;
                    planProcessMapping.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<PlanProcessMapping>(planProcessMapping);
                    await _dataRepository.SaveChangesAsync();

                    //Add duplicate mapping without workprogram id
                    PlanProcessMapping addPlanProcessMapping = new PlanProcessMapping()
                    {
                        PlanId = planProcessMapping.PlanId,
                        ProcessId = planProcessMapping.ProcessId,
                        CreatedBy = currentUserId,
                        CreatedDateTime = DateTime.UtcNow
                    };
                    await _dataRepository.AddAsync<PlanProcessMapping>(addPlanProcessMapping);
                    await _dataRepository.SaveChangesAsync();

                    //Delete workprogram team
                    List<WorkProgramTeam> exisitingWorkProgramTeamList = await _dataRepository.Where<WorkProgramTeam>(x => x.WorkProgramId == id && !x.IsDeleted).ToListAsync();

                    if (exisitingWorkProgramTeamList != null && exisitingWorkProgramTeamList.Count() > 0)
                    {
                        for (var i = 0; i < exisitingWorkProgramTeamList.Count(); i++)
                        {
                            exisitingWorkProgramTeamList[i].IsDeleted = true;
                            exisitingWorkProgramTeamList[i].UpdatedBy = currentUserId;
                            exisitingWorkProgramTeamList[i].UpdatedDateTime = DateTime.UtcNow;

                        }
                    }

                    _dataRepository.UpdateRange<WorkProgramTeam>(exisitingWorkProgramTeamList);
                    await _dataRepository.SaveChangesAsync();

                    //Delete work papers
                    List<WorkPaper> existingWorkPaperList = await _dataRepository.Where<WorkPaper>(x => x.WorkProgramId == id && !x.IsDeleted).ToListAsync();

                    if (existingWorkPaperList != null && existingWorkPaperList.Count() > 0)
                    {
                        for (var i = 0; i < existingWorkPaperList.Count(); i++)
                        {
                            existingWorkPaperList[i].IsDeleted = true;
                            existingWorkPaperList[i].UpdatedBy = currentUserId;
                            existingWorkPaperList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<WorkPaper>(existingWorkPaperList);
                    await _dataRepository.SaveChangesAsync();

                    //Delete rcm
                    List<RiskControlMatrix> existingRcmList = await _dataRepository.Where<RiskControlMatrix>(x => x.WorkProgramId == id && !x.IsDeleted).ToListAsync();

                    if (existingRcmList != null && existingRcmList.Count() > 0)
                    {
                        for (var i = 0; i < existingRcmList.Count(); i++)
                        {
                            existingRcmList[i].WorkProgramId = null;
                            existingRcmList[i].UpdatedBy = currentUserId;
                            existingRcmList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<RiskControlMatrix>(existingRcmList);
                    await _dataRepository.SaveChangesAsync();

                    //Delete Workprogram
                    WorkProgram workProgram = planProcessMapping.WorkProgram;
                    workProgram.IsDeleted = true;
                    workProgram.UpdatedBy = currentUserId;
                    workProgram.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<WorkProgram>(workProgram);
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

        /// <summary>
        /// Delete work paper from db and from azure
        /// </summary>
        /// <param name="id">Work paper id</param>
        /// <returns>Void</returns>
        public async Task DeleteWorkPaperAync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    WorkPaper workPaper = await _dataRepository.FirstAsync<WorkPaper>(x => x.Id == id);
                    if (await _fileUtility.DeleteFileAsync(workPaper.DocumentPath))//Delete file from azure
                    {
                        workPaper.IsDeleted = true;
                        workPaper.UpdatedBy = currentUserId;
                        workPaper.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<WorkPaper>(workPaper);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                    }

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to download workpaper
        /// </summary>
        /// <param name="id">WorkPaper Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadWorkPaperAsync(Guid id)
        {
            WorkPaper workPaper = await _dataRepository.FirstAsync<WorkPaper>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(workPaper.DocumentPath);
        }

        /// <summary>
        /// Export Workprogram to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportToExcelAsync(string entityid, int timeOffset)
        {
            List<WorkProgramAC> workProgramACList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted
                                                    && !x.AuditPlan.IsDeleted
                                                    && x.AuditPlan.EntityId == new Guid(entityid)
                                                    && x.WorkProgramId != null)
                                                    .Include(x => x.WorkProgram)
                                                    .Include(x => x.Process)
                                                    .Select(x =>
                                                        new WorkProgramAC
                                                        {
                                                            Id = (Guid)x.WorkProgramId,
                                                            Name = x.WorkProgram.Name,
                                                            AuditPlanName = x.AuditPlan.Title,
                                                            ProcessName = x.Process.ParentProcess.Name,
                                                            SubProcessName = x.Process.Name,
                                                            AuditTitle = x.WorkProgram.AuditTitle,
                                                            AuditPeriod = x.WorkProgram.AuditPeriodStartDate != null &&
                                                                  x.WorkProgram.AuditPeriodEndDate != null ?
                                                                  string.Format(StringConstant.AuditPeriodString, x.WorkProgram.AuditPeriodStartDate != null ? x.WorkProgram.AuditPeriodStartDate.Value.ToString(StringConstant.DateFormat) : string.Empty,
                                                                  x.WorkProgram.AuditPeriodEndDate != null ? x.WorkProgram.AuditPeriodEndDate.Value.ToString(StringConstant.DateFormat) : string.Empty) : string.Empty,
                                                            Scope = x.WorkProgram.Scope,
                                                            StatusString = x.WorkProgram.Status.ToString(),
                                                            CreatedDate = (x.WorkProgramId != null && x.WorkProgram.CreatedDateTime != null) ? x.WorkProgram.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                            UpdatedDate = (x.WorkProgramId != null && x.WorkProgram.UpdatedDateTime != null) ? x.WorkProgram.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                            CreatedDateTime = x.WorkProgram.CreatedDateTime,
                                                            UpdatedDateTime = x.WorkProgram.UpdatedDateTime

                                                        }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();


            List<Guid> workProgramIdList = workProgramACList.Select(x => x.Id ?? new Guid()).ToList();

            //Get work program participant users from mapping table
            List<WorkProgramTeamAC> workProgramUserList = await _dataRepository.Where<WorkProgramTeam>(x =>
                                          workProgramIdList.Contains(x.WorkProgramId) && !x.IsDeleted)
                                          .Include(x => x.WorkProgram)
                                          .Include(x => x.User)
                                          .Select(x => new WorkProgramTeamAC
                                          {
                                              Id = x.Id,
                                              WorkProgramName = x.WorkProgram.Name,
                                              Name = x.User.Name,
                                              Designation = x.User.Designation,
                                              UserType = x.User.UserType,
                                              WorkProgramId = x.WorkProgramId,
                                              CreatedDate = (x.Id != null && x.WorkProgram.CreatedDateTime != null) ? x.WorkProgram.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                              UpdatedDate = string.Empty,
                                              CreatedDateTime = x.WorkProgram.CreatedDateTime
                                          }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();


            List<WorkProgramTeamAC> workProgramTeamACList = new List<WorkProgramTeamAC>();
            List<WorkProgramTeamAC> workProgramClientParticipantsACList = new List<WorkProgramTeamAC>();
            if (workProgramUserList != null && workProgramUserList.Count() > 0)
            {
                for (var i = 0; i < workProgramACList.Count(); i++)
                {
                    workProgramTeamACList.AddRange(workProgramUserList
                                                   .Where(x => x.UserType == DomailModel.Enums.UserType.Internal
                                                   && x.WorkProgramId == workProgramACList[i].Id).ToList());
                    workProgramClientParticipantsACList.AddRange(workProgramUserList
                                                            .Where(x => x.UserType == DomailModel.Enums.UserType.External
                                                            && x.WorkProgramId == workProgramACList[i].Id).ToList());
                }
            }
            if (workProgramACList == null || workProgramACList.Count == 0)
            {
                WorkProgramAC workProgramAC = new WorkProgramAC();
                workProgramACList.Add(workProgramAC);
            }
            if (workProgramTeamACList == null || workProgramTeamACList.Count == 0)
            {
                WorkProgramTeamAC workProgramTeamAC = new WorkProgramTeamAC();
                workProgramTeamACList.Add(workProgramTeamAC);
            }
            if (workProgramClientParticipantsACList == null || workProgramClientParticipantsACList.Count == 0)
            {
                WorkProgramTeamAC clientParticipants = new WorkProgramTeamAC();
                workProgramClientParticipantsACList.Add(clientParticipants);
            }
            // regex which match tags

            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            List<RiskControlMatrixAC> riskControlMatrixACList = await _dataRepository.Where<RiskControlMatrix>(x => x.WorkProgramId != null
                                            && workProgramIdList.Contains(x.WorkProgramId ?? new Guid())
                                            && !x.IsDeleted && x.EntityId == new Guid(entityid)).Include(x => x.RcmSubProcess)
                                            .Include(x => x.StrategicAnalysis)
                                            .Include(x => x.RcmProcess)
                                            .Select(x => new RiskControlMatrixAC
                                            {
                                                Id = x.Id,
                                                WorkProgramName = x.WorkProgram.Name,
                                                RcmProcessName = x.RcmProcess.Process,
                                                RcmSubProcessName = x.RcmSubProcess.SubProcess,
                                                RiskCategory = string.IsNullOrEmpty(x.RiskCategory) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.RiskCategory, string.Empty)),
                                                RiskDescription = string.IsNullOrEmpty(x.RiskDescription) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.RiskDescription, string.Empty)),
                                                ControlObjective = string.IsNullOrEmpty(x.ControlObjective) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.ControlObjective, string.Empty)),
                                                ControlCategoryString = x.ControlCategory.ToString(),
                                                ControlTypeString = x.ControlType.ToString(),
                                                ControlDescription = string.IsNullOrEmpty(x.ControlDescription) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.ControlDescription, string.Empty)),
                                                NatureOfControlString = x.NatureOfControl.ToString(),
                                                AntiFraudControlString = x.AntiFraudControl == true ? StringConstant.Yes : StringConstant.No,
                                                TestSteps = string.IsNullOrEmpty(x.TestSteps) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.TestSteps, string.Empty)),
                                                TestResultString = x.TestResults == true ? StringConstant.Pass : x.TestResults == false ? StringConstant.Fail : string.Empty,
                                                CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                                CreatedDateTime = x.CreatedDateTime
                                            }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            if (riskControlMatrixACList == null || riskControlMatrixACList.Count == 0)
            {
                RiskControlMatrixAC riskControlMatrixAC = new RiskControlMatrixAC();
                riskControlMatrixACList.Add(riskControlMatrixAC);
            }

            List<MomAC> momACList = await _dataRepository.Where<Mom>(x => workProgramIdList.Contains(x.WorkProgramId)
                                        && !x.IsDeleted && !x.WorkProgram.IsDeleted).
                                        Select(x => new MomAC
                                        {
                                            WorkProgramString = x.WorkProgram.Name,
                                            MomDateToString = (x.Id != null && x.MomDate != null) ? x.MomDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                            StartTime = x.Id != null ? x.MomStartTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatOnlyTime) : string.Empty,
                                            EndTime = x.Id != null ? x.MomEndTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatOnlyTime) : string.Empty,
                                            ClosureMeetingDateToString = x.Id != null ? x.ClosureMeetingDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                            Agenda = x.Id != null ? x.Agenda : string.Empty,
                                            CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                            UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty,
                                            CreatedDateTime = x.CreatedDateTime
                                        }).OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            if (momACList == null || momACList.Count == 0)
            {
                MomAC momAC = new MomAC();
                momACList.Add(momAC);
            }


            //create dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.WorkProgramString, workProgramACList);
            dynamicDictionary.Add(StringConstant.WorkProgramrTeamString, workProgramTeamACList);
            dynamicDictionary.Add(StringConstant.WorkProgramClientParticipantsString, workProgramClientParticipantsACList);
            dynamicDictionary.Add(StringConstant.MomModuleName, momACList);
            dynamicDictionary.Add(StringConstant.RiskControlMatrixString, riskControlMatrixACList);

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityid);

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFileWithMultipleTable(dynamicDictionary, StringConstant.WorkprogrameModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private method(s)
        /// <summary>
        /// Check if work program name exist
        /// </summary>
        /// <param name="workPrgramName">Name of workprogram entered by user</param>
        /// <param name="workProgramId">If it is in edit page then this id of workprogram will be there else in add form it will be null</param>
        /// <returns>Returns if name is duplicate or not</returns>
        private bool CheckIfWorkProgramNameExistAsync(string workPrgramName, Guid? workProgramId = null)
        {
            bool isNameExists;
            if (workProgramId != null)
            {
                isNameExists = _dataRepository.Any<WorkProgram>(x => x.Id != workProgramId && !x.IsDeleted && x.Name.ToLower() == workPrgramName.ToLower());
            }
            else
            {
                isNameExists = _dataRepository.Any<WorkProgram>(x => x.Name.ToLower() == workPrgramName.ToLower() && !x.IsDeleted);
            }
            return isNameExists;
        }

        /// <summary>
        /// Add and upload Workpaper files
        /// </summary>
        /// <param name="workPaperFiles">Work paper files</param>
        /// <param name="workProgramId">WorkProgram id</param>
        /// <returns>List of Workpaper AC</returns>
        private async Task<List<WorkPaperAC>> AddAndUploadWorkPaperFiles(List<IFormFile> workPaperFiles, Guid workProgramId)
        {
            List<string> filesUrl = new List<string>();
            //validation
            int fileCount = _dataRepository.Where<WorkPaper>(a => !a.IsDeleted && a.WorkProgramId == workProgramId).Count();
            int totalFiles = fileCount + workPaperFiles.Count;

            if (totalFiles >= StringConstant.FileLimit)
            {
                throw new InvalidFileCount();
            }

            bool isValidFormate = _globalRepository.CheckFileExtention(workPaperFiles);

            if (isValidFormate)
            {
                throw new InvalidFileFormate();
            }

            bool isFileSizeValid = _globalRepository.CheckFileSize(workPaperFiles);

            if (isFileSizeValid)
            {
                throw new InvalidFileSize();
            }

            //Upload file to azure
            filesUrl = await _fileUtility.UploadFilesAsync(workPaperFiles);
            List<WorkPaper> addedWorkPaperList = new List<WorkPaper>();
            for (int i = 0; i < filesUrl.Count(); i++)
            {
                WorkPaper newWorkPaper = new WorkPaper()
                {
                    Id = new Guid(),
                    DocumentPath = filesUrl[i],
                    WorkProgramId = workProgramId,
                    CreatedBy = currentUserId,
                    CreatedDateTime = DateTime.UtcNow
                };
                addedWorkPaperList.Add(newWorkPaper);
            }
            await _dataRepository.AddRangeAsync(addedWorkPaperList);
            await _dataRepository.SaveChangesAsync();
            List<WorkPaperAC> workpaperACList = _mapper.Map<List<WorkPaperAC>>(addedWorkPaperList);
            for (int i = 0; i < workpaperACList.Count(); i++)
            {
                workpaperACList[i].fileName = workpaperACList[i].DocumentPath;
                workpaperACList[i].DocumentPath = _azureRepository.DownloadFile(workpaperACList[i].DocumentPath);
            }
            return workpaperACList.OrderByDescending(x => x.CreatedDateTime).ToList();
        }
        #endregion
    }
}
