using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalAuditSystem.DomailModel.Models
{
    public class BaseModel
    {
        /// <summary>
        /// Primary key for all models
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Date of creation for all models
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Date of updation for all models
        /// </summary>
        public DateTime? UpdatedDateTime { get; set; }

        /// <summary>
        /// Created by user foreign key
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Updated by user foreign key
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Is deleted boolean key for soft delete
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Foreign key relation with User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public virtual User UserCreatedBy { get; set; }

        /// <summary>
        /// Foreign key relation with User
        /// </summary>
        [ForeignKey("UpdatedBy")]
        public virtual User UserUpdatedBy { get; set; }
    }
}
