using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models
{
    public class EntityUserMapping : BaseModel
    {
        /// <summary>
        ///  Foreign key for user
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key relation with User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
