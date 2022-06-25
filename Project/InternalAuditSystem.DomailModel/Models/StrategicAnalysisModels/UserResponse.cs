using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.RiskAndControlModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class UserResponse : BaseModel
    {
        /// <summary>
        /// Defines Foreign Key : ID from table QuestionsGroup
        /// </summary>
        public Guid? QuestionId { get; set; }

        /// <summary>
        /// Defines the table Question 
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Defines strategic analysis id
        /// </summary>
        public Guid StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines foreign key relation with strategic analysis 
        /// </summary>

        [ForeignKey("StrategicAnalysisId")]
        public virtual StrategicAnalysis StrategicAnalysis { get; set; }

        /// <summary>
        /// Defines UserId of User
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Defines foreign key relation with user
        /// </summary>

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Defines parent user response id for file upload
        /// </summary>
        public Guid? ParentFileUploadUserResponseId { get; set; }

        /// <summary>
        /// Defines self foreign key relationship with user response of file upload
        /// </summary>
        [ForeignKey("ParentFileUploadUserResponseId")]
        public virtual UserResponse ParentFileUploadUserResponse { get; set; }

        /// <summary>
        /// Defines option id
        /// </summary>
        public List<Guid>? OptionIds { get; set; }

        /// <summary>
        /// Defines foreign key relation with option 
        /// </summary>
        [ForeignKey("OptionIds")]
        public virtual List<Option> Options { get; set; }

        /// <summary>
        /// Defines other text
        /// </summary>
        public string? Other { get; set; }

        /// <summary>
        /// Defines representation number
        /// </summary>
        public int? RepresentationNumber { get; set; }

        /// <summary>
        /// Defines answer text
        /// </summary>
        public string? AnswerText { get; set; }

        /// <summary>
        /// Defines dropdown option selected
        /// </summary>
        public string? SelectedDropdownOption { get; set; }

        /// <summary>
        /// Determines if files are from email attachments or question's user response files
        /// </summary>
        public bool IsEmailAttachements { get; set; }

        /// <summary>
        /// Defines user response's status
        /// </summary>
        public StrategicAnalysisStatus UserResponseStatus { get; set; }

        /// <summary>
        /// Defines status of sampling
        /// </summary>
        public SamplingResponseStatus? SamplingResponseStatus { get; set; }

        /// <summary>
        /// Defines RiskControlMatrixId for sampling
        /// </summary>
        public Guid? RiskControlMatrixId { get; set; }

        /// <summary>
        /// Defines foreign key relation with strategic analysis 
        /// </summary>

        [ForeignKey("RiskControlMatrixId")]
        public virtual RiskControlMatrix RiskControlMatrix { get; set; }


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

        public bool IsDetailAndEstimatedValueOfOpportunity { get; set; }

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
