using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserConnect : EntitySys
    {
        public EntityUserConnect(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserConnectPara
        {
            public enum ParaField
            {
                CONNECT_DT_BEGIN, CONNECT_DT_END
            }

            public DBDateTime ConnectDTBegin;
            public DBDateTime ConnectDTEnd;
        }

        public class UserConnect : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, LAST_CONNECT_DT, SESSION_ID, CUST_LOGOUT, IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBDateTime LastConnectDT;
            public DBChar SessionID;
            public DBChar CustLogout;
            public DBVarChar IPAddress;
        }

        public List<UserConnect> SelectUserConnectList(UserConnectPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT C.USER_ID, dbo.FN_GET_USER_NM(C.USER_ID) AS USER_NM ", Environment.NewLine,
                "     , C.LAST_CONNECT_DT, C.CUST_LOGOUT, C.IP_ADDRESS ", Environment.NewLine,
                "FROM SYS_USER_CONNECT C ", Environment.NewLine,
                "WHERE C.LAST_CONNECT_DT>={CONNECT_DT_BEGIN} ", Environment.NewLine,
                "  AND C.LAST_CONNECT_DT<={CONNECT_DT_END} ", Environment.NewLine,
                "ORDER BY C.LAST_CONNECT_DT DESC, C.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.CONNECT_DT_BEGIN.ToString(), Value = para.ConnectDTBegin });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.CONNECT_DT_END.ToString(), Value = para.ConnectDTEnd });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserConnect> userConnectList = new List<UserConnect>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserConnect userConnect = new UserConnect()
                    {
                        UserID = new DBVarChar(dataRow[UserConnect.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserConnect.DataField.USER_NM.ToString()]),
                        LastConnectDT = new DBDateTime(dataRow[UserConnect.DataField.LAST_CONNECT_DT.ToString()]),
                        CustLogout = new DBChar(dataRow[UserConnect.DataField.CUST_LOGOUT.ToString()]),
                        IPAddress = new DBVarChar(dataRow[UserConnect.DataField.IP_ADDRESS.ToString()])
                    };
                    userConnectList.Add(userConnect);
                }
                return userConnectList;
            }
            return null;
        }
    }
}