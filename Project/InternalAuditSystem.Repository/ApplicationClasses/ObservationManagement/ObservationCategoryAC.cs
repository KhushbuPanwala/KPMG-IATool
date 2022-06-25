using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.Repository.ApplicationClasses.ObservationManagement
{
    public class ObservationCategoryAC : BaseModelAC
    {
        /// <summary>
        /// Name of selected entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Defines the name of the Category
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Defines the ParentId:- CategoryId in itself
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table AuditableEntity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }
    }
}