using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RiskAssessmentsController : Controller
    {
        public IRiskAssessmentRepository _riskAssessmentRepository;

        public RiskAssessmentsController(IRiskAssessmentRepository riskAssessmentRepository)
        {
            _riskAssessmentRepository = riskAssessmentRepository;
        }

        /// <summary>
        /// Get RiskAssessment details by id for edit page
        /// </summary>
        /// <param name="id">RiskAssessment Id</param>
        /// <returns>Detail of RiskAssessment</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RiskAssessmentAC>> GetRiskAssessmentDetailsByIdAsync(string id)
        {
            return await _riskAssessmentRepository.GetRiskAssessmentDetailsAsync(new Guid(id));
        }

        /// <summary>
        /// Get RiskAssessment list for list page 
        /// </summary>
        /// <param name="entityId">Auditable entity Id</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <returns>Pagination object of Risk Assessment</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<RiskAssessmentAC>>> GetRiskAssessmentListAsync(string entityId, int? pageIndex, int pageSize, string searchString)
        {
            Pagination<RiskAssessmentAC> pagination = new Pagination<RiskAssessmentAC>()
            {
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString,
                EntityId = new Guid(entityId)
            };
            return await _riskAssessmentRepository.GetRiskAssessmentListAsync(pagination);
        }

        /// <summary>
        /// Update RiskAssessment
        /// </summary>
        /// <param name="riskControlMatrixAC">riskControlMatrixAC</param>
        /// <returns>riskControlMatrixAC</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<RiskAssessmentAC>> UpdateRiskAssessmentAsync([FromForm] RiskAssessmentAC riskAssessmentAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await _riskAssessmentRepository.UpdateRiskAssessmentAsync(riskAssessmentAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Add RiskAssessment
        /// </summary>
        /// <param name="riskAssessmentAC">riskAssessmentAC</param>
        /// <returns>riskAssessmentAC</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<RiskAssessmentAC>> AddRiskAssessmentAsync([FromForm] RiskAssessmentAC riskAssessmentAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await _riskAssessmentRepository.AddRiskAssessmentAsync(riskAssessmentAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete RiskAssessment from list page
        /// </summary>
        /// <param name="id">RiskAssessment id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRiskAssessmentAync(string id)
        {
            await _riskAssessmentRepository.DeleteRiskAssessmentAync(new Guid(id));
            return NoContent();
        }

        /// <summary>
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">RiskAssessmentDocumment id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetRiskAssessmentDocummentDownloadUrlAsync(string id)
        {
            return Ok(await _riskAssessmentRepository.DownloadRiskAssessmentDocumentAsync(new Guid(id)));
        }

        /// <summary>
        /// Delete RiskAssessmentDocumment file
        /// </summary>
        /// <param name="id">RiskAssessmentDocumment Id</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRiskAssessmentDocummentAsync(string id)
        {
            await _riskAssessmentRepository.DeleteRiskAssessmentDocumentAync(new Guid(id));
            return NoContent();
        }
    }
}
