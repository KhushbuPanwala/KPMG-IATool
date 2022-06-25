using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityUserMappingAC : BaseModelAC
    {
        /// <summary>
        ///  Foreign key for user
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines name of user
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines designation of user
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Defines emailid of user
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Defines Department of user 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// User model data
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserAC User { get; set; }

        /// <summary>
        /// Defines enumtype type of user
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserType UserType { get; set; }

        /// <summary>
        /// This is to set serail no in client  side 
        /// </summary>
        [Export(IsAllowExport = false)]
        public int SrNo { get; set; }

    }
}
