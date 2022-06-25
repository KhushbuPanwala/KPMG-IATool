using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class Report : BaseModel
    {
        /// <summary>
        /// Title of report
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string ReportTitle { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key for Ratings
        /// </summary>
        public Guid RatingId { get; set; }

        /// <summary>
        /// Stage of report
        /// </summary>
        public ReportStage Stage { get; set; }

        /// <summary>
        /// Audit period start date for report
        /// </summary>
        public DateTime AuditPeriodStartDate { get; set; }

        /// <summary>
        /// Audit period end date for report
        /// </summary>
        public DateTime AuditPeriodEndDate { get; set; }

        /// <summary>
        /// Audit status for report
        /// </summary>
        public ReportStatus AuditStatus { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RatingId")]
        public virtual Rating Rating { get; set; }
    }
}
