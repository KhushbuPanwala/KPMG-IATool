using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditPlanRepository
{
    public interface IAuditPlanRepository
    {
        #region Audit Plan- List
        /// <summary>
        /// Get all the audit plans  under an auditable entity with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>List of audit plans under an auditable entity</returns>
        Task<List<AuditPlanAC>> GetAllAuditPlansPageWiseAndSearchWiseAsync(int? pageIndex, int? pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="selectedEntityId">selected Entity Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfAuditPlansAsync(string searchString, string selectedEntityId);

        /// <summary>
        ///  Update plan status 
        /// </summary>
        /// <param name="updatedAuditPlanDetails">Updated plan details</param>
        /// <param name="selectedEntityId">seelected auditable entity</param>
        /// <returns>Erro or successful transaction</returns>
        Task UpdateAuditPlanStatusAsync(AuditPlanAC updatedAuditPlanDetails, string selectedEntityId);

        /// <summary>
        /// Create new version of audit plan
        /// </summary>
        /// <param name="auditPlanId">Id of the audit plan</param>
        /// <returns>Task</returns>
        Task CreateNewVersionOfAuditPlanAsync(Guid auditPlanId);

        /// <summary>
        /// Delete audit plan
        /// </summary>
        /// <param name="auditPlanId">Id of the plan</param>
        /// <returns>Task or exception</returns>
        Task DeleteAuditPlanAsync(Guid auditPlanId);
        #endregion

        #region Audit Plan - General & Overview
        /// <summary>
        /// Get initial data required before adding or editing one audit plan
        /// </summary>
        /// <param name="entityId">Selected entity id </param>
        /// <returns>Prefilled data </returns>
        Task<AuditPlanAC> GetIntialDataOfAuditPlanAsync(Guid entityId);

        /// <summary>
        /// Get audit plan details section wise
        /// </summary>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <param name="entityId">current entity Id</param>
        /// <param name="sectionType">Section type or submenu type</param>
        /// <returns>Data of the particular section of the audit plan</returns>
        Task<AuditPlanAC> GetAuditPlanDetailsByIdAsync(Guid auditPlanId, Guid entityId, AuditPlanSectionType sectionType);

        /// <summary>
        /// Add new audit Plan under an auditable entity
        /// </summary>
        /// <param name="auditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Audit plan Id</returns>
        Task<Guid> AddAuditPlanAsync(AuditPlanAC auditPlanDetails, string selectedEntityId);

        /// <summary>
        /// Update audit Plan under an auditable entity
        /// </summary>
        /// <param name="auditPlanDetails">Details of audit plan</param>
        /// <param name="selectedEntityId">Seleted entity id</param>
        /// <returns>Audit plan Id</returns>
        Task<Guid> UpdateAuditPlanAsync(AuditPlanAC auditPlanDetails, string selectedEntityId);
        #endregion

        #region Audit Plan - Plan Process
        // <summary>
        /// Get all the plan processes under an under an audit plan add/edit page with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Page size set for display</param>
        /// <param name="searchString">search string </param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <param name="auditPlanId">Id of the adudit plan/param>
        /// <returns>List of plan processes under an audit plan add/edit page</returns>
        Task<List<PlanProcessMappingAC>> GetPlanProcessesPageWiseAndSearchWiseByPlanIdAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId, Guid auditPlanId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="auditPlanId">selected audit plan Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfPlanProcessesAsync(string searchString, Guid auditPlanId);

        /// <summary>
        /// Add new plan process under a audit plan 
        /// </summary>
        /// <param name="planProcessObj">Details of plan process/param>
        /// <returns>Added plan process details</returns>
        Task<PlanProcessMappingAC> AddPlanProcessAsync(PlanProcessMappingAC planProcessObj);

        /// <summary>
        /// Update plan process under a audit plan 
        /// </summary>
        /// <param name="updatedPlanProcessDetails">Details of plan process</param>
        /// <returns>Audit plan Id</returns>
        Task UpdatePlanProcessAsync(PlanProcessMappingAC updatedPlanProcessDetails);

        /// <summary>
        /// Delete audit plan process from an auditable entity
        /// </summary>
        /// <param name="planProcessId">Id of plan process</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeletePlanProcessAsync(Guid planProcessId);
        #endregion

        #region Audit Plan - Plan Documents
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
        Task<List<AuditPlanDocumentAC>> GetPlanDocumentsPageWiseAndSearchWiseByPlanIdAsync(int? pageIndex, int? pageSize, string searchString, Guid selectedEntityId, Guid auditPlanId);

        /// <summary>
        /// Get total count of result filtered according to search string 
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="auditPlanId">selected audit plan Id</param>
        /// <returns>Total no of records found for a search string</returns>
        Task<int> GetTotalCountOfPlaDocumentsAsync(string searchString, Guid auditPlanId);

        /// <summary>
        /// Add/Update new plan document under a audit plan
        /// </summary>
        /// <param name="planDocumentDetails">Details of plan document/param>
        /// <returns>Added plan document details</returns>
        Task<AuditPlanDocumentAC> AddAndUploadPlanDocumentAsync(AuditPlanDocumentAC planDocumentObj);

        /// <summary>
        /// Delete plan document from audit plan and azure storage
        /// </summary>
        /// <param name="planDocumentId">Id of the plan document</param>
        /// <returns>Task</returns>
        Task DeletePlanDocumentAync(Guid planDocumentId);

        /// <summary>
        /// Get file url and download plan document 
        /// </summary>
        /// <param name="planDocumentId">Plan document id</param>
        /// <returns>Url of File of particular file name passed</returns>
        Task<string> DownloadPlanDocumentAsync(Guid planDocumentId);
        #endregion

        #region General Methods
        /// <summary>
        /// Get all audit plans for showing in dropdown for other modules
        /// </summary>
        /// <param name="entityId">Id of the selected auditable entity</param>
        /// <returns>List of all audit plans with its id and name only </returns>
        Task<List<AuditPlanAC>> GetAllAuditPlansForDisplayInDropDownAsync(Guid entityId);

        /// <summary>
        /// Get all processes & subprocesses under a plan 
        /// </summary>
        /// <param name="auditPlanId">Id of the audit plan</param>
        /// <returns>List of processes & subprocesses under plan containing their name and id only</returns>
        Task<List<ProcessAC>> GetPlanWiseAllProcessesByPlanIdAsync(Guid auditPlanId);

        /// <summary>
        /// Get all plans and its processes & subprocesses of all plans under an auditable entity
        /// </summary>
        /// <param name="entityId">Id of the auditable entity</param>
        /// <returns>List of all plans and its processes & subprocesses of all plans containing their name and id only</returns>
        Task<List<AuditPlanAC>> GetAllPlansAndProcessesOfAllPlansByEntityIdAsync(Guid entityId);

        /// <summary>
        /// Method for exporting audit plan
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditPlansAsync(string entityId, int timeOffset);

        /// <summary>
        /// Method for exporting audit process
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportAuditPlanProcessAsync(string entityId, string auditPlanId, int timeOffset);
        #endregion
    }
}
