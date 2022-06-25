using InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.User
{
    public class LoggedInUserDetails
    {
        /// <summary>
        /// List of auditable entity in which user is added as a memebr
        /// </summary>
        public List<AuditableEntityAC> PermittedEntityList { get; set; }

        /// <summary>
        /// Details of current logged in user
        /// </summary>
        public UserAC UserDetails { get; set; }
            
    }
}
