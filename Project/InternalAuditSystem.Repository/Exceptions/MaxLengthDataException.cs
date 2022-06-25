using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    public class MaxLengthDataException : Exception
    {
        public MaxLengthDataException()
        {
        }
        public MaxLengthDataException(String property)
               : base(string.Format(StringConstant.MaxLengthDataMessage, property))
        {
        }
    }
}
