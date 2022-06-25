using AutoMapper;
using ExcelDataReader;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository
{
    public class RiskControlMatrixRepository : IRiskControlMatrixRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public IRcmSectorRepository _rcmSectorRepository;
        public IRcmProcessRepository _rcmProcessRepository;
        public IRcmSubProcessRepository _rcmSubProcessRepository;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        #endregion

        #region Public method(s)
        public RiskControlMatrixRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IGlobalRepository globalRepository,
            IRcmSectorRepository rcmSectorRepository,
            IRcmProcessRepository rcmProcessRepository,
            IRcmSubProcessRepository rcmSubProcessRepository,
            IHttpContextAccessor httpContextAccessor,
            IExportToExcelRepository exportToExcelRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _httpContextAccessor = httpContextAccessor;
            _rcmSectorRepository = rcmSectorRepository;
            _rcmProcessRepository = rcmProcessRepository;
            _rcmSubProcessRepository = rcmSubProcessRepository;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
        }

        /// <summary>
        /// Get Details of RCM for edit page
        /// </summary>
        /// <param name="id">Risk Control Matrix Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>RiskControlMatrixAC</returns>
        public async Task<RiskControlMatrixAC> GetRiskControlMatrixDetailsByIdAsync(Guid? id, string selectedEntityId)
        {
            RiskControlMatrixAC riskControlMatrixAC = new RiskControlMatrixAC();

            if (id != null)
            {
                RiskControlMatrix riskControlMatrix = await _dataRepository.Where<RiskControlMatrix>(x => x.Id == id && !x.IsDeleted && x.EntityId.ToString() == selectedEntityId).OrderByDescending(a => a.CreatedDateTime).FirstOrDefaultAsync();
                if (riskControlMatrix == null)
                {
                    throw new NoRecordException(StringConstant.WorkProgramString);
                }
                riskControlMatrixAC = _mapper.Map<RiskControlMatrixAC>(riskControlMatrix);
            }

            List<RiskControlMatrixSector> riskControlMatrixSectorList = await _dataRepository.Where<RiskControlMatrixSector>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
            ).ToListAsync();
            riskControlMatrixAC.RiskControlMatrixSectorACList = _mapper.Map<List<RcmSectorAC>>(riskControlMatrixSectorList);

            List<RiskControlMatrixProcess> riskControlMatrixProcessList = await _dataRepository.Where<RiskControlMatrixProcess>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
            ).ToListAsync();
            riskControlMatrixAC.RiskControlMatrixProcessACList = _mapper.Map<List<RcmProcessAC>>(riskControlMatrixProcessList);

            List<RiskControlMatrixSubProcess> riskControlMatrixSubProcessList = await _dataRepository.Where<RiskControlMatrixSubProcess>(x => !x.IsDeleted && x.EntityId.ToString() == selectedEntityId
            ).ToListAsync();
            riskControlMatrixAC.RiskControlMatrixSubProcessACList = _mapper.Map<List<RcmSubProcessAC>>(riskControlMatrixSubProcessList);


            return riskControlMatrixAC;
        }
        /// <summary>
        /// Add RCM
        /// </summary>
        /// <param name="riskControlMatrixAC">Application class of Risk Control Matrix</param>
        /// <returns>RiskControlMatrixAC</returns>
        public async Task<RiskControlMatrixAC> AddRiskControlMatrixAsync(RiskControlMatrixAC riskControlMatrixAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var entityId = riskControlMatrixAC.EntityId;
                    RiskControlMatrix riskControlMatrix = _mapper.Map<RiskControlMatrix>(riskControlMatrixAC);
                    riskControlMatrix.EntityId = entityId;
                    riskControlMatrix.WorkProgram = null;
                    riskControlMatrix.CreatedDateTime = DateTime.UtcNow;
                    riskControlMatrix.CreatedBy = currentUserId;
                    riskControlMatrix.RcmSubProcess = null;
                    riskControlMatrix.RcmProcess = null;
                    riskControlMatrix.RcmSector = null;

                    await _dataRepository.AddAsync(riskControlMatrix);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    return riskControlMatrixAC;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update RCM
        /// </summary>
        /// <param name="riskControlMatrixListAC">Application class of Risk Control Matrix List</param>
        /// <returns>RiskControlMatrixAC List</returns>
        public async Task<List<RiskControlMatrixAC>> UpdateRiskControlMatrixAsync(List<RiskControlMatrixAC> riskControlMatrixListAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    

                    List<RiskControlMatrix> riskControlMatrixList = new List<RiskControlMatrix>();
                    for (var i = 0; i < riskControlMatrixListAC.Count(); i++)
                    {
                        RiskControlMatrix riskControlMatrix = _mapper.Map<RiskControlMatrix>(riskControlMatrixListAC[i]);
                        riskControlMatrix.UpdatedDateTime = DateTime.UtcNow;
                        riskControlMatrix.UpdatedBy = currentUserId;

                        riskControlMatrix.StrategicAnalysis = null;
                        riskControlMatrix.WorkProgram = null;
                        riskControlMatrixList.Add(riskControlMatrix);

                    }
                    _dataRepository.UpdateRange(riskControlMatrixList);
                    await _dataRepository.SaveChangesAsync();


                    // delete responses for the rcm 
                    if (riskControlMatrixListAC.Any(x => x.IsToDelete))
                    {
                        var rcmIdList = riskControlMatrixListAC.Select(x => x.Id).ToList();
                        var allUserResponses = _dataRepository.Where<UserResponse>(x => !x.IsDeleted && rcmIdList.Contains(x.RiskControlMatrixId)).AsNoTracking().ToList();

                        for (var i = 0; i < allUserResponses.Count(); i++)
                        {
                            allUserResponses[i].UpdatedDateTime = DateTime.UtcNow;
                            allUserResponses[i].UpdatedBy = currentUserId;
                            allUserResponses[i].IsDeleted = true;
                        }

                        _dataRepository.UpdateRange(allUserResponses);
                        _dataRepository.SaveChanges();
                    }
                    
                    transaction.Commit();
                    return riskControlMatrixListAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Get RCM list for workprogram
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <param name="selectedEntityId">Selected entityId</param> 
        /// <returns>Pagination object</returns>
        public async Task<Pagination<RiskControlMatrixAC>> GetRCMListForWorkProgramAsync(Pagination<RiskControlMatrixAC> pagination, Guid? workProgramId, string selectedEntityId)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<RiskControlMatrix> riskControlMatrixList = _dataRepository.Where<RiskControlMatrix>(x =>
                                             (!String.IsNullOrEmpty(workProgramId.ToString()) ? x.WorkProgramId == workProgramId : true)
                                           && (!String.IsNullOrEmpty(pagination.searchText) ? x.RiskDescription.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                           && !x.IsDeleted && !x.IsDeleted && x.EntityId.ToString() == selectedEntityId).Include(x => x.RcmSubProcess).Include(x => x.RcmProcess);

            //Get total count
            pagination.TotalRecords = riskControlMatrixList.Count();

            List<RiskControlMatrix> riskControlMatriceFinalList = new List<RiskControlMatrix>();
            if (pagination.PageSize == 0)
            {
                riskControlMatriceFinalList = await riskControlMatrixList.Where(x => x.WorkProgramId == null).OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            }
            else
            {
                riskControlMatriceFinalList = await riskControlMatrixList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            }
            pagination.Items = _mapper.Map<List<RiskControlMatrixAC>>(riskControlMatriceFinalList);
            return pagination;
        }

        /// <summary>
        /// Delete RCM detail
        /// <param name="rcmId">id of deleted RCM</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteRcmAsync(Guid rcmId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskControlMatrix rcmDelete = await _dataRepository.FirstAsync<RiskControlMatrix>(x => x.Id == rcmId && !x.IsDeleted);
                    rcmDelete.IsDeleted = true;
                    rcmDelete.UpdatedDateTime = DateTime.UtcNow;
                    rcmDelete.UpdatedBy = currentUserId;
                    _dataRepository.Update<RiskControlMatrix>(rcmDelete);
                    await _dataRepository.SaveChangesAsync();

                    // delte all sampling response on delete of rcm
                    var allUserResponses = _dataRepository.Where<UserResponse>(x => !x.IsDeleted && x.RiskControlMatrixId == rcmId).AsNoTracking().ToList();

                    if(allUserResponses.Count > 0)
                    {
                        for (var i = 0; i < allUserResponses.Count(); i++)
                        {
                            allUserResponses[i].UpdatedDateTime = DateTime.UtcNow;
                            allUserResponses[i].UpdatedBy = currentUserId;
                            allUserResponses[i].IsDeleted = true;
                        }

                        _dataRepository.UpdateRange(allUserResponses);
                        _dataRepository.SaveChanges();
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
        #endregion

        #region Bulk Upload

        /// <summary>
        /// Get rcm related data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity I</param>
        /// <returns>Detail for rcm bulk upload</returns>
        public async Task<RCMUploadAC> GetRCMUploadDetailAsync(string selectedEntityId)
        {
            RCMUploadAC rcmUploadAC = new RCMUploadAC();
            List<RcmSectorAC> rcmSectorAC = await _rcmSectorRepository.GetAllSectorForDisplayInDropDownAsync(new Guid(selectedEntityId));
            List<RcmProcessAC> rcmProcessAC = await _rcmProcessRepository.GetAllProcessForDisplayInDropDownAsync(new Guid(selectedEntityId));
            List<RcmSubProcessAC> rcmSubProcessAC = await _rcmSubProcessRepository.GetAllSubProcessesForDisplayInDropDownAsync(new Guid(selectedEntityId));

            rcmUploadAC.SectorList = rcmSectorAC;
            rcmUploadAC.ProcessList = rcmProcessAC;
            rcmUploadAC.SubProcessList = rcmSubProcessAC;
            rcmUploadAC.ControlCategory = GetEnumList<ControlCategory>();
            rcmUploadAC.ControlType = GetEnumList<ControlType>();
            rcmUploadAC.NatureOfControl = GetEnumList<NatureOfControl>();
            return rcmUploadAC;
        }

        /// <summary>
        /// Upload rcm data
        /// </summary>
        /// <param name="file">Upload file</param>
        /// <returns></returns>
        public async Task UploadRCMAsync(BulkUpload bulkUpload)
        {
            List<RiskControlMatrix> rcms = new List<RiskControlMatrix>();
            List<BulkUploadRiskControlMatrixAC> bulkRCM = new List<BulkUploadRiskControlMatrixAC>();

            IFormFile file = bulkUpload.Files[0];

            #region File name check 
            string fileName = file.FileName.Replace(".xlsx", "");
            String[] strlist = fileName.Split("$$");
            string entityName;
            double entityVersion;
            if (strlist.Length > 1)
            {
                entityName = fileName.Split("$$")[1];
                entityVersion = Convert.ToDouble(fileName.Split("$$")[2].Split('(')[0].Trim());
            }
            else
            {
                throw new InvalidFileException(fileName);
            }
            #endregion


            file.OpenReadStream();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream.BaseStream))
                {
                    DataTable table = reader.AsDataSet().Tables[1];
                    List<DataRow> dataRows = new List<DataRow>();

                    if (!table.ExtendedProperties["visiblestate"].ToString().Trim().ToLowerInvariant().Equals("hidden"))
                    {
                        table = _globalRepository.SetTableColumnName<BulkUploadRiskControlMatrixAC>(table);
                        bulkRCM = _globalRepository.ConvertDataTable<BulkUploadRiskControlMatrixAC>(table);

                        // check entity exist or not 
                        var entity = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Name == entityName && x.Version == entityVersion && x.IsStrategyAnalysisDone);
                        List<RcmSectorAC> sectorList = new List<RcmSectorAC>();
                        List<RcmProcessAC> processList = new List<RcmProcessAC>();
                        List<RcmSubProcessAC> subProcessList = new List<RcmSubProcessAC>();
                        if (entity != null)
                        {
                            if(entity.Id == Guid.Empty)
                            {
                                throw new NotExistException("entity id not exist");
                            }

                            sectorList = await _rcmSectorRepository.GetAllSectorForDisplayInDropDownAsync(entity.Id);
                            if(sectorList.Count == 0)
                            {
                                throw new NotExistException("no sectors in db for this entity");
                            }
                            var procList= await _dataRepository.Where<RiskControlMatrixProcess>(x => !x.IsDeleted && x.EntityId == entity.Id).
                                            Select(x => new RcmProcessAC { Id = x.Id, Process = x.Process, EntityId = x.EntityId }).AsNoTracking().ToListAsync();

                            processList = await _rcmProcessRepository.GetAllProcessForDisplayInDropDownAsync(entity.Id);
                            if (processList.Count == 0)
                            {
                                throw new NotExistException("no process in db for this entity");
                            }
                            subProcessList = await _rcmSubProcessRepository.GetAllSubProcessesForDisplayInDropDownAsync(entity.Id);
                            if (subProcessList.Count == 0)
                            {
                                throw new NotExistException("no subprocesses in db for this entity");
                            }
                            bulkRCM = await ValidateRCMData(bulkRCM, sectorList, processList, subProcessList);
                        }
                        else
                        {
                            //invalid auditable entity
                            throw new InvalidBulkDataException(StringConstant.AuditableEntityModuleName, entityName);
                        }
                        using (var transaction = _dataRepository.BeginTransaction())
                        {
                            try
                            {
                                foreach (var item in bulkRCM)
                                {

                                    RiskControlMatrix riskControlMatrix = new RiskControlMatrix();
                                    var rcmSector= sectorList.FirstOrDefault(a => a.Sector == item.Sector.Trim());
                                    riskControlMatrix.SectorId = rcmSector != null ? (Guid)rcmSector.Id : Guid.Empty;

                                    var rcmProcess = processList.FirstOrDefault(a => a.Process == item.Process.Trim());
                                    riskControlMatrix.RcmProcessId = rcmProcess != null ? (Guid)rcmProcess.Id : Guid.Empty;

                                    var rcmSubProcess = subProcessList.FirstOrDefault(a => a.SubProcess == item.SubProcess.Trim());
                                    riskControlMatrix.RcmSubProcessId = rcmSubProcess != null ? (Guid)rcmSubProcess.Id : Guid.Empty;

                                    riskControlMatrix.RiskDescription = item.RiskDescription; // Risk Description
                                    //Control category - drop down
                                    if (item.ControlCategory == ControlCategory.Strategic.ToString() ||
                                        item.ControlCategory == ControlCategory.Operational.ToString() ||
                                        item.ControlCategory == ControlCategory.Financial.ToString() ||
                                        item.ControlCategory == ControlCategory.Compliance.ToString())
                                    {
                                        riskControlMatrix.ControlCategory = (ControlCategory)Enum.Parse(typeof(ControlCategory), item.ControlCategory);
                                    }
                                    else
                                    {
                                        throw new InvalidBulkDataException(StringConstant.controlCategoryText, item.ControlCategory);
                                    }
                                    // control type - drop down
                                    if (item.ControlType == ControlType.Manual.ToString() ||
                                        item.ControlType == ControlType.Automated.ToString() ||
                                        item.ControlType == ControlType.SemiAutomated.ToString())
                                    {
                                        riskControlMatrix.ControlType = (ControlType)Enum.Parse(typeof(ControlType), item.ControlType);
                                    }
                                    else
                                    {
                                        throw new InvalidBulkDataException(StringConstant.controlTypeText, item.ControlType);
                                    }
                                    riskControlMatrix.ControlObjective = item.ControlObjective; // Control Objective
                                    riskControlMatrix.ControlDescription = item.ControlDescription; // Control Description
                                    // nature of control - drop down
                                    if (item.NatureOfControl == NatureOfControl.Preventive.ToString() ||
                                        item.NatureOfControl == NatureOfControl.Detective.ToString())
                                    {
                                        riskControlMatrix.NatureOfControl = (NatureOfControl)Enum.Parse(typeof(NatureOfControl), item.NatureOfControl);
                                    }
                                    else
                                    {
                                        throw new InvalidBulkDataException(StringConstant.controlTypeText, item.ControlType);
                                    }
                                    // anti fraud control - drop down
                                    riskControlMatrix.AntiFraudControl = item.AntiFraudControl == "Yes" ? true : false;
                                    riskControlMatrix.RiskCategory = item.RiskCategory; // Risk category                            
                                    riskControlMatrix.EntityId = bulkUpload.EntityId;
                                    riskControlMatrix.CreatedDateTime = DateTime.UtcNow;
                                    riskControlMatrix.CreatedBy = currentUserId;
                                    rcms.Add(riskControlMatrix);
                                }
                                await _dataRepository.AddRangeAsync<RiskControlMatrix>(rcms);
                                _dataRepository.SaveChanges();
                                await transaction.CommitAsync();

                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Export Rcm main
        /// <summary>
        /// Method for exporting Rcm main
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportRcmMainAsync(string entityId, int timeOffset)
        {
            var rcmList = await _dataRepository.Where<RiskControlMatrix>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                        .Include(x => x.AuditableEntity).Include(x => x.RcmSector).Include(x => x.RcmProcess).Include(x => x.RcmSubProcess).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            var rcmACList = _mapper.Map<List<RiskControlMatrixACForExcel>>(rcmList);

            if (rcmACList.Count == 0)
            {
                RiskControlMatrixACForExcel riskControlMatrixAC = new RiskControlMatrixACForExcel();
                rcmACList.Add(riskControlMatrixAC);
            }

            var getEntityName = await _dataRepository.FirstOrDefaultAsync<AuditableEntity>(x => !x.IsDeleted && x.Id == Guid.Parse(entityId));
            string entityName = getEntityName.Name;
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            rcmACList.ForEach(x =>
            {
                x.RiskName = x.Id != null ? x.RiskName : string.Empty;
                x.RiskDescription = string.IsNullOrEmpty(x.RiskDescription) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.RiskDescription, string.Empty));
                x.ControlCategoryString = (x.Id != null && x.ControlCategory.ToString() == StringConstant.StrategicControlCategory) ? StringConstant.StrategicControlCategory : x.ControlCategory.ToString() == StringConstant.OperationalControlCategory ? StringConstant.OperationalControlCategory : x.ControlCategory.ToString() == StringConstant.FinancialControlCategory ? StringConstant.FinancialControlCategory : x.ControlCategory.ToString() == StringConstant.ComplianceControlCategory ? StringConstant.ComplianceControlCategory : string.Empty;
                x.ControlTypeString = (x.Id != null && x.ControlType.ToString() == StringConstant.ManualControlType) ? StringConstant.ManualControlType : x.ControlType.ToString() == StringConstant.AutomatedControlType ? StringConstant.AutomatedControlType : x.ControlType.ToString() == StringConstant.SemiAutomatedControlType ? StringConstant.SemiAutomatedControlType : string.Empty;
                x.ControlObjective = string.IsNullOrEmpty(x.ControlObjective) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.ControlObjective, string.Empty));
                x.ControlDescription = string.IsNullOrEmpty(x.ControlDescription) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.ControlDescription, string.Empty));
                x.NatureOfControlString = (x.Id != null && x.NatureOfControl.ToString() == StringConstant.PreventiveNatureOfControl) ? StringConstant.PreventiveNatureOfControl : x.NatureOfControl.ToString() == StringConstant.DetectiveNatureOfControl ? StringConstant.DetectiveNatureOfControl : string.Empty;
                x.AntiFraudControlString = (x.Id != null && x.AntiFraudControl == true) ? StringConstant.Yes : x.AntiFraudControl == false ? StringConstant.No : string.Empty;
                x.RcmSectorName = x.Id != null ? x.RcmSectorName : string.Empty;
                x.RiskCategory = string.IsNullOrEmpty(x.RiskCategory) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.RiskCategory, string.Empty));
                x.RcmProcessName = x.Id != null ? x.RcmProcessName : string.Empty;
                x.RcmSubProcessName = x.Id != null ? x.RcmSubProcessName : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(rcmACList, StringConstant.RiskControlMatrixString + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion        

        #region Private methods
        /// <summary>
        /// convert enum value to key,value pair list
        /// </summary>
        /// <typeparam name="T">EnumType</typeparam>
        /// <returns>List of key value for specific enum</returns>
        private List<KeyValuePair<int, string>> GetEnumList<T>()
        {
            var list = new List<KeyValuePair<int, string>>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                list.Add(new KeyValuePair<int, string>((int)e, e.ToString()));
            }
            return list;
        }

        /// <summary>
        /// Validate uploaded observation
        /// </summary>
        /// <param name="bulkObservations">Uploaded observation list</param>
        /// <param name="auditPlanId">Selected plan id</param>
        /// <returns>Validated observation list</returns>
        private async Task<List<BulkUploadRiskControlMatrixAC>> ValidateRCMData(List<BulkUploadRiskControlMatrixAC> bulkRCMs, List<RcmSectorAC> sectorList, List<RcmProcessAC> processList, List<RcmSubProcessAC> subProcessList)
        {
            List<BulkUploadRiskControlMatrixAC> rcmList = new List<BulkUploadRiskControlMatrixAC>();

            foreach (BulkUploadRiskControlMatrixAC rcmData in bulkRCMs)
            {

                if (string.IsNullOrEmpty(rcmData.Sector))
                {
                    throw new RequiredDataException(StringConstant.sectorText);
                }
                if(sectorList.Count > 0)
                {
                    var existingSector = sectorList.FirstOrDefault(a => a.Sector == rcmData.Sector.Trim());
                    if (existingSector == null)
                    {
                        throw new NotExistException(StringConstant.sectorText + "-" + rcmData.Sector.Trim());
                    }
                }
                
                if (string.IsNullOrEmpty(rcmData.Process))
                {
                    throw new RequiredDataException(StringConstant.processText);
                }

                if(processList.Count > 0)
                {
                    var existingProcess = processList.FirstOrDefault(a => a.Process == rcmData.Process.Trim());
                    if (existingProcess == null)
                    {
                        throw new NotExistException(StringConstant.processText +"-"+ rcmData.Process.Trim());
                    }
                }
                
                if (string.IsNullOrEmpty(rcmData.SubProcess))
                {
                    throw new RequiredDataException(StringConstant.subProcessText);
                }

                if(subProcessList.Count > 0)
                {
                    var existingSubProcess = subProcessList.FirstOrDefault(a => a.SubProcess == rcmData.SubProcess.Trim());

                    if (existingSubProcess == null)
                    {
                        throw new NotExistException(StringConstant.subProcessText + "-" + rcmData.SubProcess.Trim());
                    }
                }
               
                if (string.IsNullOrEmpty(rcmData.RiskDescription))
                {
                    throw new RequiredDataException(StringConstant.riskDescriptionText);
                }
                if (string.IsNullOrEmpty(rcmData.ControlCategory))
                {
                    throw new RequiredDataException(StringConstant.controlCategoryText);
                }
                if (string.IsNullOrEmpty(rcmData.ControlType))
                {
                    throw new RequiredDataException(StringConstant.controlTypeText);
                }
                if (string.IsNullOrEmpty(rcmData.ControlObjective))
                {
                    throw new RequiredDataException(StringConstant.controlObjectiveText);
                }
                if (string.IsNullOrEmpty(rcmData.ControlDescription))
                {
                    throw new RequiredDataException(StringConstant.controlDescriptionText);
                }
                if (string.IsNullOrEmpty(rcmData.NatureOfControl))
                {
                    throw new RequiredDataException(StringConstant.natureOfControlText);
                }
                if (string.IsNullOrEmpty(rcmData.AntiFraudControl))
                {
                    throw new RequiredDataException(StringConstant.antiFraudControlText);
                }
                if (string.IsNullOrEmpty(rcmData.RiskCategory))
                {
                    throw new RequiredDataException(StringConstant.riskCategoryText);
                }
                rcmList.Add(rcmData);
            }
            return rcmList;
        }

        #endregion
    }
}

