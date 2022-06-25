using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Repository.Exceptions;
using System.IO;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.DomailModel.Models.ReportManagement;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        public IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Get all reports details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="fromYear">selected fromYear </param>
        /// <param name="toYear">selected toYear </param>
        /// <returns>list of reports details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<ReportAC>>> GetReports(int? page, int pageSize, string searchString, string selectedEntityId, int fromYear, int toYear)
        {
            var result = new Pagination<ReportAC>
            {
                TotalRecords = await _reportRepository.GetReportCountAsync(searchString, selectedEntityId),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _reportRepository.GetReportsAsync(page, pageSize, searchString, selectedEntityId, fromYear, toYear)
            };
            return Ok(result);
        }

        /// <summary>
        /// Get reports by id details  
        /// <param name="id">report id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// </summary>
        /// <returns>report details</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportAC>> GetReportById(string id, string selectedEntityId, int timeOffset)
        {
            ReportAC reportDetail = new ReportAC();

            reportDetail = await _reportRepository.GetReportsByIdAsync(id, selectedEntityId, timeOffset);
            return Ok(reportDetail);
        }


        /// <summary>
        /// Delete Report 
        /// <param name="id">id of delete report </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReport(string id, string selectedEntityId)
        {
            await _reportRepository.DeleteReportAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Add Report detail
        /// </summary>
        /// <param name="report">Report Details to add</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Report Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReportAC>> AddReport([FromBody] ReportAC report, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                ReportAC reportAC = await _reportRepository.AddReportAsync(report);
                return Ok(reportAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Report detail
        /// </summary>
        /// <param name="report">Report Detail to update</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Report Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReportAC>> UpdateReport([FromBody] ReportAC report, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                ReportAC reportAC = await _reportRepository.UpdateReportAsync(report);
                return Ok(reportAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get reports comment history
        /// </summary>
        /// <param name="id">Report Id</param>
        /// <param name="timeOffset">Requested  user system timezone</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>list of reports commnet history</returns>
        [HttpGet]
        [Route("commentHistory/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportCommentHistoryAC>> GetCommentHistory(string id, int timeOffset, string entityId)
        {
            ReportCommentHistoryAC reportCommentHistory = await _reportRepository.GetCommentHistoryAsync(id, timeOffset);
            return Ok(reportCommentHistory);
        }

        /// <summary>
        /// Export reports to excel
        /// </summary>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("exportReports")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportReports(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _reportRepository.ExportReportsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }

        /// <summary>
        /// Add/Update new reviewer document under a audit plan
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
        [HttpPost]
        [Route("add-reviewer-documents")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> AddReviewerDocuments(IFormCollection formdata)
        {
            try
            {
                await _reportRepository.AddAndUploadReviewerDocumentAsync(formdata);
                return Ok();
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
        /// Delete reviewer document from reviewer docuemnt and azure storage
        /// </summary>
        /// <param name="reviewerDocumentId">Id of the reviewer document</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("delete-reviewer-document/{reviewerDocumentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReviewerDocument(string reviewerDocumentId, string entityId)
        {
            try
            {
                await _reportRepository.DeleteReviewerDocumentAync(Guid.Parse(reviewerDocumentId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get file url and download reviewer document 
        /// </summary>
        /// <param name="reviewerDocumentId">reviewer document id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("download-file/{reviewerDocumentId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> DownloadReviewerDocument(string reviewerDocumentId, string entityId)
        {
            return Ok(await _reportRepository.DownloadReviewerDocumentAsync(Guid.Parse(reviewerDocumentId)));
        }

        #region PPT Report
        /// <summary>
        /// Create PPT report
        /// </summary>
        /// <param name="id">selected report Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("generateReportPPT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GenerateReportPPT(string id, string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _reportRepository.GenerateReportPPTAsync(id, entityId, timeOffset);
            return File(fileData.Item2, StringConstant.PPTContentType, fileData.Item1);
        }

        /// <summary>
        /// Get file url for report document 
        /// </summary>
        /// <param name="id">selected report id</param>
        /// <param name="entityId">selected entity id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("addViewFile")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> AddViewAuditReport(string id, string entityId, int timeOffset)
        {
            return Ok(await _reportRepository.AddViewDocumentAsync(id, entityId, timeOffset));
        }

        #endregion
    }
}
