using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.ACMPresentationModels
{
   public class ACMReportDetail: BaseModel
    {
        /// <summary>
        /// Title of acm report
        /// </summary>
        public string ReportTitle { get; set; }

        /// <summary>
        /// define report id
        /// </summary>
        public Guid? AcmId { get; set; }

        /// <summary>
        /// Defines foreign key 
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with AuditableEntity
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Defines foreign key relationship with AuditableEntity
        /// </summary>
        [ForeignKey("AcmId")]
        public virtual ACMPresentation ACMPresentation { get; set; }
    }
}
