using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.RiskAndControl.RiskControlMatrixRepository
{
    public interface IRiskControlMatrixRepository
    {
        /// <summary>
        /// Get Details of RCM for edit page
        /// </summary>
        /// <param name="id">RCM Id</param>
        /// <param name="selectedEntityId">Selected entityId</param> 
        /// <returns>RiskControlMatrixAC</returns>
        Task<RiskControlMatrixAC> GetRiskControlMatrixDetailsByIdAsync(Guid? id, string selectedEntityId);

        /// <summary>
        /// Add RCM from workprogram page
        /// </summary>
        /// <param name="riskControlMatrixAC">riskControlMatrixAC</param>
        /// <returns>riskControlMatrixAC</returns>
        Task<RiskControlMatrixAC> AddRiskControlMatrixAsync(RiskControlMatrixAC riskControlMatrixAC);

        /// <summary>
        /// Update RCM
        /// </summary>
        /// <param name="riskControlMatrixListAC">RiskControlMatrixAC List</param>
        /// <returns>RiskControlMatrixAC List</returns>
        Task<List<RiskControlMatrixAC>> UpdateRiskControlMatrixAsync(List<RiskControlMatrixAC> riskControlMatrixListAC);

        /// <summary>
        /// Get RCM list for workprogram
        /// </summary>
        /// <param name="pagination">Pagination object</param>
        /// <param name="selectedEntityId">Selected entityId</param> 
        /// <returns>Pagination object</returns>
        Task<Pagination<RiskControlMatrixAC>> GetRCMListForWorkProgramAsync(Pagination<RiskControlMatrixAC> pagination, Guid? workProgramId, string selectedEntityId);

        /// <summary>
        /// Delete RCM detail
        /// <param name="id">id of deleted RCM</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteRcmAsync(Guid id);


        /// <summary>
        /// Method for exporting Rcm main
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportRcmMainAsync(string entityId, int timeOffset);

        /// <summary>
        /// Get rcm related data for bulk upload
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity I</param>
        /// <returns>Detail for rcm bulk upload</returns>
        Task<RCMUploadAC> GetRCMUploadDetailAsync(string selectedEntityId);

        /// <summary>
        /// Upload rcm data
        /// </summary>
        /// <param name="file">Upload file</param>
        /// <returns></returns>
        Task UploadRCMAsync(BulkUpload bulkUpload);
    }
}
