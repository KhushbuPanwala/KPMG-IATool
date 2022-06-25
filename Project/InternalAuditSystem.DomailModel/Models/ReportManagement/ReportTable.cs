using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.DomainModel.Models.ReportManagement
{
    public class ReportTable : BaseModel
    {
        /// <summary>
        /// Observation id of its parent observation
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Defines foreign key relation with observation
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
