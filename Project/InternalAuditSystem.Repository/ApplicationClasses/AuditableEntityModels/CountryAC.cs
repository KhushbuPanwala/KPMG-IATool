using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class CountryAC : BaseModelAC
    {

        /// <summary>
        /// Name of country
        /// </summary>
        [RegularExpression(RegexConstant.CountryStateRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Entity country id
        /// </summary>
        public Guid EntityCountryId { get; set; }

        /// <summary>
        /// Region Id
        /// </summary>
        public Guid RegionId { get; set; }
    }
}

