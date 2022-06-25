using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
    public class RCMUploadAC 
    {
        /// <summary>
        /// List of sector
        /// </summary>
        public List<RcmSectorAC> SectorList { get; set; }
        
        /// <summary>
        /// List of process
        /// </summary>
        public List<RcmProcessAC> ProcessList { get; set; }
        
        /// <summary>
        /// List of sub process
        /// </summary>
        public List<RcmSubProcessAC> SubProcessList { get; set; }

        /// <summary>
        /// Defines the nature of control as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> NatureOfControl { get; set; }
        
        /// <summary>
        /// Defines the control category as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ControlCategory { get; set; }

        /// <summary>
        /// Defines the control type as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ControlType { get; set; }
    }
}
