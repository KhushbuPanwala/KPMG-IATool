using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement
{
    public class ObservationAC : BaseModelAC
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
        [Export(IsAllowExport = false)]
        public Guid? RiskAndControlId { get; set; }

        /// <summary>
        /// Defines all the observations
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table ObservationCategory
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ObservationCategoryId { get; set; }

        /// <summary>
        /// Checks if there is any repeat in observation
        /// </summary>
        [Export(IsAllowExport = false)]
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
        [Export(IsAllowExport = false)]
        public Guid PersonResponsible { get; set; }

        /// <summary>
        /// Defines the observation related 
        /// </summary>
        [Export(IsAllowExport = false)]
        public string RelatedObservation { get; set; }

        /// <summary>
        /// Target date of  observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Linked observation of  observation
        /// </summary>

        public string LinkedObservation { get; set; }

        /// <summary>
        /// Defines foreign Key : Id from table Process
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Defines foreign Key : Parent process Id from table Process
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ParentProcessId { get; set; }
        /// <summary>
        /// Defines the Process name 
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Defines the Sub Process name 
        /// </summary>
        public string SubProcessName { get; set; }

        /// <summary>
        /// Defines observation is order
        /// </summary>
        [Export(IsAllowExport = false)]
        public int SortOrder { get; set; }


        /// <summary>
        /// Status string for observation
        /// </summary>
        [DisplayName("Observation List Status")]
        public string StatusString { get; set; }


        /// <summary>
        /// List of observation category
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ObservationCategoryAC> ObservationCategoryList { get; set; }

        /// <summary>
        /// List of audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditPlanAC> AuditPlanList { get; set; }

        /// <summary>
        /// List of subprocess
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<PlanProcessMappingAC> ProcessList { get; set; }

        /// <summary>
        /// List of parentprocess
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ProcessAC> ParentProcessList { get; set; }
        /// <summary>
        /// Foreign key of audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid AuditPlanId { get; set; }

        /// <summary>
        /// List of person resposible
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> PersonResponsibleList { get; set; }

        /// <summary>
        /// Defines person resposible in observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserAC UserAC { get; set; }

        /// <summary>
        /// Type of observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public ObservationType ObservationType { get; set; }

        /// <summary>
        /// Disposition of observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public Disposition Disposition { get; set; }

        /// <summary>
        /// Status of observation
        /// </summary>
        [Export(IsAllowExport = false)]
        public ObservationStatus ObservationStatus { get; set; }

        /// <summary>
        /// Status of observation list
        /// </summary>
        [Export(IsAllowExport = false)]
        public ObservationStatus ObservationListStatus { get; set; }

        /// <summary>
        /// Defines the ObservationType as (string)Ennumerable
        /// </summary>
        [DisplayName("Observation Type")]
        public string ObservationTypeToString { get; set; }

        /// <summary>
        /// Defines the Disposition as (string)Ennumerable
        /// </summary>
        [DisplayName("Disposition")]
        public string DispositionToString { get; set; }

        /// <summary>
        /// Defines the ObservationStatus as (string)Ennumerable
        /// </summary>
        [DisplayName("Observation Status")]
        public string ObservationStatusToString { get; set; }

        /// <summary>
        /// Foreign key of auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }

        /// <summary>
        /// List of observations to be linked in management tab
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ObservationAC> LinkedObservationACList { get; set; }


        /// <summary>
        /// Target date of observation
        /// </summary>
        [DisplayName("Target Date")]
        public string ObservationTargetDate { get; set; }

        /// <summary>
        /// First Person resposible of observation
        /// </summary>
        [DisplayName("First Person Resposible")]
        public string PersonResposibleName { get; set; }

        /// <summary>
        /// Observation category of observation
        /// </summary>
        [DisplayName("Observation Category")]
        public string ObservationCategoryName { get; set; }


        /// <summary>
        /// Observation category of observation
        /// </summary>
        [DisplayName("Audit Plan")]
        public string AuditPlanName { get; set; }


        /// <summary>
        /// Is Repeated Observation in observation
        /// </summary>
        [DisplayName("Is Repeated")]
        public string IsRepeated { get; set; }

        /// <summary>
        /// Defines observation is save as draft or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsDraft { get; set; }


        /// <summary>
        /// Defines attachment files
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<IFormFile> ObservationFiles { get; set; }

        /// <summary>
        /// List of observation documents
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ObservationDocumentAC> ObservationDocuments { get; set; }


        /// <summary>
        /// list of observation table
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ObservationTableAC> ObservationTableList { get; set; }

    }
}
