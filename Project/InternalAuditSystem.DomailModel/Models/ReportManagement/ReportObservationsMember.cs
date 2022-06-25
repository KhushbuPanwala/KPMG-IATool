using System;
using System.ComponentModel.DataAnnotations.Schema;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Enums;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportObservationsMember : BaseModel
    {
        /// <summary>
        /// Foreign key for user table
        /// </summary>
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for ReportObservation
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ReportObservationId")]
        public virtual ReportObservation ReportObservation { get; set; }

    }
}
