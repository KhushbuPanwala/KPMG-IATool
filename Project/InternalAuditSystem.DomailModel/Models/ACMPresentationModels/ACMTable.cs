using InternalAuditSystem.DomailModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.DomainModel.Models.ACMPresentationModels
{
    public class ACMTable : BaseModel
    {
        /// <summary>
        /// Observation id of its parent ACM
        /// </summary>
        public Guid ACMId { get; set; }

        /// <summary>
        /// Defines foreign key relation with ACM
        /// </summary>
        [ForeignKey("ACMId")]
        public virtual ACMPresentation ACMPresentation { get; set; }

        /// <summary>
        /// Json document for storing dynamic table
        /// </summary>
        public JsonDocument Table { get; set; }
    }
}
