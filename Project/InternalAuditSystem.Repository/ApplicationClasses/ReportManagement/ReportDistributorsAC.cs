using InternalAuditSystem.DomailModel.Models.UserModels;
using InternalAuditSystem.DomainModel.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class ReportDistributorsAC : BaseModelAC
    {
        /// <summary>
        /// List of available Distributor for selected auditable entity
        /// </summary>
        public List<DistributorsAC> DistributorsList { get; set; }

        /// <summary>
        /// List of distributor for specific report 
        /// </summary>
        public  List<ReportUserMappingAC> ReportDistributorsList { get; set; }
    }
}
