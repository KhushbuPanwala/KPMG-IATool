using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class Division : BaseModel
    {
        /// <summary>
        /// Defines the Name of the division
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the table AuditableEntity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity as type : virtual 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
