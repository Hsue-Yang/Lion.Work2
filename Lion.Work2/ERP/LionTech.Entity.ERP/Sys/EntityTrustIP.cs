using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityTrustIP : EntitySys
    {
        public EntityTrustIP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class TrustIPPara
        {
            public enum ParaField
            {
                IP_BEGIN, IP_END,
                COM_ID,
                TRUST_STATUS, TRUST_TYPE, SOURCE_TYPE,
                REMARK,
                CULTURE_ID
            }

            public DBObject IPBegin;
            public DBObject IPEnd;
            public DBVarChar ComID;

            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBChar SourceType;

            public DBObject Remark;

            public DBVarChar CultureID;
        }

        public class TrustIP : DBTableRow
        {
            public enum DataField
            {
                IP_BEGIN, IP_END,
                COM_ID, COM_NM,
                TRUST_STATUS, TRUST_TYPE, TRUST_TYPE_NM, SOURCE_TYPE, SOURCE_TYPE_NM,
                REMARK, SORT_ORDER,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar IPBegin;
            public DBVarChar IPEnd;

            public DBVarChar ComID;
            public DBNVarChar ComNM;

            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBNVarChar TrustTypeNM;
            public DBChar SourceType;
            public DBNVarChar SourceTypeNM;

            public DBNVarChar Remark;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<TrustIP> SelectTrustIPList(TrustIPPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.IPBegin.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.IP_BEGIN LIKE N'%{IP_BEGIN}%' ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.IPEnd.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.IP_END LIKE N'%{IP_END}%' ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.ComID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.COM_ID={COM_ID} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.TrustStatus.GetValue()) && para.TrustStatus.GetValue() == EnumYN.Y.ToString())
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.TRUST_STATUS='Y' ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.TrustType.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.TRUST_TYPE={TRUST_TYPE} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.SourceType.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.SOURCE_TYPE={SOURCE_TYPE} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.Remark.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND T.REMARK LIKE N'%{REMARK}%' ", Environment.NewLine });
            }

            if (commandWhere != string.Empty)
            {
                commandWhere = string.Concat(new object[] { "WHERE ", commandWhere.Substring(5) });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT T.IP_BEGIN, T.IP_END ", Environment.NewLine,
                "     , T.COM_ID, (CASE WHEN T.COM_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(T.COM_ID, R.COM_NM) END) AS COM_NM ", Environment.NewLine,
                "     , T.TRUST_STATUS ", Environment.NewLine,
                "     , T.TRUST_TYPE, dbo.FN_GET_NMID(T.TRUST_TYPE, dbo.FN_GET_CM_NM('0031',T.TRUST_TYPE,{CULTURE_ID})) AS TRUST_TYPE_NM ", Environment.NewLine,
                "     , T.SOURCE_TYPE, dbo.FN_GET_NMID(T.SOURCE_TYPE, dbo.FN_GET_CM_NM('0032',T.SOURCE_TYPE,{CULTURE_ID})) AS SOURCE_TYPE_NM ", Environment.NewLine,
                "     , T.REMARK, T.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(T.UPD_USER_ID) AS UPD_USER_NM, T.UPD_DT ", Environment.NewLine,
                "FROM SYS_TRUST_IP T ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_COM R ON T.COM_ID=R.COM_ID ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY T.SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.IP_BEGIN.ToString(), Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.IP_END.ToString(), Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.COM_ID.ToString(), Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.TRUST_STATUS.ToString(), Value = para.TrustStatus });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.TRUST_TYPE.ToString(), Value = para.TrustType });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.SOURCE_TYPE.ToString(), Value = para.SourceType });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.REMARK.ToString(), Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.CULTURE_ID, Value = para.CultureID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<TrustIP> trustIPList = new List<TrustIP>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TrustIP trustIP = new TrustIP()
                    {
                        IPBegin = new DBVarChar(dataRow[TrustIP.DataField.IP_BEGIN.ToString()]),
                        IPEnd = new DBVarChar(dataRow[TrustIP.DataField.IP_END.ToString()]),

                        ComID = new DBVarChar(dataRow[TrustIP.DataField.COM_ID.ToString()]),
                        ComNM = new DBNVarChar(dataRow[TrustIP.DataField.COM_NM.ToString()]),

                        TrustStatus = new DBChar(dataRow[TrustIP.DataField.TRUST_STATUS.ToString()]),
                        TrustType = new DBChar(dataRow[TrustIP.DataField.TRUST_TYPE.ToString()]),
                        TrustTypeNM = new DBNVarChar(dataRow[TrustIP.DataField.TRUST_TYPE_NM.ToString()]),
                        SourceType = new DBChar(dataRow[TrustIP.DataField.SOURCE_TYPE.ToString()]),
                        SourceTypeNM = new DBNVarChar(dataRow[TrustIP.DataField.SOURCE_TYPE_NM.ToString()]),

                        Remark = new DBNVarChar(dataRow[TrustIP.DataField.REMARK.ToString()]),
                        SortOrder = new DBVarChar(dataRow[TrustIP.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[TrustIP.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[TrustIP.DataField.UPD_DT.ToString()])
                    };
                    trustIPList.Add(trustIP);
                }
                return trustIPList;
            }
            return null;
        }
    }
}