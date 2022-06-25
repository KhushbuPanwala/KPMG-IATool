using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.ACMPresentationModels
{
    public class ACMDocument : BaseModel
    {
        /// <summary>
        /// Defines the Name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Document path for document
        /// </summary>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Foreign key for ACMPresentation 
        /// </summary>
        public Guid? ACMReportMappingId { get; set; }

        /// <summary>
        /// Foreign key relation with ACMReportMapping table
        /// </summary>
        [ForeignKey("ACMReportMappingId")]
        public virtual ACMReportMapping ACMReportMapping { get; set; }

        /// <summary>
        /// Foreign key for ACMPresentation 
        /// </summary>
        public Guid? ACMPresentationId { get; set; }

        /// <summary>
        /// Foreign key relation with ACMPresentation
        /// </summary>
        [ForeignKey("ACMPresentationId")]
        public virtual ACMPresentation ACMPresentation { get; set; }
    }
}
