using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class RiskAssessmentAC : BaseModelAC
    {
        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        [DisplayName("Auditable Entity")]
        public string AuditableEntity { get; set; }

        /// <summary>
        /// Defines the Name of the Risk
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines status as (string)ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public RiskAssessmentStatus Status { get; set; }


        /// <summary>
        /// Defines status as (string)ennumerable
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Defines the year of assessment
        /// </summary>
        [RegularExpression(RegexConstant.NaturalNumberRegex)]
       
        public int Year { get; set; }

        /// <summary>
        /// Defines the summary of the risk assessment 
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        [DisplayName("Summary of Assessment")]
        public string Summary { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the table AuditableEntity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Old id for copying data
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? OldId { get; set; }

        /// <summary>
        /// List of Risk assessmnent documents AC
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RiskAssessmentDocumentAC> RiskAssessmentDocumentACList { get; set; }

        /// <summary>
        /// List of Risk assessmnent file AC
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<IFormFile> RiskAssessmentDocumentFiles { get; set; }
    }
}
