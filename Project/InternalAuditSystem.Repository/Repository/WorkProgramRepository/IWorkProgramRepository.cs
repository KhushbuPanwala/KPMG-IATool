using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.WorkProgramRepository
{
    public interface IWorkProgramRepository
    {
        /// <summary>
        /// Get WorkProgram detail by Id
        /// </summary>
        /// <param name="workProgramId">Id of Workprogram</param>
        /// <returns>WorkProgramAC</returns>
        Task<WorkProgramAC> GetWorkProgramDetailsAsync(string id);

        /// <summary>
        /// Add Workprogram to database
        /// </summary>
        /// <param name="workProgramAC">Work program AC model to be added</param>
        /// <returns>Added work program</returns>
        Task<WorkProgramAC> AddWorkProgramAsync(WorkProgramAC workProgram);

        /// <summary>
        /// Get Details for work program add page
        /// </summary>
        /// <param name="entityId">Auditable EntityId</param>
        /// <returns>User and auditplan list</returns>
        Task<WorkProgramAC> GetWorkProgramAddDetails(Guid entityId);


        /// <summary>
        /// Method for fetching WorkProgramAC of auditableEntity
        /// </summary>
        /// <param name="auditableEntityId">Id of auditableEntity</param>
        /// <returns>List of WorkProgramAC</returns>
        Task<List<WorkProgramAC>> GetAllWorkProgramsAsync(Guid auditableEntityId);

        /// <summary>
        /// Get Workprogram list for grid in list page
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of workprogram</returns>
        Task<Pagination<WorkProgramAC>> GetWorkProgramListAsync(Pagination<WorkProgramAC> pagination);

        /// <summary>
        /// Delete work program
        /// </summary>
        /// <param name="id">Work program id</param>
        /// <returns>Work</returns>
        Task DeleteWorkProgramAync(Guid id);

        /// <summary>
        /// Method to download workpaper
        /// </summary>
        /// <param name="id">WorkPaper Id</param>
        /// <returns>Download url string</returns>
        Task<string> DownloadWorkPaperAsync(Guid id);

        /// <summary>
        /// Update work program
        /// </summary>
        /// <param name="workProgramAC">Work program ac</param>
        /// <returns>Work program ac</returns>
        Task<WorkProgramAC> UpdateWorkProgramAsync(WorkProgramAC workProgramAC);

        /// <summary>
        /// Delete work paper from db and from azure
        /// </summary>
        /// <param name="id">Work paper id</param>
        /// <returns>Void</returns>
        Task DeleteWorkPaperAync(Guid id);

        /// Export Workprogram to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
         Task<Tuple<string, MemoryStream>> ExportToExcelAsync(string entityid, int timeOffset);
    }
}
