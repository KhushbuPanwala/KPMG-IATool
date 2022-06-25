using InternalAuditSystem.Repository.ApplicationClasses.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Repository.ApplicationClasses
{
    public class Pagination<T>
    {
        /// <summary>
        /// Total no. of records
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Current page no.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// No. of items per page 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Global EntityID
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Search text
        /// </summary>
        public string searchText { get; set; }

        /// <summary>
        /// List of data 
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Current logged in user details 
        /// </summary>
        public LoggedInUserDetails CurrentUserDetails { get; set; }
    }
}
