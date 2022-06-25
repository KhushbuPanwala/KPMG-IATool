using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Migrations;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using InternalAuditSystem.DomainModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using Microsoft.AspNetCore.Hosting;

namespace InternalAuditSystem.Repository.Repository.ACMRepresentationRepository
{
    public class ACMRepository : IACMRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IRatingRepository _ratingRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public IDynamicTableRepository _dynamicTableRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepo;
        public Guid currentUserId = Guid.Empty;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IAzureRepository _azureRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        public IGlobalRepository _globalRepository;
        private readonly IGeneratePPTRepository _generatePPTRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IReportRepository _reportRepository;
        #endregion

        #region Public method(s)
        public ACMRepository(IDataRepository dataRepository, IMapper mapper, IDynamicTableRepository dynamicTableRepository, IFileUtility fileUtility,
            IAzureRepository azureRepo, IRatingRepository ratingRepository, IHttpContextAccessor httpContextAccessor, IExportToExcelRepository exportToExcelRepository,
            IAzureRepository azureRepository, IGlobalRepository globalRepository, IAuditableEntityRepository auditableEntityRepository,
            IGeneratePPTRepository generatePPTRepository, IWebHostEnvironment environment, IReportRepository reportRepository)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _ratingRepository = ratingRepository;
            _httpContextAccessor = httpContextAccessor;
            _dynamicTableRepository = dynamicTableRepository;
            _fileUtility = fileUtility;
            _azureRepo = azureRepo;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _exportToExcelRepository = exportToExcelRepository;
            _azureRepository = azureRepository;
            _globalRepository = globalRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _generatePPTRepository = generatePPTRepository;
            _environment = environment;
            _reportRepository = reportRepository;
        }

        #region General method(s) 
        /// <summary>
        /// Get ACM data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="fromYear">selected from year</param>   
        /// <param name="toYear">selected toyear</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of ACM Presentation</returns>
        public async Task<List<ACMPresentationAC>> GetACMDataAsync(int? page, int pageSize, string searchString, string selectedEntityId, int? fromYear, int? toYear)

        {
            List<ACMPresentation> acmPresentationList;
            if (!string.IsNullOrEmpty(searchString))
            {
                if ((fromYear != 0 && toYear != 0) && (fromYear != null && toYear != null))
                {
                    acmPresentationList = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.Heading.ToLower().Contains(searchString.ToLower())
                    && a.EntityId.ToString() == selectedEntityId
                    && (a.CreatedDateTime.Year <= toYear && a.CreatedDateTime.Year >= fromYear)).Include(a => a.Rating)
                    .Skip((page - 1 ?? 0) * pageSize)
                    .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
                else
                {
                    acmPresentationList = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId && a.Heading.ToLower().Contains(searchString.ToLower()))
                        .Include(a => a.Rating).Skip((page - 1 ?? 0) * pageSize)
                        .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if ((fromYear != 0 && toYear != 0) && (fromYear != null && toYear != null))
                {
                    acmPresentationList = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId && (a.CreatedDateTime.Year <= toYear && a.CreatedDateTime.Year >= fromYear)).Include(a => a.Rating).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
                else
                {
                    acmPresentationList = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId).Include(a => a.Rating).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().OrderByDescending(a => a.CreatedDateTime).ToListAsync();
                }

            }
            return _mapper.Map<List<ACMPresentationAC>>(acmPresentationList);
        }

        /// <summary>
        /// Get count of ACM
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>count of ACM </returns>
        public async Task<int> GetACMCountAsync(string searchString, string selectedEntityId)
        {
            int totalACMRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalACMRecords = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId && a.Heading.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalACMRecords = await _dataRepository.Where<ACMPresentation>(a => !a.IsDeleted && a.EntityId.ToString() == selectedEntityId).AsNoTracking().CountAsync();
            }
            return totalACMRecords;
        }

        /// <summary>
        /// Get Details of ACM by Id
        /// </summary>
        /// <param name="acmId">ACM Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>ACMPresentationAC</returns>
        public async Task<ACMPresentationAC> GetACMDetailsByIdAsync(Guid? acmId, string selectedEntityId)
        {
            ACMPresentationAC acmPresentationAC = new ACMPresentationAC();
            ACMPresentation acmPresentation = new ACMPresentation();

            if (acmId != null)
            {
                acmPresentation = await _dataRepository.FirstOrDefaultAsync<ACMPresentation>(x => x.Id == acmId && !x.IsDeleted && x.EntityId.ToString() == selectedEntityId);

                List<RatingAC> ratingList = await _ratingRepository.GetRatingsByEntityIdAsync(selectedEntityId);
                if (acmPresentation == null)
                {
                    throw new NoRecordException(StringConstant.ACMPresentationString);
                }
                acmPresentationAC = _mapper.Map<ACMPresentationAC>(acmPresentation);

                List<ACMDocument> aCMDocumentACList = await _dataRepository.Where<ACMDocument>(x => x.ACMPresentationId == acmId && !x.IsDeleted).ToListAsync();

                if (aCMDocumentACList != null && aCMDocumentACList.Count > 0)
                {
                    acmPresentationAC.ACMDocuments = _mapper.Map<List<ACMDocumentAC>>(aCMDocumentACList);
                    for (var i = 0; i < acmPresentationAC.ACMDocuments.Count(); i++)
                    {
                        acmPresentationAC.ACMDocuments[i].FileName = acmPresentationAC.ACMDocuments[i].DocumentPath;
                        acmPresentationAC.ACMDocuments[i].DocumentPath = _azureRepository.DownloadFile(acmPresentationAC.ACMDocuments[i].DocumentPath);
                    }
                }
            }
            acmPresentationAC.RatingsList = await _ratingRepository.GetRatingsByEntityIdAsync(selectedEntityId);

            return acmPresentationAC;
        }

        /// <summary>
        /// Add ACM
        /// </summary>
        /// <param name="acmPresentationAC">Application class of ACM Presentation/param>
        /// <returns>ACMPresentationAC</returns>
        public async Task<ACMPresentationAC> AddACMAsync(ACMPresentationAC acmPresentationAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    ACMPresentation acmPresentation = _mapper.Map<ACMPresentation>(acmPresentationAC);
                    acmPresentation.CreatedDateTime = DateTime.UtcNow;
                    acmPresentation.EntityId = acmPresentationAC.EntityId;
                    acmPresentation.CreatedBy = currentUserId;
                    await _dataRepository.AddAsync(acmPresentation);

                    // add report reviewer 
                    List<ACMReportMapping> reportUserMappingList = _mapper.Map<List<ACMReportMapping>>(acmPresentationAC.ReviewerList);
                    for (int i = 0; i < reportUserMappingList.Count; i++)
                    {
                        reportUserMappingList[i].ACMId = acmPresentation.Id;
                        reportUserMappingList[i].ReportUserType = ReportUserType.Reviewer;
                        reportUserMappingList[i].CreatedBy = currentUserId;
                        reportUserMappingList[i].CreatedDateTime = DateTime.UtcNow;
                    }
                    await _dataRepository.AddRangeAsync<ACMReportMapping>(reportUserMappingList);
                    await _dataRepository.SaveChangesAsync();

                    acmPresentationAC.ReviewerList = _mapper.Map<List<ACMReportMappingAC>>(reportUserMappingList);
                    await _dataRepository.SaveChangesAsync();
                    #region ACMPresentationDocument add 
                    if (acmPresentationAC.ACMFiles != null && acmPresentationAC.ACMFiles.Count() > 0)
                    {
                        acmPresentationAC.ACMDocuments = await AddAndUploadACMFilesAsync(acmPresentationAC.ACMFiles, acmPresentation.Id);
                    }
                    #endregion
                    transaction.Commit();
                    return _mapper.Map<ACMPresentationAC>(acmPresentation);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update ACM
        /// </summary>
        /// <param name="acmPresentationAC">Application class of ACM Presentation List</param>
        /// <returns>ACMPresentationAC List</returns>
        public async Task<ACMPresentationAC> UpdateACMAsync(ACMPresentationAC acmPresentationAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    ACMPresentation acmPresentation = _mapper.Map<ACMPresentation>(acmPresentationAC);
                    acmPresentation.UpdatedDateTime = DateTime.UtcNow;
                    acmPresentation.UpdatedBy = currentUserId;

                    _dataRepository.Update(acmPresentation);
                    await _dataRepository.SaveChangesAsync();

                    #region ACMPresentationDocument add 
                    if (acmPresentationAC.ACMFiles != null && acmPresentationAC.ACMFiles.Count() > 0)
                    {
                        acmPresentationAC.ACMDocuments = await AddAndUploadACMFilesAsync(acmPresentationAC.ACMFiles, acmPresentation.Id);
                    }
                    #endregion

                    transaction.Commit();
                    return acmPresentationAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete ACM detail
        /// <param name="acmId">id of deleted ACM</param>
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeleteAcmAsync(Guid acmId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    ACMPresentation acmDelete = await _dataRepository.FirstAsync<ACMPresentation>(x => x.Id == acmId && !x.IsDeleted);
                    acmDelete.IsDeleted = true;
                    acmDelete.UpdatedDateTime = DateTime.UtcNow;
                    acmDelete.UpdatedBy = currentUserId;
                    _dataRepository.Update<ACMPresentation>(acmDelete);
                    await _dataRepository.SaveChangesAsync();

                    //soft delete reviewer and distributors 
                    List<ACMReportMapping> acmUserDetailList = _dataRepository.Where<ACMReportMapping>(x => !x.IsDeleted && x.Id.ToString().Equals(acmId)).AsNoTracking().ToList();
                    for (int i = 0; i < acmUserDetailList.Count; i++)
                    {
                        acmUserDetailList[i].IsDeleted = true;
                        acmUserDetailList[i].UpdatedBy = currentUserId;
                        acmUserDetailList[i].UpdatedDateTime = DateTime.UtcNow;
                    }

                    _dataRepository.UpdateRange<ACMReportMapping>(acmUserDetailList);
                    await _dataRepository.SaveChangesAsync();


                    //soft delete reviewer document
                    HashSet<Guid> reviewerIds = new HashSet<Guid>(acmUserDetailList.Select(s => s.Id));
                    List<ACMDocument> reviewerDocumentList = _dataRepository.Where<ACMDocument>(x => !x.IsDeleted &&
                    reviewerIds.Contains((Guid)x.ACMReportMappingId)).AsNoTracking().ToList();
                    for (int i = 0; i < reviewerDocumentList.Count; i++)
                    {

                        //Delete file from azure
                        if (await _fileUtility.DeleteFileAsync(reviewerDocumentList[i].DocumentPath))
                        {
                            reviewerDocumentList[i].IsDeleted = true;
                            reviewerDocumentList[i].UpdatedBy = currentUserId;
                            reviewerDocumentList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<ACMDocument>(reviewerDocumentList);
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


        #endregion

        #region Export acm
        /// <summary>
        /// Method for exporting acm
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        public async Task<Tuple<string, MemoryStream>> ExportAcmAsync(string entityId, int timeOffset)
        {
            var acmList = await _dataRepository.Where<ACMPresentation>(x => !x.IsDeleted && x.EntityId == Guid.Parse(entityId))
                                                       .Include(x => x.Rating).OrderByDescending(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            List<ACMPresentationAC> acmACList = _mapper.Map<List<ACMPresentationAC>>(acmList);
            if (acmACList.Count == 0)
            {
                ACMPresentationAC acmPresentationAC = new ACMPresentationAC();
                acmACList.Add(acmPresentationAC);
            }

            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            acmACList.ForEach(x =>
            {
                x.Heading = x.Id != null ? x.Heading : string.Empty;
                x.Recommendation = string.IsNullOrEmpty(x.Recommendation) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.Recommendation, string.Empty));
                x.Observation = string.IsNullOrEmpty(x.Observation) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.Observation, string.Empty));
                x.ManagementResponse = string.IsNullOrEmpty(x.ManagementResponse) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(x.ManagementResponse, string.Empty));
                x.Ratings = x.Id != null ? x.Ratings : string.Empty;
                x.Implication = x.Id != null ? x.Implication : string.Empty;
                x.StatusString = x.Id != null ? x.StatusString : string.Empty;
                x.IsDraftToString = (x.Id != null && x.IsDraft == true) ? StringConstant.Yes : x.IsDraft == false ? StringConstant.No : string.Empty;
                x.CreatedDate = (x.Id != null && x.CreatedDateTime != null) ? x.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                x.UpdatedDate = (x.Id != null && x.UpdatedDateTime != null) ? x.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });


            //acm all add acm ids
            HashSet<string> acmIds = new HashSet<string>(acmACList.Select(s => s.Id.ToString()));

            List<ACMTable> acmTables = _dataRepository.Where<ACMTable>(a => !a.IsDeleted && acmIds.Contains(a.ACMId.ToString())).AsNoTracking().ToList();

            List<JSONTable> jsonACMTables = new List<JSONTable>();
            for (int i = 0; i < acmTables.Count; i++)
            {
                JSONTable jsonTable = new JSONTable();
                string acmName = string.Empty;
                ACMPresentationAC aCMPresentationAC = acmACList.FirstOrDefault(a => a.Id == acmTables[i].ACMId);
                if (aCMPresentationAC != null)
                {

                    acmName = aCMPresentationAC.Heading;

                }
                jsonTable.Name = string.Empty + "$$" + acmName;
                jsonTable.JsonData = await GetJsonACMTableAsync(acmTables[i]);
                jsonACMTables.Add(jsonTable);
            }
            //crete dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.ACMPresentationString, acmACList);
            //for json data
            dynamicDictionary.Add(StringConstant.ACMTableModuleName, jsonACMTables);

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(dynamicDictionary, jsonACMTables, StringConstant.ACMPresentationString + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get acm json document which represents the dynamic table of ACM
        /// </summary>
        /// <param name="id">acm id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        public async Task<string> GetACMTableAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var acmTable = await _dataRepository.FirstOrDefaultAsync<ACMTable>(x => x.ACMId.ToString() == id && !x.IsDeleted);
                    // Create json document table
                    if (acmTable == null)
                    {
                        var acmTableToAdd = new ACMTable();

                        var parsedDocument = _dynamicTableRepository.AddDefaultJsonDocument();
                        acmTableToAdd.Table = parsedDocument;
                        acmTableToAdd.ACMId = Guid.Parse(id);
                        acmTableToAdd.IsDeleted = false;
                        acmTableToAdd.CreatedBy = currentUserId;
                        acmTableToAdd.CreatedDateTime = DateTime.UtcNow;
                        await _dataRepository.AddAsync(acmTableToAdd);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        return JsonSerializer.Serialize(acmTableToAdd.Table);
                    }
                    var jsonDocString = JsonSerializer.Serialize(acmTable.Table);
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
        /// To update json document in acm Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="acmId">ACM id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        public async Task<string> UpdateJsonDocumentAsync(string jsonDocument, string acmId, string tableId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var acmTables = await _dataRepository.Where<ACMTable>(x => x.ACMId.ToString() == acmId
                    && !x.IsDeleted).ToListAsync();
                    var acmTable = acmTables.FirstOrDefault<ACMTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    acmTable.Table = JsonDocument.Parse(jsonDocument);
                    acmTable.UpdatedBy = currentUserId;
                    acmTable.UpdatedDateTime = DateTime.UtcNow;

                    _dataRepository.Update<ACMTable>(acmTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(acmTable.Table);
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
        /// Add column in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">ACM id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddColumnAsync(string tableId, string acmId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var acmTables = await _dataRepository.Where<ACMTable>(x => x.ACMId.ToString() == acmId
                    && !x.IsDeleted).ToListAsync();
                    var acmTable = acmTables.FirstOrDefault<ACMTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.AddColumn(acmTable.Table.RootElement);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    acmTable.Table = parsedDocument;
                    acmTable.UpdatedBy = currentUserId;
                    acmTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(acmTable);
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
        /// Add row in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">ACM id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddRowAsync(string tableId, string acmId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var acmTables = await _dataRepository.Where<ACMTable>(x => x.ACMId.ToString() == acmId
                    && !x.IsDeleted).ToListAsync();
                    var acmTable = acmTables.FirstOrDefault<ACMTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.AddRow(acmTable.Table.RootElement);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    acmTable.Table = parsedDocument;
                    acmTable.UpdatedBy = currentUserId;
                    acmTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(acmTable);
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
        /// Delete row in acmTable's table
        /// </summary>
        /// <param name="acmId">ACM id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteRowAsync(string acmId, string tableId, string rowId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var acmTables = await _dataRepository.Where<ACMTable>(x => x.ACMId.ToString() == acmId
                    && !x.IsDeleted).ToListAsync();
                    var acmTable = acmTables.FirstOrDefault<ACMTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.DeleteRow(acmTable.Table.RootElement, rowId);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    acmTable.Table = parsedDocument;
                    acmTable.UpdatedBy = currentUserId;
                    acmTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(acmTable);
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
        /// Delete column in acmTable's table
        /// </summary>
        /// <param name="acmId">ACM id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteColumnAsync(string acmId, string tableId, int columnPosition)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var acmTables = await _dataRepository.Where<ACMTable>(x => x.ACMId.ToString() == acmId
                    && !x.IsDeleted).ToListAsync();
                    var acmTable = acmTables.FirstOrDefault<ACMTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    var resultJson = _dynamicTableRepository.DeleteColumn(acmTable.Table.RootElement, columnPosition);

                    var parsedDocument = JsonDocument.Parse(resultJson);
                    acmTable.Table = parsedDocument;
                    acmTable.UpdatedBy = currentUserId;
                    acmTable.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(acmTable);
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

        #region private methods

        /// <summary>
        /// Check report already exist or not
        /// </summary>
        /// <param name="name">report name</param>
        /// <param name="entityId">entity id</param>
        /// <returns>Return boolean value for report exist or not </returns>
        private bool CheckReportExist(string name, string entityId, string reportId)
        {
            bool exist;
            if (reportId != null)
            {
                exist = _dataRepository.Any<Report>(a => a.ReportTitle.ToLower().Equals(name.ToLower())
                  && a.EntityId.ToString() == entityId && !a.IsDeleted
                && a.Id.ToString() != reportId);

            }
            else
            {
                exist = _dataRepository.Any<Report>(a => a.ReportTitle.ToLower().Equals(name.ToLower())
                && a.EntityId.ToString() == entityId && !a.IsDeleted);
            }
            return exist;
        }

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

        #endregion

        #region File upload

        /// <summary>
        /// Add and upload acm files
        /// </summary>
        /// <param name="files">ACM files to be uploaded</param>
        /// <param name="acmId">ACM id to which files are to be connected</param>
        /// <returns>List of added acmDocuments</returns>
        public async Task<List<ACMDocumentAC>> AddAndUploadACMFilesAsync(List<IFormFile> files, Guid acmId)
        {

            List<string> filesUrl = new List<string>();

            //validation 
            int fileCount = _dataRepository.Where<ACMDocument>(a => a.ACMPresentationId == acmId).Count();
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

            //Upload file to azure
            filesUrl = await _fileUtility.UploadFilesAsync(files);

            List<ACMDocument> addedFileList = new List<ACMDocument>();
            for (int i = 0; i < filesUrl.Count(); i++)
            {
                ACMDocument newDocument = new ACMDocument()
                {
                    Id = new Guid(),
                    DocumentPath = filesUrl[i],
                    ACMPresentationId = acmId,
                    CreatedBy = currentUserId,
                    CreatedDateTime = DateTime.UtcNow
                };
                addedFileList.Add(newDocument);
            }
            await _dataRepository.AddRangeAsync(addedFileList);
            await _dataRepository.SaveChangesAsync();

            List<ACMDocumentAC> acmDocumentACList = _mapper.Map<List<ACMDocumentAC>>(addedFileList);
            for (int i = 0; i < acmDocumentACList.Count(); i++)
            {
                acmDocumentACList[i].FileName = acmDocumentACList[i].DocumentPath;
                acmDocumentACList[i].DocumentPath = _azureRepo.DownloadFile(acmDocumentACList[i].DocumentPath);
            }
            return acmDocumentACList.OrderByDescending(x => x.CreatedDateTime).ToList();
        }

        /// <summary>
        /// Method to download acm document
        /// </summary>
        /// <param name="id">ACM document Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadACMDocumentAsync(Guid id)
        {
            ACMDocument acmDocument = await _dataRepository.FirstAsync<ACMDocument>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(acmDocument.DocumentPath);
        }

        /// <summary>
        /// Delete acm document from db and from azure
        /// </summary>
        /// <param name="id">ACM document id</param>
        /// <returns>Void</returns>
        public async Task DeleteACMDocumentAsync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    ACMDocument acmDocument = await _dataRepository.FirstAsync<ACMDocument>(x => x.Id == id && !x.IsDeleted);
                    if (await _fileUtility.DeleteFileAsync(acmDocument.DocumentPath))//Delete file from azure
                    {
                        acmDocument.IsDeleted = true;
                        acmDocument.UpdatedBy = currentUserId;
                        acmDocument.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<ACMDocument>(acmDocument);
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

        #endregion

        #region Generate ACM PPT
        /// <summary>
        /// Generate ACM PPT
        /// </summary>
        /// <param name="acmId">Id of ACM</param>
        /// <param name="entityId">Selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> GenerateACMPPTAsync(string acmId, string entityId, int timeOffset)
        {
            try
            {
                string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);

                ACMPresentation acmPresentation = await _dataRepository.FirstOrDefaultAsync<ACMPresentation>(x => x.Id == Guid.Parse(acmId) && !x.IsDeleted
                && x.EntityId.ToString() == entityId);

                //get observation document data
                List<ACMDocumentAC> acmDocuments = await GetACMDocumentAsync(acmPresentation.Id.ToString());

                PowerPointTemplate templateData = await CreatePPTTemplateData(entityId, acmPresentation, timeOffset, acmDocuments);

                List<ACMReportPPT> aCMReportData = await GetACMReportsAsync(acmPresentation.Id.ToString(), timeOffset);

                List<MemberTablePPT> reviewerTable = await GetACMReviewers(acmPresentation.Id.ToString());

                dynamic dynamicDictionary = new DynamicDictionary<int, dynamic>();
                dynamicDictionary.Add(6, aCMReportData);
                dynamicDictionary.Add(8, reviewerTable);

                string currentPath = Path.Combine(_environment.ContentRootPath, StringConstant.WWWRootFolder, StringConstant.TemplateFolder);
                string templateFileName = StringConstant.ACMTemplate + StringConstant.PPTFileExtantion;
                string templateFilePath = Path.Combine(currentPath, templateFileName);

                Tuple<string, MemoryStream> fileData = await _generatePPTRepository.CreatePPTFileAsync(entityName, templateFilePath, templateData, dynamicDictionary);
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create PPT template data
        /// </summary>
        /// <param name="entityId">Selected entity id</param>
        /// <param name="acmPresentation">ACM presentation detail</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Return Power point template</returns>
        private async Task<PowerPointTemplate> CreatePPTTemplateData(string entityId, ACMPresentation acmPresentation, int timeOffset, List<ACMDocumentAC> documentList)
        {
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            ACMPresentationAC acmPresentationAC = _mapper.Map<ACMPresentationAC>(acmPresentation);
            List<RatingAC> ratingList = await _ratingRepository.GetRatingsByEntityIdAsync(entityId);

            string acmGeneratedDate = acmPresentation.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);
            acmPresentationAC.Heading = _globalRepository.SetSpecialCharecters(acmPresentationAC.Heading);
            acmPresentationAC.Observation = _globalRepository.SetSpecialCharecters(acmPresentationAC.Observation);
            acmPresentationAC.Recommendation = _globalRepository.SetSpecialCharecters(acmPresentationAC.Recommendation);
            acmPresentationAC.ManagementResponse = _globalRepository.SetSpecialCharecters(acmPresentationAC.ManagementResponse);
            acmPresentationAC.Implication = _globalRepository.SetSpecialCharecters(acmPresentationAC.Implication);
            acmPresentationAC.Ratings = ratingList.FirstOrDefault(a => a.Id == acmPresentationAC.RatingId).Ratings;
            acmPresentationAC.StatusString = acmPresentationAC.Status.ToString();

            var pptTemplate = new PowerPointTemplate();
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph1#]", Text = acmPresentationAC.Heading });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph2#]", Text = acmGeneratedDate });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph3#]", Text = acmPresentationAC.Heading });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph4#]", Text = acmPresentationAC.Observation });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph5#]", Text = acmPresentationAC.Recommendation });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph6#]", Text = acmPresentationAC.ManagementResponse });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph7#]", Text = acmPresentationAC.Ratings });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph8#]", Text = acmPresentationAC.StatusString });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph9#]", Text = acmPresentationAC.Implication });

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
        /// Get observavation document detail
        /// </summary>
        /// <param name="acmId">Selected ACM id</param>
        /// <returns>List of ACM document</returns>
        private async Task<List<ACMDocumentAC>> GetACMDocumentAsync(string acmId)
        {
            List<ACMDocument> acmocuments = _dataRepository.Where<ACMDocument>(a => !a.IsDeleted && a.ACMPresentationId == Guid.Parse(acmId)).AsNoTracking().ToList();
            List<ACMDocumentAC> acmDocumentList = _mapper.Map<List<ACMDocumentAC>>(acmocuments);
            List<ACMDocumentAC> imageDocument = new List<ACMDocumentAC>();

            for (int i = 0; i < acmDocumentList.Count; i++)
            {
                acmDocumentList[i].FileName = acmDocumentList[i].DocumentPath;
                acmDocumentList[i].DocumentPath = _azureRepo.DownloadFile(acmDocumentList[i].DocumentPath);

                string fileExtention = acmDocumentList[i].FileName.Split(".").Last();
                if (fileExtention == StringConstant.JPEGFileExtantion
                    || fileExtention == StringConstant.JPGFileExtantion
                    || fileExtention == StringConstant.PNGFileExtantion
                    || fileExtention == StringConstant.GIFFileExtantion)
                {

                    imageDocument.Add(acmDocumentList[i]);
                }
            }
            await DownloadImagesAsync(imageDocument);
            return imageDocument;
        }

        /// <summary>
        /// Download ACM image files
        /// </summary>
        /// <param name="acmDocuments">ACM document list</param>
        /// <returns>Task</returns>
        private async Task DownloadImagesAsync(List<ACMDocumentAC> acmDocuments)
        {
            string currentPath = Path.GetTempPath() + StringConstant.ImageFolder;
            // Determine whether the directory exists.
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
            for (int i = 0; i < acmDocuments.Count; i++)
            {
                string saveLocation = Path.Combine(currentPath, acmDocuments[i].FileName);

                byte[] imageBytes;
                HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(acmDocuments[i].DocumentPath);
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
        /// Get ACM Report data 
        /// </summary>
        /// <param name="acmId">Selected observation id</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>List of acm report data</returns>
        private async Task<List<ACMReportPPT>> GetACMReportsAsync(string acmId, int timeOffset)
        {
            List<ACMReportMapping> acmReportMappings = _dataRepository.Where<ACMReportMapping>(a => a.ACMId == Guid.Parse(acmId))
                .Include(x => x.Report).AsNoTracking().ToList();
            List<ACMReportMappingAC> acmReportMappingAC = _mapper.Map<List<ACMReportMappingAC>>(acmReportMappings);

            List<Report> reports = _dataRepository.Where<Report>(a => !a.IsDeleted).Include(x => x.Rating).AsNoTracking().ToList();

            List<ReportAC> reportList = _mapper.Map<List<ReportAC>>(reports);

            List<ACMReportPPT> acmReports = new List<ACMReportPPT>();


            //Get count of no. of observation for report list
            List<ReportObservation> reportObservations = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted).AsNoTracking().ToListAsync();

            var reportAC = reportObservations.GroupBy(x => x.ReportId)
                      .Select(x => new ReportAC { Id = x.Key, noOfObservation = x.Count() }).ToList();

            for (int i = 0; i < reportList.Count; i++)
            {
                ReportAC totalObservation = reportAC.Where(a => a.Id == reportList[i].Id).FirstOrDefault();
                reportList[i].noOfObservation = totalObservation != null ? totalObservation.noOfObservation : 0;
            }

            for (int i = 0; i < acmReportMappingAC.Count; i++)
            {
                ReportAC report = reportList.FirstOrDefault(a => a.Id == acmReportMappingAC[i].ReportId);
                ACMReportPPT acmReportPPTData = new ACMReportPPT();
                acmReportPPTData.SrNo = (i + 1).ToString();
                acmReportPPTData.ReportTitle = report.ReportTitle;
                acmReportPPTData.NoOfObservation = report.noOfObservation.ToString();
                acmReportPPTData.Rating = report.Ratings;
                acmReportPPTData.Status = report.AuditStatus.ToString();
                acmReportPPTData.Stage = report.Stage.ToString();
                //set audit period
                string auditPeriod = report.AuditPeriodStartDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) + StringConstant.toText + report.AuditPeriodEndDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);
                acmReportPPTData.Period = auditPeriod;
                acmReports.Add(acmReportPPTData);
            }
            return acmReports;
        }

        /// <summary>
        /// Get acm reviewer data
        /// </summary>
        /// <param name="acmId">ACM id</param>
        /// <returns>List of acm reviewer</returns>
        private async Task<List<MemberTablePPT>> GetACMReviewers(string acmId)
        {
            List<ACMReviewer> acmReviewers = _dataRepository.Where<ACMReviewer>(a => !a.IsDeleted
             && a.ACMPresentationId == new Guid(acmId))
                .Include(x => x.User).AsNoTracking().ToList();
            List<ACMReviewerAC> reviewers = _mapper.Map<List<ACMReviewerAC>>(acmReviewers);

            //stack holders
            List<MemberTablePPT> acmReviewerTable = new List<MemberTablePPT>();
            for (int i = 0; i < reviewers.Count; i++)
            {
                MemberTablePPT reviewer = new MemberTablePPT();
                reviewer.SrNo = (i + 1).ToString();
                reviewer.Name = reviewers[i].Name;
                reviewer.Designation = reviewers[i].Designation;
                reviewer.Status = reviewers[i].Status.ToString();
                acmReviewerTable.Add(reviewer);
            }

            return acmReviewerTable;
        }


        #endregion

        /// <summary>
        /// Get acm table detail
        /// </summary>
        /// <param name="acmTable">ACM table</param>
        /// <returns>Json data of acm</returns>
        private async Task<JSONRoot> GetJsonACMTableAsync(ACMTable acmTable)
        {
            var jsonTable = JsonDocument.Parse(JsonSerializer.Serialize(acmTable.Table.RootElement));
            var jsonData = jsonTable.RootElement.ToString();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(jsonData);
            return result;
        }

        #region Acm Report
        /// <summary>
        /// Method for getting report details by status and stage
        /// </summary>
        /// <param name="pagination">Pagination of report AC</param>
        /// <returns>List of report</returns>
        public async Task<Pagination<ReportAC>> GetACMAllReportsByStatusAndStagAsync(Pagination<ReportAC> pagination, string selectedStatus, string selectedStage)
        {

            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            //return list as per search string 
            if (selectedStatus != null && selectedStage != null)
            {
                var getSelectedStatus = ReportStatus.Initial;
                if (selectedStatus == ReportStatus.Complete.ToString())
                {
                    getSelectedStatus = ReportStatus.Complete;
                }
                else if (selectedStatus == ReportStatus.Initial.ToString())
                {
                    getSelectedStatus = ReportStatus.Initial;
                }
                else
                {
                    getSelectedStatus = ReportStatus.Pending;
                }


                var getSelectedStage = ReportStage.Draft;
                if (selectedStage == ReportStage.Draft.ToString())
                {
                    getSelectedStage = ReportStage.Draft;
                }
                else
                {
                    getSelectedStage = ReportStage.Final;
                }

                List<Report> reportList = await _dataRepository.Where<Report>(x => !x.IsDeleted
               && x.EntityId == pagination.EntityId && x.Stage == getSelectedStage && x.AuditStatus == getSelectedStatus).Include(x => x.Rating).ToListAsync();

                List<ReportAC> reportACList = _mapper.Map<List<ReportAC>>(reportList);
                pagination.TotalRecords = reportACList.Count();

                for (int i = 0; i < reportACList.Count(); i++)
                {
                    reportACList.ToList()[i].Status = (reportACList.ToList()[i].AuditStatus == ReportStatus.Initial) ? ReportStatus.Initial.ToString()
                     : (reportACList.ToList()[i].AuditStatus == ReportStatus.Pending) ? ReportStatus.Pending.ToString()
                     : ReportStatus.Complete.ToString();

                    reportACList.ToList()[i].StageName = (reportACList.ToList()[i].Stage == ReportStage.Draft) ? ReportStage.Draft.ToString()
                                 : ReportStage.Final.ToString();
                }

                //Get count of no. of observation for report list
                HashSet<Guid> reportIds = new HashSet<Guid>(reportACList.Select(s => (Guid)s.Id));
                List<ReportObservation> reportObservations = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && reportIds.Contains(a.ReportId)).ToListAsync();

                var reportAC = reportObservations.GroupBy(x => x.ReportId)
                          .Select(x => new ReportAC { Id = x.Key, noOfObservation = x.Count() }).ToList();

                for (int i = 0; i < reportACList.ToList().Count; i++)
                {
                    ReportAC totalObservation = reportAC.FirstOrDefault(a => a.Id == reportACList.ToList()[i].Id);
                    reportACList.ToList()[i].noOfObservation = totalObservation != null ? totalObservation.noOfObservation : 0;
                }
                pagination.Items = reportACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToList();
            }
            return pagination;
        }

        /// <summary>
        /// Get Details of ACM by Id
        /// </summary>
        /// <param name="acmId">ACM Id</param>
        /// <param name="pagination">pagination of ReportAC </param>
        /// <returns>ACMPresentationAC</returns>
        public async Task<ACMReportDetailAC> GetACMReportsAndReviewersByIdAsync(Guid acmId, Pagination<ReportAC> pagination)
        {

            List<ACMReportMapping> acmReportMappingList = new List<ACMReportMapping>();
            List<ACMReportMappingAC> acmReportMappingListAC = new List<ACMReportMappingAC>();
            List<ACMReviewerDocument> reviewerDocumentList = new List<ACMReviewerDocument>();

            List<ACMReviewer> acmReviewerList = new List<ACMReviewer>();
            List<ACMReviewerAC> acmReviewerACList = new List<ACMReviewerAC>();

            var getAcmReportDetail = await _dataRepository.Where<ACMReportDetail>(c => !c.IsDeleted && c.EntityId == pagination.EntityId && c.AcmId == acmId).Include(x => x.ACMPresentation).Include(x => x.AuditableEntity).FirstOrDefaultAsync();
            ACMReportDetailAC acmReportDetailAC = getAcmReportDetail != null ? _mapper.Map<ACMReportDetailAC>(getAcmReportDetail) : new ACMReportDetailAC();
            List<Report> reportList = await _reportRepository.GetReportsForACMIdAsync(pagination.EntityId);
            acmReportMappingList = await _dataRepository.Where<ACMReportMapping>(x => !x.IsDeleted && x.ACMId == acmId).Include(x => x.Report).OrderBy(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
            acmReportMappingListAC = _mapper.Map<List<ACMReportMappingAC>>(acmReportMappingList);
            if (getAcmReportDetail != null)
            {
                if (getAcmReportDetail.Id != Guid.Empty)
                {
                    if (acmReportMappingList.Count > 0)
                    {
                        //    //get added report ids
                        if (acmReportMappingList[0].Report != null)
                        {
                            HashSet<Guid> getReportIds = new HashSet<Guid>(acmReportMappingList.Select(s => (Guid)s.ReportId));


                            var getAddedReportList = reportList.Where(x => getReportIds.Contains(x.Id));
                            acmReportDetailAC.LinkedACMReportList = _mapper.Map<List<ReportAC>>(getAddedReportList);

                            for (int i = 0; i < acmReportDetailAC.LinkedACMReportList.Count; i++)
                            {
                                acmReportDetailAC.LinkedACMReportList[i].IsChecked = true;
                            }
                            acmReviewerList = await _dataRepository.Where<ACMReviewer>(a => !a.IsDeleted &&
                  a.ACMPresentationId == acmId).Include(a => a.User).OrderBy(x => x.CreatedDateTime).AsNoTracking().ToListAsync();
                            HashSet<Guid> reviewerIds = new HashSet<Guid>(acmReviewerList.Select(s => s.Id));
                            //get reviewer documents
                            reviewerDocumentList = await _dataRepository.Where<ACMReviewerDocument>(a => !a.IsDeleted &&
                                reviewerIds.Contains((Guid)a.AcmReviewerId)).OrderBy(x => x.CreatedDateTime).AsNoTracking().ToListAsync();

                            // map reviewer document in report reviewer list
                            List<ACMReviewerDocumentAC> reviewerDocuments = _mapper.Map<List<ACMReviewerDocumentAC>>(reviewerDocumentList);
                            acmReviewerACList = _mapper.Map<List<ACMReviewerAC>>(acmReviewerList);


                            for (int i = 0; i < acmReviewerACList.Count; i++)
                            {
                                acmReviewerACList[i].ReviewerDocumentList = reviewerDocuments.Where(a => a.ACMReviewerId == acmReviewerACList[i].Id).ToList();
                            }
                        }
                        else
                        {
                            acmReportDetailAC.LinkedACMReportList = new List<ReportAC>();
                        }
                    }
                }
            }
            else
            {
                acmReportDetailAC.LinkedACMReportList = new List<ReportAC>();
            }
            if (acmReportDetailAC.LinkedACMReportList != null)
            {
                for (int i = 0; i < acmReportDetailAC.LinkedACMReportList.Count(); i++)
                {
                    acmReportDetailAC.LinkedACMReportList.ToList()[i].Status = (acmReportDetailAC.LinkedACMReportList.ToList()[i].AuditStatus == ReportStatus.Initial) ? ReportStatus.Initial.ToString()
                     : (acmReportDetailAC.LinkedACMReportList.ToList()[i].AuditStatus == ReportStatus.Pending) ? ReportStatus.Pending.ToString()
                     : ReportStatus.Complete.ToString();

                    acmReportDetailAC.LinkedACMReportList.ToList()[i].StageName = (acmReportDetailAC.LinkedACMReportList.ToList()[i].Stage == ReportStage.Draft) ? ReportStage.Draft.ToString()
                                 : ReportStage.Final.ToString();
                }

                //Get count of no. of observation for report list
                HashSet<Guid> reportIds = new HashSet<Guid>(acmReportDetailAC.LinkedACMReportList.Select(s => (Guid)s.Id));
                List<ReportObservation> reportObservations = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && reportIds.Contains(a.ReportId)).ToListAsync();

                var reportAC = reportObservations.GroupBy(x => x.ReportId)
                          .Select(x => new ReportAC { Id = x.Key, noOfObservation = x.Count() }).ToList();

                for (int i = 0; i < acmReportDetailAC.LinkedACMReportList.ToList().Count; i++)
                {
                    ReportAC totalObservation = reportAC.FirstOrDefault(a => a.Id == acmReportDetailAC.LinkedACMReportList.ToList()[i].Id);
                    acmReportDetailAC.LinkedACMReportList.ToList()[i].noOfObservation = totalObservation != null ? totalObservation.noOfObservation : 0;
                }
                int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
                pagination.TotalRecords = acmReportDetailAC.LinkedACMReportList.Count();

                pagination.Items = acmReportDetailAC.LinkedACMReportList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToList();
                acmReportDetailAC.LinkedACMReportList = pagination.Items;
                acmReportDetailAC.pageIndex = pagination.PageIndex;
            }
            acmReportDetailAC.StatusList = GetEnumList<ReportStatus>();
            acmReportDetailAC.StageList = GetEnumList<ReportStage>();
            acmReportDetailAC.ACMReportReviewerList = acmReviewerACList;
            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted &&
            a.EntityId == pagination.EntityId).Include(a => a.User).AsNoTracking().ToListAsync();
            List<EntityUserMappingAC> entityUserMappingACList = _mapper.Map<List<EntityUserMappingAC>>(entityUserMappingList);
            entityUserMappingACList = entityUserMappingACList.Where(a => a.UserType != UserType.External).ToList();
            acmReportDetailAC.UserList = entityUserMappingACList;

            return acmReportDetailAC;
        }


        /// <summary>
        /// Add acm report detail
        /// </summary>
        /// <param name="acmReportDetail">ACM report detail </param>
        /// <returns>Return ACMReportDetail AC detail</returns>
        public async Task<ACMReportDetailAC> AddAcmReportAsync(ACMReportDetailAC acmReportDetail)
        {
            string acmId = acmReportDetail.AcmId;
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    if (acmReportDetail.Id != null)
                    {
                        List<ACMReportMappingAC> newReportMappingACList = new List<ACMReportMappingAC>();
                        List<ReportAC> newReportACList = acmReportDetail.LinkedACMReportList;
                        //get acm report from database
                        List<ACMReportMapping> getReportList = await _dataRepository.Where<ACMReportMapping>(a => !a.IsDeleted &&
                       a.ACMId.ToString() == acmId && a.AcmReportId == acmReportDetail.Id).AsNoTracking().ToListAsync();

                        if (getReportList != null && getReportList.Count > 0)
                        {
                            for (int i = 0; i < newReportACList.Count; i++)
                            {
                                ACMReportMappingAC aCMReportMappingAC = new ACMReportMappingAC();
                                aCMReportMappingAC.ReportId = (Guid)newReportACList[i].Id;
                                aCMReportMappingAC.ACMId = Guid.Parse(acmReportDetail.AcmId);
                                aCMReportMappingAC.CreatedDateTime = acmReportDetail.CreatedDateTime;
                                aCMReportMappingAC.CreatedBy = currentUserId;
                                aCMReportMappingAC.UpdatedDateTime = null;
                                aCMReportMappingAC.UpdatedBy = null;
                                newReportMappingACList.Add(aCMReportMappingAC);
                            }
                            List<ACMReportMapping> newReportMappingList = _mapper.Map<List<ACMReportMapping>>(newReportMappingACList);
                            //get added acm ids
                            HashSet<Guid> getAcmReportIds = new HashSet<Guid>(getReportList.Select(s => (Guid)s.ReportId));


                            //get new added acm ids
                            HashSet<Guid> newAcmReportIds = new HashSet<Guid>(newReportMappingACList.Select(s => s.ReportId));

                            //get acm report from database
                            List<ACMReviewer> getReviewerList = await _dataRepository.Where<ACMReviewer>(a => !a.IsDeleted && a.AcmReportId == acmReportDetail.Id).AsNoTracking().ToListAsync();

                            List<ACMReviewer> newReviewerList = _mapper.Map<List<ACMReviewer>>(acmReportDetail.ACMReportReviewerList);
                            //get added reviewerid
                            HashSet<Guid> getReviewerIds = new HashSet<Guid>(getReviewerList.Select(s => (Guid)s.UserId));
                            //get new added reviewerid
                            HashSet<Guid> newReviewerIds = new HashSet<Guid>(newReviewerList.Select(s => s.UserId));

                            // Check for status Get same user id reviwer
                            List<ACMReviewer> sameReviewerList = newReviewerList.Where(m => getReviewerIds.Contains(m.UserId)).ToList();
                            List<ACMReviewer> updateReviewerList = new List<ACMReviewer>();

                            for (int i = 0; i < sameReviewerList.Count; i++)
                            {
                                ACMReviewer reportUserMapping = new ACMReviewer();
                                reportUserMapping = getReviewerList.First<ACMReviewer>(a => a.UserId == sameReviewerList[i].UserId);
                                if (sameReviewerList[i].Status != reportUserMapping.Status)
                                {
                                    ACMReviewer updatedReviewer = new ACMReviewer();
                                    updatedReviewer = _mapper.Map<ACMReviewer>(sameReviewerList[i]);
                                    updatedReviewer.AcmReportId = acmReportDetail.Id;
                                    updatedReviewer.ACMPresentationId = Guid.Parse(acmReportDetail.AcmId);
                                    updatedReviewer.UpdatedBy = currentUserId;
                                    updatedReviewer.UpdatedDateTime = DateTime.UtcNow;
                                    updateReviewerList.Add(updatedReviewer);
                                }
                            }
                            _dataRepository.UpdateRange<ACMReviewer>(updateReviewerList);
                            await _dataRepository.SaveChangesAsync();

                            //New added Reviewer from client
                            List<ACMReviewer> newAddedReviewerList = newReviewerList.Where(m => !getReviewerIds.Contains(m.UserId)).ToList();
                            //get deleted reviewers from DB
                            List<ACMReviewer> getDeletedAcmReviewers = await _dataRepository.Where<ACMReviewer>(a => a.IsDeleted && a.AcmReportId == acmReportDetail.Id
                            ).AsNoTracking().ToListAsync();
                            if (getDeletedAcmReviewers.Count != 0)
                            {
                                HashSet<Guid> deletedUserIds = new HashSet<Guid>(getDeletedAcmReviewers.Select(s => s.UserId));
                                List<ACMReviewer> updateDeletedReviewerList = new List<ACMReviewer>();
                                List<ACMReviewer> updatedReviewerList =
                                    newAddedReviewerList.Where(m => deletedUserIds.Contains(m.UserId)).ToList();

                                for (int i = 0; i < updatedReviewerList.Count; i++)
                                {
                                    ACMReviewer reportUserMapping = new ACMReviewer();
                                    reportUserMapping = getDeletedAcmReviewers.First<ACMReviewer>
                                        (a => a.UserId == updatedReviewerList[i].UserId);
                                    reportUserMapping.Status = updatedReviewerList[i].Status;
                                    reportUserMapping.UpdatedBy = currentUserId;
                                    reportUserMapping.UpdatedDateTime = DateTime.UtcNow;
                                    reportUserMapping.IsDeleted = false;
                                    updateDeletedReviewerList.Add(reportUserMapping);

                                    _dataRepository.UpdateRange<ACMReviewer>(updateDeletedReviewerList);
                                    await _dataRepository.SaveChangesAsync();
                                }

                                //soft delete reviewer document
                                HashSet<Guid> reviewerIds = new HashSet<Guid>(updateDeletedReviewerList.Select(s => s.Id));
                                List<ACMReviewerDocument> reviewerDocumentList = _dataRepository.Where<ACMReviewerDocument>(x => !x.IsDeleted &&
                                reviewerIds.Contains((Guid)x.AcmReviewerId)).AsNoTracking().ToList();
                                for (int i = 0; i < reviewerDocumentList.Count; i++)
                                {

                                    //Delete file from azure
                                    if (await _fileUtility.DeleteFileAsync(reviewerDocumentList[i].DocumentPath))
                                    {
                                        reviewerDocumentList[i].IsDeleted = true;
                                        reviewerDocumentList[i].UpdatedBy = currentUserId;
                                        reviewerDocumentList[i].UpdatedDateTime = DateTime.UtcNow;
                                    }
                                }
                                _dataRepository.UpdateRange<ACMReviewerDocument>(reviewerDocumentList);
                                await _dataRepository.SaveChangesAsync();

                                // get new added reviewer
                                newAddedReviewerList = newAddedReviewerList.Where(m => !deletedUserIds.Contains(m.UserId)).ToList();

                                for (int i = 0; i < newAddedReviewerList.Count; i++)
                                {

                                    newAddedReviewerList[i].AcmReportId = acmReportDetail.Id;
                                    newAddedReviewerList[i].ACMPresentationId = Guid.Parse(acmReportDetail.AcmId);
                                    newAddedReviewerList[i].CreatedBy = currentUserId;
                                    newAddedReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                                }
                                await _dataRepository.AddRangeAsync<ACMReviewer>(newAddedReviewerList);
                                await _dataRepository.SaveChangesAsync();
                            }
                            else
                            {
                                for (int i = 0; i < newAddedReviewerList.Count; i++)
                                {
                                    newAddedReviewerList[i].AcmReportId = acmReportDetail.Id;
                                    newAddedReviewerList[i].ACMPresentationId = Guid.Parse(acmReportDetail.AcmId);
                                    newAddedReviewerList[i].CreatedBy = currentUserId;
                                    newAddedReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                                }

                                await _dataRepository.AddRangeAsync<ACMReviewer>(newAddedReviewerList);
                                await _dataRepository.SaveChangesAsync();
                            }
                            if (getReportList.Count != 0)
                            {
                                #region Remove unchecked report 

                                //Get deleted report  
                                List<ACMReportMapping> removedReportList = getReportList.Where(a => !newAcmReportIds.Contains((Guid)a.ReportId)).ToList();

                                for (int i = 0; i < removedReportList.Count; i++)
                                {
                                    removedReportList[i].ACMId = new Guid(acmId);
                                }
                                _dataRepository.RemoveRange<ACMReportMapping>(removedReportList);
                                await _dataRepository.SaveChangesAsync();

                                List<ACMReviewer> removedReviewerList = getReviewerList.Where(a => !newReviewerIds.Contains(a.UserId)).ToList();
                                HashSet<Guid> removedReviewerIds = new HashSet<Guid>(removedReviewerList.Select(s => s.Id));
                                // Remove reviewer document --hard delete
                                List<ACMReviewerDocument> removedReviewerDocuments = _dataRepository.Where<ACMReviewerDocument>(a => removedReviewerIds.Contains((Guid)a.AcmReviewerId)).Include(x => x.ACMReviewer).ToList();
                                for (int k = 0; k < removedReviewerDocuments.Count; k++)
                                {
                                    removedReviewerDocuments[k].IsDeleted = true;
                                }
                                _dataRepository.UpdateRange<ACMReviewerDocument>(removedReviewerDocuments);
                                _dataRepository.SaveChanges();

                                // Remove report  reviewer 

                                List<ACMReviewer> removedACMReviewers = _dataRepository.Where<ACMReviewer>(a => removedReviewerIds.Contains(a.Id)).ToList();
                                for (int r = 0; r < removedACMReviewers.Count; r++)
                                {
                                    removedACMReviewers[r].IsDeleted = true;
                                }
                                _dataRepository.UpdateRange<ACMReviewer>(removedACMReviewers);
                                _dataRepository.SaveChanges();

                                #endregion

                                #region Add new selected acm report

                                // Add new report
                                List<ACMReportMappingAC> newAddedReportList = newReportMappingACList.Where(m => !getAcmReportIds.Contains(m.ReportId)).ToList();
                                List<ACMReportMapping> newAddedAcmReportList = _mapper.Map<List<ACMReportMapping>>(newAddedReportList);
                                for (int i = 0; i < newAddedAcmReportList.Count; i++)
                                {

                                    newAddedAcmReportList[i].Id = Guid.NewGuid(); // due to mapping issue set id with new guid
                                    newAddedAcmReportList[i].ACMId = new Guid(acmReportDetail.AcmId);
                                    newAddedAcmReportList[i].AcmReportId = acmReportDetail.Id;
                                    newAddedAcmReportList[i].CreatedBy = currentUserId;
                                    newAddedAcmReportList[i].CreatedDateTime = DateTime.UtcNow;
                                    newAddedAcmReportList[i].UpdatedBy = null;
                                    newAddedAcmReportList[i].UpdatedDateTime = null;
                                }

                                await _dataRepository.AddRangeAsync<ACMReportMapping>(newAddedAcmReportList);
                                await _dataRepository.SaveChangesAsync();

                                #endregion
                            }


                        }
                    }
                    else
                    {
                        #region Add new report 
                        ACMReportDetail aCMReportDetailData = _mapper.Map<ACMReportDetail>(acmReportDetail);
                        ACMReportDetail aCMReportDetailToBeAdded = new ACMReportDetail();
                        if (acmReportDetail.Id == null)
                        {

                            aCMReportDetailData.CreatedDateTime = DateTime.UtcNow;
                            aCMReportDetailData.AcmId = Guid.Parse(acmReportDetail.AcmId);
                            aCMReportDetailData.ReportTitle = acmReportDetail.acmReportTitle;
                            aCMReportDetailData.EntityId = Guid.Parse(acmReportDetail.EntityId);
                            aCMReportDetailToBeAdded = await _dataRepository.AddAsync(aCMReportDetailData);
                            await _dataRepository.SaveChangesAsync();
                        }
                        List<ACMReportMapping> newAddedReportList = new List<ACMReportMapping>();// _mapper.Map<List<ACMReportMapping>>(newReportMappingACList);
                        for (int i = 0; i < acmReportDetail.LinkedACMReportList.Count; i++)
                        {
                            ACMReportMapping aCMReportMapping = new ACMReportMapping();
                            aCMReportMapping.Id = Guid.NewGuid(); // due to mapping issue set id with new guid
                            aCMReportMapping.ACMId = Guid.Parse(acmReportDetail.AcmId);
                            aCMReportMapping.ReportId = acmReportDetail.LinkedACMReportList[i].Id;
                            aCMReportMapping.AcmReportId = acmReportDetail.Id != null ? acmReportDetail.Id : aCMReportDetailToBeAdded.Id;//: getAcmReportId.Entity.Id;
                            aCMReportMapping.CreatedBy = currentUserId;
                            aCMReportMapping.CreatedDateTime = DateTime.UtcNow;
                            aCMReportMapping.UpdatedBy = null;
                            aCMReportMapping.UpdatedDateTime = null;
                            newAddedReportList.Add(aCMReportMapping);
                        }

                        await _dataRepository.AddRangeAsync<ACMReportMapping>(newAddedReportList);
                        await _dataRepository.SaveChangesAsync();

                        List<ACMReviewer> userTeamList = new List<ACMReviewer>();
                        for (int i = 0; i < acmReportDetail.ACMReportReviewerList.Count; i++)
                        {
                            ACMReviewer aCMUserTeam = new ACMReviewer();
                            aCMUserTeam.ACMPresentationId = Guid.Parse(acmReportDetail.AcmId);
                            aCMUserTeam.UserId = acmReportDetail.ACMReportReviewerList[i].UserId;
                            aCMUserTeam.AcmReportId = acmReportDetail.Id != null ? acmReportDetail.Id : aCMReportDetailToBeAdded.Id;// getAcmReportId.Entity.Id;
                            aCMUserTeam.Status = acmReportDetail.ACMReportReviewerList[i].Status;
                            aCMUserTeam.CreatedDateTime = DateTime.UtcNow;
                            aCMUserTeam.CreatedBy = currentUserId;
                            userTeamList.Add(aCMUserTeam);

                            //aCMUserTeam.CreatedBy- will added later
                        }

                        await _dataRepository.AddRangeAsync<ACMReviewer>(userTeamList);
                        await _dataRepository.SaveChangesAsync();
                        acmReportDetail.Id = acmReportDetail.Id != null ? acmReportDetail.Id : aCMReportDetailToBeAdded.Id;// getAcmReportId.Entity.Id;

                        acmReportDetail.ACMReportReviewerList = _mapper.Map<List<ACMReviewerAC>>(userTeamList);
                        #endregion
                    }

                    transaction.Commit();

                    return acmReportDetail;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Add/Update new reviewer document under an acm reviewer
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
        public async Task AddAndUploadReviewerDocumentAsync(IFormCollection formdata)
        {
            List<ACMReviewerDocument> reviewDocumentList = new List<ACMReviewerDocument>();
            // TOdo: Change it after login implementaion
            User logginUser = await _dataRepository.FirstOrDefaultAsync<User>(a => !a.IsDeleted);

            List<IFormFile> documentFiles = new List<IFormFile>();
            for (int i = 0; i < formdata.Files.Count; i++)
            {
                documentFiles.Add(formdata.Files[i]);
            }
            //get all added reviewer document
            HashSet<string> reviewerIds = new HashSet<string>(documentFiles.Select(s => s.Name));

            List<ACMReviewerDocument> getReviewerDocuments = _dataRepository.Where<ACMReviewerDocument>(a => !a.IsDeleted && reviewerIds.Contains(a.AcmReviewerId.ToString()))
               .AsNoTracking().ToList();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    for (int i = 0; i < documentFiles.Count; i++)
                    {

                        var getReviewDocumentCount = getReviewerDocuments.Where(a => a.DocumentName == documentFiles[i].FileName && a.AcmReviewerId == Guid.Parse(documentFiles[i].Name)).ToList().Count;
                        if (getReviewDocumentCount == 0)
                        {
                            ACMReviewerDocument reviewDocument = new ACMReviewerDocument();
                            reviewDocument.CreatedBy = logginUser.Id;
                            reviewDocument.CreatedDateTime = DateTime.UtcNow;
                            reviewDocument.AcmReviewerId = Guid.Parse(documentFiles[i].Name);
                            reviewDocument.DocumentName = documentFiles[i].FileName;
                            reviewDocument.DocumentPath = await _fileUtility.UploadFileAsync(documentFiles[i]);
                            reviewDocument.CreatedBy = currentUserId;
                            reviewDocument.CreatedDateTime = DateTime.UtcNow;
                            reviewDocumentList.Add(reviewDocument);
                        }
                    }
                    _dataRepository.AddRange<ACMReviewerDocument>(reviewDocumentList);
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
        /// Delete Reviewer document from reviewer document and azure storage
        /// </summary>
        /// <param name="reviewerDocumentId">Id of the Reviewer document</param>
        /// <returns>Task</returns>
        public async Task DeleteReviewerDocumentAync(Guid reviewerDocumentId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var reviewerDocument = await _dataRepository.FirstAsync<ACMReviewerDocument>(x => x.Id == reviewerDocumentId);
                    if (await _fileUtility.DeleteFileAsync(reviewerDocument.DocumentPath))//Delete file from azure
                    {
                        reviewerDocument.IsDeleted = true;
                        reviewerDocument.UpdatedBy = currentUserId;
                        reviewerDocument.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<ACMReviewerDocument>(reviewerDocument);
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
        /// Get file url and download reviewer document 
        /// </summary>
        /// <param name="reviewerDocumentId">reviewer document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        public async Task<string> DownloadReviewerDocumentAsync(Guid reviewerDocumentId)
        {
            ACMReviewerDocument reviewerDocument = await _dataRepository.FirstAsync<ACMReviewerDocument>(x => x.Id == reviewerDocumentId && !x.IsDeleted);
            return _fileUtility.DownloadFile(reviewerDocument.DocumentPath);
        }
        #endregion

    }
}