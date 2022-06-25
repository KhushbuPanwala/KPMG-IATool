using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement
{
    public class ObservationTableAC
    {
        /// <summary>
        /// Observation id of its parent observation
        /// </summary>
        public Guid ObservationId { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
