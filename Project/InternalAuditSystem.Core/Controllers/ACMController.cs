using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.ACMRepresentationRepository;
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
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AcmController : Controller
    {
        public IACMRepository _acmRepository;
        public AcmController(IACMRepository acmRepository)
        {
            _acmRepository = acmRepository;
        }

        /// <summary>
        /// Get all ACM details
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>   
        /// <param name="fromYear">selected from year</param>   
        /// <param name="toYear">selected toyear</param>   
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>list of ACM details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Pagination<ACMPresentationAC>>> GetACMDataAsync(int? page, int pageSize, string searchString, string selectedEntityId, int? fromYear, int? toYear)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = new Pagination<ACMPresentationAC>
            {
                TotalRecords = await _acmRepository.GetACMCountAsync(searchString, selectedEntityId),
                PageIndex = page ?? 1,
                PageSize = pageSize,
                Items = await _acmRepository.GetACMDataAsync(page, pageSize, searchString, selectedEntityId, fromYear, toYear)
            };
            return Ok(result);
        }

        /// <summary>
        /// Get ACM details by id
        /// </summary>
        /// <param name="acmId">ACM Id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Detail of ACM by id</returns>
        [HttpGet]
        [Route("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ACMPresentationAC>> GetACMDetailsByIdAsync(string acmId, string selectedEntityId)
        {
            try
            {
                if (acmId == "0")
                {
                    return await _acmRepository.GetACMDetailsByIdAsync(null, selectedEntityId);
                }
                else
                {
                    return await _acmRepository.GetACMDetailsByIdAsync(Guid.Parse(acmId), selectedEntityId);

                }

            }

            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Add ACM detail
        /// </summary>s
        /// <param name="acmPresentation">ACM Details to add</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>ACM Detail</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ACMPresentationAC>> AddACMAsync([FromForm] ACMPresentationAC acmPresentation, string selectedEntityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                ACMPresentationAC acmPresentationAC = await _acmRepository.AddACMAsync(acmPresentation);
                return Ok(acmPresentationAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update ACM Presentation detail
        /// </summary>
        /// <param name="acmPresentationListAC">Update ACM Presentation Detail </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>ACM Presentation Detail</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ACMPresentationAC>> UpdateACMAsync([FromForm] ACMPresentationAC acmPresentationAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result = await _acmRepository.UpdateACMAsync(acmPresentationAC);
                return Ok(result);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete ACM  
        /// <param name="acmId">id of deleted ACM </param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete]
        [Route("{acmId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAcmPresentation(string acmId, string selectedEntityId)
        {
            try
            {
                await _acmRepository.DeleteAcmAsync(Guid.Parse(acmId));
                return NoContent();
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #region Export Acm
        /// <summary>
        /// Method of export to excel for acm
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Export file</returns>

        [HttpGet]
        [Route("exportAcm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportAcmAsync(string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _acmRepository.ExportAcmAsync(entityId, timeOffset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }
        #endregion

        #region ACM json document table

        /// <summary>
        /// Get acm json document which represents the dynamic table of acm
        /// </summary>
        /// <param name="id">acm id of which table json document is to be fetched</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Serialized json document representation of table data</returns>
        [HttpGet]
        [Route("json-documents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetACMTable(string id, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _acmRepository.GetACMTableAsync(id);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// To update json document in acm Table
        /// </summary>
        /// <param name="jsonDocument">Json document to be updated</param>
        /// <param name="acmId">acm id whose table is to be updated</param>
        /// <param name="tableId">Table if of table to be updated</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Updated json document</returns>
        public async Task<ActionResult<string>> UpdateJsonDocument(string jsonDocument, string acmId, string tableId, string selectedEntityId)
        {
            try
            {
                var updatedJsonDocument = await _acmRepository.UpdateJsonDocumentAsync(jsonDocument, acmId, tableId);
                return updatedJsonDocument;
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Add column in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddColumnInTable(string tableId, string acmId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _acmRepository.AddColumnAsync(tableId, acmId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/rows")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Add row in acmTable's table
        /// </summary>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> AddRow(string tableId, string acmId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _acmRepository.AddRowAsync(tableId, acmId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/rows/id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Delete row in acmTable's table
        /// </summary>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id </param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteRow(string acmId, string tableId, string rowId, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _acmRepository.DeleteRowAsync(acmId, tableId, rowId);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("json-documents/columns/id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        /// <summary>
        /// Delete column in acmTable's table
        /// </summary>
        /// <param name="acmId">acm id to which this json document table is linked</param>
        /// <param name="tableId">Json document table Id</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Updated serialized json document representation of table data</returns>
        public async Task<ActionResult<string>> DeleteColumn(string acmId, string tableId, int columnPosition, string selectedEntityId)
        {
            try
            {
                var jsonDocument = await _acmRepository.DeleteColumnAsync(acmId, tableId, columnPosition);
                return jsonDocument;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region File upload

        ///// <summary>
        ///// Add and upload ACM files
        ///// </summary>
        ///// <param name="ACMAC">ACM containing files</param>
        ///// <returns>List of added ACMDocuments</returns>

        [HttpPost("uploadFiles")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<List<ACMDocumentAC>>> UploadFile([FromForm] ACMPresentationAC acmPresentationAC)
        {
            try
            {
                var addedACMDocumentACs = await _acmRepository.AddAndUploadACMFilesAsync(acmPresentationAC.ACMFiles, (Guid)acmPresentationAC.Id);
                return addedACMDocumentACs;
            }
            catch (DuplicateDataException ex)
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
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">ACM document id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetACMDocumentDownloadUrl(string id, string selectedEntityId)
        {
            return Ok(await _acmRepository.DownloadACMDocumentAsync(new Guid(id)));
        }

        /// <summary>
        /// Delete acm document from db and from azure
        /// </summary>
        /// <param name="id">ACM document id</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("file/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteACMDocument(string id, string selectedEntityId)
        {
            await _acmRepository.DeleteACMDocumentAsync(new Guid(id));
            return NoContent();
        }

        #endregion


        #region ACM PPT 
        /// <summary>
        /// Create ACM PPT 
        /// </summary>
        /// <param name="id">selected acm Id</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("generateACMPPT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> generateACMPPT(string id, string entityId, int timeOffset)
        {
            Tuple<string, MemoryStream> fileData = await _acmRepository.GenerateACMPPTAsync(id, entityId, timeOffset);
            return File(fileData.Item2, StringConstant.PPTContentType, fileData.Item1);
        }
        #endregion

    }
}