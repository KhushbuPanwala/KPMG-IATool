using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportObservationsDocument : BaseModel
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
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ReportObservationId")]
        public virtual ReportObservation ReportObservation { get; set; }
    }
}
