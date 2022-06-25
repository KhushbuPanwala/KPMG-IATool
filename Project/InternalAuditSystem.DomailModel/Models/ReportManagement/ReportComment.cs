using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportComment : BaseModel
    {
        /// <summary>
        /// Foreign key for Report table
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Comment for report
        /// </summary>
        public string Comment { get; set; }


        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

    }
}
