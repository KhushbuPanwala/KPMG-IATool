using InternalAuditSystem.Repository.ApplicationClasses;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.GeneratePPTRepository
{
    public interface IGeneratePPTRepository
    {
        /// <summary>
        /// Create PPT File   
        /// </summary>
        /// <param name="fileName">Output file name</param>
        /// <param name="templateFileName">template file path</param>
        /// <param name="templateData">template data</param>
        /// <param name="tableData">table data</param>
        /// <returns>Tupple with file data and file name</returns>
        public Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFileName, PowerPointTemplate templateData, DynamicDictionary<int, dynamic> tableData);

        /// <summary>
        /// Create ppt file
        /// </summary>
        /// <param name="fileName">output file name</param>
        /// <param name="templateFileName">template file path</param>
        /// <param name="templateData">template data</param>
        /// <param name="slideNo">slide no.</param>
        /// <param name="jsonTableData">json table data</param>
        /// <returns>Tupple with file data and file name</returns>
        public Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFileName, PowerPointTemplate templateData, int slideNo, JSONRoot jsonTableData);

        /// <summary>
        /// Create ppt file
        /// </summary>
        /// <param name="fileName">Source template file name</param>
        /// <param name="templateFilePath">source template file path</param>
        /// <param name="templateData">source template data</param>
        /// <param name="tableData">table data for template</param>
        /// <param name="repeatedTemplatePath"> Repeated template path</param>
        /// <param name="templatesData">Repeated template data</param>
        /// <param name="lastPPTemplatePath">last template file path </param>
        /// <param name="lastSlidePPTFile">last template file name</param>
        /// <returns>Tupple with file data and file name</returns>
        public Task<Tuple<string, MemoryStream>> CreatePPTFileAsync(string fileName, string templateFilePath, PowerPointTemplate templateData, DynamicDictionary<int, dynamic> tableData, string repeatedTemplatePath, List<PowerPointTemplate> templatesData, string lastPPTemplatePath, string lastSlidePPTFile);
    }
}
