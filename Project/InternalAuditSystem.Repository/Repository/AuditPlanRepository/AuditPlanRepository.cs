using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.AuditCategoryRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using InternalAuditSystem.Repository.Repository.AuditTypeRepository;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditPlanRepository
{
    public class AuditPlanRepository : IAuditPlanRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private IAuditCategoryRepository _auditCategoryRepository;
        private IAuditTypeRepository _auditTypeRepository;
        private IAuditProcessSubProcessRepository _auditProcessSubProcessRepository;
        private IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public IGlobalRepository _globalRepository; 
        #endregion

        #region Constructor
        public AuditPlanRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IAuditCategoryRepository auditCategoryRepository,
            IAuditTypeRepository auditTypeRepository,
            IAuditProcessSubProcessRepository auditProcessSubProcessRepository,
            IFileUtility fileUtility,
            IAzureRepository azureRepository,
            IExportToExcelRepository exportToExcelRepository,
            IAuditableEntityRepository auditableEntityRepository,
            IHttpContextAccessor httpContextAccessor,
            IGlobalRepository globalRepository
            )
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditCategoryRepository = auditCategoryRepository;
            _auditTypeRepository = auditTypeRepository;
            _auditProcessSubProcessRepository = auditProcessSubProcessRepository;
            _fileUtility = fileUtility;
            _azureRepository = azureRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _globalRepository = globalRepository;
        }
        #endregion

        #region Public Methods

        #region Audit Plan -List
        /// <summary>
        /// Get all the audit plans  under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit plans under an auditable entity</returns>
        public async Task<List<AuditPlanAC>> GetAllAuditPlansPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<AuditPlanAC> auditPlanList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditPlanList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                               && x.Title.ToLower().Contains(searchString.ToLower()))
                                                              .Select(x => new AuditPlanAC
                                                              {
                                                                  Id = x.Id,
                                                                  Title = x.Title,
                                                                  Status = x.Status,
                                                                  Version = x.Version,
                                                                  CreatedDateTime = x.CreatedDateTime,
                                                                  TotalBudgetedHours = (double)x.TotalBudgetedHours,
                                                                  SelectedTypeId = (Guid)x.SelectedTypeId
                                                              })
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditPlanList.OrderBy(a => a.Title);
            }
            else
            {
                //when only to get audit plans data without searchstring - to show initial data on list page 

                auditPlanList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId)
                                                              .Select(x => new AuditPlanAC
                                                              {
                                                                  Id = x.Id,
                                                                  Title = x.Title,
                                                                  Status = x.Status,
                                                                  Version = x.Version,
                                                                  CreatedDateTime = x.CreatedDateTime,
                                                                  TotalBudgetedHours = (double)x.TotalBudgetedHours,
                                                                  SelectedTypeId = (Guid)x.SelectedTypeId
                                                              })
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            if (!string.IsNullOrEmpty(selectedEntityId))
            {
                var allAuditTypeData = await _auditTypeRepository.GetAllAuditTypeByEntityIdAsync(Guid.Parse(selectedEntityId));

                for (int i = 0; i < auditPlanList.Count(); i++)
                {
                    var type = allAuditTypeData.Find(x => x.Id == auditPlanList[i].SelectedTypeId);
                    auditPlanList[i].AuditTypeName = type == null ? StringConstant.NotSpecifiedMessage : type.Name;
                }
            }

            return auditPlanList;
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfAuditPlansAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<AuditPlan>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.Title.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<AuditPlan>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId);
            }

            return totalRecords;
        }

        /// <summary>
        ///  Update plan status 
        /// </summary>
        /// <param name="updatedAuditPlanDetails">Updated plan details</param>
        /// <param name="selectedEntityId">seelected auditable entity</param>
        /// <returns>Erro or successful transaction</returns>
        public async Task UpdateAuditPlanStatusAsync(AuditPlanAC updatedAuditPlanDetails, string selectedEntityId)
        {

            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                var dbData = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.Id == updatedAuditPlanDetails.Id).AsNoTracking().FirstAsync();
                // add data of general tab 
                dbData.Status = updatedAuditPlanDetails.Status;
                dbData.UpdatedDateTime = DateTime.UtcNow;
                dbData.UpdatedBy = currentUserId;
                dbData.AuditableEntity = null;
                dbData.UserUpdatedBy = null;
                _dataRepository.Update(dbData);
                await _dataRepository.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Delete audit plan
        /// </summary>
        /// <param name="auditPlanId">Id of the plan</param>
        /// <returns>Task or exception</returns>
        public async Task DeleteAuditPlanAsync(Guid auditPlanId)
        {

            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                var isLinkedWithWorkProgram = false;
                var isLinkedWithObservation = false;
                var isLinkedWithReportObservation = false;
                var moduleName = string.Empty;
                var dbAuditPlan = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.Id == auditPlanId).AsNoTracking().FirstAsync();

                isLinkedWithWorkProgram = await _dataRepository.AnyAsync<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == dbAuditPlan.Id && x.WorkProgramId != null);
                moduleName = StringConstant.WorkprogrameModuleName;
                if (!isLinkedWithWorkProgram)
                {
                    isLinkedWithObservation = await _dataRepository.AnyAsync<Observation>(x => !x.IsDeleted && x.AuditPlanId == dbAuditPlan.Id);
                    moduleName = StringConstant.ObservationModuleName;
                    if (!isLinkedWithObservation)
                    {
                        //Pending checking of report observaiton
                        isLinkedWithReportObservation = await _dataRepository.AnyAsync<ReportObservation>(x => !x.IsDeleted && x.AuditPlanId == dbAuditPlan.Id);
                        moduleName = StringConstant.ReportObservationModuleName;
                    }
                }

                // if plan is not linked to any other module then delete
                if (!isLinkedWithWorkProgram && !isLinkedWithObservation && !isLinkedWithReportObservation)
                {
                    // soft delete in plan table
                    dbAuditPlan.IsDeleted = true;
                    dbAuditPlan.UpdatedDateTime = DateTime.UtcNow;
                    dbAuditPlan.UpdatedBy = currentUserId;
                    dbAuditPlan.UserUpdatedBy = null;
                    dbAuditPlan.AuditableEntity = null;
                    _dataRepository.Update(dbAuditPlan);
                    await _dataRepository.SaveChangesAsync();

                    // hard delete plan process mapping data 
                    var planProcessMappingList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == auditPlanId).AsNoTracking().ToListAsync();
                    _dataRepository.RemoveRange(planProcessMappingList);
                    await _dataRepository.SaveChangesAsync();

                    // soft delete plan document 
                    var planDocumentList = await _dataRepository.Where<AuditPlanDocument>(x => !x.IsDeleted && x.PlanId == auditPlanId).AsNoTracking().ToListAsync();
                    for (int i = 0; i < planDocumentList.Count; i++)
                    {
                        planDocumentList[i].IsDeleted = true;
                        planDocumentList[i].UpdatedDateTime = DateTime.UtcNow;
                        planDocumentList[i].UpdatedBy = currentUserId;
                        planDocumentList[i].UserUpdatedBy = null;
                        planDocumentList[i].AuditPlan = null;
                    }
                    _dataRepository.UpdateRange(planDocumentList);
                    await _dataRepository.SaveChangesAsync();

                    transaction.Commit();
                }
                else
                {
                    //customize exception message by passing module name that is linked
                    throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, moduleName);
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Create new version of audit plan
        /// </summary>
        /// <param name="auditPlanId">Id of the audit plan</param>
        /// <returns>Task</returns>
        public async Task CreateNewVersionOfAuditPlanAsync(Guid auditPlanId)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                var auditPlan = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.Id == auditPlanId).AsNoTracking().FirstAsync();

                //get latest version no available for the new entry
                var newVersionNo = await GetLatestAvailableVersionAsync(auditPlan);

                #region General & Overview
                //add general and overview data 
                var mappedData = _mapper.Map<AuditPlanAC>(auditPlan);
                var newAuditPlan = _mapper.Map<AuditPlan>(mappedData);
                newAuditPlan.Id = Guid.Empty;
                newAuditPlan.CreatedDateTime = DateTime.UtcNow;
                newAuditPlan.CreatedBy = currentUserId;
                newAuditPlan.Version = newVersionNo;
                newAuditPlan.ParentPlanId = (auditPlan.ParentPlanId == null ? auditPlan.Id : auditPlan.ParentPlanId);
                newAuditPlan.UpdatedDateTime = null;
                newAuditPlan.UpdatedBy = null;
                newAuditPlan.UserCreatedBy = null;
                newAuditPlan.AuditableEntity = null;

                newAuditPlan = await _dataRepository.AddAsync(newAuditPlan);
                await _dataRepository.SaveChangesAsync();
                #endregion

                #region Plan Process
                var planProcessesOfExistingPlan = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == auditPlan.Id).AsNoTracking()
                                                                       .OrderBy(x => x.CreatedDateTime)
                                                                       .Select(x => new PlanProcessMapping { ProcessId = x.ProcessId, Status = x.Status, StartDateTime = x.StartDateTime, EndDateTime = x.EndDateTime })
                                                                       .ToListAsync();
                List<PlanProcessMapping> newPlanProcessList = new List<PlanProcessMapping>();
                for (int i = 0; i < planProcessesOfExistingPlan.Count; i++)
                {
                    var newPlanProcess = new PlanProcessMapping
                    {
                        PlanId = newAuditPlan.Id,
                        AuditPlan = null,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        UserCreatedBy = null,
                        ProcessId = planProcessesOfExistingPlan[i].ProcessId,
                        StartDateTime = planProcessesOfExistingPlan[i].StartDateTime,
                        EndDateTime = planProcessesOfExistingPlan[i].EndDateTime,
                        Status = planProcessesOfExistingPlan[i].Status,

                    };
                    newPlanProcessList.Add(newPlanProcess);
                }

                await _dataRepository.AddRangeAsync(newPlanProcessList);
                await _dataRepository.SaveChangesAsync();
                #endregion

                #region Plan Document
                // pending - after confirming with developer will do that to copy files into another
                var planDocumentOfExistingPlan = await _dataRepository.Where<AuditPlanDocument>(x => !x.IsDeleted && x.PlanId == auditPlan.Id).AsNoTracking()
                                                                       .OrderBy(x => x.CreatedDateTime)
                                                                       .Select(x => new AuditPlanDocument { Purpose = x.Purpose, Path = x.Path })
                                                                       .ToListAsync();

                List<AuditPlanDocument> newPlanDocumentList = new List<AuditPlanDocument>();
                for (int i = 0; i < planDocumentOfExistingPlan.Count; i++)
                {
                    var newPlanDocument = new AuditPlanDocument
                    {
                        PlanId = newAuditPlan.Id,
                        AuditPlan = null,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = currentUserId,
                        UserCreatedBy = null,
                        Purpose = planDocumentOfExistingPlan[i].Purpose,
                        Path = planDocumentOfExistingPlan[i].Path
                    };
                    newPlanDocumentList.Add(newPlanDocument);
                }

                await _dataRepository.AddRangeAsync(newPlanDocumentList);
                await _dataRepository.SaveChangesAsync();
                #endregion

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }
        #endregion

        #region Audit Plan - General & Overview
        /// <summary>
        /// Get initial data required before adding or editing one audit plan
        /// </summary>
        /// <param name="entityId">Selected entity id </param>
        /// <returns>Prefilled data </returns>
        public async Task<AuditPlanAC> GetIntialDataOfAuditPlanAsync(Guid entityId)
        {
            var allAuditTypes = await _auditTypeRepository.GetAllAuditTypeByEntityIdAsync(entityId);
            var allAuditCategories = await _auditCategoryRepository.GetAllAuditCategoriesByEntityIdAsync(entityId);

            var preFilledData = new AuditPlanAC()
            {
                AuditTypeSelectionDisaplyList = allAuditTypes,
                AuditCategorySelectionDisaplyList = allAuditCategories
            };

            return preFilledData;
        }

        /// <summary>
        /// Get audit plan details section wise
        /// </summary>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <param name="entityId">current entity Id</param>
        /// <param name="sectionType">Section type or submenu type</param>
        /// <returns>Data of the particular section of the audit plan</returns>
        public async Task<AuditPlanAC> GetAuditPlanDetailsByIdAsync(Guid auditPlanId, Guid entityId, AuditPlanSectionType sectionType)
        {
            var auditPlanDbData = await _dataRepository.FirstOrDefaultAsync<AuditPlan>(x => !x.IsDeleted && x.EntityId == entityId && x.Id == auditPlanId);
            AuditPlanAC auditPlanDetails = _mapper.Map<AuditPlanAC>(auditPlanDbData);


            //gte initial data 
            if (sectionType == AuditPlanSectionType.General && auditPlanDetails != null)
            {
                if (auditPlanDetails.SelectedTypeId == Guid.Parse(StringConstant.EmptyGuid))

                    auditPlanDetails.SelectedTypeId = null;
                var prefilledData = await GetIntialDataOfAuditPlanAsync(entityId);
                auditPlanDetails.AuditTypeSelectionDisaplyList = prefilledData.AuditTypeSelectionDisaplyList;
                auditPlanDetails.AuditCategorySelectionDisaplyList = prefilledData.AuditCategorySelectionDisaplyList;
            }


            return auditPlanDetails;
        }

        /// <summary>
        /// Add new audit Plan under an auditable entity
        /// </summary>
        /// <param name="auditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit plan details</returns>
        public async Task<Guid> AddAuditPlanAsync(AuditPlanAC auditPlanDetails, string selectedEntityId)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {

                var basicData = new AuditPlan()
                {
                    Title = auditPlanDetails.Title,
                    Status = auditPlanDetails.Status,
                    Version = auditPlanDetails.Version,
                    SelectCategoryId = auditPlanDetails.SelectCategoryId,
                    SelectedTypeId = auditPlanDetails.SelectedTypeId,
                    StartDateTime = auditPlanDetails.StartDateTime,
                    EndDateTime = auditPlanDetails.EndDateTime,
                    EntityId = Guid.Parse(selectedEntityId),
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = currentUserId,
                    UserCreatedBy = null,
                    AuditableEntity = null
                };


                var result = await _dataRepository.AddAsync(basicData);
                await _dataRepository.SaveChangesAsync();
                transaction.Commit();
                return result.Id;

            }
            catch (Exception)
            {

                transaction.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Update audit Plan general and overview details an auditable entity
        /// </summary>
        /// <param name="auditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Audit plan Id</returns>
        public async Task<Guid> UpdateAuditPlanAsync(AuditPlanAC auditPlanDetails, string selectedEntityId)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                var dbData = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.Id == auditPlanDetails.Id).AsNoTracking().FirstAsync();
                if (auditPlanDetails.SectionType == AuditPlanSectionType.General)
                {
                    // add data of general tab 
                    dbData.Title = auditPlanDetails.Title;
                    dbData.Status = auditPlanDetails.Status;
                    dbData.SelectedTypeId = auditPlanDetails.SelectedTypeId;
                    dbData.SelectCategoryId = auditPlanDetails.SelectCategoryId;
                    dbData.StartDateTime = auditPlanDetails.StartDateTime;
                    dbData.EndDateTime = auditPlanDetails.EndDateTime;
                }
                else
                {
                    // add data of overview  tab 
                    dbData.OverviewBackground = auditPlanDetails.OverviewBackground;
                    dbData.TotalBudgetedHours = auditPlanDetails.TotalBudgetedHours;
                    dbData.FinancialYear = auditPlanDetails.FinancialYear;
                }

                dbData.UpdatedDateTime = DateTime.UtcNow;
                dbData.UpdatedBy = currentUserId;
                dbData.UserUpdatedBy = null;
                dbData.AuditableEntity = null;
                _dataRepository.Update(dbData);
                await _dataRepository.SaveChangesAsync();
                transaction.Commit();

                return (Guid)auditPlanDetails.Id;

            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        #endregion

        #region Audit Plan - Plan Process
        /// <summary>
        /// Get all the plan processes under an under an audit plan add/edit page with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <returns>List of plan processes under an audit plan add/edit page</returns>
        public async Task<List<PlanProcessMappingAC>> GetPlanProcessesPageWiseAndSearchWiseByPlanIdAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId, Guid auditPlanId)
        {
            List<PlanProcessMappingAC> planProcessList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                planProcessList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == auditPlanId
                                                               && x.Process.Name.ToLower().Contains(searchString.ToLower()))
                                                              .Select(x => new PlanProcessMappingAC
                                                              {
                                                                  Id = x.Id,
                                                                  ProcessId = x.ProcessId,
                                                                  StartDateTime = x.StartDateTime,
                                                                  EndDateTime = x.EndDateTime,
                                                                  Status = x.Status,
                                                                  ProcessName = x.Process.Name,
                                                                  CreatedDateTime = x.CreatedDateTime
                                                              })
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                planProcessList.OrderBy(a => a.ProcessName);
            }
            else
            {
                //when only to get audit plans data without searchstring - to show initial data on list page 

                planProcessList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == auditPlanId)
                                                              .Select(x => new PlanProcessMappingAC
                                                              {
                                                                  Id = x.Id,
                                                                  ProcessId = x.ProcessId,
                                                                  StartDateTime = x.StartDateTime,
                                                                  EndDateTime = x.EndDateTime,
                                                                  Status = x.Status,
                                                                  ProcessName = x.Process.Name,
                                                                  CreatedDateTime = x.CreatedDateTime
                                                              })
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            // create process name to display 
            if (planProcessList.Count > 0)
            {
                var allProcessList = await _auditProcessSubProcessRepository.GetAllProcessSubProcessesByEntityIdAsync(selectedEntityId);
                for (int i = 0; i < planProcessList.Count(); i++)
                {
                    var processData = allProcessList.Find(x => x.Id == planProcessList[i].ProcessId);
                    if (processData != null && processData.ParentId != null)
                    {
                        planProcessList[i].ProcessName = planProcessList[i].ProcessName;
                        planProcessList[i].ParentProcessName = allProcessList.Find(x => x.Id == processData.ParentId)?.Name;
                    }
                }
            }

            return planProcessList;
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="auditPlanId">selected audit plan Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfPlanProcessesAsync(string searchString, Guid auditPlanId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<PlanProcessMapping>(x => !x.IsDeleted &&
                                                                                x.PlanId == auditPlanId &&
                                                                                x.Process.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == auditPlanId);
            }

            return totalRecords;
        }

        /// <summary>
        /// Add new plan process under a audit plan 
        /// </summary>
        /// <param name="planProcessObj">Details of plan process/param>
        /// <returns>Added plan process details</returns>
        public async Task<PlanProcessMappingAC> AddPlanProcessAsync(PlanProcessMappingAC planProcessObj)
        {
            //check data already added in plan or not 
            if (!await _dataRepository.AnyAsync<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == planProcessObj.PlanId && x.ProcessId == planProcessObj.ProcessId))
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        var addPlanProcess = new PlanProcessMapping
                        {
                            CreatedBy = currentUserId,
                            CreatedDateTime = DateTime.UtcNow,
                            Status = planProcessObj.Status,
                            StartDateTime = planProcessObj.StartDateTime,
                            EndDateTime = planProcessObj.EndDateTime,
                            PlanId = planProcessObj.PlanId,
                            ProcessId = (Guid)planProcessObj.ProcessId,
                            Process = null

                        };

                        var dbData = await _dataRepository.AddAsync(addPlanProcess);
                        await _dataRepository.SaveChangesAsync();

                        transaction.Commit();

                        var addedPlanProcess = _mapper.Map<PlanProcessMappingAC>(dbData);
                        addedPlanProcess.ProcessName = planProcessObj.ProcessName;
                        addedPlanProcess.StatusString = planProcessObj.StatusString;
                        addedPlanProcess.ParentProcessName = planProcessObj.ParentProcessName;
                        return addedPlanProcess;
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
                throw new DuplicateDataException(StringConstant.AuditSubProcessFieldName, planProcessObj.ProcessName);
            }

        }

        /// <summary>
        /// Update plan process under a audit plan 
        /// </summary>
        /// <param name="updatedPlanProcessDetails">Details of plan process</param>
        /// <returns>Audit plan Id</returns>
        public async Task UpdatePlanProcessAsync(PlanProcessMappingAC updatedPlanProcessDetails)
        {

            var dbPlanProcess = await _dataRepository.Where<PlanProcessMapping>(x => x.Id == updatedPlanProcessDetails.Id).AsNoTracking().FirstAsync();

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    if (dbPlanProcess.IsDeleted)
                    {
                        dbPlanProcess = await GetLatestEntry(dbPlanProcess);
                        _dataRepository.DetachEntityEntry(dbPlanProcess);
                    }

                    // update only if the following things are changed 
                    if (dbPlanProcess.Status != updatedPlanProcessDetails.Status ||
                       dbPlanProcess.StartDateTime != updatedPlanProcessDetails.StartDateTime ||
                       dbPlanProcess.EndDateTime != updatedPlanProcessDetails.EndDateTime)
                    {
                        dbPlanProcess.UpdatedBy = currentUserId;
                        dbPlanProcess.UpdatedDateTime = DateTime.UtcNow;
                        dbPlanProcess.Status = updatedPlanProcessDetails.Status;
                        dbPlanProcess.StartDateTime = updatedPlanProcessDetails.StartDateTime;
                        dbPlanProcess.EndDateTime = updatedPlanProcessDetails.EndDateTime;
                        dbPlanProcess.Process = null;
                        dbPlanProcess.UserUpdatedBy = null;

                        _dataRepository.Update<PlanProcessMapping>(dbPlanProcess);
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
        /// Delete audit plan process from an auditable entity
        /// </summary>
        /// <param name="planProcessId">Id of plan process</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeletePlanProcessAsync(Guid planProcessId)
        {
            var isLinkedWithWorkprogram = false;
            var isLinkedWithObservation = false;
            var isLinkedWithReportObservation = false;

            //check is this category is used in any active audit plan or not 
            var dbPlanProcess = await _dataRepository.Where<PlanProcessMapping>(x => x.Id == planProcessId).AsNoTracking().FirstAsync();

            // check this entry is deleted or not
            if (dbPlanProcess.IsDeleted)
            {
                dbPlanProcess = await GetLatestEntry(dbPlanProcess);
                _dataRepository.DetachEntityEntry(dbPlanProcess);

            }

            // check whether it is linked to workprogram or observation 
            isLinkedWithWorkprogram = !dbPlanProcess.IsDeleted && dbPlanProcess.WorkProgramId != null;
            if (!isLinkedWithWorkprogram)
            {
                //check is process selected in observation module or not
                isLinkedWithObservation = await _dataRepository.AnyAsync<Observation>(x => !x.IsDeleted && x.AuditPlanId == dbPlanProcess.PlanId && x.ProcessId == dbPlanProcess.ProcessId);
                if (!isLinkedWithObservation)
                {
                    // check if exist in report observation or not
                    isLinkedWithReportObservation = await _dataRepository.AnyAsync<ReportObservation>(x => !x.IsDeleted && x.AuditPlanId == dbPlanProcess.PlanId && x.ProcessId == dbPlanProcess.ProcessId);
                }
            }

            // if no plan has this category as selected then do soft delete
            if (!isLinkedWithWorkprogram && !isLinkedWithObservation && !isLinkedWithReportObservation)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        _dataRepository.Remove(dbPlanProcess);
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
            //throw exception if this audit category is currently selected in any plan under the selected entity
            else
            {
                var moduleName = string.Empty;

                if (isLinkedWithWorkprogram)
                    moduleName = StringConstant.WorkprogrameModuleName;
                else if (isLinkedWithObservation)
                    moduleName = StringConstant.ObservationModuleName;
                else
                    moduleName = StringConstant.ReportObservationModuleName;
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, moduleName);
            }
        }
        #endregion

        #region Audit Plan - Plan Documents
        /// <summary>
        /// Get all the plan documents under an under an audit plan add/edit page with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <returns>List of plan documents under an audit plan add/edit page</returns>
        public async Task<List<AuditPlanDocumentAC>> GetPlanDocumentsPageWiseAndSearchWiseByPlanIdAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId, Guid auditPlanId)
        {
            List<AuditPlanDocument> planDocumentList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                planDocumentList = await _dataRepository.Where<AuditPlanDocument>(x => !x.IsDeleted && x.PlanId == auditPlanId
                                                               && x.Purpose.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by purpose 
                planDocumentList.OrderBy(a => a.Purpose);
            }
            else
            {
                //when only to get audit plans data without searchstring - to show initial data on list page 

                planDocumentList = await _dataRepository.Where<AuditPlanDocument>(x => !x.IsDeleted && x.PlanId == auditPlanId)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            var documentList = _mapper.Map<List<AuditPlanDocumentAC>>(planDocumentList);

            documentList.ForEach(doc =>
            {
                doc.FileName = doc.Path;
                doc.Path = _azureRepository.DownloadFile(doc.Path);
            });

            return documentList;
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="auditPlanId">selected audit plan Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfPlaDocumentsAsync(string searchString, Guid auditPlanId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<AuditPlanDocument>(x => !x.IsDeleted &&
                                                                                x.PlanId == auditPlanId &&
                                                                                x.Purpose.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<AuditPlanDocument>(x => !x.IsDeleted && x.PlanId == auditPlanId);
            }

            return totalRecords;
        }

        /// <summary>
        /// Add/Update new plan document under a audit plan
        /// </summary>
        /// <param name="planDocumentObj">Details of plan document/param>
        /// <returns>Added plan document details</returns>
        public async Task<AuditPlanDocumentAC> AddAndUploadPlanDocumentAsync(AuditPlanDocumentAC planDocumentObj)
        {
            List<string> azureFilePathUrl = new List<string>();
            if (planDocumentObj.IsNewDocuemntUploaded)
            {
                //validation 
                bool isValidFormate = _globalRepository.CheckFileExtention(planDocumentObj.DocumentFile);

                if (isValidFormate)
                {
                    throw new InvalidFileFormate();
                }

                bool isFileSizeValid = _globalRepository.CheckFileSize(planDocumentObj.DocumentFile);

                if (isFileSizeValid)
                {
                    throw new InvalidFileSize();
                }

                //Upload file to azure
                azureFilePathUrl = await _fileUtility.UploadFilesAsync(planDocumentObj.DocumentFile);
            }

            var dbData = new AuditPlanDocument();
            if (planDocumentObj.Id != null)
            {
                dbData = await _dataRepository.FirstOrDefaultAsync<AuditPlanDocument>(x => !x.IsDeleted && x.Id == planDocumentObj.Id);

            }
            using var transaction = _dataRepository.BeginTransaction();
            try
            {
                AuditPlanDocument addedPlanDocument = new AuditPlanDocument();
                if (planDocumentObj.IsNewDocuemntUploaded)
                {
                    // save file path to database
                    AuditPlanDocument newAduitPlanDocument = new AuditPlanDocument()
                    {
                        Path = azureFilePathUrl[0],
                        Purpose = planDocumentObj.Purpose,
                        PlanId = planDocumentObj.PlanId,
                        CreatedBy = currentUserId,
                        CreatedDateTime = DateTime.UtcNow,
                        AuditPlan = null,
                        UserCreatedBy = null

                    };
                    addedPlanDocument = await _dataRepository.AddAsync(newAduitPlanDocument);
                    await _dataRepository.SaveChangesAsync();
                }
                else
                {

                    dbData.Purpose = planDocumentObj.Purpose;
                    dbData.UpdatedDateTime = DateTime.UtcNow;
                    dbData.UpdatedBy = currentUserId;
                    dbData.AuditPlan = null;
                    dbData.UserUpdatedBy = null;

                    _dataRepository.Update(dbData);
                    await _dataRepository.SaveChangesAsync();

                    addedPlanDocument = dbData;
                }
                transaction.Commit();
                planDocumentObj = _mapper.Map<AuditPlanDocumentAC>(addedPlanDocument);
                planDocumentObj.FileName = addedPlanDocument.Path;
                planDocumentObj.Path = _azureRepository.DownloadFile(addedPlanDocument.Path);

                return planDocumentObj;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Delete plan document from audit plan and azure storage
        /// </summary>
        /// <param name="planDocumentId">Id of the plan document</param>
        /// <returns>Task</returns>
        public async Task DeletePlanDocumentAync(Guid planDocumentId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var planDocument = await _dataRepository.FirstAsync<AuditPlanDocument>(x => x.Id == planDocumentId);
                    if (await _fileUtility.DeleteFileAsync(planDocument.Path))//Delete file from azure
                    {
                        planDocument.IsDeleted = true;
                        planDocument.UpdatedBy = currentUserId;
                        planDocument.UpdatedDateTime = DateTime.UtcNow;
                        planDocument.AuditPlan = null;
                        planDocument.UserUpdatedBy = null;
                        _dataRepository.Update<AuditPlanDocument>(planDocument);
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
        /// Get file url and download plan document 
        /// </summary>
        /// <param name="planDocumentId">Plan document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        public async Task<string> DownloadPlanDocumentAsync(Guid planDocumentId)
        {
            AuditPlanDocument planDocument = await _dataRepository.FirstAsync<AuditPlanDocument>(x => x.Id == planDocumentId && !x.IsDeleted);
            return _fileUtility.DownloadFile(planDocument.Path);
        }
        #endregion

        #region General Methods
        /// <summary>
        /// Get all audit plans for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all audit plans with its id and name only </returns>
        public async Task<List<AuditPlanAC>> GetAllAuditPlansForDisplayInDropDownAsync(Guid entityId)
        {
            var allPlans = await _dataRepository.Where<AuditPlan>(x => x.EntityId == entityId && !x.IsDeleted).
                                            Select(x => new AuditPlanAC { Id = x.Id, Title = x.Title, Version = x.Version }).AsNoTracking().ToListAsync();
            return allPlans;
        }

        /// <summary>
        /// Get all processes & subprocesses under a plan 
        /// </summary>
        /// <param name="auditPlanId">Id of the audit plan</param>
        /// <returns>List of processes & subprocesses under plan containing their name and id only</returns>
        public async Task<List<ProcessAC>> GetPlanWiseAllProcessesByPlanIdAsync(Guid auditPlanId)
        {
            var allPLanProcessList = await _dataRepository.Where<PlanProcessMapping>(x => x.PlanId == auditPlanId && !x.IsDeleted)
                                                          .OrderBy(x => x.CreatedDateTime)
                                                          .Select(x => new ProcessAC { Id = x.Process.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsNoTracking().ToListAsync();

            var parentProcessIdList = allPLanProcessList.Select(x => x.ParentId).Distinct().ToList();
            var allParentProcessDetails = _dataRepository.Where<DomailModel.Models.AuditPlanModels.Process>(x => !x.IsDeleted && parentProcessIdList.Contains(x.Id))
                                                         .Distinct().OrderBy(x => x.Name)
                                                         .Select(x => new ProcessAC { Id = x.Id, Name = x.Name, ParentId = x.ParentId });
            allPLanProcessList.AddRange(allParentProcessDetails);

            return allPLanProcessList;
        }

        /// <summary>
        /// Get all plans and its processes & subprocesses of all plans under an auditable entity
        /// </summary>
        /// <param name="entityId">Id of the auditable entity</param>
        /// <returns>List of all plans and its processes & subprocesses of all plans containing their name and id only</returns>
        public async Task<List<AuditPlanAC>> GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(Guid entityId)
        {
            // all plans
            List<AuditPlanAC> allPlansList = await GetAllAuditPlansForDisplayInDropDownAsync(entityId);
            List<Guid?> allPlansIdList = allPlansList.Select(x => x.Id).ToList();

            // all processes (i.e: subprocesses added) in audit plan
            List<PlanProcessMappingAC> allPlanProcesses = await _dataRepository.Where<PlanProcessMapping>(x => allPlansIdList.Contains(x.PlanId) && !x.IsDeleted)
                                                                                  .Include(x => x.Process)
                                                                                  .OrderBy(x => x.CreatedDateTime)
                                                                                  .Select(x => new PlanProcessMappingAC { PlanId = x.PlanId, ProcessId = x.Process.Id, ProcessName = x.Process.Name, ParentProcessId = x.Process.ParentId })
                                                                                  .AsNoTracking().ToListAsync();

            List<Guid?> allParentProcessIdList = allPlanProcesses.Select(x => x.ParentProcessId).Distinct().ToList();

            // all parent process details
            var allParentProcessDetails = await _dataRepository.Where<DomailModel.Models.AuditPlanModels.Process>(x => !x.IsDeleted && allParentProcessIdList.Contains(x.Id))
                                                         .Distinct().OrderBy(x => x.Name)
                                                         .Select(x => new ProcessAC { Id = x.Id, Name = x.Name, ParentId = x.ParentId })
                                                         .AsNoTracking().ToListAsync();


            for (int i = 0; i < allPlansList.Count; i++)
            {
                // assign sub processes added in only for the current plan
                var planSubprocesses = allPlanProcesses.Where(x => x.PlanId == allPlansList[i].Id).ToList();
                allPlansList[i].PlanProcessList = planSubprocesses;

                // assign parent processes details for the current plan
                var parentProcesses = planSubprocesses.Select(x => x.ParentProcessId).Distinct().ToList();
                allPlansList[i].ParentProcessList = allParentProcessDetails.Where(x => parentProcesses.Contains(x.Id)).ToList();
            }

            return allPlansList;
        }

        #endregion

        #region export to excel audit plan
        /// <summary>
        /// Method for exporting audit plan
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditPlansAsync(string entityId, int timeOffset)
        {
            var auditPlanList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                         .OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditPlanACList = _mapper.Map<List<AuditPlanAC>>(auditPlanList);
            if (auditPlanACList.Count == 0)
            {
                AuditPlanAC auditPlanAC = new AuditPlanAC();
                auditPlanACList.Add(auditPlanAC);
            }
            var auditTypeList = await _dataRepository.Where<AuditType>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .AsNoTracking().ToListAsync();
            var auditTypeACList = _mapper.Map<List<AuditTypeAC>>(auditTypeList);

            var auditCategoryList = await _dataRepository.Where<AuditCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .AsNoTracking().ToListAsync();
            var auditCategoryACList = _mapper.Map<List<AuditCategoryAC>>(auditCategoryList);
            List<PlanProcessMappingAC> planProcessACList = new List<PlanProcessMappingAC>();
            if (planProcessACList.Count == 0)
            {
                PlanProcessMappingAC planProcessMappingAC = new PlanProcessMappingAC();
                planProcessACList.Add(planProcessMappingAC);
            }
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            if (auditPlanACList[0].Id != null)
            {
                auditPlanACList.ForEach(x =>
                {
                    x.Title = x.Id != null ? x.Title : string.Empty;
                    x.AuditCategoryName = x.Id != null ? auditCategoryACList.FirstOrDefault(z => z.Id == x.SelectCategoryId)?.Name : string.Empty;
                    x.AuditTypeName = x.Id != null ? auditTypeACList.FirstOrDefault(z => z.Id == x.SelectedTypeId)?.Name : string.Empty;
                    x.FinancialYear = x.Id != null ? x.FinancialYear : 0;
                    x.OverviewBackground = x.Id != null ? x.OverviewBackground : string.Empty;
                    x.StartDateTimeToString = (x.Id != null && x.StartDateTime != null) ? x.StartDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.EndDateTimeToString = (x.Id != null && x.EndDateTime != null) ? x.EndDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.StatusString = x.Id != null && x.Status.ToString() == StringConstant.AuditPlanActiveStatusString ? StringConstant.AuditPlanActiveStatusString : x.Status.ToString() == StringConstant.AuditPlanUpdateStatusString ? StringConstant.AuditPlanUpdateStatusString : StringConstant.ClosedStatusString;
                    x.TotalBudgetedHours = x.Id != null ? x.TotalBudgetedHours : 0;
                    x.Version = x.Id != null ? x.Version : 0;
                    x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                });

                HashSet<Guid> getAuditPlanIds = new HashSet<Guid>(auditPlanACList.Select(s => (Guid)s.Id));

                planProcessACList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && getAuditPlanIds.Contains(x.PlanId)
                                                                  ).Include(x => x.AuditPlan).Include(x => x.Process)
                                                                 .Select(x => new PlanProcessMappingAC
                                                                 {
                                                                     Id = x.Id,
                                                                     ProcessId = x.ProcessId,
                                                                     StartDateTime = x.StartDateTime,
                                                                     EndDateTime = x.EndDateTime,
                                                                     Status = x.Status,
                                                                     ProcessName = x.Process.Name,
                                                                     CreatedDateTime = x.CreatedDateTime,
                                                                     UpdatedDateTime = x.UpdatedDateTime,
                                                                     ParentProcessId = x.Process.ParentId,
                                                                     PlanId = x.PlanId
                                                                 })
                                                                 .OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();


                if (planProcessACList.Count > 0)
                {
                    var allProcessList = await _auditProcessSubProcessRepository.GetAllProcessSubProcessesByEntityIdAsync(Guid.Parse(entityId));
                    for (int i = 0; i < planProcessACList.Count(); i++)
                    {
                        var processData = allProcessList.Find(x => x.Id == planProcessACList[i].ProcessId);
                        if (processData != null && processData.ParentId != null)
                        {
                            planProcessACList[i].ProcessName = planProcessACList[i].ProcessName;
                            planProcessACList[i].ParentProcessName = allProcessList.Find(x => x.Id == processData.ParentId)?.Name;
                        }
                    }
                }

                if (planProcessACList.Count == 0)
                {
                    PlanProcessMappingAC planProcessMappingAC = new PlanProcessMappingAC();
                    planProcessACList.Add(planProcessMappingAC);
                }

                planProcessACList.ForEach(x =>
                {
                    x.AuditPlan = x.Id != null ? auditPlanList.FirstOrDefault(z => z.Id == x.PlanId)?.Title : string.Empty;
                    x.StartDateTimeForDisplay = (x.Id != null && x.StartDateTime != null) ? x.StartDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.EndDateTimeForDisplay = (x.Id != null && x.EndDateTime != null) ? x.EndDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.ProcessName = x.Id != null ? x?.ProcessName : string.Empty;
                    x.ParentProcessName = x.Id != null ? x?.ParentProcessName : string.Empty;
                    x.StatusString = x.Id != null && x.Status.ToString() == StringConstant.InProgressStatusString ? StringConstant.InProgressStatusString : x.Status.ToString() == StringConstant.ClosedStatusString ? StringConstant.ClosedStatusString : string.Empty;
                    x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                    x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                });
            }
            //crete dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.AuditPlanModuleName, auditPlanACList);
            dynamicDictionary.Add(StringConstant.AuditProcessFieldName, planProcessACList);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFileWithMultipleTable(dynamicDictionary, StringConstant.AuditPlanModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region export to excel audit plan process
        /// <summary>
        /// Method for exporting audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditPlanProcessAsync(string entityId, string auditPlanId, int timeOffset)
        {
            var auditPlanList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.Id == Guid.Parse(auditPlanId) && x.EntityId == Guid.Parse(entityId))
                                                         .OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditPlanACList = _mapper.Map<List<AuditPlanAC>>(auditPlanList);

            HashSet<Guid> getAuditPlanIds = new HashSet<Guid>(auditPlanACList.Select(s => (Guid)s.Id));

            List<PlanProcessMappingAC> planProcessACList = await _dataRepository.Where<PlanProcessMapping>(x => !x.IsDeleted && getAuditPlanIds.Contains(x.PlanId)
                                                               ).Include(x => x.AuditPlan).Include(x => x.Process)
                                                              .Select(x => new PlanProcessMappingAC
                                                              {
                                                                  Id = x.Id,
                                                                  ProcessId = x.ProcessId,
                                                                  StartDateTime = x.StartDateTime,
                                                                  EndDateTime = x.EndDateTime,
                                                                  Status = x.Status,
                                                                  ProcessName = x.Process.Name,
                                                                  CreatedDateTime = x.CreatedDateTime,
                                                                  UpdatedDateTime = x.UpdatedDateTime,
                                                                  ParentProcessId = x.Process.ParentId,
                                                                  PlanId = x.PlanId
                                                              })
                                                              .OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();

            if (planProcessACList.Count > 0)
            {
                var allProcessList = await _auditProcessSubProcessRepository.GetAllProcessSubProcessesByEntityIdAsync(Guid.Parse(entityId));
                for (int i = 0; i < planProcessACList.Count; i++)
                {
                    var processData = allProcessList.Find(x => x.Id == planProcessACList[i].ProcessId);
                    if (processData != null && processData.ParentId != null)
                    {
                        planProcessACList[i].ProcessName = planProcessACList[i].ProcessName;
                        planProcessACList[i].ParentProcessName = allProcessList.Find(x => x.Id == processData.ParentId)?.Name;
                    }
                }
            }
            if (planProcessACList.Count == 0)
            {
                PlanProcessMappingAC planProcessMappingAC = new PlanProcessMappingAC();
                planProcessACList.Add(planProcessMappingAC);
            }

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            planProcessACList.ForEach(x =>
            {
                x.AuditPlan = x.Id != null ? auditPlanList.FirstOrDefault(z => z.Id == x.PlanId)?.Title : string.Empty;
                x.StartDateTimeForDisplay = (x.Id != null && x.StartDateTime != null) ? x.StartDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.EndDateTimeForDisplay = (x.Id != null && x.EndDateTime != null) ? x.EndDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.ProcessName = x.Id != null ? x?.ProcessName : string.Empty;
                x.ParentProcessName = x.Id != null ? x?.ParentProcessName : string.Empty;
                x.StatusString = x.Id != null && x.Status.ToString() == StringConstant.InProgressStatusString ? StringConstant.InProgressStatusString : x.Status.ToString() == StringConstant.ClosedStatusString ? StringConstant.ClosedStatusString : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(planProcessACList, StringConstant.AuditProcessFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion

        #region Private Method
        /// <summary>
        /// Fetch latest data added from work program
        /// </summary>
        /// <param name="oldPlanProcessData">old plan process objetc</param>
        /// <returns>return latest data added from work program data</returns>
        private async Task<PlanProcessMapping> GetLatestEntry(PlanProcessMapping oldPlanProcessData)
        {
            return await _dataRepository.FirstAsync<PlanProcessMapping>(x => !x.IsDeleted && x.PlanId == oldPlanProcessData.PlanId && x.ProcessId == oldPlanProcessData.ProcessId);

        }

        /// <summary>
        ///  Get latest version of the plan
        /// </summary>
        /// <param name="plan">Pbject of seleccted plan</param>
        /// <returns>Version no</returns>
        private async Task<double> GetLatestAvailableVersionAsync(AuditPlan plan)
        {
            var checkingId = plan.ParentPlanId == null ? plan.Id : plan.ParentPlanId;
            var latestVersion = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && x.ParentPlanId == checkingId)
                                              .OrderByDescending(x => x.Version).AsNoTracking().Select(x => x.Version).FirstOrDefaultAsync();
            if (latestVersion != (double)0)
            {
                latestVersion += 1;
            }
            else
            {
                latestVersion = plan.Version + 1;
            }

            return latestVersion;
        }
        #endregion
    }
}
