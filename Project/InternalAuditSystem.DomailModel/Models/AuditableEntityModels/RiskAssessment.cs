using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class RiskAssessment : BaseModel
    {
        /// <summary>
        /// Defines the Name of the Risk
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)] 
        public string Name { get; set; }

        /// <summary>
        /// Defines status as (string)ennumerable
        /// </summary>
        public RiskAssessmentStatus Status { get; set; }

        /// <summary>
        /// Defines the year of assessment
        /// </summary>
        [RegularExpression(RegexConstant.NaturalNumberRegex)]
        [MaxLength(RegexConstant.YearLength)]
        public int Year { get; set; }

        /// <summary>
        /// Defines the summary of the risk assessment 
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Summary { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the table AuditableEntity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity as type : virtual 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
