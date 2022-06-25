using InternalAuditSystem.DomailModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class DropdownQuestionAC : BaseModelAC
    {
        /// <summary>
        /// Shows all the related answers seeded in RelatedAnswer table
        /// </summary>
        public RelatedAnswer RelatedAnswer { get; set; }

        /// <summary>
        /// Options of dropdown question
        /// </summary>
        public List<OptionAC> Options { get; set; }
    }
}
