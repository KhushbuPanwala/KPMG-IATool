using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditTypeRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Core.ActionFilters;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTypesController : Controller
    {
        public IAuditTypeRepository _auditTypeRepository;
        public IHttpContextAccessor _httpContextAccessor;
       
        public AuditTypesController(IAuditTypeRepository auditTypeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditTypeRepository = auditTypeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the audit types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit types under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<AuditTypeAC>>> GetAllAuditTypes(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfAuditType = await _auditTypeRepository.GetAllAuditTypesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<AuditTypeAC>
            {
                Items = listOfAuditType,
                TotalRecords = await _auditTypeRepository.GetTotalCountOfAuditTypeSearchStringWiseAsync(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular audit type under an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit type data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<AuditTypeAC> GetAuditTypeById(string id, string selectedEntityId)
        {
            return await _auditTypeRepository.GetAuditTypeByIdAsync(id, selectedEntityId);
        }

        /// <summary>
        /// Add new audit type under an auditable entity
        /// </summary>
        /// <param name="auditTypeAC">Details of audit type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit type details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditTypeAC>> AddAuditType([FromBody] AuditTypeAC auditTypeAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditTypeRepository.AddAuditTypeAsync(auditTypeAC, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update audit type under an auditable entity
        /// </summary>
        /// <param name="updatedAuditTypeDetails">Updated details of audit type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateAuditType([FromBody] AuditTypeAC updatedAuditTypeDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditTypeRepository.UpdateAuditTypeAsync(updatedAuditTypeDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit type from an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditType(string id, string selectedEntityId)
        {
            try
            {
                await _auditTypeRepository.DeleteAuditTypeAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }         
        }

        #region Export audit type
        /// <summary>
        /// Method of export to excel for audit type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditTypes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditTypesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditTypeRepository.ExportAuditTypesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion


    }
}
