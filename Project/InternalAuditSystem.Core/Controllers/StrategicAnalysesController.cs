using Azure;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels;
using InternalAuditSystem.Repository.AzureBlobStorage;
using InternalAuditSystem.Repository.Exceptions;
using InternalAuditSystem.Repository.Repository.StrategicAnalysisRepository;
using InternalAuditSystem.Utility.Constants;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StrategicAnalysesController : Controller
    {
        #region Public variables

        public IHttpContextAccessor _httpContextAccessor;
        public IFileUtility _fileUtility;
        public IAzureRepository _azureRepository;
        public IStrategicAnalysisRepository _strategicAnalysisRepository;
        public IConfiguration _configuration;

        #endregion

        #region Constructor
        public StrategicAnalysesController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IFileUtility fileUtilty, IAzureRepository azureRepository, IFileUtility fileUtility, IStrategicAnalysisRepository strategicAnalysisRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _azureRepository = azureRepository;
            _fileUtility = fileUtility;
            _strategicAnalysisRepository = strategicAnalysisRepository;
        }
        #endregion

        #region Strategic analysis CRUD

        /// <summary>
        /// Get All Strategic Analyses
        /// </summary>
        /// <param name="page">Current Selected Page</param>
        /// <param name="pageSize">No. of items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="isSampling">Determines if this strategic analysis is of sampling</param>
        /// <param name="rcmId">Rcm Id for sampling list</param>
        /// <param name="isCallFromAdmin">is call from admin or user</param>
        /// <returns>Pagination containing list of strategic analysis model</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<StrategicAnalysisAC>>> GetAllStrategicAnalyses(int? page, int pageSize, string searchString, bool isSampling, string rcmId, bool isCallFromAdmin = true)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Pagination<StrategicAnalysisAC> pagination = new Pagination<StrategicAnalysisAC>()
            {
                PageIndex = page ?? 1,
                PageSize = pageSize,
                searchText = searchString
            };
            return Ok(await _strategicAnalysisRepository.GetAllStrategicAnalysisAsync(pagination, isSampling, rcmId, isCallFromAdmin));
        }



        /// <summary>
        /// Get strategic analysis by id
        /// </summary>
        /// <param name="id">Strategic Analysis Id</param>
        /// <param name="riskControlMatrixId">riskcontrolmatric id</param>
        /// <param name="isGeneralPage">Bit to check whether to get general page data or not/param>
        /// <param name="entityId">selected entity id</param>
        /// <returns>Details of strategic analysis</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StrategicAnalysisAC>> GetStrategicAnalysisById(string id, string riskControlMatrixId, bool isGeneralPage, string entityId)
        {
            if (!string.IsNullOrEmpty(riskControlMatrixId))
            {
                if (entityId != null)
                {
                    return await _strategicAnalysisRepository.GetStrategicAnalysisById(id, new Guid(riskControlMatrixId), isGeneralPage, entityId);

                }
                else
                {
                    return await _strategicAnalysisRepository.GetStrategicAnalysisById(id, new Guid(riskControlMatrixId), isGeneralPage, string.Empty);
                }
            }
            else
            {
                if (entityId == null)
                {
                    return await _strategicAnalysisRepository.GetStrategicAnalysisById(id, null, isGeneralPage, string.Empty);
                }
                else
                {
                    return await _strategicAnalysisRepository.GetStrategicAnalysisById(id, null, isGeneralPage, entityId);
                }

            }
        }

        /// <summary>
        /// Get strategicAnalysisId of draft status
        /// </summary>
        /// <returns>First strategic analysis id of strategic analysis whose status is draft</returns>
        [HttpGet]
        [Route("strategicanalysisId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetStrategicAnalysisIdOfNextDraftStatus()
        {
            var strategicAnalysisId = await _strategicAnalysisRepository.GetStrategicAnalysisIdOfDraftStatusAsync();
            return strategicAnalysisId;
        }

        /// <summary>
        /// Add Strategic Analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">Strategic Analysis to add</param>
        /// <returns>strategicAnalysisAC</returns>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StrategicAnalysisAC>> AddStrategicAnalysis([FromBody] StrategicAnalysisAC strategicAnalysisAC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                StrategicAnalysisAC createdStrategicAnalysisAC = await _strategicAnalysisRepository.AddStrategicAnalysisAsync(strategicAnalysisAC);
                return Ok(createdStrategicAnalysisAC);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisAC">StrategicAnalysisAC</param>
        /// <returns>Updated strategicAnalysisAC</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<StrategicAnalysisAC>> UpdateStrategicAnalysis([FromForm] StrategicAnalysisAC strategicAnalysisAC)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var updatedStrategicAnalysisAC = await _strategicAnalysisRepository.UpdateStrategicAnalysisAsync(strategicAnalysisAC);
                return Ok(updatedStrategicAnalysisAC);
            }
            catch (InvalidFileCount ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidFileFormate ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set isStrategicAnalysisDone true in auditable entity
        /// </summary>
        /// <param name="strategicAnalysisId">id of Strategic analysis of auditable entity which is to be updated</param>
        /// <param name="userId">responded user id</param>
        /// <param name="entityId">selected entity </param>
        /// <returns>Exception if any, no content otherwise</returns>
        [HttpPut]
        [Route("auditableEntity/{strategicAnalysisId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateStrategicAnalysisDoneInAuditableEntity(string strategicAnalysisId, string userId, string entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                await _strategicAnalysisRepository.UpdateStrategicAnalysisDoneInAuditableEntityAsync(strategicAnalysisId, userId, entityId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete strategic analysis
        /// </summary>
        /// <param name="id">StrategicAnalysisId</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteStrategicAnalysis(string id)
        {
            try
            {
                await _strategicAnalysisRepository.DeleteStrategicAnalysisAsync(new Guid(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Strategic analysis team CRUD

        /// <summary>
        /// Delete from strategic analysis team
        /// </summary>
        /// <param name="teamMemberId">Strategic analysis team member id</param>
        /// <param name="strategicAnalysisId">Strategic analysis id</param>
        /// <returns>Throws exception if any otherwise no content response</returns>

        [HttpDelete]
        [Route("teamMembers/{teamMemberId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteStrategicAnalysisTeamMember(string teamMemberId, string strategicAnalysisId)
        {
            try
            {
                await _strategicAnalysisRepository.DeleteStrategicAnalysisTeamMemberAsync(teamMemberId, strategicAnalysisId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Strategic analysis question
        /// <summary>
        /// Get questions of a particular strategic analysis
        /// </summary>
        /// <param name="strategicAnalysisId">strategic analysis id</param>
        /// <returns>List of questionAC</returns>
        [HttpGet]
        [Route("questions/strategicanalyses/{strategicAnalysisId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<QuestionAC>>> GetQuestions(string strategicAnalysisId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(await _strategicAnalysisRepository.GetQuestionsAsync(strategicAnalysisId));
        }

        /// <summary>
        /// Get question by id
        /// </summary>
        /// <param name="id">question id passed from client side</param>
        /// <returns>QuestionAC</returns>
        [HttpGet]
        [Route("questions/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionAC>> GetQuestionById(string id)
        {
            try
            {
                QuestionAC questionAC = await _strategicAnalysisRepository.GetQuestionById(id);
                return questionAC;
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Add question
        /// </summary>
        /// <param name="questionAC">Question model passed</param>
        /// <param name="strategicAnalysisId">StrategicAnalysis id</param>
        /// <returns>Added QuestionAC</returns>

        [HttpPost("question")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionAC>> AddQuestion(QuestionAC questionAC, string strategicAnalysisId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                QuestionAC resultQuestionAC = await _strategicAnalysisRepository.AddQuestionAsync(questionAC, new Guid(strategicAnalysisId));
                return Ok(resultQuestionAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update question
        /// </summary>
        /// <param name="questionAC">questionAC to be updated</param>
        /// <returns>Updated questionAC</returns>
        [HttpPut("questions")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionAC>> UpdateQuestion([FromBody] QuestionAC questionAC)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var updatedQuestionAC = await _strategicAnalysisRepository.UpdateQuestionAsync(questionAC);
                return Ok(updatedQuestionAC);
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete question by id
        /// </summary>
        /// <param name="questionId">question id of question to be deleted</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        [HttpDelete]
        [Route("questions/{questionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteQuestionById(string questionId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                await _strategicAnalysisRepository.DeleteQuestionByIdAsync(new Guid(questionId));
                return NoContent();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// update questions order
        /// </summary>
        /// <param name="questionAC">list of questions</param>
        /// <returns>Task</returns>
        [HttpPost("updatedQuestions")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<QuestionAC>> UpdatedQuestionsOrder(List<QuestionAC> questionAC)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                await _strategicAnalysisRepository.UpdateQuestionOrderAsync(questionAC);
                return Ok();
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Strategic analysis document

        /// <summary>
        /// Get Uploaded Files names
        /// </summary>
        /// <returns>FileNames of uploaded files</returns>

        [HttpGet("uploadedFileName")]
        [ProducesResponseType(200)]
        public IActionResult GetUploadedFileNames()
        {
            IQueryable<UserResponseDocument> userResponseDocuments = _azureRepository.GetUploadedFileNames<UserResponseDocument>();
            List<string> fileNames = userResponseDocuments.Select(x => x.Path).ToList();
            return Ok(fileNames);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("files/strategicAnalysesDocuments/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteFile(string id)
        {
            await _strategicAnalysisRepository.DeleteDocumentAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get uploaded file url
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        [HttpGet]
        [Route("files/urls/strategicAnalysesDocuments/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetDocumentDownloadUrlAsync(string id)
        {
            return Ok(await _strategicAnalysisRepository.DownloadDocumentAsync(new Guid(id)));
        }

        /// <summary>
        /// Get user wise response of a strategic analysis
        /// </summary>
        /// <param name="page">Current page no.</param>
        /// <param name="pageSize">Per page item size</param>
        /// <param name="searchString">Search string </param>
        /// <param name="strategicAnalysisId">Strategic analysis id whose userwise response is to be fetched</param>
        /// <param name="userId">List of user wise response</param>
        /// <param name="entityId">entity id</param>
        /// <returns>Pagination containing list of user wise responses</returns>
        [HttpGet("userWiseResponses/strategicAnalysisId/userId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Pagination<UserWiseResponseAC>>> GetUserWiseResponse(int? page, int pageSize, string searchString, string strategicAnalysisId, string userId, string entityId)
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
                if (entityId != null)
                {
                    return Ok(await _strategicAnalysisRepository.GetUserWiseResponseAsync(pagination, strategicAnalysisId, userId, null, false, new Guid(entityId)));

                }
                return Ok(await _strategicAnalysisRepository.GetUserWiseResponseAsync(pagination, strategicAnalysisId, userId, null, false, null));
            }
            catch (DuplicateDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update strategic analysis
        /// </summary>
        /// <param name="userResponseAC">UserResponseAC</param>
        /// <returns>Updated strategicAnalysisAC</returns>
        [HttpPost("user-responses")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<List<UserResponseDocumentAC>>> AddAndUploadQuestionFiles([FromForm] UserResponseAC userResponseAC)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var addedUserResponseDocumentACs = await _strategicAnalysisRepository.AddAndUploadQuestionFilesAsync(userResponseAC);
                return Ok(addedUserResponseDocumentACs);
            }
            catch (DuplicateDataException ex)
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
        /// Get email attachments documents
        /// </summary>
        /// <param name="strategyAnalysisId">Id of the strategic analysis</param>
        /// <param name="entityId">Entity Id </param>
        /// <returns>List of files attach for email/returns>
        [HttpGet]
        [Route("email-attachments")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<string>> GetEmailAttachmentDocuments(string strategyAnalysisId, string entityId)
        {
            return Ok(await _strategicAnalysisRepository.GetEmailAttachmentDocumentsAsync(new Guid(strategyAnalysisId), new Guid(entityId)));
        }

        /// <summary>
        ///  Save email attchements for each user
        /// </summary>
        /// <param name="emailAttachmentResponse">Email attachment response</param>
        /// <returns>No content</returns>
        [HttpPut]
        [Route("user-response/save/email-attachments")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<string>> UploadEmailAttachmentDocuments([FromForm] UserResponseAC emailAttachmentResponse)
        {
            try
            {
                await _strategicAnalysisRepository.UploadEmailAttachmentDocumentsAsync(emailAttachmentResponse);
                return NoContent();
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
        /// Save file type questions for final submit of a strategy analysis
        /// </summary>
        /// <param name="formdataList">List of all file type question along with file</param>
        /// <param name="strategyAnalysisId">Id of strategy analysis</param>
        /// <param name="entityId">entity id</param>
        /// <returns>No content</returns>
        [HttpPut]
        [Route("final/file-type-question-response/{strategyAnalysisId}/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<string>> SaveFileTypeQuestionReponseforFinal(IFormCollection formdataList, string strategyAnalysisId, string entityId)
        {
            try
            {
                await _strategicAnalysisRepository.SaveFileTypeQuestionReponseAsync(formdataList, strategyAnalysisId, false, new Guid(entityId));
                return NoContent();
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
        /// Save file type questions for draft submit of a strategy analysis
        /// </summary>
        /// <param name="formdataList">List of all file type question along with file</param>
        /// <param name="strategyAnalysisId">Id of strategy analysis</param>
        /// <param name="entityId">entity Id </param>
        /// <returns>No content</returns>
        [HttpPut]
        [Route("draft/file-type-question-response/{strategyAnalysisId}/{entityId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<string>> SaveFileTypeQuestionReponseforDraft(IFormCollection formdataList, string strategyAnalysisId, string entityId)
        {
            try
            {
                await _strategicAnalysisRepository.SaveFileTypeQuestionReponseAsync(formdataList, strategyAnalysisId, true, new Guid(entityId));
                return NoContent();
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
        #endregion


        /// <summary>
        /// update strategic analysis by id
        /// </summary>
        /// <param name="id">Strategic Analysis Id</param>
        /// <returns>task</returns>
        [HttpPut]
        [Route("setStrategicAnalysis")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> SetStrategicAnalysis(string id)
        {
            await _strategicAnalysisRepository.SetStrategicAnalysis(id);
            return Ok();
        }


        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="strategicId">Id of mom</param>
        /// <param name="offset">offset of client</param>
        /// <param name="entityId">Selected Entity Id</param>
        /// <returns>Pdf file </returns>
        [HttpGet]
        [Route("downloadStrategicPDF")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DownloadStrategicPDF(string strategicId, string entityId, int offset)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _httpContextAccessor.HttpContext.User;

            var pdfFile = await _strategicAnalysisRepository.DownloadStrategicPDFAsync(strategicId, new Guid(entityId), offset, user.Identity.Name);
            return File(pdfFile, StringConstant.PdfContentType, StringConstant.StrategyPdfFileName + "_" + strategicId + StringConstant.PdfFileExtantion);
        }

        #region User survey
        /// <summary>
        /// Get all strategic analysis for dropdown in user side
        /// </summary>
        /// <returns>list of strategic analysis</returns>
        [HttpGet]
        [Route("getAllActiveStrategy")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<StrategicAnalysisAC>>> GetAllActiveStrategicAnalysisAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(await _strategicAnalysisRepository.GetAllActiveStrategicAnalysisAsync(user.Identity.Name));
        }


        #endregion
    }
}
