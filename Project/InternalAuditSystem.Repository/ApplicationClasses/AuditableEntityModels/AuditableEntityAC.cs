using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using InternalAuditSystem.Repository.ApplicationClasses.AuditPlanModels;
using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class AuditableEntityAC : BaseModelAC
    {
        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        [DisplayName("Auditable Entity")]
        public string Name { get; set; }

        /// <summary>
        /// Defines status as (string)ennumerable
        /// </summary>
        [Export(IsAllowExport = false)]
        public AuditableEntityStatus Status { get; set; }

        /// <summary>
        /// Defines status as (string)
        /// </summary>
        [DisplayName("Status")]
        public string StatusString { get; set; }

        /// <summary>
        /// Defines the description of the entity
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Checks if the Strategy Analysis is done or not
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsStrategyAnalysisDone { get; set; }

        /// <summary>
        /// Defines the version of the Auditable Entity
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// Defines the new version of the Auditable Entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public double NewVersion { get; set; }

        /// <summary>
        /// Defines Auditable Entity is new version for update
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsNewVersion { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditType
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? SelectedTypeId { get; set; }

        /// <summary>
        /// Defines SelectedType String
        /// </summary>
        [DisplayName("Entity Type")]
        public string SelectedTypeString { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditCategory
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? SelectedCategoryId { get; set; }

        /// <summary>
        /// Defines SelectedType String
        /// </summary>
        [DisplayName("Entity Sector")]
        public string SelectedCategoryString { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditCategory
        /// </summary>
        [DisplayName("Engagement Managers")]
        public string EngagementManagerStringList { get; set; }

        /// <summary>
        /// Defines Foreign  Key : Id from table AuditType for versioning
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? ParentEntityId { get; set; }

        /// <summary>
        /// EntityCategoryAC for type dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<EntityCategoryAC> EntityCategoryACList { get; set; }

        /// <summary>
        /// EntityTypeAC for type dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<EntityTypeAC> EntityTypeACList { get; set; }
    }
}
