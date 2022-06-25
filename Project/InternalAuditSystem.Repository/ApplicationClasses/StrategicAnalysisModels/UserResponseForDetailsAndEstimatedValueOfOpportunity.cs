using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class UserResponseForDetailsAndEstimatedValueOfOpportunity
    {
        /// <summary>
        /// Defines Foreign Key : ID from table QuestionsGroup
        /// </summary>
        public Guid? QuestionId { get; set; }

        /// <summary>
        /// Question which user response is connected to
        /// </summary>
        public QuestionAC? Question { get; set; }

        /// <summary>
        /// Defines UserId of User
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Defines user
        /// </summary>
        public UserAC User { get; set; }


        /// <summary>
        /// Option containing option Id
        /// </summary>
        public List<OptionAC> Options { get; set; }

        /// <summary>
        /// Defines other text
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// Defines representation number
        /// </summary>
        public int RepresentationNumber { get; set; }

        /// <summary>
        /// Defines answer text
        /// </summary>
        public string AnswerText { get; set; }

        /// <summary>
        /// Defines strategic analysis id
        /// </summary>
        public Guid? StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines mcq question id
        /// </summary>
        public Guid? MultipleChoiceQuestionId { get; set; }

        /// <summary>
        /// Defines checkbox question id
        /// </summary>
        public Guid? CheckboxQuestionId { get; set; }

        /// <summary>
        /// Defines dropdown question id
        /// </summary>
        public Guid? DropdownQuestionId { get; set; }

        /// <summary>
        /// Defines files as user response
        /// </summary>
        public List<IFormFile> Files { get; set; }

        /// <summary>
        /// Defines checkbox options
        /// </summary>
        public List<string> CheckboxOptions { get; set; }

        /// <summary>
        /// Defines mcq option
        /// </summary>
        public string McqOption { get; set; }

        /// <summary>
        /// Defines selected dropdown option
        /// </summary>
        public string SelectedDropdownOption { get; set; }

        /// <summary>
        /// Defines user response's status
        /// </summary>
        public StrategicAnalysisStatus UserResponseStatus { get; set; }

        /// <summary>
        /// Defines status of sampling
        /// </summary>
        public SamplingResponseStatus? SamplingResponseStatus { get; set; }

        /// <summary>
        /// Defines user response's status string
        /// </summary>
        public string UserResponseStatusString { get; set; }

        /// <summary>
        /// Determines if files are from email attachments or question's user response files
        /// </summary>
        public bool IsEmailAttachements { get; set; }

        /// <summary>
        /// Defines RiskControlMatrixId for sampling
        /// </summary>
        public Guid? RiskControlMatrixId { get; set; }


        ///// <summary>
        ///// Defines the details of opportunity of the Analysis
        ///// </summary>
        //[RegularExpression(RegexConstant.CommonInputRegex)]
        //[MaxLength(RegexConstant.MaxInputLength)]
        public string DetailsOfOpportunity { get; set; }

        ///// <summary>
        ///// Defines the estimated value of oppertunity of the Analysis
        ///// </summary>
        //[RegularExpression(RegexConstant.CommonInputRegex)]
        //[MaxLength(RegexConstant.MaxInputLength)]
        public string EstimatedValueOfOpportunity { get; set; }

        public bool IsDetailAndEstimatedValueOfOpportunity { get; set; }

        /// <summary>
        /// Defines the name of the Auditable entity
        /// </summary>
        public string AuditableEntityName { get; set; }
    }
}
