using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    public class RequiredDataException : Exception
    {
        public RequiredDataException()
        {
        }
        public RequiredDataException(String property)
               : base(string.Format(StringConstant.RequiredDataMessage, property))
        {
        }
    }
}
