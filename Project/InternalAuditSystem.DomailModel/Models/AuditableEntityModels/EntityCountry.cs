using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class EntityCountry : BaseModel
    {

        /// <summary>
        /// Foreign key for Region
        /// </summary>
        public Guid RegionId { get; set; }
        
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }
        
        /// <summary>
        /// Foreign key for Country
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

    }
}
