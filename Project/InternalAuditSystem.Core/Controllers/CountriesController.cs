using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.CountryRepository;
using InternalAuditSystem.Repository.Repository.RegionRepository;
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
    public   class CountriesController: Controller
    {
        public ICountryRepository _countryRepository;
        public IHttpContextAccessor _httpContextAccessor;
       
        public CountriesController(ICountryRepository countryRepository,
            IHttpContextAccessor httpContextAccessor, IRegionRepository regionRepository)
        {
            _countryRepository = countryRepository;
            _httpContextAccessor = httpContextAccessor;
           
        }

        #region Public API
        /// <summary>
        /// Get all the countries under an region with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of countries under an region</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityCountryAC>>> GetAllCountryAsync(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            //create pagination wise data
            var pagination = new Pagination<EntityCountryAC>
            {
                EntityId = new Guid(selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };

            return await _countryRepository.GetAllCountryPageWiseAndSearchWiseAsync(pagination);
        }


        /// <summary>
        /// Get data of a particular country under an auditable entity
        /// </summary>
        /// <param name="countryId">Id of the country</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular country data</returns>
        [HttpGet]
        [Route("GetCountryById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityCountryAC>> GetCountryByIdAsync(string countryId,string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await _countryRepository.GetCountryByIdAsync(countryId, selectedEntityId);
            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add new country under an auditable entity
        /// </summary>
        /// <param name="countryDetails">Details of country</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Added country details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityCountryAC>> AddCountryAsync([FromBody] EntityCountryAC countryDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _countryRepository.AddCountryAsync(countryDetails);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update country under an auditable entity
        /// </summary>
        /// <param name="updatedCountryDetails">Updated details of country</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateCountryAsync([FromBody] EntityCountryAC updatedCountryDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _countryRepository.UpdateCountryAsync(updatedCountryDetails);
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
        /// Delete country from an auditable entity
        /// </summary>
        /// <param name="id">Id of the country that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteCountryAsync(string id, string selectedEntityId)
        {
            try
            {
                await _countryRepository.DeleteCountryAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all countries based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Object of EntityCountryAC</returns>
        [HttpGet]
        [Route("getCountries")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityCountryAC>> GetAllCountryBasedOnSearch(string searchString,string selectedEntityId)
        {
            var result = await _countryRepository.GetAllCountryBasedOnSearchAsync(searchString, selectedEntityId);

            return Ok(result);
        }
        #region Export country
        /// <summary>
        /// Method of export to excel for country
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportCountries")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportCountriesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _countryRepository.ExportCountriesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
