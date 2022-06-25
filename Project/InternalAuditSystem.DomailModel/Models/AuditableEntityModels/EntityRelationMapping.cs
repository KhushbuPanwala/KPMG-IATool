using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class EntityRelationMapping : BaseModel
    {
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key for RelationshipType
        /// </summary>
        public Guid RelationTypeId { get; set; }
        
        /// <summary>
        /// Foreign key for RelationEntity
        /// </summary>
        public Guid RelatedEntityId { get; set; }
        
        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RelatedEntityId")]
        public virtual AuditableEntity RelatedAuditableEntity { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RelationTypeId")]
        public virtual RelationshipType RelationshipType { get; set; }

    }
}
