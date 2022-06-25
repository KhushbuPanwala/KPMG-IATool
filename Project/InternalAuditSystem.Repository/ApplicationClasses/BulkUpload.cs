using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class BulkUpload
    {
        /// <summary>
        /// Defines seleted Entity id
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines files as user response
        /// </summary>
        public List<IFormFile> Files { get; set; }
    }
}
