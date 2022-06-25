using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.RelationshipTypeRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipTypesController : Controller
    {
        public IRelationshipTypeRepository _relationshipTypeRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public RelationshipTypesController(IRelationshipTypeRepository relationshipTypeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _relationshipTypeRepository = relationshipTypeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the relationship types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of relationship types under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<RelationshipTypeAC>>> GetAllRelationshipTypes(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfRelationshipType = await _relationshipTypeRepository.GetAllRelationshipTypesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<RelationshipTypeAC>
            {
                Items = listOfRelationshipType,
                TotalRecords = await _relationshipTypeRepository.GetTotalCountOfRelationshipTypeSearchStringWiseAsync(searchString, Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular relationship type under an auditable entity
        /// </summary>
        /// <param name="id">Id of the relationship type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular relationship type data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<RelationshipTypeAC>GetRelationshipTypeById(string id, string selectedEntityId)
        {
            return await _relationshipTypeRepository.GetRelationshipTypeByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new relationship type under an auditable entity
        /// </summary>
        /// <param name="relationshipTypeAC">Details of relationship type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added relationship type details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RelationshipTypeAC>> AddRelationshipType([FromBody] RelationshipTypeAC relationshipTypeAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _relationshipTypeRepository.AddRelationshipTypeAsync(relationshipTypeAC, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update relationship type under an auditable entity
        /// </summary>
        /// <param name="updatedRelationshipTypeDetails">Updated details of relationship type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateRelationshipType([FromBody] RelationshipTypeAC updatedRelationshipTypeDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _relationshipTypeRepository.UpdateRelationshipTypeAsync(updatedRelationshipTypeDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete relationship type from an auditable entity
        /// </summary>
        /// <param name="id">Id of the relationship type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRelationshipType(string id, string selectedEntityId)
        {
            try
            {
                await _relationshipTypeRepository.DeleteRelationshipTypeAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel auditable entity relationship type
        /// <summary>
        /// Method of export to excel for auditable entity relationship type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityRelationShipTypes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityRelationShipTypeAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _relationshipTypeRepository.ExportEntityRelationShipTypeAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
