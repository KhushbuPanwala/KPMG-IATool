using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.ExportToExcelRepository
{
    public interface IExportToExcelRepository
    {

        /// <summary>
        /// Create Excel File for single table data
        /// </summary>
        /// <typeparam name="T">Any data model</typeparam>
        /// <param name="exportData">Data to be Exported </param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Tupple with file data and file name</returns>
        public Task<Tuple<string, MemoryStream>> CreateExcelFile<T>(List<T> exportData, string outputFileName);


        /// <summary>
        /// Create Excel File for multiple table data
        /// </summary>
        /// <param name="exportData">Data to be Exported </param>
        /// <param name="outputFileName">Output file name</param>
        /// <returns>Tupple with file data and file name</returns>
        public Task<Tuple<string, MemoryStream>> CreateExcelFileWithMultipleTable(DynamicDictionary<string, dynamic> exportData, string outputFileName);

        /// <summary>
        /// Create Excel File with json data 
        /// </summary>
        /// <param name="exportData">Data to be Exported </param>
        /// <param name="jsonData">Json data</param>
        /// <param name="fileName">Output file name</param>
        /// <returns>Generated file Name</returns>
        public Task<Tuple<string, MemoryStream>> CreateExcelFile(DynamicDictionary<string, dynamic> exportData, List<JSONTable> jsonData, string fileName);

    }
}
