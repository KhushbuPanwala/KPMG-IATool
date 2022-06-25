using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses.StrategicAnalysisModels
{
    public class FileUploadQuestionAC : BaseModelAC
    {
        /// <summary>
        /// Defines the Guidance 
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string Guidance { get; set; }

        /// <summary>
        /// checks if the format : Doc type is allowed
        /// </summary>
        public bool IsDocAllowed { get; set; }

        /// <summary>
        /// checks if the format : Gif type is allowed
        /// </summary>
        public bool IsGifAllowed { get; set; }

        /// <summary>
        /// checks if the format : Jpeg type is allowed
        /// </summary>
        public bool IsJpegAllowed { get; set; }

        /// <summary>
        /// checks if the format : Ppx type is allowed
        /// </summary>
        public bool IsPpxAllowed { get; set; }

        /// <summary>
        /// checks if the format : Png type is allowed
        /// </summary>
        public bool IsPngAllowed { get; set; }

        /// <summary>
        /// checks if the format : Pdf type is allowed
        /// </summary>
        public bool IsPdfAllowed { get; set; }

    }
}
