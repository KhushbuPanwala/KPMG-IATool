using InternalAuditSystem.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.Exceptions
{
    [Serializable]
    public class DeleteValidationException : Exception
    {
        public DeleteValidationException()
        {
        }
        public DeleteValidationException(string moduleName, string dataExistModuleName)
               : base(string.Format(StringConstant.DeleteValidationMessage, dataExistModuleName, moduleName))
        {
        }
    }
}
