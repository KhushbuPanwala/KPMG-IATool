using AutoMapper;
using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
using InternalAuditSystem.Repository.Repository.AuditTeamRepository;
using InternalAuditSystem.Repository.Repository.Common;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InternalAuditSystem.Utility.PdfGeneration;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using InternalAuditSystem.DomailModel.Models.UserModels;

namespace InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository
{
    public class StrategicAnalysisRespository : IStrategicAnalysisRepository
    {
        #region Private Variables
        public IHttpContextAccessor _httpContextAccessor;
        public Guid currentUserId = Guid.Empty;
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IFileUtility _fileUtility;
        private readonly IAzureRepository _azureRepo;
        private readonly IAuditableEntityRepository _auditableEntityRepo;
        private readonly IAuditTeamRepository _auditTeamRepository;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConfiguration _iConfig;
        #endregion

        #region Public variables
        public IGlobalRepository _globalRepository;
        public IUserRepository _userRepository;
        #endregion

        #region Constructor
        public StrategicAnalysisRespository(IHttpContextAccessor httpContextAccessor,
            IDataRepository dataRepository, IMapper mapper, IFileUtility fileUtility,
            IAzureRepository azureRepository, IGlobalRepository globalRepository, IAuditableEntityRepository auditableEntityRepository,
            IAuditTeamRepository auditTeamRepository, IUserRepository userRepository, IViewRenderService viewRenderService, IConfiguration iConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            _dataRepository = dataRepository;
            _mapper = mapper;
            _fileUtility = fileUtility;
            _azureRepo = azureRepository;
            _globalRepository = globalRepository;
            _auditableEntityRepo = auditableEntityRepository;
            _auditTeamRepository = auditTeamRepository;
            _userRepository = userRepository;
            _viewRenderService = viewRenderService;
            _iConfig = iConfig;
        }
        #endregion

        #region CRUD for Strategic Analysis

        /// <summary>
        /// Get sampling response of admin side
        /// </summary>
        /// <param name="pagination">Pagination data</param>
        /// <returns>List of sampling data</returns>
        public async Task<Pagination<StrategicAnalysisAC>> GetAllAdminSideSamplingsDataAsync(Pagination<StrategicAnalysisAC> pagination)
        {
            int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);
            var listOfSamplingInDb = _dataRepository.Where<StrategicAnalysis>(x => !x.IsDeleted
                        && x.CreatedBy == currentUserId
                        && (!String.IsNullOrEmpty(pagination.searchText) ? x.SurveyTitle.ToLower().Contains(pagination.searchText.ToLower()) : true)
                        && x.IsSampling == true)
                        .OrderByDescending(o => o.CreatedDateTime).Select(x =>
                        new StrategicAnalysis
                        {
                            Id = x.Id,
                            AuditableEntityName = x.AuditableEntityName,
                            SurveyTitle = x.SurveyTitle,
                            CreatedDateTime = x.CreatedDateTime,
                            UpdatedDateTime = x.UpdatedDateTime,
                            IsActive = x.IsActive,
                            Status = x.Status,
                            Version = x.Version
                        });

            pagination.TotalRecords = listOfSamplingInDb.Count();

            var finalListOfSampling = _mapper.Map<List<StrategicAnalysis>, List<StrategicAnalysisAC>>(listOfSamplingInDb.Skip(skippedRecords).Take(pagination.PageSize).AsNoTracking().ToList());
            var samplingIds = finalListOfSampling.Select(x => x.Id ?? new Guid()).ToList();
            var questionsOfAllSamplings = await _dataRepository.Where<Question>(x => samplingIds.Contains(x.StrategyAnalysisId) && !x.IsDeleted).Select(x => new Question { Id = x.Id, StrategyAnalysisId = x.StrategyAnalysisId }).AsNoTracking().ToListAsync();
            var userResponseList = await _dataRepository.Where<UserResponse>(x => samplingIds.Contains(x.StrategicAnalysisId) && !x.IsDeleted).AsNoTracking().ToListAsync();

            for (int i = 0; i < finalListOfSampling.Count(); i++)
            {
                finalListOfSampling[i].QuestionsCount = questionsOfAllSamplings.Count(x => x.StrategyAnalysisId == finalListOfSampling[i].Id && !x.IsDeleted);

                finalListOfSampling[i].ResponseCount = userResponseList.Where(x => x.StrategicAnalysisId == finalListOfSampling[i].Id && x.UserResponseStatus != StrategicAnalysisStatus.Draft && x.QuestionId != null).Select(x => x.RiskControlMatrixId).Distinct().Count();

            }
            pagination.Items = finalListOfSampling;
            return pagination;
        }

        /// <summary>
        /// Get all strategic analysis
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <param name="isSampling">Determines if this strategic analysis is of sampling</param>
        /// <param name="rcmId">Rcm Id for sampling list</param>
        /// <param name="isCallFromAdmin">Is Call from admin or user</param>
        /// <returns>Pagination containing list of strategic analysis</returns>
        public async Task<Pagination<StrategicAnalysisAC>> GetAllStrategicAnalysisAsync(Pagination<StrategicAnalysisAC> pagination, bool isSampling, string? rcmId, bool isCallFromAdmin)
        {
            try
            {
                // Apply pagination
                int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

                List<Guid> currentUsersStrategicAnalysisIdList = new List<Guid>();
                //if (!isSampling)
                //{
                //    currentUsersStrategicAnalysisIdList = _dataRepository.Where<StrategicAnalysisTeam>(x => x.UserId == currentUserId && !x.IsDeleted)
                //                                                         .Select(x => x.StrategicAnalysisId).ToList();
                //}

                var listOfStraticDbData = _dataRepository.Where<StrategicAnalysis>(x => !x.IsDeleted
                        && (!String.IsNullOrEmpty(pagination.searchText) ? x.SurveyTitle.ToLower().Contains(pagination.searchText.ToLower()) : true)
                        && x.IsSampling == isSampling)
                    //&& (!isSampling ? currentUsersStrategicAnalysisIdList.Contains(x.Id) : true)
                    //.Include(x => x.AuditableEntity)
                    .OrderByDescending(o => o.CreatedDateTime)
                .Select(x =>
                new StrategicAnalysis
                {
                    Id = x.Id,
                    AuditableEntityId = x.AuditableEntityId,
                    AuditableEntityName = x.AuditableEntityName,
                    SurveyTitle = x.SurveyTitle,
                    CreatedDateTime = x.CreatedDateTime,
                    UpdatedDateTime = x.UpdatedDateTime,
                    IsActive = x.IsActive,
                    Status = x.Status,
                    Version = x.Version,
                    CreatedBy = x.CreatedBy

                });

                //var strategicAnalyses = strategicAnalysesFromDB.Where<StrategicAnalysis>(x => !x.IsDeleted).OrderByDescending(o => o.CreatedDateTime);

                //Get total count
                pagination.TotalRecords = listOfStraticDbData.Count();

                var allStrategicAnalysisList = _mapper.Map<List<StrategicAnalysis>, List<StrategicAnalysisAC>>(listOfStraticDbData.Skip(skippedRecords).Take(pagination.PageSize).AsNoTracking().ToList());

                var strategicAnalysisIds = allStrategicAnalysisList.Select(x => x.Id ?? new Guid()).ToList();

                Guid? riskControlMatrixId = null;
                if (!string.IsNullOrEmpty(rcmId))
                {
                    riskControlMatrixId = new Guid(rcmId);
                }

                //set question count for each strategic analysis
                var questionListOfAllStrategy = await _dataRepository.Where<Question>(x => strategicAnalysisIds.Contains(x.StrategyAnalysisId) && !x.IsDeleted).Select(x => new Question { Id = x.Id, StrategyAnalysisId = x.StrategyAnalysisId }).AsNoTracking().ToListAsync();
                var questionsIds = questionListOfAllStrategy.Select(x => x.Id).ToList();



                var userResponseList = await _dataRepository.Where<UserResponse>(x =>
                                        strategicAnalysisIds.Contains(x.StrategicAnalysisId)
                                        && (riskControlMatrixId != null ? x.RiskControlMatrixId == riskControlMatrixId : true)
                                        && !x.IsDeleted).AsNoTracking().ToListAsync();

                List<UserResponse> userResponseListQuestionWise = userResponseList.Where<UserResponse>(x => questionsIds.Any(y => y == (x.QuestionId ?? new Guid()))).ToList();

                for (int i = 0; i < allStrategicAnalysisList.Count(); i++)
                {
                    //allStrategicAnalysisList [i].Questions = _mapper.Map<List<Question>, List<QuestionAC>>(questionList.Where<Question>(x => x.StrategyAnalysisId == allStrategicAnalysisList [i].Id).ToList());
                    allStrategicAnalysisList[i].QuestionsCount = questionListOfAllStrategy.Count(x => x.StrategyAnalysisId == allStrategicAnalysisList[i].Id && !x.IsDeleted);

                    if (allStrategicAnalysisList[i].QuestionsCount == 0)
                    {
                        // no question has been added yet
                        allStrategicAnalysisList[i].IsResponseDrafted = true;
                        allStrategicAnalysisList[i].Status = StrategicAnalysisStatus.Draft;
                    }
                    else
                    {
                        // if not sampling 
                        if (riskControlMatrixId == null)
                        {
                            var emailResponsesForCurrentStrategy = userResponseList.FirstOrDefault(x => x.StrategicAnalysisId == allStrategicAnalysisList[i].Id && x.IsEmailAttachements && x.QuestionId == null && x.UserId == currentUserId);

                            if (emailResponsesForCurrentStrategy != null)
                            {
                                allStrategicAnalysisList[i].Status = emailResponsesForCurrentStrategy.UserResponseStatus;
                                allStrategicAnalysisList[i].IsResponseDrafted = emailResponsesForCurrentStrategy.UserResponseStatus == StrategicAnalysisStatus.Draft;
                                allStrategicAnalysisList[i].IsEmailAttached = true;
                            }
                            else
                            {
                                var questionReponses = userResponseListQuestionWise.Where(x => x.StrategicAnalysisId == allStrategicAnalysisList[i].Id && x.UserId == currentUserId).ToList();
                                if (questionReponses.Count == 0)
                                {
                                    // if  no reponse has been added for any question
                                    allStrategicAnalysisList[i].Status = StrategicAnalysisStatus.Draft;
                                    allStrategicAnalysisList[i].IsResponseDrafted = true;
                                }
                                else
                                {
                                    allStrategicAnalysisList[i].Status = questionReponses.Count(x => x.UserResponseStatus == StrategicAnalysisStatus.Draft) > 0 ? StrategicAnalysisStatus.Draft : StrategicAnalysisStatus.UnderReview;
                                    allStrategicAnalysisList[i].IsResponseDrafted = false;

                                }

                            }
                        }
                        else
                        {
                            var rcmWiseResponseAvailable = userResponseList.Count(p => p.RiskControlMatrixId == riskControlMatrixId && p.StrategicAnalysisId == allStrategicAnalysisList[i].Id);
                            allStrategicAnalysisList[i].Status = rcmWiseResponseAvailable == 0 ? StrategicAnalysisStatus.Draft : StrategicAnalysisStatus.Final;

                        }

                    }
                    userResponseList = userResponseList.Where(a => a.UserResponseStatus != StrategicAnalysisStatus.Draft && a.QuestionId != null).ToList();
                    //group by auditable entity
                    var responseAC = userResponseList.GroupBy(x => new { x.StrategicAnalysisId, x.AuditableEntityId })
                              .Select(x => new UserResponseAC { StrategicAnalysisId = x.Key.StrategicAnalysisId, AuditableEntityId = x.Key.AuditableEntityId }).ToList();

                    allStrategicAnalysisList[i].ResponseCount = responseAC.Where(a => a.StrategicAnalysisId == allStrategicAnalysisList[i].Id).Count();
                    //removed for taking count auditable entity wise
                    //allStrategicAnalysisList[i].ResponseCount = userResponseList.Where(x => x.StrategicAnalysisId == allStrategicAnalysisList[i].Id && x.UserResponseStatus != StrategicAnalysisStatus.Draft && x.QuestionId != null).Select(x => x.UserId).Distinct().Count();

                    // TODO : Uncomment below section after email checking
                    //if (allStrategicAnalysisList [i].QuestionsCount > 0)
                    //{


                    //    for (int j = 0; j < allStrategicAnalysisList [i].QuestionsCount; j++)
                    //    {
                    //        UserResponse response = userResponseListQuestionWise.FirstOrDefault<UserResponse>(x => x.StrategicAnalysisId == allStrategicAnalysisList [i].Id && x.QuestionId == allStrategicAnalysisList [i].Questions[j].Id && x.UserId == currentUserId && x.UserResponseStatus != StrategicAnalysisStatus.Draft);
                    //        if (response == null)
                    //        {
                    //            // if under review or final response is not found then take draft user response
                    //            response = userResponseListQuestionWise.FirstOrDefault<UserResponse>(x => x.StrategicAnalysisId == allStrategicAnalysisList [i].Id && x.QuestionId == allStrategicAnalysisList [i].Questions[j].Id && x.UserId == currentUserId);
                    //        }
                    //        allStrategicAnalysisList [i].Questions[j].UserResponse = _mapper.Map<UserResponse, UserResponseAC>(response);
                    //        if (allStrategicAnalysisList [i].Questions[j].UserResponse != null)
                    //        {
                    //            UserResponse responseStatus = userResponseList.FirstOrDefault<UserResponse>(x => x.StrategicAnalysisId == allStrategicAnalysisList [i].Id
                    //            && x.IsEmailAttachements
                    //            && x.UserId == currentUserId);
                    //            allStrategicAnalysisList [i].Status = responseStatus == null ? allStrategicAnalysisList [i].Questions[j].UserResponse.UserResponseStatus : responseStatus.UserResponseStatus;

                    //        }
                    //    }
                    //}

                }

                //isCallFromAdmin from admin 
                if (isCallFromAdmin)
                {
                    pagination.Items = allStrategicAnalysisList.Where(a => a.CreatedBy == currentUserId).ToList();
                }
                else
                {
                    //is call from user 

                    HashSet<Guid> analysisIds = new HashSet<Guid>(allStrategicAnalysisList.Select(s => s.Id.Value));

                    //get user responses for getting auditable entity
                    var userResponses = _dataRepository.Where<UserResponse>(a => !a.IsDeleted && analysisIds.Contains(a.StrategicAnalysisId))
                    .Include(a => a.AuditableEntity).ToList();

                    //group by fto get analysis wise auditable entity
                    List<UserResponseAC> userResponseDetails = _mapper.Map<List<UserResponseAC>>(userResponses);
                    var responseAC = userResponseDetails.GroupBy(x => new { x.StrategicAnalysisId, x.AuditableEntityId, x.AuditableEntityName })
                              .Select(x => new UserResponseAC { StrategicAnalysisId = x.Key.StrategicAnalysisId, AuditableEntityId = x.Key.AuditableEntityId, AuditableEntityName = x.Key.AuditableEntityName }).ToList();

                    List<StrategicAnalysisAC> filterdAnalysisList = new List<StrategicAnalysisAC>();
                    for (int i = 0; i < allStrategicAnalysisList.Count; i++)
                    {
                        for (int j = 0; j < responseAC.Count; j++)
                        {
                            StrategicAnalysisAC strategicAnalysisAC = new StrategicAnalysisAC();
                            if (responseAC[j].StrategicAnalysisId == allStrategicAnalysisList[i].Id)
                            {
                                strategicAnalysisAC.Id = allStrategicAnalysisList[i].Id;
                                strategicAnalysisAC.SurveyTitle = allStrategicAnalysisList[i].SurveyTitle;
                                strategicAnalysisAC.QuestionsCount = allStrategicAnalysisList[i].QuestionsCount;
                                //strategicAnalysisAC.Status = allStrategicAnalysisList[i].Status;
                                strategicAnalysisAC.Status = userResponseDetails.Where(a => a.StrategicAnalysisId == allStrategicAnalysisList[i].Id
                                && a.AuditableEntityId == responseAC[j].AuditableEntityId).OrderByDescending(x => x.UpdatedDateTime).FirstOrDefault().UserResponseStatus;
                                strategicAnalysisAC.CreatedDate = allStrategicAnalysisList[i].CreatedDate;

                                strategicAnalysisAC.AuditableEntityName = responseAC[j].AuditableEntityName;
                                strategicAnalysisAC.AuditableEntityId = responseAC[j].AuditableEntityId;
                                strategicAnalysisAC.IsEmailAttached = allStrategicAnalysisList[i].IsEmailAttached;
                                strategicAnalysisAC.CreatedBy = userResponseDetails.Where(a => a.StrategicAnalysisId == allStrategicAnalysisList[i].Id
                                && a.AuditableEntityId == responseAC[j].AuditableEntityId).FirstOrDefault().CreatedBy;
                                strategicAnalysisAC.CreatedDateTime = userResponseDetails.Where(a => a.StrategicAnalysisId == allStrategicAnalysisList[i].Id
                                 && a.AuditableEntityId == responseAC[j].AuditableEntityId).LastOrDefault().CreatedDateTime;
                                filterdAnalysisList.Add(strategicAnalysisAC);
                            }
                        }
                    }

                    pagination.TotalRecords = responseAC.Count;
                    pagination.Items = filterdAnalysisList.Where(a => a.CreatedBy == currentUserId)
                        .OrderByDescending(a => a.CreatedDateTime)
                        .ToList();
                }
                return pagination;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get user responses count
        /// </summary>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>User response count</returns>
        public async Task<int> GetUserResponsesCountAsync(string strategicAnalysisId)
        {
            int totalRecords;

            totalRecords = await _dataRepository.Where<UserResponse>(a => !a.IsDeleted && a.StrategicAnalysisId == new Guid(strategicAnalysisId)).AsNoTracking().CountAsync();

            return totalRecords;
        }

        /// <summary>
        /// Get strategic analyses count
        /// </summary>
        /// <param name="searchString">Search value</param>
        /// <returns>Count of strategic analyses</returns>
        public async Task<int> GetStrategicAnalysesCountAsync(string searchString = null)
        {

            int totalRecords;
            if (!string.IsNullOrEmpty(searchString))
            {
                totalRecords = await _dataRepository.Where<StrategicAnalysis>(a => !a.IsDeleted && a.SurveyTitle.ToLower().Contains(searchString.ToLower())).AsNoTracking().CountAsync();
            }
            else
            {
                totalRecords = await _dataRepository.Where<StrategicAnalysis>(a => !a.IsDeleted).AsNoTracking().CountAsync();
            }

            return totalRecords;
        }

        /// <summary>
        /// Get email attachments documents
        /// </summary>
        /// <param name="strategyAnalysisId">Id of the strategic analysis</param>
        /// <param name="entityId">Entity Id </param>
        /// <returns>List of files attach for email/returns>
        public async Task<StrategicAnalysisAC> GetEmailAttachmentDocumentsAsync(Guid strategyAnalysisId, Guid entityId)
        {
            var strategicAnalysisAC = new StrategicAnalysisAC();

            var userResponsesOfEmailAttachments = await _dataRepository.Where<UserResponse>(
                    x => x.IsEmailAttachements
                    && x.UserId == currentUserId
                    && x.StrategicAnalysisId == strategyAnalysisId
                    && x.AuditableEntityId == entityId
                    && !x.IsDeleted).AsNoTracking().ToListAsync();

            var userResponsesOfEmailAttachmentsIds = userResponsesOfEmailAttachments.Select(x => x.Id).ToList();
            var emailAttachmentDocs = await _dataRepository.Where<UserResponseDocument>(x => userResponsesOfEmailAttachmentsIds.Contains(x.UserResponseId) && !x.IsDeleted).AsNoTracking().ToListAsync();

            if (emailAttachmentDocs != null && emailAttachmentDocs.Count > 0)
            {
                strategicAnalysisAC.UserResponseDocumentACs = _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(emailAttachmentDocs);
                for (int k = 0; k < strategicAnalysisAC.UserResponseDocumentACs.Count(); k++)
                {
                    strategicAnalysisAC.UserResponseDocumentACs[k].FileName = strategicAnalysisAC.UserResponseDocumentACs[k].Path;
                    strategicAnalysisAC.UserResponseDocumentACs[k].Path = _azureRepo.DownloadFile(strategicAnalysisAC.UserResponseDocumentACs[k].Path);
                }
            }

            return strategicAnalysisAC;
        }

        /// <summary>
        /// Save email attachment for a particular strategy analysis
        /// </summary>
        /// <param name="emailAttachmentResponse">Email attachment object</param>
        /// <returns>Task</returns>
        public async Task UploadEmailAttachmentDocumentsAsync(UserResponseAC emailAttachmentResponse)
        {
            await AddAndUploadStrategicAnalysisFiles(emailAttachmentResponse.Files, (Guid)emailAttachmentResponse.StrategicAnalysisId, currentUserId, null, true, (Guid)emailAttachmentResponse.AuditableEntityId);
        }

        /// <summary>
        /// Save only file type question responses for each strategy 
        /// </summary>
        /// <param name="formdataList">List of all file response</param>
        /// <param name="strategyAnalysisId">Id of the strategy analysis</param>
        /// <param name="isDrafted">Bit to check response drafted or not </param>
        /// <param name="entityId">entity id</param>
        /// <returns>Task</returns>
        public async Task SaveFileTypeQuestionReponseAsync(IFormCollection formdataList, string strategyAnalysisId, bool isDrafted, Guid? entityId = null)
        {

            // List<UserResponseAC> questionResponseList = new List<UserResponseAC>();


            var questionWiseResponseList = formdataList.Files.GroupBy(x => x.Name).ToList();
            HashSet<string> questionIds = new HashSet<string>(questionWiseResponseList.Select(s => s.Key));


            // get all file type question response
            var savedResponses = _dataRepository.Where<UserResponse>(x => !x.IsDeleted
                                                                       && x.UserId == currentUserId
                                                                       && questionIds.Contains(x.QuestionId.ToString())
                                                                       && entityId == Guid.Empty ? true : x.AuditableEntityId == entityId
                                                                       && x.StrategicAnalysisId.ToString() == strategyAnalysisId).AsNoTracking().ToList();

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < questionWiseResponseList.Count; i++)
                    {

                        var questionWiseFiles = new List<IFormFile>();
                        var existingData = savedResponses.FirstOrDefault(x => x.QuestionId.ToString() == questionWiseResponseList[i].Key);
                        UserResponse userResponse = new UserResponse();

                        if (existingData == null)
                        {
                            // Add user response to which userResponseDocument is to be connected
                            userResponse.UserId = currentUserId;
                            userResponse.CreatedBy = currentUserId;
                            userResponse.CreatedDateTime = DateTime.UtcNow;
                            userResponse.StrategicAnalysisId = new Guid(strategyAnalysisId);
                            userResponse.IsEmailAttachements = false;
                            userResponse.UserResponseStatus = isDrafted ? StrategicAnalysisStatus.Draft : StrategicAnalysisStatus.UnderReview;

                            userResponse = await _dataRepository.AddAsync(userResponse);
                            await _dataRepository.SaveChangesAsync();
                        }
                        else
                        {
                            existingData.UpdatedBy = currentUserId;
                            existingData.UpdatedDateTime = DateTime.UtcNow;
                            existingData.UserResponseStatus = isDrafted ? StrategicAnalysisStatus.Draft : existingData.UserResponseStatus;

                            _dataRepository.Update(existingData);
                            await _dataRepository.SaveChangesAsync();
                        }

                        //Upload file to azure
                        foreach (var file in questionWiseResponseList[i].ToList())
                        {
                            questionWiseFiles.Add(file);
                        }
                        // documentFiles.Add(questionWiseResponseList[i].cou);
                        List<string> filesUrl = new List<string>();
                        filesUrl = await _fileUtility.UploadFilesAsync(questionWiseFiles);
                        List<UserResponseDocument> addedFileList = new List<UserResponseDocument>();
                        for (int j = 0; j < filesUrl.Count(); j++)
                        {
                            UserResponseDocument newDocument = new UserResponseDocument()
                            {
                                Id = new Guid(),
                                Path = filesUrl[j],
                                UserResponseId = existingData == null ? userResponse.Id : (Guid)existingData.Id,
                                CreatedBy = currentUserId,
                                CreatedDateTime = DateTime.UtcNow
                            };
                            addedFileList.Add(newDocument);
                        }
                        await _dataRepository.AddRangeAsync(addedFileList);
                        await _dataRepository.SaveChangesAsync();
                    }
                    _dataRepository.CommitTransaction();

                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }

        }

        /// <summary>
        /// Get strategic analysis by id
        /// </summary>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <param name="riskControlMatrixId">riskcontrolmatric id</param>
        /// <param name="isGeneralPgae">Bit to check general page or not </param>
        /// <param name="entityId">selected entity id</param>
        /// <returns>StrategicAnalysisAC</returns>
        public async Task<StrategicAnalysisAC> GetStrategicAnalysisById(string strategicAnalysisId, Guid? riskControlMatrixId = null, bool isGeneralPgae = false, string entityId = "")
        {
            try
            {
                StrategicAnalysis strategicAnalysis = await _dataRepository.FirstOrDefaultAsync<StrategicAnalysis>(a => a.Id.ToString() == strategicAnalysisId && !a.IsDeleted);
                StrategicAnalysisAC strategicAnalysisAC = _mapper.Map<StrategicAnalysisAC>(strategicAnalysis);
                #region Get question Page data
                var userResponses = new List<UserResponse>();
                if (!isGeneralPgae)
                {
                    List<QuestionAC> questions = await this.GetQuestionsAsync(strategicAnalysisId, riskControlMatrixId);
                    var questionIds = questions.Select(x => x.Id).ToList();

                    if (riskControlMatrixId != null)
                    {
                        userResponses = await _dataRepository.Where<UserResponse>(x => questionIds.Contains(x.QuestionId)
                                                                               //&& (riskControlMatrixId != null ? true : x.UserId == currentUserId)
                                                                               //&& riskControlMatrixId != null ? true : (entityId == null ? false : x.AuditableEntityId == entityId)
                                                                               && !x.IsDeleted)
                                                                               .AsNoTracking()
                                                                               .Include(x => x.Options)
                                                                               .AsNoTracking()
                                                                               .ToListAsync();

                    }
                    else
                    {
                        userResponses = await _dataRepository.Where<UserResponse>(x => questionIds.Contains(x.QuestionId) &&
                                     (riskControlMatrixId != null ? true : x.UserId == currentUserId)
                                    && (entityId == string.Empty ? false : x.AuditableEntityId.ToString() == entityId)
                                    && !x.IsDeleted)
                                    .AsNoTracking()
                                    .Include(x => x.Options)
                                    .AsNoTracking()
                                    .ToListAsync();


                        //userResponses = await _dataRepository.Where<UserResponse>(x => questionIds.Contains(x.QuestionId)
                        //                                                       && x.UserId == currentUserId &&
                        //                                                       entityId == null ? false : x.AuditableEntityId == entityId
                        //                                                       && !x.IsDeleted)
                        //                                                       .AsNoTracking()
                        //                                                       .Include(x => x.Options)
                        //                                                       .AsNoTracking()
                        //.ToListAsync();

                    }
                    if (riskControlMatrixId != null)
                    {
                        userResponses = userResponses.Where(x => x.RiskControlMatrixId == riskControlMatrixId).ToList();
                    }
                    List<UserResponse> fileUserResponses = new List<UserResponse>();
                    var userResponsesIds = userResponses.Select(x => x.Id).ToList();
                    var userResponseDocumentACs = _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(await _dataRepository.Where<UserResponseDocument>(x => userResponsesIds.Contains(x.UserResponseId) && !x.IsDeleted).AsNoTracking().ToListAsync());
                    if (userResponses.Count() > 0)
                    {
                        for (var i = 0; i < questions.Count(); i++)
                        {
                            //addd user id and entityid
                            var userResponseAC = userResponses.FirstOrDefault<UserResponse>(x => x.QuestionId == questions[i].Id && x.AuditableEntityId.ToString() == entityId);
                            if (userResponseAC != null)
                            {
                                questions[i].UserResponse = _mapper.Map<UserResponse, UserResponseAC>(userResponseAC);
                            }
                            if (userResponseAC != null && !userResponseAC.IsEmailAttachements)
                            {
                                fileUserResponses = userResponses.Where<UserResponse>(x => x.QuestionId == questions[i].Id
                                && x.AuditableEntityId.ToString() == entityId).ToList();
                                var fileUserResponsesIds = fileUserResponses.Select(x => x.Id).ToList();
                                questions[i].UserResponseDocumentACs = userResponseDocumentACs.Where<UserResponseDocumentAC>(x => fileUserResponsesIds.Contains(x.UserResponseId)).ToList();
                                //ToDo: Document file path update in questions' document list
                                for (int k = 0; k < questions[i].UserResponseDocumentACs.Count(); k++)
                                {
                                    questions[i].UserResponseDocumentACs[k].FileName = questions[i].UserResponseDocumentACs[k].Path;
                                    questions[i].UserResponseDocumentACs[k].Path = _azureRepo.DownloadFile(questions[i].UserResponseDocumentACs[k].Path);
                                }

                            }

                        }
                    }
                    else
                    {
                        // if not sampling then only refresh the responses for new entity
                        if (riskControlMatrixId == null)
                        {
                            for (int i = 0; i < questions.Count; i++)
                            {
                                if (questions[i].UserResponse != null)
                                {
                                    questions[i].UserResponse.Id = Guid.Empty;
                                    questions[i].UserResponse.UserResponseStatus = StrategicAnalysisStatus.Draft;
                                    questions[i].UserResponse.RepresentationNumber = 0;
                                    questions[i].UserResponse.SelectedDropdownOption = null;
                                    questions[i].UserResponse.AnswerText = string.Empty;

                                }
                            }

                        }

                    }
                    if (questions.Count > 0)
                    {
                        strategicAnalysisAC.Questions = questions;
                    }
                }
                #endregion

                #region Get General Page data 
                if (isGeneralPgae && riskControlMatrixId == null)
                {


                    // user response with details of opportunity and estimated value of opportunity of current user
                    UserResponse userResponseWithDetailsAndEstimatedValue = await _dataRepository.FirstOrDefaultAsync<UserResponse>
                    (x => x.IsDetailAndEstimatedValueOfOpportunity
                    && x.AuditableEntityId.ToString() == entityId
                    && x.StrategicAnalysisId == strategicAnalysisAC.Id
                    && x.UserId == currentUserId
                    && !x.IsDeleted);
                    //add filter for auditable entity id

                    var internalUserList = await _dataRepository.Where<StrategicAnalysisTeam>(x => x.StrategicAnalysisId == strategicAnalysisAC.Id
                    && x.AuditableEntityId.ToString() == entityId && !x.IsDeleted).Include(x => x.User).AsNoTracking().ToListAsync();

                    strategicAnalysisAC.InternalUserList = _mapper.Map<List<StrategicAnalysisTeamAC>>(internalUserList);

                    if (userResponseWithDetailsAndEstimatedValue != null)
                    {
                        strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue = _mapper.Map<UserResponseForDetailsAndEstimatedValueOfOpportunity>(userResponseWithDetailsAndEstimatedValue);
                    }
                }
                #endregion
                var entity = _dataRepository.FirstOrDefault<AuditableEntity>(a => !a.IsDeleted && a.Id.ToString() == entityId);
                strategicAnalysisAC.AuditableEntityName = entity != null ? entity.Name : string.Empty;
                strategicAnalysisAC.AuditableEntityId = entityId != string.Empty ? Guid.Parse(entityId) : new Guid();
                return strategicAnalysisAC;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Get strategicAnalysisId of draft status
        /// </summary>
        /// <returns>First strategic analysis id of strategic analysis whose status is draft</returns>
        public async Task<string> GetStrategicAnalysisIdOfDraftStatusAsync()
        {
            try
            {
                // Check if current user is present in any team of strategic analysis whose questions' user response is draft
                var thisTeamUsersOfDraftStrategicAnalyses = await _dataRepository.Where<StrategicAnalysisTeam>
                (x => x.UserId == currentUserId
                && x.StrategicAnalysis.Status == StrategicAnalysisStatus.Draft
                && !x.StrategicAnalysis.IsDeleted
                && !x.IsDeleted).Include(x => x.StrategicAnalysis).AsNoTracking().ToListAsync();
                var returnIdOrAllCompletedString = String.Empty;

                StrategicAnalysis strategicAnalysisToReturn = null;

                if (thisTeamUsersOfDraftStrategicAnalyses.Count > 0)
                {
                    for (int i = 0; i < thisTeamUsersOfDraftStrategicAnalyses.Count(); i++)
                    {
                        StrategicAnalysis strategicAnalysis = thisTeamUsersOfDraftStrategicAnalyses[i].StrategicAnalysis;
                        List<QuestionAC> questions = await this.GetQuestionsAsync(strategicAnalysis.Id.ToString());
                        if ((questions != null) && (questions.Count() > 0) && (questions[0].UserResponse.UserResponseStatus == StrategicAnalysisStatus.Draft))
                        {
                            strategicAnalysisToReturn = strategicAnalysis;
                            break;
                        }
                    }

                    returnIdOrAllCompletedString = strategicAnalysisToReturn == null ? StringConstant.CompletedAllSurvey : strategicAnalysisToReturn.Id.ToString();
                }
                else
                {
                    returnIdOrAllCompletedString = StringConstant.CompletedAllSurvey;
                }
                return returnIdOrAllCompletedString;


            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Add strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">strategicAnalysisAC</param>
        /// <returns>strategicAnalysisAC</returns>
        public async Task<StrategicAnalysisAC> AddStrategicAnalysisAsync(StrategicAnalysisAC strategicAnalysisAC)
        {
            if (!strategicAnalysisAC.IsVersionToBeChanged && CheckStrategicAnalysisExist(strategicAnalysisAC.SurveyTitle, strategicAnalysisAC.AuditableEntityId.ToString(), strategicAnalysisAC.Id.ToString(), strategicAnalysisAC.Version))
            {
                throw new DuplicateDataException(StringConstant.StrategicAnalysisString, strategicAnalysisAC.SurveyTitle);
            }
            else
            {

                using (_dataRepository.BeginTransaction())
                {
                    try
                    {
                        StrategicAnalysis strategicAnalysis = _mapper.Map<StrategicAnalysisAC, StrategicAnalysis>(strategicAnalysisAC);
                        strategicAnalysis.IsActive = true;
                        strategicAnalysis.CreatedBy = currentUserId;
                        strategicAnalysis.CreatedDateTime = DateTime.UtcNow;
                        strategicAnalysis.UserCreatedBy = null;

                        #region new version create
                        if (strategicAnalysisAC.IsVersionToBeChanged)
                        {
                            var oldStrategicAnalysisId = strategicAnalysis.Id;
                            // Create new strategicAnalysis Id
                            strategicAnalysis.Id = Guid.NewGuid();

                            List<StrategicAnalysis> allStrategicAnalyses = await _dataRepository.Where<StrategicAnalysis>(x => x.AuditableEntityId == strategicAnalysisAC.AuditableEntityId && !x.IsDeleted).AsNoTracking().ToListAsync();
                            var version = allStrategicAnalyses.Count > 0 ? allStrategicAnalyses.Select(x => x.Version).ToList().Max() : 1;
                            strategicAnalysis.Version = version + 1.0;
                            strategicAnalysis.AuditableEntityId = null;
                            strategicAnalysis.AuditableEntity = null;


                            //Add strategic analysis
                            await _dataRepository.AddAsync(strategicAnalysis);
                            await _dataRepository.SaveChangesAsync();

                            // add questions to new version
                            List<QuestionAC> questionsAC = await GetQuestionsAsync(strategicAnalysisAC.Id.ToString());

                            var i = 0;
                            for (i = 0; i < questionsAC.Count(); i++)
                            {
                                questionsAC[i].Id = null;
                                await CommonAddQuestionAsync(questionsAC[i], strategicAnalysis.Id, oldStrategicAnalysisId.ToString());
                            }

                            await _dataRepository.SaveChangesAsync();

                        }
                        #endregion
                        else
                        {
                            strategicAnalysis.AuditableEntity = null;
                            //Add strategic analysis
                            await _dataRepository.AddAsync(strategicAnalysis);
                            await _dataRepository.SaveChangesAsync();
                        }

                        StrategicAnalysisAC strategicAnalysisAllDataAc = _mapper.Map<StrategicAnalysis, StrategicAnalysisAC>(strategicAnalysis);
                        _dataRepository.CommitTransaction();
                        return strategicAnalysisAllDataAc;
                    }
                    catch (RequiredDataException)
                    {
                        _dataRepository.RollbackTransaction();
                        throw;
                    }
                }
            }

        }

        /// <summary>
        /// Update Strategic Analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">StrategicAnalysisAC</param>
        /// <returns>Updated strategicAnalysisAC</returns>
        public async Task<StrategicAnalysisAC> UpdateStrategicAnalysisAsync(StrategicAnalysisAC strategicAnalysisAC)
        {
            // if not sampling data then only check this condition
            if (!strategicAnalysisAC.IsSampling)
            {
                // check duplicate entry
                if (CheckStrategicAnalysisExist(strategicAnalysisAC.SurveyTitle, strategicAnalysisAC.AuditableEntityId.ToString(), strategicAnalysisAC.Id.ToString(), strategicAnalysisAC.Version))
                {
                    throw new DuplicateDataException(StringConstant.StrategicAnalysisString, strategicAnalysisAC.SurveyTitle);
                }

                //// if current admin user is not being added to team 
                //if (!await _dataRepository.AnyAsync<StrategicAnalysisTeam>(x => x.UserId == currentUserId && x.StrategicAnalysisId == strategicAnalysisAC.Id && !x.IsDeleted))
                //{
                //    throw new RequiredDataException(StringConstant.AddCurrentAdminUser);
                //}
            }

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    StrategicAnalysis strategicAnalysis = _mapper.Map<StrategicAnalysis>(strategicAnalysisAC);
                    bool isEntityAdded = false;
                    Guid auditableEntityId = new Guid();
                    StrategicAnalysis originalStrategicAnalysis = await _dataRepository.FirstAsync<StrategicAnalysis>(x => x.Id == strategicAnalysisAC.Id);
                    if (!strategicAnalysisAC.IsSampling)
                    {

                        if (strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue != null)
                        {
                            // Add user response with details of opportunity and estimated value of opportunity if not added otherwise update

                            UserResponse userResponseOfDetailsAndEstimatedValueInDb = await _dataRepository.FirstOrDefaultAsync<UserResponse>(x =>
                            x.StrategicAnalysisId == strategicAnalysis.Id &&
                            strategicAnalysisAC.AuditableEntityId == null ? false : x.AuditableEntityId == strategicAnalysisAC.AuditableEntityId
                            && x.UserId == currentUserId
                            && x.IsDetailAndEstimatedValueOfOpportunity);


                            if (userResponseOfDetailsAndEstimatedValueInDb == null)
                            {

                                #region create entity only for strategy analysis
                                //Add auditable entity with auditable entity name passed with strategicAnalysisAC
                                AuditableEntityAC addedAuditableEntity = new AuditableEntityAC();
                                addedAuditableEntity.Name = strategicAnalysisAC.AuditableEntityName;
                                addedAuditableEntity.Status = AuditableEntityStatus.Active;
                                addedAuditableEntity.IsStrategyAnalysisDone = false;
                                addedAuditableEntity.Version = 1;
                                addedAuditableEntity.Description = "";
                                addedAuditableEntity.SelectedCategoryId = null;
                                addedAuditableEntity.SelectedTypeId = null;

                                if (!strategicAnalysisAC.IsVersionToBeChanged && !strategicAnalysisAC.IsSampling)
                                {
                                    addedAuditableEntity = await _auditableEntityRepo.AddAuditableEntityAsync(addedAuditableEntity, true);
                                }
                                //set  new added auditableEntityId 
                                auditableEntityId = (Guid)addedAuditableEntity.Id;
                                #endregion


                                // create user response
                                UserResponse userResponseWithDetailsAndEstimatedValue = _mapper.Map<UserResponse>(strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue);
                                userResponseWithDetailsAndEstimatedValue.IsDetailAndEstimatedValueOfOpportunity = true;
                                userResponseWithDetailsAndEstimatedValue.StrategicAnalysisId = strategicAnalysis.Id;
                                userResponseWithDetailsAndEstimatedValue.IsDeleted = false;
                                userResponseWithDetailsAndEstimatedValue.CreatedBy = currentUserId;
                                userResponseWithDetailsAndEstimatedValue.UserId = currentUserId;
                                userResponseWithDetailsAndEstimatedValue.CreatedDateTime = DateTime.UtcNow;
                                userResponseWithDetailsAndEstimatedValue.UserResponseStatus = StrategicAnalysisStatus.Draft;
                                //add entity id
                                userResponseWithDetailsAndEstimatedValue.AuditableEntityId = auditableEntityId;
                                userResponseWithDetailsAndEstimatedValue.AuditableEntity = null;

                                await _dataRepository.AddAsync<UserResponse>(userResponseWithDetailsAndEstimatedValue);
                                await _dataRepository.SaveChangesAsync();
                                isEntityAdded = true;
                            }
                            else
                            {
                                //update auditable entity name 
                                var updatedAuditableEntity = _dataRepository.FirstOrDefault<AuditableEntity>(a => !a.IsDeleted && a.Id == userResponseOfDetailsAndEstimatedValueInDb.AuditableEntityId);

                                if (updatedAuditableEntity.Name != strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue.AuditableEntityName)
                                {
                                    updatedAuditableEntity.Name = strategicAnalysisAC.AuditableEntityName;
                                    _dataRepository.Update<AuditableEntity>(updatedAuditableEntity);
                                }

                                // update user response
                                userResponseOfDetailsAndEstimatedValueInDb.DetailsOfOpportunity = strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue.DetailsOfOpportunity;
                                userResponseOfDetailsAndEstimatedValueInDb.EstimatedValueOfOpportunity = strategicAnalysisAC.UserResponseForDetailsOfOppAndEstimatedValue.EstimatedValueOfOpportunity;
                                userResponseOfDetailsAndEstimatedValueInDb.UpdatedBy = currentUserId;
                                userResponseOfDetailsAndEstimatedValueInDb.UpdatedDateTime = DateTime.UtcNow;
                                userResponseOfDetailsAndEstimatedValueInDb.UserResponseStatus = (userResponseOfDetailsAndEstimatedValueInDb.UserResponseStatus == StrategicAnalysisStatus.Draft ? StrategicAnalysisStatus.UnderReview : userResponseOfDetailsAndEstimatedValueInDb.UserResponseStatus);
                                _dataRepository.Update<UserResponse>(userResponseOfDetailsAndEstimatedValueInDb);
                                await _dataRepository.SaveChangesAsync();

                                //set updated auditable entity id
                                auditableEntityId = updatedAuditableEntity.Id;
                            }
                        }


                        originalStrategicAnalysis.SurveyTitle = strategicAnalysis.SurveyTitle;
                        originalStrategicAnalysis.Message = strategicAnalysis.Message;
                        originalStrategicAnalysis.UpdatedBy = currentUserId;
                        originalStrategicAnalysis.UpdatedDateTime = DateTime.UtcNow;
                        originalStrategicAnalysis.Status = strategicAnalysis.Status;
                        originalStrategicAnalysis.IsSampling = strategicAnalysis.IsSampling;
                        originalStrategicAnalysis.AuditableEntityId = null;
                        // if email attachments are there set status as underreview
                        if (strategicAnalysisAC.Files != null && strategicAnalysisAC.Files.Count() > 0)
                        {
                            originalStrategicAnalysis.Status = StrategicAnalysisStatus.UnderReview;
                        }

                        _dataRepository.Update(originalStrategicAnalysis);
                        await _dataRepository.SaveChangesAsync();
                    }
                    else
                    {
                        // update only sampling title 
                        originalStrategicAnalysis.SurveyTitle = strategicAnalysis.SurveyTitle;
                        originalStrategicAnalysis.UpdatedBy = currentUserId;
                        originalStrategicAnalysis.UpdatedDateTime = DateTime.UtcNow;
                        originalStrategicAnalysis.AuditableEntityId = null;
                        originalStrategicAnalysis.AuditableEntity = null;
                        _dataRepository.Update(originalStrategicAnalysis);
                        await _dataRepository.SaveChangesAsync();
                    }


                    StrategicAnalysisAC updatedStrategicAnalysisReturnAC = _mapper.Map<StrategicAnalysisAC>(originalStrategicAnalysis);
                    Guid? riskControlMatrixId = null;
                    if (strategicAnalysisAC.Questions != null && strategicAnalysisAC.Questions.Count() > 0)
                    {
                        riskControlMatrixId = strategicAnalysisAC.Questions[0].UserResponse?.RiskControlMatrixId;

                    }

                    #region Add or update user response
                    if ((bool)strategicAnalysisAC.IsUserResponseToBeUpdated && strategicAnalysisAC.Questions != null)
                    {
                        // Add user responses if new else update
                        List<UserResponseAC> listOfUserResponses = strategicAnalysisAC.Questions.Select(x => x.UserResponse).ToList();
                        //listOfUserResponses = listOfUserResponses.Where(a => a.AuditableEntityId == strategicAnalysisAC.AuditableEntityId).ToList();
                        List<UserResponse> responsesToSave = _mapper.Map<List<UserResponseAC>, List<UserResponse>>(listOfUserResponses);


                        if (riskControlMatrixId != null)
                        {
                            responsesToSave = responsesToSave.Where(x => x.RiskControlMatrixId == riskControlMatrixId).ToList();
                        }

                        if (responsesToSave.Count() > 0 && responsesToSave[0] != null)
                        {
                            List<Option> questionOptions = await _dataRepository.Where<Option>(x => !x.IsDeleted).AsNoTracking().ToListAsync();
                            List<UserResponse> userResponsesToAdd = new List<UserResponse>();
                            List<UserResponse> userResponsesToUpdate = new List<UserResponse>();

                            // Find if any user response of any question is null
                            var isNullUserResponseExists = listOfUserResponses.Any(x => x == null);
                            if (!isNullUserResponseExists)
                            {
                                // To update option of user response for mcq questions
                                for (var i = 0; i < listOfUserResponses.Count(); i++)
                                {
                                    List<string> listOfOptionSelectedByUser = new List<string>();

                                    if (listOfUserResponses[i] != null)
                                    {
                                        // if user response is of mcq question
                                        if (listOfUserResponses[i].MultipleChoiceQuestionId != null)
                                        {
                                            if (listOfUserResponses[i].Options != null)
                                            {
                                                listOfOptionSelectedByUser = listOfUserResponses[i].Options.Select(x => x.OptionText).ToList();
                                                var optionsOfCurrentQuestion = questionOptions.Where(x => x.MultipleChoiceQuestionId == listOfUserResponses[i].MultipleChoiceQuestionId
                                                    && listOfOptionSelectedByUser.Contains(x.OptionText)).ToList();
                                                responsesToSave[i].OptionIds = optionsOfCurrentQuestion.Select(x => x.Id).ToList();
                                                listOfUserResponses[i].Options = _mapper.Map<List<Option>, List<OptionAC>>(optionsOfCurrentQuestion);
                                            }
                                        }
                                        // if user response is of checkbox question
                                        if (listOfUserResponses[i].CheckboxQuestionId != null)
                                        {
                                            if (listOfUserResponses[i].Options != null)
                                            {
                                                listOfOptionSelectedByUser = listOfUserResponses[i].Options.Select(x => x.OptionText).ToList();
                                                var optionsOfCurrentQuestion = questionOptions.Where(x => x.CheckboxQuestionId == listOfUserResponses[i].CheckboxQuestionId
                                                    && listOfOptionSelectedByUser.Contains(x.OptionText)).ToList();
                                                responsesToSave[i].OptionIds = optionsOfCurrentQuestion.Select(x => x.Id).ToList();
                                                listOfUserResponses[i].Options = _mapper.Map<List<Option>, List<OptionAC>>(optionsOfCurrentQuestion);
                                            }

                                        }
                                        // if user response is of dropdown question
                                        if (listOfUserResponses[i].DropdownQuestionId != null)
                                        {
                                            listOfOptionSelectedByUser = listOfUserResponses[i].Options.Select(x => x.OptionText).ToList();
                                            var optionsOfCurrentQuestion = questionOptions.Where(x => x.DropdownQuestionId == listOfUserResponses[i].DropdownQuestionId
                                                && listOfUserResponses[i].SelectedDropdownOption == x.OptionText).ToList();
                                            responsesToSave[i].OptionIds = questionOptions.Select(x => x.Id).ToList();
                                            listOfUserResponses[i].Options = _mapper.Map<List<Option>, List<OptionAC>>(optionsOfCurrentQuestion);
                                        }

                                        responsesToSave[i].UserId = currentUserId;
                                        responsesToSave[i].StrategicAnalysisId = (Guid)strategicAnalysisAC.Id;

                                        if (riskControlMatrixId != null)
                                        {
                                            responsesToSave[i].SamplingResponseStatus = SamplingResponseStatus.Completed;
                                        }
                                        responsesToSave[i].CreatedBy = currentUserId;
                                        responsesToSave[i].UpdatedBy = currentUserId;
                                        responsesToSave[i].QuestionId = listOfUserResponses[i].QuestionId;
                                        responsesToSave[i].UpdatedDateTime = DateTime.UtcNow;
                                        //add entity id
                                        responsesToSave[i].AuditableEntityId = strategicAnalysisAC.AuditableEntityId;
                                        responsesToSave[i].AuditableEntity = null;

                                        // if (listOfUserResponses[i]. != QuestionType.FileUpload)
                                        {
                                            userResponsesToUpdate.Add(responsesToSave[i]);
                                        }
                                    }
                                }

                                if (userResponsesToUpdate.Count() > 0)
                                {
                                    _dataRepository.UpdateRange(userResponsesToUpdate);
                                    await _dataRepository.SaveChangesAsync();
                                }

                                listOfUserResponses = _mapper.Map<List<UserResponse>, List<UserResponseAC>>(userResponsesToUpdate);
                                updatedStrategicAnalysisReturnAC.Questions = strategicAnalysisAC.Questions;
                                for (var a = 0; a < strategicAnalysisAC.Questions.Count(); a++)
                                {
                                    updatedStrategicAnalysisReturnAC.Questions[a].UserResponse = listOfUserResponses[a];
                                }

                            }
                        }

                        var filesResponses = _mapper.Map<List<List<UserResponseAC>>, List<List<UserResponse>>>(strategicAnalysisAC.Questions.Select(x => x.FileResponseList).ToList());
                        var allFileResponses = filesResponses.SelectMany(i => i).Distinct().ToList();
                        if (allFileResponses.Count() > 0 && allFileResponses[0] != null)
                        {
                            for (var j = 0; j < allFileResponses.Count(); j++)
                            {
                                allFileResponses[j].UpdatedBy = currentUserId;
                                allFileResponses[j].UpdatedDateTime = DateTime.UtcNow;
                            }
                            _dataRepository.UpdateRange(allFileResponses);
                            //await _dataRepository.SaveChangesAsync();
                        }

                    }
                    #endregion

                    #region Add email attachments
                    //if (strategicAnalysisAC.Files != null && strategicAnalysisAC.Files.Count() > 0)
                    //{
                    //    updatedStrategicAnalysisReturnAC.UserResponseDocumentACs = await AddAndUploadStrategicAnalysisFiles(strategicAnalysisAC.Files, strategicAnalysisAC.Id ?? new Guid(), currentUserId, null, true, strategicAnalysisAC);
                    //    updatedStrategicAnalysisReturnAC.Files = null;
                    //}
                    //else
                    //{
                    //    updatedStrategicAnalysisReturnAC.UserResponseDocumentACs = new List<UserResponseDocumentAC>();
                    //}
                    #endregion


                    #region Add or update audit team 

                    if (isEntityAdded)
                    {
                        List<StrategicAnalysisTeam> addedTeamMembers = _dataRepository.Where<StrategicAnalysisTeam>(a => !a.IsDeleted && a.StrategicAnalysisId == strategicAnalysisAC.Id && a.AuditableEntityId == auditableEntityId).ToList();
                        List<StrategicAnalysisTeam> newTeamMembers = _mapper.Map<List<StrategicAnalysisTeam>>(strategicAnalysisAC.InternalUserList);

                        if (addedTeamMembers.Count == 0)
                        {

                            for (int k = 0; k < newTeamMembers.Count; k++)
                            {
                                newTeamMembers[k].AuditableEntityId = auditableEntityId;
                                newTeamMembers[k].AuditableEntity = null;

                            }

                            _dataRepository.AddRange<StrategicAnalysisTeam>(newTeamMembers);
                            _dataRepository.SaveChanges();
                            //auditTeamMembers = await _auditTeamRepository.AddListOfAuditTeamMembersAsync(strategicAnalysisAC.TeamCollection, auditableEntityId.ToString());
                        }
                    }

                    #endregion
                    updatedStrategicAnalysisReturnAC.AuditableEntityId = auditableEntityId;
                    transaction.Commit();
                    return updatedStrategicAnalysisReturnAC;
                }
                catch (DuplicateDataException)
                {
                    transaction.Rollback();
                    throw;
                }
            }


        }

        /// <summary>
        /// Set isStrategicAnalysisDone true in auditable entity
        /// </summary>
        /// <param name="strategicAnalysisId">id of Strategic analysis of auditable entity which is to be updated</param>
        /// <param name="userId">responded user id</param>
        /// <returns>Void</returns>
        public async Task UpdateStrategicAnalysisDoneInAuditableEntityAsync(string strategicAnalysisId, string userId, string entityId)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    //StrategicAnalysis strategicAnalysis = await _dataRepository.FirstAsync<StrategicAnalysis>(x => x.Id.ToString() == strategicAnalysisId && !x.IsDeleted);

                    ////update status for stategic analysis from under review to final
                    //strategicAnalysis.Status = StrategicAnalysisStatus.Final;
                    //_dataRepository.Update<StrategicAnalysis>(strategicAnalysis);

                    //add filter for entityId
                    List<UserResponse> userResponse = _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId.ToString() == strategicAnalysisId
                    && x.UserId.ToString() == userId
                    && x.AuditableEntityId.ToString() == entityId
                    && !x.IsDeleted).AsNoTracking().ToList();

                    for (int i = 0; i < userResponse.Count; i++)
                    {
                        userResponse[i].UpdatedDateTime = DateTime.UtcNow;
                        userResponse[i].UpdatedBy = currentUserId;
                        userResponse[i].UserResponseStatus = StrategicAnalysisStatus.Final;

                    }

                    _dataRepository.UpdateRange(userResponse);

                    var auditableEntityToUpdate = await _dataRepository.FirstAsync<AuditableEntity>(x => x.Id == Guid.Parse(entityId) && !x.IsDeleted);

                    if (!auditableEntityToUpdate.IsStrategyAnalysisDone)
                    {
                        auditableEntityToUpdate.IsStrategyAnalysisDone = true;
                        auditableEntityToUpdate.UpdatedDateTime = DateTime.UtcNow;
                        auditableEntityToUpdate.UpdatedBy = currentUserId;

                        _dataRepository.Update<AuditableEntity>(auditableEntityToUpdate);
                        await _dataRepository.SaveChangesAsync();

                        //add entityid filter
                        var allTeamMembers = await _dataRepository.Where<StrategicAnalysisTeam>(x => x.StrategicAnalysisId.ToString() == strategicAnalysisId && !x.IsDeleted
                        && x.AuditableEntityId == Guid.Parse(entityId)).Select(x => x.User).ToListAsync();
                        // Add strategic analysis team members to audit team
                        var allTeamMembersIds = allTeamMembers.Select(x => x.Id).ToList();

                        // Add these users of strategic analysis team to Entity User Mapping table after isStrategicAnalysisDone is set to true 

                        var entityUserMappings = new List<EntityUserMapping>();
                        for (int i = 0; i < allTeamMembersIds.Count(); i++)
                        {
                            if (!await _dataRepository.AnyAsync<EntityUserMapping>(x => x.EntityId == auditableEntityToUpdate.Id && x.UserId == allTeamMembersIds[i]))
                            {
                                //map client participants with auditable entity
                                var entityUserMapping = new EntityUserMapping()
                                {
                                    EntityId = auditableEntityToUpdate.Id,
                                    UserId = allTeamMembersIds[i],
                                    CreatedDateTime = DateTime.UtcNow,
                                    CreatedBy = currentUserId
                                };
                                entityUserMappings.Add(entityUserMapping);
                            }
                            else
                            {
                                var auditMemberEmailId = allTeamMembers.First(x => x.Id == allTeamMembersIds[i]).EmailId;
                                throw new DuplicateDataException(StringConstant.EmailIdFieldName, auditMemberEmailId);
                            }
                        }

                        if (entityUserMappings.Count > 0)
                        {
                            await _dataRepository.AddRangeAsync<EntityUserMapping>(entityUserMappings);
                            await _dataRepository.SaveChangesAsync();
                        }
                    }



                    _dataRepository.CommitTransaction();

                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }


        }

        /// <summary>
        /// Set stretegic analysis active or not
        /// </summary>
        /// <param name="strategicAnalysisId">id of Strategic analysis </param>
        /// <returns>Task</returns>
        public async Task SetStrategicAnalysis(string strategicAnalysisId)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    StrategicAnalysis strategicAnalysis = await _dataRepository.FirstAsync<StrategicAnalysis>(x => x.Id.ToString() == strategicAnalysisId && !x.IsDeleted);
                    strategicAnalysis.IsActive = !strategicAnalysis.IsActive;
                    strategicAnalysis.UpdatedDateTime = DateTime.UtcNow;
                    strategicAnalysis.UpdatedBy = currentUserId;

                    _dataRepository.Update<StrategicAnalysis>(strategicAnalysis);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.CommitTransaction();

                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete from strategic analysis team
        /// </summary>
        /// <param name="teamMemberId">Strategic analysis team member id</param>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        public async Task DeleteStrategicAnalysisTeamMemberAsync(string teamMemberId, string strategicAnalysisId)
        {


            var teamMemberToBeDeleted = await _dataRepository.FirstAsync<StrategicAnalysisTeam>(x => x.Id.ToString() == teamMemberId && x.StrategicAnalysisId.ToString() == strategicAnalysisId && !x.IsDeleted);
            var isTeamMemberHasSubmittedResponse = await _dataRepository.AnyAsync<UserResponse>(x => x.UserId == teamMemberToBeDeleted.UserId && x.StrategicAnalysisId.ToString() == strategicAnalysisId
            && x.UserResponseStatus != StrategicAnalysisStatus.Draft
            && !x.IsDeleted);
            if (isTeamMemberHasSubmittedResponse)
            {
                //throw exception if one try to delete user who has submitted user response
                throw new DeleteLinkedDataException(StringConstant.UserResponsePresentMessage, StringConstant.StrategicAnalysisString);
            }
            else
            {
                var userResponseOfThisMember = await _dataRepository.FirstOrDefaultAsync<UserResponse>(x =>
                    x.StrategicAnalysisId.ToString() == strategicAnalysisId
                    && (x.UserResponseStatus == StrategicAnalysisStatus.UnderReview || x.UserResponseStatus == StrategicAnalysisStatus.Final)
                    && x.UserId.ToString() == teamMemberId
                    && !x.IsDeleted);
                if (userResponseOfThisMember == null)
                {
                    using (var transaction = _dataRepository.BeginTransaction())
                    {
                        try
                        {
                            _dataRepository.Remove(teamMemberToBeDeleted);
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
                else
                {
                    //throw exception if one try to delete super admin
                    throw new DeleteLinkedDataException(StringConstant.UserResponsePresentMessage, StringConstant.StrategicAnalysisString);
                }
            }
        }

        /// <summary>
        /// Delete Strategic Analysis/ Sampling 
        /// </summary>
        /// <param name="strategicAnalysisId">StrategicAnalysisId</param>
        /// <param name="isSampling">Determines if this strategic analysis is of sampling</param>
        /// <returns>Task</returns>
        public async Task DeleteStrategicAnalysisAsync(Guid strategicAnalysisId, bool isSampling)
        {

            //If this strategic analysis is referenced by other tables then user is not allowed to delete it, throw exception
            if (!isSampling && await CheckStrategicAnalysisReferenceAsync(strategicAnalysisId))
            {
                throw new DeleteLinkedDataException(StringConstant.StrategicAnalysisString, StringConstant.StrategicAnalysisString);
            }


            StrategicAnalysis strategicAnalysisToDelete = await _dataRepository.FindAsync<StrategicAnalysis>(strategicAnalysisId);

            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {
                    // Delete questions 

                    List<MultipleChoiceQuestion> multipleChoiceQuestions = new List<MultipleChoiceQuestion>();
                    List<RatingScaleQuestion> ratingScaleQuestions = new List<RatingScaleQuestion>();
                    List<SubjectiveQuestion> subjectiveQuestions = new List<SubjectiveQuestion>();
                    List<CheckboxQuestion> checkboxQuestions = new List<CheckboxQuestion>();
                    List<FileUploadQuestion> fileUploadQuestions = new List<FileUploadQuestion>();
                    List<TextboxQuestion> textboxQuestions = new List<TextboxQuestion>();
                    List<DropdownQuestion> dropdownQuestions = new List<DropdownQuestion>();


                    List<Question> existingQuestionList = await _dataRepository.Where<Question>(x => x.StrategyAnalysisId == strategicAnalysisId && !x.IsDeleted)
                        .Include(x => x.MultipleChoiceQuestion)
                        .Include(x => x.RatingScaleQuestion)
                        .Include(x => x.SubjectiveQuestion)
                        .Include(x => x.CheckboxQuestion)
                        .Include(x => x.FileUploadQuestion)
                        .Include(x => x.TextboxQuestion)
                        .Include(x => x.DropdownQuestion)
                        .AsNoTracking().ToListAsync();

                    if (existingQuestionList != null && existingQuestionList.Count() > 0)
                    {
                        for (var i = 0; i < existingQuestionList.Count(); i++)
                        {
                            // Delete question
                            existingQuestionList[i].IsDeleted = true;
                            existingQuestionList[i].UpdatedBy = currentUserId;
                            existingQuestionList[i].UpdatedDateTime = DateTime.UtcNow;

                            // Delete multiple choice question
                            if (existingQuestionList[i].MultipleChoiceQuestion != null)
                            {
                                MultipleChoiceQuestion multipleChoiceQuestion = new MultipleChoiceQuestion();
                                multipleChoiceQuestion = existingQuestionList[i].MultipleChoiceQuestion;
                                multipleChoiceQuestions.Add(multipleChoiceQuestion);
                                existingQuestionList[i].MultipleChoiceQuestion.IsDeleted = true;
                                existingQuestionList[i].MultipleChoiceQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].MultipleChoiceQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }

                            // Delete rating scale question
                            if (existingQuestionList[i].RatingScaleQuestion != null)
                            {
                                RatingScaleQuestion ratingScaleQuestion = new RatingScaleQuestion();
                                ratingScaleQuestion = existingQuestionList[i].RatingScaleQuestion;
                                ratingScaleQuestions.Add(ratingScaleQuestion);
                                existingQuestionList[i].RatingScaleQuestion.IsDeleted = true;
                                existingQuestionList[i].RatingScaleQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].RatingScaleQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }


                            // Delete subjective question
                            if (existingQuestionList[i].SubjectiveQuestion != null)
                            {
                                SubjectiveQuestion subjectiveQuestion = new SubjectiveQuestion();
                                subjectiveQuestion = existingQuestionList[i].SubjectiveQuestion;
                                subjectiveQuestions.Add(subjectiveQuestion);
                                existingQuestionList[i].SubjectiveQuestion.IsDeleted = true;
                                existingQuestionList[i].SubjectiveQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].SubjectiveQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }

                            // Delete checkbox question
                            if (existingQuestionList[i].CheckboxQuestion != null)
                            {
                                CheckboxQuestion checkboxQuestion = new CheckboxQuestion();
                                checkboxQuestion = existingQuestionList[i].CheckboxQuestion;
                                checkboxQuestions.Add(checkboxQuestion);
                                existingQuestionList[i].CheckboxQuestion.IsDeleted = true;
                                existingQuestionList[i].CheckboxQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].CheckboxQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }

                            // Delete file upload question
                            if (existingQuestionList[i].FileUploadQuestion != null)
                            {
                                FileUploadQuestion fileUploadQuestion = new FileUploadQuestion();
                                fileUploadQuestion = existingQuestionList[i].FileUploadQuestion;
                                fileUploadQuestions.Add(fileUploadQuestion);
                                existingQuestionList[i].FileUploadQuestion.IsDeleted = true;
                                existingQuestionList[i].FileUploadQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].FileUploadQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }

                            // Delete textbox question
                            if (existingQuestionList[i].TextboxQuestion != null)
                            {
                                TextboxQuestion textboxQuestion = new TextboxQuestion();
                                textboxQuestion = existingQuestionList[i].TextboxQuestion;
                                textboxQuestions.Add(textboxQuestion);
                                existingQuestionList[i].TextboxQuestion.IsDeleted = true;
                                existingQuestionList[i].TextboxQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].TextboxQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }

                            // Delete dropdown question
                            if (existingQuestionList[i].DropdownQuestion != null)
                            {
                                DropdownQuestion dropdownQuestion = new DropdownQuestion();
                                dropdownQuestion = existingQuestionList[i].DropdownQuestion;
                                dropdownQuestions.Add(dropdownQuestion);
                                existingQuestionList[i].DropdownQuestion.IsDeleted = true;
                                existingQuestionList[i].DropdownQuestion.UpdatedBy = currentUserId;
                                existingQuestionList[i].DropdownQuestion.UpdatedDateTime = DateTime.UtcNow;
                            }
                        }
                    }
                    _dataRepository.UpdateRange<Question>(existingQuestionList);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<MultipleChoiceQuestion>(multipleChoiceQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<RatingScaleQuestion>(ratingScaleQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<SubjectiveQuestion>(subjectiveQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<CheckboxQuestion>(checkboxQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<FileUploadQuestion>(fileUploadQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<TextboxQuestion>(textboxQuestions);
                    await _dataRepository.SaveChangesAsync();

                    _dataRepository.UpdateRange<DropdownQuestion>(dropdownQuestions);
                    await _dataRepository.SaveChangesAsync();

                    // Delete dropdown options
                    var existingDropdownIds = dropdownQuestions.Select(x => x.Id).ToList();
                    var existingDropdownOptions = await _dataRepository.Where<Option>(x => existingDropdownIds.Contains((Guid)x.DropdownQuestionId) && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingDropdownOptions != null && existingDropdownOptions.Count() > 0)
                    {
                        for (var i = 0; i < existingDropdownOptions.Count(); i++)
                        {
                            existingDropdownOptions[i].IsDeleted = true;
                            existingDropdownOptions[i].UpdatedBy = currentUserId;
                            existingDropdownOptions[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<Option>(existingDropdownOptions);
                    await _dataRepository.SaveChangesAsync();

                    // Delete multiple choice options
                    var existingMultipleIds = multipleChoiceQuestions.Select(x => x.Id).ToList();
                    var existingMultipleOptions = await _dataRepository.Where<Option>(x => existingMultipleIds.Contains((Guid)x.MultipleChoiceQuestionId) && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingMultipleOptions != null && existingMultipleOptions.Count() > 0)
                    {
                        for (var i = 0; i < existingMultipleOptions.Count(); i++)
                        {
                            existingMultipleOptions[i].IsDeleted = true;
                            existingMultipleOptions[i].UpdatedBy = currentUserId;
                            existingMultipleOptions[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<Option>(existingMultipleOptions);
                    await _dataRepository.SaveChangesAsync();

                    // Delete rating options
                    var existingRatingScaleIds = ratingScaleQuestions.Select(x => x.Id).ToList();
                    var existingRatingOptions = await _dataRepository.Where<Option>(x => existingRatingScaleIds.Contains((Guid)x.RatingQuestionId) && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingRatingOptions != null && existingRatingOptions.Count() > 0)
                    {
                        for (var i = 0; i < existingRatingOptions.Count(); i++)
                        {
                            existingRatingOptions[i].IsDeleted = true;
                            existingRatingOptions[i].UpdatedBy = currentUserId;
                            existingRatingOptions[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<Option>(existingRatingOptions);
                    await _dataRepository.SaveChangesAsync();

                    // Delete checkbox options
                    var existingCheckboxIds = checkboxQuestions.Select(x => x.Id).ToList();
                    var existingCheckboxOptions = await _dataRepository.Where<Option>(x => existingCheckboxIds.Contains((Guid)x.CheckboxQuestionId) && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingCheckboxOptions != null && existingCheckboxOptions.Count() > 0)
                    {
                        for (var i = 0; i < existingCheckboxOptions.Count(); i++)
                        {
                            existingCheckboxOptions[i].IsDeleted = true;
                            existingCheckboxOptions[i].UpdatedBy = currentUserId;
                            existingCheckboxOptions[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<Option>(existingCheckboxOptions);
                    await _dataRepository.SaveChangesAsync();


                    // Delete user response
                    List<UserResponse> existingUserResponseList = await _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId == strategicAnalysisId && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingUserResponseList != null && existingUserResponseList.Count() > 0)
                    {
                        for (var i = 0; i < existingUserResponseList.Count(); i++)
                        {
                            existingUserResponseList[i].IsDeleted = true;
                            existingUserResponseList[i].UpdatedBy = currentUserId;
                            existingUserResponseList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<UserResponse>(existingUserResponseList);
                    await _dataRepository.SaveChangesAsync();

                    // Delete User Response Document
                    var existingUserResponseIds = existingUserResponseList.Select(x => x.Id).ToList();
                    List<UserResponseDocument> existingUserResponseDocuments = await _dataRepository.Where<UserResponseDocument>(x => existingUserResponseIds.Contains(x.UserResponseId) && !x.IsDeleted).AsNoTracking().ToListAsync();
                    if (existingUserResponseDocuments != null && existingUserResponseDocuments.Count() > 0)
                    {
                        for (var i = 0; i < existingUserResponseDocuments.Count(); i++)
                        {
                            existingUserResponseDocuments[i].IsDeleted = true;
                            existingUserResponseDocuments[i].UpdatedBy = currentUserId;
                            existingUserResponseDocuments[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                    }
                    _dataRepository.UpdateRange<UserResponseDocument>(existingUserResponseDocuments);
                    await _dataRepository.SaveChangesAsync();

                    //Delete strategic analysis team
                    if (!isSampling)
                    {
                        var strategicAnalysisTeamUsers = await _dataRepository.Where<StrategicAnalysisTeam>
                                                                                    (x => x.StrategicAnalysisId == strategicAnalysisToDelete.Id
                                                                                    && x.UserId == currentUserId
                                                                                    && !x.IsDeleted).AsNoTracking().ToListAsync();
                        var strategicAnalysisTeamUsersList = new List<StrategicAnalysisTeam>();
                        for (int i = 0; i < strategicAnalysisTeamUsers.Count(); i++)
                        {
                            StrategicAnalysisTeam strategicAnalysisTeam = strategicAnalysisTeamUsers[i];
                            strategicAnalysisTeam.IsDeleted = true;
                            strategicAnalysisTeam.UpdatedBy = currentUserId;
                            strategicAnalysisTeam.UpdatedDateTime = DateTime.UtcNow;
                            strategicAnalysisTeamUsersList.Add(strategicAnalysisTeam);
                        }
                        _dataRepository.UpdateRange<StrategicAnalysisTeam>(strategicAnalysisTeamUsersList);
                        await _dataRepository.SaveChangesAsync();
                    }


                    //Delete strategic analysis
                    strategicAnalysisToDelete.IsDeleted = true;
                    strategicAnalysisToDelete.UpdatedBy = currentUserId;
                    strategicAnalysisToDelete.UpdatedDateTime = DateTime.UtcNow;
                    _dataRepository.Update(strategicAnalysisToDelete);
                    await _dataRepository.SaveChangesAsync();

                    transaction.Commit();
                }

                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }



        }
        #endregion

        #region CRUD for Question

        /// <summary>
        /// Get questions of strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisId">StrategicAnalysisId</param>
        /// <param name="riskControlMatrixId">riskcontrolmatric id</param>
        /// <returns>List of questionsAC</returns>
        public async Task<List<QuestionAC>> GetQuestionsAsync(string strategicAnalysisId, Guid? riskControlMatrixId = null)
        {
            List<Question> questionList = await _dataRepository.Where<Question>(a => !a.IsDeleted && a.StrategyAnalysisId.ToString() == strategicAnalysisId)
                .Include(x => x.DropdownQuestion)
                .Include(x => x.CheckboxQuestion)
                .Include(x => x.FileUploadQuestion)
                .Include(x => x.MultipleChoiceQuestion)
                .Include(x => x.RatingScaleQuestion)
                .Include(x => x.SubjectiveQuestion)
                .Include(x => x.TextboxQuestion)
                .OrderBy(x => x.SortOrder)
                .AsNoTracking().ToListAsync();
            //get all options

            List<QuestionAC> questionsAC = _mapper.Map<List<QuestionAC>>(questionList);
            var questionsIds = questionsAC.Select(x => x.Id).ToList();
            var dropdownQuestions = await _dataRepository.Where<DropdownQuestion>(x => questionsIds.Any(y => y == x.QuestionId)).AsNoTracking().ToListAsync();
            var dropdownQuestionIds = dropdownQuestions.Select(x => x.Id).ToList();
            var multipleChoiceQuestions = await _dataRepository.Where<MultipleChoiceQuestion>(x => questionsIds.Any(y => y == x.QuestionId)).AsNoTracking().ToListAsync();
            var multipleChoiceQuestionIds = multipleChoiceQuestions.Select(x => x.Id).ToList();
            var ratingQuestions = await _dataRepository.Where<RatingScaleQuestion>(x => questionsIds.Any(y => y == x.QuestionId)).AsNoTracking().ToListAsync();
            var ratingQuestionIds = ratingQuestions.Select(x => x.Id).ToList();
            var checkboxQuestions = await _dataRepository.Where<CheckboxQuestion>(x => questionsIds.Any(y => y == x.QuestionId)).AsNoTracking().ToListAsync();
            var checkboxQuestionIds = checkboxQuestions.Select(x => x.Id).ToList();


            List<Option> dropdownOptions = await _dataRepository.Where<Option>(x => dropdownQuestionIds.Contains((Guid)x.DropdownQuestionId) && !x.IsDeleted).Include(x => x.DropdownQuestion).AsNoTracking().ToListAsync();
            List<Option> multipleChoiceOptions = await _dataRepository.Where<Option>(x => multipleChoiceQuestionIds.Contains((Guid)x.MultipleChoiceQuestionId) && !x.IsDeleted).Include(x => x.MultipleChoiceQuestion).AsNoTracking().ToListAsync();
            List<Option> ratingOptions = await _dataRepository.Where<Option>(x => ratingQuestionIds.Contains((Guid)x.RatingQuestionId) && !x.IsDeleted).Include(x => x.RatingScaleQuestion).AsNoTracking().ToListAsync();
            List<Option> checkboxOptions = await _dataRepository.Where<Option>(x => checkboxQuestionIds.Contains((Guid)x.CheckboxQuestionId) && !x.IsDeleted).Include(x => x.CheckboxQuestionQuestion).AsNoTracking().ToListAsync();

            // Get all user responses of this question
            List<UserResponse> userResponses = new List<UserResponse>();
            var currentUser = await _userRepository.GetCurrentLoggedInUserDetailsById(currentUserId);

            if (currentUser.UserRole == UserRole.EngagementManager || currentUser.UserRole == UserRole.EngagementPartner)
            {
                // EP/EM user
                userResponses = await _dataRepository.Where<UserResponse>(x => questionsIds.Any(y => y == x.QuestionId) && !x.IsDeleted).AsNoTracking().ToListAsync();
            }
            else
            {
                // Team member user
                userResponses = await _dataRepository.Where<UserResponse>(x => questionsIds.Any(y => y == x.QuestionId) && x.UserId == currentUserId && !x.IsDeleted).AsNoTracking().ToListAsync();
            }

            if (riskControlMatrixId != null)
            {
                userResponses = userResponses.Where(x => x.RiskControlMatrixId == riskControlMatrixId).ToList();
            }
            List<UserResponseAC> userResponseACs = _mapper.Map<List<UserResponse>, List<UserResponseAC>>(userResponses);

            for (int i = 0; i < questionsAC.Count(); i++)
            {
                if (currentUser.UserRole == UserRole.EngagementManager || currentUser.UserRole == UserRole.EngagementPartner)
                {
                    // EP/EM user
                    var response = userResponseACs.FirstOrDefault(x => x.QuestionId == questionsAC[i].Id && x.UserResponseStatus != StrategicAnalysisStatus.Draft);
                    questionsAC[i].UserResponse = response == null ? userResponseACs.FirstOrDefault(x => x.QuestionId == questionsAC[i].Id && x.UserResponseStatus == StrategicAnalysisStatus.Draft && x.ParentFileUploadUserResponseId == null) : response;
                }
                else
                {
                    // Team member user
                    questionsAC[i].UserResponse = userResponseACs.FirstOrDefault(x => x.QuestionId == questionsAC[i].Id && x.ParentFileUploadUserResponseId == null);
                }

                questionsAC[i].IsUserResponseExists = true;

                if (questionsAC[i].Type == QuestionType.FileUpload)
                {
                    for (int j = 0; j < questionsIds.Count(); j++)
                    {
                        var listOfOptions = dropdownOptions.Where<Option>(x => (x.DropdownQuestion.QuestionId.ToString() == questionsIds[j].ToString()) && !x.IsDeleted)
                            .AsQueryable()
                            .AsNoTracking()
                            .Select(a => a.OptionText).ToList();

                        if (listOfOptions.Count() > 0)
                        {
                            var index = questionsAC.FindIndex(x => x.Id == questionsIds[j]);
                            questionsAC[index].Options = listOfOptions;
                        }
                    }

                }
                if (questionsAC[i].Type == QuestionType.Dropdown)
                {
                    for (int j = 0; j < questionsIds.Count(); j++)
                    {
                        var listOfOptions = dropdownOptions.Where<Option>(x => (x.DropdownQuestion.QuestionId.ToString() == questionsIds[j].ToString()) && !x.IsDeleted)
                            .AsQueryable()
                            .AsNoTracking()
                            .Select(a => a.OptionText).ToList();

                        if (listOfOptions.Count() > 0)
                        {
                            var index = questionsAC.FindIndex(x => x.Id == questionsIds[j]);
                            questionsAC[index].Options = listOfOptions;
                        }
                    }

                }

                if (questionsAC[i].Type == QuestionType.MultipleChoice)
                {
                    for (int j = 0; j < questionsIds.Count(); j++)
                    {
                        var listOfOptions = multipleChoiceOptions.Where<Option>(x => (x.MultipleChoiceQuestion.QuestionId.ToString() == questionsIds[j].ToString()) && !x.IsDeleted)
                            .AsQueryable()
                            .AsNoTracking()
                            .Select(a => a.OptionText).ToList();

                        if (listOfOptions.Count() > 0)
                        {
                            var index = questionsAC.FindIndex(x => x.Id == questionsIds[j]);
                            questionsAC[index].Options = listOfOptions;
                        }

                    }
                }

                if (questionsAC[i].Type == QuestionType.RatingScale)
                {
                    for (int j = 0; j < questionsIds.Count(); j++)
                    {
                        var listOfOptions = ratingOptions.Where<Option>(x => (x.RatingScaleQuestion.QuestionId.ToString() == questionsIds[j].ToString()) && !x.IsDeleted)
                            .AsQueryable()
                            .AsNoTracking()
                            .Select(a => a.OptionText).ToList();

                        if (listOfOptions.Count() > 0)
                        {
                            var index = questionsAC.FindIndex(x => x.Id == questionsIds[j]);
                            questionsAC[index].Options = listOfOptions;
                        }
                    }
                }

                if (questionsAC[i].Type == QuestionType.Checkbox)
                {
                    for (int j = 0; j < questionsIds.Count(); j++)
                    {
                        var listOfOptions = checkboxOptions.Where<Option>(x => (x.CheckboxQuestionQuestion.QuestionId.ToString() == questionsIds[j].ToString()) && !x.IsDeleted)
                            .AsQueryable()
                            .AsNoTracking()
                            .Select(a => a.OptionText).ToList();

                        if (listOfOptions.Count() > 0)
                        {
                            var index = questionsAC.FindIndex(x => x.Id == questionsIds[j]);
                            questionsAC[index].Options = listOfOptions;
                        }
                    }
                }
            }




            return questionsAC;
        }

        /// <summary>
        /// Get question by id
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns>Question to be returned</returns>
        public async Task<QuestionAC> GetQuestionById(string id)
        {
            List<Question> questions = await _dataRepository.Where<Question>(x => x.Id.ToString() == id)
                .Include(x => x.DropdownQuestion)
                .Include(x => x.CheckboxQuestion)
                .Include(x => x.FileUploadQuestion)
                .Include(x => x.MultipleChoiceQuestion)
                .Include(x => x.RatingScaleQuestion)
                .Include(x => x.SubjectiveQuestion)
                .Include(x => x.TextboxQuestion).AsNoTracking().ToListAsync();

            var isUserResponseExists = _dataRepository.Any<UserResponse>(x => x.QuestionId == questions[0].Id
            && x.UserResponseStatus != StrategicAnalysisStatus.Draft
            && !x.IsDeleted);

            QuestionAC questionAC = _mapper.Map<QuestionAC>(questions[0]);
            questionAC.IsUserResponseExists = isUserResponseExists;

            if (questionAC.Type == QuestionType.Dropdown)
            {
                var listOfOptions = _dataRepository.Where<Option>(x => x.DropdownQuestionId == questionAC.DropdownQuestion.Id && !x.IsDeleted)
                    .AsQueryable()
                    .AsNoTracking()
                    .Select(a => a.OptionText).ToList();
                questionAC.Options = listOfOptions;

            }
            if (questionAC.Type == QuestionType.Checkbox)
            {
                var listOfOptions = _dataRepository.Where<Option>(x => x.CheckboxQuestionId == questionAC.CheckboxQuestion.Id && !x.IsDeleted)
                    .AsQueryable()
                    .AsNoTracking()
                    .Select(a => a.OptionText).ToList();
                questionAC.Options = listOfOptions;

            }

            if (questionAC.Type == QuestionType.MultipleChoice)
            {
                var listOfOptions = _dataRepository.Where<Option>(x => x.MultipleChoiceQuestionId == questionAC.MultipleChoiceQuestion.Id && !x.IsDeleted)
                    .AsQueryable()
                    .AsNoTracking()
                    .Select(a => a.OptionText).ToList();
                questionAC.Options = listOfOptions;

            }
            if (questionAC.Type == QuestionType.RatingScale)
            {
                var listOfOptions = _dataRepository.Where<Option>(x => x.RatingQuestionId == questionAC.RatingScaleQuestion.Id && !x.IsDeleted)
                    .AsQueryable()
                    .AsNoTracking()
                    .Select(a => a.OptionText).ToList();
                questionAC.Options = listOfOptions;

            }

            return questionAC;
        }

        /// <summary>
        /// Add question to strategic analysis
        /// </summary>
        /// <param name="questionAC">QuestionAC</param>
        /// <param name="strategicAnalysisId">Strategic analysis Id</param>
        /// <returns>Added questionAC</returns>
        public async Task<QuestionAC> AddQuestionAsync(QuestionAC questionAC, Guid strategicAnalysisId)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    QuestionAC questionACToBeReturned = await CommonAddQuestionAsync(questionAC, strategicAnalysisId, String.Empty);
                    _dataRepository.CommitTransaction();
                    return questionACToBeReturned;
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Update question
        /// </summary>
        /// <param name="questionAC">questionAC</param>
        /// <returns>Updated questionAC</returns>
        public async Task<QuestionAC> UpdateQuestionAsync(QuestionAC questionAC)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {


                    Question updatedQuestion = _mapper.Map<Question>(questionAC);
                    updatedQuestion.UpdatedDateTime = DateTime.UtcNow;
                    updatedQuestion.UpdatedBy = currentUserId;

                    _dataRepository.Update(updatedQuestion);
                    await _dataRepository.SaveChangesAsync();

                    if (updatedQuestion.Type == QuestionType.Dropdown)
                    {
                        var dropdownQuestion = await _dataRepository.FirstAsync<DropdownQuestion>(x => x.Id.ToString() == questionAC.DropdownQuestion.Id.ToString() && !x.IsDeleted);

                        dropdownQuestion.Question = updatedQuestion;
                        dropdownQuestion.UpdatedDateTime = DateTime.UtcNow;
                        dropdownQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(dropdownQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Update options
                        var optionList = await _dataRepository.Where<Option>(x => x.DropdownQuestionId == dropdownQuestion.Id).AsNoTracking().ToListAsync();

                        for (int i = 0; i < optionList.Count(); i++)
                        {
                            optionList[i].IsDeleted = true;
                            optionList[i].UpdatedBy = currentUserId;
                            optionList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                        _dataRepository.UpdateRange<Option>(optionList);
                        await _dataRepository.SaveChangesAsync();

                        List<Option> options = new List<Option>();
                        for (int i = 0; i < questionAC.Options.Count(); i++)
                        {
                            Option option = new Option();
                            option.OptionText = questionAC.Options[i];
                            option.DropdownQuestionId = questionAC.DropdownQuestion.Id;
                            option.UpdatedDateTime = DateTime.UtcNow;
                            option.UpdatedBy = currentUserId;
                            options.Add(option);
                        }

                        await _dataRepository.AddRangeAsync<Option>(options);
                        await _dataRepository.SaveChangesAsync();
                    }


                    if (updatedQuestion.Type == QuestionType.MultipleChoice)
                    {
                        var mcqQuestion = await _dataRepository.FirstAsync<MultipleChoiceQuestion>(x => x.Id.ToString() == questionAC.MultipleChoiceQuestion.Id.ToString() && !x.IsDeleted);

                        mcqQuestion.Question = updatedQuestion;
                        mcqQuestion.UpdatedDateTime = DateTime.UtcNow;
                        mcqQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(mcqQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Update options
                        var optionList = await _dataRepository.Where<Option>(x => x.MultipleChoiceQuestionId == mcqQuestion.Id).AsNoTracking().ToListAsync();

                        for (int i = 0; i < optionList.Count(); i++)
                        {
                            optionList[i].IsDeleted = true;
                            optionList[i].UpdatedBy = currentUserId;
                            optionList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                        _dataRepository.UpdateRange<Option>(optionList);
                        await _dataRepository.SaveChangesAsync();

                        List<Option> options = new List<Option>();
                        for (int i = 0; i < questionAC.Options.Count(); i++)
                        {
                            Option option = new Option();
                            option.OptionText = questionAC.Options[i];
                            option.MultipleChoiceQuestionId = questionAC.MultipleChoiceQuestion.Id;
                            option.UpdatedDateTime = DateTime.UtcNow;
                            option.UpdatedBy = currentUserId;
                            options.Add(option);
                        }

                        await _dataRepository.AddRangeAsync<Option>(options);
                        await _dataRepository.SaveChangesAsync();
                    }

                    if (updatedQuestion.Type == QuestionType.Checkbox)
                    {
                        var checkboxQuestion = await _dataRepository.FirstAsync<CheckboxQuestion>(x => x.Id.ToString() == questionAC.CheckboxQuestion.Id.ToString() && !x.IsDeleted);

                        checkboxQuestion.Question = updatedQuestion;
                        checkboxQuestion.UpdatedDateTime = DateTime.UtcNow;
                        checkboxQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(checkboxQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Update options
                        var optionList = await _dataRepository.Where<Option>(x => x.CheckboxQuestionId == checkboxQuestion.Id).AsNoTracking().ToListAsync();

                        for (int i = 0; i < optionList.Count(); i++)
                        {
                            optionList[i].IsDeleted = true;
                            optionList[i].UpdatedBy = currentUserId;
                            optionList[i].UpdatedDateTime = DateTime.UtcNow;
                        }
                        _dataRepository.UpdateRange<Option>(optionList);
                        await _dataRepository.SaveChangesAsync();

                        List<Option> options = new List<Option>();
                        for (int i = 0; i < questionAC.Options.Count(); i++)
                        {
                            Option option = new Option();
                            option.OptionText = questionAC.Options[i];
                            option.CheckboxQuestionId = questionAC.CheckboxQuestion.Id;
                            option.UpdatedDateTime = DateTime.UtcNow;
                            option.UpdatedBy = currentUserId;
                            options.Add(option);
                        }

                        await _dataRepository.AddRangeAsync<Option>(options);
                        await _dataRepository.SaveChangesAsync();
                    }

                    if (updatedQuestion.Type == QuestionType.Subjective)
                    {
                        var subjectiveQuestion = await _dataRepository.FirstAsync<SubjectiveQuestion>(x => x.Id.ToString() == questionAC.SubjectiveQuestion.Id.ToString() && !x.IsDeleted);

                        subjectiveQuestion.Question = updatedQuestion;
                        subjectiveQuestion.UpdatedDateTime = DateTime.UtcNow;
                        subjectiveQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(subjectiveQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    if (updatedQuestion.Type == QuestionType.Textbox)
                    {
                        var textboxQuestion = await _dataRepository.FirstAsync<TextboxQuestion>(x => x.Id.ToString() == questionAC.TextboxQuestion.Id.ToString() && !x.IsDeleted);

                        textboxQuestion.Question = updatedQuestion;
                        textboxQuestion.UpdatedDateTime = DateTime.UtcNow;
                        textboxQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(textboxQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    if (updatedQuestion.Type == QuestionType.FileUpload)
                    {
                        var fileUploadQuestion = await _dataRepository.FirstAsync<FileUploadQuestion>(x => x.Id.ToString() == questionAC.FileUploadQuestion.Id.ToString() && !x.IsDeleted);

                        fileUploadQuestion.Question = updatedQuestion;
                        fileUploadQuestion.UpdatedDateTime = DateTime.UtcNow;
                        fileUploadQuestion.UpdatedBy = currentUserId;
                        _dataRepository.Update(fileUploadQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    _dataRepository.CommitTransaction();
                    QuestionAC returnQuestionAC = _mapper.Map<QuestionAC>(updatedQuestion);
                    returnQuestionAC.Options = questionAC.Options;
                    return returnQuestionAC;
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete question by id
        /// </summary>
        /// <param name="questionId">questionId</param>
        /// <returns></returns>
        public async Task DeleteQuestionByIdAsync(Guid questionId)
        {
            var isQuestionExistInResponse = await _dataRepository.AnyAsync<UserResponse>(x => !x.IsDeleted && x.QuestionId == questionId);
            if (isQuestionExistInResponse)
            {
                throw new DeleteLinkedDataException();
            }


            using (_dataRepository.BeginTransaction())
            {
                try
                {

                    List<Question> questions = await _dataRepository.Where<Question>(x => x.Id == questionId)
                    .Include(x => x.DropdownQuestion)
                    .Include(x => x.CheckboxQuestion)
                    .Include(x => x.FileUploadQuestion)
                    .Include(x => x.MultipleChoiceQuestion)
                    .Include(x => x.RatingScaleQuestion)
                    .Include(x => x.SubjectiveQuestion)
                    .Include(x => x.TextboxQuestion).AsNoTracking().ToListAsync();
                    Question questionToBeDeleted = questions[0];


                    // Delete question
                    questionToBeDeleted.IsDeleted = true;
                    questionToBeDeleted.UpdatedBy = currentUserId;
                    questionToBeDeleted.UpdatedDateTime = DateTime.UtcNow;

                    _dataRepository.Update(questionToBeDeleted);
                    await _dataRepository.SaveChangesAsync();

                    // Delete dropdown question
                    if (questionToBeDeleted.Type == QuestionType.Dropdown)
                    {
                        var dropdownQuestion = await _dataRepository.FirstAsync<DropdownQuestion>(x => x.Id == questionToBeDeleted.DropdownQuestion.Id && !x.IsDeleted);
                        dropdownQuestion.IsDeleted = true;
                        dropdownQuestion.UpdatedBy = currentUserId;
                        dropdownQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(dropdownQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Delete dropdown options
                        var existingDropdownOptions = await _dataRepository.Where<Option>(x => x.DropdownQuestionId == dropdownQuestion.Id && !x.IsDeleted).AsNoTracking().ToListAsync();
                        if (existingDropdownOptions != null && existingDropdownOptions.Count() > 0)
                        {
                            for (var i = 0; i < existingDropdownOptions.Count(); i++)
                            {
                                existingDropdownOptions[i].IsDeleted = true;
                                existingDropdownOptions[i].UpdatedBy = currentUserId;
                                existingDropdownOptions[i].UpdatedDateTime = DateTime.UtcNow;
                            }
                        }
                        _dataRepository.UpdateRange<Option>(existingDropdownOptions);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete MultipleChoice question
                    if (questionToBeDeleted.Type == QuestionType.MultipleChoice)
                    {
                        var mcqQuestion = await _dataRepository.FirstAsync<MultipleChoiceQuestion>(x => x.Id == questionToBeDeleted.MultipleChoiceQuestion.Id && !x.IsDeleted);
                        mcqQuestion.IsDeleted = true;
                        mcqQuestion.UpdatedBy = currentUserId;
                        mcqQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(mcqQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Delete dropdown options
                        var existingMCQOptions = await _dataRepository.Where<Option>(x => x.MultipleChoiceQuestionId == mcqQuestion.Id && !x.IsDeleted).AsNoTracking().ToListAsync();
                        if (existingMCQOptions != null && existingMCQOptions.Count() > 0)
                        {
                            for (var i = 0; i < existingMCQOptions.Count(); i++)
                            {
                                existingMCQOptions[i].IsDeleted = true;
                                existingMCQOptions[i].UpdatedBy = currentUserId;
                                existingMCQOptions[i].UpdatedDateTime = DateTime.UtcNow;
                            }
                        }
                        _dataRepository.UpdateRange<Option>(existingMCQOptions);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete rating scale question
                    if (questionToBeDeleted.Type == QuestionType.RatingScale)
                    {
                        var ratingScaleQuestion = await _dataRepository.FirstAsync<RatingScaleQuestion>(x => x.Id == questionToBeDeleted.RatingScaleQuestion.Id && !x.IsDeleted);
                        ratingScaleQuestion.IsDeleted = true;
                        ratingScaleQuestion.UpdatedBy = currentUserId;
                        ratingScaleQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(ratingScaleQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Delete rating scale options
                        var existingRatingScaleOptions = await _dataRepository.Where<Option>(x => x.RatingQuestionId == ratingScaleQuestion.Id && !x.IsDeleted).AsNoTracking().ToListAsync();
                        if (existingRatingScaleOptions != null && existingRatingScaleOptions.Count() > 0)
                        {
                            for (var i = 0; i < existingRatingScaleOptions.Count(); i++)
                            {
                                existingRatingScaleOptions[i].IsDeleted = true;
                                existingRatingScaleOptions[i].UpdatedBy = currentUserId;
                                existingRatingScaleOptions[i].UpdatedDateTime = DateTime.UtcNow;
                            }
                        }
                        _dataRepository.UpdateRange<Option>(existingRatingScaleOptions);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete checkbox question
                    if (questionToBeDeleted.Type == QuestionType.Checkbox)
                    {
                        var checkboxQuestion = await _dataRepository.FirstAsync<CheckboxQuestion>(x => x.Id == questionToBeDeleted.CheckboxQuestion.Id && !x.IsDeleted);
                        checkboxQuestion.IsDeleted = true;
                        checkboxQuestion.UpdatedBy = currentUserId;
                        checkboxQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(checkboxQuestion);
                        await _dataRepository.SaveChangesAsync();

                        // Delete checkbox options
                        var existingCheckboxOptions = await _dataRepository.Where<Option>(x => x.CheckboxQuestionId == checkboxQuestion.Id && !x.IsDeleted).AsNoTracking().ToListAsync();
                        if (existingCheckboxOptions != null && existingCheckboxOptions.Count() > 0)
                        {
                            for (var i = 0; i < existingCheckboxOptions.Count(); i++)
                            {
                                existingCheckboxOptions[i].IsDeleted = true;
                                existingCheckboxOptions[i].UpdatedBy = currentUserId;
                                existingCheckboxOptions[i].UpdatedDateTime = DateTime.UtcNow;
                            }
                        }
                        _dataRepository.UpdateRange<Option>(existingCheckboxOptions);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete subjective question
                    if (questionToBeDeleted.Type == QuestionType.Subjective)
                    {
                        var subjectiveQuestion = await _dataRepository.FirstAsync<SubjectiveQuestion>(x => x.Id == questionToBeDeleted.SubjectiveQuestion.Id && !x.IsDeleted);
                        subjectiveQuestion.IsDeleted = true;
                        subjectiveQuestion.UpdatedBy = currentUserId;
                        subjectiveQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(subjectiveQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete textbox question
                    if (questionToBeDeleted.Type == QuestionType.Textbox)
                    {
                        var textboxQuestion = await _dataRepository.FirstAsync<TextboxQuestion>(x => x.Id == questionToBeDeleted.TextboxQuestion.Id && !x.IsDeleted);
                        textboxQuestion.IsDeleted = true;
                        textboxQuestion.UpdatedBy = currentUserId;
                        textboxQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(textboxQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    // Delete file upload question
                    if (questionToBeDeleted.Type == QuestionType.FileUpload)
                    {
                        var fileUploadQuestion = await _dataRepository.FirstAsync<FileUploadQuestion>(x => x.Id == questionToBeDeleted.FileUploadQuestion.Id && !x.IsDeleted);
                        fileUploadQuestion.IsDeleted = true;
                        fileUploadQuestion.UpdatedBy = currentUserId;
                        fileUploadQuestion.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update(fileUploadQuestion);
                        await _dataRepository.SaveChangesAsync();
                    }

                    _dataRepository.CommitTransaction();
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }


        /// <summary>
        /// Update question orders
        /// </summary>
        /// <param name="questionAC">List of questionAC</param>
        /// <returns>task</returns>
        public async Task UpdateQuestionOrderAsync(List<QuestionAC> questionAC)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    List<Question> updatedQuestions = _mapper.Map<List<Question>>(questionAC);
                    for (int i = 0; i < updatedQuestions.Count; i++)
                    {

                        updatedQuestions[i].UpdatedDateTime = DateTime.UtcNow;
                        updatedQuestions[i].UpdatedBy = currentUserId;
                    }

                    _dataRepository.UpdateRange(updatedQuestions);
                    await _dataRepository.SaveChangesAsync();
                    _dataRepository.CommitTransaction();
                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }
            }
        }
        #endregion

        #region User Responses

        /// <summary>
        /// Get user response
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="userResponseId">userResponseId</param>
        /// <returns>List of userResponse</returns>
        public async Task<List<UserResponse>> GetUserResponseAsync(int? page, int pageSize, string searchString, Guid userResponseId)
        {
            var userResponseList = await _dataRepository.Where<UserResponse>(x => !x.IsDeleted && x.Id == userResponseId).AsNoTracking().ToListAsync();
            return userResponseList;
        }

        /// <summary>
        /// Add user response
        /// </summary>
        /// <param name="userResponseAC">userResponseAC</param>
        /// <returns>Added userResponseAC</returns>
        public async Task<UserResponseAC> AddUserResponseAsync(UserResponseAC userResponseAC)
        {
            UserResponse userResponse = _mapper.Map<UserResponseAC, UserResponse>(userResponseAC);
            userResponse.IsDeleted = false;
            userResponse.CreatedBy = currentUserId;
            userResponse.CreatedDateTime = DateTime.UtcNow;

            await _dataRepository.AddAsync<UserResponse>(userResponse);
            await _dataRepository.SaveChangesAsync();

            //if userResponseDoc contains fileupload
            //Upload file in azure blob storage using file utility
            if (userResponseAC.Files.Count() > 0)
            {
                var filesUrl = await _fileUtility.UploadFilesAsync(userResponseAC.Files);

                //Create entry for user response document to be added in UserResponseDocument table
                List<UserResponseDocument> userResponseDocuments = new List<UserResponseDocument>();

                int i = 0;
                for (i = 0; i < filesUrl.Count(); i++)
                {
                    UserResponseDocument userResponseDocument = new UserResponseDocument();
                    userResponseDocument.UpdatedDateTime = DateTime.UtcNow;
                    userResponseDocument.Path = filesUrl[i];
                    userResponseDocument.UserResponseId = (Guid)userResponse.Id;
                    userResponseDocument.UserResponse = userResponse;
                    userResponseDocuments.Add(userResponseDocument);
                }

                //add urls in userresponsedocument table
                await _azureRepo.AddUrlsInDocumentStorageAsync<UserResponseDocument>(userResponseDocuments);
            }

            return userResponseAC;
        }

        /// <summary>
        /// Delete document from db and from azure
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>Void</returns>
        public async Task DeleteDocumentAsync(string id)
        {
            using (var transaction = _dataRepository.BeginTransaction())
            {
                try
                {

                    UserResponseDocument userResponseDoc = await _dataRepository.FirstAsync<UserResponseDocument>(x => x.Id.ToString() == id);
                    UserResponse userResponse = await _dataRepository.FirstAsync<UserResponse>(x => x.Id == userResponseDoc.UserResponseId);

                    if (await _fileUtility.DeleteFileAsync(userResponseDoc.Path))//Delete file from azure
                    {
                        userResponse.IsDeleted = false;
                        userResponse.UpdatedBy = currentUserId;
                        userResponse.UpdatedDateTime = DateTime.UtcNow;
                        _dataRepository.Update<UserResponse>(userResponse);
                        await _dataRepository.SaveChangesAsync();

                        userResponseDoc.IsDeleted = true;
                        userResponseDoc.UpdatedBy = currentUserId;
                        userResponseDoc.UpdatedDateTime = DateTime.UtcNow;

                        _dataRepository.Update<UserResponseDocument>(userResponseDoc);
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
        /// Method to download document
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns>Download url string</returns>
        public async Task<string> DownloadDocumentAsync(Guid id)
        {
            UserResponseDocument doc = await _dataRepository.FirstAsync<UserResponseDocument>(x => x.Id == id && !x.IsDeleted);
            return _fileUtility.DownloadFile(doc.Path);
        }

        /// <summary>
        /// Get sampling response rcm wise
        /// </summary>
        /// <param name="samplingId">Id of the sampling</param>
        /// <param name="rcmId">Id of the rcm</param>
        /// <returns>List of rcm response</returns>
        public async Task<List<UserWiseResponseAC>> GetRcmWiseResponseAsync(string samplingId, string rcmId)
        {
            List<UserWiseResponseAC> listOfRcmWiseResponseList = new List<UserWiseResponseAC>();
            UserWiseResponseAC rcmWiseReponse = new UserWiseResponseAC();
            List<UserResponseDocumentAC> allUsersDocumentList = new List<UserResponseDocumentAC>();

            List<UserResponseDocument> userResponseDocuments = await _dataRepository.Where<UserResponseDocument>(x => !x.IsDeleted)
                                                                                                       .Include(x => x.UserResponse)
                                                                                                       .Where(x => x.UserResponse.StrategicAnalysisId.ToString() == samplingId)
                                                                                                       .AsNoTracking().ToListAsync();



            allUsersDocumentList = _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(userResponseDocuments);

            // StrategicAnalysis strategicAnalysis = await _dataRepository.FirstAsync<StrategicAnalysis>(x => x.Id.ToString() == strategicAnalysisId && !x.IsDeleted);

            List<UserResponse> questionResponses = await _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId.ToString() == samplingId
                                                                                        && x.RiskControlMatrixId.ToString() == rcmId
                                                                                        // Searching on basis of client's name
                                                                                        && x.ParentFileUploadUserResponseId == null
                                                                                        && x.QuestionId != null
                                                                                        && !x.IsDeleted)
                                                                                            .AsNoTracking()
                                                                                            .Include(x => x.Question.DropdownQuestion)
                                                                                            .Include(x => x.Question.CheckboxQuestion)
                                                                                            .Include(x => x.Question.FileUploadQuestion)
                                                                                            .Include(x => x.Question.MultipleChoiceQuestion)
                                                                                            .Include(x => x.Question.RatingScaleQuestion)
                                                                                            .Include(x => x.Question.SubjectiveQuestion)
                                                                                            .Include(x => x.Question.TextboxQuestion)
                                                                                            .Include(x => x.Options)
                                                                                            .Include(x => x.User)
                                                                                            .AsNoTracking().ToListAsync();

            // get question responses 
            List<UserResponseAC> questionReponsesList = _mapper.Map<List<UserResponse>, List<UserResponseAC>>(questionResponses);

            //var uniqueUserIds = userId == null ? questionResponses.Select(x => x.UserId).Distinct().ToList() : userIdList.Select(e => new Guid?(e)).ToList();


            // Add file type question's user response document
            for (var j = 0; j < questionReponsesList.Count(); j++)
            {
                if (questionReponsesList[j].Question != null)
                {
                    if (questionReponsesList[j].Question.Type == QuestionType.FileUpload)
                    {
                        questionReponsesList[j].Question.FileResponseList = questionReponsesList.Where<UserResponseAC>(x => x.QuestionId == questionReponsesList[j].QuestionId).ToList();
                        var fileResponseIds = questionReponsesList[j].Question.FileResponseList.Select(x => x.Id).ToList();
                        questionReponsesList[j].Question.UserResponseDocumentACs = allUsersDocumentList.Where<UserResponseDocumentAC>(x => fileResponseIds.Contains(x.UserResponseId)).ToList();
                        questionReponsesList[j].Question.FileResponseList = null;
                    }
                }
            }
            rcmWiseReponse.ListOfUserResponseWithQuestion = questionReponsesList.Where<UserResponseAC>(x => x.RiskControlMatrixId.ToString() == rcmId && !x.IsEmailAttachements).ToList();
            // rcmWiseReponse.UserName = questionResponses[0].RiskControlMatrix.de

            //set order by user response 
            for (int i = 0; i < questionReponsesList.Count; i++)
            {
                List<UserResponseAC> listOfUserResponseWithQuestion = new List<UserResponseAC>();
                List<QuestionAC> questions = new List<QuestionAC>();
                for (int j = 0; j < questionReponsesList.Count; j++)
                {
                    listOfUserResponseWithQuestion = rcmWiseReponse.ListOfUserResponseWithQuestion;
                    questions.Add(rcmWiseReponse.ListOfUserResponseWithQuestion[j].Question);
                }
                questions = questions.OrderBy(a => a.SortOrder).ToList();
                rcmWiseReponse.ListOfUserResponseWithQuestion = new List<UserResponseAC>();
                UserResponseAC userResponseWithQuestion = new UserResponseAC();
                for (int k = 0; k < questions.Count; k++)
                {
                    rcmWiseReponse.ListOfUserResponseWithQuestion.Add(listOfUserResponseWithQuestion.FirstOrDefault(a => a.QuestionId == questions[k].Id));

                }
            }

            listOfRcmWiseResponseList.Add(rcmWiseReponse);

            return listOfRcmWiseResponseList;

        }

        /// <summary>
        /// Get user wise response of a strategic analysis
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <param name="strategicAnalysisId">Strategic analysis id whose userwise response is to be fetched</param>
        /// <param name="userId">List of user wise response</param>
        /// <param name="isSampling">bit to check is from sampling or not</param>
        /// <param name="entityId">entity id</param>
        /// <returns>Pagination containing list of user wise responses</returns>
        public async Task<Pagination<UserWiseResponseAC>> GetUserWiseResponseAsync(Pagination<UserWiseResponseAC> pagination, string strategicAnalysisId, string userId, string rcmId, bool isSampling, Guid? entityId)
        {
            try
            {
                // Apply pagination
                int skippedRecords = _globalRepository.GetSkipRecordsCount(pagination.PageIndex, pagination.PageSize);

                if (pagination.searchText == null)
                {
                    pagination.searchText = String.Empty;
                }
                var userIdList = new List<Guid>();
                List<UserWiseResponseAC> listOfAllUsersResponses = new List<UserWiseResponseAC>();

                #region All documents of users of a strategic analysis
                List<UserResponseDocumentAC> allUsersDocumentList = new List<UserResponseDocumentAC>();
                // if not from sampling module
                if (!isSampling)
                {
                    List<UserResponseDocument> userResponseDocuments = await _dataRepository.Where<UserResponseDocument>(x => !x.IsDeleted)
                                                                                                        .Include(x => x.UserResponse)
                                                                                                        .Where(x => x.UserResponse.StrategicAnalysisId.ToString() == strategicAnalysisId)
                                                                                                        .AsNoTracking().ToListAsync();



                    allUsersDocumentList = _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(userResponseDocuments);
                }
                #endregion


                if (userId == null)
                {
                    #region Get only list page display data 

                    // get response of estimated value 
                    List<UserResponse> alluserResponsesList = await _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId.ToString() == strategicAnalysisId
                                                                                                            && x.User.Name.ToLower().Contains(pagination.searchText.ToLower())
                                                                                                            && !x.IsDeleted).Include(x => x.User).AsNoTracking().ToListAsync();

                    List<AuditableEntity> auditableEntities = _dataRepository.Where<AuditableEntity>(a => !a.IsDeleted).ToList();

                    if (!isSampling)
                    {
                        //var uniqueUserIdList = alluserResponsesList.Select(x => x.UserId).Distinct().ToList();
                        var uniqueUserIdList = alluserResponsesList.GroupBy(x => new { x.UserId, x.AuditableEntityId })
                      .Select(x => new UserResponseAC { UserId = x.Key.UserId, AuditableEntityId = x.Key.AuditableEntityId }).ToList();

                        for (var i = 0; i < uniqueUserIdList.Count(); i++)
                        {
                            // check if user has submitted questions
                            if (alluserResponsesList.Any(x => x.UserId == uniqueUserIdList[i].UserId && x.UserResponseStatus != StrategicAnalysisStatus.Draft && x.QuestionId != null))
                            {
                                UserWiseResponseAC userWiseResponseAC = new UserWiseResponseAC();
                                userWiseResponseAC.UserId = uniqueUserIdList[i].UserId.ToString();
                                userWiseResponseAC.UserName = alluserResponsesList.FirstOrDefault(x => x.UserId.ToString() == uniqueUserIdList[i].UserId.ToString())?.User.Name;
                                userWiseResponseAC.IsReponseDrafted = alluserResponsesList.Any(x => x.QuestionId != null && x.UserId.ToString() == userWiseResponseAC.UserId
                                && x.AuditableEntityId == uniqueUserIdList[i].AuditableEntityId && x.UserResponseStatus == StrategicAnalysisStatus.Draft);
                                userWiseResponseAC.AuditableEntityName = auditableEntities.FirstOrDefault(a => a.Id == uniqueUserIdList[i].AuditableEntityId).Name;
                                userWiseResponseAC.AuditableEntityId = auditableEntities.FirstOrDefault(a => a.Id == uniqueUserIdList[i].AuditableEntityId).Id;
                                // need optimization
                                userWiseResponseAC.StrategicAnalysis = new StrategicAnalysisAC();
                                userWiseResponseAC.StrategicAnalysis.Id = Guid.Parse(strategicAnalysisId);
                                #region Get general page & email attachment details
                                // if not from sampling module
                                if (!isSampling)
                                {
                                    var userReponseForGeneralPage = alluserResponsesList.FirstOrDefault<UserResponse>
                                                                    (x => x.IsDetailAndEstimatedValueOfOpportunity
                                                                    && x.UserId == uniqueUserIdList[i].UserId
                                                                    && x.AuditableEntityId == uniqueUserIdList[i].AuditableEntityId && !x.IsDeleted);
                                    if (uniqueUserIdList[i].UserId != currentUserId)
                                    {
                                        if (userReponseForGeneralPage != null)
                                        {
                                            userWiseResponseAC.StrategicAnalysis.UserResponseForDetailsOfOppAndEstimatedValue = new UserResponseForDetailsAndEstimatedValueOfOpportunity()
                                            {
                                                EstimatedValueOfOpportunity = userReponseForGeneralPage.EstimatedValueOfOpportunity,
                                                DetailsOfOpportunity = userReponseForGeneralPage.DetailsOfOpportunity
                                            };
                                            userWiseResponseAC.StrategicAnalysis.Status = StrategicAnalysisStatus.Final;
                                        }
                                    }
                                    // add email attachments to strategic analysis
                                    var emailResponseIdList = alluserResponsesList.Where(x => x.IsEmailAttachements && x.UserId == uniqueUserIdList[i].UserId && x.AuditableEntityId == uniqueUserIdList[i].AuditableEntityId).Select(x => x.Id).ToList();
                                    userWiseResponseAC.StrategicAnalysis.UserResponseDocumentACs = allUsersDocumentList.Where(x => emailResponseIdList.Contains(x.UserResponseId)
                                                                                                                                                           && !x.IsDeleted).ToList();
                                }
                                #endregion
                                if (alluserResponsesList.Count(x => x.QuestionId != null) > 0)
                                {
                                    if (!userWiseResponseAC.IsReponseDrafted)
                                    {
                                        listOfAllUsersResponses.Add(userWiseResponseAC);
                                    }
                                }


                            }

                        }

                    }
                    else
                    {
                        // unique rcm id 
                        List<Guid?> uniqueRcmIdList = alluserResponsesList.Select(x => x.RiskControlMatrixId).Distinct().ToList();
                        var rcmDetails = _dataRepository.Where<RiskControlMatrix>(x => uniqueRcmIdList.Contains(x.Id))
                                                        .Select(x => new RiskControlMatrix { Id = x.Id, RiskDescription = x.RiskDescription }).AsNoTracking().ToList();
                        for (var i = 0; i < uniqueRcmIdList.Count(); i++)
                        {
                            UserWiseResponseAC rcmWiseReponse = new UserWiseResponseAC();
                            var userIdOfCurrentRcmReponse = alluserResponsesList.FirstOrDefault(x => x.RiskControlMatrixId == uniqueRcmIdList[i]);
                            if (userIdOfCurrentRcmReponse != null)
                            {
                                rcmWiseReponse.UserName = userIdOfCurrentRcmReponse.User.Name;
                                rcmWiseReponse.StrategicAnalysis = new StrategicAnalysisAC();
                                rcmWiseReponse.StrategicAnalysis.Id = Guid.Parse(strategicAnalysisId);
                                rcmWiseReponse.StrategicAnalysis.RcmId = uniqueRcmIdList[i].ToString();
                                rcmWiseReponse.RcmDescription = rcmDetails.First(x => x.Id == uniqueRcmIdList[i]).RiskDescription;
                                listOfAllUsersResponses.Add(rcmWiseReponse);
                            }

                        }
                    }


                    pagination.TotalRecords = listOfAllUsersResponses.Count();
                    listOfAllUsersResponses = listOfAllUsersResponses.Skip(skippedRecords).Take(pagination.PageSize).ToList();
                    #endregion
                }
                else
                {
                    UserWiseResponseAC userWiseResponseAC = new UserWiseResponseAC();

                    // StrategicAnalysis strategicAnalysis = await _dataRepository.FirstAsync<StrategicAnalysis>(x => x.Id.ToString() == strategicAnalysisId && !x.IsDeleted);

                    List<UserResponse> questionResponses = await _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId.ToString() == strategicAnalysisId
                                                                                                // Searching on basis of client's name
                                                                                                && x.UserId.ToString() == userId
                                                                                                && x.AuditableEntityId == entityId
                                                                                                && x.ParentFileUploadUserResponseId == null
                                                                                                && x.QuestionId != null
                                                                                                && !x.IsDeleted)
                                                                                                    .AsNoTracking()
                                                                                                    .Include(x => x.Question.DropdownQuestion)
                                                                                                    .Include(x => x.Question.CheckboxQuestion)
                                                                                                    .Include(x => x.Question.FileUploadQuestion)
                                                                                                    .Include(x => x.Question.MultipleChoiceQuestion)
                                                                                                    .Include(x => x.Question.RatingScaleQuestion)
                                                                                                    .Include(x => x.Question.SubjectiveQuestion)
                                                                                                    .Include(x => x.Question.TextboxQuestion)
                                                                                                    .Include(x => x.Options)
                                                                                                    .Include(x => x.User)
                                                                                                    .AsNoTracking().ToListAsync();

                    // get question responses 
                    List<UserResponseAC> questionReponsesList = _mapper.Map<List<UserResponse>, List<UserResponseAC>>(questionResponses);

                    //var uniqueUserIds = userId == null ? questionResponses.Select(x => x.UserId).Distinct().ToList() : userIdList.Select(e => new Guid?(e)).ToList();


                    // Add file type question's user response document
                    for (var j = 0; j < questionReponsesList.Count(); j++)
                    {
                        if (questionReponsesList[j].Question != null)
                        {
                            if (questionReponsesList[j].Question.Type == QuestionType.FileUpload)
                            {
                                questionReponsesList[j].Question.FileResponseList = questionReponsesList.Where<UserResponseAC>(x => x.QuestionId == questionReponsesList[j].QuestionId).ToList();
                                var fileResponseIds = questionReponsesList[j].Question.FileResponseList.Select(x => x.Id).ToList();
                                questionReponsesList[j].Question.UserResponseDocumentACs = allUsersDocumentList.Where<UserResponseDocumentAC>(x =>
                                //fileResponseIds.Contains(x.UserResponseId)
                                //&& 
                                x.CreatedBy.ToString() == userId
                                && fileResponseIds.Contains(x.UserResponseId)).ToList();
                                questionReponsesList[j].Question.FileResponseList = null;
                            }
                        }
                    }
                    userWiseResponseAC.ListOfUserResponseWithQuestion = questionReponsesList.Where<UserResponseAC>(x => x.UserId.ToString() == userId && !x.IsEmailAttachements).ToList();

                    //set order by user response 
                    for (int i = 0; i < questionReponsesList.Count; i++)
                    {
                        List<UserResponseAC> listOfUserResponseWithQuestion = new List<UserResponseAC>();
                        List<QuestionAC> questions = new List<QuestionAC>();
                        for (int j = 0; j < questionReponsesList.Count; j++)
                        {
                            listOfUserResponseWithQuestion = userWiseResponseAC.ListOfUserResponseWithQuestion;
                            questions.Add(userWiseResponseAC.ListOfUserResponseWithQuestion[j].Question);
                        }
                        questions = questions.OrderBy(a => a.SortOrder).ToList();
                        userWiseResponseAC.ListOfUserResponseWithQuestion = new List<UserResponseAC>();
                        UserResponseAC userResponseWithQuestion = new UserResponseAC();
                        for (int k = 0; k < questions.Count; k++)
                        {
                            userWiseResponseAC.ListOfUserResponseWithQuestion.Add(listOfUserResponseWithQuestion.FirstOrDefault(a => a.QuestionId == questions[k].Id));

                        }
                    }

                    listOfAllUsersResponses.Add(userWiseResponseAC);

                }



                pagination.Items = listOfAllUsersResponses;
                return pagination;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Add and upload question's files
        /// </summary>
        /// <param name="userResponseAC">User response</param>
        /// <returns>List of user response documents</returns>
        public async Task<List<UserResponseDocumentAC>> AddAndUploadQuestionFilesAsync(UserResponseAC userResponseAC)
        {

            List<UserResponse> addedUserResponseList = new List<UserResponse>();
            for (int i = 0; i < userResponseAC.Files.Count(); i++)
            {
                UserResponse addedUserResponse = new UserResponse();
                // Add user response to which userResponseDocument is to be connected
                UserResponse userResponse = new UserResponse();
                userResponse.UserId = currentUserId;
                userResponse.CreatedBy = currentUserId;
                userResponse.CreatedDateTime = DateTime.UtcNow;
                userResponse.ParentFileUploadUserResponseId = userResponseAC.Id;
                userResponse.StrategicAnalysisId = (Guid)userResponseAC.StrategicAnalysisId;
                userResponse.QuestionId = (Guid)userResponseAC.QuestionId;
                addedUserResponseList.Add(userResponse);
            }
            if (addedUserResponseList.Count > 0)
            {
                await _dataRepository.AddRangeAsync(addedUserResponseList);
                await _dataRepository.SaveChangesAsync();
            }


            List<string> filesUrl = new List<string>();
            //validation
            bool isValidFormate = _globalRepository.CheckFileExtention(userResponseAC.Files);

            if (isValidFormate)
            {
                throw new InvalidFileFormate();
            }

            bool isFileSizeValid = _globalRepository.CheckFileSize(userResponseAC.Files);

            if (isFileSizeValid)
            {
                throw new InvalidFileSize();
            }

            //Upload file to azure
            filesUrl = await _fileUtility.UploadFilesAsync(userResponseAC.Files);

            List<UserResponseDocument> addedFileList = new List<UserResponseDocument>();
            for (int i = 0; i < filesUrl.Count(); i++)
            {
                UserResponseDocument newDocument = new UserResponseDocument()
                {
                    Id = new Guid(),
                    Path = filesUrl[i],
                    UserResponseId = addedUserResponseList[i].Id,
                    CreatedBy = currentUserId,
                    CreatedDateTime = DateTime.UtcNow
                };
                addedFileList.Add(newDocument);
            }
            await _dataRepository.AddRangeAsync(addedFileList);
            await _dataRepository.SaveChangesAsync();
            List<UserResponseDocumentAC> docACList = _mapper.Map<List<UserResponseDocumentAC>>(addedFileList);

            var allUserResponsesIdsOfThisQuestion = await _dataRepository.Where<UserResponse>(x => x.QuestionId == userResponseAC.QuestionId
            && !x.IsEmailAttachements
            && x.UserId == currentUserId
            && !x.IsDeleted).Select(x => x.Id).ToListAsync();
            var allUserDocsOfThisQuestion = await _dataRepository.Where<UserResponseDocument>(x => allUserResponsesIdsOfThisQuestion.Contains(x.UserResponseId)
            && !x.IsDeleted).AsNoTracking().ToListAsync();

            docACList = _mapper.Map<List<UserResponseDocumentAC>>(allUserDocsOfThisQuestion);

            for (int i = 0; i < docACList.Count(); i++)
            {
                docACList[i].FileName = docACList[i].Path;
                docACList[i].Path = _azureRepo.DownloadFile(docACList[i].Path);
            }
            return docACList.OrderByDescending(x => x.CreatedDateTime).ToList();
        }

        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="strategicId">Id of strategy</param>
        /// <param name="entityId">Entity Id </param>
        /// <param name="offset">offset of client</param>
        /// <param name="emailId">Current user email id</param>
        /// <returns>Data of file in memory stream</returns>
        public async Task<MemoryStream> DownloadStrategicPDFAsync(string strategicId, Guid entityId, int offset, string emailId)
        {
            StrategicAnalysis strategicAnalysis = await _dataRepository.Where<StrategicAnalysis>(x => x.Id == Guid.Parse(strategicId) && !x.IsDeleted)
                  .Include(w => w.AuditableEntity)
                  .AsNoTracking().FirstAsync();

            List<User> userDetails = _dataRepository.Where<User>(a => !a.IsDeleted).ToList();
            User userDetail = userDetails.FirstOrDefault(a => a.EmailId.ToLower() == emailId.ToLower());



            StrategicAnalysisAC strategicAnalysisAC = _mapper.Map<StrategicAnalysisAC>(strategicAnalysis);

            List<StrategicAnalysisTeam> teams = await _dataRepository.Where<StrategicAnalysisTeam>(a => a.StrategicAnalysisId.ToString() == strategicId
                                                && a.AuditableEntityId == entityId && !a.IsDeleted).Include(x => x.User).AsNoTracking().ToListAsync();

            strategicAnalysisAC.InternalUserList = _mapper.Map<List<StrategicAnalysisTeamAC>>(teams);

            strategicAnalysisAC.Questions = await this.GetQuestionsAsync(strategicId);
            var userResponse = await _dataRepository.FirstOrDefaultAsync<UserResponse>(x =>
                         x.StrategicAnalysisId == Guid.Parse(strategicId)
                         && x.IsDetailAndEstimatedValueOfOpportunity
                         && x.AuditableEntityId == entityId && !x.IsDeleted);
            strategicAnalysisAC.DetailsOfOpportunity = userResponse.DetailsOfOpportunity;
            strategicAnalysisAC.EstimatedValueOfOpportunity = userResponse.EstimatedValueOfOpportunity;
            strategicAnalysisAC.AuditableEntityName = _dataRepository.FirstOrDefault<AuditableEntity>(a => a.Id == entityId).Name;

            for (int i = 0; i < strategicAnalysisAC.Questions.Count; i++)
            {
                strategicAnalysisAC.Questions[i].QuestionType = strategicAnalysisAC.Questions[i].Type.ToString();
            }
            #region Question Response

            List<UserResponseDocumentAC> allUsersDocumentList = new List<UserResponseDocumentAC>();

            List<UserResponseDocument> userResponseDocuments = await _dataRepository.Where<UserResponseDocument>(x => !x.IsDeleted)
                                                                                                       .Include(x => x.UserResponse)
                                                                                                       .Where(x => x.UserResponse.StrategicAnalysisId.ToString() == strategicId
                                                                                                       && x.UserResponse.AuditableEntityId == entityId)
                                                                                                       .AsNoTracking().ToListAsync();



            allUsersDocumentList = _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(userResponseDocuments);


            UserWiseResponseAC userWiseResponseAC = new UserWiseResponseAC();

            List<UserResponse> questionResponses = await _dataRepository.Where<UserResponse>(x => x.StrategicAnalysisId.ToString() == strategicId
                                                                                        // Searching on basis of client's name
                                                                                        && x.UserId.ToString() == userDetail.Id.ToString()
                                                                                        && x.AuditableEntityId == entityId
                                                                                        && x.ParentFileUploadUserResponseId == null
                                                                                        && x.QuestionId != null
                                                                                        && !x.IsDeleted)
                                                                                            .AsNoTracking()
                                                                                            .Include(x => x.Question.DropdownQuestion)
                                                                                            .Include(x => x.Question.CheckboxQuestion)
                                                                                            .Include(x => x.Question.FileUploadQuestion)
                                                                                            .Include(x => x.Question.MultipleChoiceQuestion)
                                                                                            .Include(x => x.Question.RatingScaleQuestion)
                                                                                            .Include(x => x.Question.SubjectiveQuestion)
                                                                                            .Include(x => x.Question.TextboxQuestion)
                                                                                            .Include(x => x.Options)
                                                                                            .Include(x => x.User)
                                                                                            .AsNoTracking().ToListAsync();

            // get question responses 
            List<UserResponseAC> questionReponsesList = _mapper.Map<List<UserResponse>, List<UserResponseAC>>(questionResponses);

            // Add file type question's user response document
            for (var j = 0; j < questionReponsesList.Count(); j++)
            {
                if (questionReponsesList[j].Question != null)
                {
                    if (questionReponsesList[j].Question.Type == QuestionType.FileUpload)
                    {
                        questionReponsesList[j].Question.FileResponseList = questionReponsesList.Where<UserResponseAC>(x => x.QuestionId == questionReponsesList[j].QuestionId).ToList();
                        var fileResponseIds = questionReponsesList[j].Question.FileResponseList.Select(x => x.Id).ToList();
                        questionReponsesList[j].Question.UserResponseDocumentACs = allUsersDocumentList.Where<UserResponseDocumentAC>(x =>
                        x.CreatedBy.ToString() == userDetail.Id.ToString()
                        && fileResponseIds.Contains(x.UserResponseId)).ToList();
                        questionReponsesList[j].Question.FileResponseList = null;
                    }
                }
            }
            userWiseResponseAC.ListOfUserResponseWithQuestion = questionReponsesList.Where<UserResponseAC>(x => x.UserId.ToString() == userDetail.Id.ToString() && !x.IsEmailAttachements).ToList();

            //set order by user response 
            for (int i = 0; i < questionReponsesList.Count; i++)
            {
                List<UserResponseAC> listOfUserResponseWithQuestion = new List<UserResponseAC>();
                List<QuestionAC> questions = new List<QuestionAC>();
                for (int j = 0; j < questionReponsesList.Count; j++)
                {
                    listOfUserResponseWithQuestion = userWiseResponseAC.ListOfUserResponseWithQuestion;
                    questions.Add(userWiseResponseAC.ListOfUserResponseWithQuestion[j].Question);
                }
                questions = questions.OrderBy(a => a.SortOrder).ToList();
                userWiseResponseAC.ListOfUserResponseWithQuestion = new List<UserResponseAC>();
                UserResponseAC userResponseWithQuestion = new UserResponseAC();
                for (int k = 0; k < questions.Count; k++)
                {
                    userWiseResponseAC.ListOfUserResponseWithQuestion.Add(listOfUserResponseWithQuestion.FirstOrDefault(a => a.QuestionId == questions[k].Id));

                }
            }


            for (int i = 0; i < userWiseResponseAC.ListOfUserResponseWithQuestion.Count; i++)
            {
                userWiseResponseAC.ListOfUserResponseWithQuestion[i].Question.QuestionType = userWiseResponseAC.ListOfUserResponseWithQuestion[i].Question.Type.ToString();
            }

            strategicAnalysisAC.UserResponseList = userWiseResponseAC;
            #endregion

            var result = await _viewRenderService.RenderToStringAsync(StringConstant.StrategyPdfFilePath, strategicAnalysisAC, StringConstant.StrategyPdfFileName + strategicId);
            var headerResult = await _viewRenderService.RenderToStringAsync(StringConstant.StrategyHeaderPdfFilePath, strategicAnalysisAC, StringConstant.StrategyHeaderPdfFileName + strategicId);
            strategicAnalysisAC.PdfFileString = result;

            var requestUrl = _iConfig.GetValue<string>("PdfGenerationRequestUrl");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var jsonContent = JsonConvert.SerializeObject(new { html = strategicAnalysisAC.PdfFileString, pdfOptions = new { headerTemplate = headerResult, footerTemplate = " ", displayHeaderFooter = true, margin = new { top = "100px", bottom = "100px", right = "30px", left = "30px" } } });
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync(requestUrl, httpContent);

            var memoryStream = new MemoryStream();

            await httpResponse.Content.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Check strategic analysis exists or not
        /// </summary>
        /// <param name="surveyTitle">Survey title of strategic analysis</param>
        /// <param name="entityId">Auditable entity id related to strategic analysis</param>
        /// <param name="strategicAnalysisId">Strategic Analsysis Id</param>
        /// <returns>Boolean value saying if strategic analysis with sent param exists or not</returns>
        private bool CheckStrategicAnalysisExist(string surveyTitle, string entityId, string strategicAnalysisId, double version)
        {
            bool isStrategicAnalysisExists = false;

            if (strategicAnalysisId != null && strategicAnalysisId != String.Empty)
            {
                isStrategicAnalysisExists = _dataRepository.Any<StrategicAnalysis>(a => a.SurveyTitle.ToLower().Equals(surveyTitle.ToLower())
               && !a.IsDeleted && a.Id.ToString() != strategicAnalysisId
               && a.CreatedBy == currentUserId
               && a.Version == version);
            }
            else
            {
                isStrategicAnalysisExists = _dataRepository.Any<StrategicAnalysis>(a => a.SurveyTitle.ToLower().Equals(surveyTitle.ToLower())
                && a.CreatedBy == currentUserId
                && !a.IsDeleted && a.Version == version);

            }

            return isStrategicAnalysisExists;
        }

        /// <summary>
        /// Check if version exist or not
        /// </summary>
        /// <param name="surveyTitle">Survey title of strategic analysis</param>
        /// <param name="entityId">Auditable entity id related to strategic analysis</param>
        /// <param name="version">Strategic Analysis Version</param>
        /// <returns> true or false depending on the value</returns>
        private bool CheckIfVersionExists(string surveyTitle, string entityId, double version)
        {
            bool isStrategicAnalysisExists = false;
            isStrategicAnalysisExists = _dataRepository.Any<StrategicAnalysis>(a => a.SurveyTitle.ToLower().Equals(surveyTitle.ToLower())
               && !a.IsDeleted && a.Version == version);
            return isStrategicAnalysisExists;
        }
        /// <summary>
        /// Check if strategic analysis is refered by any other dependent tables
        /// </summary>
        /// <param name="strategicAnalysisId">strategicAnalysisId</param>
        /// <returns>Boolean value saying if strategic analysis is dependents on other tables or not</returns>
        private async Task<bool> CheckStrategicAnalysisReferenceAsync(Guid strategicAnalysisId)
        {
            bool isStrategicAnalysisReferenced = false, isRCMExists, isAuditableEntityExists;
            StrategicAnalysis strategicAnalysis = await _dataRepository.FindAsync<StrategicAnalysis>(strategicAnalysisId);

            isRCMExists = await _dataRepository.AnyAsync<RiskControlMatrix>(x => x.StrategicAnalysisId == strategicAnalysisId && !x.IsDeleted);
            if (!isRCMExists)
            {
                isAuditableEntityExists = false;
                if (isAuditableEntityExists)
                    isStrategicAnalysisReferenced = true;
            }
            else
                isStrategicAnalysisReferenced = true;

            return isStrategicAnalysisReferenced;
        }

        /// <summary>
        /// Check if question exist
        /// </summary>
        /// <param name="questionText">Question text of question to be added</param>
        /// <param name="questionId">If it is in edit page then this id of question will be there else in add form it will be null</param>
        /// <returns></returns>
        private bool CheckIfQuestionTextExistAsync(string questionText, Guid? questionId = null)
        {
            bool isQuestionTextExists;
            if (questionId != null)
            {
                isQuestionTextExists = _dataRepository.Any<Question>(x => x.Id != questionId && !x.IsDeleted && x.QuestionText.ToLower() == questionText.ToLower());
            }
            else
            {
                isQuestionTextExists = _dataRepository.Any<Question>(x => x.QuestionText.ToLower() == questionText.ToLower() && !x.IsDeleted);
            }
            return isQuestionTextExists;
        }

        /// <summary>
        /// Add question in strategic analysis
        /// </summary>
        /// <param name="questionAC">Question to be added</param>
        /// <param name="strategicAnalysisId">Strategic Analysis Id</param>
        /// <returns>returnQuestionAC</returns>
        private async Task<QuestionAC> CommonAddQuestionAsync(QuestionAC questionAC, Guid strategicAnalysisId, string oldStrategicAnalysisId)
        {
            try
            {
                Question question = new Question();

                questionAC.Id = Guid.NewGuid();

                Question questionAdded = _mapper.Map<QuestionAC, Question>(questionAC);
                questionAdded.StrategyAnalysisId = strategicAnalysisId;
                questionAdded.CreatedBy = currentUserId;
                questionAdded.CreatedDateTime = DateTime.UtcNow;

                questionAdded.DropdownQuestion = null;
                questionAdded.MultipleChoiceQuestion = null;
                questionAdded.RatingScaleQuestion = null;
                questionAdded.SubjectiveQuestion = null;
                questionAdded.TextboxQuestion = null;
                questionAdded.FileUploadQuestion = null;
                questionAdded.CheckboxQuestion = null;

                await _dataRepository.AddAsync(questionAdded);
                await _dataRepository.SaveChangesAsync();

                if (questionAdded.Type == QuestionType.Dropdown)
                {
                    var dropdownQuestion = _mapper.Map<DropdownQuestionAC, DropdownQuestion>(questionAC.DropdownQuestion);
                    dropdownQuestion.Id = Guid.NewGuid();
                    dropdownQuestion.QuestionId = questionAdded.Id;
                    dropdownQuestion.CreatedBy = currentUserId;
                    dropdownQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(dropdownQuestion);
                    await _dataRepository.SaveChangesAsync();

                    // Add options
                    List<Option> options = new List<Option>();
                    for (int i = 0; i < questionAC.Options.Count(); i++)
                    {
                        Option option = new Option();
                        option.DropdownQuestionId = dropdownQuestion.Id;
                        option.OptionText = questionAC.Options[i];
                        option.CreatedBy = currentUserId;
                        option.CreatedDateTime = DateTime.UtcNow;
                        options.Add(option);

                    }
                    await _dataRepository.AddRangeAsync(options);
                    await _dataRepository.SaveChangesAsync();
                }

                if (questionAdded.Type == QuestionType.MultipleChoice)
                {
                    var mcqQuestion = _mapper.Map<MultipleChoiceQuestionAC, MultipleChoiceQuestion>(questionAC.MultipleChoiceQuestion);
                    mcqQuestion.Id = Guid.NewGuid();
                    mcqQuestion.QuestionId = questionAdded.Id;
                    mcqQuestion.CreatedBy = currentUserId;
                    mcqQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(mcqQuestion);
                    await _dataRepository.SaveChangesAsync();

                    // Add options
                    List<Option> options = new List<Option>();
                    for (int i = 0; i < questionAC.Options.Count(); i++)
                    {
                        Option option = new Option();
                        option.MultipleChoiceQuestionId = mcqQuestion.Id;
                        option.OptionText = questionAC.Options[i];
                        option.CreatedBy = currentUserId;
                        option.CreatedDateTime = DateTime.UtcNow;
                        options.Add(option);
                    }
                    await _dataRepository.AddRangeAsync(options);
                    await _dataRepository.SaveChangesAsync();
                }

                if (questionAdded.Type == QuestionType.RatingScale)
                {
                    var ratingScaleQuestion = _mapper.Map<RatingScaleQuestionAC, RatingScaleQuestion>(questionAC.RatingScaleQuestion);
                    ratingScaleQuestion.Id = Guid.NewGuid();
                    ratingScaleQuestion.QuestionId = questionAdded.Id;
                    ratingScaleQuestion.CreatedBy = currentUserId;
                    ratingScaleQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(ratingScaleQuestion);
                    await _dataRepository.SaveChangesAsync();

                }

                if (questionAdded.Type == QuestionType.Subjective)
                {
                    var subjectiveQuestion = _mapper.Map<SubjectiveQuestionAC, SubjectiveQuestion>(questionAC.SubjectiveQuestion);
                    subjectiveQuestion.Id = Guid.NewGuid();
                    subjectiveQuestion.QuestionId = questionAdded.Id;
                    subjectiveQuestion.CreatedBy = currentUserId;
                    subjectiveQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(subjectiveQuestion);
                    await _dataRepository.SaveChangesAsync();
                }

                if (questionAdded.Type == QuestionType.Textbox)
                {
                    var textboxQuestion = _mapper.Map<TextboxQuestionAC, TextboxQuestion>(questionAC.TextboxQuestion);
                    textboxQuestion.Id = Guid.NewGuid();
                    textboxQuestion.QuestionId = questionAdded.Id;
                    textboxQuestion.CreatedBy = currentUserId;
                    textboxQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(textboxQuestion);
                    await _dataRepository.SaveChangesAsync();
                }

                if (questionAdded.Type == QuestionType.FileUpload)
                {
                    var fileUploadQuestion = _mapper.Map<FileUploadQuestionAC, FileUploadQuestion>(questionAC.FileUploadQuestion);
                    fileUploadQuestion.Id = Guid.NewGuid();
                    fileUploadQuestion.QuestionId = questionAdded.Id;
                    fileUploadQuestion.CreatedBy = currentUserId;
                    fileUploadQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(fileUploadQuestion);
                    await _dataRepository.SaveChangesAsync();
                }

                if (questionAdded.Type == QuestionType.Checkbox)
                {
                    var checkboxQuestion = _mapper.Map<CheckboxQuestionAC, CheckboxQuestion>(questionAC.CheckboxQuestion);
                    checkboxQuestion.Id = Guid.NewGuid();
                    checkboxQuestion.QuestionId = questionAdded.Id;
                    checkboxQuestion.CreatedBy = currentUserId;
                    checkboxQuestion.CreatedDateTime = DateTime.UtcNow;
                    await _dataRepository.AddAsync(checkboxQuestion);
                    await _dataRepository.SaveChangesAsync();

                    // Add options
                    List<Option> options = new List<Option>();
                    for (int i = 0; i < questionAC.Options.Count(); i++)
                    {
                        Option option = new Option();
                        option.CheckboxQuestionId = checkboxQuestion.Id;
                        option.OptionText = questionAC.Options[i];
                        option.CreatedBy = currentUserId;
                        option.CreatedDateTime = DateTime.UtcNow;
                        options.Add(option);
                    }
                    await _dataRepository.AddRangeAsync(options);
                    await _dataRepository.SaveChangesAsync();
                }


                List<Guid> strategicAnalysisTeamMembersUserIds = new List<Guid>();

                if (oldStrategicAnalysisId.ToString() == String.Empty)
                {
                    strategicAnalysisTeamMembersUserIds = await _dataRepository.Where<StrategicAnalysisTeam>(x => x.StrategicAnalysisId == strategicAnalysisId && !x.IsDeleted).AsNoTracking().Select(x => x.UserId).ToListAsync();

                }
                else
                {
                    strategicAnalysisTeamMembersUserIds = await _dataRepository.Where<StrategicAnalysisTeam>(x => x.StrategicAnalysisId.ToString() == oldStrategicAnalysisId && !x.IsDeleted).AsNoTracking().Select(x => x.UserId).ToListAsync();

                }

                //List<UserResponse> userResponsesToAdd = new List<UserResponse>();
                //for (int i = 0; i < strategicAnalysisTeamMembersUserIds.Count(); i++)
                //{
                //    UserResponse userResponse = new UserResponse()
                //    {
                //        QuestionId = questionAdded.Id,
                //        AnswerText = String.Empty,
                //        Other = String.Empty,
                //        RepresentationNumber = 0,
                //        SelectedDropdownOption = String.Empty,
                //        StrategicAnalysisId = strategicAnalysisId,
                //        IsEmailAttachements = false,
                //        UserResponseStatus = StrategicAnalysisStatus.Draft,
                //        SamplingResponseStatus = SamplingResponseStatus.Draft,
                //        CreatedBy = currentUserId,
                //        CreatedDateTime = DateTime.UtcNow
                //    };
                //    userResponse.UserId = strategicAnalysisTeamMembersUserIds[i];
                //    userResponsesToAdd.Add(userResponse);
                //}
                //// For sampling - as it does not contain any team member
                //if (strategicAnalysisTeamMembersUserIds.Count() == 0)
                //{
                //    UserResponse userResponse = new UserResponse()
                //    {
                //        QuestionId = questionAdded.Id,
                //        AnswerText = String.Empty,
                //        Other = String.Empty,
                //        RepresentationNumber = 0,
                //        SelectedDropdownOption = String.Empty,
                //        StrategicAnalysisId = strategicAnalysisId,
                //        IsEmailAttachements = false,
                //        UserResponseStatus = StrategicAnalysisStatus.Draft,
                //        SamplingResponseStatus = SamplingResponseStatus.Draft,
                //        CreatedBy = currentUserId,
                //        CreatedDateTime = DateTime.UtcNow,
                //        UserId = currentUserId
                //    };
                //    userResponsesToAdd.Add(userResponse);
                //}

                //await _dataRepository.AddRangeAsync<UserResponse>(userResponsesToAdd);
                //await _dataRepository.SaveChangesAsync();


                QuestionAC returnQuestionAC = _mapper.Map<QuestionAC>(questionAdded);
                returnQuestionAC.Options = questionAC.Options;

                //UserResponse userResponseToBeAppended = userResponsesToAdd.Find(x => x.QuestionId == returnQuestionAC.Id
                //&& x.UserId == currentUserId
                //&& x.UserResponseStatus == StrategicAnalysisStatus.Draft
                //&& !x.IsDeleted);

                //returnQuestionAC.UserResponse = _mapper.Map<UserResponseAC>(userResponseToBeAppended);
                return returnQuestionAC;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add and upload strategic analysis files
        /// </summary>
        /// <param name="strategicAnalysisFiles">Strategic analysis files</param>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <param name="currentUserId">Current user id</param>
        /// <param name="auditableEntityId">auditable Entity id</param>
        /// <returns>List of user response documents</returns>
        private async Task<List<UserResponseDocumentAC>> AddAndUploadStrategicAnalysisFiles(List<IFormFile> strategicAnalysisFiles, Guid strategicAnalysisId, Guid currentUserId, Guid? userRespondId, bool isEmailAttachments, Guid auditableEntityId)
        {
            using (_dataRepository.BeginTransaction())
            {
                try
                {
                    #region File validation
                    //validation 
                    int existingFileCount = _dataRepository.Where<UserResponseDocument>(a => !a.IsDeleted)
                                                            .Include(x => x.UserResponse)
                                                            .Where(x => x.UserResponse.StrategicAnalysisId == strategicAnalysisId
                                                            && x.UserResponse.AuditableEntityId == auditableEntityId).Count();
                    int totalFiles = existingFileCount + strategicAnalysisFiles.Count;

                    // if file limit exceed
                    if (totalFiles >= StringConstant.FileLimit)
                    {
                        throw new InvalidFileCount();
                    }

                    // if proper file format not uploaded
                    if (_globalRepository.CheckFileExtention(strategicAnalysisFiles))
                    {
                        throw new InvalidFileFormate();
                    }
                    #endregion

                    UserResponse addedUserResponse = new UserResponse();
                    if (userRespondId == null)
                    {
                        // Add user response to which userResponseDocument is to be connected
                        UserResponse userResponse = new UserResponse();
                        userResponse.UserId = currentUserId;
                        userResponse.CreatedBy = currentUserId;
                        userResponse.CreatedDateTime = DateTime.UtcNow;
                        userResponse.StrategicAnalysisId = strategicAnalysisId;
                        userResponse.IsEmailAttachements = isEmailAttachments;
                        userResponse.UserResponseStatus = isEmailAttachments ? StrategicAnalysisStatus.UnderReview : userResponse.UserResponseStatus;
                        userResponse.AuditableEntityId = auditableEntityId;
                        userResponse.AuditableEntity = null;
                        addedUserResponse = await _dataRepository.AddAsync(userResponse);
                        await _dataRepository.SaveChangesAsync();
                    }

                    List<string> filesUrl = new List<string>();
                    var emailAttachments = new List<IFormFile>();
                    strategicAnalysisFiles.ForEach(f =>
                    {
                        emailAttachments.Add(f);
                    });


                    //Upload file to azure
                    filesUrl = await _fileUtility.UploadFilesAsync(emailAttachments);

                    List<UserResponseDocument> addedFileList = new List<UserResponseDocument>();
                    for (int i = 0; i < filesUrl.Count(); i++)
                    {
                        UserResponseDocument newDocument = new UserResponseDocument()
                        {
                            Id = new Guid(),
                            Path = filesUrl[i],
                            UserResponseId = userRespondId == null ? addedUserResponse.Id : (Guid)userRespondId,
                            CreatedBy = currentUserId,
                            CreatedDateTime = DateTime.UtcNow
                        };
                        addedFileList.Add(newDocument);
                    }
                    await _dataRepository.AddRangeAsync(addedFileList);
                    await _dataRepository.SaveChangesAsync();
                    _dataRepository.CommitTransaction();
                    return _mapper.Map<List<UserResponseDocument>, List<UserResponseDocumentAC>>(addedFileList);

                }
                catch (Exception)
                {
                    _dataRepository.RollbackTransaction();
                    throw;
                }

            }
        }

        #endregion

        #region User Survey
        /// <summary>
        /// Get all strategic analysis for dropdown in user side
        /// </summary>
        /// <param name="userName">Logged in user name</param>
        /// <returns>list of strategic analysis</returns>
        public async Task<List<StrategicAnalysisAC>> GetAllActiveStrategicAnalysisAsync(string userName)
        {
            try
            {
                //var internalUserList = await _dataRepository.Where<StrategicAnalysisTeam>(x => x.StrategicAnalysisId == strategicAnalysisAC.Id && !x.IsDeleted).
                //Include(x => x.User).AsNoTracking().ToListAsync();

                List<StrategicAnalysis> listOfStraticDbData = _dataRepository.Where<StrategicAnalysis>(x => !x.IsDeleted && !x.IsSampling && x.IsActive == true)
                                                                                .Include(x => x.UserCreatedBy)
                                                                                .OrderByDescending(o => o.CreatedDateTime).ToList();
                List<StrategicAnalysisAC> allStrategicAnalysisList = _mapper.Map<List<StrategicAnalysis>, List<StrategicAnalysisAC>>(listOfStraticDbData);
                List<User> userDetails = _dataRepository.Where<User>(a => !a.IsDeleted).ToList();
                User userDetail = userDetails.FirstOrDefault(a => a.EmailId.ToLower() == userName.ToLower());


                UserAC loggedInUser = _mapper.Map<UserAC>(userDetail);
                for (int i = 0; i < allStrategicAnalysisList.Count; i++)
                {
                    //added logged in user
                    StrategicAnalysisTeamAC survayCreatedBy = new StrategicAnalysisTeamAC();
                    survayCreatedBy.StrategicAnalysisId = allStrategicAnalysisList[i].Id;
                    survayCreatedBy.UserId = allStrategicAnalysisList[i].CreatedBy;
                    survayCreatedBy.Name = allStrategicAnalysisList[i].UserName;
                    survayCreatedBy.Designation = allStrategicAnalysisList[i].Designation;
                    survayCreatedBy.EmailId = allStrategicAnalysisList[i].Email;
                    allStrategicAnalysisList[i].InternalUserList = new List<StrategicAnalysisTeamAC>();

                    StrategicAnalysisTeamAC teamUser = new StrategicAnalysisTeamAC();
                    teamUser.StrategicAnalysisId = allStrategicAnalysisList[i].Id;
                    teamUser.UserId = loggedInUser.Id;
                    teamUser.Name = loggedInUser.Name;
                    teamUser.Designation = loggedInUser.Designation;
                    teamUser.EmailId = loggedInUser.EmailId;

                    allStrategicAnalysisList[i].InternalUserList = new List<StrategicAnalysisTeamAC>();
                    allStrategicAnalysisList[i].InternalUserList.Add(survayCreatedBy);
                    allStrategicAnalysisList[i].InternalUserList.Add(teamUser);

                }
                return allStrategicAnalysisList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
    }
}
