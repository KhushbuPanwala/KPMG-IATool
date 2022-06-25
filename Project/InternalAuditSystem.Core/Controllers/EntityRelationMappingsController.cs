using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityRelationMappingRepository;
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
    public class EntityRelationMappingsController : Controller
    {
        public IEntityRelationMappingRepository _entityRelationMappingRepository;

        public EntityRelationMappingsController(IEntityRelationMappingRepository entityRelationMappingRepository)
        {
            _entityRelationMappingRepository = entityRelationMappingRepository;
        }


        /// <summary>
        /// Update EntityRelationMapping
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping AC model</param>
        /// <returns>No content</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _entityRelationMappingRepository.UpdateEntityRelationMappingAsync(entityRelationMappingAC);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add EntityRelationMapping
        /// </summary>
        /// <param name="EntityRelationMappingAC">EntityRelationMapping AC model</param>
        /// <returns>EntityRelationMapping AC model</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityRelationMappingAC>> AddEntityRelationMappingAsync(EntityRelationMappingAC entityRelationMappingAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _entityRelationMappingRepository.AddEntityRelationMappingAsync(entityRelationMappingAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get EntityRelationMappingAC list for list page
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <param name="currentEntityId">Entityid</param>
        /// <returns>Pagination of EntityRelationMappingAC</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityRelationMappingAC>>> GetEntityRelationMappingListAsync(int? pageIndex, int pageSize, string searchString, string currentEntityId)
        {
            Pagination<EntityRelationMappingAC> pagination = new Pagination<EntityRelationMappingAC>()
            {
                EntityId = new Guid(currentEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return await _entityRelationMappingRepository.GetEntityRelationMappingListAsync(pagination);
        }

        /// <summary>
        /// Delete EntityRelationMapping from list page
        /// </summary>
        /// <param name="id">EntityRelationMapping id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteEntityRelationMappingAsync(string id)
        {
            await _entityRelationMappingRepository.DeleteEntityRelationMappingAync(new Guid(id));
            return NoContent();
        }
    }
}
