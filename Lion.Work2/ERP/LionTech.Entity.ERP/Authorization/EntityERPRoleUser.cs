using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPRoleUser : EntityAuthorization
    {
        public EntityERPRoleUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RoleUserPara
        {
            public enum ParaField
            {
                SYS_ID,
                EXEC_SYS_ID,
                ROLE_ID,
                ERP_WFNO,
                USER_ID,
                MEMO,
                API_NO,
                IP_ADDRESS,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar ExecSysID;
            public DBVarChar RoleID;
            public DBChar ApiNO;
            public DBVarChar ErpWFNO;
            public DBNVarChar Memo;
            public DBVarChar IpAddress;
            public List<DBVarChar> UserIDList;
            public List<DBVarChar> LogUserIDList;
            public DBBit IsOverride;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditRoleUserResult
        {
            Success, Failure
        }

        public EnumEditRoleUserResult EditRoleUser(RoleUserPara para)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (para.IsOverride.Bool())
            {
                commandTextStringBuilder.Append("DELETE FROM SYS_USER_SYSTEM_ROLE WHERE SYS_ID = @SYS_ID AND ROLE_ID = @ROLE_ID; ");
            }

            foreach (DBVarChar userID in para.UserIDList)
            {
                string insertCommand = string.Join(Environment.NewLine, new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID = {USER_ID} AND SYS_ID = @SYS_ID AND ROLE_ID = @ROLE_ID; ",
                    "IF EXISTS(SELECT * FROM SYS_SYSTEM_ROLE WHERE SYS_ID = @SYS_ID AND ROLE_ID = @ROLE_ID) ",
                    "BEGIN ",
                    "INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID}, @SYS_ID, @ROLE_ID, @UPD_USER_ID, GETDATE()); ",
                    "END "
                });

                dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.USER_ID, Value = userID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, insertCommand, dbParameters));
                dbParameters.Clear();
            }

            foreach (DBVarChar userID in para.LogUserIDList)
            {
                string logUserSystemRoleCommand = string.Join(Environment.NewLine, new object[]
                {
                    "EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE {USER_ID} ,@ERP_WFNO ,@MEMO ,@API_NO ,'" + Mongo_BaseAP.EnumModifyType.U + "', @EXEC_SYS_ID ,@IP_ADDRESS ,@UPD_USER_ID;"
                });

                dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.USER_ID, Value = userID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, logUserSystemRoleCommand, dbParameters));
                dbParameters.Clear();
            }

            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "DECLARE @ERP_WFNO CHAR(8) = {ERP_WFNO};",
                "DECLARE @MEMO NVARCHAR(1000) = {MEMO};",
                "DECLARE @API_NO CHAR(16) = {API_NO};",
                "DECLARE @EXEC_SYS_ID VARCHAR(12) = {EXEC_SYS_ID};",
                "DECLARE @IP_ADDRESS VARCHAR(12) = {IP_ADDRESS};",
                "DECLARE @UPD_USER_ID VARCHAR(50) = {UPD_USER_ID};",
                "DECLARE @SYS_ID VARCHAR(50) = {SYS_ID};",
                "DECLARE @ROLE_ID VARCHAR(50) = {ROLE_ID};",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",
                commandTextStringBuilder.ToString(),
                "        SET @RESULT = 'Y'; ",
                "        COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = 'N'; ",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH ",
                "; ",
                "SELECT @RESULT; ",
                Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.ERP_WFNO, Value = para.ErpWFNO });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.MEMO, Value = para.Memo });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.API_NO, Value = para.ApiNO });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.IP_ADDRESS, Value = para.IpAddress });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRoleUserResult.Success : EnumEditRoleUserResult.Failure;
        }

        #region - 查詢角色使用者清單 -
        public class RoleUser : DBTableRow
        {
            public DBVarChar UserID;
        }

        /// <summary>
        /// 查詢角色使用者清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RoleUser> SelectRoleUserList(RoleUserPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_ID AS UserID",
                    "  FROM SYS_USER_SYSTEM_ROLE",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND ROLE_ID = {ROLE_ID};"
                }));

            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RoleUserPara.ParaField.ROLE_ID, Value = para.RoleID });
            return GetEntityList<RoleUser>(commandText.ToString(), dbParameters);
        }
        #endregion

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