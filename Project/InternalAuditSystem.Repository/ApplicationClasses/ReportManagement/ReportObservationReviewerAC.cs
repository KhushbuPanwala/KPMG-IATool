using System;
using System.ComponentModel.DataAnnotations.Schema;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using System.ComponentModel.DataAnnotations;
using InternalAuditSystem.DomainModel.Constants;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportObservationReviewerAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for user table
        /// </summary>
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for ReportObservation
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Comment for report observation members
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// define name of comment created by
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// define name of reviewer
        /// </summary>
        public string ReviewerName { get; set; }


        /// <summary>
        /// define title of report observation
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string ReportObservationTitle { get; set; }
    }
}
