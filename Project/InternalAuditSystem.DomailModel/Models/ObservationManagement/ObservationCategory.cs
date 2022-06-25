using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ObservationManagement
{
    public class ObservationCategory : BaseModel
    {
        /// <summary>
        /// Defines the name of the Category
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Defines the ParentId:- CategoryId in itself
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table AuditableEntity
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines the table AuditableEntityDetail as type : virtual 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }

        /// <summary>
        /// Defines the table ObservationCategory in itself as type : virtual 
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual ObservationCategory ParentObservationCategory { get; set; }
    }
}