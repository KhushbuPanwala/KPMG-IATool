using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.MomModels
{
    public  class MomUserMapping:BaseModel
    {
        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines foreign key of MomId
        /// </summary>
        public Guid MomId { get; set; }

        /// <summary>
        /// Defines foreign key of SubPointOfDiscussion
        /// </summary>
        public Guid? SubPointOfDiscussionId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Defines foreign key relationship with Mom
        /// </summary>
        [ForeignKey("MomId")]
        public virtual Mom Mom { get; set; }

        /// <summary>
        /// Defines foreign key relationship with SubPointOfDiscussion
        /// </summary>
        [ForeignKey("SubPointOfDiscussionId")]
        public virtual SubPointOfDiscussion SubPointOfDiscussion { get; set; }


    }
}
