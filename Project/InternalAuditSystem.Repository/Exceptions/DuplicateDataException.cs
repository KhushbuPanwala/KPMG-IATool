using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;
namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class DuplicateDataException : Exception
    {
        public DuplicateDataException()
        {
        }
        public DuplicateDataException(string fieldName, string name)
            : base(string.Format(StringConstant.DuplicateDataMessage, fieldName, name))
        {
        }
        public DuplicateDataException(string fieldName, string name,string regionName)
            : base(string.Format(StringConstant.DuplicateCountryNameMessage, fieldName, name, regionName))
        {
        }
    }
}