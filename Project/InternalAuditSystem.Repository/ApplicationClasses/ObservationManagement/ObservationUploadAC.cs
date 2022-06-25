using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement
{
    public class ObservationUploadAC
    {
        /// <summary>
        /// List of audit plan
        /// </summary>
        public List<AuditPlanAC> AuditPlanList { get; set; }

        /// <summary>
        /// Defines the ObservationType as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ObservationType { get; set; }

        /// <summary>
        /// Defines the Disposition as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> Disposition { get; set; }

        /// <summary>
        /// Defines the ObservationStatus as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ObservationStatus { get; set; }

    }
}
