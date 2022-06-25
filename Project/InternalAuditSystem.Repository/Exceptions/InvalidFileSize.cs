using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;
namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class InvalidFileSize : Exception
    {
        public InvalidFileSize()
            : base(string.Format(StringConstant.InvalidFilSizeMessage))
        {
        }
    }
}

