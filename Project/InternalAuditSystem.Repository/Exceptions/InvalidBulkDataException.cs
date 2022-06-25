using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    public class InvalidBulkDataException : Exception
    {
        public InvalidBulkDataException()
        {
        }
        public InvalidBulkDataException(String property, string value)
               : base(string.Format(StringConstant.InvalidBulkDataMessage, property, value))
        {
        }
    }
}
