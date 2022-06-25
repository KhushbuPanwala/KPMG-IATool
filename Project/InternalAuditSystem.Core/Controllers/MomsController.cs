using InternalAuditSystem.Core.ActionFilters;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using InternalAuditSystem.Repository.Repository.MomRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(EntityRestrictionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MomsController : Controller
    {
        private readonly IMomRepository _momRepository;
        public MomsController(IMomRepository momRepository)
        {
            _momRepository = momRepository;
        }

        /// <summary>
        /// Get all mom details
        /// </summary>
        /// <param name="pageIndex">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>list of mom details</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<Pagination<MomAC>>> GetAllMomsAsync(int? pageIndex, int pageSize, string searchString,string entityId)
        {

            if (!ModelState.IsValid)
                return BadRequest();
          var result = new Pagination<MomAC>
            {
                TotalRecords = await _momRepository.GetMomCountAsync(searchString, entityId),
                PageIndex = pageIndex ?? 1,
                PageSize = pageSize,
                Items = await _momRepository.GetMomAsync(pageIndex, pageSize, searchString, entityId)
            };
            return Ok(result);
        }



        /// <summary>
        /// Method gets details of particular mom
        /// </summary>
        /// <param name="momId">Id of mom</param>
        /// <param name="entityId">Id of selected entity</param>
        /// <returns>Details of particular mom</returns>
        /// <response code="200">Returns mom of particular Id</response>
        /// <resonse code="500">If it returns server error</resonse>
        /// <resonse code="400">It returns if server was unable to process the request</resonse>
        [HttpGet]
        [Route("add/{momId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MomAC>> GetMomDetailByIdAsync([FromRoute]string momId,string entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            MomAC momDetail = await _momRepository.GetMomDetailByIdAsync(momId.ToString(),entityId);
            return Ok(momDetail);
        }

        /// <summary>
        /// Method for  Adding Mom 
        /// </summary>
        /// <param name="momAc">Application class of Mom</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Id of new added mom and if errors throws then return not found</returns>
        /// <response code="200">Returns all the moms</response>
        /// <resonse code="500">if it returns server error</resonse>
        /// <resonse code="400">It returns if server was unable to process the request</resonse>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MomAC>> AddMomAsync([FromBody]MomAC momAC, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            MomAC getAddedMom = await _momRepository.AddMomAsync(momAC);
            return Ok(getAddedMom);
        }

        /// <summary>
        /// Method for updating particular mom's details
        /// </summary>
        /// <param name="momAc">Application class of mom</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Ok</returns>
        /// <response code="200">Returns Edited details of  mom</response>
        /// <resonse code="500">It returns if server error</resonse>
        /// <resonse code="400">It returns if server was unable to process the request </resonse>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<MomAC>> UpdateMomDetailAsync([FromBody] MomAC momAc, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            MomAC updatedMomAc = await _momRepository.UpdateMomDetailAsync(momAc);
           
            return Ok(updatedMomAc);
        }

        /// <summary>
        /// Method for deleting mom's details
        /// </summary>
        /// <param name="Id">Id of mom</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>ok if sucess otherwise not found</returns>
        /// <response code="200">Returns Edited details of  mom</response>
        /// <resonse code="204">No content </resonse>
        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteMomAsync([FromRoute]Guid Id, string selectedEntityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _momRepository.DeleteMomAsync(Id);
            return NoContent();
        }

        /// <summary>
        ///  Method for getting predefined data for mom
        /// </summary>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>List of users and workprogram</returns>
        [HttpGet]
        [Route("{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MomAC>> GetPredefinedDataForMomAsync([FromRoute]Guid entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            MomAC momAc = await _momRepository.GetPredefinedDataForMomAsync(entityId);
            return Ok(momAc);
        }


        /// <summary>
        /// Export ratings to excel
        /// </summary>
        /// <param name="entityId">Selected Entity Id</param>
        /// <param name="offset">offset of client</param>
        /// <returns>Download file</returns>
        [HttpGet]
        [Route("exportMoms")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ExportMoms(string entityId,int offset)
        {
            Tuple<string, MemoryStream> fileData = await _momRepository.ExportMomsAsync(entityId, offset);
            return File(fileData.Item2, StringConstant.ExcelContentType, fileData.Item1);
        }

        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="momId">Id of mom</param>
        /// <param name="offset">offset of client</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>Pdf file </returns>
        [HttpGet]
        [Route("downloadMomPDF")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DownloadMomPDF(string momId,double offset,string entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
           var pdfFile = await _momRepository.DownloadMomPDFAsync(momId.ToString(), offset);
            return File(pdfFile, StringConstant.PdfContentType, StringConstant.MomPdfFileName+"_"+momId+ StringConstant.PdfFileExtantion);
        }

    }
}
