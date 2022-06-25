using System;
using System.ComponentModel;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportObservationsMemberAC : BaseModelAC
    {

        /// <summary>
        /// Report title 
        /// </summary>
        [DisplayName("Report Title")]
        public string ReportTitle { get; set; }

        /// <summary>
        /// Foreign key for user table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for ReportObservation
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Title of report observation
        /// </summary>
        [DisplayName("Observation Title")]
        public string ObservationTitle { get; set; }

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
