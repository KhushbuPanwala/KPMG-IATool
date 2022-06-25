using InternalAuditSystem.DomailModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class MultipleChoiceQuestionAC : BaseModelAC
    {

        /// <summary>
        /// Shows all the related answers seeded in RelatedAnswer table
        /// </summary>
        public RelatedAnswer RelatedAnswer { get; set; }

        /// <summary>
        /// Checks if other to be shown
        /// </summary>
        public bool IsOtherToBeShown { get; set; }

        /// <summary>
        /// Stores other field name
        /// </summary>
        public string? FieldLabel { get; set; }

        /// <summary>
        /// Options of multiple choice question
        /// </summary>
        public List<OptionAC> Options { get; set; }
    }
}
