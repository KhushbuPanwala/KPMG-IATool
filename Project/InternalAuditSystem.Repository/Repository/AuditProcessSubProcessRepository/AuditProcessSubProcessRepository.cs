using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using Microsoft.EntityFrameworkCore;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using System.IO;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository
{
    public class AuditProcessSubProcessRepository : IAuditProcessSubProcessRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        public AuditProcessSubProcessRepository(IDataRepository dataRepository, IMapper mapper, IExportToExcelRepository exportToExcelRepository,
            IAuditableEntityRepository auditableEntityRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditableEntityRepository = auditableEntityRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        #region Audit Process
        /// <summary>
        /// Get all the audit processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit processes under an auditable entity</returns>
        public async Task<List<ProcessAC>> GetAllAuditProcessesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<Process> auditProcessList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ParentId == null
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditProcessList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit process data without searchstring - to show initial data on list page 

                auditProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ParentId == null)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<ProcessAC>>(auditProcessList);

        }

        /// <summary>        /// Get only all the audit processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit processes</returns>
        public async Task<List<ProcessAC>> GetOnlyProcessesByEntityIdAsync(Guid selectedEntityId)
        {
            var auditProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.ParentId == null).OrderBy(x => x.CreatedDateTime)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<ProcessAC>>(auditProcessList);
        }


        /// <summary>
        /// Get alll the audit processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit processes</returns>
        public async Task<List<ProcessAC>> GetAllAuditProcessesByEntityIdAsync(Guid selectedEntityId)
        {
            var auditProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.ParentId == null).OrderBy(x => x.CreatedDateTime)
                                                          .AsNoTracking().ToListAsync();
            return _mapper.Map<List<ProcessAC>>(auditProcessList);
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfAuditProcessSearchStringWiseAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<Process>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.ParentId == null &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Process>(x => !x.IsDeleted
                                                                                    && x.EntityId.ToString() == selectedEntityId
                                                                                    && x.ParentId == null);
            }

            return totalRecords;
        }
        #endregion

        #region Audit SubProcess
        /// <summary>
        /// Get all the audit sub-processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit sub-processes under an auditable entity</returns>
        public async Task<List<ProcessAC>> GetAllAuditSubProcessesPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId)
        {
            List<Process> auditSubProcessList;
            //return list as per search string 
            if (!string.IsNullOrEmpty(searchString))
            {
                auditSubProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ParentId != null
                                                               && x.Name.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.ParentProcess)
                                                              .AsNoTracking().ToListAsync();

                //order by name 
                auditSubProcessList.OrderBy(a => a.Name);
            }
            else
            {
                //when only to get audit sub-processes data without searchstring - to show initial data on list page 

                auditSubProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ParentId != null)
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.ParentProcess)
                                                              .AsNoTracking().ToListAsync();
            }

            return _mapper.Map<List<ProcessAC>>(auditSubProcessList);

        }

        /// <summary>
        /// Get alll the audit sub-processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit sub-processes </returns>
        public async Task<List<ProcessAC>> GetAllAuditSubProcessesByEntityIdAsync(Guid selectedEntityId)
        {
            var auditSubProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == selectedEntityId && x.ParentId != null)
                                                                              .Include(x => x.ParentProcess).AsNoTracking().ToListAsync();
            return _mapper.Map<List<ProcessAC>>(auditSubProcessList);
        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalCountOfAuditSubProcessSearchStringWiseAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.CountAsync<Process>(x => !x.IsDeleted &&
                                                                                x.EntityId.ToString() == selectedEntityId &&
                                                                                x.ParentId != null &&
                                                                                x.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Process>(x => !x.IsDeleted
                                                                                    && x.EntityId.ToString() == selectedEntityId
                                                                                    && x.ParentId != null);
            }

            return totalRecords;
        }
        #endregion

        #region Common method for Process & SubProcess
        /// <summary>
        /// Get data of a particular audit process or sub-process under an auditable entity
        /// </summary>
        /// <param name="auditProcessOrSubProcessId">Id of the audit process or subprocess</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit  audit process or sub-process data</returns>
        public async Task<ProcessAC> GetAuditProcessOrSubProcessByIdAsync(string auditProcessOrSubProcessId, string selectedEntityId)
        {
            var result = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == auditProcessOrSubProcessId)
                                                                              .Include(x => x.ParentProcess).AsNoTracking().FirstAsync();
            ;

            return _mapper.Map<ProcessAC>(result);
        }

        /// <summary>
        /// Add new audit process/sub-process under an auditable entity
        /// </summary>
        /// <param name="auditProcessOrSubProcessDetails">Details of audit process/sub-process/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit process/sub-process details</returns>
        public async Task<ProcessAC> AddAuditProcessOrSubProcessAsync(ProcessAC auditProcessOrSubProcessDetails, string selectedEntityId)
        {

            var isDataExist = false;

            //check process/sub-process exists  or not 
            isDataExist = await _dataRepository.AnyAsync<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
                                                                                    && (auditProcessOrSubProcessDetails.ParentId == null ? x.ParentId == null : x.ParentId != null)
                                                                                    && x.Name.ToLower() == auditProcessOrSubProcessDetails.Name.ToLower());
            //check user already exist in db 
            if (!isDataExist)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        var dataToBeAdded = _mapper.Map<Process>(auditProcessOrSubProcessDetails);
                        dataToBeAdded.EntityId = new Guid(selectedEntityId);
                        dataToBeAdded.CreatedDateTime = DateTime.UtcNow;
                        dataToBeAdded.CreatedBy = currentUserId;
                        dataToBeAdded.ParentProcess = null;

                        var addedData = await _dataRepository.AddAsync<Process>(dataToBeAdded);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();

                        return _mapper.Map<ProcessAC>(addedData);
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
                // differenciate field name
                var fieldName = auditProcessOrSubProcessDetails.ParentId == null ?
                                    StringConstant.AuditProcessFieldName : StringConstant.AuditSubProcessFieldName;

                throw new DuplicateDataException(StringConstant.AuditCategoryFieldName, auditProcessOrSubProcessDetails.Name);
            }

        }

        /// <summary>
        /// Update audit process/sub-process under an auditable entity
        /// </summary>
        /// <param name="updatedProcessOrSubProcessDetails">Updated details of audit process/sub-process</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task UpdateAuditProcessOrSubProcessAsync(ProcessAC updatedProcessOrSubProcessDetails, string selectedEntityId)
        {
            var savedDbData = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.Id == updatedProcessOrSubProcessDetails.Id).AsNoTracking().FirstAsync();

            var isToUpdateAuditCategory = savedDbData.Name.ToLower() == updatedProcessOrSubProcessDetails.Name.ToLower() ? true :
                                                                            !(await _dataRepository.AnyAsync<Process>(x => !x.IsDeleted
                                                                            && (updatedProcessOrSubProcessDetails.ParentId == null ? x.ParentId == null : x.ParentId != null)
                                                                            && x.Name.ToLower() == updatedProcessOrSubProcessDetails.Name.ToLower()));
            if (isToUpdateAuditCategory)
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        savedDbData = _mapper.Map<Process>(updatedProcessOrSubProcessDetails);
                        savedDbData.UpdatedDateTime = DateTime.UtcNow;
                        savedDbData.UpdatedBy = currentUserId;
                        savedDbData.ParentProcess = null;

                        _dataRepository.Update(savedDbData);
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
                // differenciate field name
                var fieldName = updatedProcessOrSubProcessDetails.ParentId == null ?
                                    StringConstant.AuditProcessFieldName : StringConstant.AuditSubProcessFieldName;
                throw new DuplicateDataException(fieldName, updatedProcessOrSubProcessDetails.Name);
            }
        }

        /// <summary>
        /// Delete audit process/subprocess from an auditable entity
        /// </summary>
        /// <param name="processORSubProcessId">Id of the audit process/subprocess that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteAuditProcessOrSubProcessAsync(string processORSubProcessId, string selectedEntityId)
        {
            var isToDelete = false;
            var savedDbData = new Process();
            //check is this process/subprocess is associated any plan
            var totalCountOfAssociatedWithPlan = await _dataRepository.CountAsync<PlanProcessMapping>(x => !x.IsDeleted && x.ProcessId.ToString() == processORSubProcessId);
            var moduleName = StringConstant.AuditPlanModuleName;

            //if only no associated plan or workprogram 
            if (totalCountOfAssociatedWithPlan == 0)
            {
                //check whether this process is used in any observation section or not 
                var isExistInObservation = await _dataRepository.AnyAsync<Observation>(x => !x.IsDeleted && x.ProcessId.ToString() == processORSubProcessId);
                moduleName = StringConstant.ObservationModuleName;

                if (!isExistInObservation)
                {
                    //check whether this process is used in any report observation section or not 
                    var isExistInReportObservation = await _dataRepository.AnyAsync<ReportObservation>(x => x.ProcessId.ToString() == processORSubProcessId);
                    moduleName = StringConstant.ReportObservationModuleName;

                    if (!isExistInReportObservation)
                    {
                        //if-process then check is it linked to any subprocess
                        savedDbData = await _dataRepository.FirstAsync<Process>(x => x.EntityId.ToString() == selectedEntityId && x.Id.ToString() == processORSubProcessId);

                        isToDelete = savedDbData.ParentId == null ? !(await _dataRepository.AnyAsync<Process>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId && x.ParentId.ToString() == processORSubProcessId)) : true;
                        moduleName = StringConstant.AuditSubProcessFieldName;

                    }

                }
            }


            // if no plan has this process as selected then do soft delete
            if (isToDelete)
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {

                        savedDbData.IsDeleted = true;
                        savedDbData.UpdatedDateTime = DateTime.UtcNow;
                        savedDbData.UpdatedBy = currentUserId;
                        savedDbData.ParentProcess = null;
                        savedDbData.UserUpdatedBy = null;
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
                //customize exception message by passing module name that is linked
                throw new DeleteLinkedDataException(StringConstant.DataCannotBeDeletedMessage, moduleName);
            }


        }

        /// <summary>
        /// Get all the audit processes & sub processes under an auditable enitty
        /// </summary>
        /// <param name="selectedEntityId">Selected auditable Id</param>
        /// <returns>List of all processes & sub processes</returns>
        public async Task<List<ProcessAC>> GetAllProcessSubProcessesByEntityIdAsync(Guid selectedEntityId)
        {
            var allProcessSubProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == selectedEntityId)
                                               .Include(x => x.ParentProcess).AsNoTracking().ToListAsync();

            return _mapper.Map<List<ProcessAC>>(allProcessSubProcessList);
        }

        /// <summary>
        /// Method for exporting audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditProcessesAsync(string entityId, int timeOffset)
        {
            var auditProcessesList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.ParentId == null)
                                                         .OrderByDescending(x=>x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditProcessesACList = _mapper.Map<List<ProcessAC>>(auditProcessesList);
            if (auditProcessesACList.Count == 0)
            {
                ProcessAC auditProcessAC = new ProcessAC();
                auditProcessesACList.Add(auditProcessAC);
            }
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            auditProcessesACList.ForEach(x =>
            {
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(auditProcessesACList, StringConstant.AuditProcessFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method for exporting audit subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAuditSubProcessesAsync(string entityId, int timeOffset)
        {
            var auditSubProcessesList = await _dataRepository.Where<Process>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.ParentId != null)
                                                        .Include(x=>x.ParentProcess).OrderByDescending(x=>x.CreatedDateTime).AsNoTracking().ToListAsync();
            var auditSubProcessesACList = _mapper.Map<List<ProcessAC>>(auditSubProcessesList);
            if (auditSubProcessesACList.Count == 0)
            {
                ProcessAC auditProcessAC = new ProcessAC();
                auditSubProcessesACList.Add(auditProcessAC);
            }
           
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            auditSubProcessesACList.ForEach(x =>
            {
                x.Name = x.Id != null ? x.Name : string.Empty;
                x.ParentProcessName =(x.Id != null && x.ParentProcess!=null) ? x.ParentProcess.Name : string.Empty;
                x.ScopeBasedOn = x.Id != null ? x.ScopeBasedOn : string.Empty;
                x.Scope = x.Id != null ? entityName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(auditSubProcessesACList, StringConstant.AuditSubProcessFieldName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}