using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository;
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
    public class SamplingsController : Controller
    {
        #region Public variables

        public IStrategicAnalysisRepository _strategicAnalysisRepository;

        #endregion

        #region Constructor
        public SamplingsController(IStrategicAnalysisRepository strategicAnalysisRepository)
        {
            _strategicAnalysisRepository = strategicAnalysisRepository;
        }
        #endregion

        #region Sampling get

        /// <summary>
        /// Get All sampling data
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <returns>Pagination containing list of samplings</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<StrategicAnalysisAC>>> GetAllSamplingPageAndSearchWise(int? page, int pageSize, string searchString)
        {

            if (!ModelState.IsValid)
                return BadRequest();
            Pagination<StrategicAnalysisAC> pagination = new Pagination<StrategicAnalysisAC>()
            {
                PageIndex = page ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return Ok(await _strategicAnalysisRepository.GetAllAdminSideSamplingsDataAsync(pagination));
        }

        /// <summary>
        /// Get sampling user reponse
        /// </summary>
        /// <param name="page">current page no</param>
        /// <param name="pageSize">total no of items per page</param>
        /// <param name="searchString">search item</param>
        /// <param name="samplingId">Id of the smapling</param>
        /// <param name="userId">Id of the current user</param>
        /// <returns>List of user response</returns>
        [HttpGet("sampling-user-response")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<UserWiseResponseAC>>> GetUserWiseResponse(int? page, int pageSize, string searchString, string samplingId, string rcmId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                Pagination<UserWiseResponseAC> pagination = new Pagination<UserWiseResponseAC>()
                {
                    PageIndex = page ?? 1,
                    PageSize = pageSize,
                    searchText = searchString
                };
                return Ok(await _strategicAnalysisRepository.GetUserWiseResponseAsync(pagination, samplingId, null,rcmId, true));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get sampling response rcm wise
        /// </summary>
        /// <param name="samplingId">Id of the sampling</param>
        /// <param name="rcmId">Id of the rcm</param>
        /// <returns>List of rcm response</returns>
        [HttpGet("rcmwise/sampling-response")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<UserWiseResponseAC>>> GetRcmWiseResponse(string samplingId, string rcmId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                return Ok(await _strategicAnalysisRepository.GetRcmWiseResponseAsync(samplingId,rcmId));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete sampling data 
        /// </summary>
        /// <param name="samplingId">Id of sampling id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{samplingId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteSamplingData(string samplingId)
        {
            try
            {
                await _strategicAnalysisRepository.DeleteStrategicAnalysisAsync(new Guid(samplingId), true);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
