using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement;
using System.Collections.Generic;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportDetailAC : BaseModelAC
    { 
        /// <summary>
        /// define process id
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// defind subprocess id
        /// </summary>
        public string SubProcessId { get; set; }

        /// <summary>
        /// define entity id
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// define report id
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// List of Observations 
        /// </summary>
        public List<ReportObservationAC> ReportObservationList { get; set; }

        /// <summary>
        /// List of ratings
        /// </summary>
        public List<RatingAC> RatingList { get; set; }

        /// <summary>
        /// Defines the ObservationType as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ObservationTypeList { get; set; }

        /// <summary>
        /// Defines the Disposition as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> Disposition { get; set; }

        /// <summary>
        /// Defines the ObservationStatus as (string)Ennumerable
        /// </summary>
        public List<KeyValuePair<int, string>> ObservationStatus { get; set; }

        /// <summary>
        /// List of Observation Category 
        /// </summary>
        public List<ObservationCategoryAC> ObservationCategoryList { get; set; }

        /// <summary>
        /// Count of Report observations
        /// </summary>
        public int TotalReportObservation { get; set; }


        /// <summary>
        /// list of auditor 
        /// </summary>
        public List<EntityUserMappingAC> AuditorList { get; set; }

        /// <summary>
        /// list of person responsible shows in management comment tab drop down
        /// </summary>
        public List<EntityUserMappingAC> ResponsibleUserList { get; set; }

        /// <summary>
        /// list of observation reviewer
        /// </summary>
        public List<ReportUserMappingAC> ObservationReviewerList { get; set; }


        /// <summary>
        /// List of linked observation Observations 
        /// </summary>
        public List<ReportObservationAC> LinkedObservationList { get; set; }
    }
}
