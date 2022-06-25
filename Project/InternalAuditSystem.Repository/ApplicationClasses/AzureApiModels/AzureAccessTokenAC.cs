using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models
{
    public class AzureAccessTokenAC
    {
        /// <summary>
        /// Defines the acess token type 
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Defines the acess token value
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
