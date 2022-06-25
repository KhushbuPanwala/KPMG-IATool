using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class RelationshipTypeAC : BaseModelAC
    {
        /// <summary>
        /// Name of selected entity 
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Name of relationship type
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }
    }
}
