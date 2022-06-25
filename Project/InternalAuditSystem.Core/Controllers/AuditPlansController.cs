using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.AuditPlanRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditPlansController : Controller
    {
        public Guid loggedInUserId;
        public IAuditPlanRepository _auditPlanRepository;
        public AuditPlansController(IAuditPlanRepository auditPlanRepository)
        {
            _auditPlanRepository = auditPlanRepository;
        }

        #region Audit Plan List
        /// <summary>
        /// Get all the audit plans under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit plans under an auditable entity</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<AuditPlanAC>>> GetAllAuditPlansPageWiseAndSearchWise(int? pageIndex, int pageSize, string searchString, string selectedEntityId)
        {
            var listOfAuditProcesses = await _auditPlanRepository.GetAllAuditPlansPageWiseAndSearchWiseAsync(pageIndex, pageSize, searchString, selectedEntityId);

            //create pagination wise data
            var result = new Pagination<AuditPlanAC>
            {
                Items = listOfAuditProcesses,
                TotalRecords = await _auditPlanRepository.GetTotalCountOfAuditPlansAsync(searchString, selectedEntityId),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Get all audit plans for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all audit plans with its id and name only </returns>
        [HttpGet]
        [Route("{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<AuditPlanAC>>> GetAllAuditPlansForDisplayInDropDown(string entityId)
        {
            return await _auditPlanRepository.GetAllAuditPlansForDisplayInDropDownAsync(new Guid(entityId));
        }

        /// <summary>
        /// Update audit Plan status 
        /// </summary>
        /// <param name="updatedAuditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>No content</returns>
        [HttpPut]
        [Route("update-status")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAuditPlanStatus([FromBody] AuditPlanAC updatedAuditPlanDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditPlanRepository.UpdateAuditPlanStatusAsync(updatedAuditPlanDetails, selectedEntityId);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create new version of audit plan
        /// </summary>
        /// <param name="auditPlanId">Id of the selected audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>No content</returns>
        [HttpPost]
        [Route("new-version")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateNewVersionOfAuditPlan(string auditPlanId, string selectedEntityId)
        {
            await _auditPlanRepository.CreateNewVersionOfAuditPlanAsync(Guid.Parse(auditPlanId));
            return NoContent();

        }

        /// <summary>
        /// Delete plan document from audit plan and azure storage
        /// </summary>
        /// <param name="auditPlanId">Id of the plan document</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("delete-plan/{auditPlanId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteAuditPlan(string auditPlanId, string selectedEntityId)
        {
            try
            {
                await _auditPlanRepository.DeleteAuditPlanAsync(Guid.Parse(auditPlanId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Audit Plan - General & Overview
        /// <summary>
        /// Get initial data required before adding or editing one audit plan
        /// </summary>
        /// <param name="entityId">Selected entity id </param>
        /// <returns>Prefilled data </returns>
        [HttpGet]
        [Route("initial-data/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditPlanAC>> GetInitialDataForAuditPlanAdd(string entityId)
        {
            return await _auditPlanRepository.GetIntialDataOfAuditPlanAsync(new Guid(entityId));
        }

        /// <summary>
        /// Get audit plan details section wise
        /// </summary>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <param name="entityId">current entity Id</param>
        /// <param name="sectionType">Section type or submenu type</param>
        /// <returns>Data of the particular section of the audit plan</returns>
        [HttpGet]
        [Route("plan-data/{auditPlanId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AuditPlanAC>> GetAuditPlanDetailsById(string auditPlanId, string entityId, AuditPlanSectionType sectionType)
        {
            return await _auditPlanRepository.GetAuditPlanDetailsByIdAsync(Guid.Parse(auditPlanId), Guid.Parse(entityId), sectionType);
        }

        /// <summary>
        /// Add new audit plan under an auditable entity
        /// </summary>
        /// <param name="auditPlanDetails">Details of audit plan/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added audit plan details</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Guid>> AddAuditPlan([FromBody] AuditPlanAC auditPlanDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditPlanRepository.AddAuditPlanAsync(auditPlanDetails, selectedEntityId);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update audit Plan under an auditable entity
        /// </summary>
        /// <param name="updatedAuditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Audit plan Id</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Guid>> UpdateAuditPlan([FromBody] AuditPlanAC updatedAuditPlanDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                var planId = await _auditPlanRepository.UpdateAuditPlanAsync(updatedAuditPlanDetails, selectedEntityId);
                return Ok(planId);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Audit Plan - Plan Process       
        /// <summary>
        /// Get all the plan processes under an under an audit plan add/edit page with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <returns>List of plan processes under an audit plan add/edit page</returns>
        [HttpGet]
        [Route("get-plan-process/{auditPlanId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<PlanProcessMappingAC>>> GetPlanProcessesPageWiseAndSearchWiseByPlanId(int? pageIndex, int pageSize, string searchString, string selectedEntityId, string auditPlanId)
        {
            var listOfPlanProcesses = await _auditPlanRepository.GetPlanProcessesPageWiseAndSearchWiseByPlanIdAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId), Guid.Parse(auditPlanId));

            //create pagination wise data
            var result = new Pagination<PlanProcessMappingAC>
            {
                Items = listOfPlanProcesses,
                TotalRecords = await _auditPlanRepository.GetTotalCountOfPlanProcessesAsync(searchString, Guid.Parse(auditPlanId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Add new plan process under a audit plan 
        /// </summary>
        /// <param name="planProcessDetails">Details of plan process/param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Added plan process details</returns>
        [HttpPost]
        [Route("add-plan-process")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Guid>> AddPlanProcess([FromBody] PlanProcessMappingAC planProcessDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditPlanRepository.AddPlanProcessAsync(planProcessDetails);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update plan process under a audit plan 
        /// </summary>
        /// <param name="planProcessDetails">Details of plan process</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Audit plan Id</returns>
        [HttpPut]
        [Route("update-plan-process")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePlanProcess([FromBody] PlanProcessMappingAC planProcessDetails, string selectedEntityId)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                //if model state is valid update data
                await _auditPlanRepository.UpdatePlanProcessAsync(planProcessDetails);
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete audit plan process from an auditable entity
        /// </summary>
        /// <param name="planProcessId">Id of plan process</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("delete-plan-process/{planProcessId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeletePlanProcess(string planProcessId, string selectedEntityId)
        {
            try
            {
                await _auditPlanRepository.DeletePlanProcessAsync(Guid.Parse(planProcessId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Audit Plan - Plan Document
        /// <summary>
        /// Get all the plan documents under an under an audit plan add/edit page with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <returns>List of plan documents under an audit plan add/edit page</returns>
        [HttpGet]
        [Route("get-plan-documents/{auditPlanId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<AuditPlanDocumentAC>>> GetPlanDocumentsPageWiseAndSearchWiseByPlanId(int? pageIndex, int pageSize, string searchString, string selectedEntityId, string auditPlanId)
        {

            var listOfAuditProcesses = await _auditPlanRepository.GetPlanDocumentsPageWiseAndSearchWiseByPlanIdAsync(pageIndex, pageSize, searchString, Guid.Parse(selectedEntityId), Guid.Parse(auditPlanId));

            //create pagination wise data
            var result = new Pagination<AuditPlanDocumentAC>
            {
                Items = listOfAuditProcesses,
                TotalRecords = await _auditPlanRepository.GetTotalCountOfPlaDocumentsAsync(searchString, Guid.Parse(auditPlanId)),
                PageIndex = pageIndex ?? 1,
                PageSize = (int)pageSize,
            };

            return Ok(result);
        }

        /// <summary>
        /// Add/Update new plan document under a audit plan
        /// </summary>
        /// <param name="planDocumentDetails">Details of plan document/param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Added plan document details</returns>
        [HttpPost]
        [Route("add-plan-documents")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Guid>> AddAuditPlanDocument([FromForm] AuditPlanDocumentAC planDocumentDetails, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _auditPlanRepository.AddAndUploadPlanDocumentAsync(planDocumentDetails);
                return Ok(result);
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
        /// Get file url and download plan document 
        /// </summary>
        /// <param name="planDocumentId">Plan document id</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("download-file/{planDocumentId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> DownloadPlanDocument(string planDocumentId, string selectedEntityId)
        {
            return Ok(await _auditPlanRepository.DownloadPlanDocumentAsync(Guid.Parse(planDocumentId)));
        }


        /// <summary>
        /// Delete plan document from audit plan and azure storage
        /// </summary>
        /// <param name="planDocumentId">Id of the plan document</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("delete-plan-document/{planDocumentId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeletePlanDocument(string planDocumentId, string selectedEntityId)
        {
            try
            {
                await _auditPlanRepository.DeletePlanDocumentAync(Guid.Parse(planDocumentId));
                return NoContent();
            }
            catch (DeleteLinkedDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region export to excel audit plan
        /// <summary>
        /// Method of export to excel for audit plan
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditPlans")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditPlansAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditPlanRepository.ExportAuditPlansAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

        #region export to excel audit process
        /// <summary>
        /// Method of export to excel for audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAuditPlanProcess")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAuditPlanProcessAsync(string entityId, string auditPlanId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _auditPlanRepository.ExportAuditPlanProcessAsync(entityId, auditPlanId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion
        #endregion

    }
}
