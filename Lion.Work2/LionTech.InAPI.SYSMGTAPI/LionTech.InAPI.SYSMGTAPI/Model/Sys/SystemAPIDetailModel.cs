using System.Collections.Generic;
using System.Data;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Model.Sys
{
    public class SystemAPIDetailModel
    {
        public string SysID { get; set; }
        public string APIGroupID { get; set; }
        public string APIFunID { get; set; }
        public string APINMZHTW { get; set; }
        public string APINMZHCN { get; set; }
        public string APINMENUS { get; set; }
        public string APINMTHTH { get; set; }
        public string APINMJAJP { get; set; }
        public string APINMKOKR { get; set; }
        public string APIPara { get; set; }
        public string APIReturn { get; set; }
        public string APIParaDesc { get; set; }
        public string APIReturnContent { get; set; }
        public string IsOutside { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public IEnumerable<string> RoleIDs { get; set; }

        public SystemAPIMain GetSystemAPIDetail()
        {
            SystemAPIMain systemAPI = new SystemAPIMain
            {
                SysID = SysID,
                APIGroupID = APIGroupID,
                APIFunID = APIFunID,
                APINMZHTW = APINMZHTW,
                APINMZHCN = APINMZHCN,
                APINMENUS = APINMENUS,
                APINMTHTH = APINMTHTH,
                APINMJAJP = APINMJAJP,
                APINMKOKR = APINMKOKR,
                APIPara = APIPara,
                APIReturn = APIReturn,
                APIParaDesc = APIParaDesc,
                APIReturnContent = APIReturnContent,
                IsOutside = IsOutside,
                IsDisable = IsDisable,
                SortOrder = SortOrder,
                UpdUserID = UpdUserID
            };

            return systemAPI;
        }

        public DataTable GetSystemRoleAPIDataTable()
        {
            DataTable tb = new DataTable(EnumUserDefinedTableType.type_SystemRoleAPI.ToString());
            tb.Columns.Add(nameof(SysID));
            tb.Columns.Add("RoleID");
            tb.Columns.Add(nameof(APIGroupID));
            tb.Columns.Add(nameof(APIFunID));
            tb.Columns.Add(nameof(UpdUserID));

            if (RoleIDs != null)
            {
                foreach (var roleID in RoleIDs)
                {
                    var dr = tb.NewRow();
                    dr[nameof(SysID)] = SysID;
                    dr["RoleID"] = roleID;
                    dr[nameof(APIGroupID)] = APIGroupID;
                    dr[nameof(APIFunID)] = APIFunID;
                    dr[nameof(UpdUserID)] = UpdUserID;
                    tb.Rows.Add(dr);
                }
            }
            return tb;
        }
    }
}