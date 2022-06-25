using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditPlanModels
{
   public class AuditCategory:BaseModel
    {
        /// <summary>
        /// Defines name of audit category
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines foreign key of auditableentity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with auditable entity
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
