using InternalAuditSystem.DomailModel.Models.WorkProgramModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.MomModels
{
    public class Mom : BaseModel
    { 
        /// <summary>
        /// Defines foreign key of work program 
        /// </summary>
        public Guid WorkProgramId { get; set; }

        /// <summary>
        /// Defines start date of Mom
        /// </summary>
        public DateTime MomDate { get; set; }

        /// <summary>
        /// Defines start time of Mom
        /// </summary>
        public DateTime MomStartTime { get; set; }

        /// <summary>
        /// Defines end time of Mom
        /// </summary>
        public DateTime MomEndTime { get; set; }

        /// <summary>
        /// Defines closure meeting date of Mom
        /// </summary>
        public DateTime ClosureMeetingDate { get; set; }

        /// <summary>
        /// Defines agenda of Mom
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Agenda { get; set; }

        /// <summary>
        /// Defines foreign key relationship with work program
        /// </summary>
        [ForeignKey("WorkProgramId")]
        public virtual WorkProgram WorkProgram { get; set; }

        /// <summary>
        /// Foreign key of auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with AuditableEntity
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntityModels.AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Collection of MainDiscussionPoint
        /// </summary>
        public virtual ICollection<MainDiscussionPoint> MainDiscussionPointCollection { get; set; }

        /// <summary>
        /// Collection of SubPointOfDiscussion
        /// </summary>
        public virtual ICollection<SubPointOfDiscussion> SubPointDiscussionCollection { get; set; }

        /// <summary>
        /// Collection of MomUserMapping
        /// </summary>
        public virtual ICollection<MomUserMapping> MomUserMappingCollection { get; set; }

    }
}
