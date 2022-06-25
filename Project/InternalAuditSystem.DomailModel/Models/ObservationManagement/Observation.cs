using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using InternalAuditSystem.DomainModel.Models.ObservationManagement;

namespace InternalAuditSystem.DomailModel.Models.ObservationManagement
{
    public class Observation : BaseModel
    {
        /// <summary>
        /// Defines the heading of the observation
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Heading { get; set; }

        /// <summary>
        /// Defines the background 
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table RiskAndControl
        /// </summary>
        public Guid? RiskAndControlId { get; set; }

        /// <summary>
        /// Defines all the observations
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Defines the type of the Observation
        /// </summary>
        public ObservationType ObservationType { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table ObservationCategory
        /// </summary>
        public Guid? ObservationCategoryId { get; set; }

        /// <summary>
        /// Checks if there is any repeat in observation
        /// </summary>
        public bool IsRepeatObservation { get; set; }

        /// <summary>
        /// Defines the root cause of the observation
        /// </summary>

        public string RootCause { get; set; }

        /// <summary>
        /// Defines the implication of the observation
        /// </summary>

        public string Implication { get; set; }

        /// <summary>
        /// Defines the disposition of the observation
        /// </summary>
        public Disposition Disposition { get; set; }

        /// <summary>
        /// Defines the observation status
        /// </summary>
        public ObservationStatus ObservationStatus { get; set; }

        /// <summary>
        /// Defines the observation list status
        /// </summary>
        public ObservationStatus ObservationListStatus { get; set; }

        /// <summary>
        /// Defines the recommendation for the observation
        /// </summary>
        public string Recommendation { get; set; }

        /// <summary>
        /// Defines the Management response for the observation
        /// </summary>
        public string ManagementResponse { get; set; }

        /// <summary>
        /// Defines the conclusion for the observation
        /// </summary>
        public string Conclusion { get; set; }

        /// <summary>
        /// Defines the responsible person : The Client Participants Id
        /// </summary>
        public Guid PersonResponsible { get; set; }

        /// <summary>
        /// Defines the observation related 
        /// </summary>
        public string RelatedObservation { get; set; }

        /// <summary>
        /// Target date of  observation
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Linked observation of  observation
        /// </summary>
        public string LinkedObservation { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table Audit plan
        /// </summary>
        public Guid? AuditPlanId { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table Process
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table auditable entity
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Defines observation is save as draft or not
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Defines the table RiskControlMatrixDetail as type : virtual 
        /// </summary>
        [ForeignKey("RiskAndControlId")]
        public virtual RiskControlMatrix RiskControlMatrix { get; set; }

        /// <summary>
        /// Defines the table ObservationCategory as type : virtual 
        /// </summary>
        [ForeignKey("ObservationCategoryId")]
        public virtual ObservationCategory ObservationCategory { get; set; }

        /// <summary>
        /// Defines the table SubProcess as type : virtual 
        /// </summary>
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }

        /// <summary>
        /// Defines the table AuditPlan as type : virtual 
        /// </summary>
        [ForeignKey("AuditPlanId")]
        public virtual AuditPlan AuditPlan { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity as type : virtual 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntityModels.AuditableEntity AuditableEntity { get; set; }


        /// <summary>
        /// Defines observationTable table list
        /// </summary>
        public virtual ICollection<ObservationTable> ObservationTables { get; set; }


    }
}
