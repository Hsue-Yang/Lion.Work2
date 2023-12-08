using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserPurview : EntitySys
    {
        public EntityUserPurview(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得使用者資料權限清單 -
        public class SysUserPurviewPara : DBCulture
        {
            public SysUserPurviewPara(string cultureID)
                : base(cultureID)
            {
                
            }

            public enum ParaField
            {
                USER_ID,
                UPD_USER_ID,
                SYS_NM,
                PURVIEW_NM
            }

            public DBVarChar UserID;
            public DBVarChar UpdUserID;
        }

        public class SysUserPurview : DBTableRow
        {
            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                PURVIEW_ID,
                PURVIEW_NM,
                HAS_DATAPUR_AUTH,
                HAS_UPDATE_AUTH
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
            public DBBit HasDataPurAuth;
            public DBBit HasUpdateAuth;
        }

        public List<SysUserPurview> SelectSysUserPurviewList(SysUserPurviewPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT DISTINCT P.SYS_ID AS SysID",
                "     , M.{SYS_NM} AS SysNM",
                "     , P.PURVIEW_ID AS PurviewID",
                "     , P.{PURVIEW_NM} AS PurviewNM",
                "     , (CASE WHEN U.CODE_TYPE IS NULL THEN 0 ELSE 1 END) AS HasDataPurAuth",
                "     , (CASE WHEN R.ROLE_ID IS NULL THEN 0 ELSE 1 END) AS HasUpdateAuth",
                "  FROM SYS_SYSTEM_PURVIEW P",
                "  JOIN SYS_SYSTEM_MAIN M",
                "    ON P.SYS_ID = M.SYS_ID",
                "  LEFT JOIN SYS_USER_SYSTEM_ROLE R",
                "    ON R.SYS_ID = P.SYS_ID",
                "   AND R.ROLE_ID = '" + EnumSystemRoleID.IT +"'",
                "   AND R.USER_ID = {UPD_USER_ID}",
                "  LEFT JOIN SYS_USER_PURVIEW U",
                "    ON U.SYS_ID = P.SYS_ID",
                "   AND U.USER_ID = {USER_ID}",
                "   AND P.PURVIEW_ID = U.PURVIEW_ID"
            }));

            dbParameters.Add(new DBParameter { Name = SysUserPurviewPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysUserPurviewPara.ParaField.PURVIEW_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysUserPurviewPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysUserPurviewPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysUserPurviewPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SysUserPurviewPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            return GetEntityList<SysUserPurview>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}