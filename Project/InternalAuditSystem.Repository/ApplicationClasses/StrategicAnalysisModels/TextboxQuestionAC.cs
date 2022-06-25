using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class TextboxQuestionAC : BaseModelAC
    {
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
        public string Guidance { get; set; }

        /// <summary>
        /// Defines answer of the question
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string AnswerText { get; set; }
    }
}
