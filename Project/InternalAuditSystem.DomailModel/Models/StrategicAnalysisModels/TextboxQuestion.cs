using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class TextboxQuestion : BaseModel
    {
        /// <summary>
        /// Defines Foreign Key : Id from table Question
        /// </summary>
        public Guid QuestionId;

        /// <summary>
        /// Defines the table question 
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Defines the Lower Limit of the character
        /// </summary>
        public int CharacterLowerLimit { get; set; }

        /// <summary>
        /// Defines the Upper Limit of the character
        /// </summary>
        public int CharacterUpperLimit { get; set; }

        /// <summary>
        /// Defines the Guidance 
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string? Guidance { get; set; }
    }
}
