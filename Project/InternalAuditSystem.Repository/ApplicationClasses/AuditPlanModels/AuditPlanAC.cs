using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlan
{
    public class AuditPlanAC : BaseModelAC
    {
        [MaxLength(RegexConstant.MaxInputLength)]
        /// <summary>
        /// Defines title of audit plan
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Defines foreign key of audit type
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid? SelectedTypeId { get; set; }

        /// <summary>
        /// Display property for audit type
        /// </summary>
        public string AuditTypeName { get; set; }

        /// <summary>
        /// Defines foreign key of audit category
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? SelectCategoryId { get; set; }

        /// <summary>
        /// Display property for audit category
        /// </summary>
        public string AuditCategoryName { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines due on date of audit plan
        /// </summary>  
        [Export(IsAllowExport = false)]
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Defines indentified on date of audit cycle
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Defines enum type status of audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public AuditPlanStatus Status { get; set; }

        /// <summary>
        /// Defines enum string status of audit plan for display
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Defines version of audit plan
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// Defines overview background of audit plan
        /// </summary>
        public string OverviewBackground { get; set; }

        [RegularExpression(RegexConstant.DecimalNumberRegex)]
        /// <summary>
        /// Defines total budgetedhours of audit plan
        /// </summary>
        public double? TotalBudgetedHours { get; set; }

        /// <summary>
        ///  List of audit types under an auditable entity for display in dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditTypeAC> AuditTypeSelectionDisaplyList { get; set; }

        /// <summary>
        /// List of audit categories under an auditable entity for display in dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditCategoryAC> AuditCategorySelectionDisaplyList { get; set; }

        /// <summary>
        /// List of all plan processes that added in a plan to add/edit
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<PlanProcessMappingAC> PlanProcessList { get; set; }

        /// <summary>
        ///  List of all parent process for every subprocess added in plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ProcessAC> ParentProcessList { get; set; }

        /// <summary>
        /// List of all plan documents that added in a plan to add/edit
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditPlanDocumentAC> PlanDocumentList { get; set; }

        /// <summary>
        /// Defines the audit plan section whether its general/overview/planprocess or document section
        /// </summary>
        [Export(IsAllowExport = false)]
        public AuditPlanSectionType SectionType { get; set; }

        /// <summary>
        /// Defines financial year of audit plan
        /// </summary>
        [RegularExpression(RegexConstant.NaturalNumberRegex)]
        public int FinancialYear { get; set; }

        /// <summary>
        /// Defines self foreign key relationship with audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ParentPlanId { get; set; }

        /// <summary>
        /// Defines due on date of audit plan
        /// </summary>  
        [DisplayName("IdentifiedOn")]
        public string EndDateTimeToString { get; set; }

        /// <summary>
        /// Defines indentified on date of audit cycle
        /// </summary>
        [DisplayName("DueDate")]
        public string StartDateTimeToString { get; set; }
    }
}
