using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.RiskAndControlModels
{
    public class RiskControlMatrixSubProcess : BaseModel
    {
        /// <summary>
        /// Defines process of Rcm
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.SubProcessMaxInputLength)]
        public string SubProcess { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
