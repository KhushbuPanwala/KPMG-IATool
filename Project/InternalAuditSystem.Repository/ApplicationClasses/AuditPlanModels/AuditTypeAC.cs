using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels
{
    public class AuditTypeAC : BaseModelAC
    {

        /// <summary>
        /// Defines name of audit type
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

    }
}
