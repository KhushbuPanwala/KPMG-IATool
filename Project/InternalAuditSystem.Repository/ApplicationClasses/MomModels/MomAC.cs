using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.ApplicationClasses.WorkProgram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.MomModels
{
    public class MomAC : BaseModelAC
    {
        /// <summary>
        /// Defines foreign key of work program 
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid WorkProgramId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with work program
        /// </summary>
        [Export(IsAllowExport = false)]
        public WorkProgramAC WorkProgramAc { get; set; }

        /// <summary>
        /// Defines start date of Mom
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime MomDate { get; set; }

        /// <summary>
        /// Defines start time of Mom
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime MomStartTime { get; set; }

        /// <summary>
        /// Defines end time of Mom
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime MomEndTime { get; set; }

        /// <summary>
        /// Defines closure meeting date of Mom
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime ClosureMeetingDate { get; set; }

        /// <summary>
        /// Defines WorkProgram 
        /// </summary>
        [DisplayName("WorkProgram")]
        public string WorkProgramString { get; set; }

        /// <summary>
        /// Defines agenda of Mom
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Agenda { get; set; }

        /// <summary>
        /// Defines foreign key of auditable entity 
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with AuditableEntityAC
        /// </summary>
        [Export(IsAllowExport = false)]
        public AuditableEntityModels.AuditableEntityAC AuditableEntityAc { get; set; }

        /// <summary>
        /// Collection of SubPointOfDiscussion
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<MainDiscussionPointAC> MainDiscussionPointACCollection { get; set; }

        /// <summary>
        /// Collection of SubPointOfDiscussion
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<SubPointOfDiscussionAC> SubPointDiscussionACCollection { get; set; }

        /// <summary>
        /// Collection of selected person responsible 
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<MomUserMappingAC> PersonResposibleACCollection { get; set; }

        /// <summary>
        /// Collection for All person resposible  showing in dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> AllPersonResposibleACDataCollection { get; set; }

        /// <summary>
        /// Collection of Team
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> TeamCollection { get; set; }

        /// <summary>
        /// Collection of client participants
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<UserAC> ClientParticipantCollection { get; set; }

        /// <summary>
        /// Collection of WorkProgram
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<WorkProgramAC> WorkProgramCollection { get; set; }

        /// <summary>
        /// Collection of MomUserMappingAC
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<MomUserMappingAC> InternalUserList { get; set; }

        /// <summary>
        /// Collection of MomUserMappingAC
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<MomUserMappingAC> ExternalUserList { get; set; }

        /// <summary>
        /// This is to set serail no in client  side 
        /// </summary>
        [Export(IsAllowExport = false)]
        public int SrNo { get; set; }

        /// <summary>
        /// String data of cshtml for generating pdf
        /// </summary>
        [Export(IsAllowExport = false)]
        public string PdfFileString { get; set; }

        /// <summary>
        /// Defines start date of Mom
        /// </summary>
        [DisplayName("MomDate")]
        public string MomDateToString { get; set; }

        /// <summary>
        /// Defines start time of Mom
        /// </summary>
        [DisplayName("MomStartTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// Defines end time of Mom
        /// </summary>
        [DisplayName("MomEndTime")]
        public string EndTime { get; set; }

        /// <summary>
        /// Defines closure meeting date of Mom
        /// </summary>
        [DisplayName("ClosureMeetingDate")]
        public string ClosureMeetingDateToString { get; set; }
    }
}
