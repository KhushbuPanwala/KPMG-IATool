using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.ReportManagement;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomailModel.Models
{
    public class ACMPresentation : BaseModel
    {
        /// <summary>
        /// Heading of ACM Precentation
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Heading { get; set; }

        /// <summary>
        /// Recommendation for ACM presentation
        /// </summary>
        /// Note :Regex for this text area type field will be added later
        public string Recommendation { get; set; }

        /// <summary>
        /// Defines foreign key of Rating master
        /// </summary>
        public Guid? RatingId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with rating master
        /// </summary>
        [ForeignKey("RatingId")]
        public virtual Rating Rating { get; set; }

        /// <summary>
        /// ManagementResponse for ACM Presentation
        /// </summary>
        public string ManagementResponse { get; set; }

        /// <summary>
        /// Observation for ACM Presentation
        /// </summary>
        public string Observation { get; set; }

        /// <summary>
        /// Implications for AM Presentation
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Implication { get; set; }

        /// <summary>
        /// Status for ACM Presentation
        /// </summary>
        public ACMStatus Status { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("EntityId")]
        public virtual AuditableEntityModels.AuditableEntity AuditableEntity { get; set; }
    }
}
