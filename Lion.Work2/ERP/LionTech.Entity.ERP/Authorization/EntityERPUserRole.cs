using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPUserRole : EntityAuthorization
    {
        public EntityERPUserRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserSystemRolePara : DBCulture
        {
            public UserSystemRolePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                USER_ID,
                SYS_ID,
                SYS_NM,
                ROLE_NM
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
        }

        public class UserSystemRole : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.SYS_ID AS SysID",
                "     , M.{SYS_NM} AS SysNM ",
                "     , U.ROLE_ID AS RoleID ",
                "     , S.{ROLE_NM} AS RoleNM",
                "     , U.UPD_USER_ID AS UpdUserID",
                "     , U.UPD_DT AS UpdDT",
                "  FROM SYS_USER_SYSTEM_ROLE U",
                "  JOIN SYS_SYSTEM_MAIN M",
                "    ON M.SYS_ID = U.SYS_ID",
                "  JOIN SYS_SYSTEM_ROLE S",
                "    ON U.SYS_ID = S.SYS_ID",
                "   AND U.ROLE_ID = S.ROLE_ID",
                " WHERE U.USER_ID = {USER_ID} "
            }));

            if (para.SysID != null)
            {
                commandText.AppendLine(" AND U.SYS_ID = {SYS_ID} ");
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            }

            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.SYS_NM)) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.ROLE_NM)) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.UserID });
            return GetEntityList<UserSystemRole>(commandText.ToString(), dbParameters);
        }

        #region - 取得系統角色名稱 -
        public class SystemRoleNMPara : DBCulture
        {
            public SystemRoleNMPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_NM
            }

            public DBVarChar SysID;
        }

        public class SystemRoleNM : DBTableRow
        {
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
        }

        public List<SystemRoleNM> SelectSystemRoleNMList(SystemRoleNMPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT ROLE_ID AS RoleID",
                "     , {ROLE_NM} AS RoleNM",
                "  FROM SYS_SYSTEM_ROLE ",
                " WHERE SYS_ID = {SYS_ID}"
            });

            dbParameters.Add(new DBParameter { Name = SystemRoleNMPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleNMPara.ParaField.ROLE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemRoleNMPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            return GetEntityList<SystemRoleNM>(commandText, dbParameters);
        }
        #endregion
    }
}