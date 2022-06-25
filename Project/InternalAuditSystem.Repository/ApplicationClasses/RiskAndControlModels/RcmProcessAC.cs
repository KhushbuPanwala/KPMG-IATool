using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
    public class RcmProcessAC : BaseModelAC
    {
        /// <summary>
        /// Sector for RCM Process 
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }
    }
}
