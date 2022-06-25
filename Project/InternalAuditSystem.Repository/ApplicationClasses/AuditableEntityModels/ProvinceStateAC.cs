using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.AuditableEntityModels
{
    public class ProvinceStateAC : BaseModelAC
    {
        /// <summary>
        /// Name of province or state
        /// </summary>
        [RegularExpression(RegexConstant.CountryStateRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set;}

        /// <summary>
        /// Foreign key for Country
        /// </summary>
        public Guid CountryId { get; set; }
    }
}
