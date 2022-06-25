using InternalAuditSystem.DomailModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class RatingScaleQuestion : BaseModel
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
        /// Defines the starting of the scale
        /// </summary>
        public int ScaleStart { get; set; }

        /// <summary>
        /// Defines the ending of the scale
        /// </summary>
        public int ScaleEnd { get; set; }

        /// <summary>
        /// Defines the start label
        /// </summary>
        public string StartLabel { get; set; }

        /// <summary>
        /// Defines the end label
        /// </summary>
        public string EndLabel { get; set; }

        /// <summary>
        /// Defines the representation
        /// </summary>
        public string Representation { get; set; }

        /// <summary>
        /// Defines the smiley list to be shown on basis of ScaleStart and ScaleEnd
        /// </summary>
        public List<int> ResultSmileyList { get; set; }
    }
}
