using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ClientParticipantRepository;
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
    public class ClientParticipantsController : Controller
    {

        public IClientParticipantRepository _clientParticipantRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public ClientParticipantsController(IClientParticipantRepository clientParticipantRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientParticipantRepository = clientParticipantRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Public API
        /// <summary>
        /// Get all the client participants under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>List of client participants under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<EntityUserMappingAC>>> GetAllClientParticipants(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfParticipants = await _clientParticipantRepository.GetAllClientParticipantsAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId));

            //create pagination wise data
            var result = new Pagination<EntityUserMappingAC>
            {
                Items = listOfParticipants,
                TotalRecords = await _clientParticipantRepository.GetTotalClientParticipantsPerSearchStringAsync(searchString, Guid.Parse(selectedEntityId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get data of a particular client participant under an auditable entity
        /// </summary>
        /// <param name="id">Id of the client participant</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular participant Id</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<UserAC> GetClientParticipantById(string id, string selectedEntityId)
        {
            return await _clientParticipantRepository.GetClientParticipantByIdAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
        }

        /// <summary>
        /// Add new Client participant under an auditable entity
        /// </summary>
        /// <param name="userAc">Details of client participant</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added client participant details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityUserMappingAC>> AddClientParticipant([FromBody] UserAC userAc, string selectedEntityId)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _clientParticipantRepository.AddClientParticipantAsync(userAc, Guid.Parse(selectedEntityId));
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        /// <summary>
        /// Update Client participant under an auditable entity
        /// </summary>
        /// <param name="updatedClientDetails">Updated details of client participant</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateClientParticipant([FromBody] UserAC updatedClientDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _clientParticipantRepository.UpdateClientParticipantAsync(updatedClientDetails, Guid.Parse(selectedEntityId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Client participant from an auditable entity
        /// </summary>
        /// <param name="id">Id of the client participant that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteClientParticipant(string id, string selectedEntityId)
        {
            try
            {
                await _clientParticipantRepository.DeleteClientParticipantAsync(Guid.Parse(id), Guid.Parse(selectedEntityId));
            return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Export client participant
        /// <summary>
        /// Method of export to excel for client participant
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportClientParticipants")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditClientParticipantsAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _clientParticipantRepository.ExportAuditClientParticipantsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

        #endregion

    }
}
