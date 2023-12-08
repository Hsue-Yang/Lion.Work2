using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleCondition : EntitySys
    {
        public EntitySystemRoleCondition(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢系統角色預設條件列表 -
        public class SysSystemRoleConditionPara : DBCulture
        {
            public SysSystemRoleConditionPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_CONDITION_ID,
                ROLE_CONDITION_NM,
                ROLE_ID
            }
            
            public DBVarChar SysID;
            public DBVarChar RoleConditionID;
            public DBVarChar RoleID;
        }

        public class SysSystemRoleCondition : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar RoleConditionID;
            public DBVarChar RoleConditionNM;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar SysRole;
        }

        /// <summary>
        /// 查詢系統角色預設條件列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SysSystemRoleCondition> SelectSysSystemRoleConditionList(SysSystemRoleConditionPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    " SELECT M.SYS_ID AS SysID",
                    "      , M.ROLE_CONDITION_ID AS RoleConditionID",
                    "      , M.{ROLE_CONDITION_NM} AS RoleConditionNM",
                    "      , M.ROLE_CONDITION_SYNTAX AS RoleConditionSyntax",
                    "      , M.SORT_ORDER AS SortOrder",
                    "      , M.REMARK AS Remark",
                    "      , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UpdUserNM",
                    "      , M.UPD_DT AS UpdDT",
                    "      , STUFF((",
                    "          SELECT '、' + D.ROLE_ID",
                    "          FROM SYS_SYSTEM_ROLE_CONDITION_COLLECT D",
                    "          WHERE D.SYS_ID = M.SYS_ID",
                    "          AND D.ROLE_CONDITION_ID = M.ROLE_CONDITION_ID",
                    "          FOR XML PATH('')",
                    "          ), 1, 1, '') AS SysRole",
                    " FROM SYS_SYSTEM_ROLE_CONDITION M",
                    " WHERE M.SYS_ID = {SYS_ID}"
                }));

            if (para.RoleConditionID.IsNull() == false)
            {
                commandText.AppendLine(" AND M.ROLE_CONDITION_ID = {ROLE_CONDITION_ID}");
                dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionPara.ParaField.ROLE_CONDITION_ID, Value = para.RoleConditionID });
            }

            if (para.RoleID.IsNull() == false)
            {
                commandText.AppendLine(" AND EXISTS (SELECT * FROM SYS_SYSTEM_ROLE_CONDITION_COLLECT D WHERE M.SYS_ID = D.SYS_ID  AND M.ROLE_CONDITION_ID = D.ROLE_CONDITION_ID AND D.ROLE_ID = {ROLE_ID})");
                dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionPara.ParaField.ROLE_ID, Value = para.RoleID });
            }

            commandText.AppendLine(" ORDER BY M.SORT_ORDER");

            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionPara.ParaField.ROLE_CONDITION_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleConditionPara.ParaField.ROLE_CONDITION_NM.ToString())) });
            return GetEntityList<SysSystemRoleCondition>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}