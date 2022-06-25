using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.AuditableEntityRepository.RiskAssessmentRepository
{
    public interface IRiskAssessmentRepository
    {
        /// <summary>
        /// Get RiskAssessment list for grid in list page
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of risk assessment</returns>
        Task<Pagination<RiskAssessmentAC>> GetRiskAssessmentListAsync(Pagination<RiskAssessmentAC> pagination);

        /// <summary>
        /// Add risk assessment details
        /// </summary>
        /// <param name="riskAssessmentAC">RiskAssessmentAC</param>
        /// <returns>RiskAssessmentAC</returns>
        Task<RiskAssessmentAC> AddRiskAssessmentAsync(RiskAssessmentAC riskAssessmentAC);

        /// <summary>
        /// Update risk assessment details
        /// </summary>
        /// <param name="riskAssessmentAC">RiskAssessmentAC</param>
        /// <returns>RiskAssessmentAC</returns>
        Task<RiskAssessmentAC> UpdateRiskAssessmentAsync(RiskAssessmentAC riskAssessmentAC);

        /// <summary>
        /// Delete RiskAssessment
        /// </summary>
        /// <param name="id">RiskAssessment id</param>
        /// <returns>Void</returns>
        Task DeleteRiskAssessmentAync(Guid id);

        /// <summary>
        /// Delete RiskAssessmentDocument from db and from azure
        /// </summary>
        /// <param name="id">RiskAssessmentDocument id</param>
        /// <returns>Void</returns>
        Task DeleteRiskAssessmentDocumentAync(Guid id);

        /// <summary>
        /// Method to download RiskAssessmentDocument
        /// </summary>
        /// <param name="id">RiskAssessmentDocument Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadRiskAssessmentDocumentAsync(Guid id);

        /// <summary>
        /// Get risk assessment details
        /// </summary>
        /// <param name="id">Risk assessment id</param>
        /// <returns>RiskAssessmentAC</returns>
        Task<RiskAssessmentAC> GetRiskAssessmentDetailsAsync(Guid id);

        /// <summary>
        /// Method to Add Risk Assessment Area In New Version
        /// </summary>
        /// <param name="riskAssessmentACList">RiskAssessmentAC list</param>
        /// <param name="versionId">New version entityId</param>
        /// <returns>Void</returns>
        Task AddRiskAssessmentInNewVersionAsync(List<RiskAssessmentAC> riskAssessmentACList, Guid versionId);

    }
}
