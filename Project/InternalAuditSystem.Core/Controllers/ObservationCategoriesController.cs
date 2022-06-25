using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ObservationCategoryRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
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
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public  class ObservationCategoriesController : Controller
    {
        #region Private variables
        private readonly IObservationCategoryRepository _observationCategoryRepository;
        #endregion

        #region Constructor
        public ObservationCategoriesController(IObservationCategoryRepository observationCategoryRepository)
        {
            _observationCategoryRepository = observationCategoryRepository;
           
        }
        #endregion

        #region Public Methods

        #region Observation Category CRUD 

        /// <summary>
        /// Get all the observations under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        ///  <param name="entityId">Id of auditable entity</param>
        /// <returns>List of observation categories </returns>
        [HttpGet]
        [Route("getObservationCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<ObservationCategoryAC>>> GetAllObservationCategoriesAsync(int? pageIndex, int pageSize, string searchString, string entityId)
        {
            //create pagination wise data
            var pagination = new Pagination<ObservationCategoryAC>
            {
                EntityId = new Guid(entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };

            return await _observationCategoryRepository.GetAllObservationCategoriesPageWiseAndSearchWiseAsync(pagination);
        }


        /// <summary>
        /// Method for getting details of observation category by Id
        /// </summary>
        /// <param name="observationCategoryId">Id of observation category</param>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>Details of observation by id</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ObservationCategoryAC>> GetObservationCategoryDetailsByIdAsync(string observationCategoryId, string entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ObservationCategoryAC observationAC = await _observationCategoryRepository.GetObservationCategoryDetailsByIdAsync(observationCategoryId, entityId);
            return observationAC;
        }

        /// <summary>
        /// Method for saving observation category data
        /// </summary>
        /// <param name="observationCategoryAC">Application class object of observation category</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>Details of observation</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ObservationCategoryAC>> AddObservationCategoryAsync([FromBody] ObservationCategoryAC observationCategoryAC, string entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _observationCategoryRepository.AddObservationCategoryAsync(observationCategoryAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for updating observation
        /// </summary>
        /// <param name="observationCategoryAC">Application class object of ovservation category</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>Updated details of observation category</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ObservationCategoryAC>> UpdateObservationCategoryAsync([FromBody] ObservationCategoryAC observationCategoryAC,string entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
               await _observationCategoryRepository.UpdateObservationCategoryAsync(observationCategoryAC);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method for deleting observation category
        /// </summary>
        /// <param name="observationCategoryId">Id of observation category</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("{observationCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteObservationCategoryAsync(string observationCategoryId, string entityId)
        {
            try
            {
                await _observationCategoryRepository.DeleteObservationCategoryAsync(observationCategoryId, entityId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel observation category
        /// <summary>
        /// Method of export to excel for observation category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportObservationCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportObservationCategoriesAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _observationCategoryRepository.ExportObservationCategoriesAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

        #endregion

        #endregion

    }
}
