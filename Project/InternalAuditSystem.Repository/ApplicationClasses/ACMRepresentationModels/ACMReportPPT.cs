namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMReportPPT
    {

        /// <summary>
        /// Serial no.
        /// </summary>
        public string SrNo { get; set; }

        /// <summary>
        /// Name of Report
        /// </summary>
        public string ReportTitle { get; set; }

        /// <summary>
        /// No. of Observation
        /// </summary>
        public string NoOfObservation { get; set; }

        /// <summary>
        /// Name of Rating
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// Report Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Report stage
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// Report period
        /// </summary>
        public string Period { get; set; }
    }

}
