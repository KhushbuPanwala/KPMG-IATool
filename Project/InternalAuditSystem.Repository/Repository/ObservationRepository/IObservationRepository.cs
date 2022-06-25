using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using Microsoft.AspNetCore.Http;

namespace InternalAuditSystem.Repository.Repository.ObservationRepository
{
    public interface IObservationRepository
    {

        #region Observation CRUD

        /// <summary>
        /// Get Observations data
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="SubProcessId">SubProcess Id</param>
        /// <returns>List of Observation</returns>
        Task<List<Observation>> GetObservationsByPlanSubProcessIdAsync(string planId, string SubProcessId);

        /// <summary>
        /// Get all the observations  with searchstring wise , without
        /// search string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="entityId">Id of auditable entity </param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>List of observations under an auditable entity</returns>
        Task<List<ObservationAC>> GetAllObservationsAsync(int? pageIndex, int? pageSize, string searchString, string entityId, int fromYear, int toYear);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="entityId">Selected entityId</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalObservationsPerSearchStringAsync(string searchString, string entityId);

        /// <summary>
        /// Get details of observation by Id
        /// </summary>
        /// <param name="observationId">Id of observation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Application class of observation</returns>
        Task<ObservationAC> GetObservationDetailsByIdAsync(string observationId, string entityId);

        /// <summary>
        /// Method for adding observation data
        /// </summary>
        /// <param name="observationAC">Application class of observation</param>
        /// <returns>Object of newly added observation</returns>
        Task<ObservationAC> AddObservationAsync(ObservationAC observationAC);

        /// <summary>
        /// Method for updating observation
        /// </summary>
        /// <param name="observationAC">Application class of observation</param>
        /// <returns>Updated data of observation</returns>
        Task<ObservationAC> UpdateObservationAsync(ObservationAC observationAC);

        /// <summary>
        /// Method for deleting observation
        /// </summary>
        /// <param name="id">Id of observation</param>
        /// <returns>Task</returns>
        Task DeleteObservationAync(Guid id);


        /// <summary>
        /// Method for export to excel
        /// </summary>
        /// <param name="entityId">Id of auditableEntity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportObservationsAsync(string entityId, int timeOffset);

        #endregion

        #region Dynamic table CRUD

        /// <summary>
        /// Get observation json document which represents the dynamic table of observation
        /// </summary>
        /// <param name="id">Observation id of which table json document is to be fetched</param>
        /// <returns>Serialized json document representation of table data</returns>
        Task<string> GetObservationTableAsync(string id);

        /// <summary>
        /// To update json document in observation Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="observationId">Observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <returns>Updated json document</returns>
        Task<string> UpdateJsonDocumentAsync(string jsonDocument, string observationId, string tableId);

        /// <summary>
        /// Add column in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddColumnAsync(string tableId, string observationId);

        /// <summary>
        /// Add row in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> AddRowAsync(string tableId, string observationId);

        /// <summary>
        /// Delete row in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteRowAsync(string observationId, string tableId, string rowId);

        /// <summary>
        /// Delete column in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        Task<string> DeleteColumnAsync(string observationId, string tableId, int columnPosition);

        #endregion

        #region Bulk Upload
        /// <summary>
        /// Get observation releated data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId"></param>
        /// <returns>Detail for observation bulk upload</returns>
        Task<ObservationUploadAC> GetObservationUploadDetailAsync(string selectedEntityId);



        /// <summary>
        /// Upload observation data
        /// </summary>
        /// <param name="file">Upload file</param>
        /// <returns></returns>
        Task UploadObservationAsync(BulkUpload bulkUpload);

        #endregion

        #region File upload

        /// <summary>
        /// Add and upload observation files
        /// </summary>
        /// <param name="files">Observation files to be uploaded</param>
        /// <param name="observationId">Observation id to which files are to be connected</param>
        /// <returns>List of added observationDocuments</returns>
        Task<List<ObservationDocumentAC>> AddAndUploadObservationFilesAsync(List<IFormFile> files, Guid observationId);

        /// <summary>
        /// Method to download observation document
        /// </summary>
        /// <param name="id">Observation document Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadObservationDocumentAsync(Guid id);

        /// <summary>
        /// Delete observation document from db and from azure
        /// </summary>
        /// <param name="id">Observation document id</param>
        /// <returns>Void</returns>
        Task DeleteObservationDocumentAsync(Guid id);

        #endregion

        #region Generate Observation
        /// <summary>
        /// Generate Observation to ppt file
        /// </summary>
        /// <param name="observationId">selected Observation Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> GenerateObservationPPTAsync(string observationId, string entityId, int timeOffset);

        #endregion
    }
}
