using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class DocumentUpload
    {
        /// <summary>
        /// Defines seleted report Observation id
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Defines files as user response
        /// </summary>
        public List<IFormFile> Files { get; set; }
    }
}
