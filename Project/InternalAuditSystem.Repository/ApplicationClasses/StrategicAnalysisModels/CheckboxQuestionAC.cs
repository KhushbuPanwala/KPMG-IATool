using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class CheckboxQuestionAC : BaseModelAC
    {
        /// <summary>
        /// Determines if Other field to be included or not
        /// </summary>
        public bool IsOtherToBeShown { get; set; }

        /// <summary>
        /// Shows all the related answers seeded in RelatedAnswer table
        /// </summary>
        public RelatedAnswer RelatedAnswer { get; set; }

        /// <summary>
        /// Stores other field name
        /// </summary>
        public string? FieldLabel { get; set; }

        /// <summary>
        /// Options of checkbox question
        /// </summary>
        public List<OptionAC> Options { get; set; }

    }
}
