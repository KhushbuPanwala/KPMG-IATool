using InternalAuditSystem.DomailModel.DataRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.DynamicTableRepository
{
    public class DynamicTableRepository : IDynamicTableRepository
    {
        private readonly IDataRepository _dataRepository;
        public DynamicTableRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        #region Public Methods

        /// <summary>
        /// Add default json document
        /// </summary>
        /// <returns>Default json document</returns>
        public JsonDocument AddDefaultJsonDocument() {
            var jsonDoc = "{\"tableId\":\"" + 
                           Guid.NewGuid().ToString() +
                           "\",\"columnNames\":[\"rowId\",\"Column Name\",\"Column Name\"],\"data\":[{\"RowData\":[\"" +
                           Guid.NewGuid().ToString() +
                           "\",\"Data1\",\"Data2\"]},{\"RowData\":[\"" +
                           Guid.NewGuid().ToString() +
                           "\",\"Data1\",\"Data2\"]}]}";
            var parsedDocument = JsonDocument.Parse(jsonDoc);
            return parsedDocument;
        }

        /// <summary>
        /// Add column in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        public string AddColumn(JsonElement rootElement)
        {
            string resultJson = String.Empty;

            using (MemoryStream memoryStream1 = new MemoryStream())
            {
                using (Utf8JsonWriter utf8JsonWriter1 = new Utf8JsonWriter(memoryStream1))
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(rootElement)))
                    {
                        utf8JsonWriter1.WriteStartObject();

                        foreach (var element in jsonDocument.RootElement.EnumerateObject())
                        {
                            if (element.Name == "columnNames")
                            {
                                // Staring new object
                                utf8JsonWriter1.WriteStartArray(element.Name);

                                // Copying existing values
                                foreach (var columnData in element.Value.EnumerateArray())
                                {
                                    utf8JsonWriter1.WriteStringValue(columnData.GetString());
                                }

                                // Adding new column in columnNames json array
                                utf8JsonWriter1.WriteStringValue("Column Name");

                                utf8JsonWriter1.WriteEndArray();
                            }
                            else if (element.Name == "data")
                            {
                                utf8JsonWriter1.WriteStartArray(element.Name);

                                // Copying existing values
                                foreach (var data in element.Value.EnumerateArray())
                                {
                                    utf8JsonWriter1.WriteStartObject();
                                    utf8JsonWriter1.WriteStartArray("RowData");
                                    foreach (var rowDataArray in data.GetProperty("RowData").EnumerateArray())
                                    {
                                        utf8JsonWriter1.WriteStringValue(rowDataArray.GetString());
                                    }
                                    utf8JsonWriter1.WriteStringValue("data");
                                    utf8JsonWriter1.WriteEndArray();
                                    utf8JsonWriter1.WriteEndObject();
                                }

                                utf8JsonWriter1.WriteEndArray();
                            }
                            else
                            {
                                element.WriteTo(utf8JsonWriter1);
                            }
                        }
                        utf8JsonWriter1.WriteEndObject();
                    }
                }
                resultJson = Encoding.UTF8.GetString(memoryStream1.ToArray());
            }
            return resultJson;
        }

        /// <summary>
        /// Add row in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        public string AddRow(JsonElement rootElement)
        {
            string resultJson = String.Empty;

            using (MemoryStream memoryStream1 = new MemoryStream())
            {
                using (Utf8JsonWriter utf8JsonWriter1 = new Utf8JsonWriter(memoryStream1))
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(rootElement)))
                    {
                        utf8JsonWriter1.WriteStartObject();

                        foreach (var element in jsonDocument.RootElement.EnumerateObject())
                        {
                            if (element.Name == "data")
                            {
                                utf8JsonWriter1.WriteStartArray(element.Name);
                                JsonElement rowDataForCreatingNewRow = new JsonElement();
                                // Copying existing values
                                foreach (var rowData in element.Value.EnumerateArray())
                                {
                                    utf8JsonWriter1.WriteStartObject();
                                    utf8JsonWriter1.WriteStartArray("RowData");
                                    foreach (var rowDataArray in rowData.GetProperty("RowData").EnumerateArray())
                                    {
                                        utf8JsonWriter1.WriteStringValue(rowDataArray.GetString());
                                    }
                                    utf8JsonWriter1.WriteEndArray();
                                    utf8JsonWriter1.WriteEndObject();
                                    rowDataForCreatingNewRow = rowData;
                                }

                                // Add new row in the end
                                utf8JsonWriter1.WriteStartObject();
                                utf8JsonWriter1.WriteStartArray("RowData");
                                var count = 0;
                                foreach (var rowDataArray in rowDataForCreatingNewRow.GetProperty("RowData").EnumerateArray())
                                {
                                    if (count == 0)
                                    {
                                        utf8JsonWriter1.WriteStringValue(Guid.NewGuid().ToString());
                                    }
                                    else
                                    {
                                        utf8JsonWriter1.WriteStringValue("data");
                                    }
                                    count++;
                                }
                                utf8JsonWriter1.WriteEndArray();
                                utf8JsonWriter1.WriteEndObject();

                                utf8JsonWriter1.WriteEndArray();
                            }
                            else
                            {
                                element.WriteTo(utf8JsonWriter1);
                            }
                        }

                        utf8JsonWriter1.WriteEndObject();
                    }
                }
                resultJson = Encoding.UTF8.GetString(memoryStream1.ToArray());
            }
            return resultJson;
        }

        /// <summary>
        /// Delete row in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <param name="rowId">Row id of row which is to be deleted</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        public string DeleteRow(JsonElement rootElement, string rowId) {
            string resultJson = String.Empty;

            using (MemoryStream memoryStream1 = new MemoryStream())
            {
                using (Utf8JsonWriter utf8JsonWriter1 = new Utf8JsonWriter(memoryStream1))
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(rootElement)))
                    {
                        utf8JsonWriter1.WriteStartObject();

                        foreach (var element in jsonDocument.RootElement.EnumerateObject())
                        {
                            if (element.Name == "data")
                            {
                                utf8JsonWriter1.WriteStartArray(element.Name);
                                JsonElement rowDataForCreatingNewRow = new JsonElement();
                                // Copying existing values
                                foreach (var rowData in element.Value.EnumerateArray())
                                {
                                    var rowDataToBeDeleted = rowData.GetProperty("RowData").EnumerateArray().ToList();
                                    if (!(rowDataToBeDeleted[0].GetString() == rowId))
                                    {
                                        utf8JsonWriter1.WriteStartObject();
                                        utf8JsonWriter1.WriteStartArray("RowData");
                                        foreach (var rowDataArray in rowData.GetProperty("RowData").EnumerateArray())
                                        {
                                            utf8JsonWriter1.WriteStringValue(rowDataArray.GetString());
                                        }
                                        utf8JsonWriter1.WriteEndArray();
                                        utf8JsonWriter1.WriteEndObject();
                                        rowDataForCreatingNewRow = rowData;
                                    }
                                }
                                utf8JsonWriter1.WriteEndArray();
                            }
                            else
                            {
                                element.WriteTo(utf8JsonWriter1);
                            }
                        }

                        utf8JsonWriter1.WriteEndObject();
                    }
                }
                resultJson = Encoding.UTF8.GetString(memoryStream1.ToArray());
            }
            return resultJson;
        }

        /// <summary>
        /// Delete column in dynamic table
        /// </summary>
        /// <param name="rootElement">Root element of table</param>
        /// <param name="columnPosition">Position of column to be deleted</param>
        /// <returns>UTF8 encoded memory stream string</returns>
        public string DeleteColumn(JsonElement rootElement, int columnPosition) {

            string resultJson = String.Empty;

            using (MemoryStream memoryStream1 = new MemoryStream())
            {
                using (Utf8JsonWriter utf8JsonWriter1 = new Utf8JsonWriter(memoryStream1))
                {
                    using (JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(rootElement)))
                    {
                        utf8JsonWriter1.WriteStartObject();

                        foreach (var element in jsonDocument.RootElement.EnumerateObject())
                        {
                            if (element.Name == "columnNames")
                            {
                                // Staring new object
                                utf8JsonWriter1.WriteStartArray(element.Name);
                                var columnToBeDeleted = element.Value.EnumerateArray().ToList()[columnPosition];
                                foreach (var columnData in element.Value.EnumerateArray())
                                {
                                    if (!columnData.Equals(columnToBeDeleted))
                                    {
                                        utf8JsonWriter1.WriteStringValue(columnData.GetString());
                                    }
                                }
                                utf8JsonWriter1.WriteEndArray();
                            }
                            else if (element.Name == "data")
                            {
                                utf8JsonWriter1.WriteStartArray(element.Name);

                                // Copying existing values
                                foreach (var rowData in element.Value.EnumerateArray())
                                {
                                    var rowDataList = rowData.GetProperty("RowData").EnumerateArray().ToList();
                                    utf8JsonWriter1.WriteStartObject();
                                    utf8JsonWriter1.WriteStartArray("RowData");
                                    var countPosition = 0;
                                    foreach (var rowDataArray in rowData.GetProperty("RowData").EnumerateArray())
                                    {
                                        if (countPosition != columnPosition)
                                        {
                                            utf8JsonWriter1.WriteStringValue(rowDataArray.GetString());
                                        }
                                        countPosition++;
                                    }
                                    utf8JsonWriter1.WriteEndArray();
                                    utf8JsonWriter1.WriteEndObject();
                                }
                                utf8JsonWriter1.WriteEndArray();
                            }
                            else
                            {
                                element.WriteTo(utf8JsonWriter1);
                            }
                        }

                        utf8JsonWriter1.WriteEndObject();
                    }
                }
                resultJson = Encoding.UTF8.GetString(memoryStream1.ToArray());
            }
            return resultJson;
        }

        #endregion
    }
}
