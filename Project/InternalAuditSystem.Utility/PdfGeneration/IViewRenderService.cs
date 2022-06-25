using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Utility.PdfGeneration
{
    public interface IViewRenderService
    {
        /// <summary>
        /// Method for converting file content to string 
        /// </summary>
        /// <param name="viewName">File name to be converted</param>
        /// <param name="model">Model to be passed in cshtml</param>
        /// <param name="fileName">File name of html</param>
        /// <returns>Converts file content to string and create html of cshtml</returns>
        Task<string> RenderToStringAsync(string viewName, object model,string fileName);
    }
}
