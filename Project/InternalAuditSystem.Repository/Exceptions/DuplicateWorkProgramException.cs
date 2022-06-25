using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class DuplicateWorkProgramException : Exception
    {
        public DuplicateWorkProgramException()
        {
        }
        public DuplicateWorkProgramException(string process)
               : base(string.Format(StringConstant.DuplicateWorkProgramMessage, process))
        {
        }
    }
}
