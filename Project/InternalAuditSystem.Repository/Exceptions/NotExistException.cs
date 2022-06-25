using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class NotExistException : Exception
    {
        public NotExistException()
        {
        }
        public NotExistException(string fieldName)
            : base(string.Format(StringConstant.NotExistMessage, fieldName))
        {
        }

    }
}
