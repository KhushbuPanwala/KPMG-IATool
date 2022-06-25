using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditCategoryRepository;
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
    public class AuditCategoriesController : Controller
    {
        public IAuditCategoryRepository _auditCategoryRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public AuditCategoriesController(IAuditCategoryRepository auditCategoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditCategoryRepository = auditCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the audit categories under an auditable entity with searchstring wise , without
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
        public async Task<ActionResult<Pagination<AuditCategoryAC>>> GetAllAuditCategories(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfAuditCategory = await _auditCategoryRepository.GetAllAuditCategoriesPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<AuditCategoryAC>
            {
                Items = listOfAuditCategory,
                TotalRecords = await _auditCategoryRepository.GetTotalCountOfAuditCategorySearchStringWiseAsync(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular audit category under an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit category</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular aduit category data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<AuditCategoryAC> GetAuditCategoryById(string id, string selectedEntityId)
        {
            return await _auditCategoryRepository.GetAuditCategoryByIdAsync(id, selectedEntityId);
        }

        /// <summary>
        /// Add new audit category under an auditable entity
        /// </summary>
        /// <param name="auditCategoryAC">Details of audit category/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit category details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditCategoryAC>> AddAuditCategory([FromBody] AuditCategoryAC auditCategoryAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditCategoryRepository.AddAuditCategoryAsync(auditCategoryAC, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update audit category under an auditable entity
        /// </summary>
        /// <param name="updatedAuditCategoryDetails">Updated details of audit category</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateAuditCategory([FromBody] AuditCategoryAC updatedAuditCategoryDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditCategoryRepository.UpdateAuditCategoryAsync(updatedAuditCategoryDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit category from an auditable entity
        /// </summary>
        /// <param name="id">Id of the audit category that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditCategory(string id, string selectedEntityId)
        {
            try
            {
                await _auditCategoryRepository.DeleteAuditCategoryAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Export audit category
        /// <summary>
        /// Method of export to excel for audit category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditCategoriesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditCategoryRepository.ExportAuditCategoriesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion


    }
}
