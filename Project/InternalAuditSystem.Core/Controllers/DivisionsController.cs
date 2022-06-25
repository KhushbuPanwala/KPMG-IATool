using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.DivisionRepository;
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
    public class DivisionsController : Controller
    {
        public IDivisionRepository _divisionRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public DivisionsController(IDivisionRepository divisionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _divisionRepository = divisionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the divisions under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of divisions under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<DivisionAC>>> GetAllDivision(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfDivision = await _divisionRepository.GetAllDivisionPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<DivisionAC>
            {
                Items = listOfDivision,
                TotalRecords = await _divisionRepository.GetTotalCountOfDivisionSearchStringWiseAsync(searchString, Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular division under an auditable entity
        /// </summary>
        /// <param name="id">Id of the division</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular division data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<DivisionAC> GetDivisionById(string id, string selectedEntityId)
        {
            return await _divisionRepository.GetDivisionByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new division under an auditable entity
        /// </summary>
        /// <param name="divisionDetails">Details of division</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added division details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DivisionAC>> AddDivision([FromBody] DivisionAC divisionDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _divisionRepository.AddDivisionAsync(divisionDetails, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update division under an auditable entity
        /// </summary>
        /// <param name="updatedDivisionDetails">Updated details of division</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateDivision([FromBody] DivisionAC updatedDivisionDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _divisionRepository.UpdateDivisionAsync(updatedDivisionDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete division from an auditable entity
        /// </summary>
        /// <param name="id">Id of the division that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteDivision(string id, string selectedEntityId)
        {
            try
            {
                await _divisionRepository.DeleteDivisionAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region export to excel auditable entity division
        /// <summary>
        /// Method of export to excel for auditable entity division
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityDivisions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityDivisionsAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _divisionRepository.ExportEntityDivisionsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
