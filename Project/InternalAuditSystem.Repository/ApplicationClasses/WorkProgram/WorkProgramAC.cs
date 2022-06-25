using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlan;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.WorkProgram
{
    public class WorkProgramAC : BaseModelAC
    {
        /// <summary>
        /// Defines name of workprogram
        /// </summary>
        [DisplayName("Workprogram Name")]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        ///  Parent process Id for work Program
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ParentProcessId { get; set; }

        /// <summary>
        ///  Sub-process Id for work Program
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid ProcessId { get; set; }

        /// <summary>
        /// Name of Process
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }

        /// <summary>
        /// Name of Process
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Sub Process Name")]
        public string SubProcessName { get; set; }

        /// <summary>
        /// Name of Audit plan
        /// </summary>
        [DisplayName("Audit Plan")]
        public string AuditPlanName { get; set; }

        /// <summary>
        /// Defines audit title
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Audit Title")]
        public string AuditTitle { get; set; }

        /// <summary>
        /// Status string
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Defines enum type status of workprogram
        /// </summary>
        [Export(IsAllowExport = false)]
        public WorkProgramStatus Status { get; set; }

        /// <summary>
        /// Defines foreign key of audit plan
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid AuditPlanId { get; set; }

        /// <summary>
        /// Defines start date of audit period
        /// </summary
        [Export(IsAllowExport = false)]
        public DateTime? AuditPeriodStartDate { get; set; }

        /// <summary>
        /// Defines end date of audit period
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime? AuditPeriodEndDate { get; set; }

        /// <summary>
        /// Defines start and date of audit period for grid
        /// </summary
        [DisplayName("Audit Period")]
        public string AuditPeriod { get; set; }

        /// <summary>
        /// Defines scope of workprogram
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Scope { get; set; }

        /// <summary>
        /// WorkPapers of workprogram
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<WorkPaperAC> WorkPaperACList { get; set; }

        /// <summary>
        /// Work program team list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<WorkProgramTeamAC> WorkProgramTeamACList { get; set; }

        /// <summary>
        /// WorkProgram Team's First name to show in list page
        /// </summary>
        [Export(IsAllowExport = false)]
        public string TeamFirstName { get; set; }

        /// <summary>
        /// Work program team list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<WorkProgramTeamAC> WorkProgramClientParticipantsACList { get; set; }

        /// <summary>
        /// AuditPlan AC list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditPlanAC> AuditPlanACList { get; set; }

        /// <summary>
        /// ProcessList
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ProcessAC> ProcessACList { get; set; }

        /// <summary>
        /// SubProcessACList
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<PlanProcessMappingAC> SubProcessACList { get; set; }

        /// <summary>
        /// InternalUserAC list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> InternalUserAC { get; set; }

        /// <summary>
        /// ExternalUserAC list
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> ExternalUserAC { get; set; }

        /// <summary>
        /// Property to get file from client side
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<IFormFile> WorkPaperFiles { get; set; }

        /// <summary>
        /// Current selected entity id to check
        /// </summary>
        [Export(IsAllowExport = false)]
        public string SelectedEntityId { get; set; }
    }
}
