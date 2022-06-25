using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class EntityCategory : BaseModel
    {
        /// <summary>
        /// Name for category for entity
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Foreign key for auditable entity 
        /// </summary>
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
