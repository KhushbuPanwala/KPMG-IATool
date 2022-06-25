using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityRelationMappingAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        [DisplayName("Auditable Entity")]
        public string AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key for RelationshipType
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RelationTypeId { get; set; }

        /// <summary>
        /// Related Entity Name
        /// </summary>
        [DisplayName("Related Entity")]
        public string RelatedEntityName { get; set; }

        /// <summary>
        /// Relation Name
        /// </summary>
        [DisplayName("Relationship Type")]
        public string RelationName { get; set; }

        /// <summary>
        /// Foreign key for RelationEntity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RelatedEntityId { get; set; }


        /// <summary>
        /// Auditable list for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<AuditableEntityAC> AuditableEntityACList { get; set; }

        /// <summary>
        /// RelationshipType list for dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RelationshipTypeAC> RelationshipTypeACList { get; set; }
    }
}
