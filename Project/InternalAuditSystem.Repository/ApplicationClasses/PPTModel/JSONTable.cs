using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class JSONTable
    {
        /// <summary>
        /// defines name of observation / report observation etc..
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines list of json table data
        /// </summary>
        public JSONRoot JsonData { get; set; }
    }
}
