using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditPlanModels
{
    public class Process : BaseModel
    {
        /// <summary>
        /// Defines name of process
        /// </summary>
        [MaxLength(RegexConstant.ProcessSubProcessMaxLength)]
        public string  Name { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines foreign key of process
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Defines on what basis scope will be selected
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string ScopeBasedOn { get; set; }

        /// <summary>
        /// Defines foreign key relationship with auditable entity
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Defines self foreign key relationship with process
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual Process ParentProcess { get; set; }

    }
}
