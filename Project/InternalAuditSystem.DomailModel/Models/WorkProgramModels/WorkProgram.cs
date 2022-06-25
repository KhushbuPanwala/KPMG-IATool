using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;

namespace InternalAuditSystem.DomailModel.Models.WorkProgramModels
{
    public class WorkProgram : BaseModel
    {
        /// <summary>
        /// Defines name of workprogram
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines audit title
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string AuditTitle { get; set; }

        /// <summary>
        /// Defines enum type status of workprogram
        /// </summary>
        public WorkProgramStatus Status { get; set; }

        /// <summary>
        /// Defines start date of audit period
        /// </summary
        public DateTime? AuditPeriodStartDate { get; set; }

        /// <summary>
        /// Defines end date of audit period
        /// </summary>
        public DateTime? AuditPeriodEndDate { get; set; }

        /// <summary>
        /// Defines scope of workprogram
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Scope { get; set; }

        /// <summary>
        /// Defines draft issues 
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string DraftIssues { get; set; }




    }
}
