using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemPurview : EntitySys
    {
        public EntitySystemPurview(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemPurviewPara : DBCulture
        {
            public SystemPurviewPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, PURVIEW_ID,
                SYS_NM, PURVIEW_NM
            }

            public DBVarChar SysID;
            public DBVarChar PurviewID;
        }

        public class SystemPurview : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                PURVIEW_ID, PURVIEW_NM, SORT_ORDER, REMARK,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemPurview> SelectSystemPurviewList(SystemPurviewPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.PurviewID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND P.PURVIEW_ID={PURVIEW_ID} ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT P.SYS_ID, dbo.FN_GET_NMID(P.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , P.PURVIEW_ID, dbo.FN_GET_NMID(P.PURVIEW_ID, P.{PURVIEW_NM}) AS PURVIEW_NM ", Environment.NewLine,
                "     , P.SORT_ORDER, P.REMARK ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(P.UPD_USER_ID) AS UPD_USER_NM, P.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_PURVIEW P ", Environment.NewLine,
                "INNER JOIN SYS_SYSTEM_MAIN M ON P.SYS_ID = M.SYS_ID ", Environment.NewLine,
                "WHERE P.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY M.SORT_ORDER, P.SORT_ORDER ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemPurviewPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemPurviewPara.ParaField.PURVIEW_ID.ToString(), Value = para.PurviewID });
            dbParameters.Add(new DBParameter { Name = SystemPurviewPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemPurviewPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemPurviewPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemPurviewPara.ParaField.PURVIEW_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemPurview> systemPurviewList = new List<SystemPurview>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemPurview systemPurview = new SystemPurview()
                    {
                        SysID = new DBVarChar(dataRow[SystemPurview.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemPurview.DataField.SYS_NM.ToString()]),

                        PurviewID = new DBVarChar(dataRow[SystemPurview.DataField.PURVIEW_ID.ToString()]),
                        PurviewNM = new DBNVarChar(dataRow[SystemPurview.DataField.PURVIEW_NM.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemPurview.DataField.SORT_ORDER.ToString()]),
                        Remark = new DBNVarChar(dataRow[SystemPurview.DataField.REMARK.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemPurview.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemPurview.DataField.UPD_DT.ToString()]),
                    };
                    systemPurviewList.Add(systemPurview);
                }
                return systemPurviewList;
            }
            return null;
        }
    }
}