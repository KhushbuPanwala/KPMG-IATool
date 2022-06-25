using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class PageRequest
    {
        /// <summary>
        /// Current page no.
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// No. of items per page 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Search string value
        /// </summary>
        public string SearchString { get; set; }
    }
}
