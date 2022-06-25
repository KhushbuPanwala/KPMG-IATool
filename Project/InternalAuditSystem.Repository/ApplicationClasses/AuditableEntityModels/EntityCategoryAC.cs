using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityCategoryAC : BaseModelAC
    {

        /// <summary>
        /// Name of selected entity
        /// </summary>
        [DisplayName("Enitty Name")]
        public string EntityName { get; set; }

        /// <summary>
        /// Name for category for entity
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Entity Sector Name")]
        public string CategoryName { get; set; }


        /// <summary>
        /// Foreign key for auditable entity 
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

    }
}
