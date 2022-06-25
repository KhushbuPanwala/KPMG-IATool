using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.Utility.FileUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.AzureBlobStorage
{
    public class AzureRepository : IAzureRepository
    {
        #region Private variables
        private readonly IFileUtility _fileUtility;
        private readonly IDataRepository _dataRepository;
        #endregion

        #region Constructor
        public AzureRepository(IFileUtility fileUtility, IDataRepository dataRepository)
        {
            _fileUtility = fileUtility;
            _dataRepository = dataRepository;
        }
        #endregion

        /// <summary>
        /// Upload file
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="documentStorage">documentStorage</param>
        /// <returns></returns>
        public async Task UploadFileAsync<T>(T documentStorage) where T : class
        {
            await _dataRepository.AddAsync<T>(documentStorage);
            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Add data to respective storage table in database
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="docStorages">Document Storage</param>
        /// <returns></returns>
        public async Task AddUrlsInDocumentStorageAsync<T>(List<T> docStorages) where T : class
        {
            await _dataRepository.AddRangeAsync<T>(docStorages);
            await _dataRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Get uploaded file name
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <returns>List of files</returns>
        public IQueryable<T> GetUploadedFileNames<T>() where T : class
        {
            var listOfFiles = _dataRepository.GetAll<T>();
            return listOfFiles;
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>url</returns>
        public string DownloadFile(string fileName)
        {
            string url = _fileUtility.DownloadFile(fileName);
            return url;
        }

        /// <summary>
        /// Delete file 
        /// </summary>
        /// <typeparam name="T">Generic class</typeparam>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public async Task DeleteFileAsync<T>(string fileName)
        {
            //_dataRepository.Update<T>()
        }

    }


}
