using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomailModel.Models.UserModels;

namespace InternalAuditSystem.DomailModel.Models.MomModels
{
   public class SubPointOfDiscussion:BaseModel
    {
        /// <summary>
        /// Defines subpoint of mainpoint of Mom
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string SubPoint { get; set; }

        /// <summary>
        /// Defines status of subpoint
        /// </summary>
        public SubPointStatus Status { get; set; }

        /// <summary>
        /// Defines target date of subpoint
        /// </summary>
        public DateTime TargetDate { get; set; }

        /// <summary>
        /// Defines foreign key of mainpoint 
        /// </summary>
        public Guid MainPointId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with main discussion points
        /// </summary>
        [ForeignKey("MainPointId")]
        public virtual MainDiscussionPoint  MainDiscussionPoint { get; set; }


        /// <summary>
        /// List of person responsible
        /// </summary>
        public virtual List<MomUserMapping> PersonResponsibleCollection { get; set; }
    }
}
