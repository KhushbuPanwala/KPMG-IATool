using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMTableAC
    {
        /// <summary>
        /// Observation id of its parent observation
        /// </summary>
        public Guid ACMId { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
