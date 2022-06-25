﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using InternalAuditSystem.DomailModel.Models.UserModels;

namespace InternalAuditSystem.DomailModel.Models.ReportManagement
{
    public class ReportObservationReviewer : BaseModel
    {
        /// <summary>
        /// Defienes reviewer id Foreign key for user table
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for ReportObservation
        /// </summary>
        public Guid ReportObservationId { get; set; }

        /// <summary>
        /// Comment for report observation members
        /// </summary>
        public string Comment { get; set; }


        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Foreign key relation 
        /// </summary>
        [ForeignKey("ReportObservationId")]
        public virtual ReportObservation ReportObservation { get; set; }

    }
}
