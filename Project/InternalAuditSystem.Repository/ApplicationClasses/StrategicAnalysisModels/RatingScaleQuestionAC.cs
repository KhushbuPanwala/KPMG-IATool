using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class RatingScaleQuestionAC : BaseModelAC
    {
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
