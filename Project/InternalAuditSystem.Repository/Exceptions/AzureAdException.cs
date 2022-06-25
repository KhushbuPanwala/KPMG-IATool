using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class AzureAdException : Exception
    {
        public AzureAdException()
        {
        }
        public AzureAdException(string exceptionMessage,int? statusCode =null)
            : base(string.Format(exceptionMessage))
        {
        }
    }
}
