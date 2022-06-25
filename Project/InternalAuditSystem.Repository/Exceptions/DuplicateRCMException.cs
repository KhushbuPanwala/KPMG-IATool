using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    public class DuplicateRCMException : Exception
    {
        public DuplicateRCMException()
        {
        }
        public DuplicateRCMException(String rcm)
               : base(string.Format(StringConstant.DuplicateRCMMessage, rcm))
        {
        }
    }
}
