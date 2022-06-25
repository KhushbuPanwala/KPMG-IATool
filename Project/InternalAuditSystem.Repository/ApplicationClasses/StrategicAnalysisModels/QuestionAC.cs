using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class QuestionAC : BaseModelAC
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
        /// defines sort order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Defines the table strategyAnalysis as type : virtual
        /// </summary>
        public StrategicAnalysisAC StrategyAnalysis { get; set; }
        /// <summary>
        /// Defines checkbox question
        /// </summary>
        public CheckboxQuestionAC CheckboxQuestion { get; set; }

        /// <summary>
        /// Defines dropdown question
        /// </summary>
        public DropdownQuestionAC DropdownQuestion { get; set; }

        /// <summary>
        /// Defines file upload question
        /// </summary>
        public FileUploadQuestionAC FileUploadQuestion { get; set; }

        /// <summary>
        /// Defines multiple choice question
        /// </summary>
        public MultipleChoiceQuestionAC MultipleChoiceQuestion { get; set; }

        /// <summary>
        /// Defines rating scale question
        /// </summary>
        public RatingScaleQuestionAC RatingScaleQuestion { get; set; }

        /// <summary>
        /// Defines subjective question
        /// </summary>
        public SubjectiveQuestionAC SubjectiveQuestion { get; set; }

        /// <summary>
        /// Defines textbox question
        /// </summary>
        public TextboxQuestionAC TextboxQuestion { get; set; }

        /// <summary>
        /// List of options
        /// </summary>
        public List<string> Options { get; set; }

        /// <summary>
        /// User response of each question
        /// </summary>
        public UserResponseAC UserResponse { get; set; }

        /// <summary>
        /// File user response of each question
        /// </summary>
        public List<UserResponseAC> FileResponseList { get; set; }

        /// <summary>
        /// Defines if user response exists for this question
        /// </summary>
        public bool IsUserResponseExists { get; set; }

        /// <summary>
        /// Defines user response documents of file type question
        /// </summary>
        public List<UserResponseDocumentAC> UserResponseDocumentACs { get; set; }
        /// <summary>
        /// Defines the type of question
        /// </summary>
        public string QuestionType { get; set; }

    }
}
