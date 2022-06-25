using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class StrategicAnalysisUserMappingAC : BaseModelAC
    {

        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines foreign key of StrategicAnalysisId
        /// </summary>
        public Guid? StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with User
        /// </summary>
        public UserAC UserAC { get; set; }

        /// <summary>
        /// Defines foreign key relationship with Strategic Analysis
        /// </summary>
        public StrategicAnalysisAC StrategicAnalysisAC { get; set; }
    }
}
