using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class OptionAC : BaseModelAC
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
        public DropdownQuestionAC DropdownQuestion { get; set; }

        /// <summary>
        /// Defines multiple choice question Id 
        /// </summary>
        public Guid? MultipleChoiceQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with multiple choice question 
        /// </summary>
        public MultipleChoiceQuestionAC MultipleChoiceQuestion { get; set; }

        /// <summary>
        /// Defines rating question id
        /// </summary>
        public Guid? RatingQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with rating scale question
        /// </summary>
        public RatingScaleQuestionAC RatingScaleQuestion { get; set; }
              

        /// <summary>
        /// Defines checkbox question id
        /// </summary>
        public Guid? CheckboxQuestionId { get; set; }

        /// <summary>
        /// Defines foreign key relation with checkbox question
        /// </summary>
        public CheckboxQuestionAC CheckboxQuestionQuestion { get; set; }
        
    }
}
