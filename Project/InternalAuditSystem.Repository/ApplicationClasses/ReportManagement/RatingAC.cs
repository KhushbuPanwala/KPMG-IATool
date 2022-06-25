using InternalAuditSystem.DomainModel.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class RatingAC : BaseModelAC
    {
        /// <summary>
        /// Rating for report
        /// </summary>
        [RegularExpression(RegexConstant.AlphanumericRegex)]
        [MaxLength(RegexConstant.RatingsLength)]
        public string Ratings { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Qualitative factor for ratings
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string QualitativeFactors { get; set; }

        /// <summary>
        /// Quantitative factors for rating
        /// </summary>
        [MaxLength(RegexConstant.MaxInputLength)]
        public string QuantitativeFactors { get; set; }

        /// <summary>
        /// Score for ratings
        /// </summary>
        [RegularExpression(RegexConstant.NaturalNumberRegex)]
        public int Score { get; set; }

        /// <summary>
        /// Legend for ratings
        /// </summary>
        public string Legend { get; set; }
    }
}
