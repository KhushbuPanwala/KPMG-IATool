using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.EntityCategoryRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InternalAuditSystem.Utility.Constants;
using System.IO;
namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class EntityCategoriesController : Controller
    {
        public IEntityCategoryRepository _entityCategoryRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public EntityCategoriesController(IEntityCategoryRepository enittyCategoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _entityCategoryRepository = enittyCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the audit catgory under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit catgory under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityCategoryAC>>> GetAllEntityCategory(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfAuditType = await _entityCategoryRepository.GetAllEntityCategoryPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString,Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<EntityCategoryAC>
            {
                Items = listOfAuditType,
                TotalRecords = await _entityCategoryRepository.GetTotalCountOfEntityCategorySearchStringWiseAsync(searchString, Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular entity catgory under an auditable entity
        /// </summary>
        /// <param name="id">Id of the entity catgory</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular entity catgory data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<EntityCategoryAC> GetEntityCategoryById(string id, string selectedEntityId)
        {
            return await _entityCategoryRepository.GetEntityCategoryByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new entity catgory under an auditable entity
        /// </summary>
        /// <param name="enittyCategoryAC">Details of entity catgory</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added entity catgory details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityCategoryAC>> AddEntityCategory([FromBody] EntityCategoryAC enittyCategoryAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _entityCategoryRepository.AddEntityCategoryAsync(enittyCategoryAC, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update entity catgory under an auditable entity
        /// </summary>
        /// <param name="updatedEntityCategoryDetails">Updated details of entity catgory</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateEntityCategory([FromBody] EntityCategoryAC updatedEntityCategoryDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _entityCategoryRepository.UpdateEntityCategoryAsync(updatedEntityCategoryDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete entity catgory from an auditable entity
        /// </summary>
        /// <param name="id">Id of the entity catgory that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteEntityCategory(string id, string selectedEntityId)
        {
            try
            {
                await _entityCategoryRepository.DeleteEntityCategoryAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel auditable entity category
        /// <summary>
        /// Method of export to excel for auditable entity category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityCategoriesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _entityCategoryRepository.ExportEntityCategoriesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}