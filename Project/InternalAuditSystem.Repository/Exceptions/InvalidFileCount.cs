using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;
namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class InvalidFileCount : Exception
    {
        public InvalidFileCount()
            : base(string.Format(StringConstant.InvalidFileLimitExceedMessage))
        {
        }
    }
}

