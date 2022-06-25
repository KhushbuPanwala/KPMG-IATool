using AutoMapper;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using System.IO;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using System.Data;
using InternalAuditSystem.Repository.Repository.ExportToExcelRepository;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.DomainModel.Models.ReportManagement;
using InternalAuditSystem.Repository.Repository.DynamicTableRepository;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.Repository.GeneratePPTRepository;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.Repository.AzureBlobStorage;
using System.Net;
using InternalAuditSystem.Repository.Repository.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace InternalAuditSystem.Repository.Repository.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IDistributionRepository _distributionRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IObservationRepository _observationRepository;
        private readonly IAuditPlanRepository _auditPlanRepository;
        private readonly IAuditableEntityRepository _auditableEntityRepository;
        private readonly IExportToExcelRepository _exportToExcelRepository;
        private readonly IGeneratePPTRepository _generatePPTRepository;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepo;
        private readonly IDynamicTableRepository _dynamicTableRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IGlobalRepository _globalRepository;
        private readonly IWebHostEnvironment _environment;
        public ReportRepository(IDataRepository dataRepository, IMapper mapper, IDistributionRepository distributionRepository, IRatingRepository ratingRepository,
            IObservationRepository observationRepository, IAuditPlanRepository auditPlanRepository, IAuditableEntityRepository auditableEntityRepository,
                 IExportToExcelRepository exportToExcelRepository, IFileUtility fileUtility,
                 IDynamicTableRepository dynamicTableRepository, IHttpContextAccessor httpContextAccessor,
                 IGeneratePPTRepository generatePPTRepository, IAzureRepository azureRepo, IGlobalRepository globalRepository,
                 IWebHostEnvironment environment)
        {
            _distributionRepository = distributionRepository;
            _observationRepository = observationRepository;
            _ratingRepository = ratingRepository;
            _dataRepository = dataRepository;
            _mapper = mapper;
            _auditPlanRepository = auditPlanRepository;
            _auditableEntityRepository = auditableEntityRepository;
            _exportToExcelRepository = exportToExcelRepository;
            _fileUtility = fileUtility;
            _dynamicTableRepository = dynamicTableRepository;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _generatePPTRepository = generatePPTRepository;
            _azureRepo = azureRepo;
            _globalRepository = globalRepository;
            _environment = environment;
        }

        #region Generate Report

        /// <summary>
        /// Get Report data for ACM
        /// </summary>
        /// <param name="entityId">selected entityid</param>
        /// <returns>List of Reports</returns>
        public async Task<List<Report>> GetReportsForACMIdAsync(Guid entityId)
        {
            List<Report> reportList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == entityId).Include(x => x.Rating).AsNoTracking().ToListAsync();
            return reportList;
        }

        /// <summary>
        /// Get Reports data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entity id</param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>List of reports</returns>
        public async Task<List<ReportAC>> GetReportsAsync(int? page, int pageSize, string searchString, string selectedEntityId, int fromYear, int toYear)
        {
            List<Report> reportsList;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (fromYear != 0 && toYear != 0)
                {
                    reportsList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId) && a.ReportTitle.ToLower().Contains(searchString.ToLower()) && (a.CreatedDateTime.Year <= toYear && a.CreatedDateTime.Year >= fromYear))
                        .Include(a => a.Rating)
                        .Skip((page - 1 ?? 0) * pageSize)
                      .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
                else
                {
                    reportsList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId) && a.ReportTitle.ToLower().Contains(searchString.ToLower()))
                   .Include(a => a.Rating)
                   .Skip((page - 1 ?? 0) * pageSize)
                 .Take(pageSize).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if (fromYear != 0 && toYear != 0)
                {
                    reportsList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId) && (a.CreatedDateTime.Year <= toYear && a.CreatedDateTime.Year >= fromYear)).Include(a => a.Rating).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
                else
                {
                    reportsList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId)).Include(a => a.Rating).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
                }
            }

            List<ReportAC> reportACList = _mapper.Map<List<ReportAC>>(reportsList);

            for (int i = 0; i < reportACList.Count; i++)
            {
                reportACList[i].Status = (reportACList[i].AuditStatus == ReportStatus.Initial) ? ReportStatus.Initial.ToString()
                 : (reportACList[i].AuditStatus == ReportStatus.Pending) ? ReportStatus.Pending.ToString()
                 : ReportStatus.Complete.ToString();

                reportACList[i].StageName = (reportACList[i].Stage == ReportStage.Draft) ? ReportStage.Draft.ToString()
                             : ReportStage.Final.ToString();
            }

            //Get count of no. of observation for report list
            HashSet<Guid> reportIds = new HashSet<Guid>(reportsList.Select(s => s.Id));
            List<ReportObservation> reportObservations = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && reportIds.Contains(a.ReportId)).ToListAsync();

            var reportAC = reportObservations.GroupBy(x => x.ReportId)
                      .Select(x => new ReportAC { Id = x.Key, noOfObservation = x.Count() }).ToList();

            for (int i = 0; i < reportACList.Count; i++)
            {
                ReportAC totalObservation = reportAC.Where(a => a.Id == reportACList[i].Id).FirstOrDefault();
                reportACList[i].noOfObservation = totalObservation != null ? totalObservation.noOfObservation : 0;
            }
            return reportACList;
        }


        /// <summary>
        /// Get count of reports
        /// </summary>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entity id</param>
        /// <returns>count of reports</returns>
        public async Task<int> GetReportCountAsync(string searchString, string selectedEntityId)
        {
            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId) && a.ReportTitle.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId == Guid.Parse(selectedEntityId)).AsNoTracking().CountAsync();
            }
            return totalRecords;

        }


        /// <summary>
        /// Get detail of reports
        /// </summary>
        /// <param name="reportId">Report id</param>
        /// <param name="selectedEntityId">Selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>count of reports</returns>
        public async Task<ReportAC> GetReportsByIdAsync(string reportId, string selectedEntityId, int timeOffset)
        {
            Report reportDetail = new Report();
            ReportAC reportDetailAC = new ReportAC();

            List<ReportComment> reportCommentList = new List<ReportComment>();
            List<ReportUserMapping> reportUserMappingList = new List<ReportUserMapping>();
            List<ReportUserMappingAC> reportReviewerList = new List<ReportUserMappingAC>();
            List<ReviewerDocument> reviewerDocumentList = new List<ReviewerDocument>();
            if (reportId != "0")
            {
                reportDetail = await _dataRepository.FirstOrDefaultAsync<Report>(a => a.Id.ToString() == reportId && !a.IsDeleted);

                reportCommentList = await _dataRepository.Where<ReportComment>(a => !a.IsDeleted && a.ReportId.ToString() == reportId &&
                a.CreatedBy == currentUserId).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

                reportUserMappingList = await _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted &&
                a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Reviewer).Include(a => a.User).AsNoTracking().ToListAsync();
                HashSet<Guid> reportUserMappingIds = new HashSet<Guid>(reportUserMappingList.Select(s => s.Id));


                //get reviewer documents
                reviewerDocumentList = await _dataRepository.Where<ReviewerDocument>(a => !a.IsDeleted &&
                    reportUserMappingIds.Contains(a.ReportUserMappingId)).AsNoTracking().ToListAsync();

                // map reviewer document in report reviewer list
                List<ReviewerDocumentAC> reviewerDocuments = _mapper.Map<List<ReviewerDocumentAC>>(reviewerDocumentList);
                reportReviewerList = _mapper.Map<List<ReportUserMappingAC>>(reportUserMappingList);


                for (int i = 0; i < reportReviewerList.Count; i++)
                {
                    reportReviewerList[i].ReviewerDocumentList = reviewerDocuments.Where(a => a.ReportUserMappingId == reportReviewerList[i].Id).ToList();
                }
            }

            reportDetailAC = _mapper.Map<ReportAC>(reportDetail);
            if (reportCommentList.Count > 0)
            {
                reportDetailAC.Comment = reportCommentList[0].Comment;
            }

            reportDetailAC.ReviewerList = reportReviewerList;
            reportDetailAC.RatingsList = await _ratingRepository.GetRatingsByEntityIdAsync(selectedEntityId);

            reportDetailAC.StageList = GetEnumList<ReportStage>();
            reportDetailAC.StatusList = GetEnumList<ReportStatus>();

            reportDetailAC.ReviewerStatus = GetEnumList<ReviewStatus>();

            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted &&
            a.EntityId.ToString() == selectedEntityId).Include(a => a.User).AsNoTracking().ToListAsync();
            List<EntityUserMappingAC> entityUserMappingACList = _mapper.Map<List<EntityUserMappingAC>>(entityUserMappingList);
            entityUserMappingACList = entityUserMappingACList.Where(a => a.UserType != UserType.External).ToList();
            reportDetailAC.UserList = entityUserMappingACList;

            reportDetailAC.AuditStartDate = reportDetailAC.AuditPeriodStartDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);
            reportDetailAC.AuditEndDate = reportDetailAC.AuditPeriodEndDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);
            return reportDetailAC;
        }

        /// <summary>
        /// Delete report detail
        /// <param name="id">id of delete report</param>
        /// </summary>
        /// <returns>Task</returns> 
        public async Task DeleteReportAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    Report reportToDelete = await _dataRepository.FirstOrDefaultAsync<Report>(x => x.Id.ToString().Equals(id));
                    reportToDelete.IsDeleted = true;
                    reportToDelete.UpdatedBy = currentUserId;
                    reportToDelete.UpdatedDateTime = DateTime.UtcNow;
                    await _dataRepository.SaveChangesAsync();

                    //soft delete reviewer and distributors 
                    List<ReportUserMapping> reportUserDetailList = _dataRepository.Where<ReportUserMapping>(x => !x.IsDeleted && x.ReportId.ToString().Equals(id)).AsNoTracking().ToList();
                    for (int i = 0; i < reportUserDetailList.Count; i++)
                    {
                        reportUserDetailList[i].IsDeleted = true;
                        reportUserDetailList[i].UpdatedBy = currentUserId;
                        reportUserDetailList[i].UpdatedDateTime = DateTime.UtcNow;
                    }

                    _dataRepository.UpdateRange<ReportUserMapping>(reportUserDetailList);
                    await _dataRepository.SaveChangesAsync();


                    //soft delete reviewer document
                    HashSet<Guid> reviewerIds = new HashSet<Guid>(reportUserDetailList.Select(s => s.Id));
                    List<ReviewerDocument> reviewerDocumentList = _dataRepository.Where<ReviewerDocument>(x => !x.IsDeleted &&
                    reviewerIds.Contains(x.ReportUserMappingId)).AsNoTracking().ToList();
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
                    _dataRepository.UpdateRange<ReviewerDocument>(reviewerDocumentList);
                    await _dataRepository.SaveChangesAsync();

                    //soft delete observation list
                    List<ReportObservation> reportObservationList = _dataRepository.Where<ReportObservation>(x => !x.IsDeleted && x.ReportId.ToString().Equals(id)).AsNoTracking().ToList();
                    for (int i = 0; i < reportObservationList.Count; i++)
                    {
                        reportObservationList[i].IsDeleted = true;
                        reportObservationList[i].UpdatedBy = currentUserId;
                        reportObservationList[i].UpdatedDateTime = DateTime.UtcNow;
                    }

                    _dataRepository.UpdateRange<ReportObservation>(reportObservationList);
                    await _dataRepository.SaveChangesAsync();

                    //soft delete observation list
                    HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));
                    List<ReportObservationsMember> reportObservationsMemberList = _dataRepository.Where<ReportObservationsMember>
                        (x => !x.IsDeleted && reportObservationIds.Contains(x.ReportObservationId)).AsNoTracking().ToList();
                    for (int i = 0; i < reportObservationsMemberList.Count; i++)
                    {
                        reportObservationsMemberList[i].IsDeleted = true;
                        reportObservationsMemberList[i].UpdatedBy = currentUserId;
                        reportObservationsMemberList[i].UpdatedDateTime = DateTime.UtcNow;
                    }

                    _dataRepository.UpdateRange<ReportObservationsMember>(reportObservationsMemberList);
                    await _dataRepository.SaveChangesAsync();

                    //soft delete report observation reviewer
                    List<ReportObservationReviewer> reportObservationReviewerList = _dataRepository.Where<ReportObservationReviewer>
                        (x => !x.IsDeleted && reportObservationIds.Contains(x.ReportObservationId)).AsNoTracking().ToList();
                    for (int i = 0; i < reportObservationsMemberList.Count; i++)
                    {
                        reportObservationsMemberList[i].IsDeleted = true;
                        reportObservationsMemberList[i].UpdatedBy = currentUserId;
                        reportObservationsMemberList[i].UpdatedDateTime = DateTime.UtcNow;
                    }

                    _dataRepository.UpdateRange<ReportObservationReviewer>(reportObservationReviewerList);
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
        /// Add Report detail
        /// <param name="report">Report detail to add</param>
        /// </summary>
        /// <returns>Return Report AC detail</returns>
        public async Task<ReportAC> AddReportAsync(ReportAC reportAC)
        {
            if (CheckReportExist(reportAC.ReportTitle, reportAC.EntityId.ToString(), null))
            {
                throw new DuplicateDataException(StringConstant.ReportText, reportAC.ReportTitle);
            }
            else
            {
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        Report report = _mapper.Map<Report>(reportAC);
                        report.CreatedDateTime = DateTime.UtcNow;
                        report.CreatedBy = currentUserId;

                        await _dataRepository.AddAsync<Report>(report);
                        await _dataRepository.SaveChangesAsync();


                        //add report comment
                        if (!string.IsNullOrEmpty(reportAC.Comment))
                        {
                            ReportComment reportComments = new ReportComment();
                            reportComments.ReportId = report.Id;
                            reportComments.Comment = reportAC.Comment;
                            reportComments.CreatedBy = currentUserId;
                            reportComments.CreatedDateTime = DateTime.UtcNow;
                            await _dataRepository.AddAsync<ReportComment>(reportComments);
                            await _dataRepository.SaveChangesAsync();
                        }

                        // add report reviewer 
                        List<ReportUserMapping> reportUserMappingList = _mapper.Map<List<ReportUserMapping>>(reportAC.ReviewerList);
                        for (int i = 0; i < reportUserMappingList.Count; i++)
                        {
                            reportUserMappingList[i].ReportId = report.Id;
                            reportUserMappingList[i].ReportUserType = ReportUserType.Reviewer;
                            reportUserMappingList[i].CreatedBy = currentUserId;
                            reportUserMappingList[i].CreatedDateTime = DateTime.UtcNow;
                        }
                        await _dataRepository.AddRangeAsync<ReportUserMapping>(reportUserMappingList);
                        await _dataRepository.SaveChangesAsync();

                        transaction.Commit();
                        reportAC = _mapper.Map<ReportAC>(report);
                        reportAC.ReviewerList = _mapper.Map<List<ReportUserMappingAC>>(reportUserMappingList);
                        return reportAC;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Update Report detail
        /// <param name="report">Report detail to add</param>
        /// </summary>
        /// <returns>Return Report AC detail</returns>
        public async Task<ReportAC> UpdateReportAsync(ReportAC reportAC)
        {
            if (CheckReportExist(reportAC.ReportTitle, reportAC.EntityId.ToString(), reportAC.Id.ToString()))
            {
                throw new DuplicateDataException(StringConstant.ReportText, reportAC.ReportTitle);
            }
            else
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        // update report details
                        Report report = _mapper.Map<Report>(reportAC);
                        report.UpdatedBy = currentUserId;
                        report.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update<Report>(report);
                        await _dataRepository.SaveChangesAsync();

                        //get report comment detail from database
                        List<ReportComment> reportCommentList = _dataRepository.Where<ReportComment>
                            (a => a.ReportId == reportAC.Id && !a.IsDeleted).OrderByDescending(a => a.CreatedDateTime)
                            .AsNoTracking().ToList();
                        ReportComment reportComment = new ReportComment();
                        if (reportCommentList.Count != 0)
                        {
                            //add report comment detail when comment detil will be updated
                            if (reportCommentList[0].Comment != reportAC.Comment && reportAC.Comment != null)
                            {
                                reportComment.ReportId = report.Id;
                                reportComment.Comment = reportAC.Comment.Trim();
                                reportComment.CreatedBy = currentUserId;
                                reportComment.CreatedDateTime = DateTime.UtcNow;
                                await _dataRepository.AddAsync<ReportComment>(reportComment);
                                await _dataRepository.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            reportComment.ReportId = report.Id;
                            reportComment.Comment = reportAC.Comment != null ? reportAC.Comment.Trim() : "";
                            reportComment.CreatedBy = currentUserId;
                            reportComment.CreatedDateTime = DateTime.UtcNow;
                            await _dataRepository.AddAsync<ReportComment>(reportComment);
                            await _dataRepository.SaveChangesAsync();
                        }


                        #region Update Reviewers
                        //get reviewers from DB
                        List<ReportUserMapping> getReportReviewers = await _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted &&
                         a.ReportId == report.Id && a.ReportUserType == ReportUserType.Reviewer).AsNoTracking().ToListAsync();

                        //reviwer list from client
                        List<ReportUserMapping> newReviewerList = _mapper.Map<List<ReportUserMapping>>(reportAC.ReviewerList);

                        //get added reviewer UserIds
                        HashSet<Guid> getUserIds = new HashSet<Guid>(getReportReviewers.Select(s => s.UserId));

                        // Check for status Get same user id reviwer
                        List<ReportUserMapping> sameReviewerList = newReviewerList.Where(m => getUserIds.Contains(m.UserId)).ToList();
                        List<ReportUserMapping> updateReviewerList = new List<ReportUserMapping>();

                        for (int i = 0; i < sameReviewerList.Count; i++)
                        {
                            ReportUserMapping reportUserMapping = new ReportUserMapping();
                            reportUserMapping = getReportReviewers.First<ReportUserMapping>(a => a.UserId == sameReviewerList[i].UserId);
                            if (sameReviewerList[i].Status != reportUserMapping.Status)
                            {
                                ReportUserMapping updatedReviewer = new ReportUserMapping();
                                updatedReviewer = sameReviewerList[i];
                                updatedReviewer.ReportUserType = ReportUserType.Reviewer;
                                updatedReviewer.UpdatedBy = currentUserId;
                                updatedReviewer.UpdatedDateTime = DateTime.UtcNow;
                                updateReviewerList.Add(updatedReviewer);
                            }
                        }
                        _dataRepository.UpdateRange<ReportUserMapping>(updateReviewerList);
                        await _dataRepository.SaveChangesAsync();


                        //Get deleted reviewer 
                        HashSet<Guid> updatedUserIds = new HashSet<Guid>(newReviewerList.Select(s => s.UserId));
                        List<ReportUserMapping> removedReviewerList = getReportReviewers.Where(m => !updatedUserIds.Contains(m.UserId)).ToList();

                        for (int i = 0; i < removedReviewerList.Count; i++)
                        {
                            removedReviewerList[i].IsDeleted = true;
                        }
                        _dataRepository.UpdateRange<ReportUserMapping>(removedReviewerList);
                        await _dataRepository.SaveChangesAsync();


                        //New added Reviewer from client
                        List<ReportUserMapping> newAddedReviewerList = newReviewerList.Where(m => !getUserIds.Contains(m.UserId)).ToList();

                        //get deleted reviewers from DB
                        List<ReportUserMapping> getDeletedReportReviewers = await _dataRepository.Where<ReportUserMapping>(a => a.IsDeleted && a.ReportId == report.Id
                        && a.ReportUserType == ReportUserType.Reviewer).AsNoTracking().ToListAsync();
                        if (getDeletedReportReviewers.Count != 0)
                        {
                            HashSet<Guid> deletedUserIds = new HashSet<Guid>(getDeletedReportReviewers.Select(s => s.UserId));
                            List<ReportUserMapping> updateDeletedReviewerList = new List<ReportUserMapping>();
                            List<ReportUserMapping> updatedReviewerList =
                                newAddedReviewerList.Where(m => deletedUserIds.Contains(m.UserId)).ToList();

                            for (int i = 0; i < updatedReviewerList.Count; i++)
                            {
                                ReportUserMapping reportUserMapping = new ReportUserMapping();
                                reportUserMapping = getDeletedReportReviewers.First<ReportUserMapping>
                                    (a => a.UserId == updatedReviewerList[i].UserId);
                                reportUserMapping.ReportUserType = ReportUserType.Reviewer;
                                reportUserMapping.Status = updatedReviewerList[i].Status;
                                reportUserMapping.UpdatedBy = currentUserId;
                                reportUserMapping.UpdatedDateTime = DateTime.UtcNow;
                                reportUserMapping.IsDeleted = false;
                                updateDeletedReviewerList.Add(reportUserMapping);

                                _dataRepository.UpdateRange<ReportUserMapping>(updateDeletedReviewerList);
                                await _dataRepository.SaveChangesAsync();
                            }


                            //soft delete reviewer document
                            HashSet<Guid> reviewerIds = new HashSet<Guid>(updateDeletedReviewerList.Select(s => s.Id));
                            List<ReviewerDocument> reviewerDocumentList = _dataRepository.Where<ReviewerDocument>(x => !x.IsDeleted &&
                            reviewerIds.Contains(x.ReportUserMappingId)).AsNoTracking().ToList();
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
                            _dataRepository.UpdateRange<ReviewerDocument>(reviewerDocumentList);
                            await _dataRepository.SaveChangesAsync();

                            // get new added reviewer
                            newAddedReviewerList = newAddedReviewerList.Where(m => !deletedUserIds.Contains(m.UserId)).ToList();

                            for (int i = 0; i < newAddedReviewerList.Count; i++)
                            {

                                newAddedReviewerList[i].ReportId = report.Id;
                                newAddedReviewerList[i].ReportUserType = ReportUserType.Reviewer;
                                newAddedReviewerList[i].CreatedBy = currentUserId;
                                newAddedReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                            }
                            await _dataRepository.AddRangeAsync<ReportUserMapping>(newAddedReviewerList);
                            await _dataRepository.SaveChangesAsync();
                        }

                        else
                        {
                            for (int i = 0; i < newAddedReviewerList.Count; i++)
                            {
                                newAddedReviewerList[i].ReportId = report.Id;
                                newAddedReviewerList[i].ReportUserType = ReportUserType.Reviewer;
                                newAddedReviewerList[i].CreatedBy = currentUserId;
                                newAddedReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                            }

                            await _dataRepository.AddRangeAsync<ReportUserMapping>(newAddedReviewerList);
                            await _dataRepository.SaveChangesAsync();
                        }

                        #endregion

                        transaction.Commit();
                        reportAC = _mapper.Map<ReportAC>(report);
                        List<ReportUserMapping> getReportReviewerList = _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted && a.ReportId == report.Id).AsNoTracking().ToList();
                        reportAC.ReviewerList = _mapper.Map<List<ReportUserMappingAC>>(getReportReviewerList);

                        return reportAC;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Export Reports to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> ExportReportsAsync(string entityId, int timeOffset)
        {
            //Export report table data
            List<Report> reportList = await _dataRepository.Where<Report>(a => !a.IsDeleted && a.EntityId.ToString() == entityId)
                .OrderByDescending(a => a.CreatedDateTime).Include(a => a.Rating).AsNoTracking().ToListAsync();

            List<ReportAC> exportReportList = _mapper.Map<List<ReportAC>>(reportList);
            if (exportReportList.Count == 0)
            {
                ReportAC reportAC = new ReportAC();
                exportReportList.Add(reportAC);
            }
            //convert UTC date time to system date time format
            exportReportList.ForEach(a =>
            {
                a.ReportTitle = _globalRepository.SetSpecialCharecters(a.ReportTitle);
                a.Comment = _globalRepository.SetSpecialCharecters(a.Comment);
                a.StageName = a.Id != null ? a.Stage.ToString() : string.Empty;
                a.Status = a.Id != null ? a.AuditStatus.ToString() : string.Empty;
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;

                a.AuditStartDate = a.Id != null ? a.AuditPeriodStartDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.AuditEndDate = a.Id != null ? a.AuditPeriodEndDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            });

            //get added report ids
            HashSet<Guid> getReportIds = new HashSet<Guid>(exportReportList.Select(s => s.Id ?? new Guid()));
            //Export report comment
            List<ReportComment> reportCommentList = await _dataRepository.Where<ReportComment>
                (a => !a.IsDeleted && getReportIds.Contains(a.ReportId)).OrderByDescending(a => a.CreatedDateTime)
                .Include(a => a.Report).Include(a => a.UserCreatedBy).AsNoTracking().ToListAsync();
            List<ReportCommentAC> exportReportCommentList = _mapper.Map<List<ReportCommentAC>>(reportCommentList);
            if (exportReportCommentList.Count == 0)
            {
                ReportCommentAC reportCommentAC = new ReportCommentAC();
                exportReportCommentList.Add(reportCommentAC);
            }
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            //convert UTC date time to system date time format
            exportReportCommentList.ForEach(a =>
            {
                a.Comment = string.IsNullOrEmpty(a.Comment) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Comment, string.Empty));
                // convert html contant to text
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            //Get report reviewer and distributors
            List<ReportUserMapping> reportUserMappingList = await _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted && getReportIds.Contains(a.ReportId))
               .Include(a => a.Report).Include(a => a.User).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

            //Export report reviewer
            List<ReportUserMappingAC> exportReportReviewerList = _mapper.Map<List<ReportUserMappingAC>>(reportUserMappingList);
            if (exportReportReviewerList.Count == 0)
            {
                ReportUserMappingAC reportUserMappingAC = new ReportUserMappingAC();
                exportReportReviewerList.Add(reportUserMappingAC);
            }

            //convert UTC date time to system date time format
            exportReportReviewerList.ForEach(a =>
            {
                a.UserType = a.Id != null ? a.ReportUserType.ToString() : string.Empty;
                a.StatusName = (a.Id != null && a.ReportUserType != ReportUserType.Distributor) ? a.Status.ToString() : string.Empty;
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            //Export report observation
            List<ReportObservation> reportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
            getReportIds.Contains(a.ReportId))
            .Include(a => a.Report).Include(a => a.Process).Include(a => a.ObservationCategory)
            .Include(a => a.User).Include(a => a.Rating)
                .OrderBy(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            List<ReportObservationAC> reportObservationACList = _mapper.Map<List<ReportObservationAC>>(reportObservationList);

            List<AuditPlanAC> auditPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(new Guid(entityId));

            // bind plan, process and subprocess name
            for (int i = 0; i < reportObservationACList.Count; i++)
            {
                AuditPlanAC auditPlan = auditPlanList.FirstOrDefault(a => a.Id == reportObservationACList[i].AuditPlanId);
                if (auditPlan != null)
                {
                    reportObservationACList[i].AuditPlanName = auditPlan.Title;
                    reportObservationACList[i].SubProcessName = reportObservationACList[i].ProcessName;
                    var subProcess = auditPlan.PlanProcessList.Find(a => a.ProcessId == reportObservationACList[i].ProcessId);
                    reportObservationACList[i].ProcessName = auditPlan.ParentProcessList.FirstOrDefault(a => a.Id == subProcess.ParentProcessId).Name;
                }
            }

            List<ReportObservationAC> exportReportObservationList = new List<ReportObservationAC>();
            //get added report observations ids
            HashSet<Guid> getReportObservationsIds = new HashSet<Guid>(reportObservationACList.Select(s => s.Id ?? new Guid()));

            if (reportObservationACList.Count == 0)
            {
                ReportObservationAC reportObservationAC = new ReportObservationAC();
                exportReportObservationList.Add(reportObservationAC);
            }
            else
            {
                //get report observation reviewer comment
                List<ReportObservationReviewer> reportObservationReviewerList = await _dataRepository.Where<ReportObservationReviewer>
                    (a => !a.IsDeleted && getReportObservationsIds.Contains(a.ReportObservationId))
                    .Include(a => a.User).Include(a => a.UserCreatedBy)
                    .OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

                List<ReportObservationReviewerAC> exportReportObservationReviewerList = _mapper.Map<List<ReportObservationReviewerAC>>(reportObservationReviewerList);
                //Todo: merge two list  and create new list with repeated data
                for (int i = 0; i < reportObservationACList.Count; i++)
                {
                    ReportObservationAC addedRepeatedObservation = _mapper.Map<ReportObservationAC>(reportObservationACList[i]);
                    List<ReportObservationReviewerAC> reviewerList =
                        exportReportObservationReviewerList.Where(a => a.ReportObservationId == reportObservationACList[i].Id).ToList();
                    int reportObservationCount = reportObservationACList.Where(a => a.Id == reportObservationACList[i].Id).Count();

                    if (reviewerList.Count != 0)
                    {
                        for (int l = 0; l < reviewerList.Count; l++)
                        {
                            ReportObservationAC getRepeatedObservation = new ReportObservationAC();
                            //due to issue of binding static data in list need to bind data manually each and every property
                            getRepeatedObservation.Id = addedRepeatedObservation.Id;
                            getRepeatedObservation.ReportTitle = addedRepeatedObservation.ReportTitle;
                            getRepeatedObservation.AuditPlanName = addedRepeatedObservation.AuditPlanName;
                            getRepeatedObservation.ProcessName = addedRepeatedObservation.ProcessName;
                            getRepeatedObservation.SubProcessName = addedRepeatedObservation.SubProcessName;
                            getRepeatedObservation.ObservationCategory = addedRepeatedObservation.ObservationCategory;
                            getRepeatedObservation.Heading = addedRepeatedObservation.Heading;
                            getRepeatedObservation.Background = addedRepeatedObservation.Background;
                            getRepeatedObservation.Observations = addedRepeatedObservation.Observations;
                            getRepeatedObservation.Rating = addedRepeatedObservation.Rating;
                            getRepeatedObservation.ObservationType = addedRepeatedObservation.ObservationType;
                            getRepeatedObservation.IsRepeatObservation = addedRepeatedObservation.IsRepeatObservation;
                            getRepeatedObservation.RootCause = addedRepeatedObservation.RootCause;
                            getRepeatedObservation.Implication = addedRepeatedObservation.Implication;
                            getRepeatedObservation.Disposition = addedRepeatedObservation.Disposition;
                            getRepeatedObservation.ObservationStatus = addedRepeatedObservation.ObservationStatus;
                            getRepeatedObservation.Recommendation = addedRepeatedObservation.Recommendation;
                            getRepeatedObservation.ManagementResponse = addedRepeatedObservation.ManagementResponse;
                            getRepeatedObservation.TargetDate = addedRepeatedObservation.TargetDate;
                            getRepeatedObservation.LinkedObservation = addedRepeatedObservation.LinkedObservation;
                            getRepeatedObservation.Conclusion = addedRepeatedObservation.Conclusion;
                            getRepeatedObservation.Auditor = addedRepeatedObservation.Auditor;
                            getRepeatedObservation.CreatedDateTime = addedRepeatedObservation.CreatedDateTime;
                            getRepeatedObservation.UpdatedDateTime = addedRepeatedObservation.UpdatedDateTime;
                            // Reviwer comment data
                            getRepeatedObservation.ReviewerName = reviewerList[l].ReviewerName;
                            getRepeatedObservation.Comment = reviewerList[l].Comment;
                            getRepeatedObservation.CommentCreatedDateTime = reviewerList[l].CreatedDateTime;
                            exportReportObservationList.Add(getRepeatedObservation);
                        }
                    }
                    else
                    {
                        ReportObservationAC getRepeatedObservation = new ReportObservationAC();
                        //due to issue of binding static data in list need to bind data manually each and every property
                        getRepeatedObservation.Id = addedRepeatedObservation.Id;
                        getRepeatedObservation.ReportTitle = addedRepeatedObservation.ReportTitle;
                        getRepeatedObservation.AuditPlanName = addedRepeatedObservation.AuditPlanName;
                        getRepeatedObservation.ProcessName = addedRepeatedObservation.ProcessName;
                        getRepeatedObservation.SubProcessName = addedRepeatedObservation.SubProcessName;
                        getRepeatedObservation.ObservationCategory = addedRepeatedObservation.ObservationCategory;
                        getRepeatedObservation.Heading = addedRepeatedObservation.Heading;
                        getRepeatedObservation.Background = addedRepeatedObservation.Background;
                        getRepeatedObservation.Observations = addedRepeatedObservation.Observations;
                        getRepeatedObservation.Rating = addedRepeatedObservation.Rating;
                        getRepeatedObservation.ObservationType = addedRepeatedObservation.ObservationType;
                        getRepeatedObservation.IsRepeatObservation = addedRepeatedObservation.IsRepeatObservation;
                        getRepeatedObservation.RootCause = addedRepeatedObservation.RootCause;
                        getRepeatedObservation.Implication = addedRepeatedObservation.Implication;
                        getRepeatedObservation.Disposition = addedRepeatedObservation.Disposition;
                        getRepeatedObservation.ObservationStatus = addedRepeatedObservation.ObservationStatus;
                        getRepeatedObservation.Recommendation = addedRepeatedObservation.Recommendation;
                        getRepeatedObservation.ManagementResponse = addedRepeatedObservation.ManagementResponse;
                        getRepeatedObservation.TargetDate = addedRepeatedObservation.TargetDate;
                        getRepeatedObservation.LinkedObservation = addedRepeatedObservation.LinkedObservation;
                        getRepeatedObservation.Conclusion = addedRepeatedObservation.Conclusion;
                        getRepeatedObservation.Auditor = addedRepeatedObservation.Auditor;
                        getRepeatedObservation.CreatedDateTime = addedRepeatedObservation.CreatedDateTime;
                        getRepeatedObservation.UpdatedDateTime = addedRepeatedObservation.UpdatedDateTime;
                        exportReportObservationList.Add(getRepeatedObservation);
                    }
                }
            }

            //convert UTC date time to system date time format
            exportReportObservationList.ForEach(a =>
            {
                // convert html contant to text
                a.Background = string.IsNullOrEmpty(a.Background) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Background, string.Empty));
                a.Observations = string.IsNullOrEmpty(a.Observations) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Observations, string.Empty));
                a.RootCause = string.IsNullOrEmpty(a.RootCause) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.RootCause, string.Empty));
                a.Implication = string.IsNullOrEmpty(a.Implication) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Implication, string.Empty));
                a.Recommendation = string.IsNullOrEmpty(a.Recommendation) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Recommendation, string.Empty));
                a.ManagementResponse = string.IsNullOrEmpty(a.ManagementResponse) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.ManagementResponse, string.Empty));
                a.Conclusion = string.IsNullOrEmpty(a.Conclusion) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Conclusion, string.Empty));
                a.Comment = string.IsNullOrEmpty(a.Comment) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(a.Comment, string.Empty));
                a.LinkedObservation = (a.LinkedObservation != string.Empty && a.LinkedObservation != null) ? reportObservationACList.FirstOrDefault(x => x.Id == new Guid(a.LinkedObservation))?.Heading : string.Empty;
                a.IsRepeated = a.Id != null ? (a.IsRepeatObservation ? StringConstant.Yes : StringConstant.No) : string.Empty;
                a.ObservationTypeName = a.Id != null ? a.ObservationType.ToString() : string.Empty;
                a.DispositionName = a.Id != null ? a.Disposition.ToString() : string.Empty;
                a.Status = a.Id != null ? a.ObservationStatus.ToString() : string.Empty;
                a.ObservationTargetDate = a.Id != null ? a.TargetDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.CommentCreatedDate = (a.Id != null && a.CommentCreatedDateTime != null) ? a.CommentCreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            //Export report observation responsible person
            List<ReportObservationsMember> reportObservationsMemberList = await _dataRepository.Where<ReportObservationsMember>(a => !a.IsDeleted
            && getReportObservationsIds.Contains(a.ReportObservationId))
                .Include(a => a.User).Include(a => a.ReportObservation)
                .OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            List<ReportObservationsMemberAC> exportReportObservationsMemberList = _mapper.Map<List<ReportObservationsMemberAC>>(reportObservationsMemberList);
            foreach (var item in exportReportObservationsMemberList)
            {
                ReportObservationAC reportObservation = exportReportObservationList.FirstOrDefault(a => a.Id == item.ReportObservationId);
                item.ReportTitle = reportObservation != null ? reportObservation.ReportTitle : string.Empty;
            }

            if (exportReportObservationsMemberList.Count == 0)
            {
                ReportObservationsMemberAC reportObservationsMemberAC = new ReportObservationsMemberAC();
                exportReportObservationsMemberList.Add(reportObservationsMemberAC);
            }
            //convert UTC date time to system date time format
            exportReportObservationsMemberList.ForEach(a =>
            {
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            //get all added observation ids
            HashSet<string> reportObservationIds = new HashSet<string>(exportReportObservationList.Select(s => s.Id.ToString()));

            List<ReportObservationTable> observationsTable = _dataRepository.Where<ReportObservationTable>(a => !a.IsDeleted
            && reportObservationIds.Contains(a.ReportObservationId.ToString())).AsNoTracking().ToList();

            List<JSONTable> jsonObservationsTable = new List<JSONTable>();

            for (int i = 0; i < observationsTable.Count; i++)
            {
                JSONTable jsonTable = new JSONTable();
                string reportName = string.Empty, observationName = string.Empty;
                ReportObservationAC reportObservationAC = exportReportObservationList.FirstOrDefault(a => a.Id == observationsTable[i].ReportObservationId);
                if (reportObservationAC != null)
                {
                    reportName = reportObservationAC.ReportTitle;
                    observationName = reportObservationAC.Heading;
                }
                jsonTable.Name = reportName + "$$" + observationName;
                jsonTable.JsonData = await GetJsonReportObservationTableAsync(observationsTable[i]);
                jsonObservationsTable.Add(jsonTable);
            }
            //crete dynamic directory
            dynamic dynamicDictionary = new DynamicDictionary<string, dynamic>();
            dynamicDictionary.Add(StringConstant.ReportModule, exportReportList);
            dynamicDictionary.Add(StringConstant.ReportCommentModule, exportReportCommentList);
            dynamicDictionary.Add(StringConstant.ReportReviewerModule, exportReportReviewerList);
            dynamicDictionary.Add(StringConstant.ReportObservationModule, exportReportObservationList);
            dynamicDictionary.Add(StringConstant.ReportObservationMemberModule, exportReportObservationsMemberList);
            //for json data
            dynamicDictionary.Add(StringConstant.ReportObservationTableModule, null);

            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
            try
            {
                Tuple<string, MemoryStream> fileData = await _exportToExcelRepository.CreateExcelFile(dynamicDictionary, jsonObservationsTable, StringConstant.ReportModule + "(" + entityName + ")");
                return fileData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add/Update new reviewer document under a report reviewer
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
        public async Task AddAndUploadReviewerDocumentAsync(IFormCollection formdata)
        {
            List<ReviewerDocument> reviewDocumentList = new List<ReviewerDocument>();

            List<IFormFile> documentFiles = new List<IFormFile>();
            for (int i = 0; i < formdata.Files.Count; i++)
            {
                documentFiles.Add(formdata.Files[i]);
            }

            bool isValidFormate = _globalRepository.CheckFileExtention(documentFiles);

            if (isValidFormate)
            {
                throw new InvalidFileFormate();
            }

            bool isFileSizeValid = _globalRepository.CheckFileSize(documentFiles);

            if (isFileSizeValid)
            {
                throw new InvalidFileSize();
            }

            HashSet<string> reviewerIds = new HashSet<string>(documentFiles.Select(s => s.Name));
            List<ReviewerDocument> getReviewerDocuments = _dataRepository.Where<ReviewerDocument>(a => !a.IsDeleted &&
            //get all added reviewer document
            reviewerIds.Contains(a.ReportUserMappingId.ToString())).AsNoTracking().ToList();
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    //to do remove after login implementation
                    
                    for (int i = 0; i < documentFiles.Count; i++)
                    {
                        var getReviewDocumentCount = getReviewerDocuments.Where(a => a.ReportUserMappingId.ToString() == documentFiles[i].Name
                                && a.DocumentName == documentFiles[i].FileName).ToList().Count;
                        if (getReviewDocumentCount == 0)
                        {
                            ReviewerDocument reviewDocument = new ReviewerDocument();
                            reviewDocument.CreatedBy = currentUserId;
                            reviewDocument.CreatedDateTime = DateTime.UtcNow;
                            reviewDocument.ReportUserMappingId = new Guid(documentFiles[i].Name);
                            reviewDocument.DocumentName = documentFiles[i].FileName;
                            reviewDocument.DocumentPath = await _fileUtility.UploadFileAsync(documentFiles[i]);
                            reviewDocumentList.Add(reviewDocument);
                        }
                    }
                    _dataRepository.AddRange<ReviewerDocument>(reviewDocumentList);
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
                    var reviewerDocument = await _dataRepository.FirstAsync<ReviewerDocument>(x => x.Id == reviewerDocumentId);
                    if (await _fileUtility.DeleteFileAsync(reviewerDocument.DocumentPath))//Delete file from azure
                    {
                        reviewerDocument.IsDeleted = true;
                        reviewerDocument.UpdatedBy = currentUserId;
                        reviewerDocument.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<ReviewerDocument>(reviewerDocument);
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
            ReviewerDocument reviewerDocument = await _dataRepository.FirstAsync<ReviewerDocument>(x => x.Id == reviewerDocumentId && !x.IsDeleted);
            return _fileUtility.DownloadFile(reviewerDocument.DocumentPath);
        }
        #endregion

        #region Report Distributors

        /// <summary>
        /// Add distributor detail
        /// </summary>
        /// <param name="distributor">detail of distributor</param>
        /// <returns>Task</returns>
        public async Task AddDistributorsAsync(List<ReportUserMappingAC> distributors)
        {

            Guid reportId = distributors[0].ReportId;

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    #region Update Distributors
                    //distributor list from client
                    List<ReportUserMapping> addReportDistributorsList = _mapper.Map<List<ReportUserMapping>>(distributors);

                    //get distributor from DB
                    List<ReportUserMapping> getReportDistributors = await _dataRepository.Where<ReportUserMapping>(a => a.ReportId == reportId && !a.IsDeleted
                    && a.ReportUserType == ReportUserType.Distributor).AsNoTracking().ToListAsync();
                    if (getReportDistributors.Count != 0)
                    {
                        //get added distributor UserIds
                        HashSet<Guid> getUserIds = new HashSet<Guid>(getReportDistributors.Select(s => s.UserId));

                        //Get deleted distributor 
                        HashSet<Guid> updatedUserIds = new HashSet<Guid>(addReportDistributorsList.Select(s => s.UserId));
                        List<ReportUserMapping> removedDistributorList = getReportDistributors.Where(m => !updatedUserIds.Contains(m.UserId)).ToList();


                        for (int i = 0; i < removedDistributorList.Count; i++)
                        {
                            removedDistributorList[i].IsDeleted = true;
                            removedDistributorList[i].UpdatedBy = currentUserId;
                            removedDistributorList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                        _dataRepository.UpdateRange<ReportUserMapping>(removedDistributorList);
                        await _dataRepository.SaveChangesAsync();

                        //New added distributor from client
                        List<ReportUserMapping> newAddedDistributorList = addReportDistributorsList.Where(m => !getUserIds.Contains(m.UserId)).ToList();

                        //get deleted distributors from DB
                        List<ReportUserMapping> getDeletedReportDistributors = await _dataRepository.Where<ReportUserMapping>(a => a.IsDeleted && a.ReportId == reportId
                        && a.ReportUserType == ReportUserType.Distributor).AsNoTracking().ToListAsync();

                        HashSet<Guid> deletedUserIds = new HashSet<Guid>(getDeletedReportDistributors.Select(s => s.UserId));
                        List<ReportUserMapping> updateDeletedReviewerList = new List<ReportUserMapping>();
                        List<ReportUserMapping> updatedDistributorList =
                            newAddedDistributorList.Where(m => deletedUserIds.Contains(m.UserId)).ToList();

                        for (int i = 0; i < updatedDistributorList.Count; i++)
                        {
                            ReportUserMapping reportUserMapping = new ReportUserMapping();
                            reportUserMapping = getDeletedReportDistributors.First<ReportUserMapping>(a => a.UserId == updatedDistributorList[i].UserId);
                            reportUserMapping.ReportUserType = ReportUserType.Distributor;
                            reportUserMapping.Status = updatedDistributorList[i].Status;
                            reportUserMapping.UpdatedBy = currentUserId;
                            reportUserMapping.UpdatedDateTime = DateTime.UtcNow;
                            reportUserMapping.IsDeleted = false;
                            updateDeletedReviewerList.Add(reportUserMapping);

                        }

                        _dataRepository.UpdateRange<ReportUserMapping>(updateDeletedReviewerList);
                        await _dataRepository.SaveChangesAsync();

                        newAddedDistributorList = newAddedDistributorList.Where(m => !deletedUserIds.Contains(m.UserId)).ToList();


                        for (int i = 0; i < newAddedDistributorList.Count; i++)
                        {
                            newAddedDistributorList[i].ReportId = reportId;
                            newAddedDistributorList[i].ReportUserType = ReportUserType.Distributor;
                            newAddedDistributorList[i].CreatedBy = currentUserId;
                            newAddedDistributorList[i].CreatedDateTime = DateTime.UtcNow;
                        }

                        await _dataRepository.AddRangeAsync<ReportUserMapping>(newAddedDistributorList);
                        await _dataRepository.SaveChangesAsync();
                    }
                    else
                    {
                        for (int i = 0; i < addReportDistributorsList.Count; i++)
                        {
                            addReportDistributorsList[i].ReportId = reportId;
                            addReportDistributorsList[i].ReportUserType = ReportUserType.Distributor;
                            addReportDistributorsList[i].CreatedBy = currentUserId;
                            addReportDistributorsList[i].CreatedDateTime = DateTime.UtcNow;
                        }
                        await _dataRepository.AddRangeAsync<ReportUserMapping>(addReportDistributorsList);
                        await _dataRepository.SaveChangesAsync();
                    }

                    #endregion
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
        /// Get Distributors for Report
        /// </summary>
        /// <param name="selectedEntityId">selected Entity Id </param>
        /// <param name="reportId">Report Id </param>
        /// <returns>Detail of available distributor, report distibutors</returns>
        public async Task<ReportDistributorsAC> GetDistributorsByReportIdAsync(string selectedEntityId, string reportId)
        {
            ReportDistributorsAC reportDistributorsAC = new ReportDistributorsAC();
            reportDistributorsAC.DistributorsList = await _distributionRepository.GetAllDistributorsAsync(selectedEntityId);

            List<ReportUserMapping> distributorsList = await _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted && a.ReportId.ToString() == reportId
            && a.ReportUserType == ReportUserType.Distributor).Include(a => a.User).AsNoTracking().ToListAsync();

            List<ReportUserMappingAC> reportUserMappingACList = _mapper.Map<List<ReportUserMappingAC>>(distributorsList);
            reportDistributorsAC.ReportDistributorsList = reportUserMappingACList;
            return reportDistributorsAC;

        }

        #endregion

        #region Report Observation List
        /// <summary>
        /// Get Process data
        /// </summary>
        /// <param name="selectedEntityId">Selected entity id</param>
        /// /// <param name="reportId">Selected report Id </param>
        /// <returns>List of Audit plan process</returns>
        public async Task<List<AuditPlanAC>> GetPlanProcesessInitDataAsync(string selectedEntityId, string reportId)
        {
            List<AuditPlanAC> auditPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(new Guid(selectedEntityId));
            return auditPlanList;
        }

        /// <summary>
        /// Get all observation from observation table and report observation table
        /// </summary>
        /// <param name="planId">selected Plan id</param>
        /// <param name="subProcessId">Selected subprocess id</param>
        /// <param name="selectedEntityId">current entity id</param>
        /// <param name="reportId">current report Id</param>
        /// <returns>ReportDetailAC detail</returns>
        public async Task<ReportDetailAC> GetAllObservationsAsync(string planId, string subProcessId, string selectedEntityId, string reportId)
        {
            ReportDetailAC reportDetailAC = new ReportDetailAC();
            //get plan wise observation list
            List<Observation> observationList = await _observationRepository.GetObservationsByPlanSubProcessIdAsync(planId, subProcessId);

            List<ReportObservation> getReportObservation = new List<ReportObservation>();
            //bind data in edit mode 
            if (planId == null && subProcessId == null)
            {
                //get report observation from database
                getReportObservation = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
                 a.ReportId.ToString() == reportId).OrderBy(a => a.SortOrder).AsNoTracking().ToListAsync();
            }
            else
            {
                //get report observation from database
                getReportObservation = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
               a.ReportId.ToString() == reportId && a.ProcessId.ToString() == subProcessId)
                    .OrderBy(a => a.SortOrder).AsNoTracking().ToListAsync();
            }

            //get report observation list
            List<ReportObservationAC> getReportObservationAC = _mapper.Map<List<ReportObservationAC>>(getReportObservation);

            if (getReportObservationAC.Count != 0)
            {
                //get added observation ids
                HashSet<Guid> getObservationIds = new HashSet<Guid>(getReportObservationAC.Select(s => s.Id ?? new Guid()));

                //get new added observation ids
                HashSet<Guid> newObservationIds = new HashSet<Guid>(observationList.Select(s => s.Id));

                //Get new observation 
                observationList = observationList.Where(m => !getObservationIds.Contains(m.Id)).ToList();
                HashSet<Guid> observationIds = new HashSet<Guid>(observationList.Select(s => s.Id));

            }
            //get observation from report observation for edit openUploadFileModal
            for (int i = 0; i < getReportObservationAC.Count; i++)
            {
                getReportObservationAC[i].IsSelected = true;
                getReportObservationAC[i].IsAllowEdit = true;
                getReportObservationAC[i].IsAllowDelete = true;
                getReportObservationAC[i].IsAllowView = true;
            }
            reportDetailAC.ReportObservationList = getReportObservationAC;

            List<ObservationAC> observationACList = _mapper.Map<List<ObservationAC>>(observationList);
            reportDetailAC.ReportObservationList.AddRange(_mapper.Map<List<ReportObservationAC>>(observationACList));

            return reportDetailAC;
        }

        /// <summary>
        /// Delete report detail
        /// <param name="id">Report id for delete</param>
        /// <param name="reportObservationId">id of delete report observation</param>
        /// </summary>
        /// <returns>Task</returns> 
        public async Task DeleteReportObservationAsync(string id, string reportObservationId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    ReportObservation reportObservationToDelete = await _dataRepository.FirstOrDefaultAsync<ReportObservation>(x => x.ReportId.ToString() == id
                    && x.Id.ToString() == reportObservationId);
                    _dataRepository.Remove<ReportObservation>(reportObservationToDelete);
                    await _dataRepository.SaveChangesAsync();

                    //delete report observation user 
                    List<ReportObservationsMember> removedObservationMemebers = _dataRepository.Where<ReportObservationsMember>(a => a.ReportObservationId.ToString() == reportObservationId).ToList();
                    _dataRepository.RemoveRange<ReportObservationsMember>(removedObservationMemebers);
                    await _dataRepository.SaveChangesAsync();

                    // to do :remove report observation document after implementation
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
        /// Add Observations detail
        /// </summary>
        /// <param name="reportObservations">report Observations </param>
        /// <returns>Task</returns>
        public async Task AddObservationsAsync(ReportDetailAC reportObservations)
        {
            {
                string reportId = reportObservations.ReportId;
                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        //get report observation from database
                        List<ReportObservation> getReportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
                       a.ReportId.ToString() == reportId).AsNoTracking().ToListAsync();

                        //observation list from client
                        List<ReportObservationAC> newReportObservationList = reportObservations.ReportObservationList;

                        for (int i = 0; i < newReportObservationList.Count; i++)
                        {
                            newReportObservationList[i].CreatedDateTime = DateTime.UtcNow;
                            newReportObservationList[i].CreatedBy = currentUserId;
                            newReportObservationList[i].UpdatedBy = null;
                            newReportObservationList[i].UpdatedDateTime = null;
                        }

                        //get added observation ids
                        HashSet<Guid> getObservationIds = new HashSet<Guid>(getReportObservationList.Select(s => s.ObservationId));

                        //get new added observation ids
                        HashSet<Guid> newObservationIds = new HashSet<Guid>(newReportObservationList.Select(s => s.ObservationId));

                        if (getReportObservationList.Count != 0)
                        {
                            #region Remove unchecked observation 

                            //Get deleted report observation 
                            List<ReportObservation> removedReportObservationList = getReportObservationList.Where(a => !newObservationIds.Contains(a.ObservationId)).ToList();

                            for (int i = 0; i < removedReportObservationList.Count; i++)
                            {
                                removedReportObservationList[i].ReportId = new Guid(reportId);
                            }
                            _dataRepository.RemoveRange<ReportObservation>(removedReportObservationList);
                            await _dataRepository.SaveChangesAsync();

                            // Remove report observation reviewer and first person responsible
                            HashSet<Guid> removedObservationIds = new HashSet<Guid>(removedReportObservationList.Select(s => s.ObservationId));
                            List<ReportObservationsMember> removedObservationMembers = _dataRepository.Where<ReportObservationsMember>(a => removedObservationIds.Contains(a.ReportObservationId)).ToList();
                            _dataRepository.RemoveRange<ReportObservationsMember>(removedObservationMembers);
                            _dataRepository.SaveChanges();

                            // Remove report observation table --hard delete
                            List<ReportObservationTable> removedObservationTables = _dataRepository.Where<ReportObservationTable>(a => removedObservationIds.Contains(a.ReportObservationId)).ToList();
                            _dataRepository.RemoveRange<ReportObservationTable>(removedObservationTables);
                            _dataRepository.SaveChanges();

                            // Remove report observation document --hard delete
                            List<ReportObservationsDocument> removedReportObservationsDocuments = _dataRepository.Where<ReportObservationsDocument>(a => removedObservationIds.Contains(a.ReportObservationId)).ToList();
                            _dataRepository.RemoveRange<ReportObservationsDocument>(removedReportObservationsDocuments);
                            _dataRepository.SaveChanges();

                            #endregion

                            #region Add new selected observation

                            // Add new report observation
                            List<ReportObservationAC> newAddedObservationList = newReportObservationList.Where(m => !getObservationIds.Contains(m.ObservationId)).ToList();


                            List<ReportObservation> newAddedReportObservationList = _mapper.Map<List<ReportObservation>>(newAddedObservationList);
                            for (int i = 0; i < newAddedReportObservationList.Count; i++)
                            {

                                newAddedReportObservationList[i].Id = new Guid(); // due to mapping issue set id with new guid
                                newAddedReportObservationList[i].ReportId = new Guid(reportObservations.ReportId);
                                newAddedReportObservationList[i].CreatedBy = currentUserId;
                                newAddedReportObservationList[i].CreatedDateTime = DateTime.UtcNow;
                                newAddedReportObservationList[i].UpdatedBy = null;
                                newAddedReportObservationList[i].UpdatedDateTime = null;
                                newAddedReportObservationList[i].AuditPlan = null;
                                newAddedReportObservationList[i].Process = null;
                            }

                            await _dataRepository.AddRangeAsync<ReportObservation>(newAddedReportObservationList);
                            await _dataRepository.SaveChangesAsync();

                            // add report observation table and observation document detail
                            await AddTableAndDocumentFromObservation(newObservationIds, newAddedReportObservationList, currentUserId);

                            #endregion

                            #region update report observations ordering
                            List<ReportObservationAC> updatedReportObservationList = newReportObservationList.Where(a => getObservationIds.Contains(a.ObservationId)).ToList();
                            List<ReportObservation> orderdReportObservationList = _mapper.Map<List<ReportObservation>>(updatedReportObservationList);

                            for (int i = 0; i < orderdReportObservationList.Count; i++)
                            {
                                orderdReportObservationList[i].CreatedBy = currentUserId;
                                orderdReportObservationList[i].CreatedDateTime = DateTime.UtcNow;
                                orderdReportObservationList[i].UpdatedBy = null;
                                orderdReportObservationList[i].UpdatedDateTime = null;
                                orderdReportObservationList[i].AuditPlan = null;
                                orderdReportObservationList[i].Process = null;
                            }
                            _dataRepository.UpdateRange<ReportObservation>(orderdReportObservationList);
                            await _dataRepository.SaveChangesAsync();
                            #endregion
                        }
                        else
                        {
                            #region Add new report observation 
                            List<ReportObservation> newAddedReportObservationList = _mapper.Map<List<ReportObservation>>(newReportObservationList);
                            for (int i = 0; i < newAddedReportObservationList.Count; i++)
                            {
                                newAddedReportObservationList[i].Id = new Guid(); // due to mapping issue set id with new guid
                                newAddedReportObservationList[i].ReportId = new Guid(reportObservations.ReportId);
                                newAddedReportObservationList[i].CreatedBy = currentUserId;
                                newAddedReportObservationList[i].CreatedDateTime = DateTime.UtcNow;
                                newAddedReportObservationList[i].UpdatedBy = null;
                                newAddedReportObservationList[i].UpdatedDateTime = null;
                                newAddedReportObservationList[i].AuditPlan = null;
                                newAddedReportObservationList[i].Process = null;
                            }

                            await _dataRepository.AddRangeAsync<ReportObservation>(newAddedReportObservationList);
                            await _dataRepository.SaveChangesAsync();

                            // add report observation table and observation document detail
                            await AddTableAndDocumentFromObservation(newObservationIds, newAddedReportObservationList, currentUserId);
                            #endregion
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
        }
        #endregion

        #region Observation Tab
        /// <summary>
        /// Get Reports data
        /// </summary>
        /// <param name="reportId">Report Id</param>
        /// <param name="reportObservationId">Report Observation id</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>List of reports</returns>
        public async Task<ReportDetailAC> GetReportObservationsAsync(string reportId, string reportObservationId, string selectedEntityId)
        {
            List<ReportObservation> reportObservationList = new List<ReportObservation>();

            ReportDetailAC reportDetailAC = new ReportDetailAC();

            // Get all report observation
            if (reportObservationId == null)
            {
                reportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && a.ReportId.ToString() == reportId)
            .OrderBy(a => a.SortOrder).AsNoTracking().ToListAsync();
            }
            else
            {
                //get report observation for edit
                reportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && a.ReportId.ToString() == reportId
               && a.Id.ToString() == reportObservationId).OrderBy(a => a.SortOrder).AsNoTracking().ToListAsync();

            }

            #region bind data for report observation

            List<ReportObservationAC> reportObservationACList = _mapper.Map<List<ReportObservationAC>>(reportObservationList);

            //get report observation reviewer adn first person responsible
            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));

            //get document list 
            List<ReportObservationsDocument> reportObservationsDocuments = await _dataRepository.Where<ReportObservationsDocument>(a => !a.IsDeleted
            && reportObservationIds.Contains(a.ReportObservationId)).ToListAsync();
            List<ReportObservationsDocumentAC> reportObservationsDocumentList = _mapper.Map<List<ReportObservationsDocumentAC>>(reportObservationsDocuments);

            List<ReportObservationTable> reportObservationTables = await _dataRepository.Where<ReportObservationTable>(a => !a.IsDeleted
                && reportObservationIds.Contains(a.ReportObservationId)).ToListAsync();


            List<ReportObservationsMember> reportObservationMembers = await _dataRepository.Where<ReportObservationsMember>(a => !a.IsDeleted
               && reportObservationIds.Contains(a.ReportObservationId)).Include(a => a.User).ToListAsync();


            //get first person responsible for report observation
            List<ReportObservationsMember> personResponsible = reportObservationMembers.Where(a => !a.IsDeleted).ToList();
            List<ReportObservationsMemberAC> personResponsibleAC = _mapper.Map<List<ReportObservationsMemberAC>>(personResponsible);

            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = await _dataRepository.Where<ReportObservationReviewer>(a => !a.IsDeleted
            && reportObservationIds.Contains(a.ReportObservationId)).OrderByDescending(a => a.CreatedDateTime).Include(a => a.User).ToListAsync();

            List<ReportObservationReviewerAC> reviewerACList = _mapper.Map<List<ReportObservationReviewerAC>>(reportObservationReviewerList);

            //get last added comment
            List<ReportObservationReviewerAC> distinctReviewerList = reviewerACList
                          .GroupBy(p => new { p.ReportObservationId, p.UserId })
                          .Select(g => g.First()).ToList();

            //assign person responsible and reviewer to report observation
            for (int i = 0; i < reportObservationACList.Count; i++)
            {
                reportObservationACList[i].PersonResponsibleList = personResponsibleAC.Where(a => a.ReportObservationId == reportObservationACList[i].Id).ToList();
                reportObservationACList[i].ObservationReviewerList = distinctReviewerList.Where(a => a.ReportObservationId == reportObservationACList[i].Id).ToList();
                reportObservationACList[i].ReportObservationDocumentList = reportObservationsDocumentList.Where(a => a.ReportObservationId == reportObservationACList[i].Id).ToList();
                reportObservationACList[i].TableCount = reportObservationTables.Where(a => a.ReportObservationId == reportObservationACList[i].Id).Count();
                reportObservationACList[i].FileCount = reportObservationACList[i].ReportObservationDocumentList.Count();
            }
            #endregion

            reportDetailAC.TotalReportObservation = reportObservationACList.Count;
            reportDetailAC.ReportObservationList = reportObservationACList;
            reportDetailAC.RatingList = await _ratingRepository.GetRatingsByEntityIdAsync(selectedEntityId);
            reportDetailAC.ObservationTypeList = GetEnumList<ReportObservationType>();
            reportDetailAC.ObservationStatus = GetEnumList<ReportObservationStatus>();
            reportDetailAC.Disposition = GetEnumList<ReportDisposition>();

            //get linked observation list
            List<ReportObservation> linkedObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && a.ReportId.ToString() == reportId)
               .AsNoTracking().ToListAsync();
            List<ReportObservationAC> linkedObservations = _mapper.Map<List<ReportObservationAC>>(linkedObservationList);
            reportDetailAC.LinkedObservationList = linkedObservations;
            //get responsible person list
            List<EntityUserMapping> userMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted &&
                  a.EntityId.ToString() == selectedEntityId).Include(a => a.User).AsNoTracking().ToListAsync();
            userMappingList = userMappingList.Where(a => a.User.UserType == UserType.External).ToList();
            reportDetailAC.ResponsibleUserList = _mapper.Map<List<EntityUserMappingAC>>(userMappingList);

            //get list for auditor user
            List<EntityUserMappingAC> auditorACList = _mapper.Map<List<EntityUserMappingAC>>(userMappingList);
            reportDetailAC.AuditorList = auditorACList.Where(a => a.UserType != UserType.External).ToList();

            //get observation category
            List<ObservationCategory> observationCategoryList = await _dataRepository.Where<ObservationCategory>(a => !a.IsDeleted
            && a.EntityId.ToString() == selectedEntityId).AsNoTracking().ToListAsync();
            reportDetailAC.ObservationCategoryList = _mapper.Map<List<ObservationCategoryAC>>(observationCategoryList);

            //get report reviewer for observation review
            List<ReportUserMapping> reportUserMappingList = await _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted &&
             a.ReportId.ToString() == reportId && a.ReportUserType == ReportUserType.Reviewer).Include(a => a.User).AsNoTracking().ToListAsync();
            reportDetailAC.ObservationReviewerList = _mapper.Map<List<ReportUserMappingAC>>(reportUserMappingList);

            return reportDetailAC;
        }

        /// <summary>
        /// Add Observations detail
        /// </summary>
        /// <param name="reportObservations">report Observations </param>
        /// <returns>ReportObservation detail</returns>
        public async Task<ReportObservationAC> UpdateReportObservationAsync(ReportDetailAC reportDetailAC)
        {
            {

                using (var transaction = _dataRepository.BeginTransaction())
                {
                    try
                    {
                        ReportObservationAC reportObservationAC = reportDetailAC.ReportObservationList[0];
                        string reportObservationId = reportObservationAC.Id.ToString();

                        #region Update report observation detail

                        ReportObservation reportObservation = await _dataRepository.Where<ReportObservation>(a => a.Id.ToString() == reportObservationId).FirstAsync();
                        _dataRepository.DetachEntityEntry(reportObservation);
                        reportObservation = _mapper.Map<ReportObservation>(reportObservationAC);
                        reportObservation.UpdatedBy = currentUserId;
                        reportObservation.UpdatedDateTime = DateTime.UtcNow;
                        reportObservation.AuditPlan = null;
                        reportObservation.Process = null;
                        reportObservation.ObservationCategory = null;
                        _dataRepository.Update<ReportObservation>(reportObservation);
                        await _dataRepository.SaveChangesAsync();
                        #endregion

                        #region Person Responsible
                        //compare person responsible member is exist or not
                        List<ReportObservationsMember> getReportObservationMembers = await _dataRepository.Where<ReportObservationsMember>(a => !a.IsDeleted &&
                       a.ReportObservationId.ToString() == reportObservationId).AsNoTracking().ToListAsync();

                        List<ReportObservationsMember> getPersonResponsibleMembers = getReportObservationMembers.Where(a => !a.IsDeleted).ToList();

                        //get data from client side
                        List<ReportObservationsMember> personResponsibleList = _mapper.Map<List<ReportObservationsMember>>(reportObservationAC.PersonResponsibleList);

                        if (getPersonResponsibleMembers.Count > 0)
                        {
                            //delete removed person responsible
                            //get added observation members ids
                            HashSet<Guid> getObservationMemberIds = new HashSet<Guid>(getPersonResponsibleMembers.Select(s => s.UserId));

                            //get new added observation ids
                            HashSet<Guid> newObservationMemberIds = new HashSet<Guid>(personResponsibleList.Select(s => s.UserId));

                            //Get deleted observation 
                            List<ReportObservationsMember> removedObservationMemberList = getPersonResponsibleMembers.Where(m => !newObservationMemberIds.Contains(m.UserId)).ToList();

                            _dataRepository.RemoveRange<ReportObservationsMember>(removedObservationMemberList);
                            await _dataRepository.SaveChangesAsync();

                            // Add new report observation
                            List<ReportObservationsMember> newAddedObservationMembersId = personResponsibleList.Where(m => !getObservationMemberIds.Contains(m.UserId)).ToList();
                            for (int i = 0; i < newAddedObservationMembersId.Count; i++)
                            {
                                newAddedObservationMembersId[i].ReportObservationId = new Guid(reportObservationId);
                                newAddedObservationMembersId[i].CreatedBy = currentUserId;
                                newAddedObservationMembersId[i].CreatedDateTime = DateTime.UtcNow;
                                newAddedObservationMembersId[i].UpdatedBy = null;
                                newAddedObservationMembersId[i].UpdatedDateTime = null;
                            }
                            _dataRepository.AddRange<ReportObservationsMember>(newAddedObservationMembersId);
                            await _dataRepository.SaveChangesAsync();
                        }
                        else
                        {
                            // Add new report observation
                            for (int i = 0; i < personResponsibleList.Count; i++)
                            {
                                personResponsibleList[i].ReportObservationId = new Guid(reportObservationId);
                                personResponsibleList[i].CreatedBy = currentUserId;
                                personResponsibleList[i].CreatedDateTime = DateTime.UtcNow;
                                personResponsibleList[i].UpdatedBy = null;
                                personResponsibleList[i].UpdatedDateTime = null;
                            }
                            _dataRepository.AddRange<ReportObservationsMember>(personResponsibleList);
                            await _dataRepository.SaveChangesAsync();
                        }
                        #endregion

                        #region Observation reviewer
                        //get observation reviewer from database
                        List<ReportObservationReviewer> reportObservationReviewerList = await _dataRepository.Where<ReportObservationReviewer>(a => !a.IsDeleted
                        && a.ReportObservationId.ToString() == reportObservationId).OrderByDescending(a => a.CreatedDateTime).Include(a => a.User).ToListAsync();

                        //get last added comment
                        List<ReportObservationReviewer> distinctReviewerList = reportObservationReviewerList
                                      .GroupBy(p => new { p.UserId }).Select(g => g.First()).ToList();


                        //get report observation reviewer from client
                        List<ReportObservationReviewer> observationReviewerList = _mapper.Map<List<ReportObservationReviewer>>(reportObservationAC.ObservationReviewerList);

                        //get added reviewer UserIds
                        HashSet<Guid> getReviewerIds = new HashSet<Guid>(distinctReviewerList.Select(s => s.UserId));

                        if (distinctReviewerList.Count != 0)
                        {
                            List<ReportObservationReviewer> newAddedReviewerList = observationReviewerList.Where(m => !getReviewerIds.Contains(m.UserId)).ToList();
                            for (int i = 0; i < newAddedReviewerList.Count; i++)
                            {
                                newAddedReviewerList[i].Id = new Guid();
                                newAddedReviewerList[i].CreatedBy = currentUserId;
                                newAddedReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                            }

                            _dataRepository.AddRange<ReportObservationReviewer>(newAddedReviewerList);
                            await _dataRepository.SaveChangesAsync();

                            //check comment and add new comment for history

                            //get added reviewer UserIds
                            HashSet<Guid> newReviewerIds = new HashSet<Guid>(observationReviewerList.Select(s => s.UserId));

                            List<ReportObservationReviewer> sameObservationReviewerList = distinctReviewerList.Where(m => newReviewerIds.Contains(m.UserId)).ToList();
                            List<ReportObservationReviewer> newObservationReviewerList = new List<ReportObservationReviewer>();

                            for (int i = 0; i < sameObservationReviewerList.Count; i++)
                            {
                                ReportObservationReviewer reportObservationReviewer = new ReportObservationReviewer();
                                reportObservationReviewer = observationReviewerList.First<ReportObservationReviewer>
                                    (a => a.UserId == sameObservationReviewerList[i].UserId);
                                if (sameObservationReviewerList[i].Comment != reportObservationReviewer.Comment)
                                {
                                    ReportObservationReviewer newReviewerComment = new ReportObservationReviewer();
                                    newReviewerComment.ReportObservationId = new Guid(reportObservationId);
                                    newReviewerComment.Comment = reportObservationReviewer.Comment.Trim();
                                    newReviewerComment.UserId = reportObservationReviewer.UserId;
                                    newReviewerComment.CreatedBy = currentUserId;
                                    newReviewerComment.CreatedDateTime = DateTime.UtcNow;
                                    newObservationReviewerList.Add(newReviewerComment);
                                }
                            }

                            _dataRepository.AddRange<ReportObservationReviewer>(newObservationReviewerList);
                            await _dataRepository.SaveChangesAsync();
                        }
                        else
                        {
                            for (int i = 0; i < observationReviewerList.Count; i++)
                            {
                                observationReviewerList[i].Id = new Guid();
                                observationReviewerList[i].CreatedBy = currentUserId;
                                observationReviewerList[i].CreatedDateTime = DateTime.UtcNow;
                            }

                            _dataRepository.AddRange<ReportObservationReviewer>(observationReviewerList);
                            await _dataRepository.SaveChangesAsync();
                        }
                        #endregion
                        transaction.Commit();
                        return _mapper.Map<ReportObservationAC>(reportObservationAC);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Add/Update new report Observation document under a report reportObservation
        /// </summary>
        /// <param name="formdata">Details of reportObservation document/param>
        /// <returns>List of observation document details</returns>
        public async Task<List<ReportObservationsDocumentAC>> AddAndUploadReportObservationDocumentAsync(DocumentUpload upload)
        {
            using var transaction = _dataRepository.BeginTransaction();
            try
            {

                List<ReportObservationsDocument> reportObservationsDocuments = new List<ReportObservationsDocument>();
                List<IFormFile> files = upload.Files == null ? new List<IFormFile>() : upload.Files;

                //validation 
                int fileCount = _dataRepository.Where<ReportObservationsDocument>(a => !a.IsDeleted && a.ReportObservationId == upload.ReportObservationId).Count();
                int totalFiles = fileCount + files.Count;

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

                for (int i = 0; i < files.Count; i++)
                {
                    ReportObservationsDocument reportObservationsDocument = new ReportObservationsDocument();
                    reportObservationsDocument.ReportObservationId = upload.ReportObservationId;
                    reportObservationsDocument.DocumentName = upload.Files[i].FileName;
                    reportObservationsDocument.DocumentPath = await _fileUtility.UploadFileAsync(upload.Files[i]);
                    reportObservationsDocument.CreatedBy = currentUserId;
                    reportObservationsDocument.CreatedDateTime = DateTime.UtcNow;
                    reportObservationsDocuments.Add(reportObservationsDocument);
                }
                await _dataRepository.AddRangeAsync(reportObservationsDocuments);
                await _dataRepository.SaveChangesAsync();

                transaction.Commit();
                reportObservationsDocuments = _dataRepository.Where<ReportObservationsDocument>(a => !a.IsDeleted && a.ReportObservationId == upload.ReportObservationId).AsNoTracking().ToList();
                return _mapper.Map<List<ReportObservationsDocumentAC>>(reportObservationsDocuments);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Delete Reviewer document from report Observation document and azure storage
        /// </summary>
        /// <param name="reportObservationDocumentId">Id of the Reviewer document</param>
        /// <returns>Task</returns>
        public async Task DeleteReportObservationDocumentAync(Guid reportObservationDocumentId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    ReportObservationsDocument reportObservationDocument = await _dataRepository.FirstAsync<ReportObservationsDocument>(x => x.Id == reportObservationDocumentId);
                    if (await _fileUtility.DeleteFileAsync(reportObservationDocument.DocumentPath))//Delete file from azure
                    {
                        _dataRepository.Remove<ReportObservationsDocument>(reportObservationDocument);
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
        /// Get file url and download report Observation document 
        /// </summary>
        /// <param name="reportObservationDocumentId">reportObservation document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        public async Task<string> DownloadReportObservationDocumentAsync(Guid reportObservationDocumentId)
        {
            ReportObservationsDocument reportObservationDocument = await _dataRepository.FirstAsync<ReportObservationsDocument>(x => x.Id == reportObservationDocumentId && !x.IsDeleted);
            return _fileUtility.DownloadFile(reportObservationDocument.DocumentPath);
        }
        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get report observation json document which represents the dynamic table of observation
        /// </summary>
        /// <param name="id">report Observation id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        public async Task<string> GetReportObservationTableAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var reportObservationTable = await _dataRepository.FirstOrDefaultAsync<ReportObservationTable>(x => x.ReportObservationId.ToString() == id && !x.IsDeleted);
                    // Create json document table
                    if (reportObservationTable == null)
                    {
                        var reportObservationTableToAdd = new ReportObservationTable();

                        var parsedDocument = _dynamicTableRepository.AddDefaultJsonDocument();
                        reportObservationTableToAdd.Table = parsedDocument;
                        reportObservationTableToAdd.ReportObservationId = Guid.Parse(id);
                        reportObservationTableToAdd.IsDeleted = false;
                        reportObservationTableToAdd.CreatedBy = currentUserId;
                        reportObservationTableToAdd.CreatedDateTime = DateTime.UtcNow;
                        await _dataRepository.AddAsync(reportObservationTableToAdd);
                        await _dataRepository.SaveChangesAsync();
                        transaction.Commit();
                        return JsonSerializer.Serialize(reportObservationTableToAdd.Table);
                    }
                    var jsonDocString = JsonSerializer.Serialize(reportObservationTable.Table);
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
        /// <param name="reportObservationId">report Observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        public async Task<string> UpdateJsonDocumentAsync(string jsonDocument, string reportObservationId, string tableId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var reportObservationTables = await _dataRepository.Where<ReportObservationTable>(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).ToListAsync();
                    var reportObservationTable = reportObservationTables.FirstOrDefault<ReportObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

                    reportObservationTable.Table = JsonDocument.Parse(jsonDocument);
                    reportObservationTable.UpdatedBy = currentUserId;
                    reportObservationTable.UpdatedDateTime = DateTime.UtcNow;

                    _dataRepository.Update<ReportObservationTable>(reportObservationTable);
                    await _dataRepository.SaveChangesAsync();
                    transaction.Commit();
                    var jsonObj = JsonSerializer.Serialize(reportObservationTable.Table);
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
        /// Add column in report observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reportObservationId">report Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddColumnAsync(string tableId, string reportObservationId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var observationTables = await _dataRepository.Where<ReportObservationTable>(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ReportObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

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
        /// Add row in report observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reportObservationId">report Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> AddRowAsync(string tableId, string reportObservationId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    var observationTables = await _dataRepository.Where<ReportObservationTable>(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ReportObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

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
        /// Delete row in report observationTable's table
        /// </summary>
        /// <param name="reportObservationId">report Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteRowAsync(string reportObservationId, string tableId, string rowId)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ReportObservationTable>(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ReportObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

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
        /// Delete column in report observationTable's table
        /// </summary>
        /// <param name="reportObservationId">Report Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<string> DeleteColumnAsync(string reportObservationId, string tableId, int columnPosition)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    var observationTables = await _dataRepository.Where<ReportObservationTable>(x => x.ReportObservationId.ToString() == reportObservationId
                    && !x.IsDeleted).ToListAsync();
                    var observationTable = observationTables.FirstOrDefault<ReportObservationTable>(x => x.Table.RootElement.GetString("tableId") == tableId);

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

        #region Comment History
        /// <summary>
        /// Get Reports comment history
        /// </summary>
        /// <param name="reportId">Selected Report Id</param>
        /// <param name="timeOffset">Requested user system timezone</param
        /// <returns>List of Report comments</returns>
        public async Task<ReportCommentHistoryAC> GetCommentHistoryAsync(string reportId, int timeOffset)
        {
            ReportCommentHistoryAC reportCommentHistory = new ReportCommentHistoryAC();
            //get report Comments
            List<ReportComment> reportComments = await _dataRepository.Where<ReportComment>(a => !a.IsDeleted && a.ReportId.ToString() == reportId)
              .Include(a => a.UserCreatedBy).OrderByDescending(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            reportCommentHistory.CommentList = _mapper.Map<List<ReportCommentAC>>(reportComments);
            reportCommentHistory.ReportTitle = _dataRepository.FirstOrDefault<Report>(a => a.Id.ToString() == reportId && !a.IsDeleted).ReportTitle;
            for (int i = 0; i < reportCommentHistory.CommentList.Count; i++)
            {

                DateTime createdDateTime = reportCommentHistory.CommentList[i].CreatedDateTime ?? DateTime.UtcNow;
                reportCommentHistory.CommentList[i].CreatedDateTime = createdDateTime.AddMinutes(-1 * timeOffset);
                DateTime updatedDateTime = reportCommentHistory.CommentList[i].UpdatedDateTime ?? DateTime.UtcNow;
                reportCommentHistory.CommentList[i].UpdatedDateTime = updatedDateTime.AddMinutes(-1 * timeOffset);
            }
            //report observation comments
            List<ReportObservation> reportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
                 a.ReportId.ToString() == reportId).OrderBy(a => a.CreatedDateTime).AsNoTracking().ToListAsync();
            List<ReportObservationAC> reportObservationACList = _mapper.Map<List<ReportObservationAC>>(reportObservationList);

            HashSet<Guid> reportObservationIds = new HashSet<Guid>(reportObservationList.Select(s => s.Id));
            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = await _dataRepository.Where<ReportObservationReviewer>(a => !a.IsDeleted
            && reportObservationIds.Contains(a.ReportObservationId)).OrderByDescending(a => a.CreatedDateTime).Include(a => a.UserCreatedBy).ToListAsync();

            List<ReportObservationReviewerAC> reviewerACList = _mapper.Map<List<ReportObservationReviewerAC>>(reportObservationReviewerList);

            for (int i = 0; i < reviewerACList.Count; i++)
            {
                DateTime createdDateTime = reviewerACList[i].CreatedDateTime ?? DateTime.UtcNow;
                reviewerACList[i].CreatedDateTime = createdDateTime.AddMinutes(-1 * timeOffset);
                DateTime updatedDateTime = reviewerACList[i].UpdatedDateTime ?? DateTime.UtcNow;
                reviewerACList[i].UpdatedDateTime = updatedDateTime.AddMinutes(-1 * timeOffset);
            }
            //assign person responsible and reviewer to report observation
            for (int i = 0; i < reportObservationACList.Count; i++)
            {
                reportObservationACList[i].ObservationReviewerList = reviewerACList.Where(a => a.ReportObservationId == reportObservationACList[i].Id).ToList();
                reportObservationACList[i].ObservationReviewerList.ForEach(a => { a.ReportObservationTitle = reportObservationACList[i].Heading; });
            }

            reportCommentHistory.ReportObservationComment = reportObservationACList;
            for (int i = 0; i < reportCommentHistory.ReportObservationComment.Count; i++)
            {

                DateTime createdDateTime = reportCommentHistory.ReportObservationComment[i].CreatedDateTime ?? DateTime.UtcNow;
                reportCommentHistory.ReportObservationComment[i].CreatedDateTime = createdDateTime.AddMinutes(-1 * timeOffset);
                DateTime updatedDateTime = reportCommentHistory.ReportObservationComment[i].UpdatedDateTime ?? DateTime.UtcNow;
                reportCommentHistory.ReportObservationComment[i].UpdatedDateTime = updatedDateTime.AddMinutes(-1 * timeOffset);
            }
            return reportCommentHistory;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Check report alrdy exist or notea
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

        /// <summary>
        /// Add observation table and document detail
        /// </summary>
        /// <param name="newObservationIds">list of new added observation</param>
        /// <param name="newAddedReportObservationList">list of new added observation</param>
        /// <param name="createdUserId">current loggedin user</param>
        /// <returns>Task</returns>
        private async Task AddTableAndDocumentFromObservation(HashSet<Guid> newObservationIds, List<ReportObservation> newAddedReportObservationList, Guid createdUserId)
        {
            //remove files from azure for preview ppt
            await _fileUtility.DeletFilesAsync();

            // add report observation table detail get observation table detail

            List<ObservationTable> observationTableList = _dataRepository.Where<ObservationTable>(a => !a.IsDeleted
           && newObservationIds.Contains(a.ObservationId)).AsNoTracking().ToList();

            List<ReportObservationTable> addedReportObservationTableList = new List<ReportObservationTable>();

            for (int i = 0; i < newAddedReportObservationList.Count; i++)
            {
                ObservationTable observationTable = observationTableList.FirstOrDefault(a => a.ObservationId ==
               newAddedReportObservationList[i].ObservationId);
                if (observationTable != null)
                {
                    ReportObservationTable reportObservationTable = new ReportObservationTable();
                    reportObservationTable.ReportObservationId = newAddedReportObservationList[i].Id;
                    reportObservationTable.Table = observationTable.Table;
                    reportObservationTable.CreatedBy = createdUserId;
                    reportObservationTable.CreatedDateTime = DateTime.Now;
                    addedReportObservationTableList.Add(reportObservationTable);
                }
            }
            await _dataRepository.AddRangeAsync<ReportObservationTable>(addedReportObservationTableList);
            await _dataRepository.SaveChangesAsync();

            //add report observation document detail
            //get observation document detail
            List<ObservationDocument> observationDocuments = _dataRepository.Where<ObservationDocument>(a => !a.IsDeleted
            && newObservationIds.Contains(a.ObservationId)).AsNoTracking().ToList();

            List<ReportObservationsDocument> addReportObservationsDocumentList = new List<ReportObservationsDocument>();


            for (int i = 0; i < newAddedReportObservationList.Count; i++)
            {
                List<ObservationDocument> observationDocumentList = observationDocuments.Where(a => a.ObservationId ==
                newAddedReportObservationList[i].ObservationId).ToList();
                for (int j = 0; j < observationDocumentList.Count; j++)
                {
                    if (observationDocumentList[j] != null)
                    {
                        ReportObservationsDocument reportObservationsDocument = new ReportObservationsDocument();
                        reportObservationsDocument.ReportObservationId = newAddedReportObservationList[i].Id;
                        reportObservationsDocument.DocumentPath = observationDocumentList[j].DocumentPath;
                        reportObservationsDocument.CreatedBy = createdUserId;
                        reportObservationsDocument.CreatedDateTime = DateTime.Now;
                        addReportObservationsDocumentList.Add(reportObservationsDocument);
                    }
                }
            }
            await _dataRepository.AddRangeAsync<ReportObservationsDocument>(addReportObservationsDocumentList);
            await _dataRepository.SaveChangesAsync();
        }
        #endregion

        #region Generate Report PPT
        /// <summary>
        /// Generate report PPT
        /// </summary>
        /// <param name="reportId">Id of Report</param>
        /// <param name="entityId">Selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> GenerateReportPPTAsync(string reportId, string entityId, int timeOffset)
        {
            try
            {
                string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
                Report report = _dataRepository.FirstOrDefault<Report>(a => !a.IsDeleted && a.Id == new Guid(reportId));
                report.ReportTitle = _globalRepository.SetSpecialCharecters(report.ReportTitle);

                List<ReportObservation> reportObservations = _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && a.ReportId == new Guid(reportId))
                                    .Include(x => x.Process).Include(x => x.Rating)
                                    .Include(a => a.Report).Include(a => a.ObservationCategory).Include(a => a.User)
                                    .OrderBy(a => a.SortOrder).AsNoTracking().ToList();

                List<ReportObservationAC> reportObservationList = _mapper.Map<List<ReportObservationAC>>(reportObservations);
                reportObservationList = await SetReportObservatonDetailsAsync(reportObservationList, timeOffset);

                Tuple<string, string, List<PlanProcessMappingAC>> pptData = await GetProcessSubprocessData(entityId);
                List<ReportObservationPPT> reportObservationTable = await GetReportObservations(pptData.Item3, reportObservationList);
                List<MemberTablePPT> distributors = await GetReportDistributors(reportId);
                List<MemberTablePPT> teamMembers = await GetTeamMembers(entityId);

                PowerPointTemplate templateData = await CreatePPTTemplateData(entityId, report, timeOffset, pptData.Item1, pptData.Item2, reportObservationList);

                dynamic dynamicDictionary = new DynamicDictionary<int, dynamic>();
                dynamicDictionary.Add(5, reportObservationTable);
                dynamicDictionary.Add(7, distributors);
                dynamicDictionary.Add(8, teamMembers);

                string currentPath = Path.Combine(_environment.ContentRootPath, StringConstant.WWWRootFolder, StringConstant.TemplateFolder);
                string templateFileName = StringConstant.AuditReportTemplate + StringConstant.PPTFileExtantion;
                string templateFilePath = Path.Combine(currentPath, templateFileName);

                string repeatedSlideFileName = StringConstant.ObservationsTemplate + StringConstant.PPTFileExtantion;
                string repeatedTemplatePath = Path.Combine(currentPath, repeatedSlideFileName);

                string lastSlidePPTFile = StringConstant.LastSlideTemplate + StringConstant.PPTFileExtantion;
                string lastPPTemplatePath = currentPath + "\\";
                List<PowerPointTemplate> observationTemplateData = await SetRepeatedObservationTemplateAsync(reportObservationList);
                Tuple<string, MemoryStream> fileData = await _generatePPTRepository.CreatePPTFileAsync(entityName, templateFilePath, templateData, dynamicDictionary, repeatedTemplatePath, observationTemplateData, lastPPTemplatePath, lastSlidePPTFile);
                return fileData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create PPT template data
        /// </summary>
        /// <param name="entityId">Selected entity id</param>
        /// <param name="report">Report detail</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <param name="processName">process name</param>
        /// <param name="subProcessName">subprocess name</param>
        /// <returns>Return Power point template</returns>
        private async Task<PowerPointTemplate> CreatePPTTemplateData(string entityId, Report report, int timeOffset, string processName, string subProcessName, List<ReportObservationAC> observationData)
        {
            string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);

            string reportGeneratedDate = report.CreatedDateTime.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);
            string reportTitle = report.ReportTitle;
            //set audit period
            string auditPeriod = report.AuditPeriodStartDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) + StringConstant.toText + report.AuditPeriodEndDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);

            var pptTemplate = new PowerPointTemplate();
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph1#]", Text = entityName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph2#]", Text = reportTitle });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph3#]", Text = reportGeneratedDate });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph4#]", Text = auditPeriod });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List1(string[])#]", Text = processName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List2(string[])#]", Text = subProcessName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List3(string[])#]", Text = "" });

            return pptTemplate;
        }

        /// <summary>
        /// Get process subprocess Data
        /// </summary>
        /// <param name="entityId">Entity Id</param>
        /// <returns>Tupple with process, subprocess data, subprocess list</returns>
        private async Task<Tuple<string, string, List<PlanProcessMappingAC>>> GetProcessSubprocessData(string entityId)
        {
            //get process, subprocess list
            List<AuditPlanAC> auditPlanList = await _auditPlanRepository.GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(new Guid(entityId));
            string processNames = "";
            string subProcessNames = "";
            List<ProcessAC> processes = new List<ProcessAC>();
            List<PlanProcessMappingAC> subProcesses = new List<PlanProcessMappingAC>();

            for (int i = 0; i < auditPlanList.Count; i++)
            {
                for (int j = 0; j < auditPlanList[i].ParentProcessList.Count; j++)
                {
                    processes.Add(auditPlanList[i].ParentProcessList[j]);
                    processNames = processNames + auditPlanList[i].ParentProcessList[j].Name + " \n ";
                }
                for (int j = 0; j < auditPlanList[i].PlanProcessList.Count; j++)
                {
                    if (auditPlanList[i].PlanProcessList[j].ParentProcessId != null)
                    {
                        if (processes.Count != 0)
                        {
                            auditPlanList[i].PlanProcessList[j].ParentProcessName = processes.FirstOrDefault(a => a.Id == auditPlanList[i].PlanProcessList[j].ParentProcessId).Name;
                        }
                        else
                        {
                            auditPlanList[i].PlanProcessList[j].ParentProcessName = string.Empty;
                        }
                        subProcesses.Add(auditPlanList[i].PlanProcessList[j]);
                        subProcessNames = subProcessNames + auditPlanList[i].PlanProcessList[j].ProcessName + " \n ";
                    }
                }
            }
            Tuple<string, string, List<PlanProcessMappingAC>> processSubprocessData = new Tuple<string, string, List<PlanProcessMappingAC>>(processNames, subProcessNames, subProcesses);
            return processSubprocessData;
        }

        /// <summary>
        /// Get Report observations data 
        /// </summary>
        /// <param name="subProcesses">Sub process list</param>
        /// <param name="reportObservationList">report bbservation list</param>
        /// <returns>List of observations data</returns>
        private async Task<List<ReportObservationPPT>> GetReportObservations(List<PlanProcessMappingAC> subProcesses, List<ReportObservationAC> reportObservationList)
        {
            List<ReportObservationPPT> reportObservationsTable = new List<ReportObservationPPT>();

            for (int i = 0; i < reportObservationList.Count; i++)
            {
                ReportObservationPPT reportPPTData = new ReportObservationPPT();
                reportPPTData.SrNo = (i + 1).ToString();
                if (subProcesses.FirstOrDefault(a => a.ProcessId == reportObservationList[i].ProcessId) != null)
                {
                    reportPPTData.Process = subProcesses.FirstOrDefault(a => a.ProcessId == reportObservationList[i].ProcessId).ParentProcessName;
                }
                else
                {
                    reportPPTData.Process = string.Empty;
                }
                reportPPTData.SubProcess = reportObservationList[i].ProcessName;
                reportPPTData.ObservationHeading = reportObservationList[i].Heading;
                reportPPTData.Ratings = reportObservationList[i].Rating;
                reportObservationsTable.Add(reportPPTData);
            }
            return reportObservationsTable;
        }

        /// <summary>
        /// Get distributors data
        /// </summary>
        /// <param name="reportId">Report id</param>
        /// <returns>List of report distributors</returns>
        private async Task<List<MemberTablePPT>> GetReportDistributors(string reportId)
        {
            List<ReportUserMapping> distributorsList = _dataRepository.Where<ReportUserMapping>(a => !a.IsDeleted
            && a.ReportId == new Guid(reportId) && a.ReportUserType == ReportUserType.Distributor)
                .Include(x => x.User).AsNoTracking().ToList();
            List<ReportUserMappingAC> distributors = _mapper.Map<List<ReportUserMappingAC>>(distributorsList);

            //stack holders
            List<MemberTablePPT> distributorTable = new List<MemberTablePPT>();
            for (int i = 0; i < distributors.Count; i++)
            {
                MemberTablePPT distributor = new MemberTablePPT();
                distributor.SrNo = (i + 1).ToString();
                distributor.Name = distributors[i].Name + "|" + distributors[i].Designation;
                distributorTable.Add(distributor);
            }

            return distributorTable;
        }

        /// <summary>
        /// Get audit team user data
        /// </summary>
        /// <param name="entityId">entity id</param>
        /// <returns>List of audit team members</returns>
        private async Task<List<MemberTablePPT>> GetTeamMembers(string entityId)
        {
            List<EntityUserMapping> entityUserMappingList = await _dataRepository.Where<EntityUserMapping>(a => !a.IsDeleted &&
                        a.EntityId.ToString() == entityId).Include(a => a.User).AsNoTracking().ToListAsync();
            List<EntityUserMapping> teamMembers = new List<EntityUserMapping>();
            for (int i = 0; i < entityUserMappingList.Count; i++)
            {
                if (entityUserMappingList[i].User.UserType == UserType.Internal)
                {
                    teamMembers.Add(entityUserMappingList[i]);
                }
            }

            List<EntityUserMappingAC> entityUserMappings = _mapper.Map<List<EntityUserMappingAC>>(teamMembers);

            //stack holders
            List<MemberTablePPT> teamMemberTable = new List<MemberTablePPT>();
            for (int i = 0; i < entityUserMappings.Count; i++)
            {
                MemberTablePPT teamMember = new MemberTablePPT();
                teamMember.SrNo = (i + 1).ToString();
                teamMember.Name = entityUserMappings[i].Name;
                teamMember.Designation = entityUserMappings[i].Designation;
                teamMemberTable.Add(teamMember);
            }

            return teamMemberTable;
        }

        /// <summary>
        /// Set report observation data
        /// </summary>
        /// <param name="observationData">List of Report Obseravation data</param>
        /// <param name="timeOffset">requested system time offset</param>
        /// <returns>List of Report Observation detail</returns>
        private async Task<List<ReportObservationAC>> SetReportObservatonDetailsAsync(List<ReportObservationAC> observationData, int timeOffset)
        {
            List<ReportObservationAC> linkedObservationList = observationData;
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            HashSet<Guid> reportObservationIds = new HashSet<Guid>(observationData.Select(s => s.Id ?? new Guid()));
            //get first person responsible for report observation
            List<ReportObservationsMember> reportObservationMembers = await _dataRepository.Where<ReportObservationsMember>(a => !a.IsDeleted
             && reportObservationIds.Contains(a.ReportObservationId)).Include(a => a.User).ToListAsync();

            List<ReportObservationsMemberAC> personResponsibleAC = _mapper.Map<List<ReportObservationsMemberAC>>(reportObservationMembers);

            //convert UTC date time to system date time format
            observationData.ForEach(a =>
            {
                a.PersonResponsibleList = personResponsibleAC.Where(x => x.ReportObservationId == a.Id).ToList();
                // convert html contant to text
                a.Rating = string.IsNullOrEmpty(a.Rating) ? string.Empty : a.Rating;
                a.Heading = _globalRepository.SetSpecialCharecters(a.Heading);
                a.Background = _globalRepository.SetSpecialCharecters(a.Background);
                a.RootCause = _globalRepository.SetSpecialCharecters(a.RootCause);
                a.Implication = _globalRepository.SetSpecialCharecters(a.Implication);
                a.Recommendation = _globalRepository.SetSpecialCharecters(a.Recommendation);
                a.Observations = _globalRepository.SetSpecialCharecters(a.Observations);
                a.ManagementResponse = _globalRepository.SetSpecialCharecters(a.ManagementResponse);
                a.Conclusion = _globalRepository.SetSpecialCharecters(a.Conclusion);
                a.Comment = _globalRepository.SetSpecialCharecters(a.Comment);
                a.LinkedObservation = a.LinkedObservation != null && a.LinkedObservation != string.Empty ? linkedObservationList.FirstOrDefault(x => x.Id == new Guid(a.LinkedObservation))?.Heading : string.Empty;
                a.LinkedObservation = _globalRepository.SetSpecialCharecters(a.LinkedObservation);
                a.ObservationCategory = (a.ObservationCategoryId != null && a.ObservationCategoryId.ToString() != "") ? a.ObservationCategory : string.Empty;
                a.IsRepeated = a.Id != null ? (a.IsRepeatObservation ? StringConstant.Yes : StringConstant.No) : string.Empty;
                a.ObservationTypeName = a.Id != null ? a.ObservationType.ToString() : string.Empty;
                a.DispositionName = a.Id != null ? a.Disposition.ToString() : string.Empty;
                a.Status = a.Id != null ? a.ObservationStatus.ToString() : string.Empty;
                a.ObservationTargetDate = a.Id != null ? a.TargetDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
                a.CreatedDate = (a.Id != null && a.CreatedDateTime != null) ? a.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.UpdatedDate = (a.Id != null && a.UpdatedDateTime != null) ? a.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                a.CommentCreatedDate = (a.Id != null && a.CommentCreatedDateTime != null) ? a.CommentCreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            });

            return observationData;
        }

        /// <summary>
        /// Set Repeatad report observationvaion template data
        /// </summary>
        /// <param name="observationData">List of report observation</param>
        /// <returns>List of powerpoint template</returns>
        private async Task<List<PowerPointTemplate>> SetRepeatedObservationTemplateAsync(List<ReportObservationAC> observationData)
        {

            List<PowerPointTemplate> reportObservationTemplate = new List<PowerPointTemplate>();
            //create ppt with dynamic data slide
            for (int i = 0; i < observationData.Count; i++)
            {

                PowerPointTemplate pptTemplate = new PowerPointTemplate();
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph1#]", Text = observationData[i].Heading });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph2#]", Text = observationData[i].Background });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph3#]", Text = observationData[i].Observations });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph4#]", Text = observationData[i].ObservationTypeName });
                string observationCategory = observationData[i].ObservationCategory != null && observationData[i].ObservationCategory != "" ? observationData[i].ObservationCategory : "";
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph5#]", Text = observationCategory });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph6#]", Text = observationData[i].Rating });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph7#]", Text = observationData[i].IsRepeated });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph8#]", Text = observationData[i].RootCause });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph9#]", Text = observationData[i].Implication });
                string linkedObservation = observationData[i].LinkedObservation != null ? observationData[i].LinkedObservation : "";
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph10#]", Text = linkedObservation });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph11#]", Text = observationData[i].Status });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph12#]", Text = observationData[i].DispositionName });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph13#]", Text = observationData[i].Recommendation });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph14#]", Text = observationData[i].ManagementResponse });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph15#]", Text = observationData[i].Conclusion });
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph16#]", Text = observationData[i].ObservationTargetDate });

                string responsiblePersons = string.Empty;
                for (int j = 0; j < observationData[i].PersonResponsibleList.Count; j++)
                {
                    responsiblePersons = responsiblePersons + observationData[i].PersonResponsibleList[j].Name + " \n ";
                }

                pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List1(string[])#]", Text = responsiblePersons });
                reportObservationTemplate.Add(pptTemplate);

            }
            return reportObservationTemplate;
        }

        /// <summary>
        /// Get file url for report document 
        /// </summary>
        /// <param name="reportId">selected report id</param>
        /// <param name="entityId">selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Url of File of particular file name passed</returns>
        public async Task<string> AddViewDocumentAsync(string reportId, string entityId, int timeOffSet)
        {
            try
            {
                await _fileUtility.DeletFilesAsync();
                Tuple<string, MemoryStream> fileData = await GenerateReportPPTAsync(reportId, entityId, timeOffSet);
                MemoryStream memoryStream = fileData.Item2;
                string fileName = fileData.Item1;

                //convert memory stream to filestream result
                FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, StringConstant.PPTContentType);

                fileStreamResult.FileStream.CopyTo(memoryStream);
                IFormFile file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName);

                //upload file to azure in specific folder
                string documentPath = await _fileUtility.UploadFilesInDirectoryAsync(file);
                documentPath = StringConstant.ReportPPTFolder + "/" + documentPath;
                documentPath = _fileUtility.DownloadFile(documentPath);
                return documentPath;
            }
            catch (Exception)
            {
                throw;
            }

        }


        #endregion

        #region Generate Observation PPT
        /// <summary>
        /// Generate report observation PPT
        /// </summary>
        /// <param name="reportObservationId">Report observation Id </param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        public async Task<Tuple<string, MemoryStream>> GenerateReportObservationPPTAsync(string reportObservationId, string entityId, int timeOffset)
        {
            try
            {
                string entityName = await _auditableEntityRepository.GetEntityNameById(entityId);
                ReportObservation reportObservation = _dataRepository.Where<ReportObservation>(a => !a.IsDeleted && a.Id == new Guid(reportObservationId))
                    .Include(a => a.Report).Include(a => a.Process).Include(a => a.ObservationCategory)
                  .Include(a => a.User).Include(a => a.Rating)
                    .AsNoTracking().ToList().First();

                ReportObservationAC reportObservationData = await SetReportObservatonDataAsync(reportObservation, entityId, timeOffset);

                //get add table data
                ReportObservationTable observationTable = await _dataRepository.FirstOrDefaultAsync<ReportObservationTable>(x => x.ReportObservationId == new Guid(reportObservationId)
                          && !x.IsDeleted);
                JSONRoot tableData = null;
                if (observationTable != null)
                {
                    tableData = await GetJsonReportObservationTableAsync(observationTable);
                }
                reportObservationData = await GetCommentHistoryPPTAsync(reportObservationData, timeOffset);

                //get observation document data
                List<ReportObservationsDocumentAC> reportObservationDocumentACList = await GetReportObservationDocumentAsync(reportObservation.Id.ToString());
                PowerPointTemplate templateData = await CreateReportObservationPPTTemplateAsync(reportObservationData, timeOffset, reportObservationDocumentACList);
                string currentPath = Path.Combine(_environment.ContentRootPath, StringConstant.WWWRootFolder, StringConstant.TemplateFolder);
                string templateFileName = StringConstant.ReportObservationTemplate + StringConstant.PPTFileExtantion;
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
        /// Set selected report observation data
        /// </summary>
        /// <param name="observationData">Report Obseravation data</param>
        /// <param name="timeOffset">requested system time offset</param>
        /// <returns>Report Observation detail</returns>
        private async Task<ReportObservationAC> SetReportObservatonDataAsync(ReportObservation observationData, string entityId, int timeOffset)
        {
            List<ReportObservation> reportObservationList = await _dataRepository.Where<ReportObservation>(a => !a.IsDeleted &&
                a.ReportId == observationData.ReportId)
          .Include(a => a.Report).Include(a => a.Process).Include(a => a.ObservationCategory)
          .Include(a => a.User).Include(a => a.Rating)
              .OrderBy(a => a.CreatedDateTime).AsNoTracking().ToListAsync();

            List<ReportObservationAC> reportObservationACList = _mapper.Map<List<ReportObservationAC>>(reportObservationList);
            ReportObservationAC reportObservation = _mapper.Map<ReportObservationAC>(observationData);
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            reportObservation.Heading = _globalRepository.SetSpecialCharecters(reportObservation.Heading);
            reportObservation.Background = _globalRepository.SetSpecialCharecters(reportObservation.Background);
            reportObservation.RootCause = _globalRepository.SetSpecialCharecters(reportObservation.RootCause);
            reportObservation.Implication = _globalRepository.SetSpecialCharecters(reportObservation.Implication);
            reportObservation.Recommendation = _globalRepository.SetSpecialCharecters(reportObservation.Recommendation);
            reportObservation.Observations = _globalRepository.SetSpecialCharecters(reportObservation.Observations);
            reportObservation.ManagementResponse = _globalRepository.SetSpecialCharecters(reportObservation.ManagementResponse);
            reportObservation.Conclusion = _globalRepository.SetSpecialCharecters(reportObservation.Conclusion);
            reportObservation.Comment = _globalRepository.SetSpecialCharecters(reportObservation.Comment);
            reportObservation.LinkedObservation = reportObservation.LinkedObservation != null && reportObservation.LinkedObservation != string.Empty ? reportObservationACList.FirstOrDefault(x => x.Id == new Guid(reportObservation.LinkedObservation))?.Heading : string.Empty;
            reportObservation.LinkedObservation = _globalRepository.SetSpecialCharecters(reportObservation.LinkedObservation);
            reportObservation.IsRepeated = reportObservation.Id != null ? (reportObservation.IsRepeatObservation ? StringConstant.Yes : StringConstant.No) : string.Empty;
            reportObservation.ObservationTypeName = reportObservation.Id != null ? reportObservation.ObservationType.ToString() : string.Empty;
            reportObservation.DispositionName = reportObservation.Id != null ? reportObservation.Disposition.ToString() : string.Empty;
            reportObservation.Status = reportObservation.Id != null ? reportObservation.ObservationStatus.ToString() : string.Empty;
            reportObservation.ObservationTargetDate = reportObservation.Id != null ? reportObservation.TargetDate.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime) : string.Empty;
            reportObservation.CreatedDate = (reportObservation.Id != null && reportObservation.CreatedDateTime != null) ? reportObservation.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            reportObservation.UpdatedDate = (reportObservation.Id != null && reportObservation.UpdatedDateTime != null) ? reportObservation.UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            reportObservation.CommentCreatedDate = (reportObservation.Id != null && reportObservation.CommentCreatedDateTime != null) ? reportObservation.CommentCreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
            return reportObservation;
        }

        /// <summary>
        /// Get report observavation document detail
        /// </summary>
        /// <param name="reportObservationId">Selected report observation id</param>
        /// <returns>List of report observation document</returns>
        private async Task<List<ReportObservationsDocumentAC>> GetReportObservationDocumentAsync(string reportObservationId)
        {
            List<ReportObservationsDocument> observationDocuments = _dataRepository.Where<ReportObservationsDocument>(a => !a.IsDeleted && a.ReportObservationId == Guid.Parse(reportObservationId)).AsNoTracking().ToList();
            List<ReportObservationsDocumentAC> observationDocumentList = _mapper.Map<List<ReportObservationsDocumentAC>>(observationDocuments);
            List<ReportObservationsDocumentAC> imageDocument = new List<ReportObservationsDocumentAC>();

            for (int i = 0; i < observationDocumentList.Count; i++)
            {
                observationDocumentList[i].DocumentName = observationDocumentList[i].DocumentPath;
                observationDocumentList[i].DocumentPath = _azureRepo.DownloadFile(observationDocumentList[i].DocumentPath);
                string fileExtention = observationDocumentList[i].DocumentName.Split(".").Last();
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
        /// Download report observation image files
        /// </summary>
        /// <param name="observationDocuments">Report Observation document list</param>
        /// <returns>Task</returns>
        private async Task DownloadImagesAsync(List<ReportObservationsDocumentAC> observationDocuments)
        {
            string currentPath = Path.GetTempPath() + StringConstant.ImageFolder;
            // Determine whether the directory exists.
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
            for (int i = 0; i < observationDocuments.Count; i++)
            {

                string saveLocation = Path.Combine(currentPath, observationDocuments[i].DocumentName);

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
        /// Get Reports observation comment history
        /// </summary>
        /// <param name="reportObservationAC">Report observation</param>
        /// <param name="timeOffset">Requested user system timezone</param
        /// <returns>Report observaion</returns>
        private async Task<ReportObservationAC> GetCommentHistoryPPTAsync(ReportObservationAC reportObservationAC, int timeOffset)
        {
            //get reviewer for report observation
            List<ReportObservationReviewer> reportObservationReviewerList = await _dataRepository.Where<ReportObservationReviewer>(a => !a.IsDeleted
            && a.ReportObservationId == reportObservationAC.Id).OrderByDescending(a => a.CreatedDateTime).Include(a => a.UserCreatedBy).ToListAsync();

            List<ReportObservationReviewerAC> reviewerACList = _mapper.Map<List<ReportObservationReviewerAC>>(reportObservationReviewerList);

            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            for (int i = 0; i < reviewerACList.Count; i++)
            {
                reviewerACList[i].CreatedDate = (reviewerACList[i].Id != null && reviewerACList[i].CreatedDateTime != null) ? reviewerACList[i].CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                reviewerACList[i].UpdatedDate = (reviewerACList[i].Id != null && reviewerACList[i].UpdatedDateTime != null) ? reviewerACList[i].UpdatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormat) : string.Empty;
                reviewerACList[i].Comment = string.IsNullOrEmpty(reviewerACList[i].Comment) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(reviewerACList[i].Comment, string.Empty));
            }

            reportObservationAC.ObservationReviewerList = reviewerACList;
            return reportObservationAC;
        }

        /// <summary>
        /// Create PPT template data
        /// </summary>
        /// <param name="reportObservation">Report Observation detail</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <param name="documentList">observation document list</param>
        /// <returns>Power point template</returns>
        private async Task<PowerPointTemplate> CreateReportObservationPPTTemplateAsync(ReportObservationAC reportObservation, int timeOffset, List<ReportObservationsDocumentAC> documentList)
        {
            string observationGeneratedDate = reportObservation.CreatedDateTime.Value.AddMinutes(-1 * timeOffset).ToString(StringConstant.DateTimeFormatWithoutTime);

            var pptTemplate = new PowerPointTemplate();
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph1#]", Text = reportObservation.Heading });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph2#]", Text = observationGeneratedDate });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph3#]", Text = reportObservation.Background });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph4#]", Text = reportObservation.Observations });
            string rating = reportObservation.Rating != null ? reportObservation.Rating : string.Empty;
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph5#]", Text = rating });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph6#]", Text = reportObservation.ObservationTypeName });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph7#]", Text = reportObservation.ObservationCategory });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph8#]", Text = reportObservation.IsRepeated });
            string auditor = reportObservation.Auditor != null ? reportObservation.Auditor : string.Empty;
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph9#]", Text = auditor });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph10#]", Text = reportObservation.RootCause });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph11#]", Text = reportObservation.Implication });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph12#]", Text = reportObservation.Recommendation });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph13#]", Text = reportObservation.DispositionName });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph14#]", Text = reportObservation.Status });
            string linkedObservation = reportObservation.LinkedObservation != null ? reportObservation.LinkedObservation : string.Empty;
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph15#]", Text = linkedObservation });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph16#]", Text = reportObservation.ObservationTargetDate });

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph17#]", Text = reportObservation.ManagementResponse });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#Paragraph18#]", Text = reportObservation.Conclusion });

            //get first person responsible for report observation
            List<ReportObservationsMember> reportObservationMembers = await _dataRepository.Where<ReportObservationsMember>(a => !a.IsDeleted
             && a.ReportObservationId == reportObservation.Id).Include(a => a.User).ToListAsync();

            List<ReportObservationsMemberAC> personResponsibleAC = _mapper.Map<List<ReportObservationsMemberAC>>(reportObservationMembers);

            string responsiblePersons = "";
            for (int i = 0; i < personResponsibleAC.Count; i++)
            {
                responsiblePersons = responsiblePersons + personResponsibleAC[i].Name + " \n ";
            }
            string reviewerComments = "";
            //comment history
            for (int i = 0; i < reportObservation.ObservationReviewerList.Count; i++)

            {
                reviewerComments = reviewerComments + reportObservation.ObservationReviewerList[i].Name + " : "
                    + reportObservation.ObservationReviewerList[i].CreatedDate + " : " + reportObservation.ObservationReviewerList[i].Comment + "\n";
            }
            reviewerComments = _globalRepository.SetSpecialCharecters(reviewerComments);

            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List1(string[])#]", Text = responsiblePersons });
            pptTemplate.PowerPointParameters.Add(new PowerPointParameter() { Name = "[#List2(string[])#]", Text = reviewerComments });

            string currentPath = Path.GetTempPath() + StringConstant.ImageFolder;
            //bind image
            for (int i = 0; i < documentList.Count; i++)
            {
                pptTemplate.PowerPointParameters.Add(new PowerPointParameter()
                {
                    Name = "Img" + (i + 1).ToString(),
                    Image = new FileInfo(currentPath + @"\" + documentList[i].DocumentName)
                });
            }

            return pptTemplate;
        }

        #endregion

        /// <summary>
        /// Get report observation table detail
        /// </summary>
        /// <param name="observationTable">report observation table</param>
        /// <returns>Json data of report observation</returns>
        private async Task<JSONRoot> GetJsonReportObservationTableAsync(ReportObservationTable observationTable)
        {
            var jsonTable = JsonDocument.Parse(JsonSerializer.Serialize(observationTable.Table.RootElement));
            var jsonData = jsonTable.RootElement.ToString();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(jsonData);
            return result;
        }

    }
}
