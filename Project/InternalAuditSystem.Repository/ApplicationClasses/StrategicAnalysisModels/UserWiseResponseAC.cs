using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class UserWiseResponseAC : BaseModelAC
    {
        /// <summary>
        /// Defines user id of user who has created this user response
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///  Name of the user who has given the response
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Check whether user response is drafted or not
        /// </summary>
        public bool IsReponseDrafted { get; set; }

        /// <summary>
        /// Rcm id 
        /// </summary>
        public string RcmId { get; set; }

        /// <summary>
        /// Description of the rcm
        /// </summary>
        public string RcmDescription { get; set; }

        /// <summary>
        /// Defines list of user responses with its question
        /// </summary>
        public List<UserResponseAC> ListOfUserResponseWithQuestion { get; set; }

        /// <summary>
        /// Defines strategic analysis to which this user response belongs to
        /// </summary>
        public StrategicAnalysisAC StrategicAnalysis { get; set; }

        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        public string AuditableEntityName { get; set; }

        /// <summary>
        /// defines auditable entity id
        /// </summary>
        public Guid? AuditableEntityId { get; set; }
    }
}
