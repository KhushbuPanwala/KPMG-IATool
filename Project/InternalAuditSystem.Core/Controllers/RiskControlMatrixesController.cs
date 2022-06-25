using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RiskControlMatrixesController : Controller
    {
        #region Private variables
        private readonly IRiskControlMatrixRepository _riskControlMatrixRepository;
        #endregion

        #region Public variables
        public IAzureRepository _azureRepository;
        #endregion

        #region Constructor
        public RiskControlMatrixesController(IRiskControlMatrixRepository riskControlMatrixRepository, IAzureRepository azureRepository)
        {
            _riskControlMatrixRepository = riskControlMatrixRepository;
            _azureRepository = azureRepository;
        }
        #endregion

        #region RCM CRUD and work program
        /// <summary>
        /// Get RCM list for list page in work program
        /// </summary>
        /// <param name="workProgramId">workProgram Id</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<RiskControlMatrixAC>>> GetRCMListForWorkProgramAsync(string workProgramId, int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            Pagination<RiskControlMatrixAC> pagination = new Pagination<RiskControlMatrixAC>()
            {
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            if (!String.IsNullOrEmpty(workProgramId))
            {
                return await _riskControlMatrixRepository.GetRCMListForWorkProgramAsync(pagination, new Guid(workProgramId), selectedEntityId);
            }
            else
            {
                return await _riskControlMatrixRepository.GetRCMListForWorkProgramAsync(pagination, null, selectedEntityId);
            }
        }

        /// <summary>
        /// Get rcm details by id and details for all initial dropdowns in add page and edit page
        /// </summary>
        /// <param name="id">Rcm Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Detail of rcm</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RiskControlMatrixAC>> GetRiskControlMatrixDetailsByIdAsync(string id, string selectedEntityId)
        {
            try
            {
                if (id == "0")
                {
                    return await _riskControlMatrixRepository.GetRiskControlMatrixDetailsByIdAsync(null, selectedEntityId);


                }
                else
                {
                    return await _riskControlMatrixRepository.GetRiskControlMatrixDetailsByIdAsync(Guid.Parse(id), selectedEntityId);
                }
            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update RCM
        /// </summary>
        /// <param name="riskControlMatrixListAC">riskControlMatrixAC</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>riskControlMatrixAC</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RiskControlMatrixAC>> UpdateRiskControlMatrixAsync([FromBody] List<RiskControlMatrixAC> riskControlMatrixListAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await _riskControlMatrixRepository.UpdateRiskControlMatrixAsync(riskControlMatrixListAC));

        }

        /// <summary>
        /// Add RCM
        /// </summary>
        /// <param name="riskControlMatrixAC">riskControlMatrixAC</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>riskControlMatrixAC</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RiskControlMatrixAC>> AddRiskControlMatrixAsync([FromBody] RiskControlMatrixAC riskControlMatrixAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await _riskControlMatrixRepository.AddRiskControlMatrixAsync(riskControlMatrixAC));

        }

        /// <summary>
        /// Delete RCM 
        /// <param name="id">id of deleted RCM </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteRcm(string id, string selectedEntityId)
        {
            await _riskControlMatrixRepository.DeleteRcmAsync(Guid.Parse(id));
            return NoContent();
        }
        #endregion

        #region Bulk Upload

        /// <summary>
        /// Get rcm releated data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId"></param>
        /// <returns>Detail for rcm bulk upload</returns>
        [HttpGet]
        [Route("bulkUploadData")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<RCMUploadAC>> GetRCMUploadDetail(string selectedEntityId)
        {
            RCMUploadAC rcmUploadAC = await _riskControlMatrixRepository.GetRCMUploadDetailAsync(selectedEntityId);
            return Ok(rcmUploadAC);
        }


        /// <summary>
        /// Update RCM
        /// </summary>
        /// <param name="uploadRCM">Uploaded files and entity id</param>
        /// <returns>Upload RCM</returns>
        [HttpPost("uploadRCMs")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> UploadRMC([FromForm] BulkUpload uploadRCM)
        {
            try
            {
                await _riskControlMatrixRepository.UploadRCMAsync(uploadRCM);
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
            catch (NotExistException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Export Rcm main
        /// <summary>
        /// Method of export to excel for  Rcm main
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportRiskControlMatrixes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportRcmMainAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _riskControlMatrixRepository.ExportRcmMainAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
    }
}
