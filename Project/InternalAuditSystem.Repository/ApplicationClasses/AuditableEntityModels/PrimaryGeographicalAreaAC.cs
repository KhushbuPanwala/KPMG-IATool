using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class PrimaryGeographicalAreaAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines the name of the Entity
        /// </summary>
        [DisplayName("Auditable Entity")]
        public string AuditableEntity { get; set; }

        /// <summary>
        /// Foreign key for Region
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid RegionId { get; set; }

        /// <summary>
        /// Region Name to display
        /// </summary>
        [DisplayName("Region")]
        public string RegionString { get; set; }

        /// <summary>
        /// Foreign key for Entity Country
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityCountryId { get; set; }

        /// <summary>
        /// Country name to display
        /// </summary>
        [DisplayName("Country")]
        public string CountryString { get; set; }

        /// <summary>
        /// Foreign key for EntityState
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityStateId { get; set; }

        /// <summary>
        /// State name to display
        /// </summary>
        [DisplayName("State")]
        public string StateString { get; set; }

        /// <summary>
        /// Foreign key for Location
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid LocationId { get; set; }

        /// <summary>
        /// Location name to display
        /// </summary>
        [DisplayName("Location")]
        public string LocationString { get; set; }

        /// <summary>
        /// RegionAC List for type dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RegionAC> RegionACList { get; set; }

        /// <summary>
        /// LocationAC List for type dropdown
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<LocationAC> LocationACList { get; set; }
    }
}
