using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportCommentHistoryAC : BaseModelAC
    {
        /// <summary>
        /// Title of report
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)] 
        public string ReportTitle { get; set; }

        /// <summary>
        /// List of Comments for report
        /// </summary>
        public List<ReportCommentAC> CommentList { get; set; }

        /// <summary>
        /// List Report observatation comments
        /// </summary>
        public List<ReportObservationAC> ReportObservationComment { get; set; }

    }
}
