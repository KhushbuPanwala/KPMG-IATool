using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomainModel.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.StrategicAnalysisModels
{
    public class FileUploadQuestion : BaseModel
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
        /// Defines the Guidance 
        /// </summary>
        [RegularExpression(RegexConstant.CommonInputRegex)]
        [MaxLength(RegexConstant.MaxInputLength)]
        public string? Guidance { get; set; }

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
