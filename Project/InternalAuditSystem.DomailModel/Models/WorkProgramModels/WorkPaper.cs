using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.WorkProgramModels
{
    public class WorkPaper : BaseModel
    {
        /// <summary>
        /// Defines path  for saving documents
        /// </summary>
        public string DocumentPath { get; set; }

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
