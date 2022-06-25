using InternalAuditSystem.DomailModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class Option : BaseModel
    {
        /// <summary>
        /// Defines option for a question
        /// </summary>
        public string OptionText { get; set; }

        /// <summary>
        /// Defines dropdown type question Id
        /// </summary>
        public Guid? DropdownQuestionId { get; set; }

        /// <summary>
        /// Defines dropdown question
        /// </summary>
        [ForeignKey("DropdownQuestionId")]
        public virtual DropdownQuestion DropdownQuestion { get; set; }

        /// <summary>
        /// Defines multiple choice question Id 
        /// </summary>
        public Guid? MultipleChoiceQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with multiple choice question 
        /// </summary>
        [ForeignKey("MultipleChoiceQuestionId")]
        public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }

        /// <summary>
        /// Defines rating question id
        /// </summary>
        public Guid? RatingQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with rating scale question
        /// </summary>

        [ForeignKey("RatingQuestionId")]
        public virtual RatingScaleQuestion RatingScaleQuestion { get; set; }
        
        /// <summary>
        /// Defines checkbox question id
        /// </summary>
        public Guid? CheckboxQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with checkbox question
        /// </summary>

        [ForeignKey("CheckboxQuestionId")]
        public virtual CheckboxQuestion CheckboxQuestionQuestion { get; set; }
        
        
    }
}
