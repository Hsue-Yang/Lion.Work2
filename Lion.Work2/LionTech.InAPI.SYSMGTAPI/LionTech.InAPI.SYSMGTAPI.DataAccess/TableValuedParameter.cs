using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess
{
    public class TableValuedParameter : SqlMapper.ICustomQueryParameter
    {
        private DataTable _dt;
        private string _type;

        public TableValuedParameter(DataTable dt)
        {
            _type = dt.TableName;
            _dt = dt;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            var para = (SqlParameter)command.CreateParameter();
            para.ParameterName = name;
            para.SqlDbType = SqlDbType.Structured;
            para.Value = _dt;
            para.TypeName = _type;
            command.Parameters.Add(para);
        }

        public static DataTable GetDataTable<T>(T obj, string tableName, List<string> ignoreColumnNames = null)
        {
            DataTable dt = new DataTable(tableName);

            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (ignoreColumnNames == null)
                {
                    dt.Columns.Add(pi.Name);
                }
                else if (ignoreColumnNames.Any() && !ignoreColumnNames.Contains(pi.Name))
                {
                    dt.Columns.Add(pi.Name);
                }
            }

            if (obj != null)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    var data = obj.GetType().GetProperty(dc.ColumnName).GetValue(obj);
                    dr[dc] = data;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable GetDataTable<T>(List<T> list, string tableName, List<string> ignoreColumnNames = null)
        {
            DataTable dt = new DataTable(tableName);

            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (ignoreColumnNames == null)
                {
                    dt.Columns.Add(pi.Name);
                }
                else if (ignoreColumnNames.Any() && !ignoreColumnNames.Contains(pi.Name))
                {
                    dt.Columns.Add(pi.Name);
                }
            }

            if (list != null && list.Any())
            {
         
                foreach (var item in list)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var data = item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);
                        dr[dc] = data;
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}
