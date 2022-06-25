using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class RiskAssessmentDocument : BaseModel
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
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("RiskAssessmentId")]
        public virtual RiskAssessment RiskAssessment { get; set; }


    }
}
