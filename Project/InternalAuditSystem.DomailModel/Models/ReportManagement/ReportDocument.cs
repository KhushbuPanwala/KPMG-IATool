using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.ReportManagement
{
    public class ReportDocument : BaseModel
    {
        /// <summary>
        /// Defines the Path of the Document
        /// </summary>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Defines the DocumentFor as (string)ennumerable
        /// </summary>
        public ACMReportDocumentFor DocumentFor { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table Observation
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Defines the table Observation as type : virtual
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }
    }
}
