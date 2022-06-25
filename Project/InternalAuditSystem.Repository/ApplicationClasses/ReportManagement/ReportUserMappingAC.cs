using InternalAuditSystem.DomailModel.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportUserMappingAC : BaseModelAC
    {

        /// <summary>
        /// Report title 
        /// </summary>
        [DisplayName("Report Title")]
        public string ReportTitle { get; set; }

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
        /// Defines Foreign Key : Id from the Table Report
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ReportId { get; set; }

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
        /// Defines email of user
        /// </summary>
        [Export(IsAllowExport = false)]
        public string Email { get; set; }

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
        public List<ReviewerDocumentAC> ReviewerDocumentList { get; set; }
    }
}
