using InternalAuditSystem.DomailModel.Models.AuditPlanModels;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class AuditableEntity : BaseModel
    {

        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Defines status as (string)ennumerable
        /// </summary>
        public AuditableEntityStatus Status { get; set; }

        /// <summary>
        /// Defines the description of the entity
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Description { get; set; }

        /// <summary>
        /// Checks if the Strategy Analysis is done or not
        /// </summary>
        public bool IsStrategyAnalysisDone { get; set; }

        /// <summary>
        /// Defines the version of the Auditable Entity
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditType
        /// </summary>
        public Guid? SelectedTypeId { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table for versioning
        /// </summary>
        public Guid? ParentEntityId { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditCategory
        /// </summary>
        public Guid? SelectedCategoryId { get; set; }

        /// <summary>
        /// Defines the table EntityType as type : virtual
        /// </summary>
        [ForeignKey("SelectedTypeId")]
        public virtual EntityType EntityType { get; set; }

        /// <summary>
        /// Defines the table Entity Category as type : virtual
        /// </summary>
        [ForeignKey("SelectedCategoryId")]
        public virtual EntityCategory EntityCategory { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity : virtual
        /// </summary>
        [ForeignKey("ParentEntityId")]
        public virtual AuditableEntity ParentEntity { get; set; }
    }
}
