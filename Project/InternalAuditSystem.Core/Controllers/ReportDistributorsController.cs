using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using InternalAuditSystem.Repository.Repository.ReportRepository;
using InternalAuditSystem.Core.ActionFilters;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDistributorsController : Controller
    {

        public IReportRepository _reportRepository;

        public ReportDistributorsController(IReportRepository reportRepository)
        {

            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Get all distributors details
        /// </summary>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="reportId">Selected Report Id</param>
        /// <returns>list of distributors details</returns>
        [HttpGet]
        [Route("get/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReportDistributorsAC>> GetDistributorsByReportId(string entityId, string reportId)
        {
            ReportDistributorsAC reportDistributors = await _reportRepository.GetDistributorsByReportIdAsync(entityId, reportId);
            return Ok(reportDistributors);
        }

        /// <summary>
        /// Add Distributor detail
        /// </summary>
        /// <param name="distributors">Distributor Details to add</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddReportDistributors([FromBody] List<ReportUserMappingAC> distributors, string entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _reportRepository.AddDistributorsAsync(distributors);
            return Ok();
        }
    }
}
