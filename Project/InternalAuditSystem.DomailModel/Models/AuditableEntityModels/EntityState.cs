using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{ 
    public class EntityState : BaseModel
    {
        /// <summary>
        /// Foreign key for EntityCountry
        /// </summary>
        public Guid EntityCountryId { get; set; }
        
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Foreign key for state
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
        
        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityCountryId")]
        public virtual EntityCountry EntityCountry { get; set; }
        
        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("StateId")]
        public virtual ProvinceState ProvinceState { get; set; }
    }
}
