using System;
using System.ComponentModel.DataAnnotations.Schema;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Enums;
using System.ComponentModel.DataAnnotations;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomailModel.Models.UserModels;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportObservation : BaseModel
    {

        /// <summary>
        /// Foreign key for Report table
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Foreign key for Observation table
        /// </summary>
        public Guid ObservationId { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table Audit plan
        /// </summary>
        public Guid? AuditPlanId { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table Process
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Foreign key for ObservationCategory table
        /// </summary>
        public Guid? ObservationCategoryId { get; set; }

        /// <summary>
        /// Defines the heading of the observation
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Heading { get; set; }

        /// <summary>
        /// Background of report observation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Background { get; set; }

        /// <summary>
        /// Observation name
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Observations { get; set; }

        /// <summary>
        /// Rating of report observation
        /// </summary>
        public Guid? RatingId { get; set; }


        /// <summary>
        /// observation auditor
        /// </summary>
        public Guid? AuditorId { get; set; }

        /// <summary>
        /// Type of observation for report
        /// </summary>
        public ReportObservationType ObservationType { get; set; }

        /// <summary>
        /// Is Repeated Observation in report
        /// </summary>
        public bool IsRepeatObservation { get; set; }

        /// <summary>
        /// Root cause for report observation
        /// </summary>
        public string RootCause { get; set; }

        /// <summary>
        /// Implication of report observartion
        /// </summary>
        public string Implication { get; set; }

        /// <summary>
        /// Disposition of report observation
        /// </summary>
        public ReportDisposition Disposition { get; set; }

        /// <summary>
        /// Status of observation
        /// </summary>
        public ReportObservationStatus ObservationStatus { get; set; }

        /// <summary>
        /// Recommendation for report observation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Recommendation { get; set; }

        /// <summary>
        /// Response of management on report observation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string ManagementResponse { get; set; }

        /// <summary>
        /// Target date of report observation
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Linked observation of report observation
        /// </summary>
        public string LinkedObservation { get; set; }

        /// <summary>
        /// Conclusion of report observation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Conclusion { get; set; }

        /// <summary>
        /// Sort order for report observation
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ObservationId")]
        public virtual Observation ObservationDetail { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ObservationCategoryId")]
        public virtual ObservationCategory ObservationCategory { get; set; }


        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RatingId")]
        public virtual Rating Rating { get; set; }

        /// <summary>
        /// Defines the table SubProcess as type : virtual 
        /// </summary>
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }

        /// <summary>
        /// Defines the table AuditPlan as type : virtual 
        /// </summary>
        [ForeignKey("AuditPlanId")]
        public virtual AuditPlan AuditPlan { get; set; }


        /// <summary>
        /// Foreign key relation with User
        /// </summary>
        [ForeignKey("AuditorId")]
        public virtual User User { get; set; }
    }
}
