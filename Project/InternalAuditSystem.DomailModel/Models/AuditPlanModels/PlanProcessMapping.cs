using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomailModel.Models.ObservationManagement;

namespace InternalAuditSystem.DomailModel.Models.AuditPlanModels
{
    public class PlanProcessMapping:BaseModel
    {
        /// <summary>
        /// Defines foreign key of audit plan
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// Defines Fforeign key of process
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Defines start Date of process
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Defines end date of process
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Defines enum type status of process
        /// </summary>
        public PlanProcessStatus Status { get; set; }

        /// <summary>
        /// Defines foreign key of Workprogram
        /// </summary>
        public Guid? WorkProgramId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with workprogram
        /// </summary>
        [ForeignKey("WorkProgramId")]
        public virtual WorkProgram WorkProgram { get; set; }

        /// <summary>
        /// Defines foreign key relationship with audit plan
        /// </summary>
        [ForeignKey("PlanId")]
        public virtual AuditPlan AuditPlan { get; set; }


        /// <summary>
        /// Defines foreign key relationship model of process
        /// </summary>
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }
    }
}
