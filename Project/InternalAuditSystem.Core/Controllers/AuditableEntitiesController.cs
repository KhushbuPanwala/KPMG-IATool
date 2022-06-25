using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditableEntityRepository;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuditableEntitiesController : Controller
    {
        public IAuditableEntityRepository _auditableEntityRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public AuditableEntitiesController(IAuditableEntityRepository auditableEntityRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditableEntityRepository = auditableEntityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all the auditable entity whose strategy analysis is done along with current user details
        /// </summary>
        /// <returns>List of auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoggedInUserDetails>> GetPermittedEntitiesOfLoggedInUser()
        {
            var loggedInUserId = Guid.Parse(HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);
            return await _auditableEntityRepository.GetPermittedEntitiesOfLoggedInUserAsync(loggedInUserId);
        }

        /// <summary>
        /// Get auditable entity list for list page
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <returns>Pagination of AuditableEntityAC</returns>
        [HttpGet]
        [Route("get-all-entities")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<AuditableEntityAC>>> GetAuditableEntityListAsync(int? pageIndex, int pageSize, string searchString)
        {
            Pagination<AuditableEntityAC> pagination = new Pagination<AuditableEntityAC>()
            {
                EntityId = new Guid(),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return await _auditableEntityRepository.GetAuditableEntityListAsync(pagination);
        }

        /// <summary>
        /// Get the auditable entity details by id
        /// </summary>
        /// <param name="id">Auditable entity id</param>
        /// <param name="stepNo">Step nnumber</param>
        /// <returns>Auditable entity details</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditableEntityAC>> GetAuditableEntityDetailsAsync(string id, int stepNo)
        {
            return await _auditableEntityRepository.GetAuditableEntityDetailsAsync(new Guid(id), stepNo);
        }

        /// <summary>
        /// Update auditable entity 
        /// </summary>
        /// <param name="auditableEntityAC">Auditable Entity AC model</param>
        /// <returns>No content</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAuditableEntityAsync(AuditableEntityAC auditableEntityAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _auditableEntityRepository.UpdateAuditableEntityAsync(auditableEntityAC);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add auditable entity 
        /// </summary>
        /// <param name="auditableEntityAC">Auditable Entity AC model</param>
        /// <returns>Auditable Entity AC model</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditableEntityAC>> AddAuditableEntityAsync(AuditableEntityAC auditableEntityAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _auditableEntityRepository.AddAuditableEntityAsync(auditableEntityAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add auditable entity 
        /// </summary>
        /// <param name="auditableEntityAC">Auditable Entity AC model</param>
        /// <returns>Auditable Entity AC model</returns>
        [HttpPost]
        [Route("create-new-version")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateNewVersionAsync(Guid auditableEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _auditableEntityRepository.CreateNewVersionAsync(auditableEntityId);
            return NoContent();

        }

        /// <summary>
        /// Delete Auditable Entity from list page
        /// </summary>
        /// <param name="id">Auditable Entity id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditableEntityAsync(string id)
        {
            try
            {
                await _auditableEntityRepository.DeleteAuditableEntityAync(new Guid(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Export auditable entity to excel
        /// </summary>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("export-auditable-entity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditableEntityAsync(int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditableEntityRepository.ExportToExcelAsync( timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
    }
}
