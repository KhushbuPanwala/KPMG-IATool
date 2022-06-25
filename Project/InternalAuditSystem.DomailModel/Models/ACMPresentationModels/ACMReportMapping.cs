using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.ACMPresentationModels
{
    public class ACMReportMapping : BaseModel
    {
        /// <summary>
        /// Foreign key for Report table
        /// </summary>
        public Guid ACMId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ACMId")]
        public virtual ACMPresentation ACMPresentation { get; set; }

        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        public ReportUserType ReportUserType { get; set; }

        /// <summary>
        /// Defines foreign key relation
        /// </summary>
        public Guid? ReportId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with report
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }


        /// <summary>
        /// ACMReportDetail foreign key
        /// </summary>
        public Guid? AcmReportId { get; set; }

        /// <summary>
        /// Foreign key relation with ACMReportDetail
        /// </summary>
        [ForeignKey("AcmReportId")]
        public virtual ACMReportDetail ACMReportDetail { get; set; }

    }
}
