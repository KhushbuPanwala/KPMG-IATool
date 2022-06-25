using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditProcessSubProcessRepository;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuditSubProcessesController : Controller
    {
        public IAuditProcessSubProcessRepository _auditProcessSubProcessRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public AuditSubProcessesController(IAuditProcessSubProcessRepository auditProcessSubProcessRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditProcessSubProcessRepository = auditProcessSubProcessRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the audit sub-processes under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit sub-processes under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<ProcessAC>>> GetAllAuditSubProcesses(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfAuditSubProcesses = await _auditProcessSubProcessRepository.GetAllAuditSubProcessesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<ProcessAC>
            {
                Items = listOfAuditSubProcesses,
                TotalRecords = await _auditProcessSubProcessRepository.GetTotalCountOfAuditSubProcessSearchStringWiseAsync(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular audit sub-processes under an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit sub-processes</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit sub-processes data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ProcessAC> GetAuditSubProcessById(string id, string selectedEntityId)
        {
            return await _auditProcessSubProcessRepository.GetAuditProcessOrSubProcessByIdAsync(id, selectedEntityId);
        }

        /// <summary>
        /// Add new audit sub-processes under an auditable entity
        /// </summary>
        /// <param name="auditSubProcessDetails">Details of audit sub-processes/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit sub-processes details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProcessAC>> AddAuditSubProcess([FromBody] ProcessAC auditSubProcessDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditProcessSubProcessRepository.AddAuditProcessOrSubProcessAsync(auditSubProcessDetails, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update audit sub-processes under an auditable entity
        /// </summary>
        /// <param name="updatedSubProcessDetails">Updated details of audit sub-processes</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateAuditSubProcess([FromBody] ProcessAC updatedSubProcessDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditProcessSubProcessRepository.UpdateAuditProcessOrSubProcessAsync(updatedSubProcessDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit sub-processes from an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit sub-processes that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditSubProcess(string id, string selectedEntityId)
        {
            try
            {
                await _auditProcessSubProcessRepository.DeleteAuditProcessOrSubProcessAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}
