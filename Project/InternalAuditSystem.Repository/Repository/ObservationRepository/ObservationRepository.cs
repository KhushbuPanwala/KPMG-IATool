using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using System.Text.Json;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;
using Microsoft.AspNetCore.Authentication;
using System.IO;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using System.Data;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using ExcelDataReader;
using InternalAuditSystem.Utility.FileUtil;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using System.Net;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using Microsoft.AspNetCore.Hosting;

namespace InternalAuditSystem.Repository.Repository.ObservationRepository
{
    public class ObservationRepository : IObservationRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepo;
        private readonly IMapper _mapper;
        public IAuditPlanRepository _auditPlanRepository;
        public IAuditProcessSubProcessRepository _auditProcessSubProcessRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IGlobalRepository _globalRepository;
        public IDynamicTableRepository _dynamicTableRepository;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneratePPTRepository _generatePPTRepository;
        public Guid currentUserId = Guid.Empty;
        private readonly IWebHostEnvironment _environment;
        public ObservationRepository(IDataRepository dataRepository, IMapper mapper, IAuditPlanRepository auditPlanRepository,
            IAuditProcessSubProcessRepository auditProcessSubProcessRepository, IExportToExcelRepository exportToExcelRepository,
            IAuditableEntityRepository auditableEntityRepository, IGlobalRepository globalRepository, IDynamicTableRepository dynamicTableRepository,
            IFileUtility fileUtility, IAzureRepository azureRepo, IHttpContextAccessor httpContextAccessor,
            IGeneratePPTRepository generatePPTRepository, IWebHostEnvironment environment)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditPlanRepository = auditPlanRepository;
            _auditProcessSubProcessRepository = auditProcessSubProcessRepository;
            _dynamicTableRepository = dynamicTableRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _globalRepository = globalRepository;
            _fileUtility = fileUtility;
            _azureRepo = azureRepo;
            _httpContextAccessor = httpContextAccessor;
            _generatePPTRepository = generatePPTRepository;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _environment = environment;
        }

        #region Observation CRUD

        /// <summary>
        /// Get Observation  data
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="subProcessId">SubProcess Id</param>
        /// <returns>List of Observation</returns>
        public async Task<List<Observation>> GetObservationsByPlanSubProcessIdAsync(string planId, string subProcessId)
        {
            List<Observation> observationList = await _dataRepository.Where<Observation>(a => !a.IsDeleted
                    && a.AuditPlanId.ToString() == planId && a.ProcessId.ToString() == subProcessId).AsNoTracking().ToListAsync();
            return observationList;
        }

        /// <summary>
        /// Get all the observations with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="entityId">Id of auditable entity </param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>List of observations</returns>
        public async Task<List<ObservationAC>> GetAllObservationsAsync(int? pageIndex, int? pageSize, string searchString, string entityId, int fromYear, int toYear)
        {
            try
            {
                List<Observation> observationList;
                //return list as per search string 
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (fromYear != 0 && toYear != 0)
                    {
                        observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)
                                                               && x.Heading.ToLower().Contains(searchString.ToLower()) && (x.CreatedDateTime.Year <= toYear && x.CreatedDateTime.Year >= fromYear))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.Process)
                                                              .AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)
                                                               && x.Heading.ToLower().Contains(searchString.ToLower()))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                              .Take((int)pageSize)
                                                              .Include(x => x.Process)
                                                              .AsNoTracking().ToListAsync();
                    }
                }
                else
                {
                    //when only to get all observations without page wise
                    if (pageIndex == null && pageSize == 0)
                    {
                        if (fromYear != 0 && toYear != 0)
                        {
                            observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && (x.CreatedDateTime.Year <= toYear && x.CreatedDateTime.Year >= fromYear))
                                                                  .OrderByDescending(x => x.CreatedDateTime)
                                                                  .Include(x => x.Process)
                                                                  .AsNoTracking().ToListAsync();
                        }
                        else
                        {
                            observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                              .OrderByDescending(x => x.CreatedDateTime)
                                                              .Include(x => x.Process)
                                                              .AsNoTracking().ToListAsync();
                        }

                    }
                    //when only to get client participant data without searchstring 
                    else
                    {
                        if (fromYear != 0 && toYear != 0)
                        {
                            observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && (x.CreatedDateTime.Year <= toYear && x.CreatedDateTime.Year >= fromYear))
                                .OrderByDescending(x => x.CreatedDateTime)
                                                                  .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                                  .Take((int)pageSize)
                                                                  .Include(p => p.Process)
                                                                  .AsNoTracking().ToListAsync();
                        }
                        else
                        {
                            observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                 .OrderByDescending(x => x.CreatedDateTime)
                                                                  .Skip((pageIndex - 1 ?? 0) * (int)pageSize)
                                                                  .Take((int)pageSize)
                                                                  .Include(p => p.Process)
                                                                  .AsNoTracking().ToListAsync();
                        }

                    }

                }
                //order by CreatedDateTime 
                observationList.OrderByDescending(a => a.CreatedDateTime);
                List<UserAC> ObservationPersonResposibleACList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.User.UserType == UserType.External).Include(y => y.User).Select(x => new UserAC { Id = x.User.Id, Name = x.User.Name, Designation = x.User.Designation, UserType = x.User.UserType, EmailId = x.User.EmailId }).ToListAsync();
                var observationACList = _mapper.Map<List<ObservationAC>>(observationList);
                HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(observationACList.Select(s => s.AuditPlanId));
                List<ProcessAC> allProcessACList = await _dataRepository.Where<PlanProcessMapping>(x => getObservationAuditPlanIds.Contains(x.PlanId) && !x.IsDeleted)
                                                          .OrderBy(x => x.CreatedDateTime)
                                                          .Select(x => new ProcessAC { Id = x.Process.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsNoTracking().ToListAsync();
                var parentProcessIdList = allProcessACList.Select(x => x.ParentId).ToList();
                var allParentProcessDetails = _dataRepository.Where<Process>(x => !x.IsDeleted && parentProcessIdList.Contains(x.Id))
                                                             .Distinct().OrderBy(x => x.Name)
                                                             .Select(x => new ProcessAC { Id = x.Id, Name = x.Name, ParentId = x.ParentId });
                allProcessACList.AddRange(allParentProcessDetails);
                for (int i = 0; i < observationACList.Count; i++)
                {
                    observationACList[i].EntityId = Guid.Parse(entityId);
                    observationACList[i].UserAC = ObservationPersonResposibleACList.FirstOrDefault(x => x.Id == observationACList[i].PersonResponsible);
                    if (observationACList[i].ObservationListStatus == ObservationStatus.Pending)
                        observationACList[i].StatusString = ObservationStatus.Pending.ToString();
                    else if (observationACList[i].ObservationListStatus == ObservationStatus.Closed)
                        observationACList[i].StatusString = ObservationStatus.Completed.ToString();
                    observationACList[i].ParentProcessId = allProcessACList.FirstOrDefault(x => x.Id == observationACList[i].ProcessId)?.ParentId;
                    observationACList[i].ProcessName = allProcessACList.FirstOrDefault(x => x.Id == observationACList[i].ParentProcessId)?.Name;

                }

                List<Guid> observationIdList = observationACList.Select(x => x.Id ?? new Guid()).ToList();

                var observationDocuments = await _dataRepository.Where<ObservationDocument>(x => observationIdList.Contains(x.ObservationId) && !x.IsDeleted).AsNoTracking().ToListAsync();

                List<ObservationDocumentAC> observationDocumentACList = _mapper.Map<List<ObservationDocument>, List<ObservationDocumentAC>>(observationDocuments);

                if (observationDocumentACList != null && observationDocumentACList.Count() > 0)
                {
                    for (int j = 0; j < observationACList.Count(); j++)
                    {
                        observationACList[j].ObservationDocuments =
                            observationDocumentACList.Where(x => x.ObservationId == observationACList[j].Id).OrderByDescending(x => x.CreatedDateTime).ToList();
                        for (int k = 0; k < observationACList[j].ObservationDocuments.Count(); k++)
                        {
                            observationACList[j].ObservationDocuments[k].FileName = observationACList[j].ObservationDocuments[k].DocumentPath;
                            observationACList[j].ObservationDocuments[k].DocumentPath = _azureRepo.DownloadFile(observationACList[j].ObservationDocuments[k].DocumentPath);
                        }
                    }
                }

                return observationACList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="entityId">Selected entityId</param>
        /// <returns>Total no of records found for a search string</returns>
        public async Task<int> GetTotalObservationsPerSearchStringAsync(string searchString, string entityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)
                                                                                &&
                                                                                x.Heading.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.CountAsync<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId));
            }

            return totalRecords;
        }

        /// <summary>
        /// Get details of observation by Id
        /// </summary>
        /// <param name="observationId">Id of observation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Application class of observation</returns>
        public async Task<ObservationAC> GetObservationDetailsByIdAsync(string observationId, string entityId)
        {
            try
            {
                List<Observation> observationList = new List<Observation>();
                Observation observation = new Observation();
                ObservationAC observationAC = new ObservationAC();
                List<AuditPlanAC> allPlanList = new List<AuditPlanAC>();
                if (observationId != null)
                {
                    observation = await _dataRepository.FirstAsync<Observation>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId) && a.Id.ToString() == observationId);
                    observationAC = _mapper.Map<ObservationAC>(observation);
                    var observationDocuments = await _dataRepository.Where<ObservationDocument>(x => x.ObservationId == observationAC.Id && !x.IsDeleted).AsNoTracking().ToListAsync();

                    List<ObservationDocumentAC> observationDocumentACList = _mapper.Map<List<ObservationDocument>, List<ObservationDocumentAC>>(observationDocuments);
                    for (int i = 0; i < observationDocumentACList.Count(); i++)
                    {
                        observationDocumentACList[i].FileName = observationDocumentACList[i].DocumentPath;
                        observationDocumentACList[i].DocumentPath = _azureRepo.DownloadFile(observationDocumentACList[i].DocumentPath);
                    }

                    observationAC.ObservationDocuments = observationDocumentACList;
                }
                // get all observations
                observationList = await _dataRepository.Where<Observation>(a => !a.IsDeleted && a.EntityId == Guid.Parse(entityId)).AsNoTracking().ToListAsync();
                List<ObservationAC> ObservationACList = _mapper.Map<List<ObservationAC>>(observationList);

                #region bind data for  observation

                allPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(Guid.Parse(entityId));

                observationAC.AuditPlanList = allPlanList;
                if (observationId != null)
                {
                    var planDetail = allPlanList.Find(a => a.Id == observationAC.AuditPlanId);

                    observationAC.ParentProcessList = planDetail.ParentProcessList;
                    observationAC.ParentProcessId = _dataRepository.Where<Process>(x => x.Id == observation.ProcessId).FirstOrDefault()?.ParentId;
                    observationAC.ProcessList = planDetail.PlanProcessList.Where(x => x.ParentProcessId == observationAC.ParentProcessId).ToList();
                }

                var categoryList = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).ToListAsync();
                observationAC.ObservationCategoryList = _mapper.Map<List<ObservationCategoryAC>>(categoryList);

                List<UserAC> ObservationPersonResposibleACList = await _dataRepository.Where<EntityUserMapping>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId) && x.User.UserType == UserType.External).Include(y => y.User).Select(x => new UserAC { Id = x.User.Id, Name = x.User.Name, Designation = x.User.Designation, UserType = x.User.UserType, EmailId = x.User.EmailId }).ToListAsync();
                //assign person responsible of observation
                observationAC.PersonResponsibleList = ObservationPersonResposibleACList;
                observationAC.LinkedObservationACList = ObservationACList;
                #endregion

                return observationAC;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Method for adding observation data
        /// </summary>
        /// <param name="observationAC">Application class of observation</param>
        /// <returns>Object of newly added observation</returns>
        public async Task<ObservationAC> AddObservationAsync(ObservationAC observationAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    Observation observation = new Observation();

                    Guid auditPlanId = observationAC.AuditPlanId;
                    Guid parentProcessId = (Guid)observationAC.ParentProcessId;
                    observationAC.Id = Guid.NewGuid();
                    var listStatus = observationAC.StatusString;
                    observation = _mapper.Map<Observation>(observationAC);

                    observation.CreatedDateTime = DateTime.UtcNow;
                    observation.CreatedBy = currentUserId;
                    observation.Process = null;
                    observationAC = _mapper.Map<ObservationAC>(observation);
                    //Add observation
                    await _dataRepository.AddAsync(observation);
                    await _dataRepository.SaveChangesAsync();

                    observationAC.AuditPlanId = auditPlanId;
                    observationAC.StatusString = listStatus;
                    observationAC.ParentProcessId = parentProcessId;
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();

                    return observationAC;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method for updating observation
        /// </summary>
        /// <param name="observationAC">Application class of observation</param>
        /// <returns>Updated data of observation</returns>
        public async Task<ObservationAC> UpdateObservationAsync(ObservationAC observationAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    Observation observation = new Observation();
                    Guid auditPlanId = observationAC.AuditPlanId;
                    Guid parentProcessId = (Guid)observationAC.ParentProcessId;
                    var listStatus = observationAC.StatusString;
                    observation = _mapper.Map<Observation>(observationAC);

                    observation.UpdatedDateTime = DateTime.UtcNow;
                    observation.UpdatedBy = currentUserId;
                    observation.Process = null;

                    //Update observation
                    _dataRepository.Update(observation);
                    await _dataRepository.SaveChangesAsync();
                    observationAC = _mapper.Map<ObservationAC>(observation);

                    observationAC.AuditPlanId = auditPlanId;
                    observationAC.StatusString = listStatus;
                    observationAC.ParentProcessId = parentProcessId;

                    observationAC.ObservationDocuments = _mapper.Map<List<ObservationDocumentAC>>(await _dataRepository.Where<ObservationDocument>(x => x.ObservationId == observationAC.Id && !x.IsDeleted).AsNoTracking().ToListAsync());

                    transaction.Commit();

                    return observationAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Method for deleting observation
        /// </summary>
        /// <param name="id">Id of observation</param>
        /// <returns>Task</returns>
        public async Task DeleteObservationAync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    Observation observation = await _dataRepository.Where<Observation>(x =>
                                                                    x.Id == id && !x.IsDeleted)
                                                                    .Include(x => x.Process)
                                                                    .FirstAsync();
                    //Delete plan process mapping data
                    observation.IsDeleted = true;
                    observation.UpdatedBy = currentUserId;
                    observation.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<Observation>(observation);
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
        /// Method for export to excel
        /// </summary>
        /// <param name="entityId">Id of auditableEntity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportObservationsAsync(string entityId, int timeOffset)
        {
            List<Observation> observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                                    .Include(a => a.AuditPlan)
                                                                   .Include(a => a.Process)
                                                                   .Include(a => a.ObservationCategory)
                                                                    .OrderByDescending(x => x.CreatedDateTime)
                                                                   .AsNoTracking().ToListAsync();


            List<ObservationAC> exportObservationList = _mapper.Map<List<ObservationAC>>(observationList);
            if (exportObservationList.Count == 0)
            {
                ObservationAC observationAC = new ObservationAC();
                exportObservationList.Add(observationAC);
            }
            //get added observation person resposibleids
            HashSet<Guid> getObservationPersonIds = new HashSet<Guid>(exportObservationList.Select(s => s.PersonResponsible));
            //get added observation ids
            HashSet<Guid> getObservationAuditPlanIds = new HashSet<Guid>(exportObservationList.Select(s => s.AuditPlanId));
            List<AuditPlan> auditPlanList = await _dataRepository.Where<AuditPlan>(x => !x.IsDeleted && getObservationAuditPlanIds.Contains(x.Id)).AsNoTracking().ToListAsync();

            HashSet<Guid> getObservationprocessIds = new HashSet<Guid>(exportObservationList.Select(s => s.ProcessId));
            //Get person resposible
            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted && getObservationPersonIds.Contains(a.UserId))
               .Include(a => a.User).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

            var categoryList = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId)).ToListAsync();
            var ObservationCategoryList = _mapper.Map<List<ObservationCategoryAC>>(categoryList);

            List<Process> allProcessList = await _dataRepository.Where<Process>(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            List<ProcessAC> allProcessACList = _mapper.Map<List<ProcessAC>>(allProcessList);
            //convert UTC date time to system date time format
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            exportObservationList.ForEach(a =>
            {
                // convert html contant to text
                a.ParentProcessId = allProcessACList.FirstOrDefault(x => x.Id == a.ProcessId)?.ParentId;
                a.Background = string.IsNullOrEmpty(a.Background) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Background, string.Empty));
                a.RootCause = string.IsNullOrEmpty(a.RootCause) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.RootCause, string.Empty));
                a.Implication = string.IsNullOrEmpty(a.Implication) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Implication, string.Empty));
                a.Recommendation = string.IsNullOrEmpty(a.Recommendation) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Recommendation, string.Empty));
                a.Observations = string.IsNullOrEmpty(a.Observations) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Observations, string.Empty));
                a.ManagementResponse = string.IsNullOrEmpty(a.ManagementResponse) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.ManagementResponse, string.Empty));
                a.Conclusion = string.IsNullOrEmpty(a.Conclusion) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Conclusion, string.Empty));

                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.ObservationStatusToString = a.Id != null ? a.ObservationStatusToString : string.Empty;
                a.ObservationTargetDate = a.Id != null ? a.TargetDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.DispositionToString = a.Id != null ? a.DispositionToString : string.Empty;
                a.ObservationTypeToString = a.Id != null ? a.ObservationTypeToString : string.Empty;
                a.LinkedObservation = (a.Id != null && a.LinkedObservation != "" && a.LinkedObservation != null) ? exportObservationList.FirstOrDefault(x => x.Id == Guid.Parse(a.LinkedObservation))?.Heading : string.Empty;
                a.PersonResposibleName = (a.Id != null && a.PersonResponsible != null) ? entityUserMappingList.Find(x => x.UserId == a.PersonResponsible)?.User.Name : string.Empty;
                a.ObservationCategoryName = (a.Id != null && ObservationCategoryList.Count > 0 && a.ObservationCategoryId != null) ? ObservationCategoryList.Find(x => x.Id == a.ObservationCategoryId)?.CategoryName : string.Empty;
                a.AuditPlanName = (a.Id != null && a.AuditPlanId != Guid.Empty) ? auditPlanList.Find(x => x.Id == a.AuditPlanId)?.Title : string.Empty;
                a.IsRepeated = a.Id != null ? (a.IsRepeatObservation ? StringConstant.Yes : StringConstant.No) : string.Empty;
                a.StatusString = (a.Id != null && a.ObservationListStatus.ToString() == StringConstant.ClosedStatusString) ? StringConstant.CompletedStatusString : (a.ObservationListStatus.ToString() == StringConstant.PendingStatusString) ? StringConstant.PendingStatusString : string.Empty;
                a.ProcessName = a.Id != null ? allProcessACList.FirstOrDefault(x => x.Id == a.ParentProcessId)?.Name : string.Empty;
                a.SubProcessName = a.Id != null ? allProcessACList.FirstOrDefault(x => x.Id == a.ProcessId)?.Name : string.Empty;
            });

            //acm all add acm ids
            HashSet<string> observationIds = new HashSet<string>(exportObservationList.Select(s => s.Id.ToString()));

            //observation add table
            List<ObservationTable> observationsTable = _dataRepository.Where<ObservationTable>(a => !a.IsDeleted
             && observationIds.Contains(a.ObservationId.ToString())).AsNoTracking().ToList();

            List<JSONTable> jsonObservationsTable = new List<JSONTable>();
            for (int i = 0; i < observationsTable.Count; i++)
            {
                JSONTable jsonTable = new JSONTable();
                string observationName = string.Empty;
                ObservationAC observationAC = exportObservationList.FirstOrDefault(a => a.Id == observationsTable[i].ObservationId);
                if (observationAC != null)
                {

                    observationName = observationAC.Heading;

                }
                jsonTable.Name = observationName;
                jsonTable.JsonData = await GetJsonObservationTableAsync(observationsTable[i]);
                jsonObservationsTable.Add(jsonTable);
            }
            //crete dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.ObservationModuleName, exportObservationList);
            //for json data
            dynamicDictionary.Add(StringConstant.ObservationTableModuleName, jsonObservationsTable);

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(dynamicDictionary, jsonObservationsTable, StringConstant.ObservationModuleName + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Bulk Upload

        /// <summary>
        /// Get observation releated data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity I</param>
        /// <returns>Detail for observation bulk upload</returns>
        public async Task<ObservationUploadAC> GetObservationUploadDetailAsync(string selectedEntityId)
        {
            ObservationUploadAC observationUploadAC = new ObservationUploadAC();

            List<AuditPlanAC> planList = await _auditPlanRepository.GetAllAuditPlansForDisplayInDropDownAsync(new Guid(selectedEntityId));
            observationUploadAC.AuditPlanList = planList;
            observationUploadAC.ObservationType = GetEnumList<ObservationType>();
            observationUploadAC.ObservationStatus = GetEnumList<ObservationStatus>();
            observationUploadAC.Disposition = GetEnumList<Disposition>();
            return observationUploadAC;
        }

        /// <summary>
        /// Upload observation data
        /// </summary>
        /// <param name="file">Upload file</param>
        /// <returns>Task</returns>
        public async Task UploadObservationAsync(BulkUpload bulkUpload)
        {
            List<Observation> observations = new List<Observation>();
            List<BulkObservationAC> bulkObservations = new List<BulkObservationAC>();

            IFormFile file = bulkUpload.Files[0];
            string fileName = file.FileName.Replace(".xlsx", "");
            String[] strlist = fileName.Split("$$");
            string planName;
            double planVersion;
            if (strlist.Length > 1)
            {
                planName = fileName.Split("$$")[1];
                planVersion = Convert.ToDouble(fileName.Split("$$")[2].Split('(')[0].Trim());
            }
            else
            {
                throw new InvalidFileException(fileName);
            }

            file.OpenReadStream();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream.BaseStream))
                {
                    DataTable table = reader.AsDataSet().Tables[1];
                    List<DataRow> dataRows = new List<DataRow>();

                    if (!table.ExtendedProperties["visiblestate"].ToString().Trim().ToLowerInvariant().Equals("hidden"))
                    {
                        table = _globalRepository.SetTableColumnName<BulkObservationAC>(table);

                        bulkObservations = _globalRepository.ConvertDataTable<BulkObservationAC>(table);

                        //todo change: plan version
                        List<AuditPlanAC> auditPlanList = await _auditPlanRepository.GetAllAuditPlansForDisplayInDropDownAsync(bulkUpload.EntityId);
                        AuditPlanAC auditPlan = auditPlanList.FirstOrDefault(a => a.Title == planName && a.Version == planVersion);

                        if (auditPlan != null && auditPlan.Id != new Guid())
                        {
                            bulkObservations = await ValidateObservationData(bulkObservations, auditPlan.Id ?? new Guid());
                        }
                        else
                        {
                            //invalid audit plan
                            throw new InvalidBulkDataException(StringConstant.AuditPlanText, planName);
                        }

                        foreach (var item in bulkObservations)
                        {
                            Observation observation = new Observation();
                            observation.AuditPlanId = auditPlan.Id;
                            observation.ProcessId = new Guid(item.SubProcess);
                            if (item.Heading.Length > RegexConstant.MaxInputLength)
                            {
                                throw new MaxLengthDataException(StringConstant.HeadingText);
                            }
                            else
                            {
                                observation.Heading = item.Heading;
                            }
                            observation.Background = item.Background;
                            observation.Observations = item.Observations;
                            if (item.ObservationType == ObservationType.Legal.ToString() ||
                                item.ObservationType == ObservationType.Compliance.ToString() ||
                                item.ObservationType == ObservationType.Process.ToString() ||
                                item.ObservationType == ObservationType.Financial.ToString())
                            {
                                observation.ObservationType = (ObservationType)Enum.Parse(typeof(ObservationType), item.ObservationType);
                            }
                            else
                            {
                                throw new InvalidBulkDataException(StringConstant.ObservationTypeText, item.ObservationType);
                            }

                            observation.IsRepeatObservation = item.IsRepeated == "Yes" ? true : false;
                            observation.RootCause = item.RootCause;
                            observation.Implication = item.Implication;
                            if (item.Disposition == Disposition.Reportable.ToString() || item.Disposition == Disposition.NonReportable.ToString())
                            {
                                observation.Disposition = (Disposition)Enum.Parse(typeof(Disposition), item.Disposition);
                            }
                            else
                            {
                                throw new InvalidBulkDataException(StringConstant.DispositionText, item.Disposition);
                            }
                            if (item.Status == ObservationStatus.Open.ToString() ||
                                item.Status == ObservationStatus.Closed.ToString() ||
                                item.Status == ObservationStatus.Pending.ToString())
                            {
                                observation.ObservationStatus = (ObservationStatus)Enum.Parse(typeof(ObservationStatus), item.Status);
                            }
                            else
                            {
                                throw new InvalidBulkDataException(StringConstant.StatusText, item.Status);
                            }
                            observation.Recommendation = item.Recommendation;
                            observation.ManagementResponse = item.ManagementResponse;
                            observation.Conclusion = item.Conclusion;
                            observation.TargetDate = DateTime.UtcNow;
                            observation.IsDraft = true;
                            observation.EntityId = bulkUpload.EntityId;
                            observation.ObservationListStatus = ObservationStatus.Pending;
                            observation.CreatedDateTime = DateTime.UtcNow;
                            observation.CreatedBy = currentUserId;
                            observations.Add(observation);
                        }
                        await _dataRepository.AddRangeAsync<Observation>(observations);
                        _dataRepository.SaveChanges();
                    }
                }
            }
        }

        #endregion

        #region File upload

        /// <summary>
        /// Add and upload observation files
        /// </summary>
        /// <param name="files">Observation files to be uploaded</param>
        /// <param name="observationId">Observation id to which files are to be connected</param>
        /// <returns>List of added observationDocuments</returns>
        public async Task<List<ObservationDocumentAC>> AddAndUploadObservationFilesAsync(List<IFormFile> files, Guid observationId)
        {
            //validation 
            int fileCount = _dataRepository.Where<ObservationDocument>(a => a.ObservationId == observationId).Count();
            int totalFiles = fileCount + files.Count;

            //validations
            if (totalFiles >= StringConstant.FileLimit)
            {
                throw new InvalidFileCount();
            }

            bool isValidFormate = _globalRepository.CheckFileExtention(files);

            if (isValidFormate)
            {
                throw new InvalidFileFormate();
            }

            bool isFileSizeValid = _globalRepository.CheckFileSize(files);

            if (isFileSizeValid)
            {
                throw new InvalidFileSize();
            }


            List<string> filesUrl = new List<string>();

            //Upload file to azure
            filesUrl = await _fileUtility.UploadFilesAsync(files);

            List<ObservationDocument> addedFileList = new List<ObservationDocument>();
            for (int i = 0; i < filesUrl.Count(); i++)
            {
                ObservationDocument newDocument = new ObservationDocument()
                {
                    Id = new Guid(),
                    DocumentPath = filesUrl[i],
                    ObservationId = observationId,
                    CreatedBy = currentUserId,
                    CreatedDateTime = DateTime.UtcNow
                };
                addedFileList.Add(newDocument);
            }
            await _dataRepository.AddRangeAsync(addedFileList);
            await _dataRepository.SaveChangesAsync();

            List<ObservationDocumentAC> observationDocumentACList = _mapper.Map<List<ObservationDocumentAC>>(addedFileList);
            for (int i = 0; i < observationDocumentACList.Count(); i++)
            {
                observationDocumentACList[i].FileName = observationDocumentACList[i].DocumentPath;
                observationDocumentACList[i].DocumentPath = _azureRepo.DownloadFile(observationDocumentACList[i].DocumentPath);
            }
            return observationDocumentACList.OrderByDescending(x => x.CreatedDateTime).ToList();
        }

        /// <summary>
        /// Method to download observation document
        /// </summary>
        /// <param name="id">Observation document Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadObservationDocumentAsync(Guid id)
        {
            ObservationDocument observationDocument = await _dataRepository.FirstAsync<ObservationDocument>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(observationDocument.DocumentPath);
        }

        /// <summary>
        /// Delete observation document from db and from azure
        /// </summary>
        /// <param name="id">Observation document id</param>
        /// <returns>Void</returns>
        public async Task DeleteObservationDocumentAsync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    ObservationDocument observationDocument = await _dataRepository.FirstAsync<ObservationDocument>(x => x.Id == id && !x.IsDeleted);
                    if (await _fileUtility.DeleteFileAsync(observationDocument.DocumentPath))//Delete file from azure
                    {
                        observationDocument.IsDeleted = true;
                        observationDocument.UpdatedBy = currentUserId;
                        observationDocument.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<ObservationDocument>(observationDocument);
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

        #endregion

        #region Private Methods
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
        private async Task<List<BulkObservationAC>> ValidateObservationData(List<BulkObservationAC> bulkObservations, Guid auditPlanId)
        {
            List<BulkObservationAC> observationList = new List<BulkObservationAC>();
            //get plan process detail
            List<ProcessAC> processList = await _auditPlanRepository.GetPlanWiseAllProcessesByPlanIdAsync(auditPlanId);
            foreach (BulkObservationAC observation in bulkObservations)
            {
                //string processName = observation.Process.Trim();
                ProcessAC processAC = processList.FirstOrDefault(a => a.Name == observation.Process.Trim());
                if (processAC != null)
                {
                    //get sub process list
                    List<ProcessAC> subProcessList = processList.Where(a => a.ParentId == processAC.Id).ToList();
                    ProcessAC subProcessAC = subProcessList.FirstOrDefault(a => a.Name == observation.SubProcess.Trim());
                    if (subProcessAC != null)
                    {
                        observation.Process = processAC.Id.ToString();
                        observation.SubProcess = subProcessAC.Id.ToString();

                        if (string.IsNullOrEmpty(observation.Heading))
                        {
                            throw new RequiredDataException(StringConstant.HeadingText);
                        }

                        if (string.IsNullOrEmpty(observation.Background))
                        {
                            throw new RequiredDataException(StringConstant.BackgroundText);
                        }
                        if (string.IsNullOrEmpty(observation.Observations))
                        {
                            throw new RequiredDataException(StringConstant.ObservationsText);
                        }

                        if (string.IsNullOrEmpty(observation.ObservationType))
                        {
                            throw new RequiredDataException(StringConstant.ObservationTypeText);
                        }

                        if (string.IsNullOrEmpty(observation.RootCause))
                        {
                            throw new RequiredDataException(StringConstant.RootCauseText);
                        }

                        if (string.IsNullOrEmpty(observation.Implication))
                        {
                            throw new RequiredDataException(StringConstant.ImplicationText);
                        }

                        if (string.IsNullOrEmpty(observation.Disposition))
                        {
                            throw new RequiredDataException(StringConstant.DispositionText);
                        }
                        if (string.IsNullOrEmpty(observation.Status))
                        {
                            throw new RequiredDataException(StringConstant.StatusText);
                        }

                        if (string.IsNullOrEmpty(observation.Recommendation))
                        {
                            throw new RequiredDataException(StringConstant.RecommendationText);
                        }

                        observationList.Add(observation);
                    }
                    else
                    {
                        //invalid sub process
                        throw new InvalidBulkDataException(StringConstant.SubProcessText, observation.SubProcess);
                    }
                }
                else
                {
                    //invalid process
                    throw new InvalidBulkDataException(StringConstant.ProcessText, observation.Process);
                }
            }
            return observationList;
        }
        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get observation json document which represents the dynamic table of observation
        /// </summary>
        /// <param name="id">Observation id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        public async Task<string> GetObservationTableAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var observationTable = await _dataRepository.FirstOrDefaultAsync<ObservationTable>(x => x.ObservationId.ToString() == id && !x.IsDeleted);
                    // Create json document table
                    if (observationTable == null)
                    {
                        var observationTableToAdd = new ObservationTable();

                        var parsedDocument = _dynamicTableRepository.AddDefaultJsonDocument();
                        observationTableToAdd.Table = parsedDocument;
                        observationTableToAdd.ObservationId = Guid.Parse(id);
                        observationTableToAdd.IsDeleted = false;
                        observationTableToAdd.CreatedBy = currentUserId;
                        observationTableToAdd.CreatedDateTime = DateTime.UtcNow;
                        await _dataRepository.AddAsync(observationTableToAdd);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        return JsonSerializer.Serialize(observationTableToAdd.Table);
                    }
                    var jsonDocString = JsonSerializer.Serialize(observationTable.Table);
                    return jsonDocString;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// To update json document in observation Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="observationId">Observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        public async Task<string> UpdateJsonDocumentAsync(string jsonDocument, string observationId, string tableId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ObservationTable>(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    observationTable.Table = JsonDocument.Parse(jsonDocument);
                    observationTable.UpdatedBy = currentUserId;
                    observationTable.UpdatedDateTime = DateTime.UtcNow;

                    _dataRepository.Update<ObservationTable>(observationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(observationTable.Table);
                    return jsonObj;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        /// <summary>
        /// Add column in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddColumnAsync(string tableId, string observationId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var observationTables = await _dataRepository.Where<ObservationTable>(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.AddColumn(observationTable.Table.RootElement);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    observationTable.Table = parsedDocument;

                    observationTable.UpdatedBy = currentUserId;
                    observationTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(observationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(parsedDocument);
                    return jsonObj;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Add row in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddRowAsync(string tableId, string observationId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ObservationTable>(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.AddRow(observationTable.Table.RootElement);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    observationTable.Table = parsedDocument;

                    observationTable.UpdatedBy = currentUserId;
                    observationTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(observationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(parsedDocument);
                    return jsonObj;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete row in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteRowAsync(string observationId, string tableId, string rowId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ObservationTable>(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.DeleteRow(observationTable.Table.RootElement, rowId);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    observationTable.Table = parsedDocument;

                    observationTable.UpdatedBy = currentUserId;
                    observationTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(observationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(parsedDocument);
                    return jsonObj;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete column in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteColumnAsync(string observationId, string tableId, int columnPosition)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ObservationTable>(x => x.ObservationId.ToString() == observationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.DeleteColumn(observationTable.Table.RootElement, columnPosition);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    observationTable.Table = parsedDocument;
                    observationTable.UpdatedBy = currentUserId;
                    observationTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(observationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(parsedDocument);
                    return jsonObj;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region Generate Observation PPT
        /// <summary>
        /// Generate Observation PPT
        /// </summary>
        /// <param name="observationId">Id of observation</param>
        /// <param name="entityId">Selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> GenerateObservationPPTAsync(string observationId, string entityId, int timeOffset)
        {
            try
            {
                string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
                Observation observation = _dataRepository.Where<Observation>(a => !a.IsDeleted && a.Id == new Guid(observationId))
                    .Include(x => x.ObservationCategory).Include(x => x.Process).Include(x => x.AuditPlan).ToList().First();

                ObservationAC observationData = await SetObservatonDataAsync(observation, timeOffset);
                //get add table data
                ObservationTable observationTable = await _dataRepository.FirstOrDefaultAsync<ObservationTable>(x => x.ObservationId == new Guid(observationId)
                        && !x.IsDeleted);
                JSONRoot tableData = null;
                if (observationTable != null)
                {
                    tableData = await GetPPTObservationTableAsync(observationTable);
                }

                //get observation document data
                List<ObservationDocumentAC> observationDocumentACList = await GetObservationDocumentAsync(observation.Id.ToString());
                PowerPointTemplate templateData = await CreatePPTTemplateDataAsync(observationData, timeOffset, observationDocumentACList);
                string currentPath = Path.Combine(_environment.ContentRootPath, StringConstant.WWWRootFolder, StringConstant.TemplateFolder);
                string templateFileName = StringConstant.ObservationTemplate + StringConstant.PPTFileExtantion;
                string templateFilePath = Path.Combine(currentPath, templateFileName);

                Tuple<string, MemoryStream> fileData = await _generatePPTRepository.CreatePPTFileAsync(entityName, templateFilePath, templateData, 6, tableData);
                string tempPath = Path.GetTempPath() + StringConstant.ImageFolder;
                await DeleteTemporaryFilesAsync(tempPath);

                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete temporary file
        /// </summary>
        /// <param name="path">path for directory</param>
        /// <returns>Task</returns>
        private async Task DeleteTemporaryFilesAsync(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            // Determine whether the directory exists.
            if (Directory.Exists(path))
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                directory.Delete();
            }
        }

        /// <summary>
        /// Set slected observation data
        /// </summary>
        /// <param name="observationData">Obseravation data</param>
        /// <param name="timeOffset">requested system time offset</param>
        /// <returns>Observation detail</returns>
        private async Task<ObservationAC> SetObservatonDataAsync(Observation observationData, int timeOffset)
        {
            ObservationAC observation = _mapper.Map<ObservationAC>(observationData);
            List<ProcessAC> allProcessACList = await _dataRepository.Where<PlanProcessMapping>(x => x.PlanId == observation.AuditPlanId && !x.IsDeleted)
                                                         .OrderBy(x => x.CreatedDateTime)
                                                         .Select(x => new ProcessAC { Id = x.Process.Id, Name = x.Process.Name, ParentId = x.Process.ParentId }).AsNoTracking().ToListAsync();
            var parentProcessIdList = allProcessACList.Select(x => x.ParentId).ToList();
            var allParentProcessDetails = _dataRepository.Where<Process>(x => !x.IsDeleted && parentProcessIdList.Contains(x.Id))
                                                         .Distinct().OrderBy(x => x.Name)
                                                         .Select(x => new ProcessAC { Id = x.Id, Name = x.Name, ParentId = x.ParentId });
            allProcessACList.AddRange(allParentProcessDetails);

            List<Observation> observationList = await _dataRepository.Where<Observation>(x => !x.IsDeleted && x.EntityId == observationData.EntityId)
                                                                   .AsNoTracking().ToListAsync();

            List<ObservationAC> exportObservationList = _mapper.Map<List<ObservationAC>>(observationList);
            if (exportObservationList.Count == 0)
            {
                ObservationAC observationAC = new ObservationAC();
                exportObservationList.Add(observationAC);
            }

            //Get person resposible
            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted && a.UserId == observation.PersonResponsible)
               .Include(a => a.User).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

            var categoryList = await _dataRepository.Where<ObservationCategory>(x => !x.IsDeleted && x.EntityId == observationData.EntityId).ToListAsync();
            var ObservationCategoryList = _mapper.Map<List<ObservationCategoryAC>>(categoryList);
            List<AuditPlanAC> allPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(observationData.EntityId ?? new Guid());
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);

            observation.ParentProcessId = allProcessACList.FirstOrDefault(x => x.Id == observation.ProcessId)?.ParentId;
            // convert html contant to text
            observation.Heading = _globalRepository.SetSpecialCharecters(observation.Heading);
            observation.Background = _globalRepository.SetSpecialCharecters(observation.Background);
            observation.RootCause = _globalRepository.SetSpecialCharecters(observation.RootCause);
            observation.Implication = _globalRepository.SetSpecialCharecters(observation.Implication);
            observation.Recommendation = _globalRepository.SetSpecialCharecters(observation.Recommendation);
            observation.Observations = _globalRepository.SetSpecialCharecters(observation.Observations);
            observation.ManagementResponse = _globalRepository.SetSpecialCharecters(observation.ManagementResponse);
            observation.Conclusion = _globalRepository.SetSpecialCharecters(observation.Conclusion);
            observation.LinkedObservation = (observation.Id != null && observation.LinkedObservation != "") && observation.LinkedObservation != null ? exportObservationList.FirstOrDefault(x => x.Id == Guid.Parse(observation.LinkedObservation))?.Heading : string.Empty;
            observation.LinkedObservation = _globalRepository.SetSpecialCharecters(observation.LinkedObservation);

            observation.ObservationStatusToString = observation.Id != null ? observation.ObservationStatusToString : string.Empty;
            observation.ObservationTargetDate = observation.Id != null ? observation.TargetDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            observation.DispositionToString = observation.Id != null ? observation.DispositionToString : string.Empty;
            observation.ObservationTypeToString = observation.Id != null ? observation.ObservationTypeToString : string.Empty;
            observation.CreatedDate = (observation.Id != null && observation.CreatedDateTime != null) ? observation.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            observation.UpdatedDate = (observation.Id != null && observation.UpdatedDateTime != null) ? observation.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            observation.PersonResposibleName = (observation.Id != null && observation.PersonResponsible != null) ? entityUserMappingList.Find(x => x.UserId == observation.PersonResponsible)?.User.Name : string.Empty;
            observation.ObservationCategoryName = (observation.Id != null && ObservationCategoryList.Count > 0 && observation.ObservationCategoryId != null) ? ObservationCategoryList.Find(x => x.Id == observation.ObservationCategoryId)?.CategoryName : string.Empty;
            observation.AuditPlanName = (observation.Id != null && observation.AuditPlanId != Guid.Empty) ? allPlanList.Find(x => x.Id == observation.AuditPlanId)?.Title : string.Empty;
            observation.IsRepeated = observation.Id != null ? (observation.IsRepeatObservation ? StringConstant.Yes : StringConstant.No) : string.Empty;
            observation.StatusString = (observation.Id != null && observation.ObservationListStatus.ToString() == StringConstant.ClosedStatusString) ? StringConstant.CompletedStatusString : (observation.ObservationListStatus.ToString() == StringConstant.PendingStatusString) ? StringConstant.PendingStatusString : string.Empty;
            observation.ProcessName = observation.Id != null ? allProcessACList.FirstOrDefault(x => x.Id == observation.ParentProcessId)?.Name : string.Empty;
            observation.SubProcessName = observation.Id != null ? allProcessACList.FirstOrDefault(x => x.Id == observation.ProcessId)?.Name : string.Empty;

            return observation;
        }

        /// <summary>
        /// Get observavation document detail
        /// </summary>
        /// <param name="observationId">Selected observation id</param>
        /// <returns>List of observation document</returns>
        private async Task<List<ObservationDocumentAC>> GetObservationDocumentAsync(string observationId)
        {
            List<ObservationDocument> observationDocuments = _dataRepository.Where<ObservationDocument>(a => !a.IsDeleted && a.ObservationId == Guid.Parse(observationId)).AsNoTracking().ToList();
            List<ObservationDocumentAC> observationDocumentList = _mapper.Map<List<ObservationDocumentAC>>(observationDocuments);
            List<ObservationDocumentAC> imageDocument = new List<ObservationDocumentAC>();

            for (int i = 0; i < observationDocumentList.Count; i++)
            {
                observationDocumentList[i].FileName = observationDocumentList[i].DocumentPath;
                observationDocumentList[i].DocumentPath = _azureRepo.DownloadFile(observationDocumentList[i].DocumentPath);

                string fileExtention = observationDocumentList[i].FileName.Split(".").Last();
                if (fileExtention == StringConstant.JPEGFileExtantion
                    || fileExtention == StringConstant.JPGFileExtantion
                    || fileExtention == StringConstant.PNGFileExtantion
                    || fileExtention == StringConstant.GIFFileExtantion)
                {

                    imageDocument.Add(observationDocumentList[i]);
                }
            }
            await DownloadImagesAsync(imageDocument);
            return imageDocument;
        }

        /// <summary>
        /// Download observation image files
        /// </summary>
        /// <param name="observationDocuments">Observation document list</param>
        /// <returns>Task</returns>
        private async Task DownloadImagesAsync(List<ObservationDocumentAC> observationDocuments)
        {
            string currentPath = Path.GetTempPath() + StringConstant.ImageFolder;
            // Determine whether the directory exists.
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
            for (int i = 0; i < observationDocuments.Count; i++)
            {
                string saveLocation = Path.Combine(currentPath, observationDocuments[i].FileName);

                byte[] imageBytes;
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(observationDocuments[i].DocumentPath);
                WebResponse imageResponse = imageRequest.GetResponse();

                Stream responseStream = imageResponse.GetResponseStream();

                using (BinaryReader br = new BinaryReader(responseStream))
                {
                    imageBytes = br.ReadBytes(500000);
                    br.Close();
                }
                responseStream.Close();
                imageResponse.Close();

                FileStream fs = new FileStream(saveLocation, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(imageBytes);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
            }
        }

        /// <summary>
        /// Create PPT template data
        /// </summary>
        /// <param name="observation">Observation detail</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <param name="documentList">observation document list</param>
        /// <returns>Power point template</returns>
        private async Task<PowerPointTemplate> CreatePPTTemplateDataAsync(ObservationAC observation, int timeOffset, List<ObservationDocumentAC> documentList)
        {
            string observationGeneratedDate = observation.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);

            var pptTemplate = new PowerPointTemplate();
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph1#]", Text = observation.Heading });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph2#]", Text = observationGeneratedDate });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph3#]", Text = observation.Background });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph4#]", Text = observation.Observations });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph5#]", Text = observation.AuditPlanName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph6#]", Text = observation.ProcessName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph7#]", Text = observation.SubProcessName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph8#]", Text = observation.ObservationTypeToString });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph9#]", Text = observation.ObservationCategoryName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph10#]", Text = observation.IsRepeated });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph11#]", Text = observation.RootCause });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph12#]", Text = observation.Implication });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph13#]", Text = observation.Recommendation });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph14#]", Text = observation.DispositionToString });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph15#]", Text = observation.StatusString });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph16#]", Text = observation.LinkedObservation });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph17#]", Text = observation.ObservationTargetDate });
            string personeResponsible = observation.PersonResposibleName != null ? observation.PersonResposibleName : string.Empty;
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph18#]", Text = personeResponsible });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph19#]", Text = observation.ManagementResponse });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph20#]", Text = observation.Conclusion });

            string currentPath = Path.GetTempPath() + StringConstant.ImageFolder;
            //bind image
            for (int i = 0; i < documentList.Count; i++)
            {
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter()
                {
                    Name = "Img" + (i + 1).ToString(),
                    Image = new FileInfo(currentPath + @"\" + documentList[i].FileName)
                });
            }

            return pptTemplate;
        }

        /// <summary>
        /// Get observation table detail
        /// </summary>
        /// <param name="observationTable"> observation table</param>
        /// <returns>Json data of observation</returns>
        private async Task<JSONRoot> GetPPTObservationTableAsync(ObservationTable observationTable)
        {

            var jsonTable = JsonDocument.Parse(JsonSerializer.Serialize(observationTable.Table.RootElement));
            var jsonData = jsonTable.RootElement.ToString();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(jsonData);
            return result;
        }
        #endregion

        /// <summary>
        /// Get observation table detail
        /// </summary>
        /// <param name="observationTable">observation table</param>
        /// <returns>Json data of observation</returns>
        private async Task<JSONRoot> GetJsonObservationTableAsync(ObservationTable observationTable)
        {
            var jsonTable = JsonDocument.Parse(JsonSerializer.Serialize(observationTable.Table.RootElement));
            var jsonData = jsonTable.RootElement.ToString();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(jsonData);
            return result;
        }


    }
}
