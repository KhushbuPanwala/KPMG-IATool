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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository
{
    public class RcmProcessRepository : IRcmProcessRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        public RcmProcessRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        #region List RCM  Process

        /// <summary>
        /// Get all Process for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all Process with its id and name only </returns>
        public async Task<List<RcmProcessAC>> GetAllProcessForDisplayInDropDownAsync(Guid entityId)
        {
            var allProcesses = await _dataRepository.Where<RiskControlMatrixProcess>(x => !x.IsDeleted && x.EntityId == entityId).
                                            Select(x => new RcmProcessAC { Id = x.Id, Process = x.Process, EntityId = x.EntityId}).AsNoTracking().ToListAsync();
            return allProcesses;
        }

        /// <summary>
        /// Get RCM Process data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM Process</returns>
        public async Task<List<RcmProcessAC>> GetRcmProcessAsync(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            List<RiskControlMatrixProcess> rcmProcessList;
            if (!string.IsNullOrEmpty(searchString))
            {
                rcmProcessList = await _dataRepository.Where<RiskControlMatrixProcess>(a => !a.IsDeleted && a.Process.ToLower().Contains(searchString.ToLower()) && a.EntityId.ToString() == selectedEntityId
                ).Skip((page - 1 ?? 0) * pageSize)
                  .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            else
            {
                rcmProcessList = await _dataRepository.Where<RiskControlMatrixProcess>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            return _mapper.Map<List<RcmProcessAC>>(rcmProcessList);
        }

        /// <summary>
        /// Get count of RCM Process
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM Process</returns>
        public async Task<int> GetRcmProcessCountAsync(string selectedEntityId, string searchString = null)
        {
            int totalProcessRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalProcessRecords = await _dataRepository.Where<RiskControlMatrixProcess>(a => !a.IsDeleted && a.Process.ToLower().Contains(searchString.ToLower()) && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            else
            {
                totalProcessRecords = await _dataRepository.Where<RiskControlMatrixProcess>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            return totalProcessRecords;
        }

        /// <summary>
        /// Get RCM Process details
        /// </summary>
        /// <param name="processId">Process id</param>
        /// <returns>count of RCM Process</returns>
        public async Task<RcmProcessAC> GetRcmProcessByIdAsync(string processId)
        {
            RiskControlMatrixProcess rcmProcess = await _dataRepository.FirstAsync<RiskControlMatrixProcess>(a => a.Id.ToString() == processId && !a.IsDeleted);
            return _mapper.Map<RcmProcessAC>(rcmProcess);
        }

        /// <summary>
        /// Add Process detail
        /// </summary>
        /// <param name="rcmProcessAC">detail of Process</param>
        /// <returns>Task</returns>
        public async Task<RcmProcessAC> AddRcmProcess(RcmProcessAC rcmProcessAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixProcess rcmProcess = _mapper.Map<RiskControlMatrixProcess>(rcmProcessAC);
                    rcmProcess.CreatedDateTime = DateTime.UtcNow;
                    rcmProcess.CreatedBy = currentUserId;
                    await _dataRepository.AddAsync<RiskControlMatrixProcess>(rcmProcess);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmProcessAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Process detail
        /// </summary>
        /// <param name="rcmProcessAC">detail of Process</param>
        /// <returns>Task</returns>
        public async Task<RcmProcessAC> UpdateRcmProcess(RcmProcessAC rcmProcessAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixProcess rcmProcess = _mapper.Map<RiskControlMatrixProcess>(rcmProcessAC);
                    rcmProcess.UpdatedDateTime = DateTime.UtcNow;
                    rcmProcess.UpdatedBy = currentUserId;
                    _dataRepository.Update<RiskControlMatrixProcess>(rcmProcess);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmProcessAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete RCM Process detail
        /// <param name="processId">id of deleted Process</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteRcmProcess(string processId)
        {
            //Check whether it has RCM list reference
            if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.RcmProcessId == Guid.Parse(processId) && !x.IsDeleted))
            {
                throw new DuplicateRCMException(StringConstant.RiskControlMatrixString);
            }
            else if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.RcmSubProcessId == Guid.Parse(processId) && !x.IsDeleted))
            {

                throw new DuplicateRCMException(StringConstant.RcmProcessString);
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixProcess rcmProcessDelete = await _dataRepository.FirstAsync<RiskControlMatrixProcess>(x => x.Id.ToString().Equals(processId));
                    rcmProcessDelete.UpdatedDateTime = DateTime.UtcNow;
                    rcmProcessDelete.UpdatedBy = currentUserId;
                    rcmProcessDelete.IsDeleted = true;
                    _dataRepository.Update<RiskControlMatrixProcess>(rcmProcessDelete);
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

        #region Export Rcm process
        /// <summary>
        /// Method for exporting Rcm process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportRcmProcessAsync(string entityId, int timeOffset)
        {
            var rcmProcessList = await _dataRepository.Where<RiskControlMatrixProcess>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var rcmProcessACList = _mapper.Map<List<RcmProcessAC>>(rcmProcessList);

           
            if (rcmProcessACList.Count == 0)
            {
                RcmProcessAC rcmProcessAC = new RcmProcessAC();
                rcmProcessACList.Add(rcmProcessAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            rcmProcessACList.ForEach(x =>
            {
                x.Process = x.Id != null ? x.Process : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(rcmProcessACList, StringConstant.RcmProcessString + "(" + entityName + ")");
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
