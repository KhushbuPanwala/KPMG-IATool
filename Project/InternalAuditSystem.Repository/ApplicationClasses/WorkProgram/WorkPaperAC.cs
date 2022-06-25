using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.WorkProgram
{
    public class WorkPaperAC: BaseModelAC
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
        /// File name to display
        /// </summary>
        public string fileName { get; set; }
    }
}
