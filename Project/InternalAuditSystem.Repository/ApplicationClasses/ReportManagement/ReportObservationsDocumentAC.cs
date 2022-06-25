using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportObservationsDocumentAC : BaseModelAC

    {
        /// <summary>
        /// Foreign key for ReportObservation table
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Defines the Name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Document path for document
        /// </summary>
        public string DocumentPath { get; set; }


        /// <summary>
        /// Property to get file object from client side
        /// </summary>
        public List<IFormFile> DocumentFile { get; set; }
    }
}
