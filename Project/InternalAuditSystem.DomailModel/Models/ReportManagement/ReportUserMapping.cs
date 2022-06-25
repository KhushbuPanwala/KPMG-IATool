using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportUserMapping : BaseModel
    {
        /// <summary>
        /// Defines the status as (string)Ennumerable
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// Defines the ReportUserType as (string)Ennumerable
        /// </summary>
        public ReportUserType ReportUserType { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from the Table Report
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Defines Foriegn Key: Id from the table Users
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines the table Report as type : virtual 
        /// </summary>
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        /// <summary>
        /// Defines the table User as type : virtual 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
