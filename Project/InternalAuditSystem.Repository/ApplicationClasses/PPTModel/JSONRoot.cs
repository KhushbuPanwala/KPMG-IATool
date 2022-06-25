using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class JSONRoot
    {
        /// <summary>
        /// Defines list of row data
        /// </summary>
        public List<JSONDatum> data { get; set; }

        /// <summary>
        /// Defines table id
        /// </summary>
        public string tableId { get; set; }

        /// <summary>
        /// defines column name
        /// </summary>
        public List<string> columnNames { get; set; }

    }
}
