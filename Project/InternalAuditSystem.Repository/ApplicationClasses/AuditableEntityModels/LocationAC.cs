using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class LocationAC : BaseModelAC
    {
        /// <summary>
        /// Name of selected entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Defines the Name of the Location
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the table AuditableEntity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }
    }
}
