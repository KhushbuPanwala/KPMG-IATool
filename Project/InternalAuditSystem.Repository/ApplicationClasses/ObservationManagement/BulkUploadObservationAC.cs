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
    public class BulkObservationAC
    {
        /// <summary>
        /// Defines process name
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// Defines sub process name
        /// </summary>
        public string SubProcess { get; set; }

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
        /// Defines all the observations
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Defines the type of the Observation
        /// </summary>
        public string ObservationType { get; set; }

        /// <summary>
        /// Checks if there is any repeat in observation
        /// </summary>
        public string IsRepeated { get; set; }

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
        public string Disposition { get; set; }

        /// <summary>
        /// Defines the observation status
        /// </summary>
        public string Status { get; set; }

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
    }
}
