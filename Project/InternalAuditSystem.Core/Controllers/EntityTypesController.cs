using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.EntityTypeReporsitory;
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
    public class EntityTypesController : Controller
    {
        public IEntityTypeRepository _entityTypeRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public EntityTypesController(IEntityTypeRepository entityTypeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _entityTypeRepository = entityTypeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the entity types under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of entity types under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityTypeAC>>> GetAllEntityTypes(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfEntityType = await _entityTypeRepository.GetAllEntityTypesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<EntityTypeAC>
            {
                Items = listOfEntityType,
                TotalRecords = await _entityTypeRepository.GetTotalCountOfEntityTypeSearchStringWiseAsync(searchString, Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular entity type under an auditable entity
        /// </summary>
        /// <param name="id">Id of the entity type</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity type data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<EntityTypeAC> GetEntityTypById(string id, string selectedEntityId)
        {
            return await _entityTypeRepository.GetEntityTypeByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new entity type under an auditable entity
        /// </summary>
        /// <param name="entityTypeAC">Details of entity type</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity type details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityTypeAC>> AddEntityType([FromBody] EntityTypeAC entityTypeAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _entityTypeRepository.AddEntityTypeAsync(entityTypeAC, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }


        /// <summary>
        /// Update entity type under an auditable entity
        /// </summary>
        /// <param name="updatedEntityTypeDetails">Updated details of entity type</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateEntityType([FromBody] EntityTypeAC updatedEntityTypeDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _entityTypeRepository.UpdateEntityTypeAsync(updatedEntityTypeDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete entity type from an auditable entity
        /// </summary>
        /// <param name="id">Id of the entity type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteEntityType(string id, string selectedEntityId)
        {
            try
            {
                await _entityTypeRepository.DeleteEntityTypeAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel auditable entity type
        /// <summary>
        /// Method of export to excel for auditable entity type
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityTypes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityTypesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _entityTypeRepository.ExportEntityTypesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
