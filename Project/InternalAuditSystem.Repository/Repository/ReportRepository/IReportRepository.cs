using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.ReportRepository
{
    public interface IReportRepository
    {

        #region Generate Report
        /// <summary>
        /// Get Report data for ACM
        /// </summary>
        /// <param name="entityId">selected entityid</param>
        /// <returns>List of Reports</returns>
        Task<List<Report>> GetReportsForACMIdAsync(Guid entityId);

        /// <summary>
        /// Get Reports data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>List of Reports</returns>
        Task<List<ReportAC>> GetReportsAsync(int? page, int pageSize, string searchString, string selectedEntityId, int fromYear, int toYear);


        /// <summary>
        /// Get Reports data
        /// </summary>
        /// <param name="reportId">Report Id </param>
        /// <param name="selectedEntityId">search string </param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Detail of report</returns>
        Task<ReportAC> GetReportsByIdAsync(string reportId, string selectedEntityId, int timeOffset);

        /// <summary>
        /// Get count of reports
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>count of report</returns>
        Task<int> GetReportCountAsync(string searchValue, string selectedEntityId);

        /// <summary>
        /// Delete report detail
        /// <param name="id">id of delete report</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteReportAsync(string id);

        /// <summary>
        /// Add Report detail
        /// <param name="report">Report detail to add</param>
        /// </summary>
        /// <returns>Return Report AC detail</returns>
        Task<ReportAC> AddReportAsync(ReportAC report);

        /// <summary>
        /// Update Report detail
        /// <param name="report">Report detail to add</param>
        /// </summary>
        /// <returns>Return Report AC detail</returns>
        Task<ReportAC> UpdateReportAsync(ReportAC report);


        /// <summary>
        /// Export Reports to excel file
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportReportsAsync(string selectedEntityId, int timeOffset);

        /// <summary>
        /// Add/Update new reviewer document under a report reviewer
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
        Task AddAndUploadReviewerDocumentAsync(IFormCollection formdata);

        /// <summary>
        /// Delete document from reviewer document and azure storage
        /// </summary>
        /// <param name="reviewerDocumentId">Id of the reviewer document</param>
        /// <returns>Task</returns>
        Task DeleteReviewerDocumentAync(Guid reviewerDocumentId);

        /// <summary>
        /// Get file url and download reviewer document 
        /// </summary>
        /// <param name="reviewerDocumentId">reviewer document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        Task<string> DownloadReviewerDocumentAsync(Guid reviewerDocumentId);
        #endregion

        #region Distributors
        /// <summary>
        /// Add report distributor
        /// <param name="distributors">report distributors</param>
        /// </summary>
        /// <returns>Return Report AC detail</returns>
        Task AddDistributorsAsync(List<ReportUserMappingAC> distributors);

        /// <summary>
        /// Get Distributors for Report
        /// </summary>
        /// <param name="selectedEntityId">selected Entity Id </param>
        /// <param name="reportId">Report Id </param>
        /// <returns>Detail of available distributor, report distibutors</returns>
        Task<ReportDistributorsAC> GetDistributorsByReportIdAsync(string selectedEntityId, string reportId);

        #endregion

        #region Observation Details
        /// <summary>
        /// Get Process data
        /// </summary>
        /// <param name="selectedEntityId">selected Entity Id </param>
        /// <param name="reportId">Report Id </param>
        /// <returns>List of Audit plan process</returns>
        Task<List<AuditPlanAC>> GetPlanProcesessInitDataAsync(string selectedEntityId, string reportId);

        /// <summary>
        /// Get Observations data
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="subProcessId">Sub processId</param
        /// <param name="selectedEntityId">selected entity Id</param>
        /// <param name="reportId">selected report id</param>
        /// <returns>ReportDetailAC detail</returns>
        Task<ReportDetailAC> GetAllObservationsAsync(string planId, string subProcessId, string selectedEntityId, string reportId);

        /// <summary>
        /// Add Observations detail
        /// </summary>
        /// <param name="reportObservations">report Observations </param>
        /// <returns>Task</returns>
        Task AddObservationsAsync(ReportDetailAC reportObservations);

        /// <summary>
        /// Get Report Observation data
        /// </summary>
        /// <param name="reportId">Report Id </param>
        /// <param name="reportObservationId">Report observation id</param>
        /// <param name="selectedEntityId">Current Entity id</param>
        /// <returns>Detail of report</returns>
        Task<ReportDetailAC> GetReportObservationsAsync(string reportId, string reportObservationId, string selectedEntityId);

        /// <summary>
        /// Delete report detail
        /// <param name="id">id of delete report</param>
        /// <param name="reportObservationId">id of delete report observation</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteReportObservationAsync(string id, string reportObservationId);

        /// <summary>
        /// Update Observations detail
        /// </summary>
        /// <param name="reportObservations">report Observations </param>
        /// <returns></returns>
        Task<ReportObservationAC> UpdateReportObservationAsync(ReportDetailAC reportDetailAC);


        /// <summary>
        /// Add/Update new report observation document under a report reviewer
        /// </summary>
        /// <param name="upload">Details of report observation document/param>
        /// <returns>Added report observation document details</returns>
        Task<List<ReportObservationsDocumentAC>> AddAndUploadReportObservationDocumentAsync(DocumentUpload upload);

        /// <summary>
        /// Delete document from report observation document and azure storage
        /// </summary>
        /// <param name="reportObservationDocumentId">Id of the report observation  document</param>
        /// <returns>Task</returns>
        Task DeleteReportObservationDocumentAync(Guid reportObservationDocumentId);

        /// <summary>
        /// Get file url and download Report Observation document 
        /// </summary>
        /// <param name="reviewerDocumentId">Report Observation document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        Task<string> DownloadReportObservationDocumentAsync(Guid reportObservationDocumentId);

        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get report observation json document which represents the dynamic table of observation
        /// </summary>
        /// <param name="id">report Observation id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        Task<string> GetReportObservationTableAsync(string id);

        /// <summary>
        /// To update json document in report observation Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="reportObservationId">report Observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        Task<string> UpdateJsonDocumentAsync(string jsonDocument, string reportObservationId, string tableId);

        /// <summary>
        /// Add column in report observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reprotObservationId">report Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddColumnAsync(string tableId, string reprotObservationId);

        /// <summary>
        /// Add row in report observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reprotObservationId">Report Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddRowAsync(string tableId, string reprotObservationId);

        /// <summary>
        /// Delete row in report observationTable's table
        /// </summary>
        /// <param name="reprotObservationId">report observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteRowAsync(string reprotObservationId, string tableId, string rowId);

        /// <summary>
        /// Delete column in report observationTable's table
        /// </summary>
        /// <param name="reportObservationId">report observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteColumnAsync(string reprotObservationId, string tableId, int columnPosition);

        #endregion

        #region Comment History
        /// <summary>
        /// Get Reports comment history
        /// </summary>
        /// <param name="reportId">Selected Report Id</param>
        /// <param name="timeOffset">Requested  user system timezone</param>
        /// <returns>List of Report comments</returns>
        Task<ReportCommentHistoryAC> GetCommentHistoryAsync(string reportId, int timeOffset);
        #endregion

        #region Generate Report
        /// <summary>
        /// Generate Report to ppt file
        /// </summary>
        /// <param name="reportId">selected report Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> GenerateReportPPTAsync(string reportId, string entityId, int timeOffset);

        /// <summary>
        /// Generate Report observation to ppt file
        /// </summary>
        /// <param name="reportId">selected report observation Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> GenerateReportObservationPPTAsync(string reportId, string entityId, int timeOffset);

        /// <summary>
        /// Get file url for report document 
        /// </summary>
        /// <param name="reportId">selected report id</param>
        /// <param name="entityId">selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Url of File of particular file name passed</returns>
        Task<string> AddViewDocumentAsync(string reportId, string entityId, int timeOffset);
        #endregion
    }
}
