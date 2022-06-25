using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Utility.FileUtil
{
    public interface IFileUtility
    {
        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="file">file to be uploaded</param>
        /// <returns>fileUrl</returns>
        Task<string> UploadFileAsync(IFormFile file);

        /// <summary>
        /// Upload multiple files
        /// </summary>
        /// <param name="files">files to be uploaded</param>
        /// <returns>List of file names</returns>
        Task<List<string>> UploadFilesAsync(List<IFormFile> files);


        /// <summary>
        /// Download file from azure blob storage
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>url</returns>
        string DownloadFile(string fileName);

        /// <summary>
        /// Delete file from azure blob storage
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>boolean value to denote if deleted or not</returns>
        Task<bool> DeleteFileAsync(string fileName);

        /// <summary>
        /// Upload files in specific folder
        /// </summary>
        /// <returns>azure file url</returns>
        Task<string> UploadFilesInDirectoryAsync(IFormFile file);

        /// <summary>
        /// Delete files from azure 
        /// </summary>
        /// <returns>Task</returns>
        Task DeletFilesAsync();

    }
}
