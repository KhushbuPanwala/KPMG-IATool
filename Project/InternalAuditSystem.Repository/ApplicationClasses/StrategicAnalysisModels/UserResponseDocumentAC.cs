using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class UserResponseDocumentAC : BaseModelAC
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
        public UserResponseAC UserResponse { get; set; }

        /// <summary>
        /// File name to display
        /// </summary>
        public string FileName { get; set; }
    }
}
