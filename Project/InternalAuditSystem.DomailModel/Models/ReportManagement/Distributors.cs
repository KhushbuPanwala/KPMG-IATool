using System;
using System.ComponentModel.DataAnnotations.Schema;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class Distributors : BaseModel
    {
        /// <summary>
        /// Foreign key for user table
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Foreign key relation 
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
