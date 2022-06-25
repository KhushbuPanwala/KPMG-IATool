using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.Common
{
    public interface IGlobalRepository
    {
        /// <summary>
        /// Get count for pagination skip records
        /// </summary>
        /// <param name="pageNo">Current page no</param>
        /// <param name="itemsPerPage">Items per page</param>
        /// <returns>Returns number of records to be skipped</returns>
        int GetSkipRecordsCount(int pageNo, int itemsPerPage);

        /// <summary>
        /// Set Table column name
        /// </summary>
        /// <typeparam name="T">Generic model </typeparam>
        /// <param name="table">Datatable table</param>
        /// <returns>Updated table</returns>
        DataTable SetTableColumnName<T>(DataTable table);

        /// <summary>
        /// Convert datatable to specific model table
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="dt">Datatable table</param>
        /// <returns>List of generic data </returns>
        List<T> ConvertDataTable<T>(DataTable dt);

        /// <summary>
        /// Set Speical charecters
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public string SetSpecialCharecters(string value);

        /// <summary>
        /// check file size is valid or not
        /// </summary>
        /// <param name="documentFiles">list of files</param>
        /// <returns>file size is valid or not</returns>
        public bool CheckFileSize(List<IFormFile> documentFiles);

        /// <summary>
        /// check file is valid or not
        /// </summary>
        /// <param name="documentFiles">list of files</param>
        /// <returns>file is valid or not</returns>
        public bool CheckFileExtention(List<IFormFile> documentFiles);
    }
}
