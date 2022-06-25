using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ObservationCategoryRepository
{
  public  interface IObservationCategoryRepository
    {
        /// <summary>
        /// Get all the observation category  with searchstring wise , without
        /// serach string and pagination wise
        /// </summary>
        /// <param name="pagination">For getting page variables</param>
        /// <returns>List of observation categories</returns>
        Task<Pagination<ObservationCategoryAC>> GetAllObservationCategoriesPageWiseAndSearchWiseAsync(Pagination<ObservationCategoryAC> pagination);

        /// <summary>
        /// Get data of a particular ObservationCategory 
        /// </summary>
        /// <param name="observationCategoryId">Id of the ObservationCategory</param>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>Particular ObservationCategory data</returns>
        Task<ObservationCategoryAC> GetObservationCategoryDetailsByIdAsync(string observationCategoryId, string selectedEntityId);

        /// <summary>
        /// Add new ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="categoryDetails">Details of ObservationCategory</param>
        /// <returns>Added ObservationCategory details</returns>
        Task<ObservationCategoryAC> AddObservationCategoryAsync(ObservationCategoryAC categoryDetails);

        /// <summary>
        /// Update ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="updatedObservationCategoryDetails">Updated details of ObservationCategory</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task UpdateObservationCategoryAsync(ObservationCategoryAC updatedObservationCategoryDetails);

        /// <summary>
        /// Delete ObservationCategory from an auditable entity
        /// </summary>
        /// <param name="observationCategoryId">Id of the ObservationCategory that needs to be deleted</param>
        /// <param name="selectedEntityId">Selected entity Id</param>
        /// <returns>Throws exception if any otherwise Task</returns>
        Task DeleteObservationCategoryAsync(string observationCategoryId, string selectedEntityId);

        /// <summary>
        /// Get all the ObservationCategory under an auditable entity
        /// </summary>
        /// <param name="selectedEntityId">Id of the selected entity</param>
        /// <returns>list of all ObservationCategory </returns>
        Task<List<ObservationCategoryAC>> GetAllObservationCategoryByEntityIdAsync(Guid selectedEntityId);

        /// <summary>
        /// Method for exporting  auditable observation category
        /// </summary>
        /// <param name="entityId">Id of selected entity</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Excel file</returns>
        Task<Tuple<string, MemoryStream>> ExportObservationCategoriesAsync(string entityId, int timeOffset);
    }
}
