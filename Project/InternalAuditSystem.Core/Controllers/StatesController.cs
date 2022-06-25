using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ProvinceStateRepository;
using InternalAuditSystem.Repository.Repository.RegionRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class StatesController : Controller
    {
        public IStateRepository _stateRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public StatesController(IStateRepository stateRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _stateRepository = stateRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Public API
        /// <summary>
        /// Get all the states under an region with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of states under a state</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityStateAC>>> GetAllStatesAsync(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            //create pagination wise data
            var pagination = new Pagination<EntityStateAC>
            {
                EntityId = new Guid(selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };

            return await _stateRepository.GetAllStatePageWiseAndSearchWiseAsync(pagination);
        }


        /// <summary>
        /// Get data of a particular state under a country
        /// </summary>
        /// <param name="countryId">Id of the state</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular state data</returns>
        [HttpGet]
        [Route("GetStateById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityStateAC>> GetStateByIdAsync(string countryId, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await _stateRepository.GetStateByIdAsync(countryId, selectedEntityId);
            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add new state under a country
        /// </summary>
        /// <param name="countryDetails">Details of state</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Added state details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityStateAC>> AddStateAsync([FromBody] EntityStateAC stateDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _stateRepository.AddStateAsync(stateDetails);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update state under an auditable entity
        /// </summary>
        /// <param name="updatedStateDetails">Updated details of state</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateStateAsync([FromBody] EntityStateAC updatedStateDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _stateRepository.UpdateStateAsync(updatedStateDetails);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete state from a country
        /// </summary>
        /// <param name="id">Id of the state that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteStateAsync(string id, string selectedEntityId)
        {
            try
            {
                await _stateRepository.DeleteStateAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all states based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityStateAC</returns>
        [HttpGet]
        [Route("getStates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityStateAC>> GetAllStatesBasedOnSearch(string searchString, string selectedEntityId)
        {
            var result = await _stateRepository.GetAllStatesBasedOnSearchAsync(searchString, selectedEntityId);

            return Ok(result);
        }

        #region Export state
        /// <summary>
        /// Method of export to excel for state
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportStates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportStatesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _stateRepository.ExportStatesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
