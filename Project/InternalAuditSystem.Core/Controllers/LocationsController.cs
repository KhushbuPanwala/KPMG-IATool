using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.LocationRepository;
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
    public class LocationsController : Controller
    {
        public ILocationRepository _locationRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public LocationsController(ILocationRepository locationRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _locationRepository = locationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the locations under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of locations under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<LocationAC>>> GetAllLocation(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfLocation = await _locationRepository.GetAllLocationPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<LocationAC>
            {
                Items = listOfLocation,
                TotalRecords = await _locationRepository.GetTotalCountOfLocationSearchStringWiseAsync(searchString,Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular location under an auditable entity
        /// </summary>
        /// <param name="id">Id of the location</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular location data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<LocationAC> GetLocationByIdA(string id, string selectedEntityId)
        {
            return await _locationRepository.GetLocationByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new location under an auditable entity
        /// </summary>
        /// <param name="locationDetails">Details of location</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added location details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LocationAC>> AddLocation([FromBody] LocationAC locationDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _locationRepository.AddLocationAsync(locationDetails, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update location under an auditable entity
        /// </summary>
        /// <param name="updatedLocationDetails">Updated details of location</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateLocation([FromBody] LocationAC updatedLocationDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _locationRepository.UpdateLocationAsync(updatedLocationDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete location from an auditable entity
        /// </summary>
        /// <param name="id">Id of the location that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteLocation(string id, string selectedEntityId)
        {
            try
            {
                await _locationRepository.DeleteLocationAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel auditable entity location
        /// <summary>
        /// Method of export to excel for auditable entity location
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityLocations")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityLocationsAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _locationRepository.ExportEntityLocationsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
