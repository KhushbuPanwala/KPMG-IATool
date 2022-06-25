using InternalAuditSystem.DomailModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class UserResponseDocument : BaseModel
    {
        /// <summary>
        /// Defines path of the Document
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Defines Foreign Key : Id from table UserResponse
        /// </summary>
        public Guid UserResponseId { get; set; }

        /// <summary>
        /// Defines the table UserResponse 
        /// </summary>
        [ForeignKey("UserResponseId")]
        public virtual UserResponse UserResponse { get; set; }
    }
}
