using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMReportDetailAC : BaseModelAC
    {
        /// <summary>
        /// define entity id
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// define report id
        /// </summary>
        public string AcmId { get; set; }


        /// <summary>
        /// Defines report title
        /// </summary>
        public string acmReportTitle { get; set; }

        /// <summary>
        /// Defines the ObservationType as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> StageList { get; set; }

        /// <summary>
        /// Defines the ObservationStatus as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> StatusList { get; set; }

        /// <summary>
        /// Count of Report observations
        /// </summary>
        public int TotalACMReport { get; set; }

        /// <summary>
        /// list of auditor 
        /// </summary>
        public List<EntityUserMappingAC> AuditorList { get; set; }

        /// <summary>
        /// list of person responsible shows in management comment tab drop down
        /// </summary>
        public List<EntityUserMappingAC> UserList { get; set; }

        /// <summary>
        /// list of observation reviewer
        /// </summary>
        public List<ACMReviewerAC> ACMReportReviewerList { get; set; }

        /// <summary>
        /// List of linked observation Observations 
        /// </summary>
        public List<ReportAC> LinkedACMReportList { get; set; }

        /// <summary>
        /// List of Observations 
        /// </summary>
        public List<ACMReportMappingAC> ACMReportList { get; set; }
        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReviewStatus ReviewerStatus { get; set; }

        /// <summary>
        /// Status of ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportStatus ACMReportStatus { get; set; }

        /// <summary>
        /// Status of ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportStage ACMReportStage { get; set; }

        /// <summary>
        /// Defines no. of files added
        /// </summary>
        [Export(IsAllowExport = false)]
        public int FileCount { get; set; }

        /// <summary>
        /// Acm report status only for show status
        /// </summary>
        public string ACMReportStatusToString { get; set; }

        /// <summary>
        /// Status of ACM Report only for show stage
        /// </summary>
        
        public string ACMReportStageToString { get; set; }

        /// <summary>
        /// Index of acm report list page
        /// </summary>
        public int pageIndex { get; set; }


    }
}
