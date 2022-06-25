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

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository
{
    public class RcmSectorRepository : IRcmSectorRepository
    {
        #region Private Variables
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        #endregion

        public RcmSectorRepository(IDataRepository dataRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        #region List RCM  Sector

        /// <summary>
        /// Get all sectors for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all sectors with its id and name only </returns>
        public async Task<List<RcmSectorAC>> GetAllSectorForDisplayInDropDownAsync(Guid entityId)
        {
            var allSectors = await _dataRepository.Where<RiskControlMatrixSector>(x => !x.IsDeleted && x.EntityId == entityId).
                                            Select(x => new RcmSectorAC { Id = x.Id, Sector = x.Sector, EntityId = x.EntityId}).AsNoTracking().ToListAsync();
            return allSectors;
        }

        /// <summary>
        /// Get RCM sector data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of RCM sectors</returns>
        public async Task<List<RcmSectorAC>> GetRcmSectorAsync(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            List<RiskControlMatrixSector> rcmSectorList;
            if (!string.IsNullOrEmpty(searchString))
            {
                rcmSectorList = await _dataRepository.Where<RiskControlMatrixSector>(a => !a.IsDeleted && a.Sector.ToLower().Contains(searchString.ToLower()) && a.EntityId.ToString() == selectedEntityId
                ).Skip((page - 1 ?? 0) * pageSize)
                  .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            else
            {
                rcmSectorList = await _dataRepository.Where<RiskControlMatrixSector>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            }
            return _mapper.Map<List<RcmSectorAC>>(rcmSectorList);
        }

        /// <summary>
        /// Get count of RCM sectors
        /// </summary>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="searchValue">Search value</param>
        /// <returns>count of RCM sectors</returns>
        public async Task<int> GetRcmSectorCountAsync(string selectedEntityId, string searchString = null)
        {
            int totalSectorRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalSectorRecords = await _dataRepository.Where<RiskControlMatrixSector>(a => !a.IsDeleted && a.Sector.ToLower().Contains(searchString.ToLower()) && a.EntityId.ToString() == selectedEntityId
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            else
            {
                totalSectorRecords = await _dataRepository.Where<RiskControlMatrixSector>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId)
                ).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().CountAsync();
            }
            return totalSectorRecords;

        }

        /// <summary>
        /// Get RCM Sector details
        /// </summary>
        /// <param name="sectorId">Sector id</param>
        /// <returns>count of RCM Sector</returns>
        public async Task<RcmSectorAC> GetRcmSectorByIdAsync(string sectorId)
        {
            RiskControlMatrixSector rcmSector = await _dataRepository.FirstAsync<RiskControlMatrixSector>(a => a.Id.ToString() == sectorId && !a.IsDeleted);
            return _mapper.Map<RcmSectorAC>(rcmSector);
        }

        /// <summary>
        /// Add Sector detail
        /// </summary>
        /// <param name="rcmSectorAC">detail of Sector</param>
        /// <returns>Task</returns>
        public async Task<RcmSectorAC> AddRcmSector(RcmSectorAC rcmSectorAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSector rcmSector = _mapper.Map<RiskControlMatrixSector>(rcmSectorAC);
                    rcmSector.CreatedDateTime = DateTime.UtcNow;
                    rcmSector.CreatedBy = currentUserId;
                    await _dataRepository.AddAsync<RiskControlMatrixSector>(rcmSector);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmSectorAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Sector detail
        /// </summary>
        /// <param name="rcmSectorAC">detail of Sector</param>
        /// <returns>Task</returns>
        public async Task<RcmSectorAC> UpdateRcmSector(RcmSectorAC rcmSectorAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSector rcmSector = _mapper.Map<RiskControlMatrixSector>(rcmSectorAC);
                    rcmSector.UpdatedBy = currentUserId;
                    rcmSector.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<RiskControlMatrixSector>(rcmSector);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return rcmSectorAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete RCM Sector detail
        /// <param name="sectorId">id of deleted sector</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteRcmSector(string sectorId)
        {
            //Check whether it has RCM list reference
            if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.SectorId == Guid.Parse(sectorId) && !x.IsDeleted))
            {
                throw new DuplicateRCMException(StringConstant.RiskControlMatrixString);
            }
            else if (await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.SectorId == Guid.Parse(sectorId) && !x.IsDeleted))
            {

                throw new DuplicateRCMException(StringConstant.RcmSectorString);
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrixSector rcmSectorDelete = await _dataRepository.FirstAsync<RiskControlMatrixSector>(x => x.Id.ToString().Equals(sectorId));
                    rcmSectorDelete.UpdatedBy = currentUserId;
                    rcmSectorDelete.UpdatedDateTime = DateTime.UtcNow;
                    rcmSectorDelete.IsDeleted = true;
                    _dataRepository.Update<RiskControlMatrixSector>(rcmSectorDelete);
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

        #region Export Rcm sector
        /// <summary>
        /// Method for exporting Rcm sector
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportRcmSectorAsync(string entityId, int timeOffset)
        {
            var rcmSectorList = await _dataRepository.Where<RiskControlMatrixSector>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var rcmSectorACList = _mapper.Map<List<RcmSectorAC>>(rcmSectorList);


            if (rcmSectorACList.Count == 0)
            {
                RcmSectorAC rcmSectorAC = new RcmSectorAC();
                rcmSectorACList.Add(rcmSectorAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            rcmSectorACList.ForEach(x =>
            {
                x.Sector = x.Id != null ? x.Sector : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(rcmSectorACList, StringConstant.RcmSectorString + "(" + entityName + ")");
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
