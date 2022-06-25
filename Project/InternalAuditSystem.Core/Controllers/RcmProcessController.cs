using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmProcessRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class RcmProcessController : Controller
    {
        public IRcmProcessRepository _RcmProcessRepository;
        public RcmProcessController(IRcmProcessRepository RcmProcessRepository)
        {
            _RcmProcessRepository = RcmProcessRepository;
        }

        /// <summary>
        /// Get all  processes for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all  processes with its id and name only </returns>
        [HttpGet]
        [Route("export-excel/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RcmProcessAC>>> GetAllProcessesForDisplayInDropDown(string entityId)
        {
            return await _RcmProcessRepository.GetAllProcessForDisplayInDropDownAsync(new Guid(entityId));
        }

        /// <summary>
        /// Get all RCM Process details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search valee</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>list of RCM Process details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<RcmProcessAC>>> GetRcmProcess(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = new Pagination<RcmProcessAC>
            {
                TotalRecords = await _RcmProcessRepository.GetRcmProcessCountAsync(selectedEntityId, searchString),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _RcmProcessRepository.GetRcmProcessAsync(page, pageSize, searchString, selectedEntityId)
            };
            return Ok(result);

        }
        /// <summary>
        /// Get RCM Process details by id
        /// </summary>
        /// <param name="processId">Process Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Detail of RCM Process</returns>
        [HttpGet]
        [Route("{processId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmProcessAC>> GetRcmProcessById(string processId, string selectedEntityId)
        {
            var Process = await _RcmProcessRepository.GetRcmProcessByIdAsync(processId);
            return Process;
        }

        /// <summary>
        /// Add RCM Process detail
        /// </summary>s
        /// <param name="processAC">RCM Process Details to add</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>RCM Process Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmProcessAC>> AddRcmProcess([FromBody] RcmProcessAC processAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                RcmProcessAC rcmProcessAC = await _RcmProcessRepository.AddRcmProcess(processAC);
                return Ok(rcmProcessAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update RCM Process detail
        /// </summary>
        /// <param name="processAC">Update RCM Process Detail </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>RCM Process Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmProcessAC>> UpdateRcmProcess([FromBody] RcmProcessAC processAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result = await _RcmProcessRepository.UpdateRcmProcess(processAC);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete RCM Process 
        /// <param name="processId">id of delete Process </param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{processId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteRcmProcess(string processId, string selectedEntityId)
        {
            try
            {
                await _RcmProcessRepository.DeleteRcmProcess(processId);
                return NoContent();
            }
            catch (DuplicateRCMException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #region Export Rcm process
        /// <summary>
        /// Method of export to excel for  Rcm process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportRcmProcess")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportRcmProcessAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _RcmProcessRepository.ExportRcmProcessAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
    }
}
