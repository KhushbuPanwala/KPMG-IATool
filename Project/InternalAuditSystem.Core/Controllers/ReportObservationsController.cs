using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using System.Collections.Generic;
using System;
using InternalAuditSystem.Repository.Exceptions;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Core.ActionFilters;
using System.IO;
using InternalAuditSystem.Utility.Constants;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportObservationsController : Controller
    {
        public IReportRepository _reportRepository;

        public ReportObservationsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Get all Audit plan process details
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="reportId">Selected Report Id</param>
        /// <returns>list of Audit plan process details</returns>
        [HttpGet]
        [Route("getPlanProcesess/{selectedEntityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<AuditPlanAC>>> GetPlanProcesessInitData(string selectedEntityId, string reportId)
        {
            List<AuditPlanAC> auditPlans = await _reportRepository.GetPlanProcesessInitDataAsync(selectedEntityId, reportId);
            return Ok(auditPlans);
        }

        /// <summary>
        /// Get Observations details
        /// <param name="planId">Plan id</param>
        /// <param name="subProcessId">subprocess id</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <param name="reportId">current report Id</param>
        /// </summary>
        /// <returns>ReportDetailAC detail</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportDetailAC>> GetAllObservations(string planId, string subProcessId, string selectedEntityId, string reportId)
        {
            ReportDetailAC reportObservationDetails = await _reportRepository.GetAllObservationsAsync(planId, subProcessId, selectedEntityId, reportId);
            return Ok(reportObservationDetails);
        }

        /// <summary>
        /// Add Observations detail
        /// </summary>
        /// <param name="reportObservations">report Observations </param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("addObservations")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddObservations([FromBody] ReportDetailAC reportObservations, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _reportRepository.AddObservationsAsync(reportObservations);
            return Ok();
        }


        /// <summary>
        /// Delete Report 
        /// <param name="id">id of delete report id</param>
        /// <param name="reportObservationId">id of delete report observation</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReportObservation(string id, string reportObservationId, string selectedEntityId)
        {
            await _reportRepository.DeleteReportObservationAsync(id, reportObservationId);
            return NoContent();
        }


        /// <summary>
        /// Get reports by id details  
        /// <param name="id">report id</param>
        /// <param name="reportObservationId">Search value</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// </summary>
        /// <returns>report details</returns>
        [HttpGet]
        [Route("getReportObservation/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportDetailAC>> GetReportObservations(string id, string reportObservationId, string selectedEntityId)
        {

            ReportDetailAC reportDetailAC = await _reportRepository.GetReportObservationsAsync(id, reportObservationId, selectedEntityId);
            return Ok(reportDetailAC);
        }


        /// <summary>
        /// Add Report detail
        /// </summary>
        /// <param name="reportDetailAC">Report Details to add</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Report Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReportObservationAC>> UpdateReportObservation([FromBody] ReportDetailAC reportDetailAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ReportObservationAC reportObservationAC = await _reportRepository.UpdateReportObservationAsync(reportDetailAC);
            return Ok(reportObservationAC);

        }

        #region Report Observation json document table

        /// <summary>
        /// Get Report observation json document which represents the dynamic table of report observation
        /// </summary>
        /// <param name="id">Reprot Observation id of which table json document is to be fetched</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Serialized json document representation of table data</returns>
        [HttpGet]
        [Route("json-documents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetReportObservationTable(string id, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _reportRepository.GetReportObservationTableAsync(id);
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
        /// To update json document in report observation Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="reportObservationId">report observation id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Updated json document</returns>
        public async Task<ActionResult<string>> UpdateJsonDocument(string jsonDocument, string reportObservationId, string tableId, string selectedEntityId)
        {
            try
            {
                var updatedJsonDocument = await _reportRepository.UpdateJsonDocumentAsync(jsonDocument, reportObservationId, tableId);
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
        /// Add column in reportObservationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reportObservationId">Reprot Observation id to which this json document table is linked</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddColumnInTable(string tableId, string reportObservationId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _reportRepository.AddColumnAsync(tableId, reportObservationId);
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
        /// Add row in report observationTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="reportObservationId">Report Observation id to which this json document table is linked</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddRow(string tableId, string reportObservationId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _reportRepository.AddRowAsync(tableId, reportObservationId);
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
        /// Delete row in report observationTable's table
        /// </summary>
        /// <param name="reportObservationId">Report Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteRow(string reportObservationId, string tableId, string rowId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _reportRepository.DeleteRowAsync(reportObservationId, tableId, rowId);
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
        /// Delete column in report observationTable's table
        /// </summary>
        /// <param name="reportObservationId">Report Observation id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteColumn(string reportObservationId, string tableId, int columnPosition, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _reportRepository.DeleteColumnAsync(reportObservationId, tableId, columnPosition);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Document Methods
        /// <summary>
        /// Add/Update new report observation document under a audit plan
        /// </summary>
        /// <param name="uploadFiles">Details of report observationdocument/param>
        /// <returns>List of report observation document details</returns>
        [HttpPost]
        [Route("add-report-Observation-documents")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReportObservationsDocumentAC>> AddReportObservationDocuments([FromForm] DocumentUpload uploadFiles)
        {
            try
            {

                List<ReportObservationsDocumentAC> result = await _reportRepository.AddAndUploadReportObservationDocumentAsync(uploadFiles);
                return Ok(result);
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
        /// Delete reviewer document from reportObservation document and azure storage
        /// </summary>
        /// <param name="reportObservationDocumentId">Id of the reportObservation document</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("delete-report-observation-document/{reportObservationDocumentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReportObservationDocument(string reportObservationDocumentId, string selectedEntityId)
        {
            try
            {
                await _reportRepository.DeleteReportObservationDocumentAync(Guid.Parse(reportObservationDocumentId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get file url and download report observation document 
        /// </summary>
        /// <param name="observationDocumentId">report observation document id</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("download-file/{observationDocumentId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> DownloadReportObservationDocument(string observationDocumentId, string selectedEntityId)
        {
            return Ok(await _reportRepository.DownloadReportObservationDocumentAsync(Guid.Parse(observationDocumentId)));
        }

        #endregion


        #region PPT Report
        /// <summary>
        /// Create PPT report
        /// </summary>
        /// <param name="id">selected report Observation Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("generateReportObservationPPT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GenerateReportObservationPPT(string id, string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _reportRepository.GenerateReportObservationPPTAsync(id, entityId, timeOffset);
            return File(fileData.Item2, StringConstant.PPTContentType, fileData.Item1);
        }
        #endregion
    }
}
