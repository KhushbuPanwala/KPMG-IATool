using InternalAuditSystem.DomailModel.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ObservationManagement
{
    public class ObservationDocument : BaseModel
    {
        /// <summary>
        /// Defines the Path of the Document
        /// </summary>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Defines the DocumentFor as (string)ennumerable
        /// </summary>
        public ObservationDocumentFor DocumentFor { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table Observation
        /// </summary>
        public Guid ObservationId { get; set; }

        /// <summary>
        /// Defines the table Observation as type : virtual
        /// </summary>
        [ForeignKey("ObservationId")]
        public virtual Observation Observation { get; set; }
    }
}
