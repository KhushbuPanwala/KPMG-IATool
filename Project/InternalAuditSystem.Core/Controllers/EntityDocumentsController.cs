using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository.EntityDocumentRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EntityDocumentsController : Controller
    {

        public IEntityDocumentRepository _entityDocumentRepository;

        public EntityDocumentsController(IEntityDocumentRepository entityDocumentRepository)
        {
            _entityDocumentRepository = entityDocumentRepository;
        }

        /// <summary>
        /// Get EntityDocument details by id for edit page
        /// </summary>
        /// <param name="id">EntityDocument Id</param>
        /// <returns>Detail of EntityDocument</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityDocumentAC>> GetEntityDocumentDetailsByIdAsync(string id)
        {
            return await _entityDocumentRepository.GetEntityDocumentDetailsAsync(new Guid(id));
        }

        /// <summary>
        /// Get EntityDocument list for list page 
        /// </summary>
        /// <param name="entityId">Auditable entity Id</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <returns>Pagination object of Risk Assessment</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityDocumentAC>>> GetEntityDocumentListAsync(string entityId, int? pageIndex, int pageSize, string searchString)
        {
            Pagination<EntityDocumentAC> pagination = new Pagination<EntityDocumentAC>()
            {
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString,
                EntityId = new Guid(entityId)
            };
            return await _entityDocumentRepository.GetEntityDocumentListAsync(pagination);
        }

        /// <summary>
        /// Update EntityDocument
        /// </summary>
        /// <param name="entityDocumentAC">entityDocumentAC</param>
        /// <returns>riskControlMatrixAC</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> UpdateEntityDocumentAsync([FromForm] EntityDocumentAC entityDocumentAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _entityDocumentRepository.UpdateEntityDocumentAsync(entityDocumentAC);
                return NoContent();
            }
            catch (InvalidFileSize ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileFormate ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add EntityDocument
        /// </summary>
        /// <param name="EntityDocumentAC">EntityDocumentAC</param>
        /// <returns>EntityDocumentAC</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<EntityDocumentAC>> AddEntityDocumentAsync([FromForm] EntityDocumentAC entityDocumentAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return await _entityDocumentRepository.AddEntityDocumentAsync(entityDocumentAC);
            }
            catch (InvalidFileSize ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileFormate ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Delete EntityDocument from list page
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteEntityDocumentAync(string id)
        {
            await _entityDocumentRepository.DeleteEntityDocumentAync(new Guid(id));
            return NoContent();
        }

        /// <summary>
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">EntityDocument id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetEntityDocumentDownloadUrlAsync(string id)
        {
            return Ok(await _entityDocumentRepository.DownloadEntityDocumentAsync(new Guid(id)));
        }
    }
}
