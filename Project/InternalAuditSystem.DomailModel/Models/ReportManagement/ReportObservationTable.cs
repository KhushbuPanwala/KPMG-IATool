using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.DomainModel.Models.ReportManagement
{
    public class ReportObservationTable : BaseModel
    {
        /// <summary>
        /// Report Observation id of its parent report observation
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Defines foreign key relation with ReportObservation 
        /// </summary>
        [ForeignKey("ReportObservationId")]
        public virtual ReportObservation ReportObservation { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
