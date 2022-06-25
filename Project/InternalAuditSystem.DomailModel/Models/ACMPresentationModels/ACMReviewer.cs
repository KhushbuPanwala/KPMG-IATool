using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Models.ACMPresentationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.ACMPresentationModels
{
    public class ACMReviewer : BaseModel
    {

        /// <summary>
        /// Foreign key for ACMPresentation 
        /// </summary>
        public Guid ACMPresentationId { get; set; }

        /// <summary>
        /// User foreign key
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key relation with ACMPresentation
        /// </summary>
        [ForeignKey("ACMPresentationId")]
        public virtual ACMPresentation ACMPresentation { get; set; }

        /// <summary>
        /// Foreign key relation with User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// ACMReportDetail foreign key
        /// </summary>
        public Guid? AcmReportId { get; set; }

        /// <summary>
        /// Foreign key relation with ACMReportDetail
        /// </summary>
        [ForeignKey("AcmReportId")]
        public virtual ACMReportDetail ACMReportDetail {get;set;}

        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        public ReviewStatus Status { get; set; }

    }
}
