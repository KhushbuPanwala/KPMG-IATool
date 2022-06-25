using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ReportManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMReportMappingAC : BaseModelAC
    {

        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportUserType ReportUserType { get; set; }


        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        [DisplayName("User Type")]
        public string UserType { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the Table acm presentation
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ACMId { get; set; }

        /// <summary>
        /// Defines Foriegn Key: Id from the table Users
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines name of user
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines enumtype designation of user
        /// </summary>
        [DisplayName("Designation")]
        public string Designation { get; set; }
        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// Defines the status as (string)Enumerable
        /// </summary>
        [DisplayName("Status")]
        public string StatusName { get; set; }
        /// <summary>
        /// Reviwer documents list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ACMReviewerDocumentAC> ACMReviewerDocumentList { get; set; }


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
        /// Defines Foreign Key : Id from the Table Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ReportId { get; set; }


        /// <summary>
        /// Defines Foreign Key : Id from the Table acm report
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ACMReportId { get; set; }

    }
}

