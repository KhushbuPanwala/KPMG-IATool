using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class DeleteLinkedDataException : Exception
    {
        public DeleteLinkedDataException()
        {
        }
        public DeleteLinkedDataException(string exceptionMessage,string moduleName)
            : base(string.Format(exceptionMessage, moduleName))
        {
        }
    }
}
