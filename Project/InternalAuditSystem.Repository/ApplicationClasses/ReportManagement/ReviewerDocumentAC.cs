using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReviewerDocumentAC : BaseModelAC
    {
        /// <summary>
        /// Defines the Name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Defines the Path of the document
        /// </summary>
        public string DocumentPath { get; set; }


        /// <summary>
        /// Defines Foreign Key : Id from the mapping table ReportUserMapping
        /// </summary>        
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the mapping table ReportUserMapping
        /// </summary>        
        public Guid ReportUserMappingId { get; set; }


        /// <summary>
        /// File name to display
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Property to get file object from client side
        /// </summary>
        public List<IFormFile> DocumentFile { get; set; }

        /// <summary>
        /// Upload file 
        /// </summary>
        public IFormFile UploadedFile { get; set; }

    }
}
