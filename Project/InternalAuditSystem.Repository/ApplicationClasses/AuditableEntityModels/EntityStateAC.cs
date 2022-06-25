using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class EntityStateAC : BaseModelAC
    {   
        /// <summary>
        /// Name of selected entity
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Foreign key for EntityCountry
        /// </summary>
        [Export(IsAllowExport =false)]
        public Guid? EntityCountryId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Foreign key for state
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? StateId { get; set; }

        /// <summary>
        /// Name of  state
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string StateName { get; set; }

        /// <summary>
        /// Name of  country
        /// </summary>
        public string CountryName { get; set; }


        /// <summary>
        /// Name of region
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// List of country
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<EntityCountryAC> CountryACList { get; set; }


        /// <summary>
        /// List of state
        /// </summary>
        [Export(IsAllowExport = false)]
        public List<ProvinceStateAC> StateACList { get; set; }
    }
}
