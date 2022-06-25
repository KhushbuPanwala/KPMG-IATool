using System;
using InternalAuditSystem.DomailModel.Enums;
using System.ComponentModel.DataAnnotations;
using InternalAuditSystem.DomainModel.Constants;
using System.Collections.Generic;
using System.ComponentModel;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportObservationAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for Report table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ReportId { get; set; }

        /// <summary>
        /// Report title 
        /// </summary>
        [DisplayName("Report Title")]
        public string ReportTitle { get; set; }

        /// <summary>
        /// Foreign key for Observation table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ObservationId { get; set; }


        
        /// <summary>
        /// Foreign key of audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid AuditPlanId { get; set; }

        /// <summary>
        /// Audit plan name
        /// </summary>
        [DisplayName("Audit Plan")]
        public string AuditPlanName { get; set; }

        /// <summary>
        /// Foreign key for Process table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Foreign key for Process table
        /// </summary>
        [DisplayName("Process")]
        public string ProcessName { get; set; }


        /// <summary>
        /// Foreign key for Process table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? SubProcessId { get; set; }


        /// <summary>
        /// Foreign key for Process table
        /// </summary>
        [DisplayName("SubProcess")]
        public string SubProcessName { get; set; }

        /// <summary>
        /// Foreign key for ObservationCategory table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ObservationCategoryId { get; set; }


        /// <summary>
        /// Foreign key for ObservationCategory table
        /// </summary>
        [DisplayName("Category")]
        public string ObservationCategory { get; set; }

        /// <summary>
        /// Defines the heading of the observation
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Heading { get; set; }

        /// <summary>
        /// Background of report observation
        /// </summary>
        [DisplayName("Background")]
        public string Background { get; set; }

        /// <summary>
        /// Observation name
        /// </summary>
        [DisplayName("Observations")]
        public string Observations { get; set; }

        /// <summary>
        /// Rating of report observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? RatingId { get; set; }

        /// <summary>
        /// Rating of report observation
        /// </summary>
        [DisplayName("Rating")]
        public string Rating { get; set; }

        /// <summary>
        /// Type of observation for report
        /// </summary>
        [Export(IsAllowExport = false)]

        public ReportObservationType ObservationType { get; set; }
        /// <summary>
        /// Type of observation for report
        /// </summary>
        [DisplayName("Observation Type")]
        public string ObservationTypeName { get; set; }

        /// <summary>
        /// Is Repeated Observation in report
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsRepeatObservation { get; set; }

        /// <summary>
        /// Is Repeated Observation in report
        /// </summary>
        [DisplayName("Is Repeated")]
        public string IsRepeated { get; set; }

        /// <summary>
        /// Root cause for report observation
        /// </summary>
        [DisplayName("RootCause")]
        public string RootCause { get; set; }

        /// <summary>
        /// Implication of report observartion
        /// </summary>
        [DisplayName("Implication")]
        public string Implication { get; set; }

        /// <summary>
        /// Disposition of report observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportDisposition Disposition { get; set; }

        /// <summary>
        /// Disposition of report observation
        /// </summary>
        [DisplayName("Disposition")]
        public string DispositionName { get; set; }


        /// <summary>
        /// Status of observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportObservationStatus ObservationStatus { get; set; }

        /// <summary>
        /// Status of observation
        /// </summary>
        [DisplayName("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Recommendation for report observation
        /// </summary>
        [DisplayName("Recommendation")]
        public string Recommendation { get; set; }

        /// <summary>
        /// Response of management on report observation
        /// </summary>
        [DisplayName("ManagementResponse")]
        public string ManagementResponse { get; set; }

        /// <summary>
        /// Target date of report observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Target date of report observation
        /// </summary>
        [DisplayName("Target Date")]
        public string ObservationTargetDate { get; set; }


        /// <summary>
        /// Linked observation of report observation
        /// </summary>
        public string LinkedObservation { get; set; }

        /// <summary>
        /// Conclusion of report observation
        /// </summary>
        [DisplayName("Conclusion")]
        public string Conclusion { get; set; }

        /// <summary>
        /// observation auditor
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? AuditorId { get; set; }

        /// <summary>
        /// observation auditor
        /// </summary>
        [DisplayName("Auditor")]
        public string Auditor { get; set; }

        /// <summary>
        /// Sort order for report observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public int SortOrder { get; set; }


        /*Report Observation comment table */
        /// <summary>
        /// define name of user
        /// </summary>
        [DisplayName("Reviewer Name")]
        public string ReviewerName { get; set; }

        /// <summary>
        /// Comment for report observation members
        /// </summary>
        [DisplayName("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Date of creation for all models
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime? CommentCreatedDateTime { get; set; }

        /// <summary>
        /// Date of creation for all models
        /// </summary>
        [DisplayName("Comment Created Date Time")]
        public string CommentCreatedDate { get; set; }

        /// <summary>
        /// list of person responsible 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ReportObservationsMemberAC> PersonResponsibleList { get; set; }


        /// <summary>
        /// list of Reviewer 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ReportObservationReviewerAC> ObservationReviewerList { get; set; }

        /// <summary>
        /// Defines observation is selected or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Defines observation is allow to edit
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsAllowEdit { get; set; }

        /// <summary>
        /// Defines observation is allow to delete
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsAllowDelete { get; set; }

        /// <summary>
        /// Defines observation is allow to View
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsAllowView { get; set; }
        
        /// <summary>
        /// list of report observation table
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ReportObservationTableAC> ReportObservationTableList{ get; set; }

        /// <summary>
        /// list of report observation document
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ReportObservationsDocumentAC> ReportObservationDocumentList{ get; set; }

        /// <summary>
        /// Defines no. of tables added
        /// </summary>
        [Export(IsAllowExport = false)] 
        public int TableCount { get; set; }

        /// <summary>
        /// Defines no. of files added
        /// </summary>
        [Export(IsAllowExport = false)]
        public int FileCount { get; set; }
    }
}
