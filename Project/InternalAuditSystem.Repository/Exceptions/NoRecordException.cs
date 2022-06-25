using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class NoRecordException : Exception
    {
        public NoRecordException()
        {

        }
        public NoRecordException(string recordName)
            : base(string.Format(StringConstant.NoRecordMessage, recordName))
        {

        }
    }
}
