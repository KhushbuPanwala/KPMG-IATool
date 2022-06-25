using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class StrategicAnalysis : BaseModel
    {
        /// <summary>
        /// Defines the Title or Name of the Survey
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string SurveyTitle { get; set; }

        /// <summary>
        /// Defines the name of the Auditable entity
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string AuditableEntityName { get; set; }

        /// <summary>
        /// Defines the message of the Analysis
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Message { get; set; }

        /// <summary>
        /// Defines the version of the Strategic Analysis 
        /// </summary>
        [RegularExpression(RegexConstant.DecimalNumberRegex)]
        public double Version { get; set; }

        /// <summary>
        /// Defines status of strategic analysis
        /// </summary>
        public StrategicAnalysisStatus Status { get; set; }

        /// <summary>
        /// Checks for Sampling formats
        /// </summary>
        public bool IsSampling { get; set; }


        /// <summary>
        /// defines stratgic is active or not
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table AuditableEntity
        /// </summary>
        public Guid? AuditableEntityId { get; set; }



        /// <summary>
        /// Defines the table AuditableEntity as type : virtual
        /// </summary>
        [ForeignKey("AuditableEntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }


    }
}
