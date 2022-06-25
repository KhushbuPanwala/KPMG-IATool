using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Repository.Repository.WorkProgramRepository;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
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
    public class WorkProgramsController : Controller
    {
        public IWorkProgramRepository _workProgramRepository;
        public WorkProgramsController(IWorkProgramRepository workProgramRepository)
        {
            _workProgramRepository = workProgramRepository;
        }
        /// <summary>
        /// Add work program
        /// </summary>
        /// <param name="workProgramAC"></param>
        /// <returns>Work program model</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<WorkProgramAC>> AddWorkProgramAsync([FromForm] WorkProgramAC workProgramAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _workProgramRepository.AddWorkProgramAsync(workProgramAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateWorkProgramException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (InvalidFileCount ex)
            {
                return BadRequest(ex.Message);
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
        /// Update work program
        /// </summary>
        /// <param name="workProgramAC"></param>
        /// <returns>work program model</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<WorkProgramAC>> UpdateWorkProgram([FromForm]WorkProgramAC workProgramAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _workProgramRepository.UpdateWorkProgramAsync(workProgramAC));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateWorkProgramException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileCount ex)
            {
                return BadRequest(ex.Message);
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
        /// Get workprogram details by id and details for all initial dropdowns in add page
        /// </summary>
        /// <param name="id">Workprogram Id</param>
        /// <param name="entityId">EntityId</param>
        /// <returns>Detail of workprogram</returns>
        [HttpGet]
        [Route("{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WorkProgramAC>> GetWorkProgramDetailsById(string id, string entityId)
        {
            try
            {
                WorkProgramAC workProgramAC = new WorkProgramAC();

                //Get data for edit page
                if (!string.IsNullOrEmpty(id))
                {
                    workProgramAC = await _workProgramRepository.GetWorkProgramDetailsAsync(id);
                }

                //Get data for dropdowns
                WorkProgramAC workProgramAddDetailsAC = await _workProgramRepository.GetWorkProgramAddDetails(new Guid(entityId));
                workProgramAC.AuditPlanACList = workProgramAddDetailsAC.AuditPlanACList;
                workProgramAC.ProcessACList = workProgramAddDetailsAC.ProcessACList;
                workProgramAC.SubProcessACList = workProgramAddDetailsAC.SubProcessACList;
                workProgramAC.InternalUserAC = workProgramAddDetailsAC.InternalUserAC;
                workProgramAC.ExternalUserAC = workProgramAddDetailsAC.ExternalUserAC;

                return workProgramAC;
            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get work program list for list page
        /// </summary>
        /// <param name="entityId">Global entity Id</param>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">Search text if any</param>
        /// <returns>Pagination of WorkProgramAC</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<WorkProgramAC>>> GetWorkProgramListAsync(string entityId, int? pageIndex, int pageSize, string searchString)
        {
            Pagination<WorkProgramAC> pagination = new Pagination<WorkProgramAC>()
            {
                EntityId = new Guid(entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return await _workProgramRepository.GetWorkProgramListAsync(pagination);
        }

        /// <summary>
        /// Delete workprogram from list page
        /// </summary>
        /// <param name="id">Work program id</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteWorkProgramAsync(string id, string selectedEntityId)
        {
            try
            {
                await _workProgramRepository.DeleteWorkProgramAync(new Guid(id));
                return NoContent();
            }
            catch (DeleteValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">Workpaper id</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetWorkPaperDownloadUrlAsync(string id,string selectedEntityId)
        {

            return Ok(await _workProgramRepository.DownloadWorkPaperAsync(new Guid(id)));
        }

        /// <summary>
        /// Delete workpaper file
        /// </summary>
        /// <param name="id">Work paper Id</param>
        /// <param name="selectedEntityId">Selected Entity Id </param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteWorkPaperAsync(string id, string selectedEntityId)
        {
            await _workProgramRepository.DeleteWorkPaperAync(new Guid(id));
            return NoContent();
        }

        /// <summary>
        /// Export workprogram to excel
        /// </summary>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("export-workprogram")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportWorkProgram(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _workProgramRepository.ExportToExcelAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
    }
}
