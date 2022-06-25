using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.MomModels
{
    public class MomUserMappingAC : BaseModelAC
    {
        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Name of workprogram
        /// </summary>
        public string WorkProgram { get; set; }

        /// <summary>
        /// Agenda of mom
        /// </summary>
        public string Agenda { get; set; }

        /// <summary>
        /// Name of team member
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Designation of Team member
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Designation { get; set; }

        /// <summary>
        /// Type of team such as Team or Client Participant
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Type { get; set; }

        /// <summary>
        /// Defines foreign key of MomId
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? MomId { get; set; }

        /// <summary>
        /// Defines foreign key of main discussion points
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? SubPointOfDiscussionId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with User
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserAC User { get; set; }

        /// <summary>
        /// Defines foreign key relationship with main discussion points
        /// </summary>
        [Export(IsAllowExport = false)]
        public SubPointOfDiscussionAC SubPointDiscussionAC { get; set; }
    }
}
