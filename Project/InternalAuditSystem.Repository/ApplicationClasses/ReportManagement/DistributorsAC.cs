using System;

namespace InternalAuditSystem.Repository.ApplicationClasses.ReportManagement
{
    public class DistributorsAC : BaseModelAC
    {
        /// <summary>
        /// Foreign key for user table
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid UserId { get; set; }

        /// <summary>
        /// Foreign key for auditable entity
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Defines name of user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines enumtype designation of user
        /// </summary>
        public string Designation { get; set; }
    }
}
