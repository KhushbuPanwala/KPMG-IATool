using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;
namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class InvalidFileFormate : Exception
    {
        public InvalidFileFormate()
            : base(string.Format(StringConstant.InvalidFileFormateMessage))
        {
        }
    }
}

