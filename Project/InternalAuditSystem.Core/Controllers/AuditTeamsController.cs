using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditTeamRepository;
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
    public class AuditTeamsController : Controller
    {

        public IHttpContextAccessor _httpContextAccessor;
        public IAuditTeamRepository _auditTeamRepository;

        public AuditTeamsController(
            IHttpContextAccessor httpContextAccessor, IAuditTeamRepository auditTeamRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditTeamRepository = auditTeamRepository;
        }

        /// <summary>
        /// Get all users based on the search term from active directory
        /// </summary>
        /// <param name="searchString">Search string on Name field</param>
        /// <returns>List of users with ad information</returns>
        [HttpGet]
        [Route("getAdUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<UserAC>>> GetAllUserFromAdBasedOnSearch(string searchString)
        {
            var result = await _auditTeamRepository.GetAllUserFromAdBasedOnSearchAsync(searchString);

            return Ok(result);
        }

        /// <summary>
        /// Get all the audit team members under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>List of audit team member under an auditable entity</returns>
        [ServiceFilter(typeof(EntityRestrictionFilter))]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityUserMappingAC>>> GetAllAuditTeamMembers(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfParticipants = await _auditTeamRepository.GetAllAuditTeamMemebersAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<EntityUserMappingAC>
            {
                Items = listOfParticipants,
                TotalRecords = await _auditTeamRepository.GetTotalAuditTeamMembersPerSearchString(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Add new audit team member under an auditable entity
        /// </summary>
        /// <param name="userDetails">Details of audit team member</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit team member details</returns>
        [ServiceFilter(typeof(EntityRestrictionFilter))]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityUserMappingAC>> AddNewAuditTeamMember([FromBody] UserAC userDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditTeamRepository.AddNewAuditTeamMemberAsync(userDetails, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sycn Audit team Member data with Azure directive
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="selectedEntityId">Selected Entity id</param>
        /// <returns>Updated synced data</returns>
        [ServiceFilter(typeof(EntityRestrictionFilter))]
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<Pagination<EntityUserMappingAC>>> SyncAuditTeamData(int? pageIndex, int pageSize, string selectedEntityId)
        {
            try
            {
                var result = await _auditTeamRepository.SyncDataInDbForAuditTeamMembersAsync(pageIndex, pageSize, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit team member from an auditable entity
        /// </summary>
        /// <param name="teamMemberId">Id of the audit type that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [ServiceFilter(typeof(EntityRestrictionFilter))]
        [HttpDelete]
        [Route("{teamMemberId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditTeamMeberFromAuditableEntity(string teamMemberId, string selectedEntityId)
        {
            try
            {
                await _auditTeamRepository.DeleteAuditTeamMemberAsync(teamMemberId, selectedEntityId);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Export Team
        /// <summary>
        /// Method of export to excel for audit team
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditTeams")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditTeamAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditTeamRepository.ExportAuditTeamAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

    }
}
