using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using InternalAuditSystem.Repository.Repository.DistributionRepository;
using System.Collections.Generic;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Utility.Constants;
using System.IO;
using InternalAuditSystem.Core.ActionFilters;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorsController : Controller
    {
        public IDistributionRepository _distributionRepository;
        public DistributorsController(IDistributionRepository distributionRepository)
        {
            _distributionRepository = distributionRepository;
        }


        /// <summary>
        /// Get all distributors details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>list of distributors details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<DistributorsAC>>> GetDistributors(int? page, int pageSize, string searchString, string selectedEntityId)
        {

            var result = new Pagination<DistributorsAC>
            {
                TotalRecords = await _distributionRepository.GetDistributorsCountAsync(searchString, selectedEntityId),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _distributionRepository.GetDistributorsAsync(page, pageSize, searchString, selectedEntityId)
            };
            return Ok(result);
        }


        /// <summary>
        /// Get all users details  
        /// <param name="selectedEntityId">Selected entity id</param>
        /// </summary>
        /// <returns>list of user details</returns>
        [HttpGet]
        [Route("{selectedEntityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<EntityUserMappingAC>>> GetUsers(string selectedEntityId)
        {
            var userDetails = await _distributionRepository.GetUsersAsync(selectedEntityId);
            return Ok(userDetails);
        }


        /// <summary>
        /// Delete Distributor 
        /// <param name="id">id of delete distributor </param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteDistributor(string id, string selectedEntityId)
        {
            try
            {
                await _distributionRepository.DeleteDistributorAsync(id);
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Add Distributor detail
        /// </summary>s
        /// <param name="distributors">Distributor Details to add</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddDistributors([FromBody] List<EntityUserMappingAC> distributors,string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _distributionRepository.AddDistributorsAsync(distributors);
            return Ok();
        }

        /// <summary>
        /// Export Distributor to excel
        /// </summary>
        /// <param name="entityId">Selected Entity Id </param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("exportDistributors")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportDistributors(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _distributionRepository.ExportDistributorsAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
    }
}
