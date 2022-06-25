using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
    public class RcmSubProcessAC : BaseModelAC
    {
        /// <summary>
        /// Sub-process for RCM Sub process master
        /// </summary>
        public string SubProcess { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }
    }
}
