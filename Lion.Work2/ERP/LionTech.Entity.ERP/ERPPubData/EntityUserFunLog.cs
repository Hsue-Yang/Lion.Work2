using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.ERPPubData
{
    public class EntityUserFunLog : DBEntity
    {
#if !NET461
        public EntityUserFunLog(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityUserFunLog(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserFunLogPara : DBTableRow
        {
            public enum ParaField
            {
                USER_ID,
                SERVER_NAME,
                ASP_ID,
                CONTROLLER_ID,
                ACTION_NM,
                URL,
                UPD_DT
            }

            public DBVarChar USER_ID;
            public DBNVarChar SERVER_NAME;
            public DBVarChar ASP_ID;
            public DBNVarChar CONTROLLER_ID;
            public DBNVarChar ACTION_NM;
            public DBNVarChar URL;
            public DBDateTime UPD_DT;
        }

        #region - 編輯使用者功能紀錄 -
        /// <summary>
        /// 編輯使用者功能紀錄
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public void EditUserFunLog(UserFunLogPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        "INSERT INTO dbo.LOG_ERP_USER_FUN (",
                        "       USER_ID",
                        "     , SERVER_NAME",
                        "     , ASP_ID",
                        "     , CONTROLLER_ID",
                        "     , ACTION_NM",
                        "     , URL",
                        "     , UPD_DT",
                        ") VALUES (",
                        "       {USER_ID}",
                        "     , {SERVER_NAME}",
                        "     , {ASP_ID}",
                        "     , {CONTROLLER_ID}",
                        "     , {ACTION_NM}",
                        "     , {URL}",
                        "     , GETDATE()",
                        ")",
                        Environment.NewLine
                    }));

            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.USER_ID, Value = para.USER_ID });
            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.SERVER_NAME, Value = para.SERVER_NAME });
            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.ASP_ID, Value = para.ASP_ID });
            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.CONTROLLER_ID, Value = para.CONTROLLER_ID });
            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.ACTION_NM, Value = para.ACTION_NM });
            dbParameters.Add(new DBParameter { Name = UserFunLogPara.ParaField.URL, Value = para.URL });
            ExecuteNonQuery(commandText.ToString(), dbParameters);
        }
        #endregion

    }
}
