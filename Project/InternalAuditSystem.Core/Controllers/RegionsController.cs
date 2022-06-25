using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.Exceptions;
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
    public  class RegionsController : Controller
    {
        public IRegionRepository _regionRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public RegionsController(IRegionRepository regionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _regionRepository = regionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the regions under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of regions under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<RegionAC>>> GetAllRegionsAsync(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            //create pagination wise data
            var pagination = new Pagination<RegionAC>
            {
                EntityId = new Guid(selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };

            return await _regionRepository.GetAllRegionPageWiseAndSearchWiseAsync(pagination);
        }

        /// <summary>
        /// Get data of a particular region under an auditable entity
        /// </summary>
        /// <param name="id">Id of the region</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular region data</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<RegionAC> GetRegionByIdAsync(string id, string selectedEntityId)
        {
            return await _regionRepository.GetRegionByIdAsync(id, selectedEntityId);
        }

        /// <summary>
        /// Add new region under an auditable entity
        /// </summary>
        /// <param name="regionDetails">Details of region</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added region details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RegionAC>> AddRegionAsync([FromBody] RegionAC regionDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _regionRepository.AddRegionAsync(regionDetails, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update region under an auditable entity
        /// </summary>
        /// <param name="updatedRegionsDetails">Updated details of region</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateRegionAsync([FromBody] RegionAC updatedRegionsDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _regionRepository.UpdateRegionAsync(updatedRegionsDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete region from an auditable entity
        /// </summary>
        /// <param name="id">Id of the region that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRegionAsync(string id, string selectedEntityId)
        {
            try
            {
                await _regionRepository.DeleteRegionAsync(id, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region export to excel auditable entity region
        /// <summary>
        /// Method of export to excel for auditable entity region
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportEntityRegions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportEntityRegionsAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _regionRepository.ExportEntityRegionsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion
    }
}
