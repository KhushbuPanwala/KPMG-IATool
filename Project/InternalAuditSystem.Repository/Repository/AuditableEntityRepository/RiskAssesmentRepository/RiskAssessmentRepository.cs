using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository
{
    public class RiskAssessmentRepository : IRiskAssessmentRepository
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public IGlobalRepository _globalRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        #endregion

        #region Public method(s)
        public RiskAssessmentRepository(
            IDataRepository dataRepository,
            IMapper mapper,
            IGlobalRepository globalRepository,
            IFileUtility fileUtility,
            IAzureRepository azureRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _mapper = mapper;
            _globalRepository = globalRepository;
            _fileUtility = fileUtility;
            _azureRepository = azureRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
        }

        /// <summary>
        /// Get risk assessment details
        /// </summary>
        /// <param name="id">Risk assessment id</param>
        /// <returns>RiskAssessmentAC</returns>
        public async Task<RiskAssessmentAC> GetRiskAssessmentDetailsAsync(Guid id)
        {
            RiskAssessment riskAssessment = await _dataRepository.FirstOrDefaultAsync<RiskAssessment>(x => x.Id == id && !x.IsDeleted);

            RiskAssessmentAC riskAssessmentAC = _mapper.Map<RiskAssessmentAC>(riskAssessment);

            List<RiskAssessmentDocument> riskAssessmentDocumentList = await _dataRepository.Where<RiskAssessmentDocument>(x => x.RiskAssessmentId == id && !x.IsDeleted).ToListAsync();
            if (riskAssessmentDocumentList != null && riskAssessmentDocumentList.Count > 0)
            {
                riskAssessmentAC.RiskAssessmentDocumentACList = _mapper.Map<List<RiskAssessmentDocumentAC>>(riskAssessmentDocumentList.OrderByDescending(x => x.CreatedDateTime));
                for (var i = 0; i < riskAssessmentAC.RiskAssessmentDocumentACList.Count(); i++)
                {
                    riskAssessmentAC.RiskAssessmentDocumentACList[i].FileName = riskAssessmentAC.RiskAssessmentDocumentACList[i].Path;
                    riskAssessmentAC.RiskAssessmentDocumentACList[i].Path = _azureRepository.DownloadFile(riskAssessmentAC.RiskAssessmentDocumentACList[i].Path);
                }
            }

            return riskAssessmentAC;
        }

        /// <summary>
        /// Get RiskAssessment list for grid in list page
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of risk assessment</returns>
        public async Task<Pagination<RiskAssessmentAC>> GetRiskAssessmentListAsync(Pagination<RiskAssessmentAC> pagination)
        {
            // Apply pagination
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

            IQueryable<RiskAssessmentAC> riskAssessmentACList = _dataRepository.Where<RiskAssessment>(x => x.EntityId == pagination.EntityId
                                                                 && (!String.IsNullOrEmpty(pagination.searchText) ? x.Name.ToLower().Contains(pagination.searchText.ToLower()) : true)
                                                                 && !x.IsDeleted).Select(x =>
                                                                 new RiskAssessmentAC
                                                                 {
                                                                     Id = x.Id,
                                                                     Name = x.Name,
                                                                     StatusString = x.Status.ToString(),
                                                                     Year = x.Year,
                                                                     Summary = x.Summary,
                                                                     CreatedDateTime = x.CreatedDateTime
                                                                 });

            pagination.TotalRecords = riskAssessmentACList.Count();

            if (pagination.PageSize == 0)//Get all records
            {
                pagination.Items = await riskAssessmentACList.OrderByDescending(x => x.CreatedDateTime).Skip(0).Take(pagination.TotalRecords).ToListAsync();
            }
            else
            {
                pagination.Items = await riskAssessmentACList.OrderByDescending(x => x.CreatedDateTime).Skip(skippedRecords).Take(pagination.PageSize).ToListAsync();
            }
            List<Guid> riskAssessmentIdList = pagination.Items.Select(x => x.Id ?? new Guid()).ToList();

            List<RiskAssessmentDocument> riskAssessmentDocumentList = await _dataRepository.Where<RiskAssessmentDocument>(x => riskAssessmentIdList.Contains(x.RiskAssessmentId) && !x.IsDeleted).ToListAsync();

            List<RiskAssessmentDocumentAC> riskAssessmentDocumentACList = _mapper.Map<List<RiskAssessmentDocumentAC>>(riskAssessmentDocumentList);
            if (riskAssessmentDocumentACList != null && riskAssessmentDocumentACList.Count() > 0)
            {
                for (var i = 0; i < pagination.Items.Count(); i++)
                {
                    pagination.Items[i].RiskAssessmentDocumentACList = riskAssessmentDocumentACList.Where(x => x.RiskAssessmentId == pagination.Items[i].Id).OrderByDescending(x => x.CreatedDateTime).ToList();
                }
            }
            return pagination;
        }

        /// <summary>
        /// Add risk assessment details
        /// </summary>
        /// <param name="riskAssessmentAC">RiskAssessmentAC</param>
        /// <returns>Void</returns>
        public async Task<RiskAssessmentAC> AddRiskAssessmentAsync(RiskAssessmentAC riskAssessmentAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskAssessment riskAssessment = new RiskAssessment();
                   
                    //Check if riskAssessment exists
                    if (CheckIfRiskAssessmentNameExistAsync(riskAssessmentAC.Name, riskAssessmentAC.EntityId))
                    {
                        throw new DuplicateDataException(StringConstant.RiskAssessmentString, riskAssessmentAC.Name);
                    }
                    riskAssessmentAC.Id = new Guid();
                    riskAssessment = _mapper.Map<RiskAssessment>(riskAssessmentAC);

                    riskAssessment.CreatedDateTime = DateTime.UtcNow;
                    riskAssessment.CreatedBy = currentUserId;

                    //Add risk Assessment
                    await _dataRepository.AddAsync(riskAssessment);
                    await _dataRepository.SaveChangesAsync();

                    #region RiskAssessmentDocument add 
                    if (riskAssessmentAC.RiskAssessmentDocumentFiles != null && riskAssessmentAC.RiskAssessmentDocumentFiles.Count() > 0)
                    {
                        riskAssessmentAC.RiskAssessmentDocumentACList = await AddAndUploadRiskAssessmentDocumentFiles(riskAssessmentAC.RiskAssessmentDocumentFiles, riskAssessment.Id);
                    }
                    #endregion
                    riskAssessmentAC = _mapper.Map<RiskAssessmentAC>(riskAssessment);

                    transaction.Commit();
                    return riskAssessmentAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        /// <summary>
        /// Update risk assessment details
        /// </summary>
        /// <param name="riskAssessmentAC">RiskAssessmentAC</param>
        /// <returns>Void</returns>
        public async Task<RiskAssessmentAC> UpdateRiskAssessmentAsync(RiskAssessmentAC riskAssessmentAC)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    RiskAssessment riskAssessment = new RiskAssessment();

                    //Check if riskAssessment exists
                    if (CheckIfRiskAssessmentNameExistAsync(riskAssessmentAC.Name, riskAssessment.EntityId, riskAssessmentAC.Id))
                    {
                        throw new DuplicateDataException(StringConstant.RiskAssessmentString, riskAssessmentAC.Name);
                    }
                    riskAssessment = _mapper.Map<RiskAssessment>(riskAssessmentAC);

                    riskAssessment.UpdatedDateTime = DateTime.UtcNow;
                    riskAssessment.UpdatedBy = currentUserId;

                    //Add risk Assessment
                    _dataRepository.Update(riskAssessment);
                    await _dataRepository.SaveChangesAsync();

                    #region RiskAssessmentDocument add 
                    if (riskAssessmentAC.RiskAssessmentDocumentFiles != null && riskAssessmentAC.RiskAssessmentDocumentFiles.Count() > 0)
                    {
                        if (riskAssessmentAC.RiskAssessmentDocumentACList != null)
                        {
                            riskAssessmentAC.RiskAssessmentDocumentACList.AddRange(await AddAndUploadRiskAssessmentDocumentFiles(riskAssessmentAC.RiskAssessmentDocumentFiles, riskAssessment.Id));
                        }
                        else
                        {
                            riskAssessmentAC.RiskAssessmentDocumentACList = await AddAndUploadRiskAssessmentDocumentFiles(riskAssessmentAC.RiskAssessmentDocumentFiles, riskAssessment.Id);
                        }
                    }
                    #endregion

                    transaction.Commit();
                    return riskAssessmentAC;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        /// <summary>
        /// Delete RiskAssessment
        /// </summary>
        /// <param name="id">RiskAssessment id</param>
        /// <returns>Void</returns>
        public async Task DeleteRiskAssessmentAync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    //Delete RiskAssessmentDocument
                    List<RiskAssessmentDocument> existingRiskAssessmentDocumentList = await _dataRepository.Where<RiskAssessmentDocument>(x => x.RiskAssessmentId == id && !x.IsDeleted).ToListAsync();

                    if (existingRiskAssessmentDocumentList != null && existingRiskAssessmentDocumentList.Count() > 0)
                    {
                        for (var i = 0; i < existingRiskAssessmentDocumentList.Count(); i++)
                        {
                            existingRiskAssessmentDocumentList[i].IsDeleted = true;
                            existingRiskAssessmentDocumentList[i].UpdatedBy = currentUserId;
                            existingRiskAssessmentDocumentList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<RiskAssessmentDocument>(existingRiskAssessmentDocumentList);
                    await _dataRepository.SaveChangesAsync();

                    //Delete RiskAssessment
                    RiskAssessment riskAssessment = _dataRepository.FirstOrDefault<RiskAssessment>(x => x.Id == id && !x.IsDeleted);
                    riskAssessment.IsDeleted = true;
                    riskAssessment.UpdatedBy = currentUserId;
                    riskAssessment.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update<RiskAssessment>(riskAssessment);
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
        /// Delete RiskAssessmentDocument from db and from azure
        /// </summary>
        /// <param name="id">RiskAssessmentDocument id</param>
        /// <returns>Void</returns>
        public async Task DeleteRiskAssessmentDocumentAync(Guid id)
        {

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    RiskAssessmentDocument riskAssessmentDocument = await _dataRepository.FirstAsync<RiskAssessmentDocument>(x => x.Id == id);
                    if (await _fileUtility.DeleteFileAsync(riskAssessmentDocument.Path))//Delete file from azure
                    {
                        riskAssessmentDocument.IsDeleted = true;
                        riskAssessmentDocument.UpdatedBy = currentUserId;
                        riskAssessmentDocument.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<RiskAssessmentDocument>(riskAssessmentDocument);
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
        /// Method to download RiskAssessmentDocument
        /// </summary>
        /// <param name="id">RiskAssessmentDocument Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadRiskAssessmentDocumentAsync(Guid id)
        {
            RiskAssessmentDocument riskAssessmentDocument = await _dataRepository.FirstAsync<RiskAssessmentDocument>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(riskAssessmentDocument.Path);
        }
        #endregion

        #region Private method(s)

        /// <summary>
        /// Check if RiskAssessment name exist
        /// </summary>
        /// <param name="riskAssessmentName">Name of RiskAssessment entered by user</param>
        /// <param name="riskAssessmentId">If it is in edit page then this id of RiskAssessment will be there else in add form it will be null</param>
        /// <returns>Returns if name is duplicate or not</returns>
        private bool CheckIfRiskAssessmentNameExistAsync(string riskAssessmentName, Guid entityId, Guid? riskAssessmentId = null)
        {
            bool isNameExists;
            if (riskAssessmentId != null)
            {
                isNameExists = _dataRepository.Any<RiskAssessment>(x => x.Id != riskAssessmentId && x.EntityId == entityId && !x.IsDeleted && x.Name.ToLower() == riskAssessmentName.ToLower());
            }
            else
            {
                isNameExists = _dataRepository.Any<RiskAssessment>(x => x.Name.ToLower() == riskAssessmentName.ToLower() && x.EntityId == entityId && !x.IsDeleted);
            }
            return isNameExists;
        }

        /// <summary>
        /// Add/Upload RiskAssessmentDocument Files
        /// </summary>
        /// <param name="riskAssessmentDocumentFiles">Doc files</param>
        /// <param name="riskAssessmentId">RiskAssessmentId</param>
        /// <param name="currentUserId">Current user Id</param>
        /// <returns>Void</returns>
        private async Task<List<RiskAssessmentDocumentAC>> AddAndUploadRiskAssessmentDocumentFiles(List<IFormFile> riskAssessmentDocumentFiles, Guid riskAssessmentId)
        {
            List<string> filesUrl = new List<string>();

            //Upload file to azure
            filesUrl = await _fileUtility.UploadFilesAsync(riskAssessmentDocumentFiles);
            List<RiskAssessmentDocument> addedRiskAssessmentDocumentList = new List<RiskAssessmentDocument>();
            for (int i = 0; i < filesUrl.Count(); i++)
            {
                RiskAssessmentDocument newRiskAssessmentDocument = new RiskAssessmentDocument()
                {
                    Id = new Guid(),
                    Path = filesUrl[i],
                    RiskAssessmentId = riskAssessmentId,
                    CreatedBy = currentUserId,
                    CreatedDateTime = DateTime.UtcNow
                };
                addedRiskAssessmentDocumentList.Add(newRiskAssessmentDocument);
            }
            await _dataRepository.AddRangeAsync(addedRiskAssessmentDocumentList);
            await _dataRepository.SaveChangesAsync();
            return _mapper.Map<List<RiskAssessmentDocumentAC>>(addedRiskAssessmentDocumentList);
        }
        /// <summary>
        /// Method to Add Risk Assessment Area In New Version
        /// </summary>
        /// <param name="riskAssessmentACList">RiskAssessmentAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        public async Task AddRiskAssessmentInNewVersionAsync(List<RiskAssessmentAC> riskAssessmentACList, Guid versionId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    List<Guid> riskAssessmentIdList = riskAssessmentACList.Select(x => x.Id ?? new Guid()).ToList();
                    List<RiskAssessmentDocument> allRiskAssessmentDocumentList = await _dataRepository.Where<RiskAssessmentDocument>(x => riskAssessmentIdList.Contains(x.RiskAssessmentId) && !x.IsDeleted).ToListAsync();
                    List<RiskAssessment> riskAssessmentList = _mapper.Map<List<RiskAssessment>>(riskAssessmentACList);

                    for (var i = 0; i < riskAssessmentList.Count(); i++)
                    {
                        riskAssessmentACList[i].OldId = riskAssessmentList[i].Id;
                        riskAssessmentList[i].Id = new Guid();
                        riskAssessmentList[i].EntityId = versionId;
                        riskAssessmentList[i].CreatedBy = currentUserId;
                        riskAssessmentList[i].CreatedDateTime = DateTime.UtcNow;
                    }
                    await _dataRepository.AddRangeAsync(riskAssessmentList);
                    await _dataRepository.SaveChangesAsync();

                    List<RiskAssessmentDocument> riskAssessmentDocumentAddList = new List<RiskAssessmentDocument>();
                    for (var i = 0; i < riskAssessmentList.Count(); i++)
                    {
                        List<RiskAssessmentDocument> riskAssessmentDocumentList = allRiskAssessmentDocumentList.Where(x => x.RiskAssessmentId == riskAssessmentACList[i].Id).ToList();
                        for (var j = 0; j < riskAssessmentDocumentList.Count(); j++)
                        {
                            riskAssessmentDocumentList[j].Id = new Guid();
                            riskAssessmentDocumentList[j].RiskAssessmentId = riskAssessmentList[i].Id;
                        }
                        riskAssessmentDocumentAddList.AddRange(riskAssessmentDocumentList);
                    }
                    await _dataRepository.AddRangeAsync(riskAssessmentDocumentAddList);
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
    }
}
