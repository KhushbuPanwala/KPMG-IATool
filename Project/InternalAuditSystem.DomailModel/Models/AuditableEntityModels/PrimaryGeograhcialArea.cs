using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class PrimaryGeographicalArea : BaseModel
    {
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key for Region
        /// </summary>
        public Guid RegionId { get; set; }

        /// <summary>
        /// Foreign key for Entity Country
        /// </summary>
        public Guid EntityCountryId { get; set; }

        /// <summary>
        /// Foreign key for EntityState
        /// </summary>
        public Guid EntityStateId { get; set; }

        /// <summary>
        /// Foreign key for Location
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityCountryId")]
        public virtual EntityCountry EntityCountry { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityStateId")]
        public virtual EntityState EntityState { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

    }
}
