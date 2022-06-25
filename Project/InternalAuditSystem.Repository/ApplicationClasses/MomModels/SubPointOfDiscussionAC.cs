using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.MomModels
{
    public class SubPointOfDiscussionAC : BaseModelAC
    {
        /// <summary>
        /// Defines workprogram name
        /// </summary>
        public string WorkProgram { get; set; }

        /// <summary>
        /// Defines agenda
        /// </summary>
        public string Agenda { get; set; }

        /// <summary>
        /// Defines subpoint of mainpoint of Mom
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string SubPoint { get; set; }

        /// <summary>
        /// Status string
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Defines status of subpoint
        /// </summary>
        [Export(IsAllowExport = false)]
        public SubPointStatus Status { get; set; }

        /// <summary>
        /// Defines target date of subpoint
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Defines foreign key of mainpoint 
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? MainPointId { get; set; }

        /// <summary>
        /// Collection of person responsible
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<MomUserMappingAC> PersonResponsibleACCollection { get; set; }


        /// <summary>
        /// This is to set serail no in client  side 
        /// </summary>
        [Export(IsAllowExport = false)]
        public string AlphabetSrNo { get; set; }


        


       
    }
}
