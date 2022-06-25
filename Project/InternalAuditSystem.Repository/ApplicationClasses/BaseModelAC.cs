using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class BaseModelAC
    {
        /// <summary>
        /// Primary key for all models
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Date of creation for all models
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Date of creation for all models for export to excel
        /// </summary>
        [DisplayName("Created Date Time")]
        public string CreatedDate { get; set; }


        /// <summary>
        /// Date of updation for all models
        /// </summary>
        [Export(IsAllowExport = false)]
        public DateTime? UpdatedDateTime { get; set; }

        /// <summary>
        /// Date of updation for all models for export to excel
        /// </summary>
        [DisplayName("Updated Date Time")]
        public string UpdatedDate { get; set; }

        /// <summary>
        /// Created by user foreign key
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Updated by user foreign key
        /// </summary>
        [Export(IsAllowExport = false)]
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Is deleted boolean key for soft delete
        /// </summary>
        [Export(IsAllowExport = false)]
        public bool IsDeleted { get; set; }
    }
}
