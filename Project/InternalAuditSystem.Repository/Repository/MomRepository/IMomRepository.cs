using InternalAuditSystem.DomailModel.Models.MomModels;
using InternalAuditSystem.Repository.ApplicationClasses.MomModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.MomRepository
{
    public interface IMomRepository
    {

        /// <summary>
        /// Get count of Mom
        /// </summary>
        /// <param name="searchString">Search value</param>
        /// <param name="entityId">Selected entityId</param>
        /// <returns>count of Mom</returns>
        Task<int> GetMomCountAsync(string searchString,string entityId);

        /// <summary>
        /// Get mom data
        /// </summary>
        /// <param name="pageIndex">Current Page no.</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchString">Search value</param>
        /// <param name="entityId">Selected entityId</param>
        /// <returns>List of mom</returns>
        Task<List<MomAC>> GetMomAsync(int? pageIndex, int pageSize, string searchString,string entityId);

        /// <summary>
        /// Method for fetching mom detail by Id
        /// </summary>
        /// <param name="Id">Id of mom</param>
        /// <param name="entityId">Selected entityId</param>
        /// <returns>Application class of mom</returns>
        Task<MomAC> GetMomDetailByIdAsync(string Id,string entityId);

        /// <summary>
        /// Method  for adding mom data
        /// </summary>
        /// <param name="MomAC">Application class of mom</param>
        /// <returns>Object of newly added mom</returns>
        Task<MomAC> AddMomAsync(MomAC momAC);

        /// <summary>
        /// Method for update Mom
        /// </summary>
        /// <param name="MomAC">Application class of mom</param>
        /// <returns>Updated MomAC object</returns>
        Task<MomAC> UpdateMomDetailAsync(MomAC momAC);

        /// <summary>
        /// Method for deleting mom
        /// </summary>
        /// <param name="Id">Id of mom</param>
        /// <returns>Task</returns>
        Task DeleteMomAsync(Guid Id);

        /// <summary>
        /// Method for getting predefined data for mom
        /// </summary>
        /// <param name="entityId">Id of auditable entity</param>
        /// <returns>List of users and workprogram</returns>
        Task<MomAC> GetPredefinedDataForMomAsync(Guid entityId);

        /// <summary>
        /// Export Moms to Excel
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        /// <param name="timeOffset">Requsted user system timezone</param>
        /// <returns>Tupple with file data and file name</returns>
        Task<Tuple<string, MemoryStream>> ExportMomsAsync(string entityId,int timeOffset);

        /// <summary>
        /// Method for generating pdf file
        /// </summary>
        /// <param name="momId">Id of mom</param>
        /// <param name="offset">offset of client</param>
        /// <returns>Data of file in memory stream</returns>
        Task<MemoryStream> DownloadMomPDFAsync(string momId,double offset);
    }
}
