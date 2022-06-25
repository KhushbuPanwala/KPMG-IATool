using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditProcessesController : Controller
    {
        public IAuditProcessSubProcessRepository _auditProcesssSubProcessRepository;
        public IHttpContextAccessor _httpContextAccessor;
        public IAuditPlanRepository _auditPlanRepository;

        public AuditProcessesController(IAuditProcessSubProcessRepository auditProcesssSubProcessRepository,
            IHttpContextAccessor httpContextAccessor, IAuditPlanRepository auditPlanRepository)
        {
            _auditProcesssSubProcessRepository = auditProcesssSubProcessRepository;
            _httpContextAccessor = httpContextAccessor;
            _auditPlanRepository = auditPlanRepository;
        }

        #region Public API
        /// <summary>
        /// Get all the audit processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit processes under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<ProcessAC>>> GetAllAuditProcesses(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
         {
            var listOfAuditProcesses = await _auditProcesssSubProcessRepository.GetAllAuditProcessesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<ProcessAC>
            {
                Items = listOfAuditProcesses,
                TotalRecords = await _auditProcesssSubProcessRepository.GetTotalCountOfAuditProcessSearchStringWiseAsync(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular audit process under an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit process</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit category data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ProcessAC> GetAuditProcessById(string id, string selectedEntityId)
        {
            return await _auditProcesssSubProcessRepository.GetAuditProcessOrSubProcessByIdAsync(id, selectedEntityId);
        }

        /// <summary>
        /// Get all process and subprocess under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit processes</returns>
        [HttpGet]
        [Route("entityWiseProcesses/{selectedEntityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ProcessAC>>> GetEntityWiseAllProcessesByEntityIdAsync(string selectedEntityId)
        {
            return await _auditProcesssSubProcessRepository.GetAllProcessSubProcessesByEntityIdAsync(new Guid(selectedEntityId));
        }

        /// <summary>
        /// Get all processes & subprocesses under a plan 
        /// </summary>
        /// <param name="auditPlanId">Id of the audit plan</param>
        /// <returns>List of processes & subprocesses under plan containing their name and id only</returns>
        [HttpGet]
        [Route("planWiseProcesses/{planId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ProcessAC>>> GetPlanWiseAllProcessesByPlanIdAsync(string planId)
        {
            return await _auditPlanRepository.GetPlanWiseAllProcessesByPlanIdAsync(new Guid(planId));
        }

        /// <summary>
        /// Get only all the audit processes under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all audit processes</returns>
        [HttpGet]
        [Route("getOnlyProcesses/{selectedEntityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ProcessAC>>> GetOnlyProcessesByEntityIdAsync(string selectedEntityId)
        {
            return await _auditProcesssSubProcessRepository.GetOnlyProcessesByEntityIdAsync(new Guid(selectedEntityId));
        }

        /// <summary>
        /// Add new audit process under an auditable entity
        /// </summary>
        /// <param name="auditProcessDetails">Details of audit process/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit process details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProcessAC>> AddAuditProcess([FromBody] ProcessAC auditProcessDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditProcesssSubProcessRepository.AddAuditProcessOrSubProcessAsync(auditProcessDetails, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update audit process under an auditable entity
        /// </summary>
        /// <param name="updatedProcessDetails">Updated details of audit process</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateAuditProcess([FromBody] ProcessAC updatedProcessDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditProcesssSubProcessRepository.UpdateAuditProcessOrSubProcessAsync(updatedProcessDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit process from an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit process that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditProcess(string id, string selectedEntityId)
        {
            try
            {
                await _auditProcesssSubProcessRepository.DeleteAuditProcessOrSubProcessAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Export audit process
        /// <summary>
        /// Method of export to excel for audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditProcesses")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditProcessesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditProcesssSubProcessRepository.ExportAuditProcessesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

        #region Export audit subprocess
        /// <summary>
        /// Method of export to excel for audit subprocess
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditSubProcesses")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditSubProcessesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditProcesssSubProcessRepository.ExportAuditSubProcessesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
