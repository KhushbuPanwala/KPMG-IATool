using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportCommentAC : BaseModelAC
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
        /// Comment for report
        /// </summary>
        [DisplayName("Comment")]
        public string Comment { get; set; }

        /// <summary>
        /// name of user who added comments
        /// </summary>
        [DisplayName("Comment By")]
        public string Name { get; set; }

    }
}
