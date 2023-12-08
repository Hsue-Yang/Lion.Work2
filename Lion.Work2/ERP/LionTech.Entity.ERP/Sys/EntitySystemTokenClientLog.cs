using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemTokenClientLog:EntitySys
    {
        public EntitySystemTokenClientLog(string connectionstring, string providerName)
            : base(connectionstring, providerName)
        { 
            
        }
        public class TokenClientLogPara :DBCulture
        {
            public TokenClientLogPara(string culture) 
                : base(culture) { }

            public enum ParaField
            {
                USER_ID, GENERATE_DT_BEGIN, GENERATE_DT_END, TKN_NO, DEVICE_ID,SYS_ID
            }

            public DBVarChar UserID;
            public DBChar GenerateDTBegin;
            public DBChar GenerateDTEnd;
            public DBChar TokenNo;
            public DBVarChar DeviceID;
            public DBVarChar SysID;
        }
        public class TokenClientLog : DBTableRow {
            public enum DataField {
                TKN_NO, SYS_ID, DEVICE_ID, PARA_K, PARA_C,
                USER_ID, IS_LOGIN, GENERATE_DT, LAST_CONNECT_DT, UPD_USER_ID, UPD_DT, UPD_USER_NM
            }
            public DBChar TokenNO;
            public DBVarChar SysID;
            public DBVarChar DeviceID;
            public DBChar ParaK;
            public DBVarChar ParaC;
            public DBVarChar UserID;
            public DBChar IsLogin;
            public DBChar GenerateDT;
            public DBChar LastConnectDT;
            public DBVarChar UpdateUserID;
            public DBDateTime UpdateDT;
        }
        public List<TokenClientLog> SelectTokenClientLog(TokenClientLogPara para)
        {
            string commandWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(para.UserID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere," AND T.USER_ID=UPPER({USER_ID}) ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.TokenNo.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere," AND T.TKN_NO=UPPER({TKN_NO}) ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.DeviceID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere," AND T.DEVICE_ID={DEVICE_ID} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.SysID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, " AND T.SYS_ID=UPPER({SYS_ID}) ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT T.*, dbo.FN_GET_USER_NM(T.USER_ID) AS UPD_USER_NM ", Environment.NewLine,
                "FROM TKN_CLIENT T ", Environment.NewLine,
                "WHERE T.GENERATE_DT >={GENERATE_DT_BEGIN} ", Environment.NewLine,
                "  AND T.GENERATE_DT <={GENERATE_DT_END} ", Environment.NewLine,
                commandWhere, 
                "ORDER BY T.UPD_DT DESC", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.GENERATE_DT_BEGIN.ToString(), Value = para.GenerateDTBegin });
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.GENERATE_DT_END.ToString(), Value = para.GenerateDTEnd });
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.TKN_NO.ToString(), Value = para.TokenNo });
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.DEVICE_ID.ToString(), Value = para.DeviceID });
            dbParameters.Add(new DBParameter { Name = TokenClientLogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<TokenClientLog> tokenClientLogList = new List<TokenClientLog>();
                foreach (DataRow dataRow in dataTable.Rows) {
                    TokenClientLog tokenClientLog = new TokenClientLog() {
                        TokenNO = new DBChar(dataRow[TokenClientLog.DataField.TKN_NO.ToString()]),
                        SysID = new DBVarChar(dataRow[TokenClientLog.DataField.SYS_ID.ToString()]),
                        DeviceID = new DBVarChar(dataRow[TokenClientLog.DataField.DEVICE_ID.ToString()]),
                        ParaK = new DBChar(dataRow[TokenClientLog.DataField.PARA_K.ToString()]),
                        ParaC = new DBVarChar(dataRow[TokenClientLog.DataField.PARA_C.ToString()]),
                        UserID = new DBVarChar(dataRow[TokenClientLog.DataField.UPD_USER_NM.ToString()]),
                        IsLogin = new DBChar(dataRow[TokenClientLog.DataField.IS_LOGIN.ToString()]),
                        GenerateDT = new DBChar(dataRow[TokenClientLog.DataField.GENERATE_DT.ToString()]),
                        LastConnectDT = new DBChar(dataRow[TokenClientLog.DataField.LAST_CONNECT_DT.ToString()]),
                        UpdateUserID = new DBVarChar(dataRow[TokenClientLog.DataField.UPD_USER_ID.ToString()]),
                        UpdateDT = new DBDateTime(dataRow[TokenClientLog.DataField.UPD_DT.ToString()])
                    };
                    tokenClientLogList.Add(tokenClientLog);
                }
                return tokenClientLogList;
            }
            return null;
        }
    }
}
