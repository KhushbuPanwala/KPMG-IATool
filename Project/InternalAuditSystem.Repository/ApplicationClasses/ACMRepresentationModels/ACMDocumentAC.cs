using InternalAuditSystem.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.ACMRepresentationModels
{
    public class ACMDocumentAC : BaseModelAC
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
        public ACMDocumentFor DocumentFor { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table Observation
        /// </summary>
        public Guid ACMId { get; set; }

    }
}
