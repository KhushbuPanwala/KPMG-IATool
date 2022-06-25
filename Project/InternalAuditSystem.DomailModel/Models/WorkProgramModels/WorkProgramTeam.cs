using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.WorkProgramModels
{
    public class WorkProgramTeam:BaseModel
    {
        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines foreign key relationship  with User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Defines foreign key of work program
        /// </summary>
        public Guid WorkProgramId { get; set; }

        /// <summary>
        /// Defines foreign key relationship  with work program
        /// </summary>
        [ForeignKey("WorkProgramId")]
        public virtual WorkProgram WorkProgram { get; set; }


    }
}
