using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSubProcessRepository;
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
    [Route("api/[controller]")]
    [ApiController]
    public class RcmSubProcessController : Controller
    {
        public IRcmSubProcessRepository _rcmSubProcessRepository;
        public RcmSubProcessController(IRcmSubProcessRepository RcmSubProcessRepository)
        {
            _rcmSubProcessRepository = RcmSubProcessRepository;
        }

        /// <summary>
        /// Get all sub processes for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all sub processes with its id and name only </returns>
        [HttpGet]
        [Route("export-excel/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RcmSubProcessAC>>> GetAllSubProcessesForDisplayInDropDown(string entityId)
        {
            return await _rcmSubProcessRepository.GetAllSubProcessesForDisplayInDropDownAsync(new Guid(entityId));
        }

        /// <summary>
        /// Get all RCM SubProcess details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search valee</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>list of RCM SubProcess details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<RcmSubProcessAC>>> GetRcmSubProcess(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = new Pagination<RcmSubProcessAC>
            {
                TotalRecords = await _rcmSubProcessRepository.GetRcmSubProcessCountAsync(selectedEntityId, searchString),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _rcmSubProcessRepository.GetRcmSubProcessAsync(page, pageSize, searchString, selectedEntityId)
            };
            return Ok(result);

        }
        /// <summary>
        /// Get RCM SubProcess details by id
        /// </summary>
        /// <param name="subProcessId">SubProcess Id</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>Detail of RCM SubProcess</returns>
        [HttpGet]
        [Route("{subProcessId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSubProcessAC>> GetRcmSubProcessById(string subProcessId, string selectedEntityId)
        {
            var SubProcess = await _rcmSubProcessRepository.GetRcmSubProcessByIdAsync(subProcessId);
            return SubProcess;
        }

        /// <summary>
        /// Add RCM SubProcess detail
        /// </summary>s
        /// <param name="SubProcessAC">RCM SubProcess Details to add</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>RCM SubProcess Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSubProcessAC>> AddRcmSubProcess([FromBody] RcmSubProcessAC SubProcessAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                RcmSubProcessAC rcmSubProcessAC = await _rcmSubProcessRepository.AddRcmSubProcess(SubProcessAC);
                return Ok(rcmSubProcessAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update RCM SubProcess detail
        /// </summary>
        /// <param name="SubProcessAC">Update RCM SubProcess Detail </param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>RCM SubProcess Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSubProcessAC>> UpdateRcmSubProcess([FromBody] RcmSubProcessAC SubProcessAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result = await _rcmSubProcessRepository.UpdateRcmSubProcess(SubProcessAC);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete RCM SubProcess 
        /// <param name="subProcessId">id of delete SubProcess </param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{subProcessId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteRcmSubProcess(string subProcessId, string selectedEntityId)
        {
            try
            {
                await _rcmSubProcessRepository.DeleteRcmSubProcess(subProcessId);
                return NoContent();
            }
            catch (DuplicateRCMException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Export Rcm subprocess
        /// <summary>
        /// Method of export to excel for  Rcm subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportRcmSubProcess")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportRcmSubProcessAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _rcmSubProcessRepository.ExportRcmSubProcessAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
    }
}
