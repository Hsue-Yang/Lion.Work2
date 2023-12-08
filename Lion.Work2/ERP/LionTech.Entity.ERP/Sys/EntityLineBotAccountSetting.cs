using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityLineBotAccountSetting : EntitySys
    {
        public EntityLineBotAccountSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得LineBot好友清單列表 -
        public class LineBotAccountSettingPara : DBCulture
        {
            public LineBotAccountSettingPara(string cultureID) : base(cultureID)
            {
                
            }

            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_NM
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
        }

        public class LineBotAccountSetting : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBNVarChar LineNM;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<LineBotAccountSetting> SelectLineBotAccountSettingList(LineBotAccountSettingPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT SYS_ID AS SysID",
                "     , LINE_ID AS LineID",
                "     , {LINE_NM} AS LineNM",
                "	  , IS_DISABLE AS IsDisable",
                "	  , SORT_ORDER AS SortOrder",
                "	  , dbo.FN_GET_USER_NM(UPD_USER_ID) AS UpdUserNM",
                "	  , UPD_DT AS UpdDT",
                "  FROM SYS_SYSTEM_LINE",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            if (para.LineID.IsNull() == false)
            {
                commandText.AppendLine(" AND LINE_ID = {LINE_ID}");
                dbParameters.Add(new DBParameter { Name = LineBotAccountSettingPara.ParaField.LINE_ID, Value = para.LineID });
            }

            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingPara.ParaField.LINE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(LineBotAccountSettingPara.ParaField.LINE_NM.ToString())) });
            return GetEntityList<LineBotAccountSetting>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}