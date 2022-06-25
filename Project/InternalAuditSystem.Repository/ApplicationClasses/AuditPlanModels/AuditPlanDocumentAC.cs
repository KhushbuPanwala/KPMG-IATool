using InternalAuditSystem.DomainModel.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels
{
    public class AuditPlanDocumentAC : BaseModelAC
    {
        /// <summary>
        /// Defines foreign key of audit plan
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// Defines purpose of upload documents
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Purpose { get; set; }

        /// <summary>
        /// Defines path of documents to be saved
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// File name to display
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Property to get file object from client side
        /// </summary>
        public List<IFormFile> DocumentFile { get; set; }

        /// <summary>
        /// Bit to check new document uploaded or not 
        /// </summary>
        public bool IsNewDocuemntUploaded { get; set; }

        /// <summary>
        /// Current selected entity id to check
        /// </summary>
        public string SelectedEntityId { get; set; }
    }
}
