using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models.AuditableEntityModels
{
    public class ProvinceState : BaseModel
    {
        /// <summary>
        /// Name of province or state
        /// </summary>
        [RegularExpression(RegexConstant.CountryStateRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Name { get; set; }

        /// <summary>
        /// Foreign key for Country
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
