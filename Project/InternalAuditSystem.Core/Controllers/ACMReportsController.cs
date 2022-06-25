using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ACMRepresentationRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ACMReportsController : Controller
    {
       
        private readonly IACMRepository _acmRepository;
        public ACMReportsController(IACMRepository acmRepository)
        {
            _acmRepository = acmRepository;
            
        }

        /// <summary>
        /// Add/Update new reviewer document under acm report
        /// </summary>
        /// <param name="formdata">Details of reviewer document/param>
        /// <returns>Added reviewer document details</returns>
        [HttpPost]
        [Route("add-acm-reviewer-documents")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> AddReviewerDocuments(IFormCollection formdata)
        {
            await _acmRepository.AddAndUploadReviewerDocumentAsync(formdata);
            return Ok();

        }

        /// <summary>
        /// Get report details
        /// </summary>
        /// <param name="id">id of report detail</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Details of ACMReportDetailAC</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ACMReportDetailAC>> GetACMReportsAndReviewers(string id, int? pageIndex, int pageSize, string searchString, string entityId)
        {
            var pagination = new Pagination<ReportAC>
            {
                EntityId = new Guid(entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return await _acmRepository.GetACMReportsAndReviewersByIdAsync(Guid.Parse(id), pagination);
        }

        /// <summary>
        /// Delete reviewer document from reviewer docuemnt and azure storage
        /// </summary>
        /// <param name="reviewerDocumentId">Id of the reviewer document</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("delete-acm-reviewer-document/{reviewerDocumentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteReviewerDocument(string reviewerDocumentId, string entityId)
        {
            try
            {
                await _acmRepository.DeleteReviewerDocumentAync(Guid.Parse(reviewerDocumentId));
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
        [Route("download-acm-file/{reviewerDocumentId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> DownloadReviewerDocument(string reviewerDocumentId, string entityId)
        {
            return Ok(await _acmRepository.DownloadReviewerDocumentAsync(Guid.Parse(reviewerDocumentId)));
        }

        [HttpGet]
        [Route("getACMReportByStatusAndStage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<ReportAC>>> GetACMAllReportsByStatusAndStagAsync(int? pageIndex, int pageSize, string searchString, string selectedEntityId, string selectedStatus, string selectedStage)
        {
            //create pagination wise data
            var pagination = new Pagination<ReportAC>
            {
                EntityId = new Guid(selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };

            return await _acmRepository.GetACMAllReportsByStatusAndStagAsync(pagination,selectedStatus,selectedStage);
        }

        /// <summary>
        /// Add/Update new acm report
        /// </summary>
        /// <param name="reportDetail">Details of acm report/param>
        /// <param name="selectedEntityId">Selecyed entity id</param>
        /// <returns>Added acm report details</returns>
        [HttpPost]
        [Route("addAcmReport")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> AddAcmReportAsync([FromBody] ACMReportDetailAC reportDetail, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                ACMReportDetailAC reportAC = await _acmRepository.AddAcmReportAsync(reportDetail);
                return Ok(reportAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
    }
}
