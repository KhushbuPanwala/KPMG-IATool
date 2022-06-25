using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityCountryAC : BaseModelAC
    {
        /// <summary>
        /// Name of selected entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Foreign key for Region
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? RegionId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Foreign key for Country
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// Name of country
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string CountryName { get; set; }

        /// <summary>
        /// Name of region
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// List of region
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<RegionAC> RegionACList { get; set; }

        /// <summary>
        /// List of country
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<CountryAC> CountryACList { get; set; }
    }
}
