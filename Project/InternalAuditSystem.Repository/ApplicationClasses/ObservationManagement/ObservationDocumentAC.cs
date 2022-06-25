using InternalAuditSystem.DomailModel.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement
{
    public class ObservationDocumentAC : BaseModelAC
    {
        /// <summary>
        /// Defines the Path of the Document
        /// </summary>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Defines the path of the document for display
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Defines the DocumentFor as (string)ennumerable
        /// </summary>
        public ObservationDocumentFor DocumentFor { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table Observation
        /// </summary>
        public Guid ObservationId { get; set; }

    }
}
