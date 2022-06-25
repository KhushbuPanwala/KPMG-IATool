using InternalAuditSystem.DomailModel.Models;
using InternalAuditSystem.DomailModel.Models.ACMPresentationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalAuditSystem.DomainModel.Models.ACMPresentationModels
{
    public class ACMReviewerDocument : BaseModel
    {
        /// <summary>
        /// Defines the Name of the document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Defines the Path of the document
        /// </summary>
        public string DocumentPath { get; set; }


        /// <summary>
        /// Foreign key of acm reviewer
        /// </summary>
        public Guid? AcmReviewerId { get; set; }

        /// <summary>
        /// Foreign key relation with acm reviewer
        /// </summary>
        [ForeignKey("AcmReviewerId")]
        public virtual ACMReviewer ACMReviewer { get; set; }
    }
}
