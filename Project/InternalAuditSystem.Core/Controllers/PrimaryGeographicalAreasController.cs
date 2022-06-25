using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.PrimaryGeographicalAreaRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryGeographicalAreasController : Controller
    {
        public IPrimaryGeographicalAreaRepository _primaryGeographicalAreaRepository;

        public PrimaryGeographicalAreasController(IPrimaryGeographicalAreaRepository primaryGeographicalAreaRepository)
        {
            _primaryGeographicalAreaRepository = primaryGeographicalAreaRepository;
        }

        /// <summary>
        /// Update PrimaryGeographicalArea
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea AC model</param>
        /// <returns>No content</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _primaryGeographicalAreaRepository.UpdatePrimaryGeographicalAreaAsync(primaryGeographicalAreaAC);
            return NoContent();
        }

        /// <summary>
        /// Add PrimaryGeographicalArea
        /// </summary>
        /// <param name="primaryGeographicalAreaAC">PrimaryGeographicalArea AC model</param>
        /// <returns>PrimaryGeographicalArea AC model</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PrimaryGeographicalAreaAC>> AddPrimaryGeographicalAreaAsync(PrimaryGeographicalAreaAC primaryGeographicalAreaAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await _primaryGeographicalAreaRepository.AddPrimaryGeographicalAreaAsync(primaryGeographicalAreaAC));
        }

        /// <summary>
        /// Get state list by country selection
        /// </summary>
        /// <param name="countryId">Entity country id</param>
        /// <param name="entityId">Selected entity id</param>
        /// <returns>List of state</returns>
        [HttpGet]
        [Route("get-states-by-country")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<ProvinceStateAC>>> GetStateByCountryIdAsync(string countryId, string entityId)
        {
            return await _primaryGeographicalAreaRepository.GetStateACListByCountryAsync(new Guid(countryId), new Guid(entityId));
        }

        /// <summary>
        /// Get country list by region
        /// </summary>
        /// <param name="regionId">Region Id</param>
        /// <param name="entityId">Entity Id</param>
        /// <returns>List of countries for dropdown</returns>
        [HttpGet]
        [Route("get-countries-by-region")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<CountryAC>>> GetCountryListByRegionAsync(string regionId, string entityId)
        {
            return await _primaryGeographicalAreaRepository.GetCountryACListByRegionAsync(new Guid(regionId), new Guid(entityId));
        }

        /// <summary>
        /// Get PrimaryGeographicalAreaAC list for list page
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <param name="entityId">Entityid</param>
        /// <returns>Pagination of PrimaryGeographicalAreaAC</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<PrimaryGeographicalAreaAC>>> GetPrimaryGeographicalAreaListAsync(int? pageIndex, int pageSize, string searchString, string entityId)
        {
            Pagination<PrimaryGeographicalAreaAC> pagination = new Pagination<PrimaryGeographicalAreaAC>()
            {
                EntityId = new Guid(entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return await _primaryGeographicalAreaRepository.GetPrimaryGeographicalAreaListAsync(pagination);
        }

        /// <summary>
        /// Get primary geographical area details of edit and add 
        /// </summary>
        /// <param name="id">PrimaryGeographicalAreaid if on edit</param>
        /// <param name="entityId">Entity id</param>
        /// <returns>PrimaryGeographicalAreaAC</returns>
        [HttpGet]
        [Route("{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PrimaryGeographicalAreaAC>> GetPrimaryGeographicalAreaDetailsAsync(string id, string entityId)
        {
            if (string.IsNullOrEmpty(id))
            {
                return await _primaryGeographicalAreaRepository.GetPrimaryGeographicalAreaDetailsAsync(null, new Guid(entityId));
            }
            else
            {
                return await _primaryGeographicalAreaRepository.GetPrimaryGeographicalAreaDetailsAsync(new Guid(id), new Guid(entityId));

            }
        }


        /// <summary>
        /// Delete PrimaryGeographicalArea from list page
        /// </summary>
        /// <param name="id">PrimaryGeographicalArea id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeletePrimaryGeographicalAreaAsync(string id)
        {
            await _primaryGeographicalAreaRepository.DeletePrimaryGeographicalAreaAync(new Guid(id));
            return NoContent();
        }

        /// <summary>
        /// Method to Add Primary Geographical Area In New Version
        /// </summary>
        /// <param name="primaryGeographicalAreaACList">PrimaryGeographicalAreaAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        [HttpPost]
        [Route("{versionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> AddPrimaryGeographicalAreaInNewVersionAsync(List<PrimaryGeographicalAreaAC> primaryGeographicalAreaACList, string versionId)
        {
            await _primaryGeographicalAreaRepository.AddPrimaryGeographicalAreaInNewVersionAsync(primaryGeographicalAreaACList, new Guid(versionId));
            return NoContent();
        }
    }
}
