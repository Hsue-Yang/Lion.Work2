// 新增日期：2017-04-26
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityRoleUser : EntitySys
    {
        public EntityRoleUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得角色使用者權限清單 -
        public class RoleUserPara : DBCulture
        {
            public RoleUserPara(string cultureID) : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM,
                SYS_ID,
                ROLE_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
        }

        public class RoleUser : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserIDNM;
            public DBNVarChar ComIDNM;
            public DBNVarChar UserDeptIDNM;
            public DBNVarChar UnitIDNM;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserIDNM;
            public DBDateTime UpdDT;
        }

        public List<RoleUser> SelectRoleUserList(RoleUserPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.USER_ID AS UserID",
                "     , dbo.FN_GET_IDNM(U.USER_ID, U.USER_NM) AS UserIDNM",
                "     , dbo.FN_GET_IDNM(O.COM_ID, O.COM_NM) AS ComIDNM",
                "     , dbo.FN_GET_IDNM(C.CODE_ID,C.{CODE_NM}) AS UserDeptIDNM",
                "     , dbo.FN_GET_IDNM(D.UNIT_ID, D.UNIT_NM) AS UnitIDNM",
                "     , G.UPD_USER_ID AS UpdUserID",
                "     , dbo.FN_GET_USER_NM(G.UPD_USER_ID) AS UpdUserIDNM",
                "     , G.UPD_DT AS UpdDT",
                "  FROM RAW_CM_USER U",
                "  JOIN (SELECT USER_ID",
                "             , SYS_ID",
                "             , ROLE_ID",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "          FROM SYS_USER_SYSTEM_ROLE",
                "         WHERE SYS_ID = {SYS_ID}",
                "           AND ROLE_ID = {ROLE_ID}) G",
                "    ON G.USER_ID = U.USER_ID",
                "  LEFT JOIN RAW_CM_USER_ORG R",
                "    ON R.USER_ID = U.USER_ID",
                "  LEFT JOIN RAW_CM_ORG_UNIT D",
                "    ON D.UNIT_ID = U.USER_UNIT_ID",
                "  LEFT JOIN RAW_CM_ORG_COM O",
                "    ON O.COM_ID = R.USER_COM_ID",
                "  LEFT JOIN CM_CODE C",
                "    ON C.CODE_ID = R.USER_DEPT",
                "   AND C.CODE_KIND = '0018'",
                " WHERE U.IS_LEFT = '"+ EnumYN.N +"'"
            }));

            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.ROLE_ID.ToString(), Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(RoleUserPara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<RoleUser>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}