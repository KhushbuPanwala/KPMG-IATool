using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class JSONDatum
    {
        /// <summary>
        /// defines json document row data
        /// </summary>
        public List<string> RowData { get; set; }
    }
}
