using InternalAuditSystem.DomailModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
   public class ACMReviewerAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for ACMPresentation 
        /// </summary>
        public Guid ACMPresentationId { get; set; }

        /// <summary>
        /// User foreign key
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines name of user
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines enumtype designation of user
        /// </summary>
        [DisplayName("Designation")]
        public string Designation { get; set; }
        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// Defines the status as (string)Enumerable
        /// </summary>
        [DisplayName("Status")]
        public string StatusName { get; set; }


        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public ReportUserType ReportUserType { get; set; }


        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        [DisplayName("User Type")]
        public string UserType { get; set; }

        /// <summary>
        /// Reviewer documents list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ACMReviewerDocumentAC> ReviewerDocumentList { get; set; }
    }
}
