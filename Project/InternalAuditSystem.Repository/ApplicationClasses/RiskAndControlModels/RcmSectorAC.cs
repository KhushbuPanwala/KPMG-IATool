using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
    public class RcmSectorAC : BaseModelAC
    {
        /// <summary>
        /// Sector for RCM Sector list
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }

    }
}
