using System;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportObservationTableAC : BaseModelAC
    {
        /// <summary>
        /// Report Observation id of its parent report observation
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
