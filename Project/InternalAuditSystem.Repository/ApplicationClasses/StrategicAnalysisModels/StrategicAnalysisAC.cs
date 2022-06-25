using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels;
using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class StrategicAnalysisAC : BaseModelAC
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
        /// Defines Foreign Key : Id from table AuditableEntity
        /// </summary>
        public Guid? AuditableEntityId { get; set; }

        /// <summary>
        /// Rcm id 
        /// </summary>
        public string RcmId { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity as type : virtual
        /// </summary>
        public AuditableEntityAC AuditableEntity { get; set; }

        /// <summary>
        /// Question count
        /// </summary>
        public int QuestionsCount { get; set; }

        /// <summary>
        /// Response Count
        /// </summary>
        public int ResponseCount { get; set; }

        /// <summary>
        /// Collection of Team
        /// </summary>
        public List<UserAC> TeamCollection { get; set; }

        /// <summary>
        /// Collection of StrategicUserMappingAC
        /// </summary>
        public List<StrategicAnalysisTeamAC> InternalUserList { get; set; }


        /// <summary>
        /// Decides if verioning to be done or not
        /// </summary>
        public bool IsVersionToBeChanged { get; set; }

        /// <summary>
        /// Questions of strategic analysis
        /// </summary>
        public List<QuestionAC> Questions { get; set; }

        /// <summary>
        /// Decides if user response is to be added or updated
        /// </summary>
        public bool? IsUserResponseToBeUpdated { get; set; }

        /// <summary>
        /// Defines email attachment files
        /// </summary>
        public List<IFormFile> Files { get; set; }

        /// <summary>
        /// defines strategy is active or not
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Defines user response documents
        /// </summary>
        public List<UserResponseDocumentAC> UserResponseDocumentACs { get; set; }

        /// <summary>
        /// Object to send general data of strategic analysis
        /// </summary>
        public UserResponseForDetailsAndEstimatedValueOfOpportunity UserResponseForDetailsOfOppAndEstimatedValue { get; set; }

        /// <summary>
        /// Is response is drafted
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsResponseDrafted { get; set; }

        /// <summary>
        /// String data of cshtml for generating pdf
        /// </summary>
        [Export(IsAllowExport = false)]
        public string PdfFileString { get; set; }


        /// <summary>
        /// Defines the details of opportunity of the Analysis
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string DetailsOfOpportunity { get; set; }

        /// <summary>
        /// Defines the estimated value of oppertunity of the Analysis
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string EstimatedValueOfOpportunity { get; set; }

        /// <summary>
        /// Is email is attched
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool? IsEmailAttached { get; set; }

        /// <summary>
        /// List of survey user
        /// </summary>
        public List<UserAC> SurveyUserList { get; set; }

        /// <summary>
        /// defines user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// defines designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// defines email id 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// defines user wise response
        /// </summary>
        public UserWiseResponseAC  UserResponseList{ get; set; }
        
    }
}
