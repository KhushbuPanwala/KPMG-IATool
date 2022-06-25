using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditPlanModels
{
   public class AuditPlanDocument:BaseModel
    {
        /// <summary>
        /// Defines foreign key of audit plan
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// Defines purpose of upload documents
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Purpose { get; set; }

        /// <summary>
        /// Defines path of documents to be saved
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Defines foreign key relationship with audit plan
        /// </summary>
        [ForeignKey("PlanId")]
        public virtual AuditPlan AuditPlan { get; set; }

    }
}
