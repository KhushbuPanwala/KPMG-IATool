using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Repository.Repository.Common
{
    public class GlobalRepository : IGlobalRepository
    {

        #region Pagination Skip record count

        /// <summary>
        /// Get count for pagination skip records
        /// </summary>
        /// <param name="pageNo">Current page no</param>
        /// <param name="itemsPerPage">Items per page</param>
        /// <returns>Returns number of records to be skipped</returns>
        public int GetSkipRecordsCount(int pageNo, int itemsPerPage)
        {
            return (pageNo - 1) * itemsPerPage;
        }

        #endregion

        #region Bulk Upload
        /// <summary>
        /// Set Table column name
        /// </summary>
        /// <typeparam name="T">Generic model </typeparam>
        /// <param name="table">Datatable table</param>
        /// <returns>Updated table</returns>
        public DataTable SetTableColumnName<T>(DataTable table)
        {
            Type model = typeof(T);
            int i = 0;
            foreach (PropertyInfo pro in model.GetProperties())
            {
                table.Columns[i].ColumnName = pro.Name;
                i++;
                if (i < model.GetProperties().Length)
                {
                    continue;
                }
            }
            return table;
        }

        /// <summary>
        /// Convert datatable to specific model table
        /// </summary>
        /// <typeparam name="T">Generic Model</typeparam>
        /// <param name="dt">Datatable table</param>
        /// <returns>List of generic data </returns>
        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            dt.Rows.RemoveAt(0);
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// Get row data in list 
        /// </summary>
        /// <typeparam name="T">Generic model</typeparam>
        /// <param name="dr">Datarow </param>
        /// <returns>Row data</returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type model = typeof(T);
            T obj = Activator.CreateInstance<T>();

            int i = 0;
            foreach (PropertyInfo pro in model.GetProperties())
            {
                if (dr.Table.Columns[i].ColumnName == pro.Name)
                {
                    pro.SetValue(obj, string.IsNullOrEmpty(dr[dr.Table.Columns[i].ColumnName].ToString()) ? string.Empty : dr[dr.Table.Columns[i].ColumnName].ToString(), null);
                    i++;
                    if (i < model.GetProperties().Length)
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
        #endregion

        /// <summary>
        /// Set Speical charecters
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>without html string value</returns>
        public string SetSpecialCharecters(string value)
        {

            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.AmpSymbole, string.Empty);
            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.AmpersonSymbole, string.Empty);
            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.HTMLSpace, string.Empty);
            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.LessThan, StringConstant.LessThanSymbole);
            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.GreaterThan, StringConstant.GreaterThanSymbole);
            value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(StringConstant.DoubleQuatos, StringConstant.DoubleQuatosSymbole);
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(StringConstant.RemoveHtmlTagsRegex);
            value = string.IsNullOrEmpty(value) ? string.Empty : WebUtility.HtmlDecode(rx.Replace(value, string.Empty));
            return value;
        }



        /// <summary>
        /// check file size is valid or not
        /// </summary>
        /// <param name="documentFiles">list of files</param>
        /// <returns>file size is valid or not</returns>
        public bool CheckFileSize(List<IFormFile> documentFiles)
        {
            bool isValid = false;
            for (int i = 0; i < documentFiles.Count; i++)
            {
                var fileSize = documentFiles[i].Length;
                if ((fileSize / 1048576.0) >= StringConstant.FileSize)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        /// <summary>
        /// check file is valid or not
        /// </summary>
        /// <param name="documentFiles">list of files</param>
        /// <returns>file is valid or not</returns>
        public bool CheckFileExtention(List<IFormFile> documentFiles)
        {
            bool isValid = false;
            for (int i = 0; i < documentFiles.Count; i++)
            {
                string extention = documentFiles[i].FileName.Split('.')[documentFiles[i].FileName.Split('.').Length - 1];
                isValid = (extention == StringConstant.EXEFile);
            }

            return isValid;
        }
    }
}
