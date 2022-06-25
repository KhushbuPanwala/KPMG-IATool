using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
    public class BulkUploadRiskControlMatrixAC
    {
        /// <summary>
        ///  Defines test steps
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        ///  Defines test steps
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        ///  Defines test steps
        /// </summary>
        public string SubProcess { get; set; }

        /// <summary>
        /// Defines description of risk
        /// </summary>
        public string RiskDescription { get; set; }

        /// <summary>
        /// Defines category of cControl
        /// </summary>
        public string ControlCategory { get; set; }

        /// <summary>
        /// Defines type of control
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        ///  Defines objective of control
        /// </summary>
        public string ControlObjective { get; set; }

        /// <summary>
        ///  Defines description of control
        /// </summary>
        public string ControlDescription { get; set; }

        /// <summary>
        /// Defines nature of control
        /// </summary>
        public string NatureOfControl { get; set; }

        /// <summary>
        /// Defines whether control is antiFraud or not
        /// </summary>
        public string AntiFraudControl { get; set; }

        /// <summary>
        /// Defines risk of cControl
        /// </summary>
        public string RiskCategory { get; set; }
    }
}