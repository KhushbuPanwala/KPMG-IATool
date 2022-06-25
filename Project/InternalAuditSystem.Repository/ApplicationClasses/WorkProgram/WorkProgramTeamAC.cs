using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.WorkProgram
{
    public class WorkProgramTeamAC : BaseModelAC
    {
        /// <summary>
        /// Name of WorkProgram
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Workprogram Name")]
        public string WorkProgramName { get; set; } 

        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

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
        /// UserType of Team member
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserType UserType { get; set; }

        /// <summary>
        /// Type of team such as Team or Client Participant
        /// </summary>
        [Export(IsAllowExport = false)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Type { get; set; }

        /// <summary>
        /// Defines foreign key of work program
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid WorkProgramId { get; set; }
    }
}
