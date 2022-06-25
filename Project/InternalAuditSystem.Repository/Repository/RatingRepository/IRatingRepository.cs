using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;

namespace InternalAuditSystem.Repository.Repository.RatingRepository
{
    public interface IRatingRepository
    {
        /// <summary>
        /// Get Ratings data
        /// </summary>
        /// <param name="page">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>List of ratings</returns>
        Task<List<RatingAC>> GetRatingsAsync(int? page, int pageSize, string searchString, string selectedEntityId);

        /// <summary>
        /// Get Ratings data
        /// </summary>
        /// <param name="ratingId">Rating Id </param>
        /// <returns>Detail of rating</returns>
        Task<RatingAC> GetRatingsByIdAsync(string ratingId);
        /// <summary>
        /// Get count of ratings
        /// </summary>
        /// <param name="searchValue">Search value</param>
        /// <param name="selectedEntityId">Selected entityId</param>
        /// <returns>count of ratings</returns>
        Task<int> GetRatingCountAsync(string searchValue, string selectedEntityId);

        /// <summary>
        /// Delete rating detail
        /// <param name="id">id of delete rating</param>
        /// </summary>
        /// <returns>Task</returns>
        Task DeleteRatingAsync(string id);

        /// <summary>
        /// Add rating detail
        /// <param name="rating">rating detail to add</param>
        /// </summary>
        /// <returns>Return Rating AC detail</returns>
        Task<RatingAC> AddRatingAsync(RatingAC rating);

        /// <summary>
        /// Update rating detail
        /// <param name="rating">rating detail to add</param>
        /// </summary>
        /// <returns>Return Rating AC detail</returns>
        Task<RatingAC> UpdateRatingAsync(RatingAC rating);

        /// <summary>
        /// Get Ratings data
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <returns>List of ratings</returns>
        Task<List<RatingAC>> GetRatingsByEntityIdAsync(string entityId);

        /// <summary>
        /// Export Ratings to excel file
        /// </summary>
        /// <param name="selectedEntityId">Selected Entity Id</param>
        /// <param name="timeOffset">Requested user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportRatingsAsync(string selectedEntityId,int timeOffset);
    }
}
