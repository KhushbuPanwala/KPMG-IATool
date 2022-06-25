using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository
{
    public interface IStrategicAnalysisRepository
    {
        #region CRUD for Strategic Analysis

        /// <summary>
        /// Get sampling response rcm wise
        /// </summary>
        /// <param name="samplingId">Id of the sampling</param>
        /// <param name="rcmId">Id of the rcm</param>
        /// <returns>List of rcm response</returns>
        Task<List<UserWiseResponseAC>> GetRcmWiseResponseAsync(string samplingId, string rcmId);

        /// <summary>
        /// Get sampling response of admin side
        /// </summary>
        /// <param name="pagination">Pagination data</param>
        /// <returns>List of sampling data</returns>
        Task<Pagination<StrategicAnalysisAC>> GetAllAdminSideSamplingsDataAsync(Pagination<StrategicAnalysisAC> pagination);

        /// <summary>
        /// Get all strategic analysis
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <param name="isSampling">Determines if this strategic analysis is of sampling</param>
        /// <param name="rcmId">Rcm Id for sampling list</param>
        /// <param name="isCallFromAdmin">is call from admin or user</param>
        /// <param name="isCallFromAdmin">is call from admin or user</param>
        /// <returns>Pagination containing list of strategic analysis</returns>
        Task<Pagination<StrategicAnalysisAC>> GetAllStrategicAnalysisAsync(Pagination<StrategicAnalysisAC> pagination, bool isSampling = false, string rcmId = null, bool isCallFromAdmin = true);

        /// <summary>
        /// Get strategic analyses count
        /// </summary>
        /// <param name="searchString">Search value</param>
        /// <returns>Count of strategic analyses</returns>
        Task<int> GetStrategicAnalysesCountAsync(string searchString = null);

        /// <summary>
        /// Get strategic analysis by id
        /// </summary>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <param name="riskControlMatrixId">riskcontrolmatric id</param>
        /// <param name="isGeneralPage">Bit to check whether to get general page data or not/param>
        /// <param name="entityId">selected entity id</param>
        /// <returns>StrategicAnalysisAC</returns>
        Task<StrategicAnalysisAC> GetStrategicAnalysisById(string strategicAnalysisId, Guid? riskControlMatrixId = null, bool isGeneralPage = false, string entityId = "");

        /// <summary>
        /// Get strategicAnalysisId of draft status
        /// </summary>
        /// <returns>First strategic analysis id of strategic analysis whose status is draft</returns>
        Task<string> GetStrategicAnalysisIdOfDraftStatusAsync();

        /// <summary>
        /// Get user responses count
        /// </summary>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>User response count</returns>
        Task<int> GetUserResponsesCountAsync(string strategicAnalysisId);

        /// <summary>
        /// Add strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">strategicAnalysisAC</param>
        /// <returns>strategicAnalysisAC</returns>
        Task<StrategicAnalysisAC> AddStrategicAnalysisAsync(StrategicAnalysisAC strategicAnalysisAC);

        /// <summary>
        /// Update Strategic Analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">StrategicAnalysisAC</param>
        /// <returns>Updated strategicAnalysisAC</returns>
        Task<StrategicAnalysisAC> UpdateStrategicAnalysisAsync(StrategicAnalysisAC strategicAnalysisAC);

        /// <summary>
        /// Set isStrategicAnalysisDone true in auditable entity
        /// </summary>
        /// <param name="strategicAnalysisId">id of Strategic analysis of auditable entity which is to be updated</param>
        /// <param name="userId">responded user id</param>
        /// <param name="entityId">selected entity id</param>
        /// <returns>Void</returns>
        Task UpdateStrategicAnalysisDoneInAuditableEntityAsync(string strategicAnalysisId, string userId, string entityId);

        /// <summary>
        /// set strategic analysis by active or inactive
        /// </summary>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>Task</returns>
        Task SetStrategicAnalysis(string strategicAnalysisId);


        /// <summary>
        /// Delete from strategic analysis team
        /// </summary>
        /// <param name="teamMemberId">Strategic analysis team member id</param>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteStrategicAnalysisTeamMemberAsync(string teamMemberId, string strategicAnalysisId);

        /// <summary>
        /// Delete Strategic Analysis
        /// </summary>
        /// <param name="strategicAnalysisId">StrategicAnalysisId</param>
        /// <param name="isSampling">Determines if this strategic analysis is of sampling</param>
        /// <returns></returns>
        Task DeleteStrategicAnalysisAsync(Guid strategicAnalysisId, bool isSampling = false);
        #endregion

        #region CRUD for Question

        /// <summary>
        /// Get question by id
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns>Question to be returned</returns>
        Task<QuestionAC> GetQuestionById(string id);

        /// <summary>
        /// Get questions of strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisId">StrategicAnalysisId</param>
        /// <param name="riskControlMatrixId">riskcontrolmatric id</param>
        /// <returns>List of questionsAC</returns>
        Task<List<QuestionAC>> GetQuestionsAsync(string strategicAnalysisId, Guid? riskControlMatrixId = null);

        /// <summary>
        /// Add question to strategic analysis
        /// </summary>
        /// <param name="questionAC">QuestionAC</param>
        /// <param name="strategicAnalysisId">Strategic analysis Id</param>
        /// <returns>Added questionAC</returns>
        Task<QuestionAC> AddQuestionAsync(QuestionAC questionAC, Guid strategicAnalysisId);

        /// <summary>
        /// Update question
        /// </summary>
        /// <param name="questionAC">questionAC</param>
        /// <returns>Updated questionAC</returns>
        Task<QuestionAC> UpdateQuestionAsync(QuestionAC questionAC);

        /// <summary>
        /// Delete question by id
        /// </summary>
        /// <param name="questionId">questionId</param>
        /// <returns></returns>
        Task DeleteQuestionByIdAsync(Guid questionId);

        /// <summary>
        /// Update question order to strategic analysis
        /// </summary>
        /// <param name="questionAC">List of QuestionAC</param>
        /// <returns>task</returns>
        Task UpdateQuestionOrderAsync(List<QuestionAC> questionAC);

        #endregion

        #region User Responses

        /// <summary>
        /// Add and upload question's files
        /// </summary>
        /// <param name="userResponseAC">User response</param>
        /// <returns>List of user response documents</returns>
        Task<List<UserResponseDocumentAC>> AddAndUploadQuestionFilesAsync(UserResponseAC userResponseAC);


        /// <summary>
        /// Get user response
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="userResponseId">userResponseId</param>
        /// <returns>List of userResponse</returns>
        Task<List<UserResponse>> GetUserResponseAsync(int? page, int pageSize, string searchString, Guid userResponseId);

        /// <summary>
        /// Add user response
        /// </summary>
        /// <param name="userResponseAC">userResponseAC</param>
        /// <returns>Added userResponseAC</returns>
        Task<UserResponseAC> AddUserResponseAsync(UserResponseAC userResponseAC);

        /// <summary>
        /// Get user wise response of a strategic analysis
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <param name="strategicAnalysisId">Strategic analysis id whose userwise response is to be fetched</param>
        /// <param name="userId">List of user wise response</param>
        /// <param name="isSampling">bit to check is from sampling or not</param>
        /// <param name="entityId">entity id</param>
        /// <returns>Pagination containing list of user wise responses</returns>
        Task<Pagination<UserWiseResponseAC>> GetUserWiseResponseAsync(Pagination<UserWiseResponseAC> pagination, string strategicAnalysisId, string userId, string rcmId = null, bool isSampling = false, Guid? entityId = null);

        #endregion

        #region Document

        /// <summary>
        /// Delete document from db and from azure
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>Void</returns>
        Task DeleteDocumentAsync(string id);


        /// <summary>
        /// Method to download document
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadDocumentAsync(Guid id);

        /// <summary>
        /// Get email attachments documents
        /// </summary>
        /// <param name="strategyAnalysisId">Id of the strategic analysis</param>
        /// <param name="entityId">Entity Id </param>
        /// <returns>List of files attach for email/returns>
        Task<StrategicAnalysisAC> GetEmailAttachmentDocumentsAsync(Guid strategyAnalysisId, Guid entityId);

        /// <summary>
        /// Save email attachment for a particular strategy analysis
        /// </summary>
        /// <param name="emailAttachmentResponse">Email attachment object</param>
        /// <returns>Task</returns>
        Task UploadEmailAttachmentDocumentsAsync(UserResponseAC emailAttachmentResponse);

        /// <summary>
        /// Save only file type question responses for each strategy 
        /// </summary>
        /// <param name="formdataList">List of all file response</param>
        /// <param name="strategyAnalysisId">Id of the strategy analysis</param>
        /// <param name="isDrafted">Bit to check response drafted or not </param>
        /// <param name="entityId">entity id</param>
        /// <returns>Task</returns>
        Task SaveFileTypeQuestionReponseAsync(IFormCollection formdataList, string strategyAnalysisId, bool isDrafted = false, Guid? entityId = null);
        #endregion

        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="strategicId">Id of strategic</param>
        /// <param name="entityId">Entity id</param>
        /// <param name="offset">offset of client</param>
        /// <param name="emailId">Current user email id</param>
        /// <returns>Data of file in memory stream</returns>
        Task<MemoryStream> DownloadStrategicPDFAsync(string strategicId, Guid entityId, int offset,string emailId);

        #region User Survey
        /// <summary>
        /// Get all strategic analysis for dropdown in user side
        /// </summary>
        /// <param name="userName">Logged in user name</param>
        /// <returns>list of strategic analysis</returns>
        Task<List<StrategicAnalysisAC>> GetAllActiveStrategicAnalysisAsync(string userName);

        #endregion
    }
}
