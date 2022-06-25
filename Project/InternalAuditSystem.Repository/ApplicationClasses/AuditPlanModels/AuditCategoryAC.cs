using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels
{
    public class AuditCategoryAC : BaseModelAC
    {
        /// <summary>
        /// Defines name of audit category
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines foreign key of auditableentity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

    }
}
