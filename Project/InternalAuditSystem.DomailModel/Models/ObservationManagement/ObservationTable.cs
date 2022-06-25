using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.DomainModel.Models.ObservationManagement
{
    public class ObservationTable : BaseModel
    {
        /// <summary>
        /// Observation id of its parent observation
        /// </summary>
        public Guid ObservationId { get; set; }

        /// <summary>
        /// Defines foreign key relation with observation
        /// </summary>
        [ForeignKey("ObservationId")]
        public virtual Observation Observation { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }


    }
}
