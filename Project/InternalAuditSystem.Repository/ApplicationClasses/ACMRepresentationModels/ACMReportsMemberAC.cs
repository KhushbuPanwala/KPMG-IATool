using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMReportsMemberAC : BaseModelAC
    {
        /// <summary>
        /// Report title 
        /// </summary>
        [DisplayName("ACM Title")]
        public string ACMTitle { get; set; }

        /// <summary>
        /// Foreign key for user table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for ReportObservation
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ACMReportId { get; set; }

        /// <summary>
        /// Title of report observation
        /// </summary>
        [DisplayName("Report Title")]
        public string ReportTitle { get; set; }

        /// <summary>
        /// define name of user
        /// </summary>
        [DisplayName("First Person Name")]
        public string Name { get; set; }

        /// <summary>
        /// Define designation of user
        /// </summary>
        [DisplayName("Designation")]
        public string Designation { get; set; }

        /// <summary>
        /// define email id of user
        /// </summary>
        [DisplayName("Email")]
        public string EmailId { get; set; }
    }
}
