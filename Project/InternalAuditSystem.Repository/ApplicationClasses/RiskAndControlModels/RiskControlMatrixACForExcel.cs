using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.RiskAndControlModels
{
  public  class RiskControlMatrixACForExcel:BaseModelAC
    {
       
        /// <summary>
        /// Defines name of risk
        /// </summary>
        [Export(IsAllowExport = false)]
        public string RiskName { get; set; }

        /// <summary>
        /// Defines description of risk
        /// </summary>
        [DisplayName("Risk Description")]
        public string RiskDescription { get; set; }

        /// <summary>
        /// Defines category of Control Enum
        /// </summary>
        [Export(IsAllowExport = false)]
        public ControlCategory ControlCategory { get; set; }

        /// <summary>
        /// Defines category of Control
        /// </summary>
        [DisplayName("Control category")]
        public string ControlCategoryString { get; set; }

        /// <summary>
        /// Defines type of control enum
        /// </summary>
        [Export(IsAllowExport = false)]
        public ControlType ControlType { get; set; }

        /// <summary>
        /// Defines type of control
        /// </summary>
        [DisplayName("Control type")]
        public string ControlTypeString { get; set; }

        /// <summary>
        ///  Defines objective of control
        /// </summary>
        [DisplayName("Control objective")]
        public string ControlObjective { get; set; }

        /// <summary>
        ///  Defines description of control
        /// </summary>
        [DisplayName("Control description")]
        public string ControlDescription { get; set; }

        /// <summary>
        /// Defines nature of control enum
        /// </summary>
        [Export(IsAllowExport = false)]
        public NatureOfControl NatureOfControl { get; set; }

        /// <summary>
        /// Defines nature of control
        /// </summary>
        [DisplayName("Nature of control")]
        public string NatureOfControlString { get; set; }

        /// <summary>
        /// Defines whether control is antiFraud or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool AntiFraudControl { get; set; }

        /// <summary>
        /// Defines whether control is antiFraud or not
        /// </summary>
        [DisplayName("Anti-Fraud control")]
        public string AntiFraudControlString { get; set; }

        /// <summary>
        /// Defines foreign key of RcmMaster for sector
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid SectorId { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm process string
        /// </summary>
        public string RcmSectorName { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm process
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RcmProcessId { get; set; }

        /// <summary>
        /// Defines risk of cControl
        /// </summary>
        [DisplayName("Risk category")]
        public string RiskCategory { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm process string
        /// </summary>
        [DisplayName("Rcm process")]
        public string RcmProcessName { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm subprocess
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RcmSubProcessId { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm subprocess string
        /// </summary>
        [DisplayName("RCM sub-process")]
        public string RcmSubProcessName { get; set; }

        /// <summary>
        /// List of RCM process for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RcmProcessAC> RiskControlMatrixProcessACList { get; set; }

        /// <summary>
        /// List of RCM sub process for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RcmSubProcessAC> RiskControlMatrixSubProcessACList { get; set; }

        /// <summary>
        /// List of RCM process for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RcmSectorAC> RiskControlMatrixSectorACList { get; set; }

        /// <summary>
        /// Defines foreign key relationship of strategic analysis
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? StrategicAnalysisId { get; set; }


        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }
    }
}
