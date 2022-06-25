using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.MomModels
{
  public  class MainDiscussionPoint:BaseModel
    {
        /// <summary>
        /// Defines foreign key of Mom
        /// </summary>
        public Guid MomId { get; set; }

        /// <summary>
        /// Defines main point of Mom
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string MainPoint { get; set; }

        /// <summary>
        /// Defines foreign key relationship with Mom
        /// </summary>
        [ForeignKey("MomId")]
        public virtual Mom Mom { get; set; }

        /// <summary>
        /// Collection of SubPointOfDiscussion
        /// </summary>
        public virtual ICollection<SubPointOfDiscussion> SubPointDiscussionCollection { get; set; }
    }
}
