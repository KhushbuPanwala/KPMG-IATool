using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.AzureAdModel
{
    public class AzureAccessToken
    {
        /// <summary>
        /// Primary key for all models
        /// </summary>
        [Key]
        public Guid? Id { get; set; }

        /// <summary>
        /// Token Value to acess azure Ad
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Defines the acess token type
        /// </summary>
        public string TokenType { get; set; }
        /// <summary>
        /// Date of creation for all models
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Date of updation for all models
        /// </summary>
        public DateTime? UpdatedDateTime { get; set; }
    }
}
