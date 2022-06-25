using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class StrategicAnalysisTeamAC : BaseModelAC
    {
        /// <summary>
        /// Defines strategic analysis id
        /// </summary>
        public Guid? StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines strategic analysis id
        /// </summary>
        public StrategicAnalysisAC StrategicAnalysis { get; set; }

        /// <summary>
        /// Defines user id
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Defines foreign key relation with user
        /// </summary>
        public UserAC User { get; set; }


        /// <summary>
        /// Name of team member
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Designation of Team member
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Designation { get; set; }

        /// <summary>
        /// Email of Team member
        /// </summary>
        public string EmailId { get; set; }


        /// <summary>
        /// Defines the name of the Auditable entity
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string AuditableEntityName { get; set; }

    }
}
