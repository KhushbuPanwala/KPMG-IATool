using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.DynamicTableRepository
{
    public interface IDynamicTableRepository
    {
        /// <summary>
        /// Add default json document
        /// </summary>
        /// <returns>Default json document</returns>
        JsonDocument AddDefaultJsonDocument();

        /// <summary>
        /// Add column in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        string AddColumn(JsonElement rootElement);

        /// <summary>
        /// Add row in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        string AddRow(JsonElement rootElement);

        /// <summary>
        /// Delete row in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        string DeleteRow(JsonElement rootElement, string rowId);

        /// <summary>
        /// Delete column in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        string DeleteColumn(JsonElement rootElement, int columnPosition);
    }
}
