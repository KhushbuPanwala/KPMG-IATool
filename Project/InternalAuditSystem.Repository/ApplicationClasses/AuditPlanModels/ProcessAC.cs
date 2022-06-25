using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlan
{
    public class ProcessAC : BaseModelAC
    {

        /// <summary>
        /// Name of parent process only for display
        /// </summary>
        [DisplayName("Process")]
        public string ParentProcessName { get; set; }

        /// <summary>
        /// Defines name of process
        /// </summary>
        [MaxLength(RegexConstant.ProcessSubProcessMaxLength)]
        [DisplayName("SubProcess")]
        public string Name { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines foreign key of process
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Defines on what basis scope will be selected
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [Export(IsAllowExport = false)]
        public string ScopeBasedOn { get; set; }

        /// <summary>
        /// Defines self foreign key relationship with process
        /// </summary>
        [Export(IsAllowExport =false)]
        public ProcessAC ParentProcess { get; set; }
        /// <summary>
        /// List of subprocess
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ProcessAC> SubProcess { get; set; }

        /// <summary>
        ///  Audit Plan Id
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? AuditPlanId {get;set;}

        /// <summary>
        /// Display scope
        /// </summary>
        public string Scope { get; set; }

    }
}
