using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace LionTech.InAPI.SYSMGTAPI.Model.Sys
{
    public class SystemFunDetailModel
    {
        public string SysID { get; set; }
        public string SubSysID { get; set; }
        public string PurviewID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string FunNMZHTW { get; set; }
        public string FunNMZHCN { get; set; }
        public string FunNMENUS { get; set; }
        public string FunNMTHTH { get; set; }
        public string FunNMJAJP { get; set; }
        public string FunNMKOKR { get; set; }
        public string FunType { get; set; }
        public string IsOutside { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public IEnumerable<string> RoleIDs { get; set; }
        public IEnumerable<SystemMenuFun> SystemMenuFuns { get; set; }

        public DataTable GetDataTable()
        {
            DataTable tb = new DataTable("type_SystemFun");
            PropertyInfo[] props = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(w => w.PropertyType.IsGenericType == false).ToArray();
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            var values = new object[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(this, null);
            }

            tb.Rows.Add(values);

            return tb;
        }

        public DataTable GetSystemRoleFunDataTable()
        {
            DataTable tb = new DataTable("type_SystemRoleFun");
            tb.Columns.Add(nameof(SysID));
            tb.Columns.Add("RoleID");
            tb.Columns.Add(nameof(FunControllerID));
            tb.Columns.Add(nameof(FunActionName));
            tb.Columns.Add(nameof(UpdUserID));

            if (RoleIDs != null)
            {
                foreach (var roleID in RoleIDs)
                {
                    var dr = tb.NewRow();
                    dr[nameof(SysID)] = SysID;
                    dr["RoleID"] = roleID;
                    dr[nameof(FunControllerID)] = FunControllerID;
                    dr[nameof(FunActionName)] = FunActionName;
                    dr[nameof(UpdUserID)] = UpdUserID;
                    tb.Rows.Add(dr);
                }
            }

            return tb;
        }

        public DataTable GetSystemMenuFunDataTable()
        {
            DataTable tb = new DataTable("type_SystemMenuFun");
            tb.Columns.Add(nameof(SysID));
            tb.Columns.Add("FunMenuSysID");
            tb.Columns.Add("FunMenu");
            tb.Columns.Add(nameof(FunControllerID));
            tb.Columns.Add(nameof(FunActionName));
            tb.Columns.Add("FunMenuXAxis");
            tb.Columns.Add("FunMenuYAxis");
            tb.Columns.Add(nameof(UpdUserID));

            if (SystemMenuFuns != null)
            {
                foreach (var item in SystemMenuFuns)
                {
                    var dr = tb.NewRow();
                    dr[nameof(SysID)] = SysID;
                    dr["FunMenuSysID"] = item.FunMenuSysID;
                    dr["FunMenu"] = item.FunMenu;
                    dr[nameof(FunControllerID)] = FunControllerID;
                    dr[nameof(FunActionName)] = FunActionName;
                    dr["FunMenuXAxis"] = item.FunMenuXAxis;
                    dr["FunMenuYAxis"] = item.FunMenuYAxis;
                    dr[nameof(UpdUserID)] = UpdUserID;
                    tb.Rows.Add(dr);
                }
            }

            return tb;
        }

        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }

                return Nullable.GetUnderlyingType(t);
            }

            return t;
        }
    }
}