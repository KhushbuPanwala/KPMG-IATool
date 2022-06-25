using InternalAuditSystem.DomailModel.Models.ReportManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReviewerDocument : BaseModel
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
        public Guid ReportUserMappingId { get; set; }

        /// <summary>
        /// Defines the table ReportUserMapping as type : virtual
        /// </summary>
        [ForeignKey("ReportUserMappingId")]
        public virtual ReportUserMapping ReportUserMapping { get; set; }
    }
}
