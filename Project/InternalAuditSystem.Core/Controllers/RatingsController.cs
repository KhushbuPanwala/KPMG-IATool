using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.Repository.RatingRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Repository.Exceptions;
using System.IO;
using InternalAuditSystem.Core.ActionFilters;
using System.Collections.Generic;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : Controller
    {
        public IRatingRepository _ratingRepository;
        public RatingsController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }


        /// <summary>
        /// Get all ratings details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">search string </param>
        /// <returns>list of user details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<RatingAC>>> GetRatings(int? page, int pageSize, string searchString, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = new Pagination<RatingAC>
            {
                TotalRecords = await _ratingRepository.GetRatingCountAsync(searchString, selectedEntityId),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _ratingRepository.GetRatingsAsync(page, pageSize, searchString, selectedEntityId)
            };
            return Ok(result);
        }

        /// <summary>
        /// Get rating details by id
        /// </summary>
        /// <param name="id">Rating Id</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Detail of rating</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RatingAC>> GetRatingById(string id, string selectedEntityId)
        {
            var rating = await _ratingRepository.GetRatingsByIdAsync(id);
            return rating;
        }

        /// <summary>
        /// Get rating details by entity id
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Detail of rating</returns>
        [HttpGet]
        [Route("entity-wise-rating")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<RatingAC>>> GetRatingByEntityId(string selectedEntityId)
        {
            var ratings = await _ratingRepository.GetRatingsByEntityIdAsync(selectedEntityId);
            return ratings;
        }

        /// <summary>
        /// Delete Rating 
        /// <param name="id">id of delete rating </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteRating(string id, string selectedEntityId)
        {
            try
            {

                await _ratingRepository.DeleteRatingAsync(id);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add Rating detail
        /// </summary>s
        /// <param name="rating">Rating Details to add</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Rating Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RatingAC>> AddRating([FromBody] RatingAC rating, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                RatingAC ratingAC = await _ratingRepository.AddRatingAsync(rating);
                return Ok(ratingAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Rating detail
        /// </summary>
        /// <param name="rating">Rating Detail to update</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Rating Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RatingAC>> UpdateRating([FromBody] RatingAC rating, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                RatingAC result = await _ratingRepository.UpdateRatingAsync(rating);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Export ratings to excel
        /// </summary>
        /// <param name="entityId">Id of the selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("exportRatings")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportRatings(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _ratingRepository.ExportRatingsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
    }
}
