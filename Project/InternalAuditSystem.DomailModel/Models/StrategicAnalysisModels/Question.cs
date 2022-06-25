using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class Question : BaseModel
    {
        /// <summary>
        /// Defines the Question stated
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string QuestionText { get; set; }

        /// <summary>
        /// Defines the type of question
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Checks if it is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table StrategyAnalysis
        /// </summary>
        public Guid StrategyAnalysisId { get; set; }

        /// <summary>
        ///defines sort order 
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Defines the table strategyAnalysis as type : virtual
        /// </summary>
        [ForeignKey("StrategyAnalysisId")]
        public virtual StrategicAnalysis StrategyAnalysis { get; set; }

        /// <summary>
        /// Defines checkbox question
        /// </summary>
        public virtual CheckboxQuestion CheckboxQuestion { get; set; }

        /// <summary>
        /// Defines dropdown question
        /// </summary>
        public virtual DropdownQuestion DropdownQuestion { get; set; }

        /// <summary>
        /// Defines file upload question
        /// </summary>
        public virtual FileUploadQuestion FileUploadQuestion { get; set; }

        /// <summary>
        /// Defines multiple choice question
        /// </summary>
        public virtual MultipleChoiceQuestion MultipleChoiceQuestion { get; set; }

        /// <summary>
        /// Defines rating scale question
        /// </summary>
        public virtual RatingScaleQuestion RatingScaleQuestion { get; set; }

        /// <summary>
        /// Defines subjective question
        /// </summary>
        public virtual SubjectiveQuestion SubjectiveQuestion { get; set; }

        /// <summary>
        /// Defines textbox question
        /// </summary>
        public virtual TextboxQuestion TextboxQuestion { get; set; }

    }
}
