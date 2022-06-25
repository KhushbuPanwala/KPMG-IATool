using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.MomModels
{
    public class MainDiscussionPointAC : BaseModelAC
    {
        /// <summary>
        /// Defines foreign key of Mom
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? MomId { get; set; }

        /// <summary>
        /// Defines workprogram name
        /// </summary>
        public string WorkProgram { get; set; }

        /// <summary>
        /// Defines agenda
        /// </summary>
        public string Agenda { get; set; }

        /// <summary>
        /// Defines main point of Mom
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string MainPoint { get; set; }

        /// <summary>
        /// Defines sub point of Mom
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string SubPoint { get; set; }

        /// <summary>
        /// Defines target date of subpoint
        /// </summary>
        [DisplayName("TargetDate")]
        public string TargetDateToString { get; set; }

        /// <summary>
        /// Defines name of person responsible
        /// </summary>
        [DisplayName("PersonResponsibleName")]
        public string PersonResponsibleName { get; set; }

        /// <summary>
        /// Defines Designation of person resposible
        /// </summary>
        public string Designation { get; set; }
        /// <summary>
        /// Status string
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Collection of SubPointOfDiscussion
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<SubPointOfDiscussionAC> SubPointDiscussionACCollection { get; set; }


       
    }
}
