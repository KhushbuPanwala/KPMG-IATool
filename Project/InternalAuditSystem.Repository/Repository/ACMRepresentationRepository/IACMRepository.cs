using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ACMRepresentationRepository
{
    public interface IACMRepository
    {
        #region
        /// <summary>
        /// Get ACM data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="fromYear">selected from year</param>   
        /// <param name="toYear">selected toyear</param> 
        /// <returns>List of ACM</returns>
        Task<List<ACMPresentationAC>> GetACMDataAsync(int? page, int pageSize, string searchString, string selectedEntityId, int? fromYear, int? toYear);

        /// <summary>
        /// Get count of ACM
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>count of ACM</returns>
        Task<int> GetACMCountAsync(string searchString, string selectedEntityId);

        /// <summary>
        /// Get ACM details
        /// </summary>
        /// <param name="acmId">ACM id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>count of ACM</returns>
        Task<ACMPresentationAC> GetACMDetailsByIdAsync(Guid? acmId, string selectedEntityId);

        /// <summary>
        /// Add ACM detail
        /// </summary>
        /// <param name="acmPresentationAC">detail of ACM</param>
        /// <returns>Task</returns>
        Task<ACMPresentationAC> AddACMAsync(ACMPresentationAC acmPresentationAC);

        /// <summary>
        /// Update ACM detail
        /// </summary>
        /// <param name="acmPresentationListAC">details of ACM</param>
        /// <returns>Task</returns>
        Task<ACMPresentationAC> UpdateACMAsync(ACMPresentationAC acmPresentationAC);

        /// <summary>
        /// Delete ACM detail
        /// <param name="acmId">id of deleted ACM</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteAcmAsync(Guid acmId);


        /// <summary>
        /// Method for exporting acm
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAcmAsync(string entityId, int timeOffset);

        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get acm json document which represents the dynamic table of acm
        /// </summary>
        /// <param name="id">acm id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        Task<string> GetACMTableAsync(string id);

        /// <summary>
        /// To update json document in acm Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="acmId">acm id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        Task<string> UpdateJsonDocumentAsync(string jsonDocument, string acmId, string tableId);

        /// <summary>
        /// Add column in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddColumnAsync(string tableId, string acmId);

        /// <summary>
        /// Add row in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddRowAsync(string tableId, string acmId);

        /// <summary>
        /// Delete row in acmTable's table
        /// </summary>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteRowAsync(string acmId, string tableId, string rowId);

        /// <summary>
        /// Delete column in acmTable's table
        /// </summary>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteColumnAsync(string acmId, string tableId, int columnPosition);

        #endregion

        #region File upload

        /// <summary>
        /// Add and upload ACM files
        /// </summary>
        /// <param name="files">ACM files to be uploaded</param>
        /// <param name="acmId">ACM id to which files are to be connected</param>
        /// <returns>List of added ACMDocuments</returns>
        Task<List<ACMDocumentAC>> AddAndUploadACMFilesAsync(List<IFormFile> files, Guid acmId);

        /// <summary>
        /// Method to download ACM document
        /// </summary>
        /// <param name="id">ACM document Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadACMDocumentAsync(Guid id);

        /// <summary>
        /// Delete ACM document from db and from azure
        /// </summary>
        /// <param name="id">ACM document id</param>
        /// <returns>Void</returns>
        Task DeleteACMDocumentAsync(Guid id);

        #endregion

        #region Acm Report


        /// <summary>
        /// Method for getting report details by status and stage
        /// </summary>
        /// <param name="pagination">Pagination of report AC</param>
        /// <returns>List of report</returns>
        Task<Pagination<ReportAC>> GetACMAllReportsByStatusAndStagAsync(Pagination<ReportAC> pagination, string selectedStatus, string selectedStage);


        /// <summary>
        /// Get Details of ACM by Id
        /// </summary>
        /// <param name="acmId">ACM Id</param>
        /// <param name="pagination">pagination of ReportAC </param>
        /// <returns>ACMPresentationAC</returns>
         Task<ACMReportDetailAC> GetACMReportsAndReviewersByIdAsync(Guid acmId, Pagination<ReportAC> pagination);


        /// <summary>
        /// Add acm report detail
        /// </summary>
        /// <param name="acmReportDetail">ACM report detail </param>
        /// <returns>Return ACMReportDetail AC detail</returns>
        Task<ACMReportDetailAC> AddAcmReportAsync(ACMReportDetailAC acmReportDetail);

        /// <summary>
        /// Add/Update new reviewer document under an acm reviewer
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
         Task AddAndUploadReviewerDocumentAsync(IFormCollection formdata);


        /// <summary>
        /// Delete Reviewer document from reviewer document and azure storage
        /// </summary>
        /// <param name="reviewerDocumentId">Id of the Reviewer document</param>
        /// <returns>Task</returns>
        Task DeleteReviewerDocumentAync(Guid reviewerDocumentId);

        /// <summary>
        /// Get file url and download reviewer document 
        /// </summary>
        /// <param name="reviewerDocumentId">reviewer document id</param>
        /// <returns>Url of File of particular file name passed</returns>
         Task<string> DownloadReviewerDocumentAsync(Guid reviewerDocumentId);
        #endregion

        #region Generate Report
        /// <summary>
        /// Generate ACM to ppt file
        /// </summary>
        /// <param name="id">selected Acm Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> GenerateACMPPTAsync(string id, string entityId, int timeOffset);
        #endregion
    }
}