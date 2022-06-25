using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;


namespace InternalAuditSystem.Repository.AzureBlobStorage
{
    public interface IAzureRepository
    {

        /// <summary>
        /// Upload file
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="documentStorage">documentStorage</param>
        /// <returns></returns>
        Task UploadFileAsync<T>(T documentStorage) where T : class;

        /// <summary>
        /// Add data to respective storage table in database
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="docStorages">Document Storage</param>
        /// <returns></returns>
        Task AddUrlsInDocumentStorageAsync<T>(List<T> docStorages) where T : class;


        /// <summary>
        /// Get uploaded file name
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <returns>List of files</returns>
        IQueryable<T> GetUploadedFileNames<T>() where T : class;

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>url</returns>
        string DownloadFile(string fileName);

        /// <summary>
        /// Delete file 
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        Task DeleteFileAsync<T>(string fileName);





    }
}
