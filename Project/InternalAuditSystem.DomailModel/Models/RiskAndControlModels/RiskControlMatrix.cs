using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;

namespace InternalAuditSystem.DomailModel.Models.RiskAndControlModels
{
    public class RiskControlMatrix : BaseModel
    {
        /// <summary>
        /// Defines foreign key of work program
        /// </summary>
        public Guid? WorkProgramId { get; set; }

        /// <summary>
        /// Defines foreign key relationship  with work program
        /// </summary>
        [ForeignKey("WorkProgramId")]
        public virtual WorkProgram WorkProgram { get; set; }


        /// <summary>
        /// Defines description of risk
        /// </summary>
        public string RiskDescription { get; set; }

        /// <summary>
        /// Defines risk of cControl
        /// </summary>
        public string RiskCategory { get; set; }

        /// <summary>
        /// Defines category of cControl
        /// </summary>
        public ControlCategory  ControlCategory { get; set; }

        /// <summary>
        /// Defines type of control
        /// </summary>
        public ControlType ControlType { get; set; }

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
        public NatureOfControl NatureOfControl { get; set; }

        /// <summary>
        /// Defines whether control is antiFraud or not
        /// </summary>
        public bool AntiFraudControl { get; set; }

        /// <summary>
        ///  Defines test steps
        /// </summary>
        public string TestSteps { get; set; }

        /// <summary>
        /// Defines test results
        /// </summary>
        public bool? TestResults { get; set; }

        /// <summary>
        /// Defines foreign key of RcmMaster for sector
        /// </summary>
        public Guid SectorId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with  sector
        /// </summary>
        [ForeignKey("SectorId")]
        public virtual RiskControlMatrixSector RcmSector { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm process
        /// </summary>
        public Guid RcmProcessId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with  process
        /// </summary>
        [ForeignKey("RcmProcessId")]
        public virtual RiskControlMatrixProcess RcmProcess { get; set; }

        /// <summary>
        ///  Defines foreign key of  Rcm subprocess
        /// </summary>
        public Guid RcmSubProcessId { get; set; }

        /// <summary>
        /// efines foreign key relationship with  subprocess
        /// </summary>
        [ForeignKey("RcmSubProcessId")]
        public virtual RiskControlMatrixSubProcess RcmSubProcess{ get; set; }

        /// <summary>
        /// Defines foreign key relationship of strategic analysis
        /// </summary>
        public Guid? StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with strategic analysis
        /// </summary>
        [ForeignKey("StrategicAnalysisId")]
        public virtual StrategicAnalysis StrategicAnalysis { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

    }
}
