using InternalAuditSystem.DomainModel.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityDocumentAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Purpose for entity document
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Purpose { get; set; }

        /// <summary>
        /// Path for document
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// File name for document
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// List of Risk assessmnent file AC
        /// </summary>
        public List<IFormFile> EntityDocumentFiles { get; set; }
    }
}
