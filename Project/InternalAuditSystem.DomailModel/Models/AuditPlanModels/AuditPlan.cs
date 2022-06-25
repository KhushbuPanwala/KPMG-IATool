using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditPlanModels
{
    public class AuditPlan : BaseModel
    {
        /// <summary>
        /// Defines self foreign key relationship with audit plan
        /// </summary>
        public Guid? ParentPlanId { get; set; }

        /// <summary>
        /// Defines title of audit plan
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Title { get; set; }

        /// <summary>
        /// Defines foreign key of audit type
        /// </summary>
        public Guid? SelectedTypeId { get; set; }
        
        /// <summary>
        /// Defines foreign key of audit category
        /// </summary>
        public Guid? SelectCategoryId { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines due on date of audit plan
        /// </summary>  
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Defines indentified on date of audit cycle
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Defines enum type status of audit plan
        /// </summary>
        public AuditPlanStatus Status { get; set; }

        /// <summary>
        /// Defines version of audit plan
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// Defines overview background of audit plan
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string  OverviewBackground { get; set; }

        /// <summary>
        /// Defines total budgetedhours of audit plan
        /// </summary>
        [RegularExpression(RegexConstant.DecimalNumberRegex)]
        public double? TotalBudgetedHours { get; set; }

        /// <summary>
        /// Defines financial year of audit plan
        /// </summary>
        [RegularExpression(RegexConstant.NaturalNumberRegex)]
        [MaxLength(RegexConstant.YearLength)]
        public int FinancialYear { get; set; }
        
        /// <summary>
        /// Defines foreign key relationship with auditable entity
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Defines self foreign key relationship with aduit plan
        /// </summary>
        [ForeignKey("ParentPlanId")]
        public virtual AuditPlan ParentAuditPlan { get; set; }
    }
}
