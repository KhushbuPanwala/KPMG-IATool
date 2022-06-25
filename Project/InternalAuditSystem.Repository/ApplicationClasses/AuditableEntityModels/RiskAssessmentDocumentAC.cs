using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class RiskAssessmentDocumentAC : BaseModelAC
    {
        /// <summary>
        /// Path for document
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid RiskAssessmentId { get; set; }

        /// <summary>
        /// File name to display
        /// </summary>
        public string FileName { get; set; }
    }
}
