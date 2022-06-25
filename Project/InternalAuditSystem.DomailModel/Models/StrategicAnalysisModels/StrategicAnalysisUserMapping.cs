using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class StrategicAnalysisUserMapping : BaseModel
    {
        /// <summary>
        /// Defines foreign key of User
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Defines foreign key of StrategicAnalysisId
        /// </summary>
        public Guid StrategicAnalysisId { get; set; }

        /// <summary>
        /// Defines foreign key relationship with User
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Defines foreign key relationship with StrategicAnalysis
        /// </summary>
        [ForeignKey("StrategicAnalysisId")]
        public virtual StrategicAnalysis StrategicAnalysis { get; set; }


    }
}
