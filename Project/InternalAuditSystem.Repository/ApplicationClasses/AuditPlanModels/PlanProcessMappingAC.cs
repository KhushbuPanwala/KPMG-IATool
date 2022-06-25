using InternalAuditSystem.DomailModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlan
{
    public class PlanProcessMappingAC : BaseModelAC
    {
        /// <summary>
        /// Defines foreign key of audit plan
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid PlanId { get; set; }

        /// <summary>
        /// Defines Foreign key of process/ to bind subprocess id in plan process add/edit
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ProcessId { get; set; }

        /// <summary>
        /// Property to bind only process id in plan process add/edit
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ParentProcessId { get; set; }

        /// <summary>
        /// Defines start Date of process
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Plan name for display
        /// </summary>
        [DisplayName("AuditPlan")]
        public string AuditPlan { get; set; }

        /// <summary>
        /// parent process name for display purpose
        /// </summary>
        [DisplayName("Process")]
        public string ParentProcessName { get; set; }

        /// <summary>
        /// sub process name for display purpose
        /// </summary>
        [DisplayName("SubProcess")]
        public string ProcessName { get; set; }

        /// <summary>
        /// Status string for display
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Start date time for display
        /// </summary>
        [DisplayName("StartDateTime")]
        public string StartDateTimeForDisplay { get; set; }

        /// <summary>
        /// Defines end date of process
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// End date time for display
        /// </summary>
        [DisplayName("EndDateTime")]
        public string EndDateTimeForDisplay { get; set; }

        /// <summary>
        /// Defines enum type status of process
        /// </summary>
        [Export(IsAllowExport = false)]
        public PlanProcessStatus Status { get; set; }

        /// <summary>
        /// Defines foreign key of Workprogram
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? WorkProgramId { get; set; }
        
    }
}
