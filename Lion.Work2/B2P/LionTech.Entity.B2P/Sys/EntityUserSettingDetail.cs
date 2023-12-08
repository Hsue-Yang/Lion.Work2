using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntityUserSettingDetail : EntitySys
    {
        public EntityUserSettingDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserSettingDetailPara
        {
            public enum ParaField
            {
                SYS_ID, ROLE_ID,
                USER_ID, USER_NM, USER_PWD,
                COM_ID, UNIT_ID, UNIT_NM,
                USER_GENDER, USER_TITLE, USER_TEL1, USER_TEL2, USER_EMAIL, REMARK,
                IS_DISABLE, IS_GRANTOR, GRANTOR_USER_ID,
                UPD_USER_ID,

                EXEC_IP_ADDRESS
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;

            public DBVarChar ComID;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;

            public DBChar UserGender;
            public DBNVarChar UserTitle;
            public DBVarChar UserTel1;
            public DBVarChar UserTel2;
            public DBVarChar UserEmail;
            public DBNVarChar Remark;
            public DBChar IsDisable;
            public DBChar IsGrantor;
            public DBVarChar GrantorUserID;
            public DBVarChar UpdUserID;

            public DBVarChar ExecIPAddress;
        }

        public class UserSettingDetail : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                USER_PWD, USER_GENDER,
                USER_TITLE, USER_TEL1, USER_TEL2, USER_EMAIL,
                REMARK,
                IS_DISABLE, IS_GRANTOR,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBChar UserGender;
            public DBNVarChar UserTitle;
            public DBVarChar UserTel1;
            public DBVarChar UserTel2;
            public DBVarChar UserEmail;
            public DBNVarChar Remark;
            public DBChar IsDisable;
            public DBChar IsGrantor;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public UserSettingDetail SelectUserSettingDetail(UserSettingDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.USER_ID, D.USER_NM ", Environment.NewLine,
                "     , D.USER_PWD, D.USER_GENDER ", Environment.NewLine,
                "     , D.USER_TITLE, D.USER_TEL1, D.USER_TEL2, D.USER_EMAIL ", Environment.NewLine,
                "     , D.REMARK ", Environment.NewLine,
                "     , M.IS_DISABLE, D.IS_GRANTOR ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "JOIN SYS_USER_DETAIL D ON M.USER_ID=D.USER_ID ", Environment.NewLine,
                "WHERE UPPER(M.USER_ID)=UPPER({USER_ID}); "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                UserSettingDetail UserSettingDetail = new UserSettingDetail()
                {
                    UserID = new DBVarChar(dataRow[UserSettingDetail.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataRow[UserSettingDetail.DataField.USER_NM.ToString()]),
                    UserPWD = new DBVarChar(dataRow[UserSettingDetail.DataField.USER_PWD.ToString()]),
                    UserGender = new DBChar(dataRow[UserSettingDetail.DataField.USER_GENDER.ToString()]),
                    UserTitle = new DBNVarChar(dataRow[UserSettingDetail.DataField.USER_TITLE.ToString()]),
                    UserTel1 = new DBVarChar(dataRow[UserSettingDetail.DataField.USER_TEL1.ToString()]),
                    UserTel2 = new DBVarChar(dataRow[UserSettingDetail.DataField.USER_TEL2.ToString()]),
                    UserEmail = new DBVarChar(dataRow[UserSettingDetail.DataField.USER_EMAIL.ToString()]),
                    Remark = new DBNVarChar(dataRow[UserSettingDetail.DataField.REMARK.ToString()]),
                    IsDisable = new DBChar(dataRow[UserSettingDetail.DataField.IS_DISABLE.ToString()]),
                    IsGrantor = new DBChar(dataRow[UserSettingDetail.DataField.IS_GRANTOR.ToString()]),
                    UpdUserNM = new DBNVarChar(dataRow[UserSettingDetail.DataField.UPD_USER_NM.ToString()]),
                    UpdDT = new DBDateTime(dataRow[UserSettingDetail.DataField.UPD_DT.ToString()]),
                };
                return UserSettingDetail;
            }
            return null;
        }

        public enum EnumEditUserSettingDetailResult
        {
            Success, Failure
        }

        public EnumEditUserSettingDetailResult EditUserSettingDetail(UserSettingDetailPara para)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            string defaultCommand = string.Concat(new object[] 
            {
                "DECLARE @USER_PWD VARCHAR(40); ", Environment.NewLine,
                "DECLARE @DEFAULT_SYS_ID VARCHAR(6); ", Environment.NewLine,
                "DECLARE @DEFAULT_PATH NVARCHAR(300); ", Environment.NewLine,

                "SELECT @DEFAULT_SYS_ID=DEFAULT_SYS_ID, @DEFAULT_PATH=DEFAULT_PATH ", Environment.NewLine,
                "FROM SYS_USER_DETAIL ", Environment.NewLine,
                "WHERE USER_ID={GRANTOR_USER_ID}; ", Environment.NewLine,
            });

            if (para.UserPWD != null && !string.IsNullOrWhiteSpace(para.UserPWD.GetValue()))
            {
                defaultCommand = string.Concat(new object[] 
                {
                    defaultCommand,
                    "SET @USER_PWD={USER_PWD}; ", Environment.NewLine
                });
            }
            else
            {
                defaultCommand = string.Concat(new object[] 
                {
                    defaultCommand,
                    "SELECT @USER_PWD=USER_PWD FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });
            }

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_PWD, Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.GRANTOR_USER_ID, Value = para.GrantorUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, defaultCommand, dbParameters));
            dbParameters.Clear();

            string deleteCommand = string.Empty;
            if (para.UserID.GetValue() != para.GrantorUserID.GetValue())
            {
                deleteCommand = string.Concat(new object[] 
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_SYSTEM WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();
            }

            string mainCommand = string.Concat(new object[] 
            {
                "DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "DELETE FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "DELETE FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "DELETE FROM RAW_CM_ORG_UNIT WHERE UNIT_ID={UNIT_ID}; ", Environment.NewLine,
                "DELETE FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "INSERT INTO RAW_CM_USER VALUES ( ", Environment.NewLine,
                "    {USER_ID}, {USER_NM}, {COM_ID}, {UNIT_ID}, 'N', {UPD_USER_ID}, GETDATE(), NULL ", Environment.NewLine,
                "); ", Environment.NewLine,

                "INSERT INTO RAW_CM_ORG_UNIT VALUES ( ", Environment.NewLine,
                "    {UNIT_ID}, {UNIT_NM}, 'N', {UPD_USER_ID}, GETDATE(), NULL ", Environment.NewLine,
                "); ", Environment.NewLine,

                "INSERT INTO SYS_USER_MAIN VALUES ( ", Environment.NewLine,
                "    {USER_ID}, NULL ", Environment.NewLine,
                "  , {IS_DISABLE} ", Environment.NewLine,
                "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "); ", Environment.NewLine,

                "INSERT INTO SYS_USER_DETAIL VALUES ( ", Environment.NewLine,
                "    {USER_ID}, {USER_NM}, @USER_PWD, {USER_GENDER} ", Environment.NewLine,
                "  , {USER_TITLE}, {USER_TEL1}, {USER_TEL2}, {USER_EMAIL} ", Environment.NewLine,
                "  , {REMARK}, @DEFAULT_SYS_ID, @DEFAULT_PATH, {IS_GRANTOR}, {GRANTOR_USER_ID} ", Environment.NewLine,
                "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "); ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_PWD, Value = para.UserPWD });

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UNIT_ID, Value = para.UnitID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UNIT_NM, Value = para.UnitNM });

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_GENDER, Value = para.UserGender });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_TITLE, Value = para.UserTitle });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_TEL1, Value = para.UserTel1 });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_TEL2, Value = para.UserTel2 });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_EMAIL, Value = para.UserEmail });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.IS_GRANTOR, Value = para.IsGrantor });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.GRANTOR_USER_ID, Value = para.GrantorUserID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, mainCommand, dbParameters));
            dbParameters.Clear();

            string systemCommand = string.Empty;
            if (para.UserID.GetValue() != para.GrantorUserID.GetValue())
            {
                systemCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                    "SELECT {USER_ID}, SYS_ID, ROLE_ID, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                    "WHERE USER_ID={GRANTOR_USER_ID}; ", Environment.NewLine,

                    "INSERT INTO SYS_USER_SYSTEM ", Environment.NewLine,
                    "SELECT USER_ID, SYS_ID, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "GROUP BY USER_ID, SYS_ID; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.GRANTOR_USER_ID, Value = para.GrantorUserID });
                dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, systemCommand, dbParameters));
                dbParameters.Clear();
            }

            string functionCommand = string.Concat(new object[]
            {
                "INSERT INTO SYS_USER_FUN ", Environment.NewLine,
                "SELECT U.USER_ID ", Environment.NewLine,
                "     , F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "     , 'N' AS IS_ASSIGN ", Environment.NewLine,
                "     , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE U ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON U.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_ROLE_FUN R ON U.SYS_ID=R.SYS_ID AND U.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN F ON R.SYS_ID=F.SYS_ID AND R.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND R.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN ( ", Environment.NewLine,
                "    SELECT N.SYS_ID, N.FUN_MENU, N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME, M.DEFAULT_MENU_ID ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                ") Z ON F.SYS_ID=Z.SYS_ID AND F.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_USER_FUN O ON U.USER_ID=O.USER_ID ", Environment.NewLine,
                "                        AND F.SYS_ID=O.SYS_ID ", Environment.NewLine,
                "                        AND F.FUN_CONTROLLER_ID=O.FUN_CONTROLLER_ID ", Environment.NewLine,
                "                        AND F.FUN_ACTION_NAME=O.FUN_ACTION_NAME ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND F.IS_DISABLE='N' AND Z.FUN_MENU IS NOT NULL ", Environment.NewLine,
                "  AND O.IS_ASSIGN IS NULL ", Environment.NewLine,
                "GROUP BY U.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "ORDER BY F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, functionCommand, dbParameters));
            dbParameters.Clear();

            string menuCommand = string.Concat(new object[]
            {
                "INSERT INTO SYS_USER_FUN_MENU ", Environment.NewLine,
                "SELECT DISTINCT U.USER_ID, Z.FUN_MENU_SYS_ID ", Environment.NewLine,
                "     , Z.FUN_MENU, Z.DEFAULT_MENU_ID, Z.SORT_ORDER ", Environment.NewLine,
                "     , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "FROM SYS_USER_FUN U ", Environment.NewLine,
                "JOIN ( ", Environment.NewLine,
                "    SELECT N.SYS_ID ", Environment.NewLine,
                "         , N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                "         , N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME ", Environment.NewLine,
                "         , M.DEFAULT_MENU_ID, M.SORT_ORDER ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                ") Z ON U.SYS_ID=Z.SYS_ID AND U.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_USER_FUN_MENU M ON U.USER_ID=M.USER_ID AND Z.FUN_MENU_SYS_ID=M.SYS_ID AND Z.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND M.USER_ID IS NULL AND M.SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine,

                "DELETE SYS_USER_FUN_MENU ", Environment.NewLine,
                "FROM SYS_USER_FUN_MENU U ", Environment.NewLine,
                "LEFT JOIN (SELECT DISTINCT N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                "           FROM SYS_USER_FUN F ", Environment.NewLine,
                "           JOIN SYS_SYSTEM_MENU_FUN N ON F.SYS_ID=N.SYS_ID AND F.FUN_CONTROLLER_ID=N.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=N.FUN_ACTION_NAME ", Environment.NewLine,
                "           WHERE F.USER_ID={USER_ID}) M ", Environment.NewLine,
                "ON U.SYS_ID=M.FUN_MENU_SYS_ID AND U.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND M.FUN_MENU_SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, menuCommand, dbParameters));
            dbParameters.Clear();

            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(),
                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserSettingDetailResult.Success : EnumEditUserSettingDetailResult.Failure;
        }
    }
}