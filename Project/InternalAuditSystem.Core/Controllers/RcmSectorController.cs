using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RiskAndControl.RcmSectorRepository;
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
    public class RcmSectorController : Controller
    {
        public IRcmSectorRepository _RcmSectorRepository;
        public RcmSectorController(IRcmSectorRepository RcmSectorRepository)
        {
            _RcmSectorRepository = RcmSectorRepository;
        }

        /// <summary>
        /// Get all Sector for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all Sector with its id and name only </returns>
        [HttpGet]
        [Route("export-excel/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RcmSectorAC>>> GetAllSectorForDisplayInDropDown(string entityId)
        {
            return await _RcmSectorRepository.GetAllSectorForDisplayInDropDownAsync(new Guid(entityId));
        }

        /// <summary>
        /// Get all RCM sector details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search valee</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>list of RCM sector details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<RcmSectorAC>>> GetRcmSector(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = new Pagination<RcmSectorAC>
            {
                TotalRecords = await _RcmSectorRepository.GetRcmSectorCountAsync(selectedEntityId, searchString),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _RcmSectorRepository.GetRcmSectorAsync(page, pageSize, searchString, selectedEntityId)
            };
            return Ok(result);

        }
        /// <summary>
        /// Get RCM Sector details by id
        /// </summary>
        /// <param name="sectorId">Sector Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Detail of RCM Sector</returns>
        [HttpGet]
        [Route("{sectorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSectorAC>> GetRcmSectorById(string sectorId, string selectedEntityId)
        {
            var sector = await _RcmSectorRepository.GetRcmSectorByIdAsync(sectorId);
            return sector;
        }

        /// <summary>
        /// Add RCM Sector detail
        /// </summary>s
        /// <param name="sectorAC">RCM Sector Details to add</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>RCM Sector Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSectorAC>> AddRcmSector([FromBody] RcmSectorAC sectorAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                RcmSectorAC rcmSectorAC = await _RcmSectorRepository.AddRcmSector(sectorAC);
                return Ok(rcmSectorAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update RCM Sector detail
        /// </summary>
        /// <param name="sectorAC">Update RCM Sector Detail </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>RCM Sector Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RcmSectorAC>> UpdateRcmSector([FromBody] RcmSectorAC sectorAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result = await _RcmSectorRepository.UpdateRcmSector(sectorAC);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete RCM Sector 
        /// <param name="sectorId">id of delete sector </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{sectorId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteRcmSector(string sectorId, string selectedEntityId)
        {
            try
            {
                await _RcmSectorRepository.DeleteRcmSector(sectorId);
                return NoContent();
            }            
            catch (DuplicateRCMException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Export Rcm sector
        /// <summary>
        /// Method of export to excel for  Rcm sector
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportRcmSector")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportRcmSectorAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _RcmSectorRepository.ExportRcmSectorAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
    }
}
