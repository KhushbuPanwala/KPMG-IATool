using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class DropdownQuestion : BaseModel
    {

        /// <summary>
        /// Defines Foreign Key : Id from table Question
        /// </summary>
        public Guid QuestionId;

        /// <summary>
        /// Defines the table question 
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        /// <summary>
        /// Shows all the related answers seeded in RelatedAnswer table
        /// </summary>
        public RelatedAnswer? RelatedAnswer { get; set; }
    }
}
