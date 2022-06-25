using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    /// <summary>
    /// Custome attribute to ignore property in export to excel
    /// </summary>
    public class ExportAttribute : Attribute
    {
        /// <summary>
        /// Define property is exported into excel or not
        /// </summary>
        public bool IsAllowExport { get; set; }

    }
}
