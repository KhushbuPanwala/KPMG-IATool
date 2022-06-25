using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.User
{
    public class UserAC : BaseModelAC
    {
        /// <summary>
        /// Name of selected entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Defines name of user
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines Emailid of user
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [RegularExpression(RegexConstant.EmailAddressRegex)]
        public string EmailId { get; set; }


        /// <summary>
        /// Defines enumtype designation of user
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Designation { get; set; }

        /// <summary>
        /// Defines enumtype of user to display in export
        /// </summary>
        [DisplayName("UserType")]
        public string UserTypeString { get; set; }

        /// <summary>
        /// Defines enumtype type of user
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserType UserType { get; set; }

        /// <summary>
        /// Defines LastInterectedDateTime of user
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime LastInterectedDateTime { get; set; }

        /// <summary>
        /// Entity user mapping Id of a user
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityUserMappingId { get; set; }

        /// <summary>
        /// Defines Department of user 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// This property only for saving audit team data
        /// </summary>
        [Export(IsAllowExport = false)]
        public string AuditMemberEmailId { get; set; }

        /// <summary>
        /// Auditable entity id for the current selected 
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? CurrentSelectedEntityId { get; set; }

        /// <summary>
        ///  Based on user jobtitle user role of user in this system
        /// </summary>
        [Export(IsAllowExport = false)]
        public UserRole UserRole { get; set; }
    }
}
