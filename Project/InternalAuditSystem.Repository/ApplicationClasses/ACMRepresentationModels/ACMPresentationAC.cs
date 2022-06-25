using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMPresentationAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for Report table
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid? ReportId { get; set; }

        /// <summary>
        /// Heading of ACM Precentation
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Recommendation for ACM presentation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Recommendation { get; set; }

        /// <summary>
        /// Observation for ACM presentation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Observation { get; set; }

        /// <summary>
        /// ManagementResponse for ACM presentation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string ManagementResponse { get; set; }

        /// <summary>
        /// Defines foreign key of Rating Master
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? RatingId { get; set; }


        /// <summary>
        /// List of Ratings for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RatingAC> RatingsList { get; set; }

        /// <summary>
        /// Defines Rating object
        /// </summary>
        [Export(IsAllowExport = false)]
        public RatingAC Rating { get; set; }

        /// <summary>
        ///  Defines foreign key of Rating Master
        /// </summary>
        [DisplayName("Rating Name")]
        public string Ratings { get; set; }

        /// <summary>
        /// Implications for AM Presentation
        /// </summary>
        public string Implication { get; set; }

        /// <summary>
        /// Status for ACM Presentation
        /// </summary>
        [Export(IsAllowExport = false)]
        public ACMStatus Status { get; set; }

        /// <summary>
        /// Defines the Status for ACM Presentation
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Title for ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public string ACMReportTitle { get; set; }

        /// <summary>
        /// Status of ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ACMReportStatus ACMReportStatus { get; set; }
        
        /// <summary>
        /// Status of ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportStage ACMReportStage { get; set; }

        /// <summary>
        /// Defines the Status for ACM Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public string ACMReportStatusString { get; set; }

        /// <summary>
        /// Foreign key of auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }


        /// <summary>
        /// Defines acm is save as draft or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsDraft { get; set; }

        /// <summary>
        /// Defines acm is save as draft or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public string IsDraftToString { get; set; }

        /// <summary>
        /// Defines attachment files
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<IFormFile> ACMFiles { get; set; }

        /// <summary>
        /// List of acm documents
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ACMDocumentAC> ACMDocuments { get; set; }


        /// <summary>
        /// list of acm table
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ACMTableAC> ACMTableList { get; set; }

        /// <summary>
        /// Defines the responsible person : The Client Participants Id
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid PersonResponsible { get; set; }

        /// <summary>
        /// List of person resposible
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> PersonResponsibleList { get; set; }

        /// <summary>
        /// list of Reviewer 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<EntityUserMappingAC> UserList { get; set; }

        /// <summary>
        /// list of Reviewer 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ACMReportMappingAC> ReviewerList { get; set; }

        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<KeyValuePair<int, string>> ReviewerStatus { get; set; }
    }
}