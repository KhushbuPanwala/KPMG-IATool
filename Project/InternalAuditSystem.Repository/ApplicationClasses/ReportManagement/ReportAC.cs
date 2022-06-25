using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportAC : BaseModelAC
    {
        /// <summary>
        /// Title of report
        /// </summary>
        [DisplayName("Report Title")]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string ReportTitle { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key for Ratings
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RatingId { get; set; }

        /// <summary>
        /// Name of Ratings
        /// </summary>
        [DisplayName("Rating Name")]
        public string Ratings { get; set; }

        /// <summary>
        /// Stage of report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportStage Stage { get; set; }

        /// <summary>
        /// Audit period start date for report
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime AuditPeriodStartDate { get; set; }

        [DisplayName("Audit Start Date")]
        public string AuditStartDate { get; set; }

        /// <summary>
        /// Audit period end date for report
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime AuditPeriodEndDate { get; set; }

        /// <summary>
        /// Audit period end date for report
        /// </summary>
        [DisplayName("Audit End Date")]
        public string AuditEndDate { get; set; }

        /// <summary>
        /// Audit status for report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportStatus AuditStatus { get; set; }

        /// <summary>rt
        /// Comment for report
        /// </summary>
        [Export(IsAllowExport = false)]
        public string Comment { get; set; }

        /// <summary>
        /// no. of observation in report
        /// </summary>
        [Export(IsAllowExport = false)]
        public int noOfObservation { get; set; }

        /// <summary>
        /// name of report stage
        /// </summary>
        [DisplayName("Stage")]
        public string StageName { get; set; }

        /// <summary>
        /// name of audit status 
        /// </summary>
        [DisplayName("Audit Status")]
        public string Status { get; set; }

        /// <summary>
        /// list of ratings 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RatingAC> RatingsList { get; set; }

        /// <summary>
        /// list of status 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<KeyValuePair<int, string>> StatusList { get; set; }

        /// <summary>
        /// list of stage 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<KeyValuePair<int, string>> StageList { get; set; }

        /// <summary>
        /// list of Reviewer 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<EntityUserMappingAC> UserList { get; set; }

        /// <summary>
        /// list of Reviewer 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ReportUserMappingAC> ReviewerList { get; set; }

        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<KeyValuePair<int, string>> ReviewerStatus { get; set; }

        /// <summary>
        /// Defines report is selected or not in acm
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
