using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository
{
    public class RcmSubProcessRepository : IRcmSubProcessRepository
    {
        #region Private Variables
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        #endregion

        public RcmSubProcessRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        #region General Methods

        /// <summary>
        /// Get all sub Processes for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all sub Process with its id and name only </returns>
        public async Task<List<RcmSubProcessAC>> GetAllSubProcessesForDisplayInDropDownAsync(Guid entityId)
        {
            var allSubProcesses = await _dataRepository.Where<RiskControlMatrixSubProcess>(x => !x.IsDeleted && x.EntityId == entityId).
                                            Select(x => new RcmSubProcessAC { Id = x.Id, SubProcess = x.SubProcess , EntityId = x.EntityId}).AsNoTracking().ToListAsync();
            return allSubProcesses;
        }

        /// <summary>
        /// Get RCM Sub process data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM Sub Process</returns>
        public async Task<List<RcmSubProcessAC>> GetRcmSubProcessAsync(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            List<RiskControlMatrixSubProcess> rcmSubProcessList;
            if (!string.IsNullOrEmpty(searchString))
            {
                rcmSubProcessList = await _dataRepository.Where<RiskControlMatrixSubProcess>(r => !r.IsDeleted && r.SubProcess.ToLower().Contains(searchString.ToLower()) && r.EntityId.ToString() == selectedEntityId
                ).Skip((page - 1 ?? 0) * pageSize)
                  .Take(pageSize).OrderBy(r => r.SubProcess).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            else
            {
                rcmSubProcessList = await _dataRepository.Where<RiskControlMatrixSubProcess>(r => !r.IsDeleted && r.EntityId.ToString() == selectedEntityId
                ).OrderBy(r => r.SubProcess).AsNoTracking().OrderByDescending(a => a.CreatedDateTime).ToListAsync();
            }
            return _mapper.Map<List<RcmSubProcessAC>>(rcmSubProcessList);
        }
        /// <summary>
        /// Get count of RCM Sub process
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM Sub process</returns>
        public async Task<int> GetRcmSubProcessCountAsync(string selectedEntityId, string searchString = null)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<RiskControlMatrixSubProcess>(a => !a.IsDeleted && a.SubProcess.ToLower().Contains(searchString.ToLower()) && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.Where<RiskControlMatrixSubProcess>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            return totalRecords;

        }

        /// <summary>
        /// Get RCM SubProcess details
        /// </summary>
        /// <param name="subProcessId">SubProcess id</param>
        /// <returns>count of RCM SubProcess</returns>
        public async Task<RcmSubProcessAC> GetRcmSubProcessByIdAsync(string subProcessId)
        {
            RiskControlMatrixSubProcess rcmSubProcess = await _dataRepository.FirstAsync<RiskControlMatrixSubProcess>(a => a.Id.ToString() == subProcessId && !a.IsDeleted);
            return _mapper.Map<RcmSubProcessAC>(rcmSubProcess);
        }

        /// <summary>
        /// Add SubProcess detail
        /// </summary>
        /// <param name="rcmSubProcessAC">detail of SubProcess</param>
        /// <returns>Task</returns>
        public async Task<RcmSubProcessAC> AddRcmSubProcess(RcmSubProcessAC rcmSubProcessAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSubProcess rcmSubProcess = _mapper.Map<RiskControlMatrixSubProcess>(rcmSubProcessAC);
                    rcmSubProcess.CreatedDateTime = DateTime.UtcNow;
                    rcmSubProcess.CreatedBy = currentUserId;
                    await _dataRepository.AddAsync<RiskControlMatrixSubProcess>(rcmSubProcess);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmSubProcessAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update SubProcess detail
        /// </summary>
        /// <param name="rcmSubProcessAC">detail of SubProcess</param>
        /// <returns>Task</returns>
        public async Task<RcmSubProcessAC> UpdateRcmSubProcess(RcmSubProcessAC rcmSubProcessAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSubProcess rcmSubProcess = _mapper.Map<RiskControlMatrixSubProcess>(rcmSubProcessAC);
                    rcmSubProcess.UpdatedDateTime = DateTime.UtcNow;
                    rcmSubProcess.UpdatedBy = currentUserId;
                    _dataRepository.Update<RiskControlMatrixSubProcess>(rcmSubProcess);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmSubProcessAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete RCM SubProcess detail
        /// <param name="subProcessId">id of deleted SubProcess</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteRcmSubProcess(string subProcessId)
        {
            //Check whether it has RCM list reference
            if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.RcmSubProcessId == Guid.Parse(subProcessId) && !x.IsDeleted))
            {
                throw new DuplicateRCMException(StringConstant.RiskControlMatrixString);
            }
            else if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.RcmSubProcessId == Guid.Parse(subProcessId) && !x.IsDeleted))
            {

                throw new DuplicateRCMException(StringConstant.RcmSubProcessString);
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSubProcess rcmSubProcessDelete = await _dataRepository.FirstAsync<RiskControlMatrixSubProcess>(x => x.Id.ToString().Equals(subProcessId));
                    rcmSubProcessDelete.IsDeleted = true;
                    rcmSubProcessDelete.UpdatedDateTime = DateTime.UtcNow;
                    rcmSubProcessDelete.UpdatedBy = currentUserId;
                    _dataRepository.Update<RiskControlMatrixSubProcess>(rcmSubProcessDelete);
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

        #region Export Rcm subprocess
        /// <summary>
        /// Method for exporting Rcm subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportRcmSubProcessAsync(string entityId, int timeOffset)
        {
            var rcmSubProcessList = await _dataRepository.Where<RiskControlMatrixSubProcess>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var rcmSubProcessACList = _mapper.Map<List<RcmSubProcessAC>>(rcmSubProcessList);


            if (rcmSubProcessACList.Count == 0)
            {
                RcmSubProcessAC rcmSubProcessAC = new RcmSubProcessAC();
                rcmSubProcessACList.Add(rcmSubProcessAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            rcmSubProcessACList.ForEach(x =>
            {
                x.SubProcess = x.Id != null ? x.SubProcess : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(rcmSubProcessACList, StringConstant.RcmSubProcessString + "(" + entityName + ")");
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