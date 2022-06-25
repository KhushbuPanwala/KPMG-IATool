using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class Country : BaseModel
    {
        /// <summary>
        /// Name of country
        /// </summary>
        [RegularExpression(RegexConstant.CountryStateRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }
    }
}
