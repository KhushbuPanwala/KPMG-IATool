using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Utility.FileUtil
{
    public class AzureFileUtility : IFileUtility
    {
        private readonly CloudBlobContainer _container;
        private readonly CloudBlobClient _cloudBlobClient;
        public IConfiguration _configuration { get; }

        public AzureFileUtility(IConfiguration configuration)
        {
            _configuration = configuration;
            CloudStorageAccount cloudStorageAccount;
            var storageContainerName = _configuration["AzureCredentials:StorageContainerName"];
            var storageConnectionString = _configuration["AzureCredentials:StorageConnectionString"];

            if (CloudStorageAccount.TryParse(storageConnectionString.ToString(), out cloudStorageAccount))
            {
                _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                _container = _cloudBlobClient.GetContainerReference(storageContainerName.ToString());
            }
        }


        #region IFileUtility method

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="file">file to be uploaded</param>
        /// <returns>fileUrl</returns>
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileUrl = $"{file.FileName.Split('.').ToList().First()}{ DateTime.Now.ToString(StringConstant.DateFormatForFileName) }.{file.FileName.Split('.').ToList().Last()}";
            var fileReference = _container.GetBlockBlobReference(fileUrl);
            await fileReference.UploadFromStreamAsync(file.OpenReadStream());
            return fileUrl;
        }

        /// <summary>
        /// Upload multiple files
        /// </summary>
        /// <param name="files">files to be uploaded</param>
        /// <returns>List of file names</returns>
        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files)
        {
            List<string> fileNameList = new List<string>();
            Parallel.ForEach(files, async file =>
            {
                var fileName = $"{file.FileName.Split('.').ToList().First()}{ DateTime.Now.ToString(StringConstant.DateFormatForFileName) }.{file.FileName.Split('.').ToList().Last()}";
                fileNameList.Add(fileName);
                var fileReference = _container.GetBlockBlobReference(fileName);
                var fileStream = file.OpenReadStream();
                await fileReference.UploadFromByteArrayAsync(ReadFully(fileStream, fileReference.StreamWriteSizeInBytes),
                            0, (int)fileStream.Length);
            });
            Task.WaitAll();
            return fileNameList;
        }


        /// <summary>
        /// Download file from azure blob storage
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>url</returns>
        public string DownloadFile(string fileName)
        {
            string sasContainerToken = string.Empty;
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(60);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
            sasContainerToken = _container.GetSharedAccessSignature(sasConstraints);

            var containerPath = '/' + Convert.ToString(_configuration["AzureCredentials:StorageContainerName"]) + '/';

            StringBuilder url = new StringBuilder();
            url.Append(Convert.ToString(_configuration["AzureFileUrlPrefix"]));
            url.Append(containerPath);
            url.Append(fileName);
            url.Append(sasContainerToken);
            return url.ToString();
        }

        /// <summary>
        /// Delete file from azure blob storage
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <returns>boolean value to denote if deleted or not</returns>
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blob = _container.GetBlobReference(fileName);
            var isDeleted = await blob.DeleteIfExistsAsync();
            return isDeleted;
        }

        /// <summary>
        /// Upload files in specific folder
        /// </summary>
        /// <returns>azure file url</returns>
        public async Task<string> UploadFilesInDirectoryAsync(IFormFile file)
        {
            CloudBlobDirectory directory = _container.GetDirectoryReference(StringConstant.ReportPPTFolder);
            var fileUrl = $"{file.FileName.Split('.').ToList().First()}{ DateTime.Now.ToString(StringConstant.DateFormatForFileName) }.{file.FileName.Split('.').ToList().Last()}";
            var fileReference = directory.GetBlockBlobReference(fileUrl);
            await fileReference.UploadFromStreamAsync(file.OpenReadStream());
            return fileUrl;
        }
        /// <summary>
        /// Delete files from azure 
        /// </summary>
        /// <returns>Task</returns>
        public async Task DeletFilesAsync()
        {
            foreach (IListBlobItem blob in _container.GetDirectoryReference(StringConstant.ReportPPTFolder).ListBlobs(true))
            {
                if (blob.GetType() == typeof(CloudBlob) || blob.GetType().BaseType == typeof(CloudBlob))
                {
                    ((CloudBlob)blob).DeleteIfExists();
                }
            }
        }
        #endregion

        #region Static Method

        /// <summary>
        /// Method to read stream
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="size">size</param>
        /// <returns>Memory stream array</returns>
        static byte[] ReadFully(Stream input, int size)
        {
            byte[] buffer = new byte[size];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, size)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

    }
}
