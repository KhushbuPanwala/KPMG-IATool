using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException()
        {
        }
        public InvalidFileException(String fileName)
               : base(string.Format(StringConstant.InvalidFileMessage, fileName))
        {
        }
    }
}
