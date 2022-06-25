using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class StrategicAnalysisTeam : BaseModel
    {
        /// <summary>
        /// Defines strategic analysis id
        /// </summary>

        public Guid StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines strategic analysis id
        /// </summary>

        [ForeignKey("StrategicAnalysisId")]
        public virtual StrategicAnalysis StrategicAnalysis { get; set; }

        /// <summary>
        /// Defines user id
        /// </summary>
        public Guid UserId { get; set; }


        /// <summary>
        /// Defines foreign key relation with user
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table AuditableEntity
        /// </summary>
        public Guid? AuditableEntityId { get; set; }

        /// <summary>
        /// Defines the table AuditableEntity as type : virtual
        /// </summary>
        [ForeignKey("AuditableEntityId")]
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
