using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ObservationRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsManagementController : Controller
    {
        #region Private variables
        private readonly IObservationRepository _observationRepository;
        #endregion

        #region Public variables
        public IAzureRepository _azureRepository;
        #endregion

        #region Constructor
        public ObservationsManagementController(IObservationRepository observationRepository, IAzureRepository azureRepository)
        {
            _observationRepository = observationRepository;
            _azureRepository = azureRepository;
        }

        #endregion

        #region Public Methods

        #region Observation CRUD and export observation

        /// <summary>
        /// Get all the observations under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>List of observations </returns>
        [HttpGet]
        [Route("getObservations")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<ObservationAC>>> GetAllObservationsAsync(int? pageIndex, int pageSize, string searchString, string entityId, int fromYear, int toYear)
        {
            var listOfObservations = await _observationRepository.GetAllObservationsAsync(pageIndex, pageSize, searchString, entityId, fromYear, toYear);

            //create pagination wise data
            var result = new Pagination<ObservationAC>
            {
                Items = listOfObservations,
                TotalRecords = await _observationRepository.GetTotalObservationsPerSearchStringAsync(searchString, entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
            };

            return Ok(result);
        }


        /// <summary>
        /// Method for getting details of observation by Id
        /// </summary>
        /// <param name="observationId">Id of observation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Details of observation by id</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ObservationAC>> GetObservationDetailsByIdAsync(string observationId, string entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ObservationAC observationAC = await _observationRepository.GetObservationDetailsByIdAsync(observationId, entityId);
            return observationAC;
        }

        /// <summary>
        /// Method for saving observation data
        /// </summary>
        /// <param name="observationAC">Application class object of observation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Details of observation</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ObservationAC>> AddObservationAsync([FromBody] ObservationAC observationAC, string entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _observationRepository.AddObservationAsync(observationAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for updating observation
        /// </summary>
        /// <param name="observationAC">Application class object of ovservation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated details of observation</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ObservationAC>> UpdateObservationAsync([FromBody] ObservationAC observationAC, string entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _observationRepository.UpdateObservationAsync(observationAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for deleting observation
        /// </summary>
        /// <param name="id">Id of observation</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteObservationAsync(string id, string entityId)
        {
            try
            {
                await _observationRepository.DeleteObservationAync(new Guid(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for export to excel
        /// </summary>
        /// <param name="entityId">Id of auditableEntity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        [HttpGet]
        [Route("exportObservations")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportObservations(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _observationRepository.ExportObservationsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }

        #endregion

        #region Observation json document table

        /// <summary>
        /// Get observation json document which represents the dynamic table of observation
        /// </summary>
        /// <param name="id">Observation id of which table json document is to be fetched</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Serialized json document representation of table data</returns>
        [HttpGet]
        [Route("json-documents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetObservationTable(string id, string entityId)
        {
            try
            {
                var jsonDocument = await _observationRepository.GetObservationTableAsync(id);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// To update json document in observation Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="observationId">Observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated json document</returns>
        public async Task<ActionResult<string>> UpdateJsonDocument(string jsonDocument, string observationId, string tableId, string entityId)
        {
            try
            {
                var updatedJsonDocument = await _observationRepository.UpdateJsonDocumentAsync(jsonDocument, observationId, tableId);
                return updatedJsonDocument;
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Add column in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddColumnInTable(string tableId, string observationId, string entityId)
        {
            try
            {
                var jsonDocument = await _observationRepository.AddColumnAsync(tableId, observationId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/rows")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Add row in observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddRow(string tableId, string observationId, string entityId)
        {
            try
            {
                var jsonDocument = await _observationRepository.AddRowAsync(tableId, observationId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/rows/id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Delete row in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteRow(string observationId, string tableId, string rowId, string entityId)
        {
            try
            {
                var jsonDocument = await _observationRepository.DeleteRowAsync(observationId, tableId, rowId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/columns/id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Delete column in observationTable's table
        /// </summary>
        /// <param name="observationId">Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteColumn(string observationId, string tableId, int columnPosition, string entityId)
        {
            try
            {
                var jsonDocument = await _observationRepository.DeleteColumnAsync(observationId, tableId, columnPosition);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Bulk Upload
        /// <summary>
        /// Get observation releated data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId"></param>
        /// <returns>Detail for observation bulk upload</returns>
        [HttpGet]
        [Route("bulkUploadData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ObservationUploadAC>> GetObservationUploadDetail(string selectedEntityId)
        {
            ObservationUploadAC observationUploadAC = await _observationRepository.GetObservationUploadDetailAsync(selectedEntityId);
            return Ok(observationUploadAC);
        }

        /// <summary>
        /// Update observations
        /// </summary>
        /// <param name="uploadObservation">Uploaded files and entity id</param>
        /// <returns>Upload observations</returns>
        [HttpPost("uploadObservations")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> UploadObservations([FromForm] BulkUpload uploadObservation)
        {
            try
            {
                await _observationRepository.UploadObservationAsync(uploadObservation);
                return Ok();
            }

            catch (InvalidFileException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RequiredDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MaxLengthDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidBulkDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Observation document

        /// <summary>
        /// add and upload observation files
        /// </summary>
        /// <param name="observationAC">observation containing files</param>
        /// <returns>List of added observationDocuments</returns>

        [HttpPost("uploadFiles")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<List<ObservationDocumentAC>>> UploadFile([FromForm] ObservationAC observationAC)
        {
            try
            {
                var addedObservationDocumentACs = await _observationRepository.AddAndUploadObservationFilesAsync(observationAC.ObservationFiles, (Guid)observationAC.Id);
                return addedObservationDocumentACs;
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileCount ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileSize ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileFormate ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">Observation document id</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetObservationDocumentDownloadUrl(string id, string entityId)
        {
            return Ok(await _observationRepository.DownloadObservationDocumentAsync(new Guid(id)));
        }

        /// <summary>
        /// Delete observation document from db and from azure
        /// </summary>
        /// <param name="id">Observation document id</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteObservationDocument(string id, string entityId)
        {
            await _observationRepository.DeleteObservationDocumentAsync(new Guid(id));
            return NoContent();
        }

        #endregion

        #region Observation PPT 
        /// <summary>
        /// Create PPT observation
        /// </summary>
        /// <param name="id">selected observation Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("generateObservationPPT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GenerateObservationPPT(string id, string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _observationRepository.GenerateObservationPPTAsync(id, entityId, timeOffset);
            return File(fileData.Item2, StringConstant.PPTContentType, fileData.Item1);
        }

        #endregion

        #endregion
    }
}
