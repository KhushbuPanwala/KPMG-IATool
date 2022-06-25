using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.UserModels
{
    public class User : BaseModel
    {
        /// <summary>
        /// Defines name of user
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines Emailid of user
        /// </summary>
        [RegularExpression(RegexConstant.EmailAddressRegex)]
        public string EmailId { get; set; }

        /// <summary>
        /// Defines enumtype designation of user
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Defines enumtype type of user
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Defines LastInterectedDateTime of user
        /// </summary>
        public DateTime LastInterectedDateTime { get; set; }

        /// <summary>
        /// Defines Department of user 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Auditable entity id for the current selected 
        /// </summary>
        public Guid? CurrentSelectedEntityId { get; set; }      
    }
}
